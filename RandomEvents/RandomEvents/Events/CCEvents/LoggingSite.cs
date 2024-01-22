using System;
using System.Windows;
using Bannerlord.RandomEvents.Helpers;
using Bannerlord.RandomEvents.Settings;
using Ini.Net;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace Bannerlord.RandomEvents.Events.CCEvents
{
    public sealed class LoggingSite : BaseEvent
    {
        private readonly bool eventDisabled;
        private readonly int minSoldiersToGo;
        private readonly int maxSoldiersToGo;
        private readonly int minYield;
        private readonly int maxYield;
        private readonly int minYieldMultiplier;
        private readonly int maxYieldMultiplier;
        

        public LoggingSite() : base(ModSettings.RandomEvents.LoggingSiteData)
        {
            
            var ConfigFile = new IniFile(ParseIniFile.GetTheConfigFile());
            
            eventDisabled = ConfigFile.ReadBoolean("LoggingSite", "EventDisabled");
            minSoldiersToGo = ConfigFile.ReadInteger("LoggingSite", "MinSoldiersToGo");
            maxSoldiersToGo = ConfigFile.ReadInteger("LoggingSite", "MaxSoldiersToGo");
            minYield = ConfigFile.ReadInteger("LoggingSite", "MinYield");
            maxYield = ConfigFile.ReadInteger("LoggingSite", "MaxYield");
            minYieldMultiplier = ConfigFile.ReadInteger("LoggingSite", "MinYieldMultiplier");
            maxYieldMultiplier = ConfigFile.ReadInteger("LoggingSite", "MaxYieldMultiplier");
        }

        public override void CancelEvent()
        {
        }

        private bool HasValidEventData()
        {
            if (eventDisabled == false)
            {
                if (minSoldiersToGo != 0 || maxSoldiersToGo != 0 || minYield != 0 || maxYield != 0 ||  minYieldMultiplier != 0 || maxYieldMultiplier != 0)
                {
                    return true;
                }
            }
            
            return false;
        }

        public override bool CanExecuteEvent()
        {

            return HasValidEventData() && MobileParty.MainParty.MemberRoster.TotalRegulars >= maxSoldiersToGo;
        }

        public override void StartEvent()
        {
            var eventTitle = new TextObject("{=LoggingSite_Title}The Hardwood Forest").ToString();
            
            var closestSettlement = ClosestSettlements.GetClosestAny(MobileParty.MainParty).ToString();

            var soldiersGoneLogging = MBRandom.RandomInt(minSoldiersToGo, maxSoldiersToGo);
            
            var treesChopped = MBRandom.RandomInt(minYield, maxYield);

            var yieldHardwood = treesChopped * MBRandom.RandomInt(minYieldMultiplier, maxYieldMultiplier);
            
            var hardwood = MBObjectManager.Instance.GetObject<ItemObject>("hardwood");
            
            var eventDescription = new TextObject(
                    "{=LoggingSite_Event_Desc}While your party is traveling through the lands near {closestSettlement} you come across a forest rich in hardwood trees. " +
                    "You decide that it's time to stock up on hardwood so you order {soldiersGoneLogging} of your men to get to work. The men agree that this is a good opportunity " +
                    "to get some resources so they do as you say without much complaint.\nYou don't really need much, just enough to start some smithing projects next time you come " +
                    "across a forge. The rest you can easily sell for a nice profit.")
                .SetTextVariable("closestSettlement", closestSettlement)
                .SetTextVariable("soldiersGoneLogging", soldiersGoneLogging)
                .ToString();
            
            var eventOutcome1 = new TextObject(
                    "{=LoggingSite_Outcome_1}The logging crew return just as the sun is setting. In total they chopped down {treesChopped} trees which yielded {yieldHardwood} pieces of Hardwood.\n" +
                    "You admittedly had hoped for more resources than this so you berate your men for being lazy.")
                .SetTextVariable("treesChopped",treesChopped)
                .SetTextVariable("yieldHardwood",yieldHardwood)
                .ToString();
            
            var eventOutcome2 = new TextObject(
                    "{=LoggingSite_Outcome_2}The logging crew return just as the sun is setting. In total they chopped down {treesChopped} trees which yielded {yieldHardwood} pieces of Hardwood.\n" +
                    "You had hoped for a better result but all in all this should be enough to cover your own projects.")
                .SetTextVariable("treesChopped",treesChopped)
                .SetTextVariable("yieldHardwood",yieldHardwood)
                .ToString();
            
            var eventOutcome3 = new TextObject(
                    "{=LoggingSite_Outcome_3}The logging crew return just as the sun is setting. In total they chopped down {treesChopped} trees which yielded {yieldHardwood} pieces of Hardwood.\n" +
                    "This is the result you were hoping for! Enough for your own projects and enough to sell at a settlement for a nice profit. You congratulate your men on a hard days work.")
                .SetTextVariable("treesChopped",treesChopped)
                .SetTextVariable("yieldHardwood",yieldHardwood)
                .ToString();
            
            
            var eventButtonText = new TextObject("{=LoggingSite_Event_Button_Text}Continue")
                .ToString();

            var eventMsg1 =new TextObject(
                    "{=LoggingSite_Event_Msg_1}You got {yieldHardwood} hardwood.")
                .SetTextVariable("yieldHardwood", yieldHardwood)
                .ToString();
            
            var eventMsg2 =new TextObject(
                    "{=LoggingSite_Event_Msg_2}You got {yieldHardwood} hardwood. You hoped for more.")
                .SetTextVariable("yieldHardwood", yieldHardwood)
                .ToString();
            
            var eventMsg3 =new TextObject(
                    "{=LoggingSite_Event_Msg_3}You got {yieldHardwood} hardwood. Awesome!")
                .SetTextVariable("yieldHardwood", yieldHardwood)
                .ToString();

            
            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventDescription, true, false, eventButtonText, null, null, null), true);
            
            MobileParty.MainParty.ItemRoster.AddToCounts(hardwood, yieldHardwood);
            
            if (yieldHardwood > 25 && yieldHardwood <= 35)
            {
                InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOutcome1, true, false, eventButtonText, null, null, null), true);
                InformationManager.DisplayMessage(new InformationMessage(eventMsg1, RandomEventsSubmodule.Msg_Color_POS_Outcome));
            }
            else if (yieldHardwood > 35 && yieldHardwood <= 50)
            {
                InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOutcome2, true, false, eventButtonText, null, null, null), true);
                InformationManager.DisplayMessage(new InformationMessage(eventMsg2, RandomEventsSubmodule.Msg_Color_POS_Outcome));
            }
            else if (yieldHardwood > 50)
            {
                InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOutcome3, true, false, eventButtonText, null, null, null), true);
                InformationManager.DisplayMessage(new InformationMessage(eventMsg3, RandomEventsSubmodule.Msg_Color_POS_Outcome));
            }
            

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


    public class LoggingSiteData : RandomEventData
    {

        public LoggingSiteData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
        {
        }

        public override BaseEvent GetBaseEvent()
        {
            return new LoggingSite();
        }
    }
}