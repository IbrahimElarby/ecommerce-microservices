using Duende.IdentityServer.Models;

namespace eShop.Identity;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
        {
           new ApiResource("Catalog","Catalog.API")
           {
                Scopes = { "catalogapi.read" , "catalogapi.write" }
           },
           new ApiResource("Basket","Basket.API")
           {
                Scopes = { "basketapi" }
           },
           new ApiResource("EShoppingGateway","Eshopping Gateway")
           {
                Scopes = { "eshoppinggateway", "basketapi" }
           }

         };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
           new ApiScope("catalogapi"),
           new ApiScope("catalogapi.read"),
           new ApiScope("catalogapi.write"),
           new ApiScope("basketapi"),
           new ApiScope("eshoppinggateway")

        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
             new Client
            {
                ClientId = "catalogclient",
                ClientName = "Catalog Client",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },
                AllowedScopes = { "catalogapi.read" , "catalogapi.write"}
            },
             new Client
             {
                   ClientId = "basketclient",
                ClientName = "Basket Client",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("495536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },
                AllowedScopes = { "catalogapi" , "basketapi"}
             },
             new Client
             {
                   ClientId = "eshoppinggatewayclient",
                ClientName = "Eshopping Gateway Client",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("325536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },
                AllowedScopes = { "catalogapi" , "basketapi" , "eshoppinggateway" }
             }

        };
}
