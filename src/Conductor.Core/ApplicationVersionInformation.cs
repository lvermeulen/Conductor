using System;
using System.IO;
using System.Text.Json;

namespace Conductor.Core
{
	public record BuildInformation(string BranchName, string BuildNumber, string BuildId, string CommitHash);

	public class ApplicationVersionInformation
	{
		private const string buildFileName = "buildinfo.json";

		private BuildInformation _fileBuildInfo = new BuildInformation(
			BranchName: "",
			BuildNumber: DateTime.UtcNow.ToString("yyyyMMdd") + ".0",
			BuildId: "xxxxxx",
			CommitHash: $"Not yet initialized - call {nameof(InitializeBuildInformationFromFolder)}"
		);

		public void InitializeBuildInformationFromFolder(string path)
		{
			string buildFilePath = Path.Combine(path, buildFileName);
			if (!File.Exists(buildFilePath))
			{
				return;
			}

			try
			{
				string buildInfoJson = File.ReadAllText(buildFilePath);
				var buildInformation = JsonSerializer.Deserialize<BuildInformation>(buildInfoJson, new JsonSerializerOptions
				{
					PropertyNamingPolicy = JsonNamingPolicy.CamelCase
				});

				_fileBuildInfo = buildInformation ?? throw new InvalidOperationException($"Failed to deserialize {buildFileName}");
			}
			catch (Exception)
			{
				_fileBuildInfo = new BuildInformation(
					BranchName: "",
					BuildNumber: DateTime.UtcNow.ToString("yyyyMMdd") + ".0",
					BuildId: "xxxxxx",
					CommitHash: "Failed to load build info from buildinfo.json"
				);
			}
		}

		public BuildInformation GetBuildInformation() => _fileBuildInfo;
	}
}
