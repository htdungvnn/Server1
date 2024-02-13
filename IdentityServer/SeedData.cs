﻿using System.Security.Claims;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using IdentityModel;
using IdentityServer.Data;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace IdentityServer;

public class SeedData
{
    public static void EnsureSeedData(WebApplication app)
    {
        using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();

            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var alice = userMgr.FindByNameAsync("alice").Result;
            if (alice == null)
            {
                alice = new ApplicationUser
                {
                    UserName = "alice",
                    Email = "AliceSmith@email.com",
                    EmailConfirmed = true
                };
                var result = userMgr.CreateAsync(alice, "Pass123$").Result;
                if (!result.Succeeded) throw new Exception(result.Errors.First().Description);

                result = userMgr.AddClaimsAsync(alice, new[]
                {
                    new(JwtClaimTypes.Name, "Alice Smith"),
                    new Claim(JwtClaimTypes.GivenName, "Alice"),
                    new Claim(JwtClaimTypes.FamilyName, "Smith"),
                    new Claim(JwtClaimTypes.WebSite, "http://alice.com")
                }).Result;
                if (!result.Succeeded) throw new Exception(result.Errors.First().Description);

                Log.Debug("alice created");
            }
            else
            {
                Log.Debug("alice already exists");
            }

            var bob = userMgr.FindByNameAsync("bob").Result;
            if (bob == null)
            {
                bob = new ApplicationUser
                {
                    UserName = "bob",
                    Email = "BobSmith@email.com",
                    EmailConfirmed = true
                };
                var result = userMgr.CreateAsync(bob, "Pass123$").Result;
                if (!result.Succeeded) throw new Exception(result.Errors.First().Description);

                result = userMgr.AddClaimsAsync(bob, new[]
                {
                    new(JwtClaimTypes.Name, "Bob Smith"),
                    new Claim(JwtClaimTypes.GivenName, "Bob"),
                    new Claim(JwtClaimTypes.FamilyName, "Smith"),
                    new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                    new Claim("location", "somewhere")
                }).Result;
                if (!result.Succeeded) throw new Exception(result.Errors.First().Description);

                Log.Debug("bob created");
            }
            else
            {
                Log.Debug("bob already exists");
            }
        }
    }

    public static void ForceSeedDatabase(IApplicationBuilder app)
    {
        // using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
        // {
        //     var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
        //     context.Database.Migrate();
        //         foreach (var client in Config.Clients) context.Clients.Add(client.ToEntity());
        //         //foreach (var resource in Config.IdentityResources) context.IdentityResources.Add(resource.ToEntity());
        //         foreach (var scope in Config.ApiScopes) context.ApiScopes.Add(scope.ToEntity());
        //         foreach (var resource in Config.GetApiResources()) context.ApiResources.Add(resource.ToEntity());
        //         context.SaveChanges();
        // }
    }
    
    public static void InitializeDatabase(IApplicationBuilder app)
    {
        using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
        {
            serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

            var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
            context.Database.Migrate();
            if (!context.Clients.Any())
            {
                foreach (var client in Config.Clients) context.Clients.Add(client.ToEntity());
                context.SaveChanges();
            }

            if (!context.IdentityResources.Any())
            {
                foreach (var resource in Config.IdentityResources) context.IdentityResources.Add(resource.ToEntity());
                context.SaveChanges();
            }

            if (!context.ApiScopes.Any())
            {
                foreach (var scope in Config.ApiScopes) context.ApiScopes.Add(scope.ToEntity());
                context.SaveChanges();
            }
            
            if (!context.ApiResources.Any())
            {
                foreach (var resource in Config.GetApiResources()) context.ApiResources.Add(resource.ToEntity());
                context.SaveChanges();
            }
        }
    }
}