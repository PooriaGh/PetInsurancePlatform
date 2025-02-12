namespace PetInsurancePlatform.IdentityApp.Cors;

internal static class CorsPolicyInstaller
{
    public static void AddDefaultCorsPolicy(this IServiceCollection services)
    {
        string[] localUrls =
        [
            "https://localhost:5001",
            "http://localhost:3000"
        ];

        string[] stageUrls =
        [
        ];

        string[] productionUrls =
        [
        ];

        string[] remoteUrls = [.. localUrls, .. stageUrls, .. productionUrls];

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder
                .WithOrigins(remoteUrls)
                .AllowAnyMethod()
                .AllowAnyHeader();
            });
        });
    }
}
