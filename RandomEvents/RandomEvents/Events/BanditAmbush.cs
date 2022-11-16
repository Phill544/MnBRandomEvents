﻿using System;
using System.Collections.Generic;
using System.Windows;
using CryingBuffalo.RandomEvents.Helpers;
using CryingBuffalo.RandomEvents.Settings;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace CryingBuffalo.RandomEvents.Events
{
	public sealed class BanditAmbush : BaseEvent
	{

		private readonly float moneyMinPercent;
		private readonly float moneyMaxPercent;
		private readonly int troopScareCount;
		private readonly int banditCap;
		
		public BanditAmbush() : base(ModSettings.RandomEvents.BanditAmbushData)
		{
			moneyMinPercent = MCM_MenuConfig_A_M.Instance.BA_MoneyMinPercent;
			moneyMaxPercent = MCM_MenuConfig_A_M.Instance.BA_MoneyMaxPercent;
			troopScareCount = MCM_MenuConfig_A_M.Instance.BA_TroopScareCount;
			banditCap = MCM_MenuConfig_A_M.Instance.BA_BanditCap;
		}

		public override void CancelEvent()
		{
		}

		public override bool CanExecuteEvent()
		{
			return MCM_MenuConfig_A_M.Instance.BA_Disable == false && MobileParty.MainParty.CurrentSettlement == null;
		}

		public override void StartEvent()
		{
			if (MCM_ConfigMenu_General.Instance.GS_DebugMode)
			{
				InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}", RandomEventsSubmodule.Dbg_Color));
			}
			
			var eventTitle = new TextObject("{=BanditAmbush_Title}Ambushed by bandits!").ToString();
			
			var eventOption1 = new TextObject("{=BanditAmbush_Event_Option_1}Pay gold to have them leave").ToString();
			var eventOption1Hover = new TextObject("{=BanditAmbush_Event_Option_1_Hover}What is gold good for, if not to dissuade people from killing you?").ToString();
            
			var eventOption2 = new TextObject("{=BanditAmbush_Event_Option_2}Attack").ToString();

			var eventOption3 = new TextObject("{=BanditAmbush_Event_Option_3}Intimidate them").ToString();
			
			var eventButtonText1 = new TextObject("{=BanditAmbush_Event_Button_Text_1}Okay").ToString();
			var eventButtonText2 = new TextObject("{=BanditAmbush_Event_Button_Text_2}Done").ToString();

			var inquiryElements = new List<InquiryElement>
			{
				new InquiryElement("a", eventOption1, null, true, eventOption1Hover),
				new InquiryElement("b", eventOption2, null)
			};

			if (Hero.MainHero.PartyBelongedTo.MemberRoster.TotalHealthyCount > troopScareCount)
			{
				inquiryElements.Add(new InquiryElement("c", eventOption3, null)); 
			}
			
			var percentMoneyLost = MBRandom.RandomFloatRanged(moneyMinPercent, moneyMaxPercent);
			
			var goldLost = MathF.Floor(Hero.MainHero.Gold * percentMoneyLost);
			
			var eventOptionAText = new TextObject(
					"{=BanditAmbush_Event_Choice_1}You give the bandits {goldLost} coins and they quickly flee. At least you and your soldiers live to fight another day.")
				.SetTextVariable("goldLost", goldLost)
				.ToString();
			
			var eventOptionBText = new TextObject(
					"{=BanditAmbush_Event_Choice_2}Seeing you won't back down, the bandits get ready for a fight.")
				.ToString();
			
			var eventOptionCText = new TextObject(
					"{=BanditAmbush_Event_Choice_3}You laugh as you watch the rest of your party emerge over the crest of the hill. The bandits get ready to flee.")
				.ToString();

			var msid = new MultiSelectionInquiryData(eventTitle, CalculateDescription(), inquiryElements, false, 1, eventButtonText1, null, 
				elements => 
				{
					switch ((string)elements[0].Identifier)
					{
						case "a":
						{
							Hero.MainHero.ChangeHeroGold(-goldLost);
							InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionAText, true, false, eventButtonText2, null, null, null), true);
							break;
						}
						case "b":
							SpawnBandits(false);
							InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionBText, true, false, eventButtonText2, null, null, null), true);
							break;
						case "c":
							SpawnBandits(true);
							InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionCText, true, false, eventButtonText2, null, null, null), true);
							break;
						default:
							MessageBox.Show($"Error while selecting option for \"{randomEventData.eventType}\"");
							break;
					}
				},
				null);

			MBInformationManager.ShowMultiSelectionInquiry(msid, true);

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

		private string CalculateDescription()
		{
			
			var eventDescription1 = new TextObject(
					"{=BanditAmbush_Event_Desc_1}You are traveling with your forward party when you get surrounded by a group of bandits!")
				.ToString();
			
			var eventDescription2 = new TextObject(
					"{=BanditAmbush_Event_Desc_2}While traveling your party gets surrounded by a group of bandits!")
				.ToString();
			
			return Hero.MainHero.PartyBelongedTo.MemberRoster.Count > troopScareCount ? eventDescription1 : eventDescription2;
		}

		private void SpawnBandits(bool shouldFlee)
		{
			try
			{
				MobileParty banditParty = PartySetup.CreateBanditParty();

				banditParty.MemberRoster.Clear();

				if (shouldFlee)
				{
					banditParty.Aggressiveness = 0.2f;
				}
				else
				{
					banditParty.Aggressiveness = 10f;
					banditParty.SetMoveEngageParty(MobileParty.MainParty);
				}

				int numberToSpawn = Math.Min((int)(MobileParty.MainParty.MemberRoster.TotalManCount * 0.50f), banditCap);

				PartySetup.AddRandomCultureUnits(banditParty, 10 + numberToSpawn);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error while running \"{randomEventData.eventType}\" event :\n\n {ex.Message} \n\n { ex.StackTrace}");
			}
		}
	}

	public class BanditAmbushData : RandomEventData
	{

		public BanditAmbushData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
		{
		}

		public override BaseEvent GetBaseEvent()
		{
			return new BanditAmbush();
		}


	}
}
