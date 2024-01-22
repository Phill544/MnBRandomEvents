using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Bannerlord.RandomEvents.Helpers;
using Bannerlord.RandomEvents.Settings;
using Ini.Net;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace Bannerlord.RandomEvents.Events
{
	public sealed class AheadOfTime : BaseEvent
	{
		private List<Settlement> eligibleSettlements;
		private readonly bool eventDisabled;

		public AheadOfTime() : base(ModSettings.RandomEvents.AheadOfTimeData)
		{
			var ConfigFile = new IniFile(ParseIniFile.GetTheConfigFile());
            
			eventDisabled = ConfigFile.ReadBoolean("AheadOfTime", "EventDisabled");
		}

		public override void CancelEvent()
		{
		}
		
		private bool HasValidEventData()
		{
			return eventDisabled == false;
		}

		public override bool CanExecuteEvent()
		{

			if (HasValidEventData())
			{
				if (!Hero.MainHero.Clan.Settlements.Any()) return false;
				eligibleSettlements = new List<Settlement>();
				
				foreach (var s in Hero.MainHero.Clan.Settlements.Where(s => (s.IsTown || s.IsCastle) && s.Town.BuildingsInProgress.Count > 0))
				{
					eligibleSettlements.Add(s);
				}

				return eligibleSettlements.Count > 0;
			}

			return false;

		}

		public override void StartEvent()
		{	
			try
			{
				var eventTitle = new TextObject("{=AheadOfTime_Title}Ahead of Time!").ToString();

				var randomElement = MBRandom.RandomInt(eligibleSettlements.Count);
				var settlement = eligibleSettlements[randomElement];

				settlement.Town.CurrentBuilding.BuildingProgress += settlement.Town.CurrentBuilding.GetConstructionCost() - settlement.Town.CurrentBuilding.BuildingProgress;
				settlement.Town.CurrentBuilding.LevelUp();
				settlement.Town.BuildingsInProgress.Dequeue();

				var eventText =new TextObject(
						"{=AheadOfTimeEvent_Text}You receive word that {settlement} has completed its current project earlier than expected.")
					.SetTextVariable("settlement", settlement.ToString())
					.ToString();
				
				var eventButtonText1 = new TextObject("{=AheadOfTimeEvent_Event_Button_Text_1}Done").ToString();

				InformationManager.ShowInquiry(new InquiryData(eventTitle, eventText, true, false, eventButtonText1, null, null, null), true);

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
