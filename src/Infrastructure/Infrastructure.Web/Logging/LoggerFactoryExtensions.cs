﻿namespace Infrastructure.Web.Logging
{
    using System.Reflection;
    using Microsoft.Extensions.Logging;
    using NLog;
    using NLog.Extensions.Logging;

    public static class LoggerFactoryExtensions
    {
        public static ILoggerFactory AddNLog(this ILoggerFactory factory)
        {
            LogManager.AddHiddenAssembly(Assembly.Load(new AssemblyName("Microsoft.Extensions.Logging")));
            LogManager.AddHiddenAssembly(Assembly.Load(new AssemblyName("Microsoft.Extensions.Logging.Abstractions")));
            LogManager.AddHiddenAssembly(typeof(AspNetExtensions).GetTypeInfo().Assembly);
            LogManager.AddHiddenAssembly(typeof(LoggerFactoryExtensions).GetTypeInfo().Assembly);

            factory.AddProvider(new NLogLoggerProvider());
            return factory;
        }
    }
}