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
                .WithDependencyExpressionFile("Version.props", "eng", ExpressionFileType.Xml)
                .WithDependencyExpressionFile("global.json", "eng", ExpressionFileType.Json)
                .WithDependencyExpressionFile("VersionProps.json", "eng", ExpressionFileType.Json)
                .WithDependencyExpressionFile("BuildToolsVersion.txt", "eng", ExpressionFileType.Text)
                .WithDependencyExpressionFile("DotNetCLIVersion.txt", "eng", ExpressionFileType.Text)
                .WithJsonSerializer(JsonSerializerType.SystemTextJson)
                .Build();

            services.AddSingleton(conductor);

            return services;
        }
    }
}
