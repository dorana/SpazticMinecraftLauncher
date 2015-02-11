using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Epicserver_Minecraft_Installer
{
    /// <summary>
    /// The JSON file structure for minecraft.
    /// </summary>
    public class MCIdentifer
    {
        public struct Lib
        {
            public struct Rules
            {
                public struct OS
                {
                    public string name { get; set; }
                    public string version { get; set; }
                }
                public string action { get; set; }
                public OS os { get; set; }
            }
            public struct Natives
            {
                public string linux { get; set; }
                public string windows { get; set; }
                public string osx { get; set; }
            }
            public struct Extract
            {
                public string[] exclude { get; set; }
            }

            public string url { get; set; }
            public string name { get; set; }
            public Rules[] rules { get; set; }
            public Natives natives { get; set; }
            public Extract extract { get; set; }
            public string serverreq { get; set; }
        }

        public string id { get; set; }
        public string time { get; set; }
        public string releaseTime { get; set; }
        public string type { get; set; }
        public string minecraftArguments { get; set; }
        public Lib[] libraries { get; set; }
        public string mainClass { get; set; }
        public int minimumLauncherVersion { get; set; }

        /// <summary>
        /// Turn a .json file from minecraft to a format which can be easily iterated.
        /// </summary>
        /// <param name="TargetFile">Path to the file (any extension, but usually .json or .txt)</param>
        /// <returns>The MCIdentifier object that reflects the targeted json file.</returns>
        public static MCIdentifer Identify(string TargetFile)
        {
            if (File.Exists(TargetFile))
            {
                MCIdentifer mcIdentifier = null;
                try
                {
                    StreamReader reader = new StreamReader(TargetFile);
                    mcIdentifier = JsonConvert.DeserializeObject<MCIdentifer>(reader.ReadToEnd());
                    return mcIdentifier;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Couldn't parse Json File:");
                    Console.WriteLine(ex.Message);
                }
            }
            return null;
        }
        /// <summary>
        /// Create a MCIdentifier. It is unused, but might be useful in the future,
        /// when custom libraries need to be added automatically rather than manually.
        /// </summary>
        /// <param name="identifier">Creates a JSON file in Minecraft format.</param>
        /// <param name="TargetFile">for example @"C:\myIdentifier.json"</param>
        /// <returns>True if the operation succeeded.</returns>
        public static bool Create(MCIdentifer identifier, string TargetFile)
        {
            try
            {
                JsonSerializerSettings jsonSettings = new JsonSerializerSettings
                {
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                };
                string json = JsonConvert.SerializeObject(identifier, Formatting.Indented);

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(TargetFile))
                {
                    file.WriteLine(json);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Couldn't write Json File:");
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
