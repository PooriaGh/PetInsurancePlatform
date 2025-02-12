using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Reflection;

namespace PetInsurancePlatform.SharedKernel.Monitoring;

public static class MonitoringInstaller
{
    private static void AddOpenIddictOptions(this IServiceCollection services)
    {
        services
            .AddOptions<OpenTelemetryOptions>()
            .BindConfiguration(OpenTelemetryOptionsSetup.SECTION_NAME)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.TryAddSingleton(sp => sp.GetRequiredService<IOptions<OpenTelemetryOptions>>().Value);
    }

    public static void AddOpenTelemetryMonitoring(this IServiceCollection services, Assembly assembly)
    {
        services.AddOpenIddictOptions();

        var sp = services.BuildServiceProvider();
        var openTelemetryOptions = sp.GetRequiredService<OpenTelemetryOptions>();

        var name = assembly.GetName().Name ?? "unknown";
        var version = assembly.GetName().Version?.ToString() ?? "unknown";

        var resourceBuilder = ResourceBuilder
            .CreateDefault()
            .AddService(
            serviceName: name,
            serviceVersion: version,
            serviceInstanceId: Environment.MachineName);

        services
            .AddOpenTelemetry()
            .WithTracing(builder =>
            {
                builder
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .SetResourceBuilder(resourceBuilder);
            })
            .WithMetrics()
            .WithLogging(builder =>
            {
                builder.SetResourceBuilder(resourceBuilder);
                builder.AddOtlpExporter(options => options.Endpoint = openTelemetryOptions.Uri);
            });
    }
}
