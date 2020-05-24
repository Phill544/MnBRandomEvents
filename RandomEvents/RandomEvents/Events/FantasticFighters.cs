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
	public class FantasticFighters : BaseEvent
	{
		private int renownGain;

		public FantasticFighters() : base(Settings.RandomEvents.FantasticFightersData)
		{
			renownGain = Settings.RandomEvents.FantasticFightersData.renownGain;
		}

		public override void CancelEvent()
		{
		}

		public override bool CanExecuteEvent()
		{
			return Hero.MainHero.Clan != null;
		}

		public override void StartEvent()
		{
			try
			{
				Hero.MainHero.Clan.Renown += renownGain;

				InformationManager.ShowInquiry(
					new InquiryData("Combattants Fantastiques?",
						$"Une rumeur se répand que votre clan a réussi à remporter une bataille de manière décisive lorsqu'il a été en infériorité numérique à 10 contre 1.",
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

	public class FantasticFightersData : RandomEventData
	{
		public int renownGain;

		public FantasticFightersData(string eventType, float chanceWeight, int renownGain) : base(eventType, chanceWeight)
		{
			this.renownGain = renownGain;
		}

		public override BaseEvent GetBaseEvent()
		{
			return new FantasticFighters();
		}
	}
}
