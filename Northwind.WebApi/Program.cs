using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using Northwind.IoC;
using Northwind.Utilities.ConfigManager;
using Northwind.Utilities.CustExceptions;
using Northwind.Utilities.CustResp;
using Northwind.Utilities.Enum;
using Northwind.WebApi.Extensions;
using Northwind.WebApi.Filters;
using Serilog;
using System;


var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
Environment.SetEnvironmentVariable("HOME", home);

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((hostContext, services, loggerConfiguration) => {
    loggerConfiguration
        .ReadFrom.Configuration(hostContext.Configuration);
});

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ApiExceptionFilter>();
});

ConfigurationManager configuration = builder.Configuration;
ConfigManager.Initial(configuration);
builder.Services.RegisterService();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

Northwind.Services.BaseService.ServiceProvider = app.Services;

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler(errorApp =>
    {
        errorApp.Run(async context =>
        {
            var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();

            var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
            var exception = exceptionHandlerPathFeature?.Error;

            var errorResponse = new ErrorResponse
            {
                StatusCode = ReturnCode.ExceptionError,
                Message = "An unexpected error occurred."
            };
            if (exception is HttpStatusException httpEx)
            {
                context.Response.StatusCode = httpEx.HttpStatusCode;
                errorResponse.StatusCode = httpEx.AppStatusCode;
                errorResponse.Message = httpEx.Message;
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                logger.LogError(exception, "Unhandled exception occurred at path {Path}", context.Request.Path);
            }

            context.Response.ContentType = "application/json";

            context.Items["ExceptionResponse"] = JsonConvert.SerializeObject(errorResponse);

            await context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
        });
    });
}


app.UseHttpsRedirection();

app.UseRouting();

app.UseApiLogging();

app.UseAuthorization();

app.MapControllers();

app.Run();