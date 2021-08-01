using System.Diagnostics;
using System.Threading.Tasks;

namespace Conductor.RunProcess
{
	public record RunResult(int ExitCode, string Output, string Error);

	public class ProcessRunner
	{
		public async Task<RunResult> RunProcessAsync(string processName, string arguments, string workingDirectory)
		{
			var startInfo = new ProcessStartInfo(processName, arguments)
			{
				UseShellExecute = false,
				WorkingDirectory = workingDirectory,
				RedirectStandardOutput = true,
				RedirectStandardError = true
			};

			using var process = Process.Start(startInfo);
			if (process == null)
			{
				return null;
			}

			string output = await process.StandardOutput.ReadToEndAsync();
			string error = await process.StandardError.ReadToEndAsync();
			await process.WaitForExitAsync();

			return new RunResult(process.ExitCode, output, error);
		}
	}
}
