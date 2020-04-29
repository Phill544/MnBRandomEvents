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
	public class GranaryRats : BaseEvent
	{
		private float foodLossPercent;

		public GranaryRats() : base(Settings.RandomEvents.GranaryRatsData)
		{
			foodLossPercent = Settings.RandomEvents.GranaryRatsData.foodLossPercent;
		}

		public override void CancelEvent()
		{
		}

		public override bool CanExecuteEvent()
		{
			if (Hero.MainHero.Clan.Settlements.Count() > 0)
			{
				return true;
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
				// The name of the settlement that receives the food
				string ratSettlement = "";

				// The list of settlements that are able to have food added to them
				List<Settlement> eligibleSettlements = new List<Settlement>();

				// Out of the settlements the main hero owns, only the towns or castles have food.
				foreach (Settlement s in Hero.MainHero.Clan.Settlements)
				{
					if (s.IsTown || s.IsCastle)
					{
						eligibleSettlements.Add(s);
					}
				}

				// Randomly pick one of the eligible settlements
				int index = MBRandom.RandomInt(0, eligibleSettlements.Count);

				// Grab the winning settlement and add food to it
				Settlement infestedSettlement = eligibleSettlements[index];

				infestedSettlement.Town.FoodStocks -= MathF.Abs(infestedSettlement.Town.FoodChange * foodLossPercent);

				// set the name to display
				ratSettlement = infestedSettlement.Name.ToString();

				InformationManager.ShowInquiry(
					new InquiryData("Rats in the granary!",
									$"You have been informed that {ratSettlement} had an infestation of rats that went unchecked... The rats won't starve this month, but your peasants might.",
									true,
									false,
									"Done",
									null,
									null,
									null
									), true);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error while running \"{this.RandomEventData.EventType}\" event :\n\n {ex.Message} \n\n { ex.StackTrace}");
			}

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

	public class GranaryRatsData : RandomEventData
	{
		public float foodLossPercent;

		public GranaryRatsData(RandomEventType eventType, float chanceWeight, float foodLossPercent) : base(eventType, chanceWeight)
		{
			this.foodLossPercent = foodLossPercent;
		}
	}
}
