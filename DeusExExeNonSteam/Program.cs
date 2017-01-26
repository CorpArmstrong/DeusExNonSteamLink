using System;
using System.Text;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;

namespace DeusExExeNonSteam
{
    class Program
    {
        struct SteamData
        {
            public string steamPath;
            public string steamArgs;
        }

        static void Main(string[] args)
        {
            LaunchDeusEx(args);
        }

        /// <summary>
        /// Launch the legacy application with some options set.
        /// </summary>
        static void LaunchDeusEx(string[] args)
        {
            string filename = "config.json";
            string pathWithFilename = Path.Combine(Directory.GetCurrentDirectory(), filename);
            SteamData steamData = new SteamData();

            // If config file doesn't exists, than create one with default launch values
            if (!File.Exists(pathWithFilename))
            {
                ProvideDefaultLaunchValues(steamData, pathWithFilename);
            }
            else
            {
                try
                {
                    steamData = JsonConvert.DeserializeObject<SteamData>(File.ReadAllText(pathWithFilename));
                }
                catch (Exception ignored)
                {
                    // If config file is corrupted, than create one with default launch values
                    ProvideDefaultLaunchValues(steamData, pathWithFilename);
                }
            }

            try
            {
                using (Process exeProcess = Process.Start(GetSteamProcess(args, steamData)))
                {
                    //exeProcess.WaitForExit();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static void ProvideDefaultLaunchValues(SteamData steamData, string pathWithFilename)
        {
            steamData.steamPath = "C:\\program files (x86)\\steam\\steam.exe";
            steamData.steamArgs = "-applaunch 397550";
            string json = JsonConvert.SerializeObject(new { SteamPath = steamData.steamPath, SteamArgs = steamData.steamArgs });
            File.WriteAllText(pathWithFilename, json);
        }

        static ProcessStartInfo GetSteamProcess(string[] args, SteamData steamData)
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
