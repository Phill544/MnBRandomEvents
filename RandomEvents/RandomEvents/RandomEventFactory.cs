using CryingBuffalo.RandomEvents.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;

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
				case RandomEventType.BanditAmbush:
					return new BanditAmbush();
				case RandomEventType.GranaryRats:
					return new GranaryRats();
				case RandomEventType.TargetPractice:
					return new TargetPractice();
				case RandomEventType.PrisonerRebellion:
					return new PrisonerRebellion();
				case RandomEventType.Unknown:
				default:
					break;
			}

			InformationManager.DisplayMessage(new InformationMessage($"Unable to create random event of type : {eventType}!", RandomEventsSubmodule.Instance.textColor));

			return null;
		}

		public static BaseEvent CreateEvent(string eventName)
		{
			RandomEventType newType = RandomEventType.Unknown;
			Enum.TryParse(eventName, true, out newType);

			return CreateEvent(newType);
		}
	}
}
