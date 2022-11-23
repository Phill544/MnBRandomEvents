using System;
using System.Windows;
using CryingBuffalo.RandomEvents.Settings.MCM;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace CryingBuffalo.RandomEvents.Events.BicEvents
{
	public sealed class BirdSongs : BaseEvent
	{
		private readonly int minMoraleGain;
		private readonly int maxMoraleGain;

        

		public BirdSongs() : base(Settings.ModSettings.RandomEvents.BirdSongsData)
		{
			minMoraleGain = MCM_MenuConfig_A_M.Instance.BS_minMoraleGain;
			maxMoraleGain = MCM_MenuConfig_A_M.Instance.BS_maxMoraleGain;

		}

		public override void CancelEvent()
		{
        
		}

		public override bool CanExecuteEvent()
		{
			return MCM_MenuConfig_A_M.Instance.BS_Disable == false && MobileParty.MainParty.CurrentSettlement == null && CampaignTime.Now.IsDayTime;
		}

		public override void StartEvent()
		{
			if (MCM_ConfigMenu_General.Instance.GS_DebugMode)
			{
				InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}", RandomEventsSubmodule.Dbg_Color));
			}

			var moraleGain = MBRandom.RandomInt(minMoraleGain, maxMoraleGain);

			MobileParty.MainParty.RecentEventsMorale += moraleGain;
			MobileParty.MainParty.MoraleExplained.Add(moraleGain);

			var eventTitle = new TextObject("{=BirdSongs_Title}Bird Songs").ToString();
			
			var eventOption1 = new TextObject("{=BirdSongs_Event_Text}This day has been blessed by the beautiful melodies of birds singing songs. Silence falls over your ranks as the relaxing sounds of nature's choir bring a sense of joy to your men. This will surely boost their morale.")
				.ToString();
				
			var eventButtonText = new TextObject("{=BirdSongs_Event_Button_Text}Done").ToString();

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

	public class BirdSongsData : RandomEventData
	{

		public BirdSongsData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
		{

		}

		public override BaseEvent GetBaseEvent()
		{
			return new BirdSongs();
		}
	}
}
