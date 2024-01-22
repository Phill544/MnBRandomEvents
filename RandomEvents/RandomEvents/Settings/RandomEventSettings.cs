using Bannerlord.RandomEvents.Events;
using Bannerlord.RandomEvents.Events.BicEvents;
using Bannerlord.RandomEvents.Events.CCEvents;
using Bannerlord.RandomEvents.Events.CommunityEvents;

namespace Bannerlord.RandomEvents.Settings
{
    internal class RandomEventSettings
    {
        #region Phill Events
        
        public AheadOfTimeData AheadOfTimeData { get; } = new AheadOfTimeData("AheadOfTime", 0.5f);

        public BanditAmbushData BanditAmbushData { get; } = new BanditAmbushData("BanditAmbush", 0.5f);

        public BeeKindData BeeKindData { get; } = new BeeKindData("BeeKind", 0.5f);
        
        public BetMoneyData BetMoneyData { get; } = new BetMoneyData("BetMoney", 0.5f);
        
        public BumperCropData BumperCropData { get; } = new BumperCropData("BumperCrop", 0.5f);
        
        public BunchOfPrisonersData BunchOfPrisonersData { get; } = new BunchOfPrisonersData("BunchOfPrisoners", 0.5f);

        public ChattingCommandersData ChattingCommandersData { get; } = new ChattingCommandersData("ChattingCommanders", 0.55f);
        
        public DiseasedCityData DiseasedCityData { get; } = new DiseasedCityData("DiseasedCity", 0.5f);
        
        public EagerTroopsData EagerTroopsData { get; } = new EagerTroopsData("EagerTroops", 0.5f);
        
        public ExoticDrinksData ExoticDrinksData { get; } = new ExoticDrinksData("ExoticDrinks", 0.5f);
        
        public FantasticFightersData FantasticFightersData { get; } = new FantasticFightersData("FantasticFighters", 0.5f);
        
        public FoodFightData FoodFightData { get; } = new FoodFightData("FoodFight", 0.5f);
        
        public GranaryRatsData GranaryRatsData { get; } = new GranaryRatsData("GranaryRats", 0.5f);
        
        public HotSpringsData HotSpringsData { get; } = new HotSpringsData("HotSprings", 0.5f);
        
        public LookUpData LookUpData { get; } = new LookUpData("LookUp", 0.5f);
        
        public MomentumData MomentumData { get; } = new MomentumData("Momentum", 0.5f);
        
        public PerfectWeatherData PerfectWeatherData { get; } = new PerfectWeatherData("PerfectWeather", 0.5f);
        
        public PrisonerRebellionData PrisonerRebellionData { get; } = new PrisonerRebellionData("PrisonerRebellion", 0.5f);

        public SecretSingerData SecretSingerData { get; } = new SecretSingerData("SecretSinger", 0.5f);
        
        public SpeedyRecoveryData SpeedyRecoveryData { get; } = new SpeedyRecoveryData("SpeedyRecovery", 0.5f);
        
        public SuccessfulDeedsData SuccessfulDeedsData { get; } = new SuccessfulDeedsData("SuccessfulDeeds", 0.5f);
        
        public TargetPracticeData TargetPracticeData { get; } = new TargetPracticeData("TargetPractice", 0.5f);
        
        public UndercookedData UndercookedData { get; } = new UndercookedData("Undercooked", 0.5f);
        
        public WanderingLivestockData WanderingLivestockData { get; } = new WanderingLivestockData("WanderingLivestock", 0.5f);

        #endregion
        

        #region CC Events
        
        public AFlirtatiousEncounterData AFlirtatiousEncounterData { get; } = new AFlirtatiousEncounterData("AFlirtatiousEncounter", 0.5f);
        
        public BeggarBeggingData BeggarBeggingData { get; } = new BeggarBeggingData("BeggarBegging", 0.5f);
        
        public BirthdayPartyData BirthdayPartyData { get; } = new BirthdayPartyData("BirthdayParty", 0.5f);
        
        public DuelData DuelData { get; } = new DuelData("Duel", 0.35f);
        
        public FallenSoldierFamilyData FallenSoldierFamilyData { get; } = new FallenSoldierFamilyData("FallenSoldierFamily", 0.4f);
        
        public FishingSpotData FishingSpotData { get; } = new FishingSpotData("FishingSpot", 0.5f);
        
        public FleeingFateData FleeingFateData { get; } = new FleeingFateData("FleeingFate", 0.5f);
        
        public HuntingTripData HuntingTripData { get; } = new HuntingTripData("HuntingTrip", 0.5f);
        
        public LightsInTheSkiesData LightsInTheSkiesData { get; } = new LightsInTheSkiesData("LightsInTheSkies", 0.05f);
        
        public LoggingSiteData LoggingSiteData { get; } = new LoggingSiteData("LoggingSite", 0.5f);
        
        public MassGraveData MassGraveData { get; } = new MassGraveData("MassGrave", 0.5f);
        
        public NotOfThisWorldData NotOfThisWorldData { get; } = new NotOfThisWorldData("NotOfThisWorld", 0.075f);
        
        public OldRuinsData OldRuinsData { get; } = new OldRuinsData("OldRuins", 0.5f);
        
        public PassingCometData PassingCometData { get; } = new PassingCometData("PassingComet", 0.125f);
        
        public RedMoonData RedMoonData { get; } = new RedMoonData("RedMoon", 0.25f);
        
        public RobberyData RobberyData { get; } = new RobberyData("Robbery", 0.5f);
        
        public RunawaySonData RunawaySonData { get; } = new RunawaySonData("RunawaySon", 0.5f);
        
        public SuddenStormData SuddenStormData { get; } = new SuddenStormData("SuddenStorm", 0.5f);
        
        public SupernaturalEncounterData SupernaturalEncounterData { get; } = new SupernaturalEncounterData("SupernaturalEncounter", 0.1f);
        
        public TravellersData TravellersData { get; } = new TravellersData("Travellers", 0.5f);
        
        public UnexpectedWeddingData UnexpectedWeddingData { get; } = new UnexpectedWeddingData("UnexpectedWedding", 0.25f);
        
        public ViolatedGirlData ViolatedGirlData { get; } = new ViolatedGirlData("ViolatedGirl", 0.25f);
        
        #endregion
        

        #region Bickley Events
        
        public ArmyGamesData ArmyGamesData { get; } = new ArmyGamesData("ArmyGames", 0.6f);
        
        public ArmyInviteData ArmyInviteData { get; } = new ArmyInviteData("ArmyInvite", 0.6f);

        public BirdSongsData BirdSongsData { get; } = new BirdSongsData("BirdSongs", 0.52f);
        
        public BottomsUpData BottomsUpData { get; } = new BottomsUpData("BottomsUp", 0.47f);
        
        public CompanionAdmireData CompanionAdmireData { get; } = new CompanionAdmireData("CompanionAdmire", 0.2f); 
        
        public CourierData CourierData { get; } = new CourierData("Courier",0.45f);
        
        public DreadedSweatsData DreadedSweatsData { get; } = new DreadedSweatsData("DreadedSweats", 0.35f);
        
        public DysenteryData DysenteryData { get; } = new DysenteryData("Dysentery", 0.35f);
        
        public FeastData FeastData { get; } = new FeastData("Feast", 0.6f);
        
        public RefugeesData RefugeesData { get; } = new RefugeesData("Refugees", 0.3f);
        
        public TravellingmerchantData TravellingMerchantData { get; } = new TravellingmerchantData("TravellingMerchant", 0.3f);
        
        #endregion

        
        #region Community Suggested Events

        public SecretsOfSteelData SecretsOfSteelData { get; } = new SecretsOfSteelData("SecretsOfSteel", 0.35f);
        
        public PoisonedWineData PoisonedWineData { get; } = new PoisonedWineData("PoisonedWine", 0.4f);

        #endregion
        
    }
}