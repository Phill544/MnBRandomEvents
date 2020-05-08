﻿using CryingBuffalo.RandomEvents.Events;
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
				case RandomEventType.ChattingCommanders:
					return new ChattingCommanders();
				case RandomEventType.GloriousFood:
					return new GloriousFood();
				case RandomEventType.DiseasedCity:
					return new DiseasedCity();
				case RandomEventType.Momentum:
					return new Momentum();
				case RandomEventType.SecretSinger:
					return new SecretSinger();
				case RandomEventType.BeeKind:
					return new BeeKind();
				case RandomEventType.FoodFight:
					return new FoodFight();
				case RandomEventType.PerfectWeather:
					return new PerfectWeather();
				case RandomEventType.WanderingLivestock:
					return new WanderingLivestock();
				case RandomEventType.EagerTroops:
					return new EagerTroops();
				case RandomEventType.SpeedyRecovery:
					return new SpeedyRecovery();
				case RandomEventType.FantasticFighters:
					return new FantasticFighters();
				case RandomEventType.ExoticDrinks:
					return new ExoticDrinks();
				case RandomEventType.AheadOfTime:
					return new AheadOfTime();
				case RandomEventType.SuccessfulDeeds:
					return new SuccessfulDeeds();
				case RandomEventType.Unknown:
				default:
					break;
			}

			InformationManager.DisplayMessage(new InformationMessage($"Unable to create random event of type : {eventType}!", RandomEventsSubmodule.textColor));

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
