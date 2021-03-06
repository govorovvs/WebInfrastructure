﻿namespace Web
{
    using Infrastructure.Web.Configuration;
    using Infrastructure.Web.ExceptionsHandling;
    using Infrastructure.Web.Logging;
    using Infrastructure.Web.OutputFormatting;
    using Infrastructure.Web.Routing;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class Startup
    {
        public Startup(IHostingEnvironment env, CommandLineArgumentsProvider commandLineArgumentsProvider)
        {
            var builder = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddCommandLine(commandLineArgumentsProvider.Arguments)
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", false)
                .AddNLogConfig($"NLog.{env.EnvironmentName}.config");

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc(x => x
                            .UseCentralRoutePrefix($"{Configuration.GetValue("api_route_preffix", "api")}/[controller]")
                            .UseUnhandledExceptionFilter())
                .AddJsonOptions(x => x.SerializerSettings.UseDefaultSettings());

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory
                .AddConsole(Configuration.GetSection("Logging"))
                .AddNLog();

            app.UseMvc();
        }
    }
}