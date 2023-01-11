using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWCLoggingLibrary
{
    [ProviderAlias("SWCLuceneSearchLogging")]
    public class SWCLoggingProvider : ILoggerProvider
    {
        public readonly SWCLoggingOptions Options;

        public SWCLoggingProvider(IOptions<SWCLoggingOptions> options)
        {
            Options = options.Value;

            if (!Directory.Exists(Options.FolderPath))
            {
                Directory.CreateDirectory(Options.FolderPath);
            }
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new SWCLogging(this);
        }

        public void Dispose()
        {
            
        }
    }
}
