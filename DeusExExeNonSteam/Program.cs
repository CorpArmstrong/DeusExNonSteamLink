using System;
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
            LaunchDeusEx();
        }

        /// <summary>
        /// Launch the legacy application with some options set.
        /// </summary>
        static void LaunchDeusEx()
        {
            //Read config file here:
            SteamData steamData = new SteamData();
            steamData.steamPath = "C:\\program files (x86)\\steam\\steam.exe";
            steamData.steamArgs = "-applaunch 397550";

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            startInfo.FileName = steamData.steamPath;
            startInfo.Arguments = steamData.steamArgs;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;

            string json = JsonConvert.SerializeObject(new { Name = "JC", Age = 23 });
            string path = Directory.GetCurrentDirectory();
            string filename = "config.json";

            File.WriteAllText(Path.Combine(path, filename), json);
            File.WriteAllText(Path.Combine(path, filename), steamData.steamPath + " " + steamData.steamArgs);

            try
            {
                using (Process exeProcess = Process.Start(startInfo))
                {
                    //exeProcess.WaitForExit();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
