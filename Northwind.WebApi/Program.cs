using Microsoft.AspNetCore.Diagnostics;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Northwind.IoC;
using Northwind.Models.CustExceptions;
using Northwind.Utilities.ConfigManager;
using Northwind.Utilities.Enum;
using Northwind.WebApi.Extensions;
using Northwind.WebApi.Filters;
using Northwind.WebApi.Models.CustResp;
using Serilog;
using System;
using System.Reflection;

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

var withOrigins = ConfigManager.SystemSection.WithOrigins.ToArray();
var allowSpecificOrigins = "AllowOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowSpecificOrigins,
                      builder =>
                      {
                          builder.WithOrigins(withOrigins)
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                      });
});


builder.Services.RegisterService();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Northwind API", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Plz input JWT Token，format like：Bearer {your token}"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

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

            var errorResponse = new CustomErrorResponse("An unexpected error occurred.", (int)ReturnCode.ExceptionError);
            if (exception is HttpStatusException httpEx)
            {
                context.Response.StatusCode = httpEx.HttpStatusCode;
                errorResponse.StatusCode = (int)httpEx.AppStatusCode;
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

app.UseCors(allowSpecificOrigins);

app.UseRouting();

app.UseApiLogging();

app.UseAuthorization();

app.MapControllers();

app.Run();