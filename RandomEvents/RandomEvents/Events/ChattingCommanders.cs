﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace CryingBuffalo.RandomEvents.Events
{
	public class ChattingCommanders : BaseEvent
	{
		private float cohesionIncrease;

		public ChattingCommanders() : base(Settings.RandomEvents.ChattingCommandersData)
		{
			cohesionIncrease = Settings.RandomEvents.ChattingCommandersData.cohesionIncrease;
		}

		public override void CancelEvent()
		{
		}

		public override bool CanExecuteEvent()
		{
			if (MobileParty.MainParty.Army != null && MobileParty.MainParty.Army.ArmyOwner == Hero.MainHero && MobileParty.MainParty.Army.LeaderPartyAndAttachedParties.Count() > 1)
			{
				return true;
			}
			return false;
		}

		public override void StartEvent()
		{
			if (Settings.GeneralSettings.DebugMode)
			{
				InformationManager.DisplayMessage(new InformationMessage($"Starting {this.RandomEventData.EventType}", RandomEventsSubmodule.textColor));
			}

			try
			{
				MobileParty.MainParty.Army.Cohesion += cohesionIncrease;

				InformationManager.ShowInquiry(
					new InquiryData("The Same Page",
									$"After a good chat with the commanders of your army, there is a noticeable increase cohesion.",
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
				MessageBox.Show($"Error while running \"{this.RandomEventData.EventType}\" event :\n\n {ex.Message} \n\n { ex.StackTrace}");
			}

			StopEvent();
		}

		public override void StopEvent()
		{
			try
			{
				OnEventCompleted.Invoke();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error while stopping \"{this.RandomEventData.EventType}\" event :\n\n {ex.Message} \n\n { ex.StackTrace}");
			}
		}
	}

	public class ChattingCommandersData : RandomEventData
	{
		public float cohesionIncrease;

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
