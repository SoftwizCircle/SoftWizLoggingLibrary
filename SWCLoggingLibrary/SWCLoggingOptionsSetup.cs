using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWCLoggingLibrary
{
    internal class SWCLoggingOptionsSetup : ConfigureFromConfigurationOptions<SWCLoggingOptions>
    {
        /// <summary>
        /// Constructor that takes the IConfiguration instance to bind against.
        /// </summary>
        public SWCLoggingOptionsSetup(ILoggerProviderConfiguration<SWCLoggingProvider> providerConfiguration)
            : base(providerConfiguration.Configuration)
        {
        }
    }
}
