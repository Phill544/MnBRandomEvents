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
	public class ExoticDrinks : BaseEvent
	{
		private int price;

		private string eventTitle = "Boissons exotiques";

		public ExoticDrinks() : base(Settings.RandomEvents.ExoticDrinksData)
		{
			this.price = Settings.RandomEvents.ExoticDrinksData.price;
		}

		public override void CancelEvent()
		{
		}

		public override bool CanExecuteEvent()
		{
			return Hero.MainHero.Gold >= price;
		}

		public override void StartEvent()
		{
			if (Settings.GeneralSettings.DebugMode)
			{
				InformationManager.DisplayMessage(new InformationMessage($"Starting {this.RandomEventData.EventType}", RandomEventsSubmodule.textColor));
			}

			List<InquiryElement> inquiryElements = new List<InquiryElement>();
			inquiryElements.Add(new InquiryElement("a", "Acheter une boisson", null, true, "Qu'est-ce qui pourrait mal se passer?"));
			inquiryElements.Add(new InquiryElement("b", "Décliné", null, true, "Il faudrait être fou pour boire une boisson inconue!"));

			MultiSelectionInquiryData msid = new MultiSelectionInquiryData(
				eventTitle, // Title
				$"Vous rencontrez un vendeur vendant des boissons exotiques pour {price}. Il ne vous dira pas comment, mais dit que cela fera de vous une meilleure personne.", // Description
				inquiryElements, // Options
				false, // Can close menu without selecting an option. Should always be false.
				true, // Force a single option to be selected. Should usually be true
				"Okay", // The text on the button that continues the event
				null, // The text to display on the "cancel" button, shouldn't ever need it.
				(elements) => // How to handle the selected option. Will only ever be a single element unless force single option is off.
				{
					if ((string)elements[0].Identifier == "a")
					{
						GiveRandomSkillXP();
						Hero.MainHero.ChangeHeroGold(-price);

						InformationManager.ShowInquiry(new InquiryData(eventTitle, "\"Choix judicieux. \"Le vendeur vous verse une petite tasse contenant un liquide jaune bizarre, pétillant. En prenant une gorgée, vous vous dites que ça sent la pisse. Rapidement, vous vous rendez compte qu'il a le même goût. \n J'espère que ce n'était pas une erreur.", true, false, "Terminé", null, null, null), true);
					}
					else if ((string)elements[0].Identifier == "b")
					{
						InformationManager.ShowInquiry(new InquiryData(eventTitle, "\"Hehehehehe\" le vendeur rit. \"Tant pis pour toi.\"", true, false, "Terminé", null, null, null), true);
					}
					else
					{
						MessageBox.Show($"Erreur lors de la sélection de l'option pour \"{this.RandomEventData.EventType}\"");
					}

				},
				null); // What to do on the "cancel" button, shouldn't ever need it.

			InformationManager.ShowMultiSelectionInquiry(msid, true);

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

		private void GiveRandomSkillXP()
		{
			List<SkillObject> allSkills = DefaultSkills.GetAllSkills().ToList();
			int index = MBRandom.RandomInt(allSkills.Count);

			float xpToGive = Settings.GeneralSettings.GeneralLevelXpMultiplier * Hero.MainHero.GetSkillValue(allSkills[index]);
			Hero.MainHero.AddSkillXp(allSkills[index], xpToGive);
		}
	}


	public class ExoticDrinksData : RandomEventData
	{
		public int price;

		public ExoticDrinksData(string eventType, float chanceWeight, int price) : base(eventType, chanceWeight)
		{
			this.price = price;
		}

		public override BaseEvent GetBaseEvent()
		{
			return new ExoticDrinks();
		}
	}
}
