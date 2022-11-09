using System;
using System.Windows;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace CryingBuffalo.RandomEvents.Events
{
	public sealed class PerfectWeather : BaseEvent
	{
		private readonly int moraleGain;

		public PerfectWeather() : base(Settings.ModSettings.RandomEvents.PerfectWeatherData)
		{
			moraleGain = Settings.ModSettings.RandomEvents.PerfectWeatherData.moraleGain;
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
				InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}", RandomEventsSubmodule.Dbg_Color));
			}
			
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
		public readonly int moraleGain;

		public PerfectWeatherData(string eventType, float chanceWeight, int moraleGain) : base(eventType, chanceWeight)
		{
			this.moraleGain = moraleGain;
		}

		public override BaseEvent GetBaseEvent()
		{
			return new PerfectWeather();
		}
	}
}
