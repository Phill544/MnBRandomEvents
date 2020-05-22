using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Library;

namespace CryingBuffalo.RandomEvents
{
	public static class SaveSystem
	{
		private static string fileLocation;

		private static SaveData Data;

		public static void Init()
		{
			Data = new SaveData();
		}

		public static void SetFilePath()
		{
			fileLocation = BasePath.Name + "Modules/RandomEvents/Saves/SaveData.json";						
			Directory.CreateDirectory(Path.GetDirectoryName(fileLocation));
		}

		public static void LoadData()
		{
			Data.LoadDataFromFile(fileLocation);
		}

		public static void SaveData()
		{
			Data.SaveDataToFile(fileLocation);
		}

		public static void AddData(string id, object obj)
		{
			Data.AddData(id, obj);
		}

		public static void RemoveData(string id)
		{
			Data.RemoveData(id);
		}

		public static void UpdateData(string id, object obj)
		{
			Data.UpdateData(id, obj);
		}

		public static bool TryGetData(string id, out object obj)
		{
			return Data.TryGetData(id, out obj);
		}
	}
}
