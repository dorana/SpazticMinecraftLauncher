/*
 * FileName: Program.cs
 * Author: Wiebe Geertsma
 * Date: 9-8-2013
 * Description:
 *      Contains all background functions that have nothing
 *      to do with all forms.
 * 
 * Copyright (C) 2013 Positive Computers
 * 
 * This file is subject to the terms and conditions defined in
 * file 'GPL.txt', which is part of this source code package.
 */

using System;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using ICSharpCode.SharpZipLib.Zip;
using System.Diagnostics;

namespace Epicserver_Minecraft_Installer
{
    internal class Program
    {
        private static string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public static MainForm mainForm;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(mainForm = new MainForm());
        }

        

        /// <summary>
        /// Read everything in a text file, and return it in one string.
        /// </summary>
        /// <param name="Path">Path to the text file</param>
        /// <returns>The content of the file as a string</returns>
        public static string GetFileText(string Path)
        {
            String retVal = "";
            try
            {
                using (StreamReader sr = new StreamReader(Path))
                {
                    retVal = sr.ReadToEnd();
                    Console.WriteLine(sr);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            if (retVal == "")
            {
                Console.WriteLine("GetFileText used, but the file was empty.");
            }
            return retVal;
        }

        /// <summary>
        /// Encrypts the lock file.
        /// </summary>
        /// <param name="sInputFilename"></param>
        /// <param name="sOutputFilename"></param>
        /// <param name="sKey"></param>
        public static void EncryptFile(string sInputFilename, string sOutputFilename, string sKey)
        {
            FileStream fsInput = new FileStream(sInputFilename,
               FileMode.Open,
               FileAccess.Read);

            FileStream fsEncrypted = new FileStream(sOutputFilename,
               FileMode.Create,
               FileAccess.Write);
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            ICryptoTransform desencrypt = DES.CreateEncryptor();
            CryptoStream cryptostream = new CryptoStream(fsEncrypted,
               desencrypt,
               CryptoStreamMode.Write);

            byte[] bytearrayinput = new byte[fsInput.Length];
            fsInput.Read(bytearrayinput, 0, bytearrayinput.Length);
            cryptostream.Write(bytearrayinput, 0, bytearrayinput.Length);
            cryptostream.Close();
            fsInput.Close();
            fsEncrypted.Close();
        }
        
        /// <summary>
        /// Decrypts the lock file.
        /// </summary>
        /// <param name="sInputFilename"></param>
        /// <param name="sOutputFilename"></param>
        /// <param name="sKey"></param>
        public static void DecryptFile(string sInputFilename, string sOutputFilename, string sKey)
        {
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            //A 64 bit key and IV is required for this provider.
            //Set secret key For DES algorithm.
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            //Set initialization vector.
            DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);

            //Create a file stream to read the encrypted file back.
            FileStream fsread = new FileStream(sInputFilename,
                                           FileMode.Open,
                                           FileAccess.Read);
            //Create a DES decryptor from the DES instance.
            ICryptoTransform desdecrypt = DES.CreateDecryptor();
            //Create crypto stream set to read and do a 
            //DES decryption transform on incoming bytes.
            CryptoStream cryptostreamDecr = new CryptoStream(fsread,
                                                         desdecrypt,
                                                         CryptoStreamMode.Read);
            //Print the contents of the decrypted file.
            StreamWriter fsDecrypted = new StreamWriter(sOutputFilename);
            fsDecrypted.Write(new StreamReader(cryptostreamDecr).ReadToEnd());
            fsDecrypted.Flush();
            fsDecrypted.Close();
            fsread.Close();
        }

        /// <summary>
        /// Save the username and password using encryption
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        internal static void SaveCredentials(string user, string password)
        {
            byte[] storedUser = Encoding.UTF8.GetBytes(user);
            byte[] storedPass = Encoding.UTF8.GetBytes(password);
            string[] storedData = new String[2];
            foreach (byte b in storedUser)
                storedData[0] += b.ToString() + ":";
            foreach (byte b in storedPass)
                storedData[1] += b.ToString() + ":";

            storedData[0] = storedData[0].Remove(storedData[0].Length - 1);
            storedData[1] = storedData[1].Remove(storedData[1].Length - 1);

            string epicLoginCreds = storedData[0] + "&" + storedData[1];

            if (File.Exists(appData + @"\.minecraft\EpicLogin.txt"))
                File.Delete(appData + @"\.minecraft\EpicLogin.txt");
            System.IO.File.WriteAllText(appData + @"\.minecraft\EpicLogin.txt", epicLoginCreds);
            FileInfo protectionFile = new FileInfo(appData + @"\.minecraft\EpicLoginProtected.txt");
            if (File.Exists(appData + @"\.minecraft\EpicLoginProtected.txt"))
            {
                for (int attempts = 0; attempts <= 3 && Program.IsFileLocked(protectionFile); attempts++ )
                {
                    try
                    {
                        System.IO.File.Delete(appData + @"\.minecraft\EpicLoginProtected.txt");
                    }
                    catch (System.Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(0.3));
                    if (attempts == 3)
                    {   
                        MessageBox.Show("Couldn't access EpicLoginProtected after 3 tries.");
                        return;
                    }
                }
            }
            Program.EncryptFile(appData + @"\.minecraft\EpicLogin.txt", appData + @"\.minecraft\EpicLoginProtected.txt", GetKey());
            System.IO.File.Delete(appData + @"\.minecraft\EpicLogin.txt");
        }

        /// <summary>
        /// Function to Generate a 64 bits Key.
        /// </summary>
        /// <returns></returns>
        public static string GenerateKey()
        {
            // Create an instance of Symetric Algorithm. Key and IV is generated automatically.
            DESCryptoServiceProvider desCrypto = (DESCryptoServiceProvider)DESCryptoServiceProvider.Create();

            // Use the Automatically generated key for Encryption. 
            return ASCIIEncoding.ASCII.GetString(desCrypto.Key);
        }

        /// <summary>
        /// Calculates the MD5 Hash.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Check if the given file is locked, being written to.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
            return false;
        }

        /// <summary>
        /// Author: "salysle" from codeproject.com
        /// Source: http://www.codeproject.com/Articles/27606/Opening-Jars-with-C
        /// 
        /// Used under the terms of The Code Project Open License (CPOL).
        /// http://www.codeproject.com/info/cpol10.aspx
        /// 
        /// Extracts the jar file to a specified
        /// destination folder.  
        /// </summary>
        public static void ExtractJar(string pathToJar, string saveFolderPath)
        {
            string jarPath = pathToJar;
            string savePath = saveFolderPath;

            try
            {
                // verify the paths are set
                if (!String.IsNullOrEmpty(jarPath) &&
                   !String.IsNullOrEmpty(saveFolderPath))
                {
                    try
                    {
                        // use the SharpZip library FastZip
                        // utilities ExtractZip method to 
                        // extract the jar file
                        FastZip fz = new FastZip();
                        fz.ExtractZip(jarPath, saveFolderPath, "");
                    }
                    catch (Exception ex)
                    {
                        // something went wrong
                        MessageBox.Show(ex.Message, "Extraction Error");
                    }
                }
                else
                {
                    // the paths were not, tell the user to 
                    // get with the program

                    StringBuilder sb = new StringBuilder();
                    sb.Append("Set the paths to both the jar file and " + Environment.NewLine);
                    sb.Append("destination folder before attempting to " + Environment.NewLine);
                    sb.Append("to extract a jar file.");

                    MessageBox.Show(sb.ToString(), "Unable to Extract");
                }
            }
            catch (Exception ex)
            {
                // something else went wrong
                MessageBox.Show(ex.Message, "Extraction Method Error");
            }
        }

        public static FileInfo GetLibrary(string targetFileName)
        {
            FileInfo retVal = null;
            DirectoryInfo dir = new DirectoryInfo(appData + @"\.minecraft\libraries");

            foreach (FileInfo file in dir.GetFiles())
            {
                if (file.Name == targetFileName)
                    return file;
            }

            return retVal;
        }

        /// <summary>
        /// Executes the command as a batch file (used from Minecraft.cs)
        /// </summary>
        /// <param name="command"></param>
        public static void ExecCommand(string command, bool closeAppAfterwards)
        {
            if (!closeAppAfterwards)
            {
                Process process = new Process();
                ProcessStartInfo processInfo = new ProcessStartInfo("cmd.exe", "/c " + command);
                processInfo.CreateNoWindow = true;
                processInfo.UseShellExecute = false;
                processInfo.RedirectStandardError = true;
                processInfo.RedirectStandardOutput = true;
                processInfo.WindowStyle = ProcessWindowStyle.Maximized;
                process.StartInfo = processInfo;
                process.Start();

                // *** Read the streams ***
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                int exitCode = process.ExitCode;

                //this.output.outputTextBox.Text += "output>>" + (String.IsNullOrEmpty(output) ? "(none)" : output);
                Console.WriteLine("output>>" + (String.IsNullOrEmpty(output) ? "(none)" : output));
                Console.WriteLine("error>>" + (String.IsNullOrEmpty(error) ? "(none)" : error));
                Console.WriteLine("ExitCode: " + exitCode.ToString(), "ExecuteCommand");
            }
            else
            {
                ProcessStartInfo processInfo = new ProcessStartInfo("cmd.exe", "/c " + command);

                processInfo.CreateNoWindow = true;
                processInfo.UseShellExecute = false;
                processInfo.RedirectStandardError = true;
                processInfo.RedirectStandardOutput = true;
                processInfo.WindowStyle = ProcessWindowStyle.Maximized;                

                Process.Start(processInfo);



                Application.Exit();
            }
        
        }

        /// <summary>
        /// Check if any instance of java is currently running.
        /// </summary>
        /// <returns></returns>
        public static bool IsJavaRunning()
        {
            System.Diagnostics.Process[] pname = System.Diagnostics.Process.GetProcessesByName("java");
            if (pname.Length != 0)
                return true;
            return false;
        }

        /// <summary>
        /// Get a system-specific key.
        /// </summary>
        /// <returns></returns>
        internal static string GetKey()
        {
            string key = System.Environment.MachineName;
            key = key.Substring(0, 5);
            key += "???";
            return key;
        }
    }
}
