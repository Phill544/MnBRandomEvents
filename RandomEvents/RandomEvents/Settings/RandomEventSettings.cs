using CryingBuffalo.RandomEvents.Events;
using CryingBuffalo.RandomEvents.Events.CCEvents;
using Newtonsoft.Json;

namespace CryingBuffalo.RandomEvents.Settings
{
    class RandomEventSettings
    {
        [JsonProperty]
        public BetMoneyData BetMoneyData { get; private set; } = new BetMoneyData("BetMoney", 1024f, 0.15f);

        [JsonProperty]
        public BumperCropData BumperCropData { get; private set; } = new BumperCropData("BumperCrop", 1024f, 0.75f);

        [JsonProperty]
        public BanditAmbushData BanditAmbushData { get; private set; } =
            new BanditAmbushData("BanditAmbush", 1024, 0.05f, 0.15f, 50, 100);

        [JsonProperty]
        public GranaryRatsData GranaryRatsData { get; private set; } = new GranaryRatsData("GranaryRats", 1024, 0.75f);

        [JsonProperty]
        public TargetPracticeData TargetPracticeData { get; private set; } =
            new TargetPracticeData("TargetPractice", 1024, 0.5f, 30);

        [JsonProperty]
        public PrisonerRebellionData PrisonerRebellionData { get; private set; } =
            new PrisonerRebellionData("PrisonerRebellion", 1024, 30);

        [JsonProperty]
        public ChattingCommandersData ChattingCommandersData { get; private set; } =
            new ChattingCommandersData("ChattingCommanders", 1024, 30.0f);

        //[JsonProperty]
        //public GloriousFoodData GloriousFoodData { get; private set; } = new GloriousFoodData("GloriousFood", 1024, 5, 25, 5);

        [JsonProperty]
        public DiseasedCityData DiseasedCityData { get; private set; } =
            new DiseasedCityData("DiseasedCity", 1024, 0.5f, 0.75f, 75, 0.2f);

        [JsonProperty] public MomentumData MomentumData { get; private set; } = new MomentumData("Momentum", 1024);

        [JsonProperty]
        public SecretSingerData SecretSingerData { get; private set; } = new SecretSingerData("SecretSinger", 1024, 10);

        [JsonProperty]
        public BeeKindData BeeKindData { get; private set; } = new BeeKindData("BeeKind", 1024, 10, 25, 0.3f);

        [JsonProperty]
        public FoodFightData FoodFightData { get; private set; } = new FoodFightData("FoodFight", 1024, 10, 20, 5);

        [JsonProperty]
        public PerfectWeatherData PerfectWeatherData { get; private set; } =
            new PerfectWeatherData("PerfectWeather", 1024, 5);

        [JsonProperty]
        public WanderingLivestockData WanderingLivestockData { get; private set; } =
            new WanderingLivestockData("WanderingLivestock", 1024, 5, 10);

        [JsonProperty]
        public EagerTroopsData EagerTroopsData { get; private set; } = new EagerTroopsData("EagerTroops", 1024, 5, 35);

        [JsonProperty]
        public SpeedyRecoveryData SpeedyRecoveryData { get; private set; } =
            new SpeedyRecoveryData("SpeedyRecovery", 1024, 2, 20);

        [JsonProperty]
        public FantasticFightersData FantasticFightersData { get; private set; } =
            new FantasticFightersData("FantasticFighters", 1024, 50);

        [JsonProperty]
        public ExoticDrinksData ExoticDrinksData { get; private set; } =
            new ExoticDrinksData("ExoticDrinks", 1024, 5000);

        [JsonProperty]
        public AheadOfTimeData AheadOfTimeData { get; private set; } = new AheadOfTimeData("AheadOfTime", 1024);

        [JsonProperty]
        public SuccessfulDeedsData SuccessfulDeedsData { get; private set; } =
            new SuccessfulDeedsData("SuccessfulDeeds", 1024, 50.0f);

        [JsonProperty]
        public BunchOfPrisonersData BunchOfPrisonersData { get; private set; } =
            new BunchOfPrisonersData("BunchOfPrisoners", 1024, 5, 35);

        [JsonProperty]
        public UndercookedData UndercookedData { get; private set; } = new UndercookedData("Undercooked", 1024, 2, 20);

        [JsonProperty]
        public LookUpData LookUpData { get; private set; } =
            new LookUpData("LookUp", 1024, 0.25f, 0.1f, 5, 75, 250, 5000);

        [JsonProperty]
        public HotSpringsData HotSpringsData { get; private set; } = new HotSpringsData("HotSprings", 1024, 15);
        
        [JsonProperty]
        public SupernaturalEncounterData SupernaturalEncounterData { get; private set; } =
            new SupernaturalEncounterData("SupernaturalEncounter", 1024);
        
        public RunawaySonData RunawaySonData { get; } =
            new RunawaySonData("RunawaySon", 1024, 10, 50);
        
        public UnexpectedWeddingData UnexpectedWeddingData { get; } =
            new UnexpectedWeddingData("UnexpectedWedding", 1024, 10, 100, 10, 50, 200, 15, 50, 200, 1000);
        
        public ViolatedGirlData ViolatedGirlData { get; } =
            new ViolatedGirlData("ViolatedGirl", 1024, 50, 750);
        
        public FallenSoldierFamilyData FallenSoldierFamilyData { get; } =
            new FallenSoldierFamilyData("FallenSoldierFamily", 1024, 500, 2500 );
        
        public NotOfThisWorldData NotOfThisWorldData { get; } =
            new NotOfThisWorldData("NotOfThisWorld", 1024, 5, 15 );
        
        public FishingSpotData FishingSpotData { get; } =
            new FishingSpotData("FishingSpot", 1024, 2, 10, 25, 5, 20 );
        
        public HuntingTripData HuntingTripData { get; } =
            new HuntingTripData("HuntingTrip", 1024, 1, 10, 5, 5, 20, 2, 5 );
        
        public LoggingSiteData LoggingSiteData { get; } =
            new LoggingSiteData("LoggingSite", 1024, 5, 20, 10, 25);
        
        public RedMoonData RedMoonData { get; } =
            new RedMoonData("RedMoon", 1024, 500, 5000);
        
        public PassingCometData PassingCometData { get; } =
            new PassingCometData("PassingComet", 1024);
        
        public MassGraveData MassGraveData { get; } =
            new MassGraveData("MassGrave", 1024, 2, 6, 10, 30);
        
        public BeggarBeggingData BeggarBeggingData { get; } =
            new BeggarBeggingData("BeggarBegging",1024, 0, 50, 5, 10);
    }
}