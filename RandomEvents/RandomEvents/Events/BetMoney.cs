using System;
using System.Collections.Generic;
using System.Windows;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace CryingBuffalo.RandomEvents.Events
{
	class BetMoney : BaseEvent
	{
		private int moneyBetAmount;

		public BetMoney()
		{
			moneyBetAmount = Settings.RandomEvents.BetMoneyData.moneyBetAmount;
		}

		public override void StartEvent()
		{
			List<InquiryElement> inquiryElements = new List<InquiryElement>();
			inquiryElements.Add(new InquiryElement("a", "Gamble", null));
			inquiryElements.Add(new InquiryElement("b", "Decline", null));
			
			MultiSelectionInquiryData msid = new MultiSelectionInquiryData(
				"All or nothing", // Title
				$"One of your companions wants to flip a coin. Heads you win, tails they do. The prize is {moneyBetAmount} gold.", // Description
				inquiryElements, // Options
				false, // Can close menu without selecting an option. Should always be false.
				true, // Force a single option to be selected. Should usually be true
				"Okay", // The text on the button that continues the event
				null, // The text to display on the "cancel" button, shouldn't ever need it.
				(elements) => // How to handle the selected option. Will only ever be a single element unless force single option is off.
				{
					if ((string)elements[0].Identifier == "a")
					{
						string outcomeText = DoBet();
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

		private string DoBet()
		{
			float decision = MBRandom.RandomFloatRanged(0.0f, 1.0f);

			string outcomeText;

			if (decision >= 0.5f)
			{
				outcomeText = "You win!";
				Hero.MainHero.ChangeHeroGold(moneyBetAmount);
			}
			else
			{
				outcomeText = "You lost...";
				Hero.MainHero.ChangeHeroGold(-moneyBetAmount);
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
				MessageBox.Show($"Error while stopping \"{Settings.RandomEvents.BetMoneyData.EventType}\" event :\n\n {ex.Message} \n\n { ex.StackTrace}");
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
		public int moneyBetAmount;

		public BetMoneyData(RandomEventType eventType, float chanceWeight, int moneyBetAmount) : base(eventType, chanceWeight)
		{
			this.moneyBetAmount = moneyBetAmount;
		}
	}
}
