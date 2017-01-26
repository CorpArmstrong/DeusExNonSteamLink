namespace DeusExExeNonSteam
{
    class Program
    {
        static void Main(string[] args)
        {
            SteamLauncher launcher = new SteamLauncher(args);
            launcher.LaunchDeusEx();
        }
    }
}
