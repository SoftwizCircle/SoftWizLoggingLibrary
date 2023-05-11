# Why?
The idea of building this library coined when I was going through features of .Net core and we got to know about ILogger interface. I got to know about the mechanism that there is a way to write my own logging provider and !voila.

I am always fascinated about the speed of the _Elasticsearch_ which is used for log analytics, full-text search, security intelligence, business analytics, and operational intelligence use cases. This is based on Apache Lucene.
There is another logging library present called _ELMAH_ which used to be free but now it is subscription based library.

I have tried to build _SWCLogging_ which is also based on _Apache Lucene_ which provides you a way to log and search capability in its built in search page.
# What?
SWCLogging is a logging library which helps us to not only in logging but also provides us an in built user interface to search and analyze the log.

# How?
I have used the capability of _Apache Lucene_ in the library and created a logging provider which in turn could be used as a logging provider in any .Net Core application. This is very easy to use and integrate.

# Security
The built in page that this library has returns the whole HTML of the page in string format. Now it is upto you to get this HTML and place in your applicaton's page(cshtml). So whoever has right to see this particular page those will have access to the loging page. Here security is completely in your control.

# Screenshots

![Log](https://user-images.githubusercontent.com/73790753/236464681-823f4aac-ce3e-46b2-bc5f-43267a21761a.JPG)

![log search](https://user-images.githubusercontent.com/73790753/236464708-a4697249-98cf-4cf0-899d-029fd55c8b7e.JPG)
# Logging Verbosity
It takes care of logging verbosity also mentioned in the appsettings.json file. Here the "Default" cateogry is "Information" it denotes minimum label to log for the category.
"Microsoft" cateogry is "Critical" it denotes minimum label to log for the category.
```sh
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Critical",
      "Microsoft.Hosting.Lifetime": "Critical"
    }
  }
}
```

# Integration

> I. Install SWCLogging
```sh
NuGet\Install-Package SWCLogging -Version 1.0.4
```
> II. - Create a controller called SWCLController
      - Add two namespaces.
      - Add a constructor with parameter Ilogger and ILoggerProvider which will automatically be initialized by dependency injection.
```sh
using Microsoft.Extensions.Logging;
using SWCLoggingLibrary;
public class SWCLController : Controller
{
    private readonly ILogger<SWCLController> _logger;
    private readonly ILoggerProvider _loggerProvider;
        
    public SWCLController(ILogger<SWCLController> logger, ILoggerProvider loggerProvider)
    {
        _logger = logger;
        _loggerProvider = loggerProvider;
    }
}
```
> III. - Add the Index method to get the search page
       - Add a post method "GetLogs" which would help us fetching the logs on the search page based on our search criteria.
```sh
public IActionResult Index()
{
    SWCLogging swclog = SWCLogging.GetInstance((SWCLoggingProvider)_loggerProvider);
    ViewBag.Data = swcl.GetSearchPage();
    return View();
}

[HttpPost]
public IActionResult GetLogs(SWCSearchRequest model)
{
    SWCLogging swclog = SWCLogging.GetInstance((SWCLoggingProvider)_loggerProvider);
    var res = swclog.SearchLogs(model);
    return Json(new { result = res });
}
```
> IV. - Add an index view
      - Set the Layout to null (_`do not forget this step`_)
      - Place the HTML on the page
```sh
@{
    Layout = null;
}
@Html.Raw(ViewBag.Data)
```
> V. Add _SWCLogging_ as a logging provider in `Program.cs`
```sh
public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddSWCLogging();
    })
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.UseStartup<Startup>();
    });
```
# Usage
This is very easy to use. To use the logging we only need to use the `_logger` object. Here are all the methods for different log level. Every method have different parameters which could be used to add more information in the log.
```sh
_logger.LogInformation("This is log information")
_logger.LogDebug("Home screen Starting up");
_logger.LogError("This is error log of home screen.");
_logger.LogTrace("This is trace log");
_logger.LogCritical(ex,"This is critical log");
_logger.LogWarning("Just a warning");
```
We also have a way to define `scope` of logs. For example, if we want to search logs for specific method/functionality then we may define `scope` and through this we can find all the logs which falls in this scope.
In below example we can find all logs related to this functionality through the text `"Payment_09843"` as this text is defined in the `scope`. And you will also find this text in each log with key `scope`.
```sh
using (_logger.BeginScope("Payment_09843"))
{
    _logger.LogInformation("Payment module starts.")
    _logger.LogWarning("Remarks not found.");
    _logger.LogCritical("Some error has occurred at step 5");
    _logger.LogInformation("Payment module end.")
}
```
# Where
Since this is based on _Apache Lucene_, it creates logs in a folder(swclog) in `bin` folder of your project. If you need to cleanup all the logs, you only need to empty this folder.


# Launch
To launch the search page you only need to access the Index action from SWCLController
```sh
<url>/SWCL | localhost:<port number>/SWCL
```

# License

MIT

**Enjoy!**
