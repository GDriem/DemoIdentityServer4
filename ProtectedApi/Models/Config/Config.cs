using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;
using System.Collections.Generic;

public static class Config
{

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


    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new ApiScope("api1", "My API") // 🔹 Define el scope de la API
        };

    public static IEnumerable<ApiResource> ApiResources =>
        new List<ApiResource>
        {
            new ApiResource("api1")
            {
                Scopes = { "api1" },
                UserClaims = { "role" } // 🔹 Permitir que "role" sea parte del token

            }
        };

    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            new Client
            {
                ClientId = "client", 
                ClientSecrets = { new Secret("api-secret".Sha256()) },
                AllowedGrantTypes = GrantTypes.ClientCredentials, 
                AllowedScopes = { "api1" } 
            },
            new Client
            {
                ClientId = "ro-client", // 🔹 Cliente que usará credenciales de usuario
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                ClientSecrets = { new Secret("ro-secret".Sha256()) },
                    AllowedScopes = { "api1", "role" }, // 🔹 Agregar "role" al scope
                        AlwaysIncludeUserClaimsInIdToken = true

            }

        };
}
