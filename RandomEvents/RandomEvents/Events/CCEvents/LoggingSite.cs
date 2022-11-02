using System;
using System.Windows;
using CryingBuffalo.RandomEvents.Helpers;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

namespace CryingBuffalo.RandomEvents.Events.CCEvents
{
    public class LoggingSite : BaseEvent
    {
        private const string EventTitle = "The Hardwood Forest";


        private readonly int minSoldiersToGo;
        private readonly int maxSoldiersToGo;
        private readonly int minYield;
        private readonly int maxYield;
        

        public LoggingSite() : base(Settings.ModSettings.RandomEvents.LoggingSiteData)
        {
            minSoldiersToGo = Settings.ModSettings.RandomEvents.LoggingSiteData.minSoldiersToGo;
            maxSoldiersToGo = Settings.ModSettings.RandomEvents.LoggingSiteData.maxSoldiersToGo;
            minYield = Settings.ModSettings.RandomEvents.LoggingSiteData.minYield;
            maxYield = Settings.ModSettings.RandomEvents.LoggingSiteData.maxYield;
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
                InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}",
                    RandomEventsSubmodule.TextColor));
            }
            
            var closestSettlement = ClosestSettlements.GetClosestAny(MobileParty.MainParty);

            var soldiersGoneHunting = MBRandom.RandomInt(minSoldiersToGo, maxSoldiersToGo);
            
            var treesChopped = MBRandom.RandomInt(minYield, maxYield);

            var yieldHardwood = treesChopped * MBRandom.RandomInt(1, 5);
            
            ItemObject hardwood = MBObjectManager.Instance.GetObject<ItemObject>("hardwood");


            InformationManager.ShowInquiry(
                new InquiryData(EventTitle,
                $"While your party is traveling through the lands near {closestSettlement} you come across a forest rich in hardwood trees. You decide that it's time to stock up on hardwood so you order {soldiersGoneHunting} of your men " +
                "to get to work. The men agree that this is a good opportunity to get some resources so they do as you say without much complaint.\n" +
                "You don't really need much, just enough to start some smithing projects next time you come across a forge. The rest you can easily sell for a nice profit.",
                    true,
                    false,
                    "Continue",
                    null,
                    null,
                    null
                ),
                true);
            
            MobileParty.MainParty.ItemRoster.AddToCounts(hardwood, yieldHardwood);
            
            if (yieldHardwood > 5 && yieldHardwood <= 15)
            {
                InformationManager.ShowInquiry(
                    new InquiryData(EventTitle+" part II",
                        $"The logging crew return just as the sun is setting. In total they chopped down {treesChopped} trees which yielded {yieldHardwood} pieces of Hardwood.\n" +
                        "You admittedly had hoped for more resources than this so you berate your men for being lazy.",
                        true,
                        false,
                        "Continue",
                        null,
                        null,
                        null
                    ),
                    true);
            }
            else if (yieldHardwood > 15 && yieldHardwood <= 30)
            {
                InformationManager.ShowInquiry(
                    new InquiryData(EventTitle+" part II",
                        $"The logging crew return just as the sun is setting. In total they chopped down {treesChopped} trees which yielded {yieldHardwood} pieces of Hardwood.\n" +
                        "You had hoped for a better result but all in all this should be enough to cover your own projects.",
                        true,
                        false,
                        "Continue",
                        null,
                        null,
                        null
                    ),
                    true);
            }
            else if (yieldHardwood > 30 && yieldHardwood <= 50)
            {
                InformationManager.ShowInquiry(
                    new InquiryData(EventTitle+" part II",
                        $"The logging crew return just as the sun is setting. In total they chopped down {treesChopped} trees which yielded {yieldHardwood} pieces of Hardwood.\n" +
                        "This is the result you were hoping for! Enough for your own projects and enough to sell at a settlement for a nice profit. You congratulate your men on a hard days work.",
                        true,
                        false,
                        "Continue",
                        null,
                        null,
                        null
                    ),
                    true);
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
        public readonly int minSoldiersToGo;
        public readonly int maxSoldiersToGo;
        public readonly int minYield;
        public readonly int maxYield;

        public LoggingSiteData(string eventType, float chanceWeight, int minSoldiersToGo, int maxSoldiersToGo, int minYield, int maxYield) : base(eventType,
            chanceWeight)
        {
            this.minSoldiersToGo = minSoldiersToGo;
            this.maxSoldiersToGo = maxSoldiersToGo;
            this.minYield = minYield;
            this.maxYield = maxYield;
        }

        public override BaseEvent GetBaseEvent()
        {
            return new LoggingSite();
        }
    }
}