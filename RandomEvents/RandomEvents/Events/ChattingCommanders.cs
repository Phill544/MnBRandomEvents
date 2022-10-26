using System;
using System.Linq;
using System.Windows;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Library;

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
				InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}", RandomEventsSubmodule.TextColor));
			}

			try
			{
				MobileParty.MainParty.Army.Cohesion += cohesionIncrease;

				InformationManager.ShowInquiry(
					new InquiryData(EventTitle,
									"After a good chat with the commanders of your army, there is a noticeable increase cohesion.",
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
