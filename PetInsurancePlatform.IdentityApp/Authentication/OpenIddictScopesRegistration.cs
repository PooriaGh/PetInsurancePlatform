using NuGet.Packaging;
using OpenIddict.Abstractions;

namespace PetInsurancePlatform.IdentityApp.Authentication;

internal static class OpenIddictScopesRegistration
{
    private static readonly IEnumerable<OpenIddictScopeDescriptor> _descriptors =
    [
        Describe(
            "api",
            "api_resource_server")
    ];

    public static async Task RegisterAsync(this IOpenIddictScopeManager manager)
    {
        foreach (var descriptor in _descriptors)
        {
            if (!string.IsNullOrWhiteSpace(descriptor.Name)
               && await manager.FindByNameAsync(descriptor.Name) is null)
            {
                await manager.CreateAsync(descriptor);
            }
        }
    }

    private static OpenIddictScopeDescriptor Describe(
        string name,
        string? displayName = null,
        params string[] resources)
    {
        var descriptor = new OpenIddictScopeDescriptor
        {
            Name = name,
            DisplayName = displayName ?? name,
        };

        resources ??= [];
        descriptor.Resources.AddRange(resources);

        return descriptor;
    }
}
