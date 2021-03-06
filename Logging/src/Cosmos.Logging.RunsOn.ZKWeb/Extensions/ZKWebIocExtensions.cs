﻿using System;
using Cosmos.Logging.Configurations;
using Cosmos.Logging.Core;
using Cosmos.Logging.RunsOn.ZKWeb;
using Cosmos.Logging.RunsOn.ZKWeb.Core;
using Microsoft.Extensions.Options;
using ZKWebStandard.Ioc;
using ZKWebStandard.Ioc.Extensions;

// ReSharper disable once CheckNamespace
namespace Cosmos.Logging {
    // ReSharper disable once InconsistentNaming
    public static class ZKWebIocExtensions {

        public static IContainer RegisterCosmosLogging(this IContainer ioc, Action<ILogServiceCollection> config) {
            var serviceImpl = new ZkWebLogServiceCollection();

            config?.Invoke(serviceImpl);

            ioc.Register<ILoggingServiceProvider, ZKWebLoggingServiceProvider>(ReuseType.Singleton);

            serviceImpl.BuildConfiguration();
            serviceImpl.ActiveSinkSettings();
            serviceImpl.ActiveOriginConfiguration();

            ioc.RegisterFromServiceCollection(serviceImpl.ExposeServices());
            ioc.RegisterInstance(Options.Create((LoggingOptions) serviceImpl.ExposeLogSettings()), ReuseType.Singleton);
            ioc.RegisterInstance(serviceImpl.ExposeLoggingConfiguration(), ReuseType.Singleton);

            StaticServiceResolver.SetResolver(ioc.Resolve<ILoggingServiceProvider>());

            return ioc;
        }
    }
}