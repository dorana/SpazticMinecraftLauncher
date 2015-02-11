using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;

namespace Epicserver_Minecraft_Installer
{
    /// <summary>
    /// A class created to achieve persistent data for this program.
    /// The file is stored in the .minecraft folder called EpicSettings.json
    /// It can be modified manually!
    /// </summary>
    public class EpicSettings
    {
        public struct Statistics
        {
            public string LastUpdatedDate;
        }
        public struct LauncherSettings
        {
            public string minecraftVersion;
            public string forgeVersion;
            public string nativesVersion;
            public string jvmArguments;
        }

        public bool debugMode;
        public bool closeWhenStarting;
        public bool autoCheckUpdates;
        public bool launchWithForge;
        public bool ignoreAlreadyRunningMsg;
        public string quickStartUrl;
        public string mapServerUrl;
        public Statistics statistics;
        public LauncherSettings launcherSettings;

        /// <summary>
        /// Reads a JSON file that has the EpicSettings structure.
        /// </summary>
        /// <param name="TargetFile">Path to the JSON file.</param>
        /// <returns>The deserialized values</returns>
        public static EpicSettings Read(string TargetFile)
        {
            if (File.Exists(TargetFile))
            {
                EpicSettings settings = null;
                try
                {
                    StreamReader reader = new StreamReader(TargetFile);
                    settings = JsonConvert.DeserializeObject<EpicSettings>(reader.ReadToEnd());
                    reader.Close();
                    return settings;
                }
                catch (Exception ex)
                {
                    string title = "Read Error";
                    string message = "Couldn't parse Settings Json File: \n " + ex.Message +
                        "\n It could be that the structure has changed by the author \n" +
                        "Delete this file and generate a new one?";
                    DialogResult result = MessageBox.Show(message, title, MessageBoxButtons.YesNo);

                    switch (result)
                    {
                        case DialogResult.Yes:
                            File.Delete(TargetFile);
                            break;
                        case DialogResult.No:
                            return null;
                        default: // In case the user presses the X button.
                            return null;
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// Create a EpicSettings structure.
        /// </summary>
        /// <param name="identifier">Creates a JSON file in EpicSettings structure.</param>
        /// <param name="TargetFile">for example @"C:\EpicSettings.json"</param>
        /// <returns>True if the operation succeeded.</returns>
        public static bool Create(EpicSettings settings, string TargetFile)
        {
            JsonSerializerSettings jsonSettings = new JsonSerializerSettings
            {
                TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Full,
                DateFormatHandling = DateFormatHandling.MicrosoftDateFormat,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            };
            string json = JsonConvert.SerializeObject(settings, Formatting.Indented);

            RetryCreation:
            try
            {
                System.IO.StreamWriter file = new System.IO.StreamWriter(TargetFile);
            	file.WriteLine(json);
                file.Close();
            }
            catch (System.IO.IOException ex)
            {
                if (MessageBox.Show("File is in use. \n" + ex.Message, "Apparently...", MessageBoxButtons.RetryCancel) == DialogResult.Retry)
                    goto RetryCreation;
            }
            return true;            
        }
        /// <summary>
        /// Gets a default setting to create a file with.
        /// </summary>
        /// <returns>A default EpicSettings class</returns>
        public static EpicSettings GetDefault()
        {
            EpicSettings defaultSettings = new EpicSettings();
            EpicSettings.Statistics defaultStatistics = new EpicSettings.Statistics();
            EpicSettings.LauncherSettings defaultLauncherSettings = new EpicSettings.LauncherSettings();

            defaultSettings.debugMode = false;
            defaultSettings.closeWhenStarting = true;
            defaultSettings.launchWithForge = false;
            defaultSettings.quickStartUrl = "epicserver.nl";
            defaultSettings.mapServerUrl = "http://www.minecraft.net/";
            defaultSettings.ignoreAlreadyRunningMsg = false;

            defaultLauncherSettings.minecraftVersion = "1.6.2.jar";
            defaultLauncherSettings.forgeVersion = "1.6.2-Forge9.10.0.841.jar";
            defaultLauncherSettings.nativesVersion = "lwjgl-platform-2.9.1-natives-windows.jar";

            defaultStatistics.LastUpdatedDate = System.DateTime.Now.Date.ToString();

            defaultSettings.statistics = defaultStatistics;
            defaultSettings.launcherSettings = defaultLauncherSettings;
            return defaultSettings;
        }
    }
}
