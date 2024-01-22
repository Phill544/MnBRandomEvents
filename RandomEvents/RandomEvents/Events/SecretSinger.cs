using System;
using System.Windows;
using Bannerlord.RandomEvents.Helpers;
using Bannerlord.RandomEvents.Settings;
using Ini.Net;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace Bannerlord.RandomEvents.Events
{
    public sealed class SecretSinger : BaseEvent
    {
        private readonly bool eventDisabled;
        private readonly int minMoraleGain;
        private readonly int maxMoraleGain;

        public SecretSinger() : base(ModSettings.RandomEvents.SecretSingerData)
        {
            var ConfigFile = new IniFile(ParseIniFile.GetTheConfigFile());
            
            eventDisabled = ConfigFile.ReadBoolean("SecretSinger", "EventDisabled");
            minMoraleGain = ConfigFile.ReadInteger("SecretSinger", "MinMoraleGain");
            maxMoraleGain = ConfigFile.ReadInteger("SecretSinger", "MaxMoraleGain");
        }

        public override void CancelEvent()
        {
        }
        
        private bool HasValidEventData()
        {
            if (eventDisabled == false)
            {
                if (minMoraleGain != 0 || maxMoraleGain != 0)
                {
                    return true;
                }
            }
            
            return false;
        }

        public override bool CanExecuteEvent()
        {
            return HasValidEventData();
        }

        public override void StartEvent()
        {
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
