using System;
using System.Linq;
using System.Windows;
using CryingBuffalo.RandomEvents.Settings;
using CryingBuffalo.RandomEvents.Settings.MCM;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace CryingBuffalo.RandomEvents.Events.BicEvents
{
	public sealed class ArmyGames : BaseEvent
	{
		private readonly float minCohesionIncrease;
		private readonly float maxCohesionIncrease;
		private readonly int minMoraleGain;
		private readonly int maxMoraleGain;


		public ArmyGames() : base(ModSettings.RandomEvents.ArmyGamesData)
		{
			minCohesionIncrease = 10.0f;
			maxCohesionIncrease = 40.0f;
			minMoraleGain = 10;
			maxMoraleGain = 30;
		}

		public override void CancelEvent()
		{
		}

		public override bool CanExecuteEvent()
		{
			return MCM_MenuConfig_Toggle.Instance.AG_Disable == false && MobileParty.MainParty.Army != null && MobileParty.MainParty.Army.LeaderPartyAndAttachedParties.Count() > 2; 
		}

		public override void StartEvent()
		{
			if (MCM_ConfigMenu_General.Instance.GS_DebugMode)
			{
				InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}", RandomEventsSubmodule.Dbg_Color));
			}

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
