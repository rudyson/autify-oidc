using System.Text.Json;
using System.Text.Json.Serialization;
using Rudyson.Autify.Application.Contracts;
using Rudyson.Autify.Infrastructure.Services;
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

        services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
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
