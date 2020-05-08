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
	public class SuccessfulDeeds : BaseEvent
	{
		private float influenceGain;

		public SuccessfulDeeds() : base(Settings.RandomEvents.SuccessfulDeedsData)
		{
			this.influenceGain = Settings.RandomEvents.SuccessfulDeedsData.influenceGain;
		}

		public override void CancelEvent()
		{
		}

		public override bool CanExecuteEvent()
		{
			return Hero.MainHero.Clan.Kingdom != null;
		}

		public override void StartEvent()
		{
			try
			{
				Hero.MainHero.AddInfluenceWithKingdom(influenceGain);

				InformationManager.ShowInquiry(
					new InquiryData("Successful Deeds!",
						$"Some of your deeds have reached other members of the kingdom.",
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
			catch (Exception ex)
			{
				MessageBox.Show($"Error while playing \"{this.RandomEventData.EventType}\" event :\n\n {ex.Message} \n\n { ex.StackTrace}");
			}
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

	public class SuccessfulDeedsData : RandomEventData
	{
		public float influenceGain;

		public SuccessfulDeedsData(RandomEventType eventType, float chanceWeight, float influenceGain) : base(eventType, chanceWeight)
		{
			this.influenceGain = influenceGain;
		}
	}
}
