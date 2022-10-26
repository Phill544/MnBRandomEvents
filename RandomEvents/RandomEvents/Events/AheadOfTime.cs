using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace CryingBuffalo.RandomEvents.Events
{
	public sealed class AheadOfTime : BaseEvent
	{
		private List<Settlement> eligibleSettlements;

		public AheadOfTime() : base(Settings.ModSettings.RandomEvents.AheadOfTimeData)
		{
		}

		public override void CancelEvent()
		{
		}

		public override bool CanExecuteEvent()
		{
			if (!Hero.MainHero.Clan.Settlements.Any()) return false;
			eligibleSettlements = new List<Settlement>();

			// Out of the settlements the main hero owns, only the towns or castles have food.
			foreach (var s in Hero.MainHero.Clan.Settlements.Where(s => (s.IsTown || s.IsCastle) && s.Town.BuildingsInProgress.Count > 0))
			{
				eligibleSettlements.Add(s);
			}

			return eligibleSettlements.Count > 0;

		}

		public override void StartEvent()
		{	
			try
			{

				var randomElement = MBRandom.RandomInt(eligibleSettlements.Count);
				var settlement = eligibleSettlements[randomElement];

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
