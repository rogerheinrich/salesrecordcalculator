using Microsoft.OpenApi.Models;

namespace SalesRecordCalculator.Startup;

public static class DIContainer
{
    public static void AddDependencies(IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "SalesRecordCalculator", Version = "v1" });
        });
    }
}