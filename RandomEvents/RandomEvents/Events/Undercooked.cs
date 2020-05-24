using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace CryingBuffalo.RandomEvents.Events
{
	public class Undercooked : BaseEvent
	{
		private int minTroopsToInjure;
		private int maxTroopsToInjure;

		public Undercooked() : base(Settings.RandomEvents.UndercookedData)
		{
			minTroopsToInjure = Settings.RandomEvents.UndercookedData.minTroopsToInjure;
			maxTroopsToInjure = Settings.RandomEvents.UndercookedData.maxTroopsToInjure;
		}

		public override void CancelEvent()
		{
		}

		public override bool CanExecuteEvent()
		{
			return (MobileParty.MainParty.MemberRoster.TotalRegulars - MobileParty.MainParty.MemberRoster.TotalWoundedRegulars) >= minTroopsToInjure;
		}

		public override void StartEvent()
		{
			try
			{
				int numberToInjure = MBRandom.RandomInt(minTroopsToInjure, maxTroopsToInjure);
				numberToInjure = Math.Min(numberToInjure, maxTroopsToInjure);

				MobileParty.MainParty.MemberRoster.WoundNumberOfTroopsRandomly(numberToInjure);

				InformationManager.ShowInquiry(
					new InquiryData("Undercooked",
						$"Certaines de vos troupes tombent malades à cause de la mauvaise nourriture, bien que vous ne soyez pas sûr de la cause, vous êtes content que ce ne soit pas vous.",
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

	public class UndercookedData : RandomEventData
	{
		public int minTroopsToInjure;
		public int maxTroopsToInjure;

		public UndercookedData(string eventType, float chanceWeight, int minTroopsToInjure, int maxTroopsToInjure) : base(eventType, chanceWeight)
		{
			this.minTroopsToInjure = minTroopsToInjure;
			this.maxTroopsToInjure = maxTroopsToInjure;
		}

		public override BaseEvent GetBaseEvent()
		{
			return new Undercooked();
		}
	}
}
