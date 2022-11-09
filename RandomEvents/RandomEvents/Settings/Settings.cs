using System.IO;
using Newtonsoft.Json;
using TaleWorlds.Library;

namespace CryingBuffalo.RandomEvents.Settings
{
    internal static class ModSettings
    {
        public static RandomEventSettings RandomEvents { get; private set; }

        public static GeneralSettings GeneralSettings { get; private set; }

        public static void LoadRandomEventSettings(bool updateJsonFile = true)
        {
            RandomEvents = new RandomEventSettings();
        }

        public static void LoadGeneralSettings(bool updateJsonFile = true)
        {
            GeneralSettings = new GeneralSettings();
        }
    }
}