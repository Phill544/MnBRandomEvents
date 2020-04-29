using System;
using System.Collections.Generic;
using System.Windows;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.TwoDimension;

namespace CryingBuffalo.RandomEvents.Events
{
	class BetMoney : BaseEvent
	{
		private float moneyBetPercent;

		public BetMoney() : base(Settings.RandomEvents.BetMoneyData)
		{
			moneyBetPercent = Settings.RandomEvents.BetMoneyData.moneyBetPercent;
		}

		public override void StartEvent()
		{
			List<InquiryElement> inquiryElements = new List<InquiryElement>();
			inquiryElements.Add(new InquiryElement("a", "Gamble", null));
			inquiryElements.Add(new InquiryElement("b", "Decline", null));

			int goldToBet = (int)Mathf.Floor(Hero.MainHero.Gold * moneyBetPercent);

			string extraDialogue = "";
			if (goldToBet > 40000)
				extraDialogue = " You have no idea how they have that much money. You contemplate stealing it.";

			MultiSelectionInquiryData msid = new MultiSelectionInquiryData(
				"All or nothing", // Title
				$"One of your soldiers wants to flip a coin. Heads you win, tails they do. The prize is {goldToBet} gold.{extraDialogue}", // Description
				inquiryElements, // Options
				false, // Can close menu without selecting an option. Should always be false.
				true, // Force a single option to be selected. Should usually be true
				"Okay", // The text on the button that continues the event
				null, // The text to display on the "cancel" button, shouldn't ever need it.
				(elements) => // How to handle the selected option. Will only ever be a single element unless force single option is off.
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

			InformationManager.ShowMultiSelectionInquiry(msid, true);

			StopEvent();
		}

		private string DoBet(int goldToBet)
		{
			float decision = MBRandom.RandomFloatRanged(0.0f, 1.0f);
					

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

		public override void CancelEvent()
		{
		}

		public override bool CanExecuteEvent()
		{
			return true;
		}
	}

	public class BetMoneyData : RandomEventData
	{
		public float moneyBetPercent;

		public BetMoneyData(RandomEventType eventType, float chanceWeight, float moneyBetPercent) : base(eventType, chanceWeight)
		{
			this.moneyBetPercent = moneyBetPercent;
		}
	}
}
