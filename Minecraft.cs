/*
 * FileName: Minecraft.cs
 * Author: Wiebe Geertsma
 * Date: 9-8-2013
 * Description:
 *      Collection of functions that are minecraft related.
 * 
 * Copyright (C) 2013 Positive Computers
 * 
 * This file is subject to the terms and conditions defined in
 * file 'GPL.txt', which is part of this source code package.
 */

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.IO;

//Additional includes
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;

using Epicserver_Minecraft_Installer;
using Epicserver_Minecraft_Installer.Properties;
using System.Collections;
using System.Net;
using System.Threading.Tasks;
using System.Threading;

using Newtonsoft;
using Newtonsoft.Json;

namespace Minecraft
{
    /// <summary>
    /// Handles all launcher related functions
    /// </summary>
    public static class Launcher
    {
        /// <summary>
        /// Container for filling in all the required information
        /// required by the launcher.
        /// </summary>
        public class LaunchInfo
        {
            public string clientVersion = "13", username, password, character;
            public EpicSettings epicSettings;
            public bool quickStart = false,
                        launchWithForge = false;

            public string sessionID, UID;
            public MainForm caller;
        }

        private static string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private static string minecraftDir = appData + @"\.minecraft";
        private static string nativesDir = minecraftDir + @"\libraries\org\lwjgl\lwjgl\lwjgl-platform\2.9.1\";

        /// <summary>
        /// Credits go to 'Reed Copsey'.
        /// Async response function so that the application will not block.
        /// </summary>
        /// <param name="request">The request to send</param>
        /// <param name="token">The CancellationToken</param>
        /// <returns></returns>
        public static Task<WebResponse> GetResponseAsync(this WebRequest request, CancellationToken token)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            bool timeout = false;
            TaskCompletionSource<WebResponse> completionSource = new TaskCompletionSource<WebResponse>();

            AsyncCallback completedCallback =
                result =>
                {
                    try
                    {
                        completionSource.TrySetResult(request.EndGetResponse(result));
                    }
                    catch (WebException ex)
                    {
                        if (timeout)
                            completionSource.TrySetException(new WebException("No response was received during the time-out period for a request.", WebExceptionStatus.Timeout));
                        else if (token.IsCancellationRequested)
                            completionSource.TrySetCanceled();
                        else
                            completionSource.TrySetException(ex);
                    }
                    catch (Exception ex)
                    {
                        completionSource.TrySetException(ex);
                    }
                };

            IAsyncResult asyncResult = request.BeginGetResponse(completedCallback, null);
            if (!asyncResult.IsCompleted)
            {
                if (request.Timeout != Timeout.Infinite)
                {
                    WaitOrTimerCallback timedOutCallback =
                        (object state, bool timedOut) =>
                        {
                            if (timedOut)
                            {
                                timeout = true;
                                request.Abort();
                            }
                        };

                    ThreadPool.RegisterWaitForSingleObject(asyncResult.AsyncWaitHandle, timedOutCallback, null, request.Timeout, true);
                }

                if (token != CancellationToken.None)
                {
                    WaitOrTimerCallback cancelledCallback =
                        (object state, bool timedOut) =>
                        {
                            if (token.IsCancellationRequested)
                                request.Abort();
                        };

                    ThreadPool.RegisterWaitForSingleObject(token.WaitHandle, cancelledCallback, null, Timeout.Infinite, true);
                }
            }

            return completionSource.Task;
        }

        /// <summary>
        /// Used to generate a session with login.minecraft.net for online use only
        /// </summary>
        /// <param name="username">The player's username</param>
        /// <param name="password">The player's password</param>
        /// <param name="clientVer">The client version (look here http://wiki.vg/Session)</param>
        /// <returns></returns>
        public static void GenerateSession(LaunchInfo launchInfo)
        {
            string requestURL = string.Format(
                "https://authserver.mojang.com?user={0}&password={1}&version={2}",
                launchInfo.username,
                launchInfo.password,
                launchInfo.clientVersion);
            requestURL.Split(':');
            //requestURL = "https://minecraft.net/login";

            // Make a web request and do it in a seperate thread
            // to prevent blocking the Interface.
            //WebClient wc = new WebClient();
            //var nvc = new NameValueCollection();
            //nvc.Add("username",launchInfo.username);
            //nvc.Add("password", launchInfo.password);
            //byte[] resp = wc.UploadValues(requestURL,"POST", nvc);
            //string stringResponse = Encoding.UTF8.GetString(resp);


            WebRequest request = WebRequest.Create(requestURL);
            Task<WebResponse> waitingResponse = GetResponseAsync(request, new CancellationToken());

            // Wait until the request comes back with a response.
            waitingResponse.Wait();
            WebResponse response = waitingResponse.Result;
            StreamReader responseStream = new StreamReader(response.GetResponseStream());

            string stringResponse = responseStream.ReadToEnd();
            stringResponse = stringResponse.Trim();

            // Returns 5 values split by ":" Current Version : Download Ticket : Username : Session ID : UID.
            // http://wiki.vg/Legacy_Authentication
            // For information.
            string[] mcSession = stringResponse.Split(':');

            if (mcSession[0] == "Bad login")
            {
                Console.WriteLine("Unable to Login: " + mcSession[0]);
                launchInfo.caller.Message_InvalidInfo();
            }
            else
            {
                Console.WriteLine("Successfully logged in as " + mcSession[2]);

                // Write information to the container.
                launchInfo.character = mcSession[2];
                launchInfo.sessionID = mcSession[3];
                launchInfo.UID = mcSession[4];

                Start(launchInfo);
            }
        }        
        
        /// <summary>
        /// Generates a session and starts Minecraft if the session is created successfully.
        /// </summary>
        public static void Start(LaunchInfo launchInfo)
        {
            string launchArgs = "";

            Program.SaveCredentials(launchInfo.username, launchInfo.password);

            if (!Directory.Exists(nativesDir))
                return;

            //////////////////////////////////////////////////////////////////////////
            // Check if the natives have been extracted from their jar yet.
            if (!File.Exists(nativesDir + "lwjgl.dll"))
            {
                if (!File.Exists(nativesDir + launchInfo.epicSettings.launcherSettings.nativesVersion))
                {
                    // Find alternatives.
                    string[] altFiles = Directory.GetFiles(nativesDir, "lwjgl-platform-*");
                    if (altFiles != null && altFiles.Length > 1)
                    {

                    }

                    // We aren't seeing the correct version of the files.
                    MessageBox.Show("Couldn't find " + launchInfo.epicSettings.launcherSettings.nativesVersion, "Wrong Version ?", MessageBoxButtons.OK);
                    launchInfo.caller.Message_LaunchFailed("File version mismatch.");
                }
                // The natives need to be extracted so this program can read the .dll's that are packed inside the jar file.
                Epicserver_Minecraft_Installer.Program.ExtractJar(nativesDir + "lwjgl-platform-2.9.1-natives-windows.jar", nativesDir);
            }
            //////////////////////////////////////////////////////////////////////////
            // Build the launch arguments for java.

            var debug = launchInfo.epicSettings.launcherSettings.minecraftVersion.Substring(0, launchInfo.epicSettings.launcherSettings.minecraftVersion.Length - 3);

            MCIdentifer mcIdentity = MCIdentifer.Identify(
                appData + @"\.minecraft\versions\" +
                launchInfo.epicSettings.launcherSettings.minecraftVersion.Substring(0, launchInfo.epicSettings.launcherSettings.minecraftVersion.Length-4) + @"\" +
                launchInfo.epicSettings.launcherSettings.minecraftVersion.Substring(0, launchInfo.epicSettings.launcherSettings.minecraftVersion.Length-4) + ".json");
            
            MCIdentifer forgeIdentity = MCIdentifer.Identify(
                appData + @"\.minecraft\versions\" +
                launchInfo.epicSettings.launcherSettings.forgeVersion.Substring(0, launchInfo.epicSettings.launcherSettings.forgeVersion.Length - 4) + @"\" +
                launchInfo.epicSettings.launcherSettings.forgeVersion.Substring(0, launchInfo.epicSettings.launcherSettings.forgeVersion.Length - 4) + ".json");


            if (CheckLibraryPresence(mcIdentity, launchInfo.epicSettings, forgeIdentity, launchInfo.launchWithForge))
                launchInfo.caller.Message_LaunchFailed("Missing essential library files.");

            launchArgs += "java -Djava.library.path=" + nativesDir + " -cp "; // Without natives, minecraft won't launch.
            launchArgs += 
                appData + @"\.minecraft\versions\" + 
                launchInfo.epicSettings.launcherSettings.minecraftVersion.Substring(0, 6) + @"\" + 
                launchInfo.epicSettings.launcherSettings.minecraftVersion + ";";

            if (mcIdentity == null)
                return;

            if (!launchInfo.launchWithForge)
            {
                
                foreach (MCIdentifer.Lib lib in mcIdentity.libraries)
                {
                    string[] semiCSplit = lib.name.Split(':');
                    string jarName = semiCSplit[semiCSplit.Length - 2] + "-" + semiCSplit[semiCSplit.Length - 1] + ".jar";
                    if (GetLibrary(jarName) != null && !jarName.Contains("debug"))
                        launchArgs += GetLibrary(jarName) + ";";
                }
            }
            if(launchInfo.launchWithForge)
            {
                foreach(MCIdentifer.Lib lib in forgeIdentity.libraries)
                {
                    string[] semiCSplit = lib.name.Split(':');
                    string jarName = semiCSplit[semiCSplit.Length - 2] + "-" + semiCSplit[semiCSplit.Length - 1] + ".jar";
                    if (GetLibrary(jarName) != null && !jarName.Contains("debug"))
                        launchArgs += GetLibrary(jarName) + ";";
                }
                launchArgs += " net.minecraft.launchwrapper.Launch ";
                launchArgs += " --tweakClass cpw.mods.fml.common.launcher.FMLTweaker";
            }
            else
            {
                launchArgs += " net.minecraft.client.main.Main ";
            }

            launchArgs += " --username=" + launchInfo.character;
            //launchArgs += " --password=" + userPass;
            launchArgs += " --accessToken " + launchInfo.sessionID;
            launchArgs += " --version 1.7.4";
            launchArgs += " --gameDir {0}";
            launchArgs += " --assetsDir {0}\\assets\\virtual\\legacy ";
            launchArgs += launchInfo.epicSettings.launcherSettings.jvmArguments;
            launchArgs = String.Format(launchArgs, minecraftDir, "");
            launchArgs += " --userProperties {}";
            launchArgs += " --uuid " + launchInfo.UID;
            //////////////////////////////////////////////////////////////////////////

            //if (launchInfo.epicSettings.debugMode)
                Console.WriteLine(launchArgs);
            
            launchInfo.caller.Message_LaunchSucceeded(); // Signal the caller, everything went OK!

            Program.ExecCommand(launchArgs, launchInfo.caller.closeWhenMinecraftStartsToolStripMenuItem.Checked);
        }

        /// <summary>
        /// General file check. Are there any libraries missing ?
        /// The user is notified about those that are missing in a message box.
        /// </summary>
        /// <param name="checkForge">Wether forge libraries should be included.</param>
        /// <returns>True if some are missing.</returns>
        private static bool CheckLibraryPresence(MCIdentifer mcID, EpicSettings epicSettings, MCIdentifer forge = null, bool checkForge = false)
        {
            string[] libsPresent = Directory.GetFiles(appData + @"\.minecraft\libraries", "*.jar", SearchOption.AllDirectories);
            ArrayList missingLibraries = new ArrayList();

            if (!File.Exists(appData + @"\.minecraft\versions\" + epicSettings.launcherSettings.minecraftVersion.Substring(0, 6) + @"\" + epicSettings.launcherSettings.minecraftVersion))
            {
                MessageBox.Show("Couldn't find " + epicSettings.launcherSettings.minecraftVersion, "Minecraft Launcher", MessageBoxButtons.OK);
                return true;
            }

            if (mcID.libraries.Length == 0)
            {
                MessageBox.Show("Couldn't identify the JSON file of your version.");
                return true;
            }

            // Loop through the libraries for starting minecraft normally (w/o forge)
            if(!checkForge)
            {
                foreach (MCIdentifer.Lib lib in mcID.libraries)
                {
                    string[] semiCSplit = lib.name.Split(':');
                    string jar = semiCSplit[semiCSplit.Length - 2] + "-" + semiCSplit[semiCSplit.Length - 1] + ".jar";

                    if (jar.Contains("platform"))
                        jar = jar.Substring(0, jar.Length - 4) + "-natives-windows" + ".jar";
                    if (jar.Contains("twitch-platform-5.12-natives-windows") || jar.Contains("twitch-external-platform-4.5-natives-windows"))
                        jar = jar.Substring(0, jar.Length - 4) + "-64" + ".jar";
                    if (!jar.Contains(epicSettings.launcherSettings.minecraftVersion) && !jar.Contains("debug"))
                    {
                        bool present = false;
                        foreach (string libPath in libsPresent)
                        {
                            if (libPath.Contains(jar))
                                present = true;
                        }
                        if (!present)
                            missingLibraries.Add(jar);
                    }
                }
            }
            if (checkForge)
            {
                if (!File.Exists(appData + @"\.minecraft\versions\" + epicSettings.launcherSettings.forgeVersion.Substring(0, epicSettings.launcherSettings.forgeVersion.Length - 4) + @"\" + epicSettings.launcherSettings.forgeVersion))
                {
                    MessageBox.Show("Couldn't find forge. See Options > Version options", "Minecraft Forge Launcher", MessageBoxButtons.OK);
                    return true;
                }
                if (forge == null)
                {
                    MessageBox.Show("Couldn't open forge JSON file.");
                    return true;
                }
                if (forge.libraries.Length == 0)
                {
                    MessageBox.Show("Couldn't identify the JSON file of your version.");
                    return true;
                }

                // Loop through forge libraries
                foreach (MCIdentifer.Lib lib in forge.libraries)
                {
                    string[] semiCSplit = lib.name.Split(':');
                    string jar = semiCSplit[semiCSplit.Length - 2] + "-" + semiCSplit[semiCSplit.Length - 1] + ".jar";

                    if (jar.Contains("platform"))
                        jar = jar.Substring(0, jar.Length - 4) + "-natives-windows" + ".jar";
                    if (jar.Contains("twitch-platform-5.12-natives-windows") || jar.Contains("twitch-external-platform-4.5-natives-windows"))
                        jar = jar.Substring(0, jar.Length - 4) + "-64" + ".jar";
                    if (!jar.Contains(epicSettings.launcherSettings.forgeVersion) && !jar.Contains("debug"))
                    {
                        bool present = false;
                        foreach (string libPath in libsPresent)
                        {
                            if (libPath.Contains(jar))
                                present = true;
                        }
                        if (!present)
                            missingLibraries.Add(jar);
                    }
                }
            }

            if (missingLibraries.Count > 0)
            {
                string errorMsg = "SORRY! These libraries couldn't be found: \n \n";
                foreach (string lib in missingLibraries)
                {
                    errorMsg += lib + "\n";
                }
                errorMsg += "\n Press OK to try anyway, like you give a fuck RIGHT?";
                    errorMsg += "\n (Try using the regular launcher once, it will download them)";
                MessageBox.Show(errorMsg, "Missing libraries", MessageBoxButtons.OK);
            }

            return false;
        }

        /// <summary>
        /// Get a specific library path just by passing in the required jar name.
        /// </summary>
        /// <param name="jarFile">The required jar file.</param>
        /// <returns>The path of the jar file, and otherwise null.</returns>
        private static string GetLibrary(string jarFile)
        {
            string[] libraryPaths = Directory.GetFiles(appData + @"\.minecraft\libraries", "*.jar", SearchOption.AllDirectories);
            string[] versionPaths = Directory.GetFiles(appData + @"\.minecraft\versions", "*.jar", SearchOption.AllDirectories);

            if (jarFile.Contains("platform"))
                jarFile = jarFile.Substring(0, jarFile.Length - 4) + "-natives-windows" + ".jar";
            if (jarFile.Contains("twitch-platform-5.12-natives-windows") || jarFile.Contains("twitch-external-platform-4.5-natives-windows"))
                jarFile = jarFile.Substring(0, jarFile.Length - 4) + "-64" + ".jar";

            foreach (string lib in libraryPaths)
                if (lib.Contains(jarFile) && !jarFile.Contains("debug"))
                    return lib;
            foreach (string lib in versionPaths)
                if (lib.Contains(jarFile) && !jarFile.Contains("debug"))
                    return lib;

            Console.WriteLine("Couldn't find " + jarFile);
            return null;
        }
    }

    
}