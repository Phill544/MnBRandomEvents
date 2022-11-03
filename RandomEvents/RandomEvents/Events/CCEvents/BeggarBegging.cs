using System;
using System.Windows;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace CryingBuffalo.RandomEvents.Events.CCEvents
{
    public class BeggarBegging : BaseEvent
    {
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
            
            var heroName = Hero.MainHero.FirstName;
            
            var eventTitle = new TextObject("{=BeggarBegging_Title}Spare a coin").ToString();

            var goldToGive = MBRandom.RandomInt(minGoldToGive, maxGoldToGive);
            var renownGain = MBRandom.RandomInt(minRenownGain, maxRenownGain);

            var currentSettlement = MobileParty.MainParty.CurrentSettlement.Name;
            
            var eventDescription = new TextObject(
                    "{=BeggarBegging_Event_Desc}While you are relaxing in {currentSettlement} you are approached by a beggar who asks if you can spare any gold. You check your pockets to see if you have anything to spare.")
                    .SetTextVariable("currentSettlement", currentSettlement)
                    .ToString();
            
            var eventOutcome1 = new TextObject("{=BeggarBegging_Outcome_1}You inform the beggar that you currently don't have any gold on you. The beggar thanks you for your time and leaves.")
                .ToString();
            
            var eventOutcome2 = new TextObject("{=BeggarBegging_Outcome_2}You hand over {goldToGive} gold to the beggar. He thanks you and move on.")
                .SetTextVariable("goldToGive",goldToGive)
                .ToString();
            
            var eventOutcome3 = new TextObject("{=BeggarBegging_Outcome_3}You hand over {goldToGive} gold to the beggar. He shakes your hand and kisses it. He is truly thankful.")
                .SetTextVariable("goldToGive",goldToGive)
                .ToString();
            
            var eventOutcome4 = new TextObject("{=BeggarBegging_Outcome_4}You hand over {goldToGive} gold to the beggar. He drops to his knees and kisses your shoes. You tell him to get himself some food.")
                .SetTextVariable("goldToGive",goldToGive)
                .ToString();
            
            var eventButtonText = new TextObject("{=BeggarBegging_Event_Button_Text}Continue")
                .ToString();
            
            var eventMsg1 =new TextObject(
                    "{=BeggarBegging_Event_Msg_1}{heroName} gave away {goldToGive} to the beggar and gained {renownGain} renown.")
                .SetTextVariable("heroName", heroName)
                .SetTextVariable("goldToGive", goldToGive)
                .SetTextVariable("renownGain", renownGain - 5)
                .ToString();
            
            var eventMsg2 =new TextObject(
                    "{=BeggarBegging_Event_Msg_2}{heroName} gave away {goldToGive} to the beggar and gained {renownGain} renown.")
                .SetTextVariable("heroName", heroName)
                .SetTextVariable("goldToGive", goldToGive)
                .SetTextVariable("renownGain", renownGain - 2)
                .ToString();
            
            var eventMsg3 =new TextObject(
                    "{=BeggarBegging_Event_Msg_2}{heroName} gave away {goldToGive} to the beggar and gained {renownGain} renown.")
                .SetTextVariable("heroName", heroName)
                .SetTextVariable("goldToGive", goldToGive)
                .SetTextVariable("renownGain", renownGain)
                .ToString();

            InformationManager.ShowInquiry(
                new InquiryData(eventTitle,
                    eventDescription,
                    true,
                    false,
                    eventButtonText,
                    null,
                    null,
                    null
                ),
                true);

            if (goldToGive == 0)
            {
                InformationManager.ShowInquiry(
                    new InquiryData(eventTitle,
                        eventOutcome1,
                        true,
                        false,
                        eventButtonText,
                        null,
                        null,
                        null
                    ),
                    true);
            }
            else if (goldToGive > 0 && goldToGive <= 15)
            {
                InformationManager.ShowInquiry(
                    new InquiryData(eventTitle,
                        eventOutcome2,
                        true,
                        false,
                        eventButtonText,
                        null,
                        null,
                        null
                    ),
                    true);

                Hero.MainHero.ChangeHeroGold(-goldToGive);
                Hero.MainHero.Clan.Renown += renownGain - 5;
                
                InformationManager.DisplayMessage(new InformationMessage(eventMsg1, RandomEventsSubmodule.MsgColor));
            }
            else if (goldToGive > 15 && goldToGive <= 35)
            {
                InformationManager.ShowInquiry(
                    new InquiryData(eventTitle,
                        eventOutcome3,
                        true,
                        false,
                        eventButtonText,
                        null,
                        null,
                        null
                    ),
                    true);
                Hero.MainHero.ChangeHeroGold(-goldToGive);
                Hero.MainHero.Clan.Renown += renownGain - 2;
                
                InformationManager.DisplayMessage(new InformationMessage(eventMsg2, RandomEventsSubmodule.MsgColor));
            }
            else if (goldToGive > 35 && goldToGive <= maxGoldToGive)
            {
                InformationManager.ShowInquiry(
                    new InquiryData(eventTitle,
                        eventOutcome4,
                        true,
                        false,
                        eventButtonText,
                        null,
                        null,
                        null
                    ),
                    true);
                Hero.MainHero.ChangeHeroGold(-goldToGive);
                Hero.MainHero.Clan.Renown += renownGain;
                
                InformationManager.DisplayMessage(new InformationMessage(eventMsg3, RandomEventsSubmodule.MsgColor));
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