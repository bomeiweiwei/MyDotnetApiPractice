using Microsoft.AspNetCore.Mvc;
using Supplier.Api.Filters;
using Supplier.Api.Models;

namespace Supplier.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[JwtAuthActionFilter]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    [Route("GetWeathers")]
    public IEnumerable<WeatherForecast> GetWeathers(GetWeathersReq req)
    {
        return Enumerable.Range(req.Start, req.End).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}

