using CryingBuffalo.RandomEvents.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CryingBuffalo.RandomEvents
{
	public static class RandomEventFactory
	{
		public static BaseEvent CreateEvent(RandomEventType eventType)
		{
			switch (eventType)
			{
				case RandomEventType.BetMoney:
					return new BetMoney();
				case RandomEventType.BumperCrop:
					return new BumperCrop();
				case RandomEventType.Unknown:
				default:
					break;
			}

			MessageBox.Show($"Unable to create random event of type : {nameof(eventType)}");

			return null;
		}

		public static BaseEvent CreateEvent(string eventName)
		{
			return CreateEvent((RandomEventType)Enum.Parse(typeof(RandomEventType), eventName, true));
		}
	}
}
