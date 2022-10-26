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
	internal sealed class BumperCrop : BaseEvent
	{
		private readonly float cropGainPercent;

		public BumperCrop() : base(Settings.ModSettings.RandomEvents.BumperCropData)
		{
			cropGainPercent = Settings.ModSettings.RandomEvents.BumperCropData.cropGainPercent;
		}

		public override void StartEvent()
		{
			try
			{
				// The list of settlements that are able to have food added to them
				var eligibleSettlements = Hero.MainHero.Clan.Settlements.Where(s => s.IsTown || s.IsCastle).ToList();

				// Randomly pick one of the eligible settlements
				var index = MBRandom.RandomInt(0, eligibleSettlements.Count);

				// Grab the winning settlement and add food to it
				var winningSettlement = eligibleSettlements[index];
				
				winningSettlement.Town.FoodStocks += MathF.Abs(winningSettlement.Town.FoodChange * cropGainPercent);

				// set the name to display
				var bumperSettlement = winningSettlement.Name.ToString();

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
				MessageBox.Show($"Error while running \"{randomEventData.eventType}\" event :\n\n {ex.Message} \n\n { ex.StackTrace}");
			}

			StopEvent();
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

		public override void CancelEvent()
		{
		}

		public override bool CanExecuteEvent()
		{
			return Hero.MainHero.Clan.Settlements.Any();
		}
	}

	public class BumperCropData : RandomEventData
	{
		public readonly float cropGainPercent;

		public BumperCropData(string eventType, float chanceWeight, float cropGainPercent) : base(eventType, chanceWeight)
		{
			this.cropGainPercent = cropGainPercent;
		}

		public override BaseEvent GetBaseEvent()
		{
			return new BumperCrop();
		}
	}
}
