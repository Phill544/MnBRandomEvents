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
		public BetMoneyData BetMoneyData { get; private set; } = new BetMoneyData("BetMoney", 1024f, 0.15f);

		[JsonProperty]
		public BumperCropData BumperCropData { get; private set; } = new BumperCropData("BumperCrop", 1024f, 0.75f);

		[JsonProperty]
		public BanditAmbushData BanditAmbushData { get; private set; } = new BanditAmbushData("BanditAmbush", 1024, 0.05f, 0.15f, 1000, 60, 100);

		[JsonProperty]
		public GranaryRatsData GranaryRatsData { get; private set; } = new GranaryRatsData("GranaryRats", 1024, 0.75f);

		[JsonProperty]
		public TargetPracticeData TargetPracticeData { get; private set; } = new TargetPracticeData("TargetPractice", 1024, 0.5f, 30);

		[JsonProperty]
		public PrisonerRebellionData PrisonerRebellionData { get; private set; } = new PrisonerRebellionData("PrisonerRebellion", 1024, 30);

		[JsonProperty]
		public ChattingCommandersData ChattingCommandersData { get; private set; } = new ChattingCommandersData("ChattingCommanders", 1024, 30.0f);

		//[JsonProperty]
		//public GloriousFoodData GloriousFoodData { get; private set; } = new GloriousFoodData("GloriousFood", 1024, 5, 25, 5);

		[JsonProperty]
		public DiseasedCityData DiseasedCityData { get; private set; } = new DiseasedCityData("DiseasedCity", 1024, 0.5f, 0.75f, 75, 0.2f);

		[JsonProperty]
		public MomentumData MomentumData { get; private set; } = new MomentumData("Momentum", 1024);

		[JsonProperty]
		public SecretSingerData SecretSingerData { get; private set; } = new SecretSingerData("SecretSinger", 1024, 10);

		[JsonProperty]
		public BeeKindData BeeKindData { get; private set; } = new BeeKindData("BeeKind", 1024, 10, 25, 0.3f);

		[JsonProperty]
		public FoodFightData FoodFightData { get; private set; } = new FoodFightData("FoodFight", 1024, 10, 20, 5);

		[JsonProperty]
		public PerfectWeatherData PerfectWeatherData { get; private set; } = new PerfectWeatherData("PerfectWeather", 1024, 5);

		[JsonProperty]
		public WanderingLivestockData WanderingLivestockData { get; private set; } = new WanderingLivestockData("WanderingLivestock", 1024, 5, 10);

		[JsonProperty]
		public EagerTroopsData EagerTroopsData { get; private set; } = new EagerTroopsData("EagerTroops", 1024, 5, 35);

		[JsonProperty]
		public SpeedyRecoveryData SpeedyRecoveryData { get; private set; } = new SpeedyRecoveryData("SpeedyRecovery", 1024, 2, 20);

		[JsonProperty]
		public FantasticFightersData FantasticFightersData { get; private set; } = new FantasticFightersData("FantasticFighters", 1024, 50);

		[JsonProperty]
		public ExoticDrinksData ExoticDrinksData { get; private set; } = new ExoticDrinksData("ExoticDrinks", 1024, 5000);

		[JsonProperty]
		public AheadOfTimeData AheadOfTimeData { get; private set; } = new AheadOfTimeData("AheadOfTime", 1024);

		[JsonProperty]
		public SuccessfulDeedsData SuccessfulDeedsData { get; private set; } = new SuccessfulDeedsData("SuccessfulDeeds", 1024, 50.0f);

		[JsonProperty]
		public BunchOfPrisonersData BunchOfPrisonersData { get; private set; } = new BunchOfPrisonersData("BunchOfPrisoners", 1024, 5, 35);

		[JsonProperty]
		public UndercookedData UndercookedData { get; private set; } = new UndercookedData("Undercooked", 1024, 2, 20);

		[JsonProperty]
		public LookUpData LookUpData { get; private set; } = new LookUpData("LookUp", 1024, 0.25f, 0.1f, 5, 75, 250, 5000);
	}
}
