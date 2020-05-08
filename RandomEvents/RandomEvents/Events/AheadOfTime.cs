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
	public class AheadOfTime : BaseEvent
	{
		private List<Settlement> eligibleSettlements;

		public AheadOfTime() : base(Settings.RandomEvents.AheadOfTimeData)
		{
		}

		public override void CancelEvent()
		{
		}

		public override bool CanExecuteEvent()
		{
			if (Hero.MainHero.Clan.Settlements.Count() > 0)
			{
				eligibleSettlements = new List<Settlement>();

				// Out of the settlements the main hero owns, only the towns or castles have food.
				foreach (Settlement s in Hero.MainHero.Clan.Settlements)
				{
					if ((s.IsTown || s.IsCastle) && s.Town.BuildingsInProgress.Count > 0)
					{
						eligibleSettlements.Add(s);
					}
				}

				if (eligibleSettlements.Count > 0)
				{
					return true;
				}

				return false;
			}
			else
			{
				return false;
			}
		}

		public override void StartEvent()
		{	
			try
			{

				int randomElement = MBRandom.RandomInt(eligibleSettlements.Count);
				Settlement settlement = eligibleSettlements[randomElement];

				settlement.Town.CurrentBuilding.BuildingProgress += settlement.Town.CurrentBuilding.GetConstructionCost() - settlement.Town.CurrentBuilding.BuildingProgress;
				settlement.Town.CurrentBuilding.LevelUp();
				settlement.Town.BuildingsInProgress.Dequeue();

				InformationManager.ShowInquiry(
					new InquiryData("Ahead of Time!",
						$"You receive word that {settlement} has completed its current project earlier than expected.",
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

	public class AheadOfTimeData : RandomEventData
	{
		public AheadOfTimeData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
		{
		}

		public override BaseEvent GetBaseEvent()
		{
			return new AheadOfTime();
		}
	}
}
