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
					new InquiryData("Actions réussies!",
						$"Certaines de vos actions ont atteint d'autres membres du royaume.",
						true,
						false,
						"Terminé",
						null,
						null,
						null
						),
					true);

				StopEvent();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Erreur lors de la lecture \"{this.RandomEventData.EventType}\" event :\n\n {ex.Message} \n\n { ex.StackTrace}");
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
				MessageBox.Show($"Erreur lors de l'arrêt \"{this.RandomEventData.EventType}\" event :\n\n {ex.Message} \n\n { ex.StackTrace}");
			}
		}
	}

	public class SuccessfulDeedsData : RandomEventData
	{
		public float influenceGain;

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
