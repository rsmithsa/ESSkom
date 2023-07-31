//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Richard Smith">
//     Copyright (c) Richard Smith. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ESSkom.Console
{
    using ESSkom.Console.Database;
    using ESSkom.Console.EskomSePush;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Serilog;

    internal class Program
    {
        public static void Main(string[] args)
        {
            var app = Host.CreateDefaultBuilder(args)
#if DEBUG
                .UseEnvironment("Development")
#else
                .UseEnvironment("Production")
#endif
                .UseSerilog((context, services, loggerConfiguration) =>
                    loggerConfiguration
                        .ReadFrom.Configuration(context.Configuration)
                        .Enrich.FromLogContext())
                .ConfigureServices((context, services) =>
                {
                    services.Configure<ESSkomConfig>(context.Configuration.GetSection(nameof(ESSkomConfig)));

                    services.AddSingleton<IDatabaseConnectionFactory, DatabaseConnectionFactory>();
                    services.AddSingleton<IEskomSePushApi, EskomSePushApi>();

                    services.AddSingleton<IESPStatusRepository, ESPStatusRepository>();
                    services.AddSingleton<IESPAreaInfoRepository, ESPAreaInfoRepository>();

                    services.AddHostedService<ESSkomJob>();
                })
                .Build();

            app.Run();
        }
    }
}