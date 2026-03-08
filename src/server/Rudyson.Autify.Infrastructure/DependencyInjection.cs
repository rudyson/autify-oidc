using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rudyson.Autify.Application.Contracts;
using Rudyson.Autify.Domain.Repositories;
using Rudyson.Autify.Domain.Repositories.Inherit;
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
        var connectionString = configuration.GetConnectionString(ApplicationConnectionStrings.MasterDatabase);

        services.AddDbContext<AutifyContext>(options =>
            options
                .UseNpgsql(connectionString)
                .UseSnakeCaseNamingConvention());

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AutifyContext>());

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ISessionRepository, SessionRepository>();

        services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();

        return services;
    }
}
