using GoldenTree.DataModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

namespace GoldenTree.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlertsController : ControllerBase
    {
        private static readonly ConcurrentDictionary<(string Username, string Ticker), Alert> _alerts = new();

        private readonly ILogger<AlertsController> _logger;

        public AlertsController(ILogger<AlertsController> logger)
        {
            _logger = logger;
        }

        // Register an alert for a given security at a specified price
        [HttpPost("SetLowPriceAlert")]
        public IActionResult SetLowPriceAlert(string username, string ticker, decimal priceLimit)
        {
            Alert alert = new Alert
            {
                Username = username,
                Ticker = ticker,
                PriceLimit = priceLimit
            };

            _alerts[(username, ticker)] = alert;
            return Ok("Alert registered successfully.");
        }

        // Return a list of all registered alerts
        [HttpGet("GetAlerts")]
        public IActionResult GetAlerts()
        {
            List<Alert> alertList = _alerts.Values.ToList();
            return Ok(alertList);
        }

        // Return a list of alerts that would be triggered if the given security reaches the specified price
        [HttpGet("CheckAlerts")]
        public IActionResult CheckAlerts(string ticker, decimal price)
        {
            var triggeredAlerts = _alerts.Values
                .Where(alert => alert.Ticker == ticker && alert.PriceLimit >= price)
                .ToList();
            return Ok(triggeredAlerts);
        }

        // Remove the given alert
        [HttpDelete("RemoveAlert")]
        public IActionResult RemoveAlert(string username, string ticker)
        {
            bool success = _alerts.TryRemove((username, ticker), out _);
            if (!success)
            {
                return NotFound("Alert not found.");
            }
            return Ok("Alert removed successfully.");
        }
    }
}
