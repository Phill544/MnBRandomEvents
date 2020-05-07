using CryingBuffalo.RandomEvents.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace CryingBuffalo.RandomEvents
{
    public class RandomEventsSubmodule : MBSubModuleBase
    {
        public static RandomEventsSubmodule Instance { get; private set; }

        public readonly Color textColor = Color.FromUint(6750401U);

        public RandomEventGenerator RandomEventGenerator = null;

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            Instance = this;

            Settings.LoadGeneralSettings();
            Settings.LoadRandomEventSettings();

            RandomEventGenerator = new RandomEventGenerator();
            PopulateRandomEventGenerator();
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();

            InformationManager.DisplayMessage(new InformationMessage("Successfully loaded 'RandomEvents'.", textColor));
        }

        public override void OnGameLoaded(Game game, object initializerObject)
        {
            base.OnGameLoaded(game, initializerObject);

            try
            {
                //CampaignEvents.HourlyTickEvent.AddNonSerializedListener(this, ProcessRandomEvent);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while trying to initialize 'RandomEvents' :\n\n" + ex.Message + "\n\n" + ex.StackTrace);
            }
        }

        [CommandLineFunctionality.CommandLineArgumentFunction("run", "randomevent")]
        public static string RunRandomEvent(List<string> args)
        {
            if (args.Count < 1)
            {
                return "You must provide the type of event to run";
            }            

            if (RandomEventsSubmodule.Instance.currentEvent != null)
            {
                return $"Currently running event: {Instance.currentEvent.RandomEventData.EventType}. To start another first cancel this one.";
            }

            BaseEvent evnt = RandomEventFactory.CreateEvent(args[0]);

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
            if (RandomEventsSubmodule.Instance.currentEvent != null)
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
                return "Current random event cancelled!";
            }
            return "No random event running.";
        }

        private uint hoursPassed = 0;

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

            hoursPassed += 1;
            InformationManager.DisplayMessage(new InformationMessage($"{(int)hoursPassed} hours passed", textColor));

            if (hoursPassed % 5 == 0)
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
            return RandomEventFactory.CreateEvent(RandomEventGenerator.GetRandom().EventType);
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
            currentEvent = null;
        }

        private void PopulateRandomEventGenerator()
        {
            RandomEventGenerator.AddEvent(Settings.RandomEvents.BetMoneyData);
            RandomEventGenerator.AddEvent(Settings.RandomEvents.BumperCropData);
            RandomEventGenerator.AddEvent(Settings.RandomEvents.BanditAmbushData);
            RandomEventGenerator.AddEvent(Settings.RandomEvents.GranaryRatsData);
            RandomEventGenerator.AddEvent(Settings.RandomEvents.TargetPracticeData);
            RandomEventGenerator.AddEvent(Settings.RandomEvents.PrisonerRebellionData);
            RandomEventGenerator.AddEvent(Settings.RandomEvents.ChattingCommandersData);
            //RandomEventGenerator.AddEvent(Settings.RandomEvents.GloriousFoodData);
            RandomEventGenerator.AddEvent(Settings.RandomEvents.DiseasedCityData);
            RandomEventGenerator.AddEvent(Settings.RandomEvents.MomentumData);
            RandomEventGenerator.AddEvent(Settings.RandomEvents.SecretSingerData);
            RandomEventGenerator.AddEvent(Settings.RandomEvents.BeeKindData);
            RandomEventGenerator.AddEvent(Settings.RandomEvents.FoodFightData);
            RandomEventGenerator.AddEvent(Settings.RandomEvents.PerfectWeatherData);
            RandomEventGenerator.AddEvent(Settings.RandomEvents.WanderingLivestockData);
            RandomEventGenerator.AddEvent(Settings.RandomEvents.EagerTroopsData);
            RandomEventGenerator.AddEvent(Settings.RandomEvents.SpeedyRecoveryData);
            RandomEventGenerator.AddEvent(Settings.RandomEvents.FantasticFightersData);
            RandomEventGenerator.AddEvent(Settings.RandomEvents.ExoticDrinksData);
        }
    }
}
