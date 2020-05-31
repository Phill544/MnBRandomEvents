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

		private bool heroInPrisonerRoster;

		public PrisonerRebellion() : base(Settings.RandomEvents.PrisonerRebellionData)
		{
			minimumPrisoners = Settings.RandomEvents.PrisonerRebellionData.minimumPrisoners;
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
			if (Settings.GeneralSettings.DebugMode)
			{
				InformationManager.DisplayMessage(new InformationMessage($"Starting {this.RandomEventData.EventType}", RandomEventsSubmodule.textColor));
			}

			try
			{
				MobileParty prisonerParty = PartySetup.CreateBanditParty("looters", "Prisonniers évadés (Random Event)");

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
					new InquiryData("Rébellion des prisonniers!",
									$"Pendant que vos gardes ne regardaient pas, les prisonniers ont réussi à se libérer. \"Nous préférons mourir plutôt que de rester en captivité un autre jour\"{heroDialogue}",
									true,
									false,
									"Aux armes!",
									null,
									null,
									null
									), true);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Erreur lors de l'exécution \"{this.RandomEventData.EventType}\" event :\n\n {ex.Message} \n\n { ex.StackTrace}");
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
				MessageBox.Show($"Erreur lors de l'arrêt \"{this.RandomEventData.EventType}\" event :\n\n {ex.Message} \n\n { ex.StackTrace}");
			}
		}

		private void DoPrisonerTransfer(MobileParty prisonerParty)
		{
			List<TroopRosterElement> rosterAsList = MobileParty.MainParty.PrisonRoster.ToList();
			for (int i = 0; i < rosterAsList.Count; i++)
			{
				TroopRosterElement element = rosterAsList[i];
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
		public int minimumPrisoners;

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
