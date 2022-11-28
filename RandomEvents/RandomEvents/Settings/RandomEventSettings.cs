using CryingBuffalo.RandomEvents.Events;
using CryingBuffalo.RandomEvents.Events.BicEvents;
using CryingBuffalo.RandomEvents.Events.CCEvents;
using CryingBuffalo.RandomEvents.Settings.MCM;

namespace CryingBuffalo.RandomEvents.Settings
{
    internal class RandomEventSettings
    {
        #region EventData Declarations
        
        
        public BetMoneyData BetMoneyData { get; } = new BetMoneyData("BetMoney", MCM_MenuConfig_Chances.Instance.Bet_Money_Chance);
        
        
        public BumperCropData BumperCropData { get; } = new BumperCropData("BumperCrop", MCM_MenuConfig_Chances.Instance.Bumper_Crop_Chance);
        
        
        public BanditAmbushData BanditAmbushData { get; } = new BanditAmbushData("BanditAmbush", MCM_MenuConfig_Chances.Instance.Bandit_Ambush_Chance);
        
        
        public GranaryRatsData GranaryRatsData { get; } = new GranaryRatsData("GranaryRats", MCM_MenuConfig_Chances.Instance.Granary_Rats_Chance);
        
        
        public TargetPracticeData TargetPracticeData { get; } = new TargetPracticeData("TargetPractice", MCM_MenuConfig_Chances.Instance.Target_Practice_Chance);
        
        
        public PrisonerRebellionData PrisonerRebellionData { get; } = new PrisonerRebellionData("PrisonerRebellion", MCM_MenuConfig_Chances.Instance.Prisoner_Rebellion_Chance);
        
        
        public ChattingCommandersData ChattingCommandersData { get; } = new ChattingCommandersData("ChattingCommanders", MCM_MenuConfig_Chances.Instance.Chatting_Commanders_Chance);

        
        public DiseasedCityData DiseasedCityData { get; } = new DiseasedCityData("DiseasedCity", MCM_MenuConfig_Chances.Instance.Diseased_City_Chance);
        
        
        public MomentumData MomentumData { get; } = new MomentumData("Momentum", MCM_MenuConfig_Chances.Instance.Momentum_Chance);
        
        
        public SecretSingerData SecretSingerData { get; } = new SecretSingerData("SecretSinger", MCM_MenuConfig_Chances.Instance.Secret_Singer_Chance);
        
        
        public BeeKindData BeeKindData { get; } = new BeeKindData("BeeKind", MCM_MenuConfig_Chances.Instance.Bee_Kind_Chance);
        
        
        public FoodFightData FoodFightData { get; } = new FoodFightData("FoodFight", MCM_MenuConfig_Chances.Instance.Food_Fight_Chance);
        
        
        public PerfectWeatherData PerfectWeatherData { get; } = new PerfectWeatherData("PerfectWeather", MCM_MenuConfig_Chances.Instance.Perfect_Weather_Chance);
        
        
        public WanderingLivestockData WanderingLivestockData { get; } = new WanderingLivestockData("WanderingLivestock", MCM_MenuConfig_Chances.Instance.Wandering_Livestock_Chance);
        
        
        public EagerTroopsData EagerTroopsData { get; } = new EagerTroopsData("EagerTroops", MCM_MenuConfig_Chances.Instance.Eager_Troops_Chance);

        
        public SpeedyRecoveryData SpeedyRecoveryData { get; } = new SpeedyRecoveryData("SpeedyRecovery", MCM_MenuConfig_Chances.Instance.Speedy_Recovery_Chance);
        
        
        public FantasticFightersData FantasticFightersData { get; } = new FantasticFightersData("FantasticFighters", MCM_MenuConfig_Chances.Instance.Fantastic_Fighters_Chance);

        
        public ExoticDrinksData ExoticDrinksData { get; } = new ExoticDrinksData("ExoticDrinks", MCM_MenuConfig_Chances.Instance.Exotic_Drinks_Chance);

        
        public AheadOfTimeData AheadOfTimeData { get; } = new AheadOfTimeData("AheadOfTime", MCM_MenuConfig_Chances.Instance.Ahead_Of_Time_Chance);

        
        public SuccessfulDeedsData SuccessfulDeedsData { get; } = new SuccessfulDeedsData("SuccessfulDeeds", MCM_MenuConfig_Chances.Instance.Successful_Deeds_Chance);

        
        public BunchOfPrisonersData BunchOfPrisonersData { get; } = new BunchOfPrisonersData("BunchOfPrisoners", MCM_MenuConfig_Chances.Instance.Bunch_Of_Prisoners_Chance);

        
        public UndercookedData UndercookedData { get; } = new UndercookedData("Undercooked", MCM_MenuConfig_Chances.Instance.Undercooked_Chance);

        
        public LookUpData LookUpData { get; } = new LookUpData("LookUp", MCM_MenuConfig_Chances.Instance.Look_Up_Chance);
        
        
        public HotSpringsData HotSpringsData { get; } = new HotSpringsData("HotSprings", MCM_MenuConfig_Chances.Instance.Hot_Springs_Chance);
        
        
        public SupernaturalEncounterData SupernaturalEncounterData { get; } = new SupernaturalEncounterData("SupernaturalEncounter", MCM_MenuConfig_Chances.Instance.Supernatural_Encounter_Chance);
        
        
        public RunawaySonData RunawaySonData { get; } = new RunawaySonData("RunawaySon", MCM_MenuConfig_Chances.Instance.Runaway_Son_Chance);

        
        public UnexpectedWeddingData UnexpectedWeddingData { get; } = new UnexpectedWeddingData("UnexpectedWedding", MCM_MenuConfig_Chances.Instance.Unexpected_Wedding_Chance);

        
        public ViolatedGirlData ViolatedGirlData { get; } = new ViolatedGirlData("ViolatedGirl", MCM_MenuConfig_Chances.Instance.Violated_Girl_Chance);
        
        
        public FallenSoldierFamilyData FallenSoldierFamilyData { get; } = new FallenSoldierFamilyData("FallenSoldierFamily", MCM_MenuConfig_Chances.Instance.Fallen_Soldier_Family_Chance );

        
        public NotOfThisWorldData NotOfThisWorldData { get; } = new NotOfThisWorldData("NotOfThisWorld", MCM_MenuConfig_Chances.Instance.Not_Of_This_World_Chance );
        
        
        public FishingSpotData FishingSpotData { get; } = new FishingSpotData("FishingSpot", MCM_MenuConfig_Chances.Instance.Fishing_Spot_Chance );
        
        
        public HuntingTripData HuntingTripData { get; } = new HuntingTripData("HuntingTrip", MCM_MenuConfig_Chances.Instance.Hunting_Trip_Chance);
        
        
        public LoggingSiteData LoggingSiteData { get; } = new LoggingSiteData("LoggingSite", MCM_MenuConfig_Chances.Instance.Logging_Site_Chance);
        
        
        public RedMoonData RedMoonData { get; } = new RedMoonData("RedMoon", MCM_MenuConfig_Chances.Instance.Red_Moon_Chance);
        
        
        public PassingCometData PassingCometData { get; } = new PassingCometData("PassingComet", MCM_MenuConfig_Chances.Instance.Passing_Comet_Chance);
        
        
        public MassGraveData MassGraveData { get; } = new MassGraveData("MassGrave", MCM_MenuConfig_Chances.Instance.Mass_Grave_Chance);
        
        
        public BeggarBeggingData BeggarBeggingData { get; } = new BeggarBeggingData("BeggarBegging",MCM_MenuConfig_Chances.Instance.Beggar_Begging_Chance);
        
        
        public BirthdayPartyData BirthdayPartyData { get; } = new BirthdayPartyData("BirthdayParty",MCM_MenuConfig_Chances.Instance.Birthday_Party_Chance);

        
        public OldRuinsData OldRuinsData { get; } = new OldRuinsData("OldRuins", MCM_MenuConfig_Chances.Instance.Old_Ruins_Chance);
        
        
        public AFlirtatiousEncounterData AFlirtatiousEncounterData { get; } = new AFlirtatiousEncounterData("AFlirtatiousEncounter", MCM_MenuConfig_Chances.Instance.A_Flirtatious_Encounter_Chance);
        
        
        public SuddenStormData SuddenStormData { get; } = new SuddenStormData("SuddenStorm",MCM_MenuConfig_Chances.Instance.Sudden_Storm_Chance);

        
        public PrisonerTransferData PrisonerTransferData { get; } = new PrisonerTransferData("PrisonerTransfer", MCM_MenuConfig_Chances.Instance.Prisoner_Transfer_Chance);
        
        
        public LightsInTheSkiesData LightsInTheSkiesData { get; } = new LightsInTheSkiesData("LightsInTheSkies", MCM_MenuConfig_Chances.Instance.Lights_In_the_Skies_Chance);


        public RobberyData RobberyData { get; } = new RobberyData("Robbery", MCM_MenuConfig_Chances.Instance.Robbery_Chance);
        

        public BirdSongsData BirdSongsData { get; } = new BirdSongsData("BirdSongs", MCM_MenuConfig_Chances.Instance.Bird_Songs_Chance);
        
        
        public CourierData CourierData { get; } = new CourierData("Courier", MCM_MenuConfig_Chances.Instance.Courier_Chance);
        
        
        public DysenteryData DysenteryData { get; } = new DysenteryData("Dysentery", MCM_MenuConfig_Chances.Instance.Dysentery_Chance);
        
        
        public RefugeesData RefugeesData { get; } = new RefugeesData("Refugees", MCM_MenuConfig_Chances.Instance.Refugees_Chance);

        
        public BottomsUpData BottomsUpData { get; } = new BottomsUpData("BottomsUp", MCM_MenuConfig_Chances.Instance.Bottoms_Up_Chance);

        
        public TravellingmerchantData TravellingMerchantData { get; } = new TravellingmerchantData("TravellingMerchant", MCM_MenuConfig_Chances.Instance.Travelling_Merchant_Chance);
        
        
        public DreadedSweatsData DreadedSweatsData { get; } = new DreadedSweatsData("DreadedSweats", MCM_MenuConfig_Chances.Instance.Dreaded_Sweats_Chance);

        public ArmyGamesData ArmyGamesData { get; } = new ArmyGamesData("ArmyGames", MCM_MenuConfig_Chances.Instance.Army_Games_Chance);

        public CompanionAdmireData CompanionAdmireData { get; } = new CompanionAdmireData("CompanionAdmire", MCM_MenuConfig_Chances.Instance.Companion_Admire_Chance); 

        public ArmyInviteData ArmyInviteData { get; } = new ArmyInviteData("ArmyInvite", MCM_MenuConfig_Chances.Instance.ArmyInvite_Chance);

        public FeastData FeastData { get; } = new FeastData("Feast", MCM_MenuConfig_Chances.Instance.Feast_Chance);
        #endregion
    }
}