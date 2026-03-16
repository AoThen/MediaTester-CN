using System.IO;
using System.Text.Json;
using System.Threading;

namespace KrahmerSoft.MediaTesterLib
{
	public class Options
	{
		public bool StopProcessingOnFailure { get; set; } = true;
		public bool QuickTestAfterEachFile { get; set; } = true;
		public string TestDirectory { get; set; }
		public bool QuickFirstFailingByteMethod { get; set; } = true;
		public bool RemoveTempDataFilesUponCompletion { get; set; } = true;
		public bool SaveTestResultsFileToMedia { get; set; } = true;
		public long MaxBytesToTest { get; set; } = -1;
		public string LanguageCode { get; set; }

		/// <summary>
		/// Cancellation token source for graceful operation cancellation.
		/// Replaces Thread.Abort() for cross-platform compatibility.
		/// </summary>
		[System.Text.Json.Serialization.JsonIgnore]
		public CancellationTokenSource CancellationTokenSource { get; set; } = new CancellationTokenSource();

		/// <summary>
		/// Gets the cancellation token for checking cancellation requests.
		/// </summary>
		[System.Text.Json.Serialization.JsonIgnore]
		public CancellationToken CancellationToken => CancellationTokenSource.Token;

		/// <summary>
		/// Requests cancellation of the current operation.
		/// </summary>
		public void Cancel()
		{
			CancellationTokenSource.Cancel();
		}

		/// <summary>
		/// Resets the cancellation token for a new operation.
		/// </summary>
		public void ResetCancellation()
		{
			CancellationTokenSource?.Dispose();
			CancellationTokenSource = new CancellationTokenSource();
		}

		public const string CONFIG_FILENAME = "MediaTesterOptions.json";

		public void Serialize(string filePath = CONFIG_FILENAME)
		{
			File.WriteAllText(filePath, JsonSerializer.Serialize(this));
		}

		public static Options Deserialize(string filePath = CONFIG_FILENAME)
		{
			Options options;
			try
			{
				options = JsonSerializer.Deserialize<Options>(File.ReadAllText(filePath));
			}
			catch //(Exception ex)
			{
				// If anything goes wrong just create a default object
				options = new Options();
			}

			return options;
		}
	}
}
