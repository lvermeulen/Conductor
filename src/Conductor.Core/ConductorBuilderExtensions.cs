using Conductor.Abstractions;

namespace Conductor.Core
{
    public static class ConductorBuilderExtensions
    {
        public static ConductorBuilder WithDependencyDetailsFile(this ConductorBuilder builder, string fileName, string subPath, DependencyFileType fileType)
        {
            builder.DependencyDetailsFiles.Add(new DependencyDetailsFile(fileName, subPath, fileType));
            return builder;
        }

        public static ConductorBuilder WithDependencyExpressionFile(this ConductorBuilder builder, string fileName, string subPath, ExpressionFileType fileType)
        {
            builder.ExpressionFiles.Add(new ExpressionFile(fileName, subPath, fileType));
            return builder;
        }

        public static ConductorBuilder WithJsonSerializer(this ConductorBuilder builder, JsonSerializerType jsonSerializerType)
        {
	        builder.JsonSerializer = jsonSerializerType;
	        return builder;
        }
    }
}
