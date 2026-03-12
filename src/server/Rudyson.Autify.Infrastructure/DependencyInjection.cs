using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;
using Rudyson.Autify.Application.Contracts;
using Rudyson.Autify.Infrastructure.Options;
using Rudyson.Autify.Infrastructure.Persistence;
using Rudyson.Autify.Infrastructure.Services;

namespace Rudyson.Autify.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // TODO: Remove later primary context
        //var connectionStringMasterDatabase = configuration.GetConnectionString(ApplicationConnectionStrings.MasterDatabase);
        //ArgumentNullException.ThrowIfNullOrWhiteSpace(connectionStringMasterDatabase);

        var connectionStringIdentityDatabase = configuration.GetConnectionString(ApplicationConnectionStrings.IdentityDatabase);
        ArgumentNullException.ThrowIfNullOrWhiteSpace(connectionStringIdentityDatabase);

        //services.AddDbContext<AutifyContext>(options =>
        //    options
        //        .UseNpgsql(connectionStringMasterDatabase)
        //        .UseSnakeCaseNamingConvention());

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options
               .UseNpgsql(connectionStringIdentityDatabase)
               .UseSnakeCaseNamingConvention();
            options.UseOpenIddict();
        });

        services.AddIdentity<IdentityUser, IdentityRole>()
           .AddEntityFrameworkStores<ApplicationDbContext>()
           .AddDefaultTokenProviders();

        // Cionfigure OpenIddict
        services.AddOpenIddict()
             .AddCore(coreOptions =>
             {
                 coreOptions.UseEntityFrameworkCore()
                   .UseDbContext<ApplicationDbContext>();
             })
             .AddServer(options =>
             {
                 options.SetTokenEndpointUris("/connect/token");
                 options.SetAuthorizationEndpointUris("/connect/authorize");
                 options.SetUserInfoEndpointUris("/connect/userinfo");

                 options.AllowAuthorizationCodeFlow();
                 options.AllowClientCredentialsFlow().AllowRefreshTokenFlow();
                 options.AllowPasswordFlow().AllowRefreshTokenFlow();

                 // Encryption and signing of tokens
                 options
                    .AddDevelopmentEncryptionCertificate()
                    .AddDevelopmentSigningCertificate()
                    .DisableAccessTokenEncryption();

                 // Register the ASP.NET Core host and configure the ASP.NET Core options.
                 options.UseAspNetCore()
                    .EnableTokenEndpointPassthrough()
                    .EnableAuthorizationEndpointPassthrough()
                    // TODO: Find alternative or remove
                    //.EnableLogoutEndpointPassthrough()
                    .EnableUserInfoEndpointPassthrough()
                    .DisableTransportSecurityRequirement();

             });

        //services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AutifyContext>());

        //services.AddScoped<IUserRepository, UserRepository>();
        //services.AddScoped<ISessionRepository, SessionRepository>();

        services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();

        return services;
    }
}
