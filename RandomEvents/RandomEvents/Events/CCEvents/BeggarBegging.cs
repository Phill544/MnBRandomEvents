using System;
using System.Windows;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace CryingBuffalo.RandomEvents.Events.CCEvents
{
    public class BeggarBegging : BaseEvent
    {
        private const string EventTitle = "Spare a coin";
        
        private readonly int minGoldToGive;
        private readonly int maxGoldToGive;
        private readonly int minRenownGain;
        private readonly int maxRenownGain;


        public BeggarBegging() : base(Settings.ModSettings.RandomEvents.BeggarBeggingData)
        {
            minGoldToGive = Settings.ModSettings.RandomEvents.BeggarBeggingData.minGoldToGive;
            maxGoldToGive = Settings.ModSettings.RandomEvents.BeggarBeggingData.maxGoldToGive;
            minRenownGain = Settings.ModSettings.RandomEvents.BeggarBeggingData.minRenownGain;
            maxRenownGain = Settings.ModSettings.RandomEvents.BeggarBeggingData.maxRenownGain;
        }

        public override void CancelEvent()
        {
        }

        public override bool CanExecuteEvent()
        {
            
            return MobileParty.MainParty.CurrentSettlement != null && (MobileParty.MainParty.CurrentSettlement.IsTown || MobileParty.MainParty.CurrentSettlement.IsVillage);
        }

        public override void StartEvent()
        {
            if (Settings.ModSettings.GeneralSettings.DebugMode)
            {
                InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}",
                    RandomEventsSubmodule.TextColor));
            }

            var goldToGive = MBRandom.RandomInt(minGoldToGive, maxGoldToGive);
            var renownGain = MBRandom.RandomInt(minRenownGain, maxRenownGain);

            var currentSettlement = MobileParty.MainParty.CurrentSettlement.Name;

            InformationManager.ShowInquiry(
                new InquiryData(EventTitle,
                $"While you are relaxing in {currentSettlement} you are approached by a beggar who asks if you can spare any gold. You check your pockets to see if you have anything to spare.",
                    true,
                    false,
                    "Continue",
                    null,
                    null,
                    null
                ),
                true);

            if (goldToGive == 0)
            {
                InformationManager.ShowInquiry(
                    new InquiryData(EventTitle+" part II",
                        "You inform the beggar that you currently don't have any gold on you. The beggar thanks you for your time and leaves.",
                        true,
                        false,
                        "Continue",
                        null,
                        null,
                        null
                    ),
                    true);
            }
            else if (goldToGive > 0 && goldToGive <= 15)
            {
                InformationManager.ShowInquiry(
                    new InquiryData(EventTitle+" part II",
                        $"You hand over {goldToGive} gold to the beggar. He thanks you and move on.",
                        true,
                        false,
                        "Continue",
                        null,
                        null,
                        null
                    ),
                    true);

                Hero.MainHero.Clan.Renown += renownGain - 5;
            }
            else if (goldToGive > 15 && goldToGive <= 35)
            {
                InformationManager.ShowInquiry(
                    new InquiryData(EventTitle+" part II",
                        $"You hand over {goldToGive} gold to the beggar. He shakes your hand and kisses it. He is truly thankful.",
                        true,
                        false,
                        "Continue",
                        null,
                        null,
                        null
                    ),
                    true);
                Hero.MainHero.Clan.Renown += renownGain - 2;
            }
            else if (goldToGive > 35 && goldToGive <= maxGoldToGive)
            {
                InformationManager.ShowInquiry(
                    new InquiryData(EventTitle+" part II",
                        $"You hand over {goldToGive} gold to the beggar. He drops to his knees and kisses your shoes. You tell him to get himself som food.",
                        true,
                        false,
                        "Continue",
                        null,
                        null,
                        null
                    ),
                    true);
                Hero.MainHero.Clan.Renown += renownGain;
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


    public class BeggarBeggingData : RandomEventData
    {
        public readonly int minGoldToGive;
        public readonly int maxGoldToGive;
        public readonly int minRenownGain;
        public readonly int maxRenownGain;

        public BeggarBeggingData(string eventType, float chanceWeight, int minGoldToGive, int maxGoldToGive, int minRenownGain, int maxRenownGain) : base(eventType,
            chanceWeight)
        {
            this.minGoldToGive = minGoldToGive;
            this.maxGoldToGive = maxGoldToGive;
            this.minRenownGain = minRenownGain;
            this.maxRenownGain = maxRenownGain;
        }

        public override BaseEvent GetBaseEvent()
        {
            return new BeggarBegging();
        }
    }
}