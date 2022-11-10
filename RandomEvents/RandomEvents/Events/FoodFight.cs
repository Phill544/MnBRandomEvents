using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using CryingBuffalo.RandomEvents.Settings;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace CryingBuffalo.RandomEvents.Events
{
	public sealed class FoodFight : BaseEvent
	{
		private readonly int minFoodLoss;
		private readonly int maxFoodLoss;
		private readonly int moraleLoss;

		public FoodFight() : base(ModSettings.RandomEvents.FoodFightData)
		{
			minFoodLoss = ModSettings.RandomEvents.FoodFightData.minFoodLoss;
			maxFoodLoss = ModSettings.RandomEvents.FoodFightData.maxFoodLoss;
			moraleLoss = ModSettings.RandomEvents.FoodFightData.moraleLoss;
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
			if (ModSettings.GeneralSettings.DebugMode)
			{
				InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}", RandomEventsSubmodule.Dbg_Color));
			}
			
			var eventTitle = new TextObject("{=FoodFight_Title}Food Fight!").ToString();
			
			var eventDescription = new TextObject("{=FoodFight_Event_Desc}While your party is eating, a large food fight breaks out.")
				.ToString();
			
			var eventOption1 = new TextObject("{=FoodFight_Event_Option_1}Break it up").ToString();
			var eventOption1Hover = new TextObject("{=FoodFight_Event_Option_1_Hover}Where do these fools think this food comes from?").ToString();
			
			var eventOption2 = new TextObject("{=FoodFight_Event_Option_2}Join in!").ToString();
			var eventOption2Hover = new TextObject("{=FoodFight_Event_Option_2_Hover}You were done eating anyway.").ToString();
			
			var eventButtonText1 = new TextObject("{=FoodFight_Event_Button_Text_1}Okay").ToString();
			var eventButtonText2 = new TextObject("{=FoodFight_Event_Button_Text_2}Done").ToString();
			
			var eventOutcome1 = new TextObject("{=FoodFight_Event_Text_1}You command that everyone stops this nonsense. Although the party looks displeased, at least you saved the food.")
				.ToString();

			var eventExtraDialogue = new TextObject("{=FoodFight_Event_Extra_Dialogue} Quickly you realise that there is no food left. If you can't source some more soon there may be trouble.").ToString();

			var inquiryElements = new List<InquiryElement>
			{
				new InquiryElement("a", eventOption1, null, true, eventOption1Hover),
				new InquiryElement("b", eventOption2, null, true, eventOption2Hover)
			};

			var msid = new MultiSelectionInquiryData(eventTitle, eventDescription, inquiryElements, false, 1, eventButtonText1, null, 
				elements => 
				{
					switch ((string)elements[0].Identifier)
					{
						case "a":
							MobileParty.MainParty.RecentEventsMorale -= moraleLoss;

							InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOutcome1, true, false, eventButtonText2, null, null, null), true);
							break;
						case "b":
						{
							var extraDialogue = "";

							var xpToGive = MCM_MenuConfig.Instance.GS_GeneralLevelXpMultiplier * Hero.MainHero.GetSkillValue(DefaultSkills.Throwing) * 0.5f;
							Hero.MainHero.AddSkillXp(DefaultSkills.Throwing, xpToGive);

							var foodToRemove = MBRandom.RandomInt(minFoodLoss, maxFoodLoss);
							var runOutOfFood = RemoveFood(foodToRemove);
							if (runOutOfFood)
							{
								extraDialogue = eventExtraDialogue;
							}
							
							var eventOutcome2 = new TextObject("{=FoodFight_Event_Text_2}You decide to join in on the fun! You even manage to deal out some black eyes. Did you go too far? Probably.{extraDialogue}")
								.SetTextVariable("extraDialogue", extraDialogue)
								.ToString();

							InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOutcome2, true, false, eventButtonText2, null, null, null), true);
							break;
						}
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

		private static bool RemoveFood(int foodToRemove)
		{
			var currentlyRemovedFood = 0;

			while (currentlyRemovedFood < foodToRemove)
			{
				var foodItems = MobileParty.MainParty.ItemRoster.Where(item => item.EquipmentElement.Item.IsFood).ToList();

				if (!foodItems.Any())
				{
					return true;
				}

				var element = MBRandom.RandomInt(0, foodItems.Count());
				var amount = foodItems[element].Amount;
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
