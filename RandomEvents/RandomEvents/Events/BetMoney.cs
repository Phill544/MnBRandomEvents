using System;
using System.Collections.Generic;
using System.Windows;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace CryingBuffalo.RandomEvents.Events
{
	internal sealed class BetMoney : BaseEvent
	{
		
		private const string EventTitle = "All or nothing";
		private readonly float moneyBetPercent;

		public BetMoney() : base(Settings.ModSettings.RandomEvents.BetMoneyData)
		{
			moneyBetPercent = Settings.ModSettings.RandomEvents.BetMoneyData.moneyBetPercent;
		}

		public override void StartEvent()
		{
			List<InquiryElement> inquiryElements = new List<InquiryElement>
			{
				new InquiryElement("a", "Gamble", null),
				new InquiryElement("b", "Decline", null)
			};

			int goldToBet = (int)MathF.Floor(Hero.MainHero.Gold * moneyBetPercent);

			string extraDialogue = "";
			if (goldToBet > 40000)
				extraDialogue = " You have no idea how they have that much money. You contemplate stealing it.";

			MultiSelectionInquiryData msid = new MultiSelectionInquiryData(
				EventTitle, // Title
				$"One of your soldiers wants to flip a coin. Heads you win, tails they do. The prize is {goldToBet} gold.{extraDialogue}", // Description
				inquiryElements, // Options
				false, // Can close menu without selecting an option. Should always be false.
				1, // Force a single option to be selected. Should usually be true
				"Okay", // The text on the button that continues the event
				null, // The text to display on the "cancel" button, shouldn't ever need it.
				elements => // How to handle the selected option. Will only ever be a single element unless force single option is off.
				{
					if ((string)elements[0].Identifier == "a")
					{
						string outcomeText = DoBet(goldToBet);
						InformationManager.ShowInquiry(new InquiryData("All or nothing", outcomeText, true, false, "Done", null, null, null), true);
					}
					else
					{
						InformationManager.ShowInquiry(new InquiryData("All or nothing", "You walk away.", true, false, "Done", null, null, null), true);
					}
				},
				null); // What to do on the "cancel" button, shouldn't ever need it.

			MBInformationManager.ShowMultiSelectionInquiry(msid, true);

			StopEvent();
		}

		private static string DoBet(int goldToBet)
		{
			var decision = MBRandom.RandomFloatRanged(0.0f, 1.0f);
					

			string outcomeText;

			if (decision >= 0.5f)
			{
				outcomeText = "\"Well, I'm never going to make that money back...\" Your companion says with a heavy sigh as your pocket your 'hard earned' gold.";
				Hero.MainHero.ChangeHeroGold(goldToBet);
			}
			else
			{
				outcomeText = "\"Better luck next time\" Your companion says smugly.";
				Hero.MainHero.ChangeHeroGold(-goldToBet);
			}

			return outcomeText;
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

		public override void CancelEvent()
		{
		}

		public override bool CanExecuteEvent()
		{
			return MobileParty.MainParty.MemberRoster.TotalRegulars > 0;
		}
	}

	public class BetMoneyData : RandomEventData
	{
		public readonly float moneyBetPercent;

		public BetMoneyData(string eventType, float chanceWeight, float moneyBetPercent) : base(eventType, chanceWeight)
		{
			this.moneyBetPercent = moneyBetPercent;
		}

		public override BaseEvent GetBaseEvent()
		{
			return new BetMoney();
		}
	}
}
