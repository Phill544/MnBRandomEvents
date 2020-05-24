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
			inquiryElements.Add(new InquiryElement("a", "Parier", null));
			inquiryElements.Add(new InquiryElement("b", "Décliner", null));

			int goldToBet = (int)Mathf.Floor(Hero.MainHero.Gold * moneyBetPercent);

			string extraDialogue = "";
			if (goldToBet > 40000)
				extraDialogue = " Vous ne savez pas comment il à réuni autant d'argent. Vous envisagez de le voler.";

			MultiSelectionInquiryData msid = new MultiSelectionInquiryData(
				"Tout ou rien", // Title
				$"Un de vos soldats veut lancer une pièce. Têtes vous gagnez, Face il gagne. Le prix est {goldToBet} Denar.{extraDialogue}", // Description
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
						InformationManager.ShowInquiry(new InquiryData("Tout ou rien", outcomeText, true, false, "Terminé", null, null, null), true);
					}
					else
					{
						InformationManager.ShowInquiry(new InquiryData("Tout ou rien", "Vous partez.", true, false, "Terminé", null, null, null), true);
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
				outcomeText = "\"Eh bien, je ne vais jamais récupérer cet argent...\" Dit votre compagnon avec un soupir lourd en fixant votre bourse votre avec votre or 'durement gagné'. ";
				Hero.MainHero.ChangeHeroGold(goldToBet);
			}
			else
			{
				outcomeText = "\"Plus de chance la prochaine fois\" dit votre compagnon avec suffisance.";
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
				MessageBox.Show($"Erreur lors de l'arrêt \"{this.RandomEventData.EventType}\" event :\n\n {ex.Message} \n\n { ex.StackTrace}");
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
		public float moneyBetPercent;

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
