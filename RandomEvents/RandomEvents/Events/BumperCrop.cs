﻿using System;
using System.Linq;
using System.Windows;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

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
			if (Settings.ModSettings.GeneralSettings.DebugMode)
			{
				InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}", RandomEventsSubmodule.Dbg_Color));
			}
			
			try
			{
				var eligibleSettlements = Hero.MainHero.Clan.Settlements.Where(s => s.IsTown || s.IsCastle).ToList();
				
				var index = MBRandom.RandomInt(0, eligibleSettlements.Count);
				
				var winningSettlement = eligibleSettlements[index];
				
				winningSettlement.Town.FoodStocks += MathF.Abs(winningSettlement.Town.FoodChange * cropGainPercent);
				
				var bumperSettlement = winningSettlement.Name.ToString();
				
				var eventTitle = new TextObject("{=BumperCrop_Title}Bumper Crop!").ToString();
			
				var eventOption1 = new TextObject("{=BumperCrop_Event_Text}You have been informed that {bumperSettlement} has had an excellent harvest!")
					.SetTextVariable("bumperSettlement", bumperSettlement)
					.ToString();
				
				var eventButtonText = new TextObject("{=BumperCrop_Event_Button_Text}Done").ToString();

				InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOption1, true, false, eventButtonText, null, null, null), true);
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
