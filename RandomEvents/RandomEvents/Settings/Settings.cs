using CryingBuffalo.RandomEvents;
using Newtonsoft.Json;
using System.IO;
using TaleWorlds.Library;

namespace CryingBuffalo.RandomEvents
{
	class Settings
	{
		public static RandomEventSettings RandomEvents { get; set; }

		public static void Load()
		{
			string path = BasePath.Name + "Modules/RandomEvents/ModuleData/RandomEventSettings.json";
			if (!File.Exists(path))
			{
				string defaultSettingsText = JsonConvert.SerializeObject(new RandomEventSettings(), Formatting.Indented);
				File.WriteAllText(path, defaultSettingsText);
			}
			Settings.RandomEvents = JsonConvert.DeserializeObject<RandomEventSettings>(File.ReadAllText(path));
		}
	}
}
