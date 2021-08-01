using System;
using System.IO;
using System.Threading;

namespace Conductor.Tests
{
	public sealed class AutoCleanupFolder : IDisposable
	{
		private readonly string _folderPath;

		public AutoCleanupFolder(string folderPath)
		{
			_folderPath = folderPath;
		}

		public static void DeleteDirectory(string directoryPath)
        {
            // from http://stackoverflow.com/questions/329355/cannot-delete-directory-with-directory-deletepath-true/329502#329502
            if (!Directory.Exists(directoryPath))
            {
                return;
            }
            NormalizeAttributes(directoryPath);
            DeleteDirectory(directoryPath, maxAttempts: 5, initialTimeout: 16, timeoutFactor: 2);
        }

        private static void NormalizeAttributes(string directoryPath)
        {
            string[] filePaths = Directory.GetFiles(directoryPath);
            string[] subdirectoryPaths = Directory.GetDirectories(directoryPath);

            foreach (string filePath in filePaths)
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
            }
            foreach (string subdirectoryPath in subdirectoryPaths)
            {
                NormalizeAttributes(subdirectoryPath);
            }
            File.SetAttributes(directoryPath, FileAttributes.Normal);
        }

        private static void DeleteDirectory(string directoryPath, int maxAttempts, int initialTimeout, int timeoutFactor)
        {
            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    Directory.Delete(directoryPath, true);
                    return;
                }
                catch
                {
                    if (attempt < maxAttempts)
                    {
                        Thread.Sleep(initialTimeout * (int)Math.Pow(timeoutFactor, attempt - 1));
                    }
                }
            }
        }

		public void Dispose()
		{
			DeleteDirectory(_folderPath);
		}
	}
}
