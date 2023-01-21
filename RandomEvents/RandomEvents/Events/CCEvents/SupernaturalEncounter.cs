using System;
using System.Collections.Generic;
using System.Windows;
using CryingBuffalo.RandomEvents.Helpers;
using CryingBuffalo.RandomEvents.Settings;
using Ini.Net;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace CryingBuffalo.RandomEvents.Events.CCEvents
{
    public sealed class SupernaturalEncounter : BaseEvent
    {
        private readonly bool eventDisabled;

        public SupernaturalEncounter() : base(ModSettings.RandomEvents.SupernaturalEncounterData)
        {
            var ConfigFile = new IniFile(ParseIniFile.GetTheConfigFile());
            
            eventDisabled = ConfigFile.ReadBoolean("SupernaturalEncounter", "EventDisabled");
        }

        public override void CancelEvent()
        {
        }
        
        private bool EventCanRun()
        {
            return eventDisabled == false;
        }

        public override bool CanExecuteEvent()
        {
            return EventCanRun() && GeneralSettings.SupernaturalEvents.IsDisabled() == false && MobileParty.MainParty.CurrentSettlement == null && CurrentTimeOfDay.IsNight;
        }

        public override void StartEvent()
        {
            if (GeneralSettings.DebugMode.IsActive())
            {
                var debugMsg = new TextObject(
                        "Starting “{randomEvent}”. This event has no configurable settings.\n\n" +
                        "To disable these messages make sure you set the DebugMode = false in the ini settings\n\nThe ini file is located here : \n{path}"
                    )
                    .SetTextVariable("randomEvent", randomEventData.eventType)
                    .SetTextVariable("path", ParseIniFile.GetTheConfigFile())
                    .ToString();
                
                InformationManager.ShowInquiry(new InquiryData("Debug Info", debugMsg, true, false, "Start Event", null, null, null), true);
            }
            
            var eventTitle = new TextObject("{=SupernaturalEncounter_Title}A Supernatural Encounter").ToString();
            
            var eventDescription = new TextObject(
                    "{=SupernaturalEncounter_Event_Desc}You are sleeping peacefully in your tent at night when you are awoken by the apparition of a young woman. " +
                    "She stands there looking at you and then she turns to leave but stops at the exit and turns back to you. You get the feeling she wants you to follow her.")
                .ToString();
            
            var eventOption1 = new TextObject("{=SupernaturalEncounter_Event_Option_1}Follow the apparition").ToString();
            var eventOption1Hover = new TextObject("{=SupernaturalEncounter_Event_Option_1_Hover}What could she possibly want?").ToString();
            
            var eventOption2 = new TextObject("{=SupernaturalEncounter_Event_Option_2}Try talking to her").ToString();
            var eventOption2Hover = new TextObject("{=SupernaturalEncounter_Event_Option_2_Hover}Seems like the logical choice").ToString();
            
            var eventOption3 = new TextObject("{=SupernaturalEncounter_Event_Option_3}Scream and run away").ToString();
            var eventOption3Hover = new TextObject("{=SupernaturalEncounter_Event_Option_3_Hover}I swear I'm not crazy!").ToString();
            
            var eventOption4 = new TextObject("{=SupernaturalEncounter_Event_Option_4}Ignore it").ToString();
            var eventOption4Hover = new TextObject("{=SupernaturalEncounter_Event_Option_4_Hover}It's just a dream anyway").ToString();
            
            var eventButtonText1 = new TextObject("{=SupernaturalEncounter_Event_Button_Text_1}Okay").ToString();
            var eventButtonText2 = new TextObject("{=SupernaturalEncounter_Event_Button_Text_2}Done").ToString();

            var inquiryElements = new List<InquiryElement>
            {
                new InquiryElement("a", eventOption1, null, true, eventOption1Hover),
                new InquiryElement("b", eventOption2, null, true, eventOption2Hover),
                new InquiryElement("c", eventOption3, null, true, eventOption3Hover),
                new InquiryElement("d", eventOption4, null, true, eventOption4Hover)
            };
            
            var eventOptionAText = new TextObject(
                    "{=SupernaturalEncounter_Event_Choice_1}You follow the apparition. A few men who also have seen the spectacle joins in following her.\n The apparition stops under a lone tree in a meadow and disappears. " +
                    "You and your men stare at each other in disbelief.\n\n You and your men go back to the main camp and discuss what should be done.\n After some deliberation, " +
                    "you come to the conclusion that you go back and dig at the site she disappeared to see if you find anything.\n After digging for about 30 minutes you come across a single skeleton.\n " +
                    "This must be her! You and your men respectfully gather up the bones and gives them a proper burial.\n Hopefully her restless spirit will finally have peace.")
                .ToString();
            
            var eventOptionBText = new TextObject(
                    "{=SupernaturalEncounter_Event_Choice_2}You manage to utter a single 'hello'\n The women then lets out a bone chilling shriek and vanishes before your eyes.\n " +
                    "You spend the rest of the night shaking like a leaf in your bed.")
                .ToString();
            
            var eventOptionCText = new TextObject(
                    "{=SupernaturalEncounter_Event_Choice_3}HELP!\n You leap from your bed and bolt out the door. \n Your men around the campfire start laughing as soon as you tell them about the event.")
                .ToString();
            
            var eventOptionDText = new TextObject(
                    "{=SupernaturalEncounter_Event_Choice_4}You wake up fully rested the next morning with the previous night's event absent from your mind.")
                .ToString();

            var msid = new MultiSelectionInquiryData(eventTitle, eventDescription, inquiryElements, false, 1, eventButtonText1, null,
                elements =>
                {
                    switch ((string)elements[0].Identifier)
                    {
                        case "a":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle,eventOptionAText, true, false, eventButtonText2, null, null, null), true);
                            break;
                        case "b":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionBText, true, false, eventButtonText2, null, null, null), true);
                            break;
                        case "c":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionCText, true, false, eventButtonText2, null, null, null), true);
                            break;
                        case "d":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionDText, true, false, eventButtonText2, null, null, null), true);
                            break;
                        default:
                            MessageBox.Show($"Error while selecting option for \"{randomEventData.eventType}\"");
                            break;
                    }
                },
                null);

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
                MessageBox.Show($"Error while stopping \"{randomEventData.eventType}\" event :\n\n {ex.Message} \n\n {ex.StackTrace}");
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