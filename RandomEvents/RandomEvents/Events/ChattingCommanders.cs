using System;
using System.Linq;
using System.Windows;
using CryingBuffalo.RandomEvents.Helpers;
using CryingBuffalo.RandomEvents.Settings;
using Ini.Net;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace CryingBuffalo.RandomEvents.Events
{
	public sealed class ChattingCommanders : BaseEvent
	{
		private readonly bool eventDisabled;
		private readonly float cohesionIncrease;

		public ChattingCommanders() : base(ModSettings.RandomEvents.ChattingCommandersData)
		{
			var ConfigFile = new IniFile(ParseIniFile.GetTheConfigFile());
            
			eventDisabled = ConfigFile.ReadBoolean("ChattingCommanders", "EventDisabled");
			cohesionIncrease = ConfigFile.ReadFloat("ChattingCommanders", "CohesionIncrease");
		}

		public override void CancelEvent()
		{
		}
		
		private bool HasValidEventData()
		{
			if (eventDisabled == false)
			{
				if (cohesionIncrease != 0)
				{
					return true;
				}
			}
            
			return false;
		}

		public override bool CanExecuteEvent()
		{
			return HasValidEventData() && MobileParty.MainParty.Army != null && MobileParty.MainParty.Army.ArmyOwner == Hero.MainHero && MobileParty.MainParty.Army.LeaderPartyAndAttachedParties.Count() > 1;
		}

		public override void StartEvent()
		{
			if (GeneralSettings.DebugMode.IsActive())
			{
				InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}", RandomEventsSubmodule.Dbg_Color));
			}

			try
			{
				MobileParty.MainParty.Army.Cohesion += cohesionIncrease;
				
				var eventTitle = new TextObject("{=ChattingCommanders_Title}The Same Page").ToString();
			
				var eventOption1 = new TextObject("{=ChattingCommanders_Event_Text}After a good chat with the commanders of your army, there is a noticeable increase cohesion.")
					.ToString();
				
				var eventButtonText = new TextObject("{=ChattingCommanders_Event_Button_Text}Done").ToString();

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

	public class ChattingCommandersData : RandomEventData
	{

		public ChattingCommandersData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
		{
		}

		public override BaseEvent GetBaseEvent()
		{
			return new ChattingCommanders();
		}
	}
}
