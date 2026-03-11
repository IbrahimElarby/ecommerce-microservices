using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Logging
{
    public static class Logging 
    {
        public static Action<HostBuilderContext,LoggerConfiguration> ConfigureLogging => (context, configuration) =>
        {
            var env = context.HostingEnvironment;
            configuration.MinimumLevel.Information()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Application", env.ApplicationName)
                .Enrich.WithProperty("Environment", env.EnvironmentName)
                .Enrich.WithExceptionDetails()
                .MinimumLevel.Override("Microsoft.ASPNetCore", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Warning)
                .WriteTo.Console();

            if(context.HostingEnvironment.IsDevelopment())
            {
                configuration.MinimumLevel.Override("catalog", LogEventLevel.Debug);
                configuration.MinimumLevel.Override("basket", LogEventLevel.Debug);
                configuration.MinimumLevel.Override("discount", LogEventLevel.Debug);
                configuration.MinimumLevel.Override("ordering", LogEventLevel.Debug);
            }

            var ElasticSearchUrl = context.Configuration.GetValue<string>("ElasticConfiguration:Uri");
            if(!string.IsNullOrEmpty(ElasticSearchUrl))
            {
                configuration.WriteTo.Elasticsearch(new Serilog.Sinks.Elasticsearch.ElasticsearchSinkOptions(new Uri(ElasticSearchUrl))
                {
                    AutoRegisterTemplate = true,
                    IndexFormat = $"{env.ApplicationName.ToLower()}-{env.EnvironmentName.ToLower()}-{DateTime.UtcNow:yyyy-MM}",
                    MinimumLogEventLevel = LogEventLevel.Debug
                });
            } 
        };
    }
}
