using System;
using System.Collections.Generic;
using System.Windows;
using Bannerlord.RandomEvents.Helpers;
using Bannerlord.RandomEvents.Settings;
using Ini.Net;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace Bannerlord.RandomEvents.Events
{
	public sealed class BanditAmbush : BaseEvent
	{
		private readonly bool eventDisabled;
		private readonly float moneyMinPercent;
		private readonly float moneyMaxPercent;
		private readonly int troopScareCount;
		private readonly int banditCap;
		
		public BanditAmbush() : base(ModSettings.RandomEvents.BanditAmbushData)
		{
			var ConfigFile = new IniFile(ParseIniFile.GetTheConfigFile());
            
			eventDisabled = ConfigFile.ReadBoolean("BanditAmbush", "EventDisabled");
			moneyMinPercent = ConfigFile.ReadFloat("BanditAmbush", "MoneyMinPercent");
			moneyMaxPercent = ConfigFile.ReadFloat("BanditAmbush", "MoneyMaxPercent");
			troopScareCount = ConfigFile.ReadInteger("BanditAmbush", "TroopScareCount");
			banditCap = ConfigFile.ReadInteger("BanditAmbush", "BanditCap");
		}

		public override void CancelEvent()
		{
		}
		
		private bool HasValidEventData()
		{
			if (eventDisabled == false)
			{
				if (moneyMinPercent != 0 || moneyMaxPercent != 0 || troopScareCount != 0 || banditCap != 0 )
				{
					return true;
				}
			}
            
			return false;
		}

		public override bool CanExecuteEvent()
		{
			return HasValidEventData() && MobileParty.MainParty.CurrentSettlement == null;
		}

		public override void StartEvent()
		{
			var heroGold = Hero.MainHero.Gold;

			if (heroGold < 1)
			{
				//Emulate some gold
				Hero.MainHero.ChangeHeroGold(+400);
				heroGold += 400;
			}
			
			var percentMoneyLost = MBRandom.RandomFloatRanged(moneyMinPercent, moneyMaxPercent);
			var goldLost = MathF.Floor(heroGold * percentMoneyLost);
			
			var eventTitle = new TextObject("{=BanditAmbush_Title}Ambushed by bandits!").ToString();
			
			var eventOption1 = new TextObject("{=BanditAmbush_Event_Option_1}Pay {goldLost} gold to have them leave").SetTextVariable("goldLost", goldLost).ToString();
			var eventOption1Hover = new TextObject("{=BanditAmbush_Event_Option_1_Hover}What is gold good for, if not to dissuade people from killing you?").ToString();
            
			var eventOption2 = new TextObject("{=BanditAmbush_Event_Option_2}Attack").ToString();

			var eventOption3 = new TextObject("{=BanditAmbush_Event_Option_3}Intimidate them").ToString();
			
			var eventButtonText1 = new TextObject("{=BanditAmbush_Event_Button_Text_1}Okay").ToString();
			var eventButtonText2 = new TextObject("{=BanditAmbush_Event_Button_Text_2}Done").ToString();

			var inquiryElements = new List<InquiryElement>();

			if (heroGold > goldLost)
			{
				inquiryElements.Add(new InquiryElement("a", eventOption1, null, true, eventOption1Hover)); 
			}

			inquiryElements.Add(new InquiryElement("b", eventOption2, null)); 

			if (Hero.MainHero.PartyBelongedTo.MemberRoster.TotalHealthyCount > troopScareCount)
			{
				inquiryElements.Add(new InquiryElement("c", eventOption3, null)); 
			}
			
			
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

			var msid = new MultiSelectionInquiryData(eventTitle, CalculateDescription(), inquiryElements, false, 1, 1, eventButtonText1, null, 
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
				}, null, null);

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
				var banditParty = PartySetup.CreateBanditParty();

				banditParty.MemberRoster.Clear();

				if (shouldFlee)
				{
					banditParty.Aggressiveness = 0.2f;
				}
				else
				{
					banditParty.Aggressiveness = 10f;
					
					banditParty.Ai.SetMoveEngageParty(MobileParty.MainParty);
				}

				var numberToSpawn = Math.Min((int)(MobileParty.MainParty.MemberRoster.TotalManCount * 0.50f), banditCap);

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
