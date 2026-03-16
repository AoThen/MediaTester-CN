using KrahmerSoft.MediaTesterLib;
using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace KrahmerSoft.MediaTesterGui
{
	internal static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main()
		{
			Options mediaTesterOptions = null;
			try
			{
				mediaTesterOptions = Options.Deserialize();
			}
			catch
			{
				MessageBox.Show(Strings.ErrorLoadingOptionsDetails, Strings.ErrorLoadingOptions, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			bool[] restartAfterClose = new bool[] { false };
			string previousLanguage = null;
			do
			{
				string language = mediaTesterOptions.LanguageCode;

				try
				{
					language = mediaTesterOptions.LanguageCode;
					if (string.IsNullOrEmpty(language))
						language = Thread.CurrentThread.CurrentCulture.Name;

					// Only set culture if it's different from what we had before
					// This ensures resources are reloaded when language changes
					if (previousLanguage != language)
					{
						var newCulture = new CultureInfo(language);
						Thread.CurrentThread.CurrentCulture = newCulture;
						Thread.CurrentThread.CurrentUICulture = newCulture;
						CultureInfo.DefaultThreadCurrentCulture = newCulture;
						CultureInfo.DefaultThreadCurrentUICulture = newCulture;
						previousLanguage = language;
					}
				}
				catch
				{
					string invalidLanguageDetails = Strings.InvalidLanguageDetails
						.Replace("{InvalidLanguageName}", language)
						.Replace("{DefaultLanguageName}", Thread.CurrentThread.CurrentCulture.Name);
					MessageBox.Show(invalidLanguageDetails, Strings.InvalidLanguage, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}

				MessageBoxManager.OK = Strings.ButtonOk;
				MessageBoxManager.Yes = Strings.ButtonYes;
				MessageBoxManager.No = Strings.ButtonNo;
				MessageBoxManager.Register();

				restartAfterClose[0] = false;
				Application.Run(new Main(mediaTesterOptions, restartAfterClose));
			} while (restartAfterClose[0]);
		}
	}
}
