using System;
using Conductor.Core;
using Microsoft.Extensions.Hosting;
using Serilog.Events;
using Serilog;
using Serilog.Configuration;

namespace Conductor.Api.Extensions
{
	public static class SerilogExtensions
	{
		private static LoggerConfiguration WithOperationId(this LoggerEnrichmentConfiguration enrichConfiguration)
		{
			if (enrichConfiguration is null)
			{
				throw new ArgumentNullException(nameof(enrichConfiguration));
			}

			return enrichConfiguration.With<OperationIdEnricher>();
		}

		public static LoggerConfiguration ConfigureBaseLogging(this LoggerConfiguration loggerConfiguration, HostBuilderContext context, IServiceProvider services, string appName, BuildInformation buildInformation)
		{
			loggerConfiguration
				.MinimumLevel.Debug()
				.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
				.Enrich.WithOperationId()
				.Enrich.FromLogContext()
				.Enrich.WithMachineName()
				.Enrich.WithEnvironmentUserName()
				.Enrich.WithThreadId()
				.Enrich.WithProperty("ApplicationName", appName)
				.Enrich.WithProperty(nameof(buildInformation.BuildId), buildInformation.BuildId)
				.Enrich.WithProperty(nameof(buildInformation.BuildNumber), buildInformation.BuildNumber)
				.Enrich.WithProperty(nameof(buildInformation.BranchName), buildInformation.BranchName)
				.Enrich.WithProperty(nameof(buildInformation.CommitHash), buildInformation.CommitHash)
				.WriteTo.Async(x => x.Console())
				.ReadFrom.Configuration(context.Configuration)
				.ReadFrom.Services(services);

			return loggerConfiguration;
		}

		public static void ConfigureSerilog(this LoggerConfiguration _, HostBuilderContext context, IServiceProvider services, string appName, BuildInformation buildInformation)
		{
			Log.Logger = new LoggerConfiguration()
				.ConfigureBaseLogging(context, services, appName, buildInformation)
				.CreateLogger();
		}
	}
}
