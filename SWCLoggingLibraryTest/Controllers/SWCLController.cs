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
            SWCLogging swclog = SWCLogging.GetInstance((SWCLoggingProvider)_loggerProvider);
            var k = swclog.GetSearchPage();
            ViewBag.Data = k;
            return View();
        }

        [HttpPost]
        public IActionResult GetLogs(SWCSearchRequest model)
        {
            SWCLogging swclog = SWCLogging.GetInstance((SWCLoggingProvider)_loggerProvider);
            var res = swclog.SearchLogs(model);

            return Json(new { result = res });
        }
    }
}
