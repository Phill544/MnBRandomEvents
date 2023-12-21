using System;
using System.Collections.Generic;
using System.Windows;
using Bannerlord.RandomEvents.Helpers;
using Bannerlord.RandomEvents.Settings;
using Ini.Net;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace Bannerlord.RandomEvents.Events.CCEvents
{
    public sealed class MassGrave : BaseEvent
    {
        private readonly bool eventDisabled;
        private readonly int minSoldiers;
        private readonly int maxSoldiers;
        private readonly int minBodies;
        private readonly int maxBodies;
        private readonly int minBaseMoraleLoss;
        private readonly int maxBaseMoraleLoss;
        

        public MassGrave() : base(ModSettings.RandomEvents.MassGraveData)
        {
            var ConfigFile = new IniFile(ParseIniFile.GetTheConfigFile());
            
            eventDisabled = ConfigFile.ReadBoolean("MassGrave", "EventDisabled");
            minSoldiers = ConfigFile.ReadInteger("MassGrave", "MinSoldiers");
            maxSoldiers = ConfigFile.ReadInteger("MassGrave", "MaxSoldiers");
            minBodies = ConfigFile.ReadInteger("MassGrave", "MinBodies");
            maxBodies = ConfigFile.ReadInteger("MassGrave", "MaxBodies");
            minBaseMoraleLoss = ConfigFile.ReadInteger("MassGrave", "MinBaseMoraleLoss");
            maxBaseMoraleLoss = ConfigFile.ReadInteger("MassGrave", "MaxBaseMoraleLoss");
        }

        public override void CancelEvent()
        {
        }

        private bool HasValidEventData()
        {
            if (eventDisabled == false)
            {
                if (minSoldiers != 0 || maxSoldiers != 0 || minBodies != 0 || maxBodies != 0 ||  minBaseMoraleLoss != 0 || maxBaseMoraleLoss != 0)
                {
                    return true;
                }
            }
            
            return false;
        }

        public override bool CanExecuteEvent()
        {

            return HasValidEventData() && MobileParty.MainParty.CurrentSettlement == null && MobileParty.MainParty.MemberRoster.TotalRegulars >= maxSoldiers + minSoldiers;
        }

        public override void StartEvent()
        {
            var eventTitle = new TextObject("{=MassGrave_Title}The Mass Grave").ToString();
            
            var closestSettlement = ClosestSettlements.GetClosestAny(MobileParty.MainParty).ToString();

            var baseMoraleLoss = MBRandom.RandomInt(minBaseMoraleLoss, maxBaseMoraleLoss);

            var soldiersDiscovery = MBRandom.RandomInt(minSoldiers, maxSoldiers);
            var bodiesInGrave = MBRandom.RandomInt(minBodies, maxBodies);
            
            var eventDescription = new TextObject(
                    "{=MassGrave_Event_Desc}Your party has set up camp near {closestSettlement} and you have sent out some men to gather resources and hunt. Out of the blue {soldiersDiscovery} " +
                    "of your men come back and tell you that there is something you need to see. You join your men as they escort you to whatever it is they want to show you.\n" +
                    "When you arrive your are shocked to see a fresh mass grave filled with men, women and children. Your men asks you what they think we should do.")
                .SetTextVariable("closestSettlement", closestSettlement)
                .SetTextVariable("soldiersDiscovery", soldiersDiscovery)
                .ToString();
            
            var eventOption1 = new TextObject("{=MassGrave_Event_Option_1}Make individual graves").ToString();
            var eventOption1Hover = new TextObject("{=MassGrave_Event_Option_1_Hover}They should be given a proper burial").ToString();
            
            var eventOption2 = new TextObject("{=MassGrave_Event_Option_2}Fill the grave with dirt").ToString();
            var eventOption2Hover = new TextObject("{=MassGrave_Event_Option_2_Hover}The least you can do is fill the hole").ToString();
            
            var eventOption3 = new TextObject("{=MassGrave_Event_Option_3}Burn the bodies").ToString();
            var eventOption3Hover = new TextObject("{=MassGrave_Event_Option_3_Hover}Quickest and easiest way").ToString();
            
            var eventOption4 = new TextObject("{=MassGrave_Event_Option_4}Leave them").ToString();
            var eventOption4Hover = new TextObject("{=MassGrave_Event_Option_4_Hover}Not your problem").ToString();
            
            var eventButtonText1 = new TextObject("{=MassGrave_Event_Button_Text_1}Choose").ToString();
            var eventButtonText2 = new TextObject("{=MassGrave_Event_Button_Text_2}Done").ToString();
            
            var inquiryElements = new List<InquiryElement>
            {
                new InquiryElement("a", eventOption1, null, true, eventOption1Hover),
                new InquiryElement("b", eventOption2, null, true, eventOption2Hover),
                new InquiryElement("c", eventOption3, null, true, eventOption3Hover),
                new InquiryElement("d", eventOption4, null, true, eventOption4Hover)
            };
            
            var eventOptionAText = new TextObject(
                    "{=MassGrave_Event_Choice_1}You order two of your men to go back to camp and fetch shovels, linen and more men while you and the others start to remove the bodies from the grave. " +
                    "You lay the bodies next to each other in neat lines. Whenever there is a child brought up from the grave you feel a great sadness and you cannot help hold back a few tears. Several of your " +
                    "men weep as well and you feel the mood is very dark.\nYour men return with the requested supplies and additional men. Some start to dig graves while others wrap bodies in linen. " +
                    "In total you pull {bodiesInGrave} bodies from the mass grave. After spending several hours digging and burying you are finally done just before nightfall. " +
                    "You and your men return to camp and decided to sit around the campfire discussing your feelings after the today's events.")
                .SetTextVariable("bodiesInGrave", bodiesInGrave)
                .ToString();
            
            var eventOptionBText = new TextObject(
                    "{=MassGrave_Event_Choice_2}You order two of your men to go back to camp and fetch shovels and more men while you and the others start to fill the hole. " +
                    "After a few minutes men with shovels join in. You end up digging for a few hours until the grave is filled. You all decide to recite some prayers before leaving.")
                .ToString();
            
            var eventOptionCText = new TextObject(
                    "{=MassGrave_Event_Choice_3}You order your men to start the burning of the bodies. It doesn't take long for them to burn. " +
                    "When you return after a few hours only ash and bone are left in the grave.")
                .ToString();
            
            var eventOptionDText = new TextObject(
                    "{=MassGrave_Event_Choice_4}You decide to just leave the area as it is. You tell your men they can handle this in any way they want. " +
                    "Later that evening your men return having buried the bodies.")
                .ToString();
            
            var eventMsg1 =new TextObject(
                    "{=MassGrave_Event_Msg_1}Your party lost {baseMoraleLoss} morale due to recent events.")
                .SetTextVariable("baseMoraleLoss", baseMoraleLoss - 2)
                .ToString();
            
            var eventMsg2=new TextObject(
                    "{=MassGrave_Event_Msg_2}Your party lost {baseMoraleLoss} morale due to recent events.")
                .SetTextVariable("baseMoraleLoss", baseMoraleLoss - 3)
                .ToString();
            
            var eventMsg3 =new TextObject(
                    "{=MassGrave_Event_Msg_3}Your party lost {baseMoraleLoss} morale due to recent events.")
                .SetTextVariable("baseMoraleLoss", baseMoraleLoss - 4)
                .ToString();
            
            var eventMsg4 =new TextObject(
                    "{=MassGrave_Event_Msg_4}Your party lost {baseMoraleLoss} morale due to recent events.")
                .SetTextVariable("baseMoraleLoss", baseMoraleLoss - 5)
                .ToString();


            var msid = new MultiSelectionInquiryData(eventTitle, eventDescription, inquiryElements, false, 1, 1, eventButtonText1, null,
                elements =>
                {
                    switch ((string)elements[0].Identifier)
                    {
                        case "a":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionAText, true, false, eventButtonText2, null, null, null), true);
                            
                            MobileParty.MainParty.RecentEventsMorale -= baseMoraleLoss - 2;
                            MobileParty.MainParty.MoraleExplained.Add(-baseMoraleLoss - 2);
                            
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg1, RandomEventsSubmodule.Msg_Color_NEG_Outcome));
                            
                            break;
                        case "b":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionBText, true, false, eventButtonText2, null, null, null), true);
                            
                            MobileParty.MainParty.RecentEventsMorale -= baseMoraleLoss - 3;
                            MobileParty.MainParty.MoraleExplained.Add(-baseMoraleLoss - 3);
                            
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg2, RandomEventsSubmodule.Msg_Color_NEG_Outcome));
                            
                            break;
                        case "c":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionCText, true, false, eventButtonText2, null, null, null), true);
                            
                            MobileParty.MainParty.RecentEventsMorale -= baseMoraleLoss - 4;
                            MobileParty.MainParty.MoraleExplained.Add(-baseMoraleLoss - 4);
                            
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg3, RandomEventsSubmodule.Msg_Color_NEG_Outcome));
                            
                            break;
                        case "d":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionDText, true, false, eventButtonText2, null, null, null), true);
                            
                            MobileParty.MainParty.RecentEventsMorale -= baseMoraleLoss - 5;
                            MobileParty.MainParty.MoraleExplained.Add(-baseMoraleLoss - 5);
                            
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg4, RandomEventsSubmodule.Msg_Color_NEG_Outcome));
                            
                            break;
                        default:
                            MessageBox.Show($"Error while selecting option for \"{randomEventData.eventType}\"");
                            break;
                    }
                },
                null, null);
            
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


    public class MassGraveData : RandomEventData
    {
        public MassGraveData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
        {
        }

        public override BaseEvent GetBaseEvent()
        {
            return new MassGrave();
        }
    }
}