using System;
using System.Linq;
using System.Windows;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace CryingBuffalo.RandomEvents.Events
{
	public sealed class ChattingCommanders : BaseEvent
	{
		private const string EventTitle = "The Same Page";
		
		private readonly float cohesionIncrease;

		public ChattingCommanders() : base(Settings.ModSettings.RandomEvents.ChattingCommandersData)
		{
			cohesionIncrease = Settings.ModSettings.RandomEvents.ChattingCommandersData.cohesionIncrease;
		}

		public override void CancelEvent()
		{
		}

		public override bool CanExecuteEvent()
		{
			return MobileParty.MainParty.Army != null && MobileParty.MainParty.Army.ArmyOwner == Hero.MainHero && MobileParty.MainParty.Army.LeaderPartyAndAttachedParties.Count() > 1;
		}

		public override void StartEvent()
		{
			if (Settings.ModSettings.GeneralSettings.DebugMode)
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
		public readonly float cohesionIncrease;

		public ChattingCommandersData(string eventType, float chanceWeight, float cohesionIncrease) : base(eventType, chanceWeight)
		{
			this.cohesionIncrease = cohesionIncrease;
		}

		public override BaseEvent GetBaseEvent()
		{
			return new ChattingCommanders();
		}
	}
}
