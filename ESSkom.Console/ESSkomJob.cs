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
    using ESSkom.Console.EskomSePush;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public class ESSkomJob : BackgroundService
    {
        private readonly IEskomSePushApi eskomSePushApi;
        private readonly IHostApplicationLifetime applicationLifetime;
        private readonly ILogger<ESSkomJob> logger;

        public ESSkomJob(IEskomSePushApi eskomSePushApi, IHostApplicationLifetime applicationLifetime, ILogger<ESSkomJob> logger)
        {
            this.eskomSePushApi = eskomSePushApi;
            this.applicationLifetime = applicationLifetime;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            this.logger.LogInformation("Running");

            var allowance = await this.eskomSePushApi.GetAllowance();

            await Task.Delay(10000);

            this.applicationLifetime.StopApplication();
        }
    }
}
