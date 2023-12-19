namespace Bannerlord.RandomEvents.Settings
{
    internal static class ModSettings
    {
        public static RandomEventSettings RandomEvents { get; private set; }

        public static void LoadRandomEventSettings()
        {
            RandomEvents = new RandomEventSettings();
        }
    }
}
