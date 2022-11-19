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
	public sealed class PerfectWeather : BaseEvent
	{
		private readonly int minMoraleGain;
		private readonly int maxMoraleGain;

		public PerfectWeather() : base(ModSettings.RandomEvents.PerfectWeatherData)
		{
			minMoraleGain = MCM_MenuConfig_N_Z.Instance.PW_MinMoraleGain;
			maxMoraleGain = MCM_MenuConfig_N_Z.Instance.PW_MaxMoraleGain;
		}

		public override void CancelEvent()
		{
		}

		public override bool CanExecuteEvent()
		{
			return MCM_MenuConfig_N_Z.Instance.PW_Disable == false && MobileParty.MainParty.CurrentSettlement == null;
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
			
			var eventTitle = new TextObject("{=PerfectWeather_Title}Perfect Weather").ToString();
			
			var eventOption1 = new TextObject("{=PerfectWeather_Event_Text}The weather today is so perfect that everyone relaxes and the mood improves!")
				.ToString();
				
			var eventButtonText = new TextObject("{=PerfectWeather_Event_Button_Text}Done").ToString();

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

	public class PerfectWeatherData : RandomEventData
	{

		public PerfectWeatherData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
		{
		}

		public override BaseEvent GetBaseEvent()
		{
			return new PerfectWeather();
		}
	}
}
