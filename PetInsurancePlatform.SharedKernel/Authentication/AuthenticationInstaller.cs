using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using OpenIddict.Validation.AspNetCore;

namespace PetInsurancePlatform.SharedKernel.Authentication;

public static class AuthenticationInstaller
{
    private static void AddOpenIddictOptions(this IServiceCollection services)
    {
        services
            .AddOptions<OpenIddictOptions>()
            .BindConfiguration(OpenIddictOptionsSetup.SECTION_NAME)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.TryAddSingleton(sp => sp.GetRequiredService<IOptions<OpenIddictOptions>>().Value);
    }

    public static void AddOpenIddictAuthentication(this IServiceCollection services)
    {
        services.AddOpenIddictOptions();

        var sp = services.BuildServiceProvider();
        var openIddictOptions = sp.GetRequiredService<OpenIddictOptions>();

        services
            .AddOpenIddict()
            .AddValidation(options =>
            {
                options.SetIssuer(openIddictOptions.Issuer);

                options.AddAudiences(openIddictOptions.ClientId);

                options
                .UseIntrospection()
                .SetClientId(openIddictOptions.ClientId)
                .SetClientSecret(openIddictOptions.ClientSecret);

                options.UseSystemNetHttp();

                options.UseAspNetCore();
            });

        services.AddAuthentication(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);

        services.AddAuthorization();
    }
}
