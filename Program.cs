using Microsoft.AspNetCore.Authorization;
using SalesRecordCalculator.Startup;

var builder = WebApplication.CreateBuilder(args);

DIContainer.AddDependencies(builder.Services, builder.Configuration);

var app = builder.Build();

//Configure Global Exception Handler
//ExceptionHandler.Configure( app, app.Logger );

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.Run();