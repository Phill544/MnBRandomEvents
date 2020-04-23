using CryingBuffalo.RandomEvents;
using Newtonsoft.Json;
using System.IO;
using TaleWorlds.Library;

namespace CryingBuffalo.RandomEvents
{
	class Settings
	{
		public static RandomEventSettings RandomEvents { get; set; }

		public static GeneralSettings GeneralSettings { get; set; }

		public static void LoadRandomEventSettings()
		{
			string path = BasePath.Name + "Modules/RandomEvents/ModuleData/RandomEventSettings.json";
			if (!File.Exists(path))
			{
				string defaultSettingsText = JsonConvert.SerializeObject(new RandomEventSettings(), Formatting.Indented);
				File.WriteAllText(path, defaultSettingsText);
			}
			Settings.RandomEvents = JsonConvert.DeserializeObject<RandomEventSettings>(File.ReadAllText(path));
		}

		public static void LoadGeneralSettings()
		{
			string path = BasePath.Name + "Modules/RandomEvents/ModuleData/GeneralSettings.json";
			if (!File.Exists(path))
			{
				string defaultSettingsText = JsonConvert.SerializeObject(new GeneralSettings(), Formatting.Indented);
				File.WriteAllText(path, defaultSettingsText);
			}
			Settings.GeneralSettings = JsonConvert.DeserializeObject<GeneralSettings>(File.ReadAllText(path));
		}
	}
}
