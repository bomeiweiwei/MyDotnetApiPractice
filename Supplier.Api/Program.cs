using Microsoft.OpenApi.Models;
using Supplier.Api.Models;
using Supplier.Api.Models.Config.External;
using Supplier.Api.Models.Config.Sys;
using Supplier.Api.Services.Identity;
using Supplier.Api.Services.Identity.implement;
using Supplier.Api.Services.Orders;
using Supplier.Api.Services.Test;
using Supplier.Api.Services.Test.implement;

var builder = WebApplication.CreateBuilder(args);

var allowSpecificOrigins = "AllowOrigins";
var allowedOrigins = builder.Configuration.GetSection("System:WithOrigins").Get<string[]>();
builder.Services.AddCors(options =>
{
    options.AddPolicy(allowSpecificOrigins, policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Add services to the container.
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<ITestService, TestService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Supplier API", Version = "v1" });
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

builder.Services.AddHttpClient();

builder.Services.Configure<SystemSettings>(builder.Configuration.GetSection("System"));
builder.Services.Configure<ExternalSystemsOptions>(builder.Configuration.GetSection("ExternalSystems"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(allowSpecificOrigins);

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();

