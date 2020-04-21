using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace CryingBuffalo.RandomEvents.Events
{
	class BumperCrop : BaseEvent
	{
		private int cropGainAmount;

		public BumperCrop()
		{
			cropGainAmount = Settings.RandomEvents.BumperCropData.cropGainAmount;
		}

		public override void StartEvent()
		{
			try
			{
				// The name of the settlement that receives the food
				string bumperSettlement = "";

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

				// Randmly pick one of the eligible settlements
				int index = MBRandom.RandomInt(0, eligibleSettlements.Count);

				// Grab the winning settlement and add food to it
				Settlement winningSettlement = eligibleSettlements[index];
				winningSettlement.Town.FoodStocks += cropGainAmount;

				// set the name to display
				bumperSettlement = winningSettlement.Name.ToString();

				InformationManager.ShowInquiry(
					new InquiryData("Bumper Crop!",
									$"You have been informed that {bumperSettlement} has had an excellent harvest!",
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
				MessageBox.Show($"Error while running \"{nameof(Settings.RandomEvents.BumperCropData.EventType)}\" event :\n\n {ex.Message} \n\n { ex.StackTrace}");
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
				MessageBox.Show($"Error while stopping \"{nameof(Settings.RandomEvents.BumperCropData.EventType)}\" event :\n\n {ex.Message} \n\n { ex.StackTrace}");
			}
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
	}

	public class BumperCropData : RandomEventData
	{
		public int cropGainAmount;

		public BumperCropData(RandomEventType eventType, float chanceWeight, int cropGainAmount) : base(eventType, chanceWeight)
		{
			this.cropGainAmount = cropGainAmount;
		}
	}
}
