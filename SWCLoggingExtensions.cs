﻿using Microsoft.Extensions.DependencyInjection;
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
    public static class SWCLoggingExtensions
    {
        //public static ILoggingBuilder AddSWCLogging(this ILoggingBuilder builder, Action<SWCLoggingOptions> configure)
        //{
        //    builder.Services.AddSingleton<ILoggerProvider, SWCLoggingProvider>();
        //    builder.Services.Configure(configure);
        //    return builder;
        //}

        static public ILoggingBuilder AddSWCLogging(this ILoggingBuilder builder)
        {
            builder.AddConfiguration();

            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, SWCLoggingProvider>());
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IConfigureOptions<SWCLoggingOptions>, SWCLoggingOptionsSetup>());
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IOptionsChangeTokenSource<SWCLoggingOptions>, LoggerProviderOptionsChangeTokenSource<SWCLoggingOptions, SWCLoggingProvider>>());
            return builder;
        }

        /// <summary>
        /// Adds the file logger provider, aliased as 'File', in the available services as singleton and binds the file logger options class to the 'File' section of the appsettings.json file.
        /// </summary>
        static public ILoggingBuilder AddSWCLogging(this ILoggingBuilder builder, Action<SWCLoggingOptions> configure)
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            builder.Services.AddSingleton<ILoggerProvider, SWCLoggingProvider>();
            builder.AddSWCLogging();
            builder.Services.Configure(configure);

            return builder;
        }
    }
}
