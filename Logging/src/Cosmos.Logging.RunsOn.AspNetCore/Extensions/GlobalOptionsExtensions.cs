﻿using System;
using Cosmos.Logging.Configurations;
using Cosmos.Logging.Core;
using Cosmos.Logging.RunsOn.AspNetCore.Core;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection {
    public static class GlobalOptionsExtensions {
        public static ILogServiceCollection ToGlobal(this ILogServiceCollection services, Action<LoggingOptions> settingsAct) {
            var settings = new LoggingOptions();
            settingsAct?.Invoke(settings);
            return ToGlobal(services, settings);
        }

        public static ILogServiceCollection ToGlobal<TLoggingSettings>(this ILogServiceCollection services, Action<TLoggingSettings> settingsAct)
            where TLoggingSettings : LoggingOptions, new() {
            var settings = new TLoggingSettings();
            settingsAct?.Invoke(settings);
            return ToGlobal(services, settings);
        }

        public static ILogServiceCollection ToGlobal<TLoggingSettings>(this ILogServiceCollection services, TLoggingSettings settings)
            where TLoggingSettings : LoggingOptions, new() {
            if (services is AspNetCoreLogServiceCollection collection) {
                collection.ReplaceSettings(settings);
            }

            return services;
        }
    }
}