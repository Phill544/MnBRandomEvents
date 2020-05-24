using CryingBuffalo.RandomEvents;
using CryingBuffalo.RandomEvents.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection.Inventory;
using TaleWorlds.Core;

namespace CryingBuffalo.RandomEvents.Events
{
	public class FoodFight : BaseEvent
	{
		private int minFoodLoss;
		private int maxFoodLoss;
		private int moraleLoss;

		private string eventTitle = "Bataille de nourriture!";

		public FoodFight() : base(Settings.RandomEvents.FoodFightData)
		{
			minFoodLoss = Settings.RandomEvents.FoodFightData.minFoodLoss;
			maxFoodLoss = Settings.RandomEvents.FoodFightData.maxFoodLoss;
			moraleLoss = Settings.RandomEvents.FoodFightData.moraleLoss;
		}

		public override void CancelEvent()
		{
		}

		public override bool CanExecuteEvent()
		{
			if ((MobileParty.MainParty.ItemRoster.Where((item) => item.EquipmentElement.Item.IsFood).Count() > 0) && MobileParty.MainParty.MemberRoster.TotalManCount > 1)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public override void StartEvent()
		{
			if (Settings.GeneralSettings.DebugMode)
			{
				InformationManager.DisplayMessage(new InformationMessage($"Starting {this.RandomEventData.EventType}", RandomEventsSubmodule.textColor));
			}

			List<InquiryElement> inquiryElements = new List<InquiryElement>();
			inquiryElements.Add(new InquiryElement("a", "Ça suffit.", null, true, "D'où ces imbéciles pensent-ils que cette nourriture vient?"));
			inquiryElements.Add(new InquiryElement("b", "Participer!", null, true, "Vous aviez fini de manger de tous façon."));

			MultiSelectionInquiryData msid = new MultiSelectionInquiryData(
				eventTitle, // Title
				$"Pendant que votre groupe mange, une grande bataille de nourriture éclate.", // Description
				inquiryElements, // Options
				false, // Can close menu without selecting an option. Should always be false.
				true, // Force a single option to be selected. Should usually be true
				"Okay", // The text on the button that continues the event
				null, // The text to display on the "cancel" button, shouldn't ever need it.
				(elements) => // How to handle the selected option. Will only ever be a single element unless force single option is off.
				{
					if ((string)elements[0].Identifier == "a")
					{
						MobileParty.MainParty.RecentEventsMorale -= moraleLoss;

						InformationManager.ShowInquiry(new InquiryData(eventTitle, "Vous commandez que tout le monde arrête ce non-sens. Bien que la fête semble mécontente, au moins vous avez sauvé la nourriture.", true, false, "Terminé", null, null, null), true);
					}
					else if ((string)elements[0].Identifier == "b")
					{
						string extraDialogue = "";

						float xpToGive = Settings.GeneralSettings.GeneralLevelXpMultiplier * Hero.MainHero.GetSkillValue(DefaultSkills.Throwing) * 0.5f;
						Hero.MainHero.AddSkillXp(DefaultSkills.Throwing, xpToGive);

						int foodToRemove = MBRandom.RandomInt(minFoodLoss, maxFoodLoss);
						bool runOutOfFood = RemoveFood(foodToRemove);
						if (runOutOfFood)
						{
							extraDialogue = " Vous réalisez rapidement qu'il n'y a plus de nourriture. Si vous ne pouvez pas vous en procurer plus bientôt, il risque d'y avoir un problème.";
						}

						InformationManager.ShowInquiry(new InquiryData(eventTitle, $"Vous décidez de vous amuser! Vous parvenez même à faire sortir certains yeux au beurre noir. êtes vous allé trop loin ? Probablement.{extraDialogue}", true, false, "terminé", null, null, null), true);
					}
					else
					{
						MessageBox.Show($"Error while selecting option forErreur lors de la sélection de l'option pour \"{this.RandomEventData.EventType}\"");
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
				MessageBox.Show($"Error while stopping \"{this.RandomEventData.EventType}\" event :\n\n {ex.Message} \n\n { ex.StackTrace}");
			}
		}

		private bool RemoveFood(int foodToRemove)
		{
			int currentlyRemovedFood = 0;

			while (currentlyRemovedFood < foodToRemove)
			{
				List<ItemRosterElement> foodItems = MobileParty.MainParty.ItemRoster.Where((item) => item.EquipmentElement.Item.IsFood).ToList();

				if (foodItems.Count() == 0)
				{
					return true;
				}

				int element = MBRandom.RandomInt(0, foodItems.Count());
				int amount = foodItems[element].Amount;
				amount--;
				MobileParty.MainParty.ItemRoster.Remove(foodItems[element]);
				MobileParty.MainParty.ItemRoster.AddToCounts(foodItems[element].EquipmentElement.Item, amount);
				currentlyRemovedFood++;
			}

			return false;
		}
	}


	public class FoodFightData : RandomEventData
	{
		public int minFoodLoss;
		public int maxFoodLoss;
		public int moraleLoss;

		public FoodFightData(string eventType, float chanceWeight, int minFoodLoss, int maxFoodLoss, int moraleLoss) : base(eventType, chanceWeight)
		{
			this.minFoodLoss = minFoodLoss;
			this.maxFoodLoss = maxFoodLoss;
			this.moraleLoss = moraleLoss;
		}

		public override BaseEvent GetBaseEvent()
		{
			return new FoodFight();
		}
	}
}
