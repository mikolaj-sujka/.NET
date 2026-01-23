// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using System.Security.Claims;
using System.Text.Json;
using Duende.IdentityModel;
using Duende.IdentityServer;
using Duende.IdentityServer.Test;

namespace Marvin.IDP;

public static class TestUsers
{
    public static List<TestUser> Users
    {
        get
        {
            var address = new
            {
                street_address = "One Hacker Way",
                locality = "Heidelberg",
                postal_code = "69118",
                country = "Germany"
            };
                
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "123309e9-03aa-4b63-b367-59f1e03520a4",
                    Username = "Alice",
                    Password = "password",
                    Claims =
                    {
                        new Claim("role", "FreeUser"),

                        new Claim(JwtClaimTypes.GivenName, "Alice"),
                        new Claim(JwtClaimTypes.FamilyName, "Smith"),
                    }
                },
                new TestUser
                {
                    SubjectId = "41ecc841-2005-4cda-bd20-6c51b761042d",
                    Username = "Bob",
                    Password = "password",
                    Claims =
                    {
                        new Claim("role", "PayingUser"),

                        new Claim(JwtClaimTypes.GivenName, "Bob"),
                        new Claim(JwtClaimTypes.FamilyName, "Smith"),
                    }
                }
            };
        }
    }
}