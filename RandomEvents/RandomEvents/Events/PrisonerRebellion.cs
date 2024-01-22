using System;
using System.Windows;
using Bannerlord.RandomEvents.Helpers;
using Bannerlord.RandomEvents.Settings;
using Ini.Net;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace Bannerlord.RandomEvents.Events
{
	public sealed class PrisonerRebellion : BaseEvent
	{
		private readonly bool eventDisabled;
		private readonly int minimumPrisoners;

		private bool heroInPrisonerRoster;

		public PrisonerRebellion() : base(ModSettings.RandomEvents.PrisonerRebellionData)
		{
			var ConfigFile = new IniFile(ParseIniFile.GetTheConfigFile());
            
			eventDisabled = ConfigFile.ReadBoolean("PrisonerRebellion", "EventDisabled");
			minimumPrisoners = ConfigFile.ReadInteger("PrisonerRebellion", "MinimumPrisoners");
		}

		public override void CancelEvent()
		{
		}
		
		private bool HasValidEventData()
		{
			if (eventDisabled == false)
			{
				if (minimumPrisoners != 0)
				{
					return true;
				}
			}
            
			return false;
		}

		public override bool CanExecuteEvent()
		{
			return HasValidEventData() && MobileParty.MainParty.PrisonRoster.TotalHealthyCount > minimumPrisoners && MobileParty.MainParty.CurrentSettlement == null;
		}

		public override void StartEvent()
		{
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
				prisonerParty.Ai.SetMoveEngageParty(MobileParty.MainParty);

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
