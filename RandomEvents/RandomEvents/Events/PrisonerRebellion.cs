using CryingBuffalo.RandomEvents.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace CryingBuffalo.RandomEvents.Events
{
	public class PrisonerRebellion : BaseEvent
	{
		private int minimumPrisoners;

		public PrisonerRebellion()
		{
			minimumPrisoners = Settings.RandomEvents.PrisonerRebellionData.minimumPrisoners;
		}

		public override void CancelEvent()
		{
		}

		public override bool CanExecuteEvent()
		{
			if (MobileParty.MainParty.PrisonRoster.TotalHealthyCount > minimumPrisoners)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public override void StartEvent()
		{
			if (Settings.GeneralSettings.DebugMode)
			{
				InformationManager.DisplayMessage(new InformationMessage($"Starting {Settings.RandomEvents.PrisonerRebellionData.EventType}", RandomEventsSubmodule.Instance.textColor));
			}

			try
			{
				MobileParty prisonerParty = PartySetup.CreateBanditParty("looters", "Escaped prisoners (Random Event)");

				prisonerParty.MemberRoster.Clear();
				DoPrisonerTransfer(prisonerParty);

				prisonerParty.Aggressiveness = 10;
				prisonerParty.SetMoveEngageParty(MobileParty.MainParty);

				InformationManager.ShowInquiry(
					new InquiryData("Prisoner rebellion!",
									$"While your guards weren't looking the prisoners managed to break free. \"We'd rather die than stay in captivity another day!\"",
									true,
									false,
									"To arms!",
									null,
									null,
									null
									), true);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error while running \"{Settings.RandomEvents.GranaryRatsData.EventType}\" event :\n\n {ex.Message} \n\n { ex.StackTrace}");
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
				MessageBox.Show($"Error while stopping \"{Settings.RandomEvents.PrisonerRebellionData.EventType}\" event :\n\n {ex.Message} \n\n { ex.StackTrace}");
			}
		}

		private void DoPrisonerTransfer(MobileParty prisonerParty)
		{
			foreach (TroopRosterElement element in MobileParty.MainParty.PrisonRoster)
			{
				prisonerParty.AddElementToMemberRoster(element.Character, element.Number);
			}

			MobileParty.MainParty.PrisonRoster.Clear();
		}
	}

	public class PrisonerRebellionData : RandomEventData
	{
		public int minimumPrisoners;

		public PrisonerRebellionData(RandomEventType eventType, float chanceWeight, int minimumPrisoners) : base(eventType, chanceWeight)
		{
			this.minimumPrisoners = minimumPrisoners;
		}
	}
}
