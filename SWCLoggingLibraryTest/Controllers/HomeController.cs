using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SWCLoggingLibraryTest.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SWCLoggingLibraryTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            using (_logger.BeginScope("THIS IS A SCOPE"))
            {
                _logger.LogCritical("Customer {CustomerId} order {OrderId} is completed.");
                _logger.LogWarning("Just a warning");
            }
            _logger.LogInformation("scope Home screen Starting up:LogEntry represents the information of");
            _logger.LogDebug("Home screen Starting up:LogEntry represents the information of a log entry. The Logger creates an instance of this class when its Log() method is called, fills the properties and then passes that info to the provider calling WriteLog()LogEntry represents the information of a log entry. The Logger creates an instance of this class when its Log() method is called, fills the properties and then passes that info to the provider calling WriteLog()LogEntry represents the information of a log entry. The Logger creates an instance of this class when its Log() method is called, fills the properties and then passes that info to the provider calling WriteLog()");
            _logger.LogError("This is error log of home screen.");
            _logger.LogTrace("This is trace log");


            using (_logger.BeginScope("C4568"))
            {
                _logger.LogCritical("C4568-- Critical Log");
                _logger.LogWarning("C4568-- Warning Log");
            }

            try
            {
                int k = 9, u = 0;
                var p = k / u;
            }
            catch (Exception ex)
            {
                _logger.LogCritical( ex, "Error occurred while dividing two numbers");
            }


            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
