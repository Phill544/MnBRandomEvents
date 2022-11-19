using System;
using System.Collections.Generic;
using System.Windows;
using CryingBuffalo.RandomEvents.Helpers;
using CryingBuffalo.RandomEvents.Settings;
using CryingBuffalo.RandomEvents.Settings.MCM;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace CryingBuffalo.RandomEvents.Events.CCEvents
{
    public class TheDuel : BaseEvent
    {
        private readonly int victoryChance;
        private readonly int minSoldiersDefected;
        private readonly int maxSoldiersDefected;
        private readonly int minRenownLoss;
        private readonly int maxRenownLoss;
        private readonly int minRenownGain;
        private readonly int maxRenownGain;
        

        public TheDuel() : base(ModSettings.RandomEvents.TheDuelData)
        {
            /*
            minSoldiersToGo = MCM_MenuConfig_A_M.Instance.LS_MinSoldiersToGo;
            maxSoldiersToGo = MCM_MenuConfig_A_M.Instance.LS_MaxSoldiersToGo;
            minYield = MCM_MenuConfig_A_M.Instance.LS_MinYield;
            maxYield = MCM_MenuConfig_A_M.Instance.LS_MaxYield;
            minYieldMultiplier = MCM_MenuConfig_A_M.Instance.LS_MinYieldMultiplier;
            maxYieldMultiplier = MCM_MenuConfig_A_M.Instance.LS_MaxYieldMultiplier;
            */
        }

        public override void CancelEvent()
        {
        }

        public override bool CanExecuteEvent()
        {
            
            return MCM_MenuConfig_A_M.Instance.LS_Disable == false && MobileParty.MainParty.MemberRoster.TotalRegulars >= 50;
        }

        public override void StartEvent()
        {
            if (MCM_ConfigMenu_General.Instance.GS_DebugMode)
            {
                InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}", RandomEventsSubmodule.Dbg_Color));
            }
            
            var eventTitle = new TextObject("{=TheDuel_Title}The Duel").ToString();
            
            var closestSettlement = ClosestSettlements.GetClosestAny(MobileParty.MainParty).ToString();

            var defected = MBRandom.RandomInt(5, 25);

            var duelOutcome = MBRandom.RandomFloatRanged(0.0f, 1.0f);

            bool duelLost = false;
            
            if (duelOutcome <= 0.5f)
            {
                duelLost = true;
            }
            
            
            var eventDescription = new TextObject(
                    "{=TheDuel_Event_Desc}When your party is enjoying a break in the vicinity of {closestSettlement}, you are suddenly approached by one of your low ranking men who explains that he and som others are " +
                    "unhappy with the way you are leading the party. He challenges you to a duel to the death! How do you respond?")
                .SetTextVariable("closestSettlement", closestSettlement)
                .ToString();
            
            var eventOption1 = new TextObject("{=TheDuel_Event_Option_1}Accept the duel").ToString();
            var eventOption1Hover = new TextObject("{=TheDuel_Event_Option_1_Hover}You never back down from a challenge").ToString();
            
            var eventOption2 = new TextObject("{=TheDuel_Event_Option_2}Decline the duel").ToString();
            var eventOption2Hover = new TextObject("{=TheDuel_Event_Option_2_Hover}Why should you accept").ToString();
            
            var eventOption3 = new TextObject("{=TheDuel_Event_Option_3}Kill him").ToString();
            var eventOption3Hover = new TextObject("{=TheDuel_Event_Option_3_Hover}No one disrespects you").ToString();

            var eventButtonText1 = new TextObject("{=TheDuel_Event_Button_Text_1}Choose").ToString();
            var eventButtonText2 = new TextObject("{=TheDuel_Event_Button_Text_2}Done").ToString();
            var eventButtonText3 = new TextObject("{=TheDuel_Event_Button_Text_3}Continue").ToString();
            
            
            var eventOptionATextWin = new TextObject(
                    "{=TheDuelEvent_Choice_1_Win}You accepts his duel! The men are cheering as they hear your words. The men form a circle around both of you with enough space to maneuver in but not " +
                    "to get out of the circle.\n\nThe duel starts! He plunges towards you but you easily move out of the way and smack his ass with the sword which the men find amusing. You tell him " +
                    "that if he backs down now you'll let him live. No response, he instead takes a swing at you that you easily block. Having had enough of this, you easily manage to slash a deep cut in " +
                    "his thigh on his next attempt. He falls to the ground in clear agony. You approach him and places your sword at his shoulder.")
                .ToString();
            
            var eventOptionATextLose = new TextObject(
                    "{=TheDuelEvent_Choice_1_Lose}You accepts his duel! The men are cheering as they hear your words. The men form a circle around both of you with enough space to maneuver in but not " +
                    "to get out of the circle.\n\nThe duel starts! He plunges towards you but you easily move out of the way and smack his ass with the sword which the men find amusing. You tell him " +
                    "that if he backs down now you'll let him live. No response, he instead takes a swing at you, but just as you are about to block you trip on a rock and fall over. You attempt to get back " +
                    "up but you are to late. The challenger now has his sword at your shoulder, ready to cut your head off. You are therefore shocked when he throws his sword down and leaves the circle with a few " +
                    "men behind him.\n In all a total of {defected} men defected from your party after your pathetic display.")
                .SetTextVariable("defected", defected)
                .ToString();
            
            
            var eventOptionATextWinStep2Desc = new TextObject(
                    "{=TheDuelEvent_Choice_1_Win_Step_2_Desc}Do you kill him? Do you spare him? The paths are open, but you must choose.")
                .ToString();
            
            var eventOptionATextWinStep2Option1 = new TextObject(
                    "{=TheDuelEvent_Choice_1_Win_Step_2_Option_1}Spare him")
                .ToString();


            var eventOptionATextWinStep2Option2 = new TextObject(
                    "{=TheDuelEvent_Choice_1_Win_Step_2_Option_2}Kill him")
                .ToString();
            
            
            var eventOptionATextWinStep2Spare = new TextObject(
                    "{=TheDuelEvent_Choice_1_Win_Step_2_Spare}He's spared, MORE TEXT SOON")
                .SetTextVariable("defected", defected)
                .ToString();
            
            var eventOptionATextWinStep2Kill = new TextObject(
                    "{=TheDuelEvent_Choice_1_Win_Step_2_kill}He's dead, MORE TEXT SOON")
                .SetTextVariable("defected", defected)
                .ToString();
            
            var inquiryElements = new List<InquiryElement>
            {
                new InquiryElement("a", eventOption1, null, true, eventOption1Hover),
            };
            
            var inquiryElements_a2 = new List<InquiryElement>
            {
                new InquiryElement("a", eventOptionATextWinStep2Option1, null, true, eventOption1Hover),
                new InquiryElement("b", eventOptionATextWinStep2Option2, null, true, eventOption1Hover),
            };
            
            
            var msid = new MultiSelectionInquiryData(eventTitle, eventDescription, inquiryElements, false, 1,
                eventButtonText1, null,
                elements =>
                   {
                       switch ((string)elements[0].Identifier)
                       { 
                           case "a" when duelLost == false:
                                InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionATextWin, true, false, eventButtonText2, null, null, null), true);
                                
                                var msid2 = new MultiSelectionInquiryData(eventTitle, eventOptionATextWinStep2Desc, inquiryElements_a2, false, 1,
                                    eventButtonText1, null,
                                    elements_a2 =>
                                    {
                                        switch ((string)elements_a2[0].Identifier)
                                        { 
                                            case "a":
                                                InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionATextWinStep2Spare, true, false, eventButtonText2, null, null, null), true);
                                
                                                break;
                                            case "b" :
                                                InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionATextWinStep2Kill, true, false, eventButtonText2, null, null, null), true);
                                                break;
                           
                                            default:
                                                MessageBox.Show($"Error while selecting option for \"{randomEventData.eventType}\"");
                                                break;
                                        }
                                    },
                                    null);
                                
                                MBInformationManager.ShowMultiSelectionInquiry(msid2, true);
                                
                                break;
                           case "a" when duelLost:
                               InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionATextLose, true, false, eventButtonText2, null, null, null), true);
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
                MessageBox.Show(
                    $"Error while stopping \"{randomEventData.eventType}\" event :\n\n {ex.Message} \n\n {ex.StackTrace}");
            }
        }
    }


    public class TheDuelData : RandomEventData
    {

        public TheDuelData(string eventType, float chanceWeight) : base(eventType,
            chanceWeight)
        {
        }

        public override BaseEvent GetBaseEvent()
        {
            return new TheDuel();
        }
    }
}