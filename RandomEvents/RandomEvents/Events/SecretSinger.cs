using System;
using System.Windows;
using CryingBuffalo.RandomEvents.Settings;
using CryingBuffalo.RandomEvents.Settings.MCM;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace CryingBuffalo.RandomEvents.Events
{
    public sealed class SecretSinger : BaseEvent
    {
        private readonly int minMoraleGain;
        private readonly int maxMoraleGain;

        public SecretSinger() : base(ModSettings.RandomEvents.SecretSingerData)
        {
            minMoraleGain = MCM_MenuConfig_N_Z.Instance.SS_MinMoraleGained;
            maxMoraleGain = MCM_MenuConfig_N_Z.Instance.SS_MaxMoraleGained;
        }

        public override void CancelEvent()
        {
        }

        public override bool CanExecuteEvent()
        {
            return MCM_MenuConfig_N_Z.Instance.SS_Disable == false;
        }

        public override void StartEvent()
        {
            if (MCM_ConfigMenu_General.Instance.GS_DebugMode)
            {
                InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}", RandomEventsSubmodule.Dbg_Color));
            }

            var moraleGain = MBRandom.RandomInt(minMoraleGain,maxMoraleGain);
			
            MobileParty.MainParty.RecentEventsMorale += moraleGain;
            MobileParty.MainParty.MoraleExplained.Add(moraleGain);
			
            var eventTitle = new TextObject("{=SecretSinger_Title}Secret Singer!").ToString();
			
            var eventOption1 = new TextObject("{=SecretSinger_Event_Text}You discover one of your party members is an extremely good singer!")
                .ToString();
				
            var eventButtonText = new TextObject("{=SecretSinger_Event_Button_Text}Done").ToString();

            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOption1, true, false, eventButtonText, null, null, null), true);

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
                MessageBox.Show($"Error while stopping \"{randomEventData.eventType}\" event :\n\n {ex.Message} \n\n { ex.StackTrace}");
            }
        }
    }

    public class SecretSingerData : RandomEventData
    {

        public SecretSingerData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
        {
        }

        public override BaseEvent GetBaseEvent()
        {
            return new SecretSinger();
        }
    }
}
