using CryingBuffalo.RandomEvents.Helpers;
using System;
using System.Windows;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Library;

namespace CryingBuffalo.RandomEvents.Events
{
	public sealed class PrisonerRebellion : BaseEvent
	{
		private readonly int minimumPrisoners;

		private bool heroInPrisonerRoster;

		public PrisonerRebellion() : base(Settings.Settings.RandomEvents.PrisonerRebellionData)
		{
			minimumPrisoners = Settings.Settings.RandomEvents.PrisonerRebellionData.minimumPrisoners;
		}

		public override void CancelEvent()
		{
		}

		public override bool CanExecuteEvent()
		{
			if (MobileParty.MainParty.PrisonRoster.TotalHealthyCount > minimumPrisoners && MobileParty.MainParty.CurrentSettlement == null)
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
			if (Settings.Settings.GeneralSettings.DebugMode)
			{
				InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}", RandomEventsSubmodule.TextColor));
			}

			try
			{
				MobileParty prisonerParty = PartySetup.CreateBanditParty("looters", "Escaped prisoners (Random Event)");

				prisonerParty.MemberRoster.Clear();
				DoPrisonerTransfer(prisonerParty);

				prisonerParty.Aggressiveness = 10;
				prisonerParty.SetMoveEngageParty(MobileParty.MainParty);

				string heroDialogue = "";
				if (heroInPrisonerRoster)
				{
					heroDialogue = "\n\nFortunately, you keep the important prisoners separate and they were unable to escape!";
				}

				InformationManager.ShowInquiry(
					new InquiryData("Prisoner rebellion!",
									$"While your guards weren't looking the prisoners managed to break free. \"We'd rather die than stay in captivity another day!\"{heroDialogue}",
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

		private void DoPrisonerTransfer(MobileParty prisonerParty)
		{
			var rosterAsList = MobileParty.MainParty.PrisonRoster.GetTroopRoster();
			foreach (var element in rosterAsList)
			{
				if (!element.Character.IsHero)
				{
					prisonerParty.AddElementToMemberRoster(element.Character, element.Number);
					MobileParty.MainParty.PrisonRoster.RemoveTroop(element.Character, element.Number);
				}
				else
				{
					heroInPrisonerRoster = true;
				}
			}
		}
	}

	public class PrisonerRebellionData : RandomEventData
	{
		public readonly int minimumPrisoners;

		public PrisonerRebellionData(string eventType, float chanceWeight, int minimumPrisoners) : base(eventType, chanceWeight)
		{
			this.minimumPrisoners = minimumPrisoners;
		}

		public override BaseEvent GetBaseEvent()
		{
			return new PrisonerRebellion();
		}
	}
}
