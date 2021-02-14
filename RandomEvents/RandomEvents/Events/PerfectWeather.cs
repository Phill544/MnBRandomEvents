using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace CryingBuffalo.RandomEvents.Events
{
	public class PerfectWeather : BaseEvent
	{
		private int moraleGain;

		public PerfectWeather() : base(Settings.RandomEvents.PerfectWeatherData)
		{
			moraleGain = Settings.RandomEvents.PerfectWeatherData.moraleGain;
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
					$"The weather today is so perfect that everyone relaxes and the mood improves!",
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

		public override void StopEvent()
		{
			try
			{
				OnEventCompleted.Invoke();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error while stopping \"{this.RandomEventData.EventType}\" event :\n\n {ex.Message} \n\n { ex.StackTrace}");
			}
		}
	}

	public class PerfectWeatherData : RandomEventData
	{
		public int moraleGain;

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
