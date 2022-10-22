using CryingBuffalo.RandomEvents.Helpers;
using System;
using System.Collections.Generic;
using System.Windows;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

namespace CryingBuffalo.RandomEvents.Events
{
	public sealed class LookUp : BaseEvent
	{
		private readonly float treeShakeChance;
		private readonly float baseRangeChance;
		private readonly int minRangeLevel;
		private readonly int maxRangeLevel;
		private readonly int minGold;
		private readonly int maxGold;

		private const string EventTitle = "Look up!";

		public LookUp() : base(Settings.Settings.RandomEvents.LookUpData)
		{
			treeShakeChance = Settings.Settings.RandomEvents.LookUpData.treeShakeChance;
			baseRangeChance = Settings.Settings.RandomEvents.LookUpData.baseRangeChance;
			minRangeLevel = Settings.Settings.RandomEvents.LookUpData.minRangeLevel;
			maxRangeLevel = Settings.Settings.RandomEvents.LookUpData.maxRangeLevel;
			minGold = Settings.Settings.RandomEvents.LookUpData.minGold;
			maxGold = Settings.Settings.RandomEvents.LookUpData.maxGold;
		}

		public override void CancelEvent()
		{
		}

		public override bool CanExecuteEvent()
		{
			return true;
		}

		public override void StartEvent()
		{
			if (Settings.Settings.GeneralSettings.DebugMode)
			{
				InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}", RandomEventsSubmodule.TextColor));
			}

			List<InquiryElement> inquiryElements = new List<InquiryElement>();
			inquiryElements.Add(new InquiryElement("a", "Shake the tree", null));
			inquiryElements.Add(new InquiryElement("b", "Leave it be", null));

			if (PlayerStatus.HasRangedWeaponEquipped())
			{
				inquiryElements.Add(new InquiryElement("c", "Use ranged weapon", null));
			}

			MultiSelectionInquiryData msid = new MultiSelectionInquiryData(
				EventTitle, // Title
				"While walking past some trees you notice something shiny high up in its branches.", // Description
				inquiryElements, // Options
				false, // Can close menu without selecting an option. Should always be false.
				1, // Force a single option to be selected. Should usually be true
				"Okay", // The text on the button that continues the event
				null, // The text to display on the "cancel" button, shouldn't ever need it.
				(elements) => // How to handle the selected option. Will only ever be a single element unless force single option is off.
				{
					switch ((string)elements[0].Identifier)
					{
						case "a" when MBRandom.RandomFloat <= treeShakeChance:
						{
							// Success, calculate gold
							int goldGained = MBRandom.RandomInt(minGold, maxGold);
							Hero.MainHero.ChangeHeroGold(goldGained);

							InformationManager.ShowInquiry(new InquiryData(EventTitle, $"You eventually shake the shiny object free from the tree! It hits the ground with a heavy thunk. It turns out that it was a purse with {goldGained} gold inside.", true, false, "Done", null, null, null), true);
							break;
						}
						case "a":
							// Failure
							InformationManager.ShowInquiry(new InquiryData(EventTitle, $"Try as you might, you're unable to get dislodge the shiny object.", true, false, "Done", null, null, null), true);
							break;
						case "b":
							InformationManager.ShowInquiry(new InquiryData(EventTitle, $"You decide to leave the tree alone. Throughout the next few hours you can't help but wonder it was...", true, false, "Done", null, null, null), true);
							break;
						case "c":
						{
							//Get weapon
							SkillObject skillToUse = GetSkillObject();

							if (skillToUse == null)
							{
								InformationManager.ShowInquiry(new InquiryData(EventTitle, $"Something went wrong with selecting your weapon, what have you done?! Aborting event.", true, false, "Sorry", null, null, null), true);
								return;
							}

							//Check for success
							float chancePercent = 0;

							if (Hero.MainHero.GetSkillValue(skillToUse) < minRangeLevel)
							{
								chancePercent = 0;
							}
							else
							{
								var heroSkillValue = Hero.MainHero.GetSkillValue(skillToUse);
								chancePercent = ((float)heroSkillValue - minRangeLevel) / (maxRangeLevel - minRangeLevel);
								chancePercent = MathF.Clamp(chancePercent, baseRangeChance, 1.0f);
							}						

							if (MBRandom.RandomFloat < chancePercent)
							{
								// Success -- Add gold and xp
								int goldGained = MBRandom.RandomInt(minGold, maxGold);
								Hero.MainHero.ChangeHeroGold(goldGained);

								Hero.MainHero.AddSkillXp(skillToUse, Settings.Settings.GeneralSettings.GeneralLevelXpMultiplier * Hero.MainHero.GetSkillValue(skillToUse));

								InformationManager.ShowInquiry(new InquiryData(EventTitle, $"You manage to knock the shiny object out of the tree with (what you consider) a fantastic shot! Shame no one was there to see it. You notice that object was in fact a purse full of {goldGained} gold!", true, false, "Done", null, null, null), true);
							}
							else
							{
								// Failure

								ItemObject meat = MBObjectManager.Instance.GetObject<ItemObject>("meat");
								MobileParty.MainParty.ItemRoster.AddToCounts(meat, 1);

								InformationManager.ShowInquiry(new InquiryData(EventTitle, "Shot after shot you attempt to knock down the object without success. At one stage a bird drops out of the tree. It's time to give up... At least you have dinner.", true, false, "Done", null, null, null), true);
							}

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

		/// <summary>
		/// Gets which skill to use for the event
		/// </summary>
		private SkillObject GetSkillObject()
		{
			Equipment playerEquipment = Hero.MainHero.BattleEquipment;

			// Go through the player's weapons and get the first ranged one.
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
			{
				ItemObject item = playerEquipment[equipmentIndex].Item;

				if (item != null && item.Type == ItemObject.ItemTypeEnum.Thrown)
				{
					return DefaultSkills.Throwing;
				}
				else if (item != null && item.Type == ItemObject.ItemTypeEnum.Bow)
				{
					return DefaultSkills.Bow;
				}
				else if  (item != null && item.Type == ItemObject.ItemTypeEnum.Crossbow)
				{
					return DefaultSkills.Crossbow;
				}
			}
			return null;
		}
	}

	public class LookUpData : RandomEventData
	{
		public readonly float treeShakeChance; // Chance player will successfully shake the gold out of the tree
		public readonly float baseRangeChance; // Chance player will be able to get gold out of the tree with ranged weapon at minimum skill level
		public readonly int minRangeLevel; // Below, the player will always miss
		public readonly int maxRangeLevel; // At or above, the player will always succeed
		public readonly int minGold;
		public readonly int maxGold;

		public LookUpData(string eventType, float chanceWeight, float treeShakeChance, float baseRangeChance, int minRangeLevel, int maxRangeLevel, int minGold, int maxGold) : base(eventType, chanceWeight)
		{
			this.treeShakeChance = treeShakeChance;
			this.baseRangeChance = baseRangeChance;
			this.minRangeLevel = minRangeLevel;
			this.maxRangeLevel = maxRangeLevel;
			this.minGold = minGold;
			this.maxGold = maxGold;
		}

		public override BaseEvent GetBaseEvent()
		{
			return new LookUp();
		}
	}
}
