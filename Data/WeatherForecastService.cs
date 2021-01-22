using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web;
namespace TestBlazorServerB2C.Data
{
    public class WeatherForecastService
    {
        private readonly ITokenAcquisition _tokenAcquisition;
        private readonly IConfiguration _config;
        private readonly MicrosoftIdentityConsentAndConditionalAccessHandler _handler;
        public WeatherForecastService(
                    ITokenAcquisition token, 
                    IConfiguration config, 
                    MicrosoftIdentityConsentAndConditionalAccessHandler handler)
        {
            _tokenAcquisition = token;
            _config = config;
            _handler = handler;
        }
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public Task<WeatherForecast[]> GetForecastAsync(DateTime startDate)
        {
            try
            {
                var scope = _config.GetValue<string>("DownstreamApi:Scopes")?.Split(' ');
                var token = _tokenAcquisition.GetAccessTokenForUserAsync(scope);
            }
            catch (Exception ex)
            {
                 _handler.HandleException(ex);
            }
            var rng = new Random();
            return Task.FromResult(Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = startDate.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            }).ToArray());
        }
    }
}
