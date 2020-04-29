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
	public class SecretSinger : BaseEvent
	{
		private int moraleGain;

		public SecretSinger() : base(Settings.RandomEvents.SecretSingerData)
		{
			moraleGain = Settings.RandomEvents.SecretSingerData.moraleGain;
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
			MobileParty.MainParty.MoraleExplainer.AddLine("Random Event", moraleGain, StatExplainer.OperationType.Custom);

			InformationManager.ShowInquiry(
				new InquiryData("Secret Singer!",
					$"You discover one of your party members is an extremely good singer!",
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

	public class SecretSingerData : RandomEventData
	{
		public int moraleGain;

		public SecretSingerData(RandomEventType eventType, float chanceWeight, int moraleGain) : base(eventType, chanceWeight)
		{
			this.moraleGain = moraleGain;
		}
	}
}
