using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Conductor.Abstractions;

namespace Conductor.Channels.DependencyDetailsReaders.SystemTextJson.Extensions
{
    public static class DependencyExtensions
    {
        public static async Task WriteSystemTextJson(this DependencyMetadata dependencyMetadata, string fileName, CancellationToken cancellationToken = default)
        {
            await using var stream = File.Create(fileName);
            await JsonSerializer.SerializeAsync(stream, dependencyMetadata, cancellationToken: cancellationToken);
        }

        public static Task<string> WriteSystemTextJson(this Dependency dependency, string fileName)
        {
            return Task.FromResult(string.Empty);
            //dependency.Name;
            //dependency.Version;
            //dependency.Uri;
            //dependency.Sha;
            //dependency.Expression;
            //dependency.DependencyType;
            //dependency.Pinned;
        }
    }
}
