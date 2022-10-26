using System;
using System.Windows;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

namespace CryingBuffalo.RandomEvents.Events.CCEvents
{
    public class HuntingTrip : BaseEvent
    {
        private const string EventTitle = "The Great Hunt";
        
        private readonly int minSoldiersToGo;
        private readonly int maxSoldiersToGo;
        private readonly int maxCatch;
        private readonly int minMoraleGain;
        private readonly int maxMoraleGain;
        private readonly int minYieldMultiplier;
        private readonly int maxYieldMultiplier;
        

        public HuntingTrip() : base(Settings.ModSettings.RandomEvents.HuntingTripData)
        {
            minSoldiersToGo = Settings.ModSettings.RandomEvents.HuntingTripData.minSoldiersToGo;
            maxSoldiersToGo = Settings.ModSettings.RandomEvents.HuntingTripData.maxSoldiersToGo;
            maxCatch = Settings.ModSettings.RandomEvents.HuntingTripData.maxCatch;
            minMoraleGain = Settings.ModSettings.RandomEvents.HuntingTripData.minMoraleGain;
            maxMoraleGain = Settings.ModSettings.RandomEvents.HuntingTripData.maxMoraleGain;
            minYieldMultiplier = Settings.ModSettings.RandomEvents.HuntingTripData.minYieldMultiplier;
            maxYieldMultiplier = Settings.ModSettings.RandomEvents.HuntingTripData.maxYieldMultiplier;
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

            var yieldMultiplier = MBRandom.RandomInt(minYieldMultiplier, maxYieldMultiplier);

            var soldiersGoneHunting = MBRandom.RandomInt(minSoldiersToGo, maxSoldiersToGo);
            var animalsCaught = MBRandom.RandomInt(0, maxCatch);
            
            var yieldedMeatResources = animalsCaught * yieldMultiplier;

            var moraleGained = MBRandom.RandomInt(minMoraleGain, maxMoraleGain);
            
            ItemObject meat = MBObjectManager.Instance.GetObject<ItemObject>("meat");
            ItemObject hides = MBObjectManager.Instance.GetObject<ItemObject>("hides");
            

            InformationManager.ShowInquiry(
                new InquiryData(EventTitle,
                $"While your party has found a nice spot to camp for the day, {soldiersGoneHunting} of your men decide they want to go into the forest just west of camp to try hunting.\n " +
                "You could always use the additional resources and it would be a great morale booster for the party if they catch some. You tell them to be back before nightfall.",
                    true,
                    false,
                    "Continue",
                    null,
                    null,
                    null
                ),
                true);
            
            MobileParty.MainParty.ItemRoster.AddToCounts(meat, animalsCaught * 3);
            MobileParty.MainParty.ItemRoster.AddToCounts(hides, animalsCaught);

            if (animalsCaught == 0)
            {
                InformationManager.ShowInquiry(
                    new InquiryData(EventTitle+" part II",
                        "Your men return empty handed just before nightfall. At least they had a good time together.",
                        true,
                        false,
                        "Continue",
                        null,
                        null,
                        null
                    ),
                    true);
            }
            else if (animalsCaught > 0 && animalsCaught <= 5)
            {
                InformationManager.ShowInquiry(
                    new InquiryData(EventTitle+" part II",
                        $"Your hunters return just before nightfall, having successfully caught {animalsCaught} animals, yielding to {animalsCaught} pieces of hide and {yieldedMeatResources} pieces of meat. Better than nothing. " +
                        "You let the hunters finish butchering the animals.",
                        true,
                        false,
                        "Continue",
                        null,
                        null,
                        null
                    ),
                    true);
                MobileParty.MainParty.RecentEventsMorale += moraleGained - 2;
                MobileParty.MainParty.MoraleExplained.Add(moraleGained, new TaleWorlds.Localization.TextObject("Random Event"));
            }
            else if (animalsCaught > 5 && animalsCaught <= 15)
            {
                InformationManager.ShowInquiry(
                    new InquiryData(EventTitle+" part II",
                        $"Your hunters return just before nightfall, having successfully caught {animalsCaught} animals, yielding to {animalsCaught} pieces of hide and {yieldedMeatResources} pieces of meat. " +
                        "You join the hunters in storing the meat.",
                        true,
                        false,
                        "Continue",
                        null,
                        null,
                        null
                    ),
                    true);
                MobileParty.MainParty.RecentEventsMorale += moraleGained - 3;
                MobileParty.MainParty.MoraleExplained.Add(moraleGained, new TaleWorlds.Localization.TextObject("Random Event"));
            }
            else if (animalsCaught > 15 && animalsCaught <= maxCatch)
            {
                InformationManager.ShowInquiry(
                    new InquiryData(EventTitle+" part II",
                        $"Your hunters return triumphantly just before nightfall, having successfully caught {animalsCaught} animals, yielding to {animalsCaught} pieces of hide and {yieldedMeatResources} pieces of meat. " +
                        "You order your men to start preparing a feast for everyone.",
                        true,
                        false,
                        "Continue",
                        null,
                        null,
                        null
                    ),
                    true);
                MobileParty.MainParty.RecentEventsMorale += moraleGained;
                MobileParty.MainParty.MoraleExplained.Add(moraleGained, new TaleWorlds.Localization.TextObject("Random Event"));
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


    public class HuntingTripData : RandomEventData
    {
        public readonly int minSoldiersToGo;
        public readonly int maxSoldiersToGo;
        public readonly int maxCatch;
        public readonly int minMoraleGain;
        public readonly int maxMoraleGain;
        public readonly int minYieldMultiplier;
        public readonly int maxYieldMultiplier;

        public HuntingTripData(string eventType, float chanceWeight, int minSoldiersToGo, int maxSoldiersToGo, int maxCatch, int minMoraleGain, int maxMoraleGain, int minYieldMultiplier, int maxYieldMultiplier) : base(eventType,
            chanceWeight)
        {
            this.minSoldiersToGo = minSoldiersToGo;
            this.maxSoldiersToGo = maxSoldiersToGo;
            this.maxCatch = maxCatch;
            this.minMoraleGain = minMoraleGain;
            this.maxMoraleGain = maxMoraleGain;
            this.minYieldMultiplier = minYieldMultiplier;
            this.maxYieldMultiplier = maxYieldMultiplier;
        }

        public override BaseEvent GetBaseEvent()
        {
            return new HuntingTrip();
        }
    }
}