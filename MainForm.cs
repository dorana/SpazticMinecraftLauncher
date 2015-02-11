/*
 * FileName: MainForm.cs
 * Author: Wiebe Geertsma
 * Date: 9-8-2013
 * Description:
 *      Handles all elements of the main form.
 * 
 * Copyright (C) 2013 Positive Computers
 * 
 * This file is subject to the terms and conditions defined in
 * file 'GPL.txt', which is part of this source code package.
 */

using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Minecraft;
using System.Xml.Linq;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;

namespace Epicserver_Minecraft_Installer
{
    public partial class MainForm : Form
    {
        private static string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public static string settingsPath = appData + @"\.minecraft\EpicSettings.json";
        public EpicSettings settings;
        public OutputWindow outputWindow = new OutputWindow();

        private string _clientVersion;
        private string _serverVersion;
        private XDocument _config;

        /// <summary>
        /// Executed on runtime
        /// </summary>
        public MainForm()
        {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);

            InitializeComponent();
            LoadConfig();
            _clientVersion = _config.Descendants("version").First().Value;
            _serverVersion = GetServerVersion();
            progressBar.Value = 100;
            SetStatus("Ready.");
        }

        public string GetServerVersion()
        {
            try
            {
                return Regex.Replace(new WebClient().DownloadString("http://davenport.mine.nu/davenport.ver"), @"\t|\n|\r", "");
            }
            catch
            {
                MessageBox.Show("Unable to get server version");
            }
            return string.Empty;
        }

        private void GetAddons()
        {
            CompareVersions();
        }

        private void CompareVersions()
        {
            if (!string.Equals(_serverVersion.Trim(), _clientVersion.Trim(), StringComparison.InvariantCulture))
            {
                Downloader dlhandler = new Downloader();
                dlhandler.ShowDialog();
            }
            _clientVersion = _serverVersion;
            _config.Descendants("version").First().Value = _clientVersion;
            File.Delete("localdata.xml");
            File.WriteAllText("localdata.xml", _config.ToString());
        }

        private void LoadConfig()
        {
            _config = XDocument.Load("localdata.xml");
            var xElement = _config.Element("clientdata");
            if (xElement != null)
            {
                var element = xElement.Element("installed");
                if (element != null && (xElement != null && !bool.Parse(element.Value)))
                {
                    //Lunch Installer wizard
                }
            }
        }

        /// <summary>
        /// Occurs when the resolution of an assembly fails.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string dllName = args.Name.Contains(',') ? args.Name.Substring(0, args.Name.IndexOf(',')) : args.Name.Replace(".dll", "");

            dllName = dllName.Replace(".", "_");

            if (dllName.EndsWith("_resources")) return null;

            System.Resources.ResourceManager rm = new System.Resources.ResourceManager(GetType().Namespace + ".Properties.Resources", System.Reflection.Assembly.GetExecutingAssembly());

            byte[] bytes = (byte[])rm.GetObject(dllName);

            return System.Reflection.Assembly.Load(bytes);
        }      
        /// <summary>
        /// Executed on load of mainForm.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            //////////////////////////////////////////////////////////////////////////
            // Get previous settings for this launcher.
            #region Get EpicSettings

            if (System.IO.File.Exists(settingsPath))
            {
                settings = EpicSettings.Read(settingsPath);
                closeWhenMinecraftStartsToolStripMenuItem.Checked = settings.closeWhenStarting;
                automaticallyCheckForUpdatesToolStripMenuItem.Checked = settings.autoCheckUpdates;
                launchWithForge.Checked = settings.launchWithForge;
            }
            else
            {
                settings = EpicSettings.GetDefault();
                EpicSettings.Create(settings, settingsPath);
            }
            #endregion
            //////////////////////////////////////////////////////////////////////////
            // Check if minecraft (or any java process) is running.
            #region Check for running processes
            RetryCheckJava:
            if (Program.IsJavaRunning() && !settings.ignoreAlreadyRunningMsg)
            {
                var message = "An instance of Java is already running (Minecraft?) \n This COULD cause issues.";
                var title = "Continue?";
                var result = MessageBox.Show(message, title, MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Question);

                switch (result)
                {
                    case DialogResult.Abort:
                        Application.Exit();
                        break;
                    case DialogResult.Retry:
                        goto RetryCheckJava;
                    case DialogResult.Ignore:
                        if (MessageBox.Show("Ignore 'Already Running' message in the future?", "Notification", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            settings.ignoreAlreadyRunningMsg = true;
                            EpicSettings.Create(settings, settingsPath);
                        }
                        break;
                    default:
                        // X button was pressed. Abort then anyway.
                        Application.Exit();
                        break;
                }
            }
            #endregion
            //////////////////////////////////////////////////////////////////////////
            // Check for previous login credentials.
            #region Get Login Credentials
            if (System.IO.File.Exists(appData + @"\.minecraft\EpicLoginProtected.txt"))
            {
                string storedCredentials;

                try
                {
                    Program.DecryptFile(appData + @"\.minecraft\EpicLoginProtected.txt", appData + @"\.minecraft\EpicLogin.txt", Program.GetKey());
                    
                }
                catch (Exception ex)
                {
                    System.IO.File.Delete(appData + @"\.minecraft\EpicLoginProtected.txt");
                    MessageBox.Show("This current EpicLogin file is not valid for this machine. \n A new one is generated the next time you successfully login. \n" + ex.Message);
                }
                if(System.IO.File.Exists(appData + @"\.minecraft\EpicLogin.txt"))
                {
                    
                    storedCredentials = System.IO.File.ReadAllText(appData + @"\.minecraft\EpicLogin.txt");
                    System.IO.File.Delete(appData + @"\.minecraft\EpicLogin.txt");

                    string[] epicLoginCreds = storedCredentials.Split('&');
                    string[] strByteUser = epicLoginCreds[0].Split(':');
                    string[] strBytePass = epicLoginCreds[1].Split(':');

                    byte[] byteUser = new byte[strByteUser.Length];
                    byte[] bytePass = new byte[strBytePass.Length];

                    for(int i = 0; i < strByteUser.Length; i++)
                        byteUser[i] = Byte.Parse(strByteUser[i]);
                    for (int i = 0; i < strBytePass.Length; i++)
                        bytePass[i] = Byte.Parse(strBytePass[i]);

                    string storedUser = System.Text.Encoding.UTF8.GetString(byteUser);
                    string storedPass = System.Text.Encoding.UTF8.GetString(bytePass);

                    usernameInput.Text = storedUser.ToString();
                    passwordInput.Text = storedPass.ToString();
                }
            }
            #endregion

            GetAddons();
        }
        /// <summary>
        /// Exit the application button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        /// <summary>
        /// Opens a website in the default browser.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChangeVersion_Click(object sender, EventArgs e)
        {
            if(settings.mapServerUrl != null)
            try 
            {
                System.Diagnostics.Process.Start(settings.mapServerUrl);
            }
            catch (Exception ex)
            {
                string exception = ex.Message;
                MessageBox.Show("Couldn't open the map, goto options, change map server URL, \n make sure the URL looks like: \n http://example.com");
            }
            
        }
        /// <summary>
        /// Quickstart to EpicServer.nl button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            SetStatus("Logging In...");
            progressBar.Value = 10;
            progressBar.Value = 100;

            Launcher.LaunchInfo launchInfo = new Launcher.LaunchInfo();

            launchInfo.username = usernameInput.Text;
            launchInfo.password = passwordInput.Text;
            launchInfo.epicSettings = settings;
            launchInfo.quickStart = true;
            launchInfo.launchWithForge = launchWithForge.Checked;
            launchInfo.caller = this;

            Launcher.GenerateSession(launchInfo);
        }
        
        
        
        /// <summary>
        /// Displays the status in the application.
        /// </summary>
        /// <param name="statusText"></param>
        public void SetStatus(string statusText)
        {
            status.Text = statusText;
        }
        /// <summary>
        /// Listen for ENTER keypresses after typing the password, this
        /// starts minecraft normally.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void passwordInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                toolStripButton1_Click(sender, e);
            }
        }
        /// <summary>
        /// Start minecraft normally button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            SetStatus("Logging In...");
            progressBar.Value = 10;
            progressBar.Value = 100;

            Launcher.LaunchInfo launchInfo = new Launcher.LaunchInfo();

            launchInfo.username = usernameInput.Text;
            launchInfo.password = passwordInput.Text;
            launchInfo.epicSettings = settings;
            launchInfo.quickStart = false;
            launchInfo.launchWithForge = launchWithForge.Checked;
            launchInfo.caller = this;

            Launcher.GenerateSession(launchInfo);
        }
        /// <summary>
        /// Open explorer and go to the %Appdata%/.minecraft/ folder.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (System.IO.Directory.Exists(appData + "\\.minecraft"))
                Process.Start(appData + "\\.minecraft");
        }
        /// <summary>
        /// Open up a new window for the output to be seen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void showConsoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            outputWindow.Show();
        }
        /// <summary>
        /// Button to change the quickstart server URL.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void changeQuickstartServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new QuickstartEditor(this).Show();
        }
        /// <summary>
        /// Creates a window to edit the default map server url.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            new MapUrlEditor(this).Show();
        }
        /// <summary>
        /// Creates a window to edit the versions used to start Minecraft.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void versionOptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new VersionSelect(this).Show();
        }

        private void closeWhenMinecraftStartsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            closeWhenMinecraftStartsToolStripMenuItem.Checked = !closeWhenMinecraftStartsToolStripMenuItem.Checked;
            settings.closeWhenStarting = closeWhenMinecraftStartsToolStripMenuItem.Checked;
            EpicSettings.Create(settings, settingsPath);
        }

        private void automaticallyCheckForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            automaticallyCheckForUpdatesToolStripMenuItem.Checked = !automaticallyCheckForUpdatesToolStripMenuItem.Checked;
            settings.autoCheckUpdates = automaticallyCheckForUpdatesToolStripMenuItem.Checked;
            EpicSettings.Create(settings, settingsPath);
        }

        private void launchWithForge_Click(object sender, EventArgs e)
        {
            launchWithForge.Checked = !launchWithForge.Checked;
            settings.launchWithForge = launchWithForge.Checked;
            EpicSettings.Create(settings, settingsPath);
        }

        private void jVMOptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new JVMEditor(this).Show();
        }

        /// <summary>
        /// Signals that the attempt to login has failed.
        /// </summary>
        public void Message_InvalidInfo()
        {
            SetStatus("Invalid Login !");
            progressBar.Value = 0;
        }

        public void Message_LaunchFailed(string errorMsg)
        {
            Console.WriteLine(errorMsg);
            if (errorMsg.Length > 40)
                SetStatus("Launch failed: " + errorMsg.Substring(0,40));
            else
                SetStatus("Launch failed: " + errorMsg.Substring(0, errorMsg.Length));
            progressBar.Value = 0;
        }

        public void Message_LaunchSucceeded()
        {
            Console.WriteLine("Launch Succeeded");
            SetStatus("Launch Succeeded");
            progressBar.Value = 100;
        }

        public void Message_Custom(string msg)
        {
            Console.WriteLine(msg);
            SetStatus(msg);
            progressBar.Value = 100;
        }
    }
}
