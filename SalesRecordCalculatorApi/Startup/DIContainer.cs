using Microsoft.OpenApi.Models;
using SalesRecordCalculator.DomainLogic;

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

        // Adding these dependencies as transient because they do not need to hold state across a 
        // request. They are stateless and can be created and destroyed as needed.
        services.AddTransient<ISalesRecordReader, SalesRecordCsvReader>();
        services.AddTransient<IAggregateCalculator, AggregateCalculator>();
        services.AddTransient<IQuickSelectSort, QuickSelectSort>();
    }
}