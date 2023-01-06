using CryingBuffalo.RandomEvents.Events;
using CryingBuffalo.RandomEvents.Events.BicEvents;
using CryingBuffalo.RandomEvents.Events.CCEvents;
using CryingBuffalo.RandomEvents.Events.CommunityEvents;
using CryingBuffalo.RandomEvents.Settings.MCM;

namespace CryingBuffalo.RandomEvents.Settings
{
    internal class RandomEventSettings
    {
        #region Phill Events
        
        public AheadOfTimeData AheadOfTimeData { get; } = new AheadOfTimeData("AheadOfTime", 50.0f);

        public BanditAmbushData BanditAmbushData { get; } = new BanditAmbushData("BanditAmbush", 50.0f);

        public BeeKindData BeeKindData { get; } = new BeeKindData("BeeKind", 50.0f);
        
        public BetMoneyData BetMoneyData { get; } = new BetMoneyData("BetMoney", 50.0f);
        
        public BumperCropData BumperCropData { get; } = new BumperCropData("BumperCrop", 50.0f);
        
        public BunchOfPrisonersData BunchOfPrisonersData { get; } = new BunchOfPrisonersData("BunchOfPrisoners", 50.0f);

        public ChattingCommandersData ChattingCommandersData { get; } = new ChattingCommandersData("ChattingCommanders", 60.0f);
        
        public DiseasedCityData DiseasedCityData { get; } = new DiseasedCityData("DiseasedCity", 50.0f);
        
        public EagerTroopsData EagerTroopsData { get; } = new EagerTroopsData("EagerTroops", 50.0f);
        
        public ExoticDrinksData ExoticDrinksData { get; } = new ExoticDrinksData("ExoticDrinks", 50.0f);
        
        public FantasticFightersData FantasticFightersData { get; } = new FantasticFightersData("FantasticFighters", 50.0f);
        
        public FoodFightData FoodFightData { get; } = new FoodFightData("FoodFight", 50.0f);
        
        public GranaryRatsData GranaryRatsData { get; } = new GranaryRatsData("GranaryRats", 50.0f);
        
        public HotSpringsData HotSpringsData { get; } = new HotSpringsData("HotSprings", 50.0f);
        
        public LookUpData LookUpData { get; } = new LookUpData("LookUp", 50.0f);
        
        public MomentumData MomentumData { get; } = new MomentumData("Momentum", 50.0f);
        
        public PerfectWeatherData PerfectWeatherData { get; } = new PerfectWeatherData("PerfectWeather", 50.0f);
        
        public PrisonerRebellionData PrisonerRebellionData { get; } = new PrisonerRebellionData("PrisonerRebellion", 50.0f);

        public SecretSingerData SecretSingerData { get; } = new SecretSingerData("SecretSinger", 50.0f);
        
        public SpeedyRecoveryData SpeedyRecoveryData { get; } = new SpeedyRecoveryData("SpeedyRecovery", 50.0f);
        
        public SuccessfulDeedsData SuccessfulDeedsData { get; } = new SuccessfulDeedsData("SuccessfulDeeds", 50.0f);
        
        public TargetPracticeData TargetPracticeData { get; } = new TargetPracticeData("TargetPractice", 50.0f);
        
        public UndercookedData UndercookedData { get; } = new UndercookedData("Undercooked", 50.0f);
        
        public WanderingLivestockData WanderingLivestockData { get; } = new WanderingLivestockData("WanderingLivestock", 50.0f);

        #endregion
        

        #region CC Events
        
        public AFlirtatiousEncounterData AFlirtatiousEncounterData { get; } = new AFlirtatiousEncounterData("AFlirtatiousEncounter", 60.0f);
        
        public BeggarBeggingData BeggarBeggingData { get; } = new BeggarBeggingData("BeggarBegging", 60.0f);
        
        public BirthdayPartyData BirthdayPartyData { get; } = new BirthdayPartyData("BirthdayParty", 50.0f);
        
        public DuelData DuelData { get; } = new DuelData("Duel", 35.0f);
        
        public FallenSoldierFamilyData FallenSoldierFamilyData { get; } = new FallenSoldierFamilyData("FallenSoldierFamily", 60.0f);
        
        public FishingSpotData FishingSpotData { get; } = new FishingSpotData("FishingSpot", 50.0f);
        
        public HuntingTripData HuntingTripData { get; } = new HuntingTripData("HuntingTrip", 50.0f);
        
        public LightsInTheSkiesData LightsInTheSkiesData { get; } = new LightsInTheSkiesData("LightsInTheSkies", 10.0f);
        
        public LoggingSiteData LoggingSiteData { get; } = new LoggingSiteData("LoggingSite", 50.0f);
        
        public MassGraveData MassGraveData { get; } = new MassGraveData("MassGrave", 50.0f);
        
        public NotOfThisWorldData NotOfThisWorldData { get; } = new NotOfThisWorldData("NotOfThisWorld", 10.0f);
        
        public OldRuinsData OldRuinsData { get; } = new OldRuinsData("OldRuins", 50.0f);
        
        public PassingCometData PassingCometData { get; } = new PassingCometData("PassingComet", 15.0f);
        
        public PrisonerTransferData PrisonerTransferData { get; } = new PrisonerTransferData("PrisonerTransfer", 50.0f);
        
        public RedMoonData RedMoonData { get; } = new RedMoonData("RedMoon", 25.0f);
        
        public RobberyData RobberyData { get; } = new RobberyData("Robbery", 60.0f);
        
        public RunawaySonData RunawaySonData { get; } = new RunawaySonData("RunawaySon", 50.0f);
        
        public SuddenStormData SuddenStormData { get; } = new SuddenStormData("SuddenStorm", 50.0f);
        
        public SupernaturalEncounterData SupernaturalEncounterData { get; } = new SupernaturalEncounterData("SupernaturalEncounter", 10.0f);
        
        public TravellersData TravellersData { get; } = new TravellersData("Travellers", 50.0f);
        
        public UnexpectedWeddingData UnexpectedWeddingData { get; } = new UnexpectedWeddingData("UnexpectedWedding", 50.0f);
        
        public ViolatedGirlData ViolatedGirlData { get; } = new ViolatedGirlData("ViolatedGirl", 50.0f);
        
        #endregion
        

        #region Bickley Events
        
        public ArmyGamesData ArmyGamesData { get; } = new ArmyGamesData("ArmyGames", 60.0f);
        
        public ArmyInviteData ArmyInviteData { get; } = new ArmyInviteData("ArmyInvite", 60.0f);

        public BirdSongsData BirdSongsData { get; } = new BirdSongsData("BirdSongs", 50.0f);
        
        public BottomsUpData BottomsUpData { get; } = new BottomsUpData("BottomsUp", 50.0f);
        
        public CompanionAdmireData CompanionAdmireData { get; } = new CompanionAdmireData("CompanionAdmire", 20.0f); 
        
        public CourierData CourierData { get; } = new CourierData("Courier", 50.0f);
        
        public DreadedSweatsData DreadedSweatsData { get; } = new DreadedSweatsData("DreadedSweats", 35.0f);
        
        public DysenteryData DysenteryData { get; } = new DysenteryData("Dysentery", 35.0f);
        
        public FeastData FeastData { get; } = new FeastData("Feast", 60.0f);
        
        public RefugeesData RefugeesData { get; } = new RefugeesData("Refugees", 30.0f);
        
        public TravellingmerchantData TravellingMerchantData { get; } = new TravellingmerchantData("TravellingMerchant", 30.0f);
        
        #endregion

        
        #region Community Suggested Events

        public SecretsOfSteelData SecretsOfSteelData { get; } = new SecretsOfSteelData("SecretsOfSteel", 35.0f);
        
        public PoisonedWineData PoisonedWineData { get; } = new PoisonedWineData("PoisonedWine", 40.0f);

        #endregion
        
    }
}