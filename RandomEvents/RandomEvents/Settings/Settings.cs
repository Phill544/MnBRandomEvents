namespace CryingBuffalo.RandomEvents.Settings
{
    internal static class ModSettings
    {
        public static RandomEventSettings RandomEvents { get; private set; }

        public static void LoadRandomEventSettings(bool updateJsonFile = true)
        {
            RandomEvents = new RandomEventSettings();
        }
    }
}
