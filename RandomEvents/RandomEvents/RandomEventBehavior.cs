using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bannerlord.RandomEvents.Events;
using Bannerlord.RandomEvents.Helpers;
using Bannerlord.RandomEvents.Settings;
using Ini.Net;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace Bannerlord.RandomEvents
{
	public class RandomEventBehavior : CampaignBehaviorBase
	{
        private static RandomEventBehavior Instance { get; set; }

        private readonly RandomEventGenerator randomEventGenerator;

        public RandomEventBehavior()
        {
            Instance = this;

            randomEventGenerator = new RandomEventGenerator();
            PopulateRandomEventGenerator();
        }

        public override void RegisterEvents()
		{
            CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, OnSessionLaunched);
            CampaignEvents.HourlyTickEvent.AddNonSerializedListener(this, ProcessRandomEvent);
        }

		public override void SyncData(IDataStore dataStore)
		{
		}


        [CommandLineFunctionality.CommandLineArgumentFunction("run", "randomevent")]
        public static string RunRandomEvent(List<string> args)
        {
            if (args.Count < 1)
            {
                return "You must provide the type of event to run";
            }

            if (Instance.currentEvent != null)
            {
                return $"Currently running event: {Instance.currentEvent.randomEventData.eventType}. To start another first cancel this one.";
            }

            BaseEvent evnt = Instance.randomEventGenerator.GetEvent(args[0])?.GetBaseEvent();

            if (evnt == null)
            {
                return $"Unable to start event {args[0]}, did you spell it correctly?";
            }

            Instance.ExecuteRandomEvent(evnt);
            return $"Starting {args[0]}";
        }
        
        [CommandLineFunctionality.CommandLineArgumentFunction("next", "randomevent")]
        public static string RunNextEvent(List<string> args)
        {
            if (Instance.currentEvent != null)
            {
                return $"Currently running event: {Instance.currentEvent.randomEventData.eventType}. To start another first cancel this one.";
            }

            // Select which event should be played
            BaseEvent eventToPlay = Instance.SelectEvent();

            // Start the random event
            Instance.ExecuteRandomEvent(eventToPlay);

            return $"Starting {eventToPlay.randomEventData.eventType}";
        }

        [CommandLineFunctionality.CommandLineArgumentFunction("cancelevent", "randomevent")]
        public static string CancelEvent(List<string> args)
        {
            return Instance.CancelEvent() ? "Current random event canceled!" : "No random event running.";
        }
        
        [CommandLineFunctionality.CommandLineArgumentFunction("list", "randomevent")]
        public static string ListRandomEvents(List<string> args)
        {

            var randomEvents = new List<string>
            {
                "AFlirtatiousEncounter",
                "AheadOfTime",
                "ArmyGames",
                "ArmyInvite",
                "BanditAmbush",
                "BeeKind",
                "BeggarBegging",
                "BetMoney",
                "BirdSong",
                "BirthdayParty",
                "BottomsUp",
                "BumperCrops",
                "BunchOfPrisoners",
                "ChattingCommanders",
                "CompanionAdmire",
                "Courier",
                "DiseasedCity",
                "DreadedSweats",
                "Duel",
                "Dysentery",
                "EagerTroops",
                "ExoticDrinks",
                "FallenSoldierFamily",
                "FantasticFighters",
                "Feast",
                "FishingSpot",
                "FleeingFate",
                "FoodFight",
                "GranaryRats",
                "HotSprings",
                "HuntingTrip",
                "LightsInTheSky",
                "LoggingSite",
                "LookUp",
                "MassGrave",
                "Momentum",
                "NotOfThisWorld",
                "OldRuins",
                "PassingComet",
                "PerfectWeather",
                "PoisonedWine",
                "PrisonerRebellion",
                "RedMoon",
                "Refugees",
                "Robbery",
                "RunawaySon",
                "SecretSinger",
                "SecretsOfSteel",
                "SpeedyRecovery",
                "SuccessfulDeeds",
                "SuddenStorm",
                "SupernaturalEncounter",
                "TargetPractice",
                "Travellers",
                "TravellingMerchant",
                "Undercooked",
                "UnexpectedWedding",
                "ViolatedGirl",
                "WanderingLivestock"
            };
            
            var stringBuilder = new StringBuilder();
            
            foreach (var eventName in randomEvents)
            {
                stringBuilder.AppendLine(eventName);
            }

            return stringBuilder.ToString();
        }


        private void OnSessionLaunched(CampaignGameStarter cgs)
        {
            ResetEventTimer();
        }

        private uint inGameHoursPassed;

        private DateTime lastEventTime = DateTime.Now;
        private int minutesForNextEvent;

        private BaseEvent currentEvent;

        /// <summary>
        /// Check whether a random event should occur now
        /// </summary>
        private void ProcessRandomEvent()
        {
            if (currentEvent != null)
            {
                return;
            }

            inGameHoursPassed++;

            if (inGameHoursPassed < GeneralSettings.Basic.GetMinimumInGameHours() ||
                (DateTime.Now - lastEventTime).Minutes < minutesForNextEvent) return;
            // Select which event should be played
            BaseEvent eventToPlay = SelectEvent();

            // Start the random event
            ExecuteRandomEvent(eventToPlay);
        }

        /// <summary>
        /// Selects the event that will be played
        /// </summary>
        /// <returns></returns>
        
        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private Queue<string> eventHistory = new Queue<string>();
        
        private BaseEvent SelectEvent()
        {
            BaseEvent eventToPlay;
            
            var ConfigFile = new IniFile(ParseIniFile.GetTheConfigFile());
            
            var DistinctEventCycleLength = ConfigFile.ReadInteger("GeneralSettings", "DistinctEventCycleLength");
            
            do
            {
                eventToPlay = randomEventGenerator.GetRandom().GetBaseEvent();
            } while (eventHistory.Contains(eventToPlay.randomEventData.eventType));
    
            // Update the history
            eventHistory.Enqueue(eventToPlay.randomEventData.eventType);
            if (eventHistory.Count > DistinctEventCycleLength)
            {
                eventHistory.Dequeue();
            }

            return eventToPlay;
        }

        /// <summary>
        /// Pick a random event and execute it
        /// </summary>
        private void ExecuteRandomEvent(BaseEvent aEvent)
        {
            if (Hero.MainHero.IsPrisoner || !MobileParty.MainParty.IsActive)
            {
                return;
            }

            if (!aEvent.CanExecuteEvent())
            {
                return;
            }

            // Stop the processing of random events
            currentEvent = aEvent;
            aEvent.onEventCompleted += EventEnded;

            aEvent.StartEvent();
        }

        private bool CancelEvent()
        {
            if (currentEvent == null) return false;
            currentEvent.CancelEvent();
            EventEnded();
            return true;
        }

        private void EventEnded()
        {
            ResetEventTimer();
            currentEvent = null;
        }

        private void PopulateRandomEventGenerator()
        {
            randomEventGenerator.AddEvents(GetRandomEventData());
        }

        private void ResetEventTimer()
        {
            inGameHoursPassed = 0;
            minutesForNextEvent = MBRandom.RandomInt(GeneralSettings.Basic.GetMinimumRealMinutes(), GeneralSettings.Basic.GetMaximumRealMinutes());
            lastEventTime = DateTime.Now;
        }

        private static IEnumerable<RandomEventData> GetRandomEventData()
        {
            var properties = ModSettings.RandomEvents.GetType().GetProperties();

            return properties.Select(propertyInfo => (RandomEventData)propertyInfo.GetValue(ModSettings.RandomEvents, null)).ToList();
        }
    }
}
