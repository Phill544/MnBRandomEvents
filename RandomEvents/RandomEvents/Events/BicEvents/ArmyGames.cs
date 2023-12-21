using System;
using System.Windows;
using Bannerlord.RandomEvents.Helpers;
using Bannerlord.RandomEvents.Settings;
using Ini.Net;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace Bannerlord.RandomEvents.Events.BicEvents
{
	public sealed class ArmyGames : BaseEvent
	{
		private readonly bool eventDisabled;
		private readonly float minCohesionIncrease;
		private readonly float maxCohesionIncrease;
		private readonly int minMoraleGain;
		private readonly int maxMoraleGain;


		public ArmyGames() : base(ModSettings.RandomEvents.ArmyGamesData)
		{
			var ConfigFile = new IniFile(ParseIniFile.GetTheConfigFile());
            
			eventDisabled = ConfigFile.ReadBoolean("ArmyGames", "EventDisabled");
			minCohesionIncrease = ConfigFile.ReadFloat("ArmyGames", "MinCohesionIncrease");
			maxCohesionIncrease = ConfigFile.ReadFloat("ArmyGames", "MaxCohesionIncrease");
			minMoraleGain = ConfigFile.ReadInteger("ArmyGames", "MinMoraleGain");
			maxMoraleGain = ConfigFile.ReadInteger("ArmyGames", "MaxMoraleGain");
		}

		public override void CancelEvent()
		{
		}
		
		private bool HasValidEventData()
		{
			if (eventDisabled == false)
			{
				if (minCohesionIncrease != 0 || maxCohesionIncrease != 0 || minMoraleGain != 0 || maxMoraleGain != 0)
				{
					return true;
				}
			}
			return false;
		}

		public override bool CanExecuteEvent()
		{
			return HasValidEventData() && MobileParty.MainParty.Army != null && MobileParty.MainParty.Army.LeaderPartyAndAttachedPartiesCount > 2; 
		}

		public override void StartEvent()
		{
			try
			{
				var ArmyLeader = MobileParty.MainParty.Army.ArmyOwner;

				var cohesionIncrease = MBRandom.RandomFloatRanged(minCohesionIncrease, maxCohesionIncrease);
				
				MobileParty.MainParty.Army.Cohesion += cohesionIncrease;
				
				var moraleGain = MBRandom.RandomInt(minMoraleGain, maxMoraleGain);
				
				MobileParty.MainParty.RecentEventsMorale += moraleGain;
				MobileParty.MainParty.MoraleExplained.Add(moraleGain);

				var eventTitle = new TextObject("{=ArmyGames_Title}Army Games").ToString();
			
				var eventDesc = new TextObject("{=ArmyGames_Event_Text}What started as a small competition has evolved itself into a full blown tournament. {leader} has gathered all of the parties in this army for competitions to see who " +
					"is the ultimate champion team. The men haven't had this much fun in quite a long time, this will surely boost morale and cohesion.")
					.SetTextVariable("leader", ArmyLeader.Name.ToString())
					.ToString();
					
				
				var eventButtonText = new TextObject("{=ArmyGames_Event_Button_Text}Done").ToString();

				InformationManager.ShowInquiry(new InquiryData(eventTitle, eventDesc, true, false, eventButtonText, null, null, null), true);
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

	public class ArmyGamesData : RandomEventData
	{

		public ArmyGamesData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
		{
		}

		public override BaseEvent GetBaseEvent()
		{
			return new ArmyGames();
		}
	}
}
