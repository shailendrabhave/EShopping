// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace Identity
{
    public static class Config
    {
        private const string GatewayScope = "eshoppinggateway";
        private const string BasketApiScope = "basketapi";
        private const string CatalogApiScope = "catalogapi";
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope(GatewayScope),

                new ApiScope(CatalogApiScope), 

                new ApiScope(BasketApiScope),
            };

        public static IEnumerable<ApiResource> ApiResources => 
            new ApiResource[]
        {
            // List of microservices
            new ApiResource("Basket", "Basket.API")
            {
                Scopes = { BasketApiScope }
            },
            new ApiResource("Catalog", "Catalog.API")
            {
                Scopes = { CatalogApiScope }
            },
            new ApiResource("EShoppingGateway", "EShopping.Gateway")
            {
                Scopes = { GatewayScope }
            }
        };


        public static IEnumerable<Client> Clients =>
            new Client[]
            {
               // m2m flow
               new Client
               {
                   ClientName = "EShopping Gateway Client",
                   ClientId = "EShoppingGatewayClient",
                   ClientSecrets = {new Secret("6731de76-14a6-49ae-97bc-6eba6914391e".Sha256())},
                   AllowedGrantTypes = GrantTypes.ClientCredentials,
                   AllowedScopes = { GatewayScope }
               },
               new Client
               {
                   ClientName = "Catalog API Client",
                   ClientId = "CatalogAPIClient",
                   ClientSecrets = {new Secret("6731de76-14a6-49ae-97bc-6eba6917948d".Sha256())},
                   AllowedGrantTypes = GrantTypes.ClientCredentials,
                   AllowedScopes = { CatalogApiScope }
               },
               new Client
               {
                   ClientName = "Basket API Client",
                   ClientId = "BasketAPIClient",
                   ClientSecrets = {new Secret("6731de76-14a6-49ae-97bc-6eba6914234b".Sha256())},
                   AllowedGrantTypes = GrantTypes.ClientCredentials,
                   AllowedScopes = { BasketApiScope }
               }
            };
    }
}