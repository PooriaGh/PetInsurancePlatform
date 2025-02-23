using NuGet.Packaging;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace PetInsurancePlatform.IdentityApp.Authentication;

internal static class OpenIddictApplicationsRegistration
{
    private static readonly IEnumerable<OpenIddictApplicationDescriptor> _descriptors =
    [
        Describe(
            "api_resource_server",
            permissions:
            [
                Permissions.Endpoints.Introspection
            ]),

        Describe(
            "postman_client",
            displayName:"Postman Client",
            redirectUris:
            [
                new Uri("https://oauth.pstmn.io/v1/callback")
            ],
            permissions:
            [
                Permissions.Endpoints.Introspection,
                Permissions.Endpoints.Authorization,
                Permissions.Endpoints.Revocation,
                Permissions.Endpoints.Token,
                Permissions.GrantTypes.AuthorizationCode,
                Permissions.GrantTypes.RefreshToken,
                Permissions.ResponseTypes.Code,
                Permissions.Scopes.Email,
                Permissions.Scopes.Profile,
                Permissions.Scopes.Roles,
                Permissions.Scopes.Phone,
                Permissions.Prefixes.Scope + "api"]),

        Describe(
            "swagger_client",
            displayName:"Swagger Client",
            postLogoutRedirectUris:[
                new Uri("http://localhost:3000"),
            ],
            redirectUris:
            [
                new Uri("http://localhost:3000"),
            ],
            permissions:
            [
                Permissions.Endpoints.Introspection,
                Permissions.Endpoints.Authorization,
                Permissions.Endpoints.Revocation,
                Permissions.Endpoints.Token,
                Permissions.GrantTypes.AuthorizationCode,
                Permissions.GrantTypes.RefreshToken,
                Permissions.ResponseTypes.Code,
                Permissions.Scopes.Email,
                Permissions.Scopes.Profile,
                Permissions.Scopes.Roles,
                Permissions.Scopes.Phone,
                Permissions.Prefixes.Scope + "api"]),

        Describe(
            "web_client",
            displayName:"Web Client",
            postLogoutRedirectUris:[
                new Uri("http://localhost:3000"),
            ],
            redirectUris:
            [
                new Uri("http://localhost:3000/api/auth/callback/identityserver"),
            ],
            permissions:
            [
                Permissions.Endpoints.Introspection,
                Permissions.Endpoints.Authorization,
                Permissions.Endpoints.Revocation,
                Permissions.Endpoints.Token,
                Permissions.GrantTypes.AuthorizationCode,
                Permissions.GrantTypes.RefreshToken,
                Permissions.ResponseTypes.Code,
                Permissions.Scopes.Email,
                Permissions.Scopes.Profile,
                Permissions.Scopes.Roles,
                Permissions.Scopes.Phone,
                Permissions.Prefixes.Scope + "api"],
            requirements :
            [
                Requirements.Features.ProofKeyForCodeExchange
            ]),

        Describe(
            "mobile_client",
            displayName:"Mobile Client",
            postLogoutRedirectUris:[
                new Uri("http://localhost:3000/callback/logout/local")
            ],
            redirectUris:
            [
                new Uri("http://localhost:3000/callback/login/local")
            ],
            permissions:
            [
                Permissions.Endpoints.Introspection,
                Permissions.Endpoints.Authorization,
                Permissions.Endpoints.Revocation,
                Permissions.Endpoints.Token,
                Permissions.GrantTypes.AuthorizationCode,
                Permissions.GrantTypes.RefreshToken,
                Permissions.ResponseTypes.Code,
                Permissions.Scopes.Email,
                Permissions.Scopes.Profile,
                Permissions.Scopes.Roles,
                Permissions.Scopes.Phone,
                Permissions.Prefixes.Scope + "api"],
            requirements :
            [
                Requirements.Features.ProofKeyForCodeExchange
            ]),

        Describe(
            "dashboard_client",
            displayName:"Dashboard Client",
            postLogoutRedirectUris:[
                new Uri("http://localhost:3000"),
            ],
            redirectUris:
            [
                new Uri("http://localhost:3000/api/auth/callback/identityserver"),
            ],
            permissions:
            [
                Permissions.Endpoints.Introspection,
                Permissions.Endpoints.Authorization,
                Permissions.Endpoints.Revocation,
                Permissions.Endpoints.Token,
                Permissions.GrantTypes.AuthorizationCode,
                Permissions.GrantTypes.RefreshToken,
                Permissions.ResponseTypes.Code,
                Permissions.Scopes.Email,
                Permissions.Scopes.Profile,
                Permissions.Scopes.Roles,
                Permissions.Scopes.Phone,
                Permissions.Prefixes.Scope + "api"],
            requirements :
            [
                Requirements.Features.ProofKeyForCodeExchange
            ]),
    ];

    public static async Task RegisterAsync(this IOpenIddictApplicationManager manager)
    {
        foreach (var descriptor in _descriptors)
        {
            if (!string.IsNullOrWhiteSpace(descriptor.ClientId)
                && await manager.FindByClientIdAsync(descriptor.ClientId) is null)
            {
                await manager.CreateAsync(descriptor);
            }
        }
    }

    private static OpenIddictApplicationDescriptor Describe(
        string clienId,
        string? clienSecret = null,
        string? displayName = null,
        string consentTypes = ConsentTypes.Implicit,
        IEnumerable<Uri>? postLogoutRedirectUris = null,
        IEnumerable<Uri>? redirectUris = null,
        IEnumerable<string>? permissions = null,
        IEnumerable<string>? requirements = null)
    {
        var descriptor = new OpenIddictApplicationDescriptor
        {
            ClientId = clienId,
            ClientSecret = clienSecret ?? Guid.NewGuid().ToString(),
            DisplayName = displayName ?? string.Empty,
            ConsentType = consentTypes,
        };

        postLogoutRedirectUris ??= [];
        descriptor.PostLogoutRedirectUris.AddRange(postLogoutRedirectUris);

        redirectUris ??= [];
        descriptor.RedirectUris.AddRange(redirectUris);

        permissions ??= [];
        descriptor.Permissions.AddRange(permissions);

        requirements ??= [];
        descriptor.Requirements.AddRange(requirements);

        return descriptor;
    }
}
