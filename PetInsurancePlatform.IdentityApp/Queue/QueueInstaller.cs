using MassTransit;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using PetInsurancePlatform.IdentityApp.Data;

namespace PetInsurancePlatform.IdentityApp.Queue;

internal static class QueueInstaller
{
    private static void AddQueueOption(this IServiceCollection services)
    {
        services
            .AddOptions<QueueOptions>()
            .BindConfiguration(QueueOptionsSetup.SECTION_NAME)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.TryAddSingleton(sp => sp.GetRequiredService<IOptions<QueueOptions>>().Value);
    }

    public static void AddQueue(this IServiceCollection services)
    {
        services.AddQueueOption();

        var sp = services.BuildServiceProvider();
        var options = sp.GetRequiredService<QueueOptions>();

        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.SetKebabCaseEndpointNameFormatter();

            busConfigurator.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(options.Host, "/", hostConfigurator =>
                {
                    hostConfigurator.Username(options.UserName);
                    hostConfigurator.Password(options.Password);
                });

                configurator.ConfigureEndpoints(context);
            });

            busConfigurator.AddEntityFrameworkOutbox<ApplicationDbContext>(outboxConfigurator =>
            {
                outboxConfigurator.UsePostgres();
                outboxConfigurator.UseBusOutbox();
            });

            busConfigurator.AddConfigureEndpointsCallback((context, name, configurator) =>
            {
                configurator.UseEntityFrameworkOutbox<ApplicationDbContext>(context);
            });
        });
    }
}
