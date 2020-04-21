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

            Settings.Load();

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
                CampaignEvents.HourlyTickEvent.AddNonSerializedListener(this, ProcessRandomEvent);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while trying to initialize 'RandomEvents' :\n\n" + ex.Message + "\n\n" + ex.StackTrace);
            }
        }

        [CommandLineFunctionality.CommandLineArgumentFunction("run", "randomevent")]
        public static void RunRandomEvent(List<string> args)
        {
            BaseEvent evnt = RandomEventFactory.CreateEvent(args[0]);

            if (RandomEventsSubmodule.Instance.currentEvent != null)
            {
                InformationManager.DisplayMessage(new InformationMessage($"Currently running event: {nameof(Instance.currentEvent.RandomEventData.EventType)}. To start another first cancel this one.", Instance.textColor));
            }

            Instance.ExecuteRandomEvent(evnt);
        }

        [CommandLineFunctionality.CommandLineArgumentFunction("cancelevent", "randomevent")]
        public static void CancelEvent(List<string> args)
        {
            Instance.CancelEvent();
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

            if (!aEvent.CanExecuteEvent())
            {
                return;
            }

            // Stop the processing of random events
            currentEvent = aEvent;
            aEvent.OnEventCompleted += EventEnded;

            aEvent.StartEvent();
        }

        private void CancelEvent()
        {
            if (currentEvent != null)
            {
                currentEvent.CancelEvent();
                EventEnded();
            }
        }

        private void EventEnded()
        {
            currentEvent = null;
        }

        private void PopulateRandomEventGenerator()
        {
            RandomEventGenerator.AddEvent(Settings.RandomEvents.BetMoneyData);
            RandomEventGenerator.AddEvent(Settings.RandomEvents.BumperCropData);
        }
    }
}

/*InformationManager.ShowInquiry(
    new InquiryData("Time is passing!",
                    $"Just wanted you to know that {hoursPassed} hours have passed!",
                    true, 
                    true,
                    "Hell yeah!",
                    "Oh no...",
                    () => { InformationManager.DisplayMessage(new InformationMessage($"You accepted", Color.FromUint(6750401U))); },
                    () => { InformationManager.DisplayMessage(new InformationMessage($"You rejected", Color.FromUint(6750401U))); }
                    ), true);*/

/*List<InquiryElement> inquiryElements = new List<InquiryElement>();
inquiryElements.Add(new InquiryElement("a", "First option", new ImageIdentifier(Banner.CreateRandomClanBanner()), false, null));
inquiryElements.Add(new InquiryElement("b", "Second option", new ImageIdentifier(Banner.CreateRandomClanBanner()), true, "Blah blah blah"));
inquiryElements.Add(new InquiryElement("c", "Third option", new ImageIdentifier(Banner.CreateRandomClanBanner())));
inquiryElements.Add(new InquiryElement("d", "Forth option", new ImageIdentifier(Banner.CreateRandomClanBanner())));

MultiSelectionInquiryData msid = new MultiSelectionInquiryData(
                                        "Time is passing",
                                        $"{hoursPassed} have passed since you loaded the game... Now you must choose. \nsdjhv anvidnsfvinu93nrv93nv ione rivnu\n\n\n\n 9p3unv93nu4 v93unv 09u83nv93n804vun 394v nm9-3 8ngm 934gmn0938ng 983ng093ng9 3ng98 n3904gn9pg8n 9384gn \n9038n4g9p 38ng4 \t983n4g9n3g 9p8n3g9p8n3g p983n4g9p8 n3 49g n394g8j349g 8hj",
                                        inquiryElements,
                                        true,
                                        true,
                                        "Yes?",
                                        "No?",
                                        null,
                                        null);

InformationManager.ShowMultiSelectionInquiry(msid, true);*/
