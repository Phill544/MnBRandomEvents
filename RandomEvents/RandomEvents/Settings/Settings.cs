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
			string path = BasePath.Name + "Modules/RandomEvents/ModuleData/RandomEventSettings.json";
			if (!File.Exists(path))
			{
				string defaultSettingsText = JsonConvert.SerializeObject(new RandomEventSettings(), Formatting.Indented);
				File.WriteAllText(path, defaultSettingsText);
			}
			RandomEvents = JsonConvert.DeserializeObject<RandomEventSettings>(File.ReadAllText(path));

			if (!updateJsonFile) return;
			string updatedSettingsText = JsonConvert.SerializeObject(RandomEvents, Formatting.Indented);
			File.WriteAllText(path, updatedSettingsText);
		}

		public static void LoadGeneralSettings(bool updateJsonFile = true)
		{
			string path = BasePath.Name + "Modules/RandomEvents/ModuleData/GeneralSettings.json";
			if (!File.Exists(path))
			{
				string defaultSettingsText = JsonConvert.SerializeObject(new GeneralSettings(), Formatting.Indented);
				File.WriteAllText(path, defaultSettingsText);
			}
			GeneralSettings = JsonConvert.DeserializeObject<GeneralSettings>(File.ReadAllText(path));

			if (updateJsonFile)
			{
				string updatedSettingsText = JsonConvert.SerializeObject(GeneralSettings, Formatting.Indented);
				File.WriteAllText(path, updatedSettingsText);
			}
		}
	}
}
