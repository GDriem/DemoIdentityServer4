using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;
using System.Collections.Generic;

public static class Config
{
    // Definir los recursos de identidad (Claims estándar como nombre, email, etc.)
    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };

    // Definir los permisos que las APIs pueden exponer
    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new ApiScope("api1", "My API")
        };

    // Definir los recursos API
    public static IEnumerable<ApiResource> ApiResources =>
        new List<ApiResource>
        {
            new ApiResource("api1", "My API")
            {
                Scopes = { "api1" }
            }
        };

  

public static List<TestUser> TestUsers =>
    new List<TestUser>
    {
        new TestUser
        {
            SubjectId = "1",
            Username = "admin",
            Password = "admin",
            Claims = new List<System.Security.Claims.Claim>
            {
                new System.Security.Claims.Claim("name", "Admin User"),
                new System.Security.Claims.Claim("role", "Administrator")
            }
        }
    };

    public static IEnumerable<Client> Clients =>
     new List<Client>
     {
        new Client
        {
            ClientId = "mvc-client",
            ClientSecrets = { new Secret("mvc-secret".Sha256()) },
            AllowedGrantTypes = GrantTypes.Code,

            RedirectUris = { "https://localhost:7168/signin-oidc" },
            PostLogoutRedirectUris = { "https://localhost:7168/signout-callback-oidc" },

            AllowedScopes = { "openid", "profile", "api1" },
            RequirePkce = true,
            AllowOfflineAccess = true
        }
     };



}
