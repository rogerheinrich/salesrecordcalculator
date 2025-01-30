using System.Reflection;
using Microsoft.OpenApi.Models;
using SalesRecordCalculator.DomainLogic;

namespace SalesRecordCalculator.Startup;

/// <summary>
/// Static class for adding services to the Web Application.
/// </summary>
public static class DIContainer
{
    /// <summary>
    /// Adds the necessary dependencies to the service collection.
    /// </summary>
    public static void AddDependencies(IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "SalesRecordCalculator", Version = "v1" });

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });

        // Adding these dependencies as transient because they do not need to hold state across a 
        // request. They are stateless and can be created and destroyed as needed.
        services.AddTransient<ISalesRecordReader, SalesRecordCsvReader>();
        services.AddTransient<IAggregateCalculator, AggregateCalculator>();
        services.AddTransient<IQuickSelectSort, QuickSelectSort>();
    }
}