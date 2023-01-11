# Why?
The idea of building this library coined when I was going through features of .Net core and we got to know about ILogger interface. I got to know about the mechanism that there is a way to write my own logging provider and !voila.

I am always facinated about the speed of the _Elasticsearch_ which is used for log analytics, full-text search, security intelligence, business analytics, and operational intelligence use cases. This is based on Apache Lucene.
There is another logging library present called _ELMAH_ which used to be free but now it is subscription based library.

I have tried to build _SWCLogging_ which is also based on _Apache Lucene_ which provides you a way to log and search capability in its built in search page.
# What?
SWCLogging is a logging library which helps us to not only in logging but also provides us an in built user interface to search and analyze the log.

# How?
I have used the capability of _Apache Lucene_ in the library and created a logging provider which in turn could be used as a logging provider in any .Net Core application. This is very easy to use and integrate.

# Security
The built in page that this library has returns the whole HTML of the page in string format. Now it is upto you to get this HTML and place in your applicaton's page(cshtml). So whoever has right to see this particular page those will have access to the loging page. Here security is completely in your control.

# Screenshots

![log information](https://user-images.githubusercontent.com/73790753/211767436-7db231f6-22c3-48b4-adf3-f234886d3f4d.JPG)

![file not found exception](https://user-images.githubusercontent.com/73790753/211767453-264da7f7-0a42-41ba-b2fe-0c38ddcdaf13.JPG)

# Integration

> I. Install SWCLoggging
```sh
NuGet\Install-Package SWCLogging -Version 1.0.2
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
    SWCLogging swcl = new SWCLogging((SWCLoggingProvider)_loggerProvider);
    ViewBag.Data = swcl.GetSearchPage();
    return View();
}

[HttpPost]
public IActionResult GetLogs(SWCSearchRequest model)
{
    SWCLogging swclog = new SWCLogging((SWCLoggingProvider)_loggerProvider);
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
# Launch
To launch the search page you only need to access the Index action from SWCLController
```sh
<url>/SWCL | localhost:<port number>/SWCL
```

# License

MIT

**Enjoy!**
