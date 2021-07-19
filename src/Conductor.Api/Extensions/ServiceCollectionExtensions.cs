using Conductor.Abstractions;
using Conductor.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Conductor.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConductor(this IServiceCollection services)
        {
            var conductor = new ConductorBuilder()
                .WithDependencyDetailsFile("Version.Details.xml", "eng", DependencyFileType.Xml)
                .WithDependencyDetailsFile("Version.Details.json", "eng", DependencyFileType.Json)
                .WithDependencyExpressionFile("Version.props", "eng", DependencyFileType.Xml)
                .WithDependencyExpressionFile("global.json", "eng", DependencyFileType.Json)
                .WithDependencyExpressionFile("VersionProps.json", "eng", DependencyFileType.Json)
                .WithDependencyExpressionFile("BuildToolsVersion.txt", "eng", DependencyFileType.Text)
                .WithDependencyExpressionFile("DotNetCLIVersion.txt", "eng", DependencyFileType.Text)
                .Build();

            services.AddSingleton(conductor);

            return services;
        }
    }
}
