using CryingBuffalo.RandomEvents.Events;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryingBuffalo.RandomEvents
{
	class RandomEventSettings
	{
		[JsonProperty]
		public BetMoneyData BetMoneyData { get; private set; } = new BetMoneyData(RandomEventType.BetMoney, 1024f, 500);

		[JsonProperty]
		public BumperCropData BumperCropData { get; private set; } = new BumperCropData(RandomEventType.BumperCrop, 1024f, 0.75f);

		[JsonProperty]
		public BanditAmbushData BanditAmbushData { get; private set; } = new BanditAmbushData(RandomEventType.BanditAmbush, 1024, 0.05f, 0.15f, 1000, 60);

		[JsonProperty]
		public GranaryRatsData GranaryRatsData { get; private set; } = new GranaryRatsData(RandomEventType.GranaryRats, 1024, 0.75f);
	}
}
