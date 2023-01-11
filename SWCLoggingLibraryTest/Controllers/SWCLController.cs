using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SWCLoggingLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWCLoggingLibraryTest.Controllers
{
    public class SWCLController : Controller
    {
        private readonly ILogger<SWCLController> _logger;
        private readonly ILoggerProvider _loggerProvider;

        public SWCLController(ILogger<SWCLController> logger, ILoggerProvider loggerProvider)
        {
            _logger = logger;
            _loggerProvider = loggerProvider;
        }

        public IActionResult Index()
        {
            SWCLogging swlog = new SWCLogging((SWCLoggingProvider)_loggerProvider);
            var k = swlog.GetSearchPage();
            ViewBag.Data = k;
            return View();
        }

        [HttpPost]
        public IActionResult GetLogs(SWCSearchRequest model)
        {
            SWCLogging swlog = new SWCLogging((SWCLoggingProvider)_loggerProvider);
            //model.NoOfRcordsToFetch = 50;
            var res = swlog.SearchLogs(model);

            return Json(new { result = res });
        }
    }
}
