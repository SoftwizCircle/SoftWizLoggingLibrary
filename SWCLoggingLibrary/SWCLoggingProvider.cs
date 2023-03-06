using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWCLoggingLibrary
{
    [ProviderAlias("SWCLuceneSearchLogging")]
    public class SWCLoggingProvider : ILoggerProvider, ISupportExternalScope
    {
        ConcurrentDictionary<string, SWCLogging> swcLogging = new ConcurrentDictionary<string, SWCLogging>();
        public readonly SWCLoggingOptions Options;
        IExternalScopeProvider _scopeProvider;
        protected IDisposable SettingsChangeToken;

        /// <summary>
        /// Called by the logging framework in order to set external scope information source for the logger provider. 
        /// <para>ISupportExternalScope implementation</para>
        /// </summary>
        void ISupportExternalScope.SetScopeProvider(IExternalScopeProvider scopeProvider)
        {
            _scopeProvider = scopeProvider;
        }

        internal IExternalScopeProvider ScopeProvider
        {
            get
            {
                if (_scopeProvider == null)
                {
                    _scopeProvider = new LoggerExternalScopeProvider();
                }

                return _scopeProvider;
            }
        }

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
            return swcLogging.GetOrAdd(categoryName,
            (category) =>
            {
                return SWCLogging.GetInstance(this, category);
            });
        }

        public void Dispose()
        {
            
        }
    }
}
