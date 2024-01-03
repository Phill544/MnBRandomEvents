using System;

namespace Bannerlord.RandomEvents.Helpers
{
    public static class DefaultIni
    {
        public static string Content()
        {
            var timestamp = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            var year = DateTime.Now.ToString("yyyy");

            return @$"
; WARNING - REMOVAL OF THIS FILE WILL RESULT IN A DEFAULT INI FILE BEING CREATED AND IT WILL CONTAIN DEFAULT VALUES
;
; This INI was generated on {timestamp}
;
; © Random Events - {year}
;
; Do not set any value to 0 as that will result in the event being unable to run.
;
; Events can be disabled here also.


; #######################
; ### General Setting ###
; #######################
; Recommend not to mess with these unless you know what you are doing.

[GeneralSettings]
MinimumInGameHours = 24
MinimumRealMinutes = 5
MaximumRealMinutes = 30
LevelXpMultiplier = 40
DisableSupernatural = false
DisableSkillChecks = false
DistinctEventCycleLength = 5
MaxQueueDisplayCount = 5
MaxEventHistoryCount = 10



; #################
; ### BicEvents ###
; #################

[ArmyGames]
EventDisabled = false
MinCohesionIncrease = 10.0
MaxCohesionIncrease = 40.0
MinMoraleGain = 10
MaxMoraleGain = 30

[ArmyInvite]
EventDisabled = false

[BirdSongs]
EventDisabled = false
MinMoraleGain = 15
MaxMoraleGain = 30

[BottomsUp]
EventDisabled = false
MinMoraleGain = 10
MaxMoraleGain = 25
MinGold = 50
MaxGold = 300

[CompanionAdmire]
EventDisabled = false

[Courier]
EventDisabled = false
MinMoraleGain = 15
MaxMoraleGain = 30

[DreadedSweats]
EventDisabled = false
MinMoraleLoss = 10
MaxMoraleLoss = 25
MinVictims = 3
MaxVictims = 6

[Dysentery]
EventDisabled = false
MinMoraleLoss = 10
MaxMoraleLoss = 25
MinVictims = 3
MaxVictims = 6

[Feast]
EventDisabled = false

[Refugees]
EventDisabled = false
MinSoldiers = 8
MaxSoldiers = 15
MinFood = 3
MaxFood = 5
MinCaptive = 8
MaxCaptive = 15

[TravellingMerchant]
EventDisabled = false
MinLoot = 2500
MaxLoot = 10000



; ################
; ### CCEvents ###
; ################

[AFlirtatiousEncounter]
EventDisabled = false
MinWomanAge = 20
MaxWomanAge = 45
MinRelationshipIncrease = 5.0
MaxRelationshipIncrease = 10.0
MinCharmLevel = 100

[BeggarBegging]
EventDisabled = false
MinStewardLevel = 50
MaxStewardLevel = 125

[BirthdayParty]
EventDisabled = false
MinAttending = 15
MaxAttending = 60
MinYourMenAttending = 5
MaxYourMenAttending = 15
MinAge = 17
MaxAge = 23
MinBandits = 5
MaxBandits = 15
MinGoldGiven = 100
MaxGoldGiven = 750
MinInfluenceGain = 20
MaxInfluenceGain = 75
MinGoldLooted = 25
MaxGoldLooted = 250
MinRogueryLevel = 125

[Duel]
EventDisabled = false
MinTwoHandedLevel = 125
MinRogueryLevel = 100

[FallenSoldierFamily]
EventDisabled = false
MinFamilyCompensation = 750
MaxFamilyCompensation = 1750
MinGoldLooted = 1000
MaxGoldLooted = 2000
MinRogueryLevel = 125

[FishingSpot]
EventDisabled = false
MinSoldiersToGo = 3
MaxSoldiersToGo = 12
MaxFishCatch = 20
MinMoraleGain = 5
MaxMoraleGain = 25

[FleeingFate]
EventDisabled = false
MinGoldReward = 950
MaxGoldReward = 3500
MinAge = 16
MaxAge = 30
MinStewardLevel = 100
MinRogueryLevel = 125
SuccessChance = 65

[HuntingTrip]
EventDisabled = false
MinSoldiersToGo = 3
MaxSoldiersToGo = 12
MaxCatch = 20
MinMoraleGain = 7
MaxMoraleGain = 20
MinYieldMultiplier = 3
MaxYieldMultiplier = 6

[LightsInTheSkies]
EventDisabled = false

[LoggingSite]
EventDisabled = false
MinSoldiersToGo = 10
MaxSoldiersToGo = 20
MinYield = 5
MaxYield = 15
MinYieldMultiplier = 10
MaxYieldMultiplier = 15

[MassGrave]
EventDisabled = false
MinSoldiers = 3
MaxSoldiers = 8
MinBodies = 20
MaxBodies = 40
MinBaseMoraleLoss = 15
MaxBaseMoraleLoss = 25

[NotOfThisWorld]
EventDisabled = false
MinSoldiersToDisappear = 3
MaxSoldiersToDisappear = 8

[OldRuins]
EventDisabled = false
MinMen = 6
MaxMen = 12
MaxMenToKill = 10
MinGoldFound = 250
MaxGoldFound = 5000

[PassingComet]
EventDisabled = false

[PrisonerTransfer]
EventDisabled = false
MinPrisoners = 10
MaxPrisoners = 50
MinPricePrPrisoner = 50
MaxPricePrPrisoner = 400

[RedMoon]
EventDisabled = false
MinGoldLost = 700
MaxGoldLost = 4000
MinMenLost = 15
MaxMenLost = 50

[Robbery]
EventDisabled = false
MinGoldLost = 500
MaxGoldLost = 5000
MinRoguerySkill = 125
MinCharmSkill = 175
MinOneHandedSkill = 150
MinTwoHandedSkill = 150

[RunawaySon]
EventDisabled = false
MinGold = 50
MaxGold = 150
MinRogueryLevel = 125

[SuddenStorm]
EventDisabled = false
MinHorsesLost = 1
MaxHorsesLost = 3
MinMenDied = 1
MaxMenDied = 4
MinMenWounded = 3
MaxMenWounded = 7
MinMeatFromHorse = 4
MaxMeatFromHorse = 9

[SupernaturalEncounter]
EventDisabled = false

[Travellers]
EventDisabled = false
MinGoldStolen = 2500
MaxGoldStolen = 10000
MinEngineeringLevel = 75
MinRogueryLevel = 125
MinStewardLevel = 100

[UnexpectedWedding]
EventDisabled = false
MinGoldToDonate = 100
MaxGoldToDonate = 750
MinPeopleInWedding = 10
MaxPeopleInWedding = 60
EmbarrassedSoliderMaxGold = 100
MinGoldRaided = 250
MaxGoldRaided = 1500
MinRogueryLevel = 125

[ViolatedGirl]
EventDisabled = false
MinGoldCompensation = 2000
MaxGoldCompensation = 5000
MinRogueryLevel = 125



; #######################
; ### CommunityEvents ###
; #######################

[PoisonedWine]
EventDisabled = false
MinSoldiersToDie = 1
MaxSoldiersToDie = 2
MinSoldiersToHurt = 2
MaxSoldiersToHurt = 5

[SecretsOfSteel]
EventDisabled = false



; ####################
; ### Other Events ###
; ####################

[AheadOfTime]
EventDisabled = false

[BanditAmbush]
EventDisabled = false
MoneyMinPercent = 0.05
MoneyMaxPercent = 0.15
TroopScareCount = 50
BanditCap = 50

[BeeKind]
EventDisabled = false
Damage = 10
ReactionChance = 0.15
ReactionDamage = 15

[BetMoney]
EventDisabled = false
MoneyBetPercent = 0.1

[BumperCrop]
EventDisabled = false
CropGainPercent = 0.66

[BunchOfPrisoners]
EventDisabled = false
MinPrisonerGain = 15
MaxPrisonerGain = 50

[ChattingCommanders]
EventDisabled = false
CohesionIncrease = 30.0

[DiseasedCity]
EventDisabled = false
BaseSuccessChance = 0.5
HighMedicineChance = 0.25
HighMedicineLevel = 75
PercentLoss = 0.2

[EagerTroops]
EventDisabled = false
MinTroopGain = 10
MaxTroopGain = 35

[ExoticDrinks]
EventDisabled = false
MinPrice = 3000
MaxPrice = 6000
SuccessChance = 0.75
MinXp = 250
MaxXp = 1000

[FantasticFighters]
EventDisabled = false
MinRenownGain = 25
MaxRenownGain = 75

[FoodFight]
EventDisabled = false
MinFoodLoss = 5
MaxFoodLoss = 30
MinMoraleLoss = 5
MaxMoraleLoss = 20

[GranaryRats]
EventDisabled = false
MinFoodLossPercent = 0.1
MaxFoodLossPercent = 0.25

[HotSprings]
EventDisabled = false
MinMoraleGain = 10
MaxMoraleGain = 25

[LookUp]
EventDisabled = false
TreeShakeChance = 0.25
BaseRangeChance = 0.1
MinRangeLevel = 10
MaxRangeLevel = 60
MinGold = 500
MaxGold = 2500

[Momentum]
EventDisabled = false

[PerfectWeather]
EventDisabled = false
MinMoraleGain = 10
MaxMoraleGain = 25

[PrisonerRebellion]
EventDisabled = false
MinimumPrisoners = 30

[SecretSinger]
EventDisabled = false
MinMoraleGain = 10
MaxMoraleGain = 75

[SpeedyRecovery]
EventDisabled = false
MinTroopsToHeal = 5
MaxTroopsToHeal = 25

[SuccessfulDeeds]
EventDisabled = false
MinInfluenceGain = 20
MaxInfluenceGain = 100

[TargetPractice]
EventDisabled = false
MinimumSoldiers = 50
PercentageDifferenceOfCurrentTroop = 0.5

[Undercooked]
EventDisabled = false
MinTroopsToInjure = 8
MaxTroopsToInjure = 30

[WanderingLivestock]
EventDisabled = false
MinFood = 10
MaxFood = 20
";
        }
    }
}