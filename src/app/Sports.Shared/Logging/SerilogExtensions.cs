namespace Sports.Shared.Logging;

using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Sinks.Grafana.Loki;

public static class SerilogExtensions
{
    public static LoggerConfiguration ConfigureSerilog(
        this LoggerConfiguration loggerConfiguration, IConfiguration configuration, string appName)
    {
        var lokiUrl = configuration["Loki:Endpoint"] ?? "http://localhost:3100";

        return loggerConfiguration
            .MinimumLevel.Information()
            .WriteTo.Console()
            .WriteTo.GrafanaLoki(lokiUrl,
            [
                new LokiLabel { Key = "app", Value = appName }
            ]);
    }
}
