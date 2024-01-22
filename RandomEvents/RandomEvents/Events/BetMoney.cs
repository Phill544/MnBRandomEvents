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
	internal sealed class BetMoney : BaseEvent
	{
		private readonly bool eventDisabled;
		private readonly float moneyBetPercent;

		public BetMoney() : base(ModSettings.RandomEvents.BetMoneyData)
		{			
			var ConfigFile = new IniFile(ParseIniFile.GetTheConfigFile());
            
			eventDisabled = ConfigFile.ReadBoolean("BetMoney", "EventDisabled");
			moneyBetPercent = ConfigFile.ReadFloat("BetMoney", "MoneyBetPercent");
		}
		
		private bool HasValidEventData()
		{
			if (eventDisabled == false)
			{
				if (moneyBetPercent != 0)
				{
					return true;
				}
			}
            
			return false;
		}
		

		public override void CancelEvent()
		{
		}
		
		public override bool CanExecuteEvent()
		{
			return HasValidEventData() && MobileParty.MainParty.MemberRoster.TotalRegulars > 0;
		}

		public override void StartEvent()
		{
			var eventTitle = new TextObject("{=BetMoney_Title}All or nothing").ToString();
			
			var eventOption1 = new TextObject("{=BetMoney_Event_Option_1}Gamble").ToString();

			var eventOption2 = new TextObject("{=BetMoney_Event_Option_2}Decline").ToString();
			
			var inquiryElements = new List<InquiryElement>
			{
				new InquiryElement("a", eventOption1, null),
				new InquiryElement("b", eventOption2, null)
			};
			
			var eventExtraDialogue = new TextObject("{=BetMoney_Event_Extra_Dialogue}You have no idea how they have that much money. You contemplate stealing it.").ToString();

			var goldToBet = MathF.Floor(Hero.MainHero.Gold * MBRandom.RandomFloatRanged(0.01f, moneyBetPercent));

			var extraDialogue = "";
			if (goldToBet > 40000)
				extraDialogue = eventExtraDialogue;
			
			var eventDescription = new TextObject("{=BetMoney_Event_Desc}One of your soldiers wants to flip a coin. Heads you win, tails they do. The prize is {goldToBet} gold.{extraDialogue}")
				.SetTextVariable("goldToBet", goldToBet)
				.SetTextVariable("extraDialogue", extraDialogue)
				.ToString();
			
			var eventButtonText1 = new TextObject("{=BetMoney_Event_Button_Text_1}Okay").ToString();
			var eventButtonText2 = new TextObject("{=BetMoney_Event_Button_Text_2}Done").ToString();

			var eventNoBet = new TextObject("{=BetMoney_Event_No_Bet}You walk away.").ToString();

			var msid = new MultiSelectionInquiryData(eventTitle, eventDescription, inquiryElements, false, 1, 1, eventButtonText1, null, 
				elements => 
				{
					if ((string)elements[0].Identifier == "a")
					{
						var outcomeText = DoBet(goldToBet);
						InformationManager.ShowInquiry(new InquiryData(eventTitle, outcomeText, true, false, eventButtonText2, null, null, null), true);
					}
					else
					{
						InformationManager.ShowInquiry(new InquiryData(eventTitle, eventNoBet, true, false, eventButtonText2, null, null, null), true);
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

		private static string DoBet(int goldToBet)
		{
			var decision = MBRandom.RandomFloatRanged(0.0f, 1.0f);
			
			string outcomeText;
			
			var eventOutcomeText1 = new TextObject("{=BetMoney_Event_Outcome_Text_1}Well, I'm never going to make that money back... Your companion says with a heavy sigh as your pocket your 'hard earned' gold.").ToString();
			var eventOutcomeText2 = new TextObject("{=BetMoney_Event_Outcome_Text_2}Better luck next time. Your companion says smugly.").ToString();

			if (decision >= 0.5f)
			{
				outcomeText = eventOutcomeText1;
				Hero.MainHero.ChangeHeroGold(goldToBet);
			}
			else
			{
				outcomeText = eventOutcomeText2;
				Hero.MainHero.ChangeHeroGold(-goldToBet);
			}

			return outcomeText;
		}
		
	}

	public class BetMoneyData : RandomEventData
	{

		public BetMoneyData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
		{
		}

		public override BaseEvent GetBaseEvent()
		{
			return new BetMoney();
		}
	}
}
