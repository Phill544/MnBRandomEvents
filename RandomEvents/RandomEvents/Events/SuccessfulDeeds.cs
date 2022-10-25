using System;
using System.Windows;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Library;

namespace CryingBuffalo.RandomEvents.Events
{
	public sealed class SuccessfulDeeds : BaseEvent
	{
		private readonly float influenceGain;

		public SuccessfulDeeds() : base(Settings.ModSettings.RandomEvents.SuccessfulDeedsData)
		{
			influenceGain = Settings.ModSettings.RandomEvents.SuccessfulDeedsData.influenceGain;
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
						"Some of your deeds have reached other members of the kingdom.",
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
				MessageBox.Show($"Error while playing \"{randomEventData.eventType}\" event :\n\n {ex.Message} \n\n { ex.StackTrace}");
			}
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

	public class SuccessfulDeedsData : RandomEventData
	{
		public readonly float influenceGain;

		public SuccessfulDeedsData(string eventType, float chanceWeight, float influenceGain) : base(eventType, chanceWeight)
		{
			this.influenceGain = influenceGain;
		}

		public override BaseEvent GetBaseEvent()
		{
			return new SuccessfulDeeds();
		}
	}
}
