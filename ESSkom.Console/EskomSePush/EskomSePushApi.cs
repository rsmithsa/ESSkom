//-----------------------------------------------------------------------
// <copyright file="EskomSePushApi.cs" company="Richard Smith">
//     Copyright (c) Richard Smith. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ESSkom.Console.EskomSePush
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using ESSkom.Console;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    public class EskomSePushApi : IEskomSePushApi, IDisposable
    {
        private readonly IOptionsMonitor<ESSkomConfig> config;
        private readonly ILogger<EskomSePushApi> logger;

        private readonly HttpClient client;
        private readonly IDisposable? configChangeHandle;
        private readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

        public EskomSePushApi(IOptionsMonitor<ESSkomConfig> config, ILogger<EskomSePushApi> logger)
        {
            this.config = config;
            this.logger = logger;

            this.configChangeHandle = this.config.OnChange(UpdateConfiguration);

            this.client = new HttpClient();
            this.client.DefaultRequestHeaders.Add("Token", config.CurrentValue.ESPToken);
        }

        public async Task GetStatus()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://developer.sepush.co.za/business/2.0/status");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            Console.WriteLine(await response.Content.ReadAsStringAsync());

        }

        public async Task GetInformation(string areaId)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://developer.sepush.co.za/business/2.0/area?id=eskde-10-fourwaysext10cityofjohannesburggauteng&test=current");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            Console.WriteLine(await response.Content.ReadAsStringAsync());

        }

        public async Task<ESPAllowanceDto> GetAllowance()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://developer.sepush.co.za/business/2.0/api_allowance");
            var response = await this.client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();

            this.logger.LogDebug(jsonString);

            var result = JsonSerializer.Deserialize<Dictionary<string, ESPAllowanceDto>>(jsonString, this.jsonOptions);

            if (result == null || !result.TryGetValue("allowance", out var r))
            {
                throw new JsonException();
            }

            return r;
        }

        public void Dispose()
        {
            this.client.Dispose();
            this.configChangeHandle?.Dispose();
        }

        private void UpdateConfiguration(ESSkomConfig options, string? name)
        {
            this.client.DefaultRequestHeaders.Remove("Token");
            this.client.DefaultRequestHeaders.Add("Token", options.ESPToken);
        }
    }
}
