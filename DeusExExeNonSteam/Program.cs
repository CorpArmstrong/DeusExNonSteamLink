using System;

namespace DeusExExeNonSteam
{
    class Program
    {
        static void Main(string[] args)
        {
            var launcher = new SteamLauncher(args);
            launcher.LaunchDeusEx();
            // Console.ReadKey();
        }
    }
}
