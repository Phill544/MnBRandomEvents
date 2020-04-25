using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace CryingBuffalo.RandomEvents.Events
{
	class BumperCrop : BaseEvent
	{
		private float cropGainPercent;

		public BumperCrop()
		{
			cropGainPercent = Settings.RandomEvents.BumperCropData.cropGainPercent;
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

				// Randomly pick one of the eligible settlements
				int index = MBRandom.RandomInt(0, eligibleSettlements.Count);

				// Grab the winning settlement and add food to it
				Settlement winningSettlement = eligibleSettlements[index];
				
				winningSettlement.Town.FoodStocks += MathF.Abs(winningSettlement.Town.FoodChange * cropGainPercent);

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
				MessageBox.Show($"Error while running \"{Settings.RandomEvents.BumperCropData.EventType}\" event :\n\n {ex.Message} \n\n { ex.StackTrace}");
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
				MessageBox.Show($"Error while stopping \"{Settings.RandomEvents.BumperCropData.EventType}\" event :\n\n {ex.Message} \n\n { ex.StackTrace}");
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
		public float cropGainPercent;

		public BumperCropData(RandomEventType eventType, float chanceWeight, float cropGainPercent) : base(eventType, chanceWeight)
		{
			this.cropGainPercent = cropGainPercent;
		}
	}
}
