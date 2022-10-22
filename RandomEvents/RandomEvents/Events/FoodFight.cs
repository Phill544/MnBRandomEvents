using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace CryingBuffalo.RandomEvents.Events
{
	public sealed class FoodFight : BaseEvent
	{
		private readonly int minFoodLoss;
		private readonly int maxFoodLoss;
		private readonly int moraleLoss;

		private const string EventTitle = "Food Fight!";

		public FoodFight() : base(Settings.Settings.RandomEvents.FoodFightData)
		{
			minFoodLoss = Settings.Settings.RandomEvents.FoodFightData.minFoodLoss;
			maxFoodLoss = Settings.Settings.RandomEvents.FoodFightData.maxFoodLoss;
			moraleLoss = Settings.Settings.RandomEvents.FoodFightData.moraleLoss;
		}

		public override void CancelEvent()
		{
		}

		public override bool CanExecuteEvent()
		{
			return (MobileParty.MainParty.ItemRoster.Any(item => item.EquipmentElement.Item.IsFood)) && MobileParty.MainParty.MemberRoster.TotalManCount > 1;
		}

		public override void StartEvent()
		{
			if (Settings.Settings.GeneralSettings.DebugMode)
			{
				InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}", RandomEventsSubmodule.TextColor));
			}

			List<InquiryElement> inquiryElements = new List<InquiryElement>();
			inquiryElements.Add(new InquiryElement("a", "Break it up.", null, true, "Where do these fools think this food comes from?"));
			inquiryElements.Add(new InquiryElement("b", "Join in!", null, true, "You were done eating anyway."));

			MultiSelectionInquiryData msid = new MultiSelectionInquiryData(
				EventTitle, // Title
				$"While your party is eating, a large food fight breaks out.", // Description
				inquiryElements, // Options
				false, // Can close menu without selecting an option. Should always be false.
				1, // Force a single option to be selected. Should usually be true
				"Okay", // The text on the button that continues the event
				null, // The text to display on the "cancel" button, shouldn't ever need it.
				(elements) => // How to handle the selected option. Will only ever be a single element unless force single option is off.
				{
					switch ((string)elements[0].Identifier)
					{
						case "a":
							MobileParty.MainParty.RecentEventsMorale -= moraleLoss;

							InformationManager.ShowInquiry(new InquiryData(EventTitle, "You command that everyone stops this nonsense. Although the party looks displeased, at least you saved the food.", true, false, "Done", null, null, null), true);
							break;
						case "b":
						{
							string extraDialogue = "";

							float xpToGive = Settings.Settings.GeneralSettings.GeneralLevelXpMultiplier * Hero.MainHero.GetSkillValue(DefaultSkills.Throwing) * 0.5f;
							Hero.MainHero.AddSkillXp(DefaultSkills.Throwing, xpToGive);

							int foodToRemove = MBRandom.RandomInt(minFoodLoss, maxFoodLoss);
							bool runOutOfFood = RemoveFood(foodToRemove);
							if (runOutOfFood)
							{
								extraDialogue = " Quickly you realise that there is no food left. If you can't source some more soon there may be trouble.";
							}

							InformationManager.ShowInquiry(new InquiryData(EventTitle, $"You decide to join in on the fun! You even manage to deal out some black eyes. Did you go too far? Probably.{extraDialogue}", true, false, "Done", null, null, null), true);
							break;
						}
						default:
							MessageBox.Show($"Error while selecting option for \"{randomEventData.eventType}\"");
							break;
					}
				},
				null); // What to do on the "cancel" button, shouldn't ever need it.

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

		private bool RemoveFood(int foodToRemove)
		{
			int currentlyRemovedFood = 0;

			while (currentlyRemovedFood < foodToRemove)
			{
				List<ItemRosterElement> foodItems = MobileParty.MainParty.ItemRoster.Where((item) => item.EquipmentElement.Item.IsFood).ToList();

				if (!foodItems.Any())
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
		public readonly int minFoodLoss;
		public readonly int maxFoodLoss;
		public readonly int moraleLoss;

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
