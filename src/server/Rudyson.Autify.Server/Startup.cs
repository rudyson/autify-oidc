using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Rudyson.Autify.Application.Contracts;
using Rudyson.Autify.Infrastructure;
using Rudyson.Autify.Infrastructure.Options;
using Rudyson.Autify.Server.Helpers;
using Scalar.AspNetCore;

namespace Rudyson.Autify.Server;

public class Startup(IConfiguration configuration, IHostEnvironment hostEnvironment)
{
    const string DocumentName = "v1";
    const string ApiExplorerPrefix = "api/docs";
    const string ApiExplorerTitle = "Autify API";
    public void ConfigureServices(IServiceCollection services)
    {

        services
            .AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.WriteIndented = true;
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
        services.AddOpenApi(DocumentName);

        services.AddRouting(options =>
        {
            options.LowercaseQueryStrings = true;
            options.LowercaseUrls = true;
        });

        services.AddHttpContextAccessor();

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(
                builder =>
                {
                    builder
                        .WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
        });

        var jwtSection = configuration.GetSection(JwtOptions.SectionName);
        var jwtOptions = jwtSection.Get<JwtOptions>();
        ArgumentNullException.ThrowIfNull(jwtOptions);

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions!.Secret)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

        services.AddScoped<ITenantProvider, TenantProvider>();
        services.AddInfrastructure(configuration);
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseRouting();
        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            if (hostEnvironment.IsDevelopment())
            {
                endpoints.MapOpenApi();
                endpoints.MapScalarApiReference(ApiExplorerPrefix, options =>
                {
                    options.WithTitle(ApiExplorerTitle);
                });
            }
            endpoints.MapControllers();
        });
    }
}
