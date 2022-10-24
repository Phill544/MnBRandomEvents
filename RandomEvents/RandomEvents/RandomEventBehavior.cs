using CryingBuffalo.RandomEvents.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace CryingBuffalo.RandomEvents
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

            if (inGameHoursPassed < Settings.Settings.GeneralSettings.MinimumInGameHours ||
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
        private BaseEvent SelectEvent()
        {
            return randomEventGenerator.GetRandom().GetBaseEvent();
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
            minutesForNextEvent = MBRandom.RandomInt(Settings.Settings.GeneralSettings.MinimumRealMinutes, Settings.Settings.GeneralSettings.MaximumRealMinutes);
            lastEventTime = DateTime.Now;
        }

        private static List<RandomEventData> GetRandomEventData()
        {
            var properties = Settings.Settings.RandomEvents.GetType().GetProperties();

            return properties.Select(propertyInfo => (RandomEventData)propertyInfo.GetValue(Settings.Settings.RandomEvents, null)).ToList();
        }
    }
}
