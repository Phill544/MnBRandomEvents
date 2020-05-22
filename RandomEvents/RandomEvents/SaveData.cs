using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CryingBuffalo.RandomEvents
{
	public class SaveData
	{
		public Dictionary<string, object> Data = new Dictionary<string, object>();

		public SaveData()
		{
		}

		public void AddData(string id, object obj)
		{
			if (!obj.GetType().IsSerializable)
			{
				MessageBox.Show($"The object with ID {id} is not marked as Serializable! Unable to save.");
				return;
			}

			if (Data.ContainsKey(id))
			{
				MessageBox.Show($"Trying to save data with ID {id}, however the ID is already being used!");
				return;
			}
			Data.Add(id, obj);
		}

		public void RemoveData(string id)
		{
			if (Data.ContainsKey(id))
			{
				Data.Remove(id);
			}
		}

		public void UpdateData(string id, object obj)
		{
			if (!obj.GetType().IsSerializable)
			{
				MessageBox.Show($"The object with ID {id} is not marked as Serializable! Unable to update.");
				return;
			}

			if (!Data.ContainsKey(id))
			{
				MessageBox.Show($"Trying to update data with ID {id}, however the ID doesn't exist!");
				return;
			}
			Data[id] = obj;
		}

		public bool TryGetData(string id, out object obj)
		{
			if (!Data.ContainsKey(id))
			{
				obj = null;
				return false;
			}
			obj = Data[id];
			return true;
		}

		public bool SaveDataToFile(string path)
		{
			try
			{
				List<KeyValuePair<string, object>> DataList = Data.ToList();

				string updatedSettingsText = JsonConvert.SerializeObject(DataList, Formatting.Indented);
				File.WriteAllText(path, updatedSettingsText);

			}
			catch (Exception ex)
			{
				MessageBox.Show($"Unable to save Random Event Data to file! \n\n Error: {ex.Message} \n\n Trace: {ex.StackTrace}");
				return false;
			}

			return true;
		}

		public bool LoadDataFromFile(string path)
		{
			try
			{
				List<KeyValuePair<string, object>> DataList = new List<KeyValuePair<string, object>>();
				DataList = JsonConvert.DeserializeObject<List<KeyValuePair<string, object>>>(File.ReadAllText(path));

				foreach (var kvp in DataList)
				{
					Data.Add(kvp.Key, kvp.Value);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Unable to load Random Event Data to file! \n\n Error: {ex.Message} \n\n Trace: {ex.StackTrace}");
				return false;
			}

			return true;
		}
	}
}
