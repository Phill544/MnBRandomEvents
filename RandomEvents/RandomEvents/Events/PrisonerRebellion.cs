﻿using System;
using System.Windows;
using CryingBuffalo.RandomEvents.Helpers;
using CryingBuffalo.RandomEvents.Settings;
using CryingBuffalo.RandomEvents.Settings.MCM;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace CryingBuffalo.RandomEvents.Events
{
	public sealed class PrisonerRebellion : BaseEvent
	{
		private readonly int minimumPrisoners;

		private bool heroInPrisonerRoster;

		public PrisonerRebellion() : base(ModSettings.RandomEvents.PrisonerRebellionData)
		{
			minimumPrisoners = MCM_MenuConfig_N_Z.Instance.PR_MinPrisoners;
		}

		public override void CancelEvent()
		{
		}

		public override bool CanExecuteEvent()
		{
			return MCM_MenuConfig_N_Z.Instance.PR_Disable == false && MobileParty.MainParty.PrisonRoster.TotalHealthyCount > minimumPrisoners && MobileParty.MainParty.CurrentSettlement == null;
		}

		public override void StartEvent()
		{
			if (MCM_ConfigMenu_General.Instance.GS_DebugMode)
			{
				InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}", RandomEventsSubmodule.Dbg_Color));
			}

			try
			{
				
				var eventTitle = new TextObject("{=PrisonerRebellion_Title}Prisoner Rebellion!").ToString();

				var eventPartyLabel= new TextObject("{=PrisonerRebellion_Event_Party_Label}Escaped prisoners (Random Event)").ToString();
				
				var eventButtonText = new TextObject("{=PrisonerRebellion_Event_Button_Text}To arms!").ToString();
				
				var prisonerParty = PartySetup.CreateBanditParty("looters", eventPartyLabel);
				
				var eventHeroDialogue= new TextObject("{=PrisonerRebellion_Hero_Dialogue}\n\nFortunately, you keep the important prisoners separate and they were unable to escape!").ToString();

				prisonerParty.MemberRoster.Clear();
				DoPrisonerTransfer(prisonerParty);

				prisonerParty.Aggressiveness = 10;
				prisonerParty.SetMoveEngageParty(MobileParty.MainParty);

				var heroDialogue = "";
				if (heroInPrisonerRoster)
				{
					heroDialogue = eventHeroDialogue;
				}
				
				var eventOption1 = new TextObject("{=PrisonerRebellion_Event_Text}While your guards weren't looking the prisoners managed to break free. They'd rather die than stay in captivity another day! {heroDialogue}")
					.SetTextVariable("heroDialogue", heroDialogue)
					.ToString();

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

		public PrisonerRebellionData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
		{
		}

		public override BaseEvent GetBaseEvent()
		{
			return new PrisonerRebellion();
		}
	}
}
