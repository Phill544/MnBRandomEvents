using System;
using System.Linq;
using System.Windows;
using Bannerlord.RandomEvents.Helpers;
using Bannerlord.RandomEvents.Settings;
using Ini.Net;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace Bannerlord.RandomEvents.Events
{
	internal sealed class BumperCrop : BaseEvent
	{
		private readonly bool eventDisabled;
		private readonly float cropGainPercent;

		public BumperCrop() : base(ModSettings.RandomEvents.BumperCropData)
		{
			var ConfigFile = new IniFile(ParseIniFile.GetTheConfigFile());
            
			eventDisabled = ConfigFile.ReadBoolean("BumperCrop", "EventDisabled");
			cropGainPercent = ConfigFile.ReadFloat("BumperCrop", "CropGainPercent");
		}
		
		public override void CancelEvent()
		{
		}

		private bool HasValidEventData()
		{
			if (eventDisabled == false)
			{
				if (cropGainPercent != 0)
				{
					return true;
				}
			}
            
			return false;
		}
		
		public override bool CanExecuteEvent()
		{
			return HasValidEventData() && Hero.MainHero.Clan.Settlements.Any();
		}

		public override void StartEvent()
		{
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
	}

	public class BumperCropData : RandomEventData
	{
		public BumperCropData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
		{
		}

		public override BaseEvent GetBaseEvent()
		{
			return new BumperCrop();
		}
	}
}
