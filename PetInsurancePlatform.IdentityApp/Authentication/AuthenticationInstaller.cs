using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace PetInsurancePlatform.IdentityApp.Authentication;

internal static class AuthenticationInstaller
{
    private static void AddGoogleOptions(this IServiceCollection services)
    {
        services
            .AddOptions<GoogleOptions>()
            .BindConfiguration(GoogleOptionsSetup.SECTION_NAME)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.TryAddSingleton(sp => sp.GetRequiredService<IOptions<GoogleOptions>>().Value);
    }

    public static void AddGoogleAuthentication(this IServiceCollection services)
    {
        services.AddGoogleOptions();

        var sp = services.BuildServiceProvider();
        var googleOptions = sp.GetRequiredService<GoogleOptions>();

        services
            .AddAuthentication()
            .AddGoogle(options =>
            {
                options.ClientId = googleOptions.ClientId;
                options.ClientSecret = googleOptions.ClientSecret;

                options.Scope.Add("email");
                options.Scope.Add("profile");
            });
    }
}
