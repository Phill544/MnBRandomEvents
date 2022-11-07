using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using CryingBuffalo.RandomEvents.Helpers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace CryingBuffalo.RandomEvents.Events.CCEvents
{
    public class RunawaySon : BaseEvent
    {
        private readonly int minGold;
        private readonly int maxGold;

        public RunawaySon() : base(Settings.ModSettings.RandomEvents.RunawaySonData)
        {
            minGold = Settings.ModSettings.RandomEvents.RunawaySonData.minGold;
            maxGold = Settings.ModSettings.RandomEvents.RunawaySonData.maxGold;
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
            if (Settings.ModSettings.GeneralSettings.DebugMode)
            {
                InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}", RandomEventsSubmodule.TextColor));
            }
            
            var eventTitle = new TextObject("{=RunawaySon_Title}Runaway Son").ToString();
            
            var goldLooted = MBRandom.RandomInt(minGold, maxGold);
            
            var closestSettlement = ClosestSettlements.GetClosestAny(MobileParty.MainParty).ToString();
            
            var eventDescription = new TextObject(
                    "{=RunawaySon_Event_Desc}As your party moves through the land near {closestSettlement}, you are approached by a young man. " +
                    "He explains that he ran away from the family farm after suffering abuse from his parents for years. He wants to join your party and he tells you he has some skills with weapons.")
                .SetTextVariable("closestSettlement", closestSettlement)
                .ToString();
            
            var eventOption1 = new TextObject("{=RunawaySon_Event_Option_1}Take him in and train him").ToString();
            var eventOption1Hover = new TextObject("{=RunawaySon_Event_Option_1_Hover}You could use the distraction of having someone to train").ToString();
            
            var eventOption2 = new TextObject("{=RunawaySon_Event_Option_2}Tell him he can tag along").ToString();
            var eventOption2Hover = new TextObject("{=RunawaySon_Event_Option_2_Hover}You really don't have time to babysit him").ToString();
            
            var eventOption3 = new TextObject("{=RunawaySon_Event_Option_3}Go away").ToString();
            var eventOption3Hover = new TextObject("{=RunawaySon_Event_Option_3_Hover}He needs to leave").ToString();
            
            var eventOption4 = new TextObject("{=RunawaySon_Event_Option_4}Kill him").ToString();
            var eventOption4Hover = new TextObject("{=RunawaySon_Event_Option_4_Hover}It's a cruel world").ToString();
            
            var eventButtonText1 = new TextObject("{=RunawaySon_Event_Button_Text_1}Okay").ToString();
            var eventButtonText2 = new TextObject("{=RunawaySon_Event_Button_Text_2}Done").ToString();

            var inquiryElements = new List<InquiryElement>
            {
                new InquiryElement("a", eventOption1, null, true, eventOption1Hover),
                new InquiryElement("b", eventOption2, null, true, eventOption2Hover),
                new InquiryElement("c", eventOption3, null, true, eventOption3Hover),
                new InquiryElement("d", eventOption4, null, true, eventOption4Hover)
            };
            
            var eventOptionAText = new TextObject(
                    "{=RunawaySon_Event_Choice_1}You tell him he is welcome in your ranks and you will personally train him and make a fine solider of him.")
                .ToString();
            
            var eventOptionBText = new TextObject(
                    "{=RunawaySon_Event_Choice_2}You tell him he can tag along, but under no circumstance should he interfere in your affairs.")
                .ToString();
            
            var eventOptionCText = new TextObject(
                    "{=RunawaySon_Event_Choice_3}You tell him to get lost. The man turns around and promptly leaves.")
                .ToString();
            
            var eventOptionDText = new TextObject(
                    "{=RunawaySon_Event_Choice_4}You laugh as you hear his plea and your men soon join in on the laughter. You approach the man and thrust a " +
                    "dagger into his stomach. You watch him fall to the ground in a pool of blood, screaming in pain.\n " +
                    "You kneel down beside him and watch as the light soon leaves his eyes and he dies from his injury. " +
                    "You and some men decide to cut him open and hang his body from a tree as a warning but not before looting his body for {goldLooted} gold.")
                .SetTextVariable("goldLooted",goldLooted)
                .ToString();
            
            var eventMsg1 =new TextObject(
                    "{=RunawaySon_Event_Msg_1}Looted {goldLooted} from his corpse.")
                .SetTextVariable("goldLooted", goldLooted)
                .ToString();

            var msid = new MultiSelectionInquiryData(eventTitle, eventDescription, inquiryElements, false, 1, eventButtonText1, null,
                elements =>
                {
                    switch ((string)elements[0].Identifier)
                    {
                        case "a":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionAText, true, false, eventButtonText2, null, null, null), true);

                            GainOneRecruit();
                            break;
                        
                        case "b":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionBText, true, false, eventButtonText2, null, null, null), true);
                            
                            GainOneRecruit();
                            break;
                        
                        case "c":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionCText, true, false, eventButtonText2, null, null, null), true);
                            break;
                        
                        case "d":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionDText, true, false, eventButtonText2, null, null, null), true);
                            Hero.MainHero.ChangeHeroGold(goldLooted);
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg1, RandomEventsSubmodule.MsgColor));
                            
                            
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

        private static void GainOneRecruit()
        {
            var settlements = Settlement.FindAll(s => !s.IsHideout).ToList();
            var closestSettlement = settlements.MinBy(s => MobileParty.MainParty.GetPosition().DistanceSquared(s.GetPosition()));

            //Currently it gives just a random solider from the current culture. Will fix once API docs are updated
            var bandits = PartySetup.CreateBanditParty();
            bandits.MemberRoster.Clear();
            PartySetup.AddRandomCultureUnits(bandits, 1, closestSettlement.Culture);
            MobileParty.MainParty.MemberRoster.Add(bandits.MemberRoster);
            bandits.RemoveParty();
        }
    }


    public class RunawaySonData : RandomEventData
    {
        public readonly int minGold;
        public readonly int maxGold;

        public RunawaySonData(string eventType, float chanceWeight, int minGold, int maxGold) : base(eventType,
            chanceWeight)
        {
            this.minGold = minGold;
            this.maxGold = maxGold;
        }

        public override BaseEvent GetBaseEvent()
        {
            return new RunawaySon();
        }
    }
}