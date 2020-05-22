using CryingBuffalo.RandomEvents.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace CryingBuffalo.RandomEvents
{
	public class RandomEventBehavior : CampaignBehaviorBase
	{
        public static RandomEventBehavior Instance { get; private set; }
               
        public RandomEventGenerator RandomEventGenerator = null;

        public RandomEventBehavior()
        {
            Instance = this;

            RandomEventGenerator = new RandomEventGenerator();
            PopulateRandomEventGenerator();
        }

        public override void RegisterEvents()
		{
            CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(OnSessionLaunched));
            CampaignEvents.HourlyTickEvent.AddNonSerializedListener(this, ProcessRandomEvent);
        }

		public override void SyncData(IDataStore dataStore)
		{
            SaveSystem.SetFilePath();

            if (dataStore.IsSaving)
            {
                SaveSystem.SaveData();
            }
            else if (dataStore.IsLoading)
            {
                SaveSystem.LoadData();
            }
		}


        [CommandLineFunctionality.CommandLineArgumentFunction("run", "randomevent")]
        public static string RunRandomEvent(List<string> args)
        {
            if (args.Count < 1)
            {
                return "You must provide the type of event to run";
            }

            if (RandomEventBehavior.Instance.currentEvent != null)
            {
                return $"Currently running event: {Instance.currentEvent.RandomEventData.EventType}. To start another first cancel this one.";
            }

            BaseEvent evnt = Instance.RandomEventGenerator.GetEvent(args[0])?.GetBaseEvent();

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
            if (RandomEventBehavior.Instance.currentEvent != null)
            {
                return $"Currently running event: {Instance.currentEvent.RandomEventData.EventType}. To start another first cancel this one.";
            }

            // Select which event should be played
            BaseEvent eventToPlay = Instance.SelectEvent();

            // Start the random event
            Instance.ExecuteRandomEvent(eventToPlay);

            return $"Starting {eventToPlay.RandomEventData.EventType}";
        }

        [CommandLineFunctionality.CommandLineArgumentFunction("cancelevent", "randomevent")]
        public static string CancelEvent(List<string> args)
        {
            if (Instance.CancelEvent())
            {
                return "Current random event canceled!";
            }
            return "No random event running.";
        }

        private void OnSessionLaunched(CampaignGameStarter cgs)
        {
            ResetEventTimer();
        }

        private uint inGameHoursPassed = 0;

        private DateTime lastEventTime = DateTime.Now;
        private int minutesForNextEvent;

        private BaseEvent currentEvent = null;

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

            if (inGameHoursPassed >= Settings.GeneralSettings.MinimumInGameHours && (DateTime.Now - lastEventTime).Minutes >= minutesForNextEvent)
            {
                // Select which event should be played
                BaseEvent eventToPlay = SelectEvent();

                // Start the random event
                ExecuteRandomEvent(eventToPlay);
            }
        }

        /// <summary>
        /// Selects the event that will be played
        /// </summary>
        /// <returns></returns>
        private BaseEvent SelectEvent()
        {
            return RandomEventGenerator.GetRandom().GetBaseEvent();
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
            aEvent.OnEventCompleted += EventEnded;

            aEvent.StartEvent();
        }

        private bool CancelEvent()
        {
            if (currentEvent != null)
            {
                currentEvent.CancelEvent();
                EventEnded();
                return true;
            }
            return false;
        }

        private void EventEnded()
        {
            ResetEventTimer();
            currentEvent = null;
        }

        private void PopulateRandomEventGenerator()
        {
            RandomEventGenerator.AddEvents(GetRandomEventData());
        }

        private void ResetEventTimer()
        {
            inGameHoursPassed = 0;
            minutesForNextEvent = MBRandom.RandomInt(Settings.GeneralSettings.MinimumRealMinutes, Settings.GeneralSettings.MaximumRealMinutes);
            lastEventTime = DateTime.Now;
        }

        private List<RandomEventData> GetRandomEventData()
        {
            List<RandomEventData> eventData = new List<RandomEventData>();
            var properties = Settings.RandomEvents.GetType().GetProperties();

            foreach (var propertyInfo in properties)
            {
                eventData.Add((RandomEventData)propertyInfo.GetValue(Settings.RandomEvents, null));
            }

            return eventData;
        }
    }
}
