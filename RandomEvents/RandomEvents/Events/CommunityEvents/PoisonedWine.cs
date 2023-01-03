using System;
using System.Windows;
using CryingBuffalo.RandomEvents.Settings;
using CryingBuffalo.RandomEvents.Settings.MCM;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace CryingBuffalo.RandomEvents.Events.CommunityEvents
{
    public class PoisonedWine : BaseEvent
    {
        private readonly int minSoldiersToDie;
        private readonly int maxSoldiersToDie;
        private readonly int minSoldiersToHurt;
        private readonly int maxSoldiersToHurt;


        public PoisonedWine() : base(ModSettings.RandomEvents.PoisonedWineData)
        {
            minSoldiersToDie = MCM_MenuConfig_P_Z.Instance.PoWi_MinSoldiersToDie;
            maxSoldiersToDie = MCM_MenuConfig_P_Z.Instance.PoWi_MaxSoldiersToDie;
            minSoldiersToHurt = MCM_MenuConfig_P_Z.Instance.PoWi_MinSoldiersToHurt;
            maxSoldiersToHurt = MCM_MenuConfig_P_Z.Instance.PoWi_MaxSoldiersToHurt;
        }

        public override void CancelEvent()
        {
        }

        public override bool CanExecuteEvent()
        {
            return MCM_MenuConfig_P_Z.Instance.PoWi_Disable == false && MobileParty.MainParty.MemberRoster.TotalRegulars >= maxSoldiersToDie && MobileParty.MainParty.CurrentSettlement == null;
        }

        public override void StartEvent()
        {
            if (MCM_ConfigMenu_General.Instance.GS_DebugMode)
            {
                InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}", RandomEventsSubmodule.Dbg_Color));
            }

            var heroName = Hero.MainHero.FirstName.ToString();

            var menKilled = MBRandom.RandomInt(minSoldiersToDie, maxSoldiersToDie);

            var menHurt = MBRandom.RandomInt(minSoldiersToHurt, maxSoldiersToHurt);
            
            var eventTitle = new TextObject("{=PoisonedWine_Title}Poisoned Wine").ToString();
            
            var eventText = new TextObject(
                    "{=PoisonedWine_Event_Text}Late in the night your decide to throw a party to raise troop morale. Drinks are overflowing, hunger is satiated, and some troops have started games such as arm " +
                    "wrestling and cards. Everything seems to be going as planned as the hours pass by into the night. Soon however, you wished you hadn't counted your chickens before they hatched.\n\n" +
                    "Your stomach starts churning and you hear a commotion on the other side of camp. Your men have started hurling the content of their stomach across the ground. Some are clutching their " +
                    "stomachs, and even a few are crying as they beg for it to stop. Your face goes pale as you realize, you've been poisoned.\n\n" +
                    "Questions rush through your head. Who could have done it? When? But whether it was because of all of the questions or the poison, your head begins to spin. Unfortunately, this was a moment " +
                    "you knew you were going to hate. The last thing you see before you join your troops with vomiting, is a handful of men, not moving. You only hope you and the rest of your men are not so unlucky.")
                .ToString();


            var eventButtonText = new TextObject("{=PoisonedWine_Event_Button_Text}Continue")
                .ToString();
            
            var eventMsg =new TextObject(
                    "{=PoisonedWine_Event_Msg}{heroName]lost {menKilled} men and {menHurt} men got sick after drinking poisoned wine.")
                .SetTextVariable("heroName", heroName)
                .SetTextVariable("menKilled", menKilled)
                .SetTextVariable("menHurt", menHurt)
                .ToString();
            
            

            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventText, true, false, eventButtonText, null, null, null), true);
             
            MobileParty.MainParty.MemberRoster.KillNumberOfMenRandomly(menKilled, false);
            MobileParty.MainParty.MemberRoster.WoundNumberOfTroopsRandomly(menHurt);
            
            InformationManager.DisplayMessage(new InformationMessage(eventMsg, RandomEventsSubmodule.Msg_Color_NEG_Outcome));


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


    public class PoisonedWineData : RandomEventData
    {

        public PoisonedWineData(string eventType, float chanceWeight) : base(eventType,
            chanceWeight)
        {
        }

        public override BaseEvent GetBaseEvent()
        {
            return new PoisonedWine();
        }
    }
}