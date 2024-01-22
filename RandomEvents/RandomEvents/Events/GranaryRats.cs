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
	public sealed class GranaryRats : BaseEvent
	{ 
		private readonly bool eventDisabled;
		private readonly float MinFoodLossPercent;
		private readonly float MaxFoodLossPercent;

		public GranaryRats() : base(ModSettings.RandomEvents.GranaryRatsData)
		{
			var ConfigFile = new IniFile(ParseIniFile.GetTheConfigFile());
            
			eventDisabled = ConfigFile.ReadBoolean("GranaryRats", "EventDisabled");
			MinFoodLossPercent = ConfigFile.ReadFloat("GranaryRats", "MinFoodLossPercent");
			MaxFoodLossPercent = ConfigFile.ReadFloat("GranaryRats", "MaxFoodLossPercent");
		}

		public override void CancelEvent()
		{
		}
		
		private bool HasValidEventData()
		{
			if (eventDisabled == false)
			{
				if (MinFoodLossPercent != 0 || MaxFoodLossPercent != 0)
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

				var foodLossPercent = MBRandom.RandomFloatRanged(MinFoodLossPercent, MaxFoodLossPercent);
				
				var eligibleSettlements = Hero.MainHero.Clan.Settlements.Where(s => s.IsTown || s.IsCastle).ToList();
				
				var index = MBRandom.RandomInt(0, eligibleSettlements.Count);
				
				var infestedSettlement = eligibleSettlements[index];

				infestedSettlement.Town.FoodStocks -= MathF.Abs(infestedSettlement.Town.FoodStocks * foodLossPercent);
				
				var ratSettlement = infestedSettlement.Name.ToString();
				
				var eventTitle = new TextObject("{=GranaryRats_Title}Rats in the granary!").ToString();
			
				var eventOption1 = new TextObject("{=GranaryRats_Event_Text}You have been informed that {ratSettlement} had an infestation of rats that went unchecked... The rats won't starve this month, but your peasants might.")
					.SetTextVariable("ratSettlement", ratSettlement)
					.ToString();
				
				var eventMsg1 =new TextObject(
						"{=GranaryRats_Event_Msg_1}{ratSettlement} lost {foodLossPercent}% of it's food to rats in the granary.")
					.SetTextVariable("ratSettlement", ratSettlement)
					.SetTextVariable("foodLossPercent", (int)(foodLossPercent*100))
					.ToString();
				
				var eventButtonText = new TextObject("{=GranaryRats_Event_Button_Text}Done").ToString();

				InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOption1, true, false, eventButtonText, null, null, null), true);
				
				InformationManager.DisplayMessage(new InformationMessage(eventMsg1, RandomEventsSubmodule.Msg_Color));
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

	public class GranaryRatsData : RandomEventData
	{

		public GranaryRatsData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
		{
		}

		public override BaseEvent GetBaseEvent()
		{
			return new GranaryRats();
		}
	}
}
