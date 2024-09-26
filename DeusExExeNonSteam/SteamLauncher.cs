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

        public void StartDeusEx()
        {
            try
            {
                string steamUrl = $"steam://rungameid/{6910}//-ini=";
                Process.Start(new ProcessStartInfo
                {
                    FileName = steamUrl,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }
        
        /// <summary>
        /// Launch the legacy application with some options set.
        /// </summary>
        public void LaunchDeusEx()
        {
            string filename = "config.json";
            string pathWithFilename = Path.Combine(Directory.GetCurrentDirectory(), filename);
            
            // steam://rungameid/6910//-ini="D:\SteamLibrary\steamapps\common\Deus Ex\CodenameNebula\System\CNN.ini" -userini="D:\SteamLibrary\steamapps\common\Deus Ex\CodenameNebula\System\CNNUser.ini"
            
            var steamData = new SteamData();
            var currentDirectory = Directory.GetCurrentDirectory();
            
            // Old:
            // var cnnIniFilePath = $"-ini=\"{Path.Combine(currentDirectory, "System\\CNN.ini")}\"";
            // var cnnUserIniFilePath = $"-userini=\"{Path.Combine(currentDirectory, "System\\CNNUser.ini")}\"";
            
            var cnnIniFilePath = $"-ini=&quot;{Path.Combine(currentDirectory, "System\\CNN.ini")}&quot;";
            var cnnUserIniFilePath = $"-userini=&quot;{Path.Combine(currentDirectory, "System\\CNNUser.ini")}";
            
            // Default launch values:
            string steamUrl = $"steam://rungameid/{6910}//";
            steamData.steamPath = new StringBuilder()
                .Append(steamUrl)
                .Append(cnnIniFilePath)
                .Append(" ")
                .Append(cnnUserIniFilePath)
                .ToString();
            steamData.steamArgs = string.Empty;

            var htmlRunnerFile = new StringBuilder()
                    .AppendLine("<html>")
                    .AppendLine("<body>")
                    .AppendLine($"<a id=\"link\" target=\"_self\" href=\"{steamData.steamPath}\">Run Deus Ex</a>")
                    .AppendLine("</body>")
                    .AppendLine("<script>")
                    .AppendLine("function LoadGame() {")
                    .AppendLine("document.getElementById(\"link\").click();")
                    .AppendLine("};")
                    .AppendLine("window.onload = LoadGame();")
                    .AppendLine("</script>")
                    .AppendLine("</html>")
                    .ToString();
                       
            string htmlRunnerFilename = Path.Combine(Directory.GetCurrentDirectory(), "PlayCNNSteam.html");
            File.WriteAllText(htmlRunnerFilename, htmlRunnerFile);
            
            try
            {
                Process.Start(htmlRunnerFilename);
                // Process.Start(GetSteamProcess(_args, steamData));
                // Process.Start(steamData.steamPath);
            }
            catch (Exception ignored)
            {
                Console.WriteLine(ignored.Message);
            }
            
            /*
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
                // Process.Start(GetSteamProcess(_args, steamData));
                Process.Start(steamData.steamPath);
            }
            catch (Exception ignored)
            {
                Console.WriteLine(ignored.Message);
            }
            */
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
            return new ProcessStartInfo
            {
                UseShellExecute = true,
                FileName = steamData.steamPath,
            };
        }
    }
}
