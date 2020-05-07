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
		public BetMoneyData BetMoneyData { get; private set; } = new BetMoneyData(RandomEventType.BetMoney, 1024f, 0.15f);

		[JsonProperty]
		public BumperCropData BumperCropData { get; private set; } = new BumperCropData(RandomEventType.BumperCrop, 1024f, 0.75f);

		[JsonProperty]
		public BanditAmbushData BanditAmbushData { get; private set; } = new BanditAmbushData(RandomEventType.BanditAmbush, 1024, 0.05f, 0.15f, 1000, 60);

		[JsonProperty]
		public GranaryRatsData GranaryRatsData { get; private set; } = new GranaryRatsData(RandomEventType.GranaryRats, 1024, 0.75f);

		[JsonProperty]
		public TargetPracticeData TargetPracticeData { get; private set; } = new TargetPracticeData(RandomEventType.TargetPractice, 1024, 0.5f, 30);

		[JsonProperty]
		public PrisonerRebellionData PrisonerRebellionData { get; private set; } = new PrisonerRebellionData(RandomEventType.PrisonerRebellion, 1024, 30);

		[JsonProperty]
		public ChattingCommandersData ChattingCommandersData { get; private set; } = new ChattingCommandersData(RandomEventType.ChattingCommanders, 1024, 30.0f);

		[JsonProperty]
		public GloriousFoodData GloriousFoodData { get; private set; } = new GloriousFoodData(RandomEventType.GloriousFood, 1024, 5, 25, 5);

		[JsonProperty]
		public DiseasedCityData DiseasedCityData { get; private set; } = new DiseasedCityData(RandomEventType.DiseasedCity, 1024, 0.5f, 0.75f, 75, 0.2f);

		[JsonProperty]
		public MomentumData MomentumData { get; private set; } = new MomentumData(RandomEventType.Momentum, 1024);

		[JsonProperty]
		public SecretSingerData SecretSingerData { get; private set; } = new SecretSingerData(RandomEventType.SecretSinger, 1024, 10);

		[JsonProperty]
		public BeeKindData BeeKindData { get; private set; } = new BeeKindData(RandomEventType.BeeKind, 1024, 10, 25, 0.3f);

		[JsonProperty]
		public FoodFightData FoodFightData { get; private set; } = new FoodFightData(RandomEventType.FoodFight, 1024, 10, 20, 5);

		[JsonProperty]
		public PerfectWeatherData PerfectWeatherData { get; private set; } = new PerfectWeatherData(RandomEventType.PerfectWeather, 1024, 5);

		[JsonProperty]
		public WanderingLivestockData WanderingLivestockData { get; private set; } = new WanderingLivestockData(RandomEventType.WanderingLivestock, 1024, 5, 10);

		[JsonProperty]
		public EagerTroopsData EagerTroopsData { get; private set; } = new EagerTroopsData(RandomEventType.EagerTroops, 1024, 5, 35);

		[JsonProperty]
		public SpeedyRecoveryData SpeedyRecoveryData { get; private set; } = new SpeedyRecoveryData(RandomEventType.SpeedyRecovery, 1024, 2, 20);
	}
}
