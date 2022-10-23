using System;
using System.Collections.Generic;
using System.Windows;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace CryingBuffalo.RandomEvents.Events
{
    public sealed class SupernaturalEncounter : BaseEvent
    {
        private const string EventTitle = "A Supernatural Encounter";

        public SupernaturalEncounter() : base(Settings.Settings.RandomEvents.SupernaturalEncounterData)
        {
        }

        public override void CancelEvent()
        {
        }

        public override bool CanExecuteEvent()
        {
            return true;
        }

        public override void StartEvent()
        {
            if (Settings.Settings.GeneralSettings.DebugMode)
            {
                InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}",
                    RandomEventsSubmodule.TextColor));
            }

            var inquiryElements = new List<InquiryElement>
            {
                new InquiryElement("a", "Follow the apparition", null, true, "What could she possibly want?"),
                new InquiryElement("b", "Try talking to her", null, true, "Seems like the logical choice..."),
                new InquiryElement("c", "Scream and run away", null, true, "I swear I'm not crazy!"),
                new InquiryElement("d", "Ignore it", null, true, "It's just a dream anyway...")
            };

            var msid = new MultiSelectionInquiryData(
                EventTitle,
                "You are sleeping peacefully in your tent at night when you are awoken by the apparition of a young woman. She stands there looking at you and then she turns to leave but stops at the exit and turn to you. You get the feeling she wants you to follow her.",
                inquiryElements,
                false,
                1,
                "Okay",
                null,
                elements =>
                {
                    switch ((string)elements[0].Identifier)
                    {
                        case "a":
                            InformationManager.ShowInquiry(
                                new InquiryData(EventTitle,
                                    "You follow the apparition. A few men who also have seen the spectacle joins in following her.\n The apparition stops under a lone tree in a meadow and disappears. You and your men stare at each other in disbelief.\n \n You and your men go back to the main camp and discuss what should be done.\n After some deliberation, you come to the conclusion that you go back and dig at the site she disappeared to see if you find anything.\n After digging for about 30 minutes you come across a single skeleton.\n This must be her! You and your men respectfully gather up the bones and gives them a proper burial.\n Hopefully the restless spirit will finally have peace.",
                                    true, false, "Done", null, null, null), true);
                            break;
                        case "b":
                            InformationManager.ShowInquiry(
                                new InquiryData(EventTitle,
                                    "You manage to utter a single 'hello'\n The women then lets out a bone chilling shriek and vanishes before your eyes. \n You spend the rest of the night shaking like a leaf in your bed.",
                                    true, false, "Done", null, null, null), true);
                            break;
                        case "c":
                            InformationManager.ShowInquiry(
                                new InquiryData(EventTitle,
                                    "HELP!\n You leap from your bed and bolt out the door. \n Your men around the campfire starts laughing as soon as you tell them about the event.",
                                    true, false, "Done", null, null, null), true);
                            break;
                        case "d":
                            InformationManager.ShowInquiry(
                                new InquiryData(EventTitle,
                                    "You wake up fully rested the next morning, the event of the night not on your mind. ",
                                    true, false, "Done", null, null, null), true);
                            break;
                        default:
                            MessageBox.Show($"Error while selecting option for \"{randomEventData.eventType}\"");
                            break;
                    }
                },
                null); // What to do on the "cancel" button, shouldn't ever need it.

            MBInformationManager.ShowMultiSelectionInquiry(msid, true);

            StopEvent();
        }

        private void StopEvent()
        {
            try
            {
                onEventCompleted.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error while stopping \"{randomEventData.eventType}\" event :\n\n {ex.Message} \n\n {ex.StackTrace}");
            }
        }
    }


    public class SupernaturalEncounterData : RandomEventData
    {
        public SupernaturalEncounterData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
        {
        }

        public override BaseEvent GetBaseEvent()
        {
            return new SupernaturalEncounter();
        }
    }
}