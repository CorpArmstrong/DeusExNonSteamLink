using System;
using System.Text;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;

namespace DeusExExeNonSteam
{
    public class SteamLauncher
    {
        private string[] _args;

        public SteamLauncher(string[] args)
        {
            _args = args;
        }

        /// <summary>
        /// Launch the legacy application with some options set.
        /// </summary>
        public void LaunchDeusEx()
        {
            string filename = "config.json";
            string pathWithFilename = Path.Combine(Directory.GetCurrentDirectory(), filename);
            SteamData steamData = new SteamData();

            // Default launch values:
            steamData.steamPath = "C:\\program files (x86)\\steam\\steam.exe";
            steamData.steamArgs = "-applaunch 397550";

            // If config file doesn't exists, than create one.
            if (!File.Exists(pathWithFilename))
            {
                GenerateConfigFile(steamData, pathWithFilename);
            }
            else
            {
                try
                {
                    steamData = JsonConvert.DeserializeObject<SteamData>(File.ReadAllText(pathWithFilename));
                }
                catch (Exception ignored)
                {
                    // If config file is corrupted, than create one.
                    GenerateConfigFile(steamData, pathWithFilename);
                    Console.WriteLine(ignored.Message);
                }
            }

            try
            {
                using (Process exeProcess = Process.Start(GetSteamProcess(_args, steamData)))
                {
                    //exeProcess.WaitForExit();
                }
            }
            catch (Exception ignored)
            {
                Console.WriteLine(ignored.Message);
            }
        }

        private void GenerateConfigFile(SteamData steamData, string pathWithFilename)
        {
            try
            {
                string json = JsonConvert.SerializeObject(new { SteamPath = steamData.steamPath, SteamArgs = steamData.steamArgs });
                File.WriteAllText(pathWithFilename, json);
            }
            catch (Exception ignored)
            {
                Console.WriteLine(ignored.Message);
            }
        }

        private ProcessStartInfo GetSteamProcess(string[] args, SteamData steamData)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            startInfo.FileName = steamData.steamPath;
            startInfo.Arguments = steamData.steamArgs;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;

            if (args != null)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < args.Length; i++)
                {
                    sb.Append(" " + args[i] + " ");
                }
                startInfo.Arguments += sb.ToString().TrimEnd();
            }

            return startInfo;
        }
    }
}
