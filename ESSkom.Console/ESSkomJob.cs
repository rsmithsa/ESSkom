//-----------------------------------------------------------------------
// <copyright file="ESSkomJob.cs" company="Richard Smith">
//     Copyright (c) Richard Smith. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ESSkom.Console
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ESSkom.Console.Database;
    using ESSkom.Console.EskomSePush;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class ESSkomJob : BackgroundService
    {
        private readonly IESPStatusRepository espStatusRepository;
        private readonly IESPAreaInfoRepository espAreaInfoRepository;
        private readonly IEskomSePushApi eskomSePushApi;

        private readonly IOptionsMonitor<ESSkomConfig> config;
        private readonly IHostApplicationLifetime applicationLifetime;
        private readonly ILogger<ESSkomJob> logger;

        public ESSkomJob(IESPStatusRepository espStatusRepository, IESPAreaInfoRepository espAreaInfoRepository, IEskomSePushApi eskomSePushApi, IOptionsMonitor<ESSkomConfig> config, IHostApplicationLifetime applicationLifetime, ILogger<ESSkomJob> logger)
        {
            this.espStatusRepository = espStatusRepository;
            this.espAreaInfoRepository = espAreaInfoRepository;
            this.eskomSePushApi = eskomSePushApi;

            this.config = config;
            this.applicationLifetime = applicationLifetime;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            this.logger.LogInformation($"Running - {Directory.GetCurrentDirectory()}");

            //var status = await this.eskomSePushApi.GetStatus();

            //var allowance = await this.eskomSePushApi.GetAllowance();

            //await Task.Delay(10000);

            //this.applicationLifetime.StopApplication();

            await Task.WhenAll(this.PollStatus(stoppingToken), this.PollAreaInformation(stoppingToken));
        }

        private async Task PollStatus(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    var latest = await this.espStatusRepository.GetLatestTimestamp();
                    this.logger.LogInformation($"Latest status information - {latest:O}");
                    if (DateTime.Now - latest > this.config.CurrentValue.StatusPollInterval)
                    {
                        this.logger.LogInformation("Fetching status from ESP");

                        var status = await this.eskomSePushApi.GetStatus();
                        await this.espStatusRepository.Insert(status.Values.Select(ESPStatus.FromDto).ToList());

                        this.logger.LogInformation("Fetched status from ESP");
                    }
                    else
                    {
                        this.logger.LogInformation("Skipping status fetch from ESP");
                    }
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, "Error polling status from ESP");
                }

                await Task.Delay(5000, token);
            }
        }

        private async Task PollAreaInformation(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    var latest = await this.espAreaInfoRepository.GetLatestTimestamp();
                    this.logger.LogInformation($"Latest area information - {latest:O}");
                    if (DateTime.Now - latest > this.config.CurrentValue.StatusPollInterval)
                    {
                        this.logger.LogInformation("Fetching area information from ESP");

                        var area = await this.eskomSePushApi.GetAreaInformation(this.config.CurrentValue.ESPAreaId);
                        await this.espAreaInfoRepository.Insert(ESPAreaInfo.FromDto(area));

                        this.logger.LogInformation("Fetched area information from ESP");
                    }
                    else
                    {
                        this.logger.LogInformation("Skipping area information fetch from ESP");

                        var t = await this.espAreaInfoRepository.GetAll();
                    }
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, "Error polling area information from ESP");
                }

                await Task.Delay(5000, token);
            }
        }
    }
}
