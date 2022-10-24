using System;
using System.Windows;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Library;

namespace CryingBuffalo.RandomEvents.Events
{
	public sealed class PerfectWeather : BaseEvent
	{
		private readonly int moraleGain;

		public PerfectWeather() : base(Settings.Settings.RandomEvents.PerfectWeatherData)
		{
			moraleGain = Settings.Settings.RandomEvents.PerfectWeatherData.moraleGain;
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
			MobileParty.MainParty.RecentEventsMorale += moraleGain;
			MobileParty.MainParty.MoraleExplained.Add(moraleGain, new TaleWorlds.Localization.TextObject("Random Event"));

			InformationManager.ShowInquiry(
				new InquiryData("Perfect Weather",
					"The weather today is so perfect that everyone relaxes and the mood improves!",
					true,
					false,
					"Done",
					null,
					null,
					null
					),
				true);

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
