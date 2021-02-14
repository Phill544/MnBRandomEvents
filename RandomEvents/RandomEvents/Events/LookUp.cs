using CryingBuffalo.RandomEvents.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

namespace CryingBuffalo.RandomEvents.Events
{
	public class LookUp : BaseEvent
	{
		private float treeShakeChance;
		private float baseRangeChance;
		private int minRangeLevel;
		private int maxRangeLevel;
		private int minGold;
		private int maxGold;

		private string eventTitle = "Look up!";

		public LookUp() : base(Settings.RandomEvents.LookUpData)
		{
			this.treeShakeChance = Settings.RandomEvents.LookUpData.treeShakeChance;
			this.baseRangeChance = Settings.RandomEvents.LookUpData.baseRangeChance;
			this.minRangeLevel = Settings.RandomEvents.LookUpData.minRangeLevel;
			this.maxRangeLevel = Settings.RandomEvents.LookUpData.maxRangeLevel;
			this.minGold = Settings.RandomEvents.LookUpData.minGold;
			this.maxGold = Settings.RandomEvents.LookUpData.maxGold;
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
			if (Settings.GeneralSettings.DebugMode)
			{
				InformationManager.DisplayMessage(new InformationMessage($"Starting {this.RandomEventData.EventType}", RandomEventsSubmodule.textColor));
			}

			List<InquiryElement> inquiryElements = new List<InquiryElement>();
			inquiryElements.Add(new InquiryElement("a", "Shake the tree", null));
			inquiryElements.Add(new InquiryElement("b", "Leave it be", null));

			if (PlayerStatus.HasRangedWeaponEquipped())
			{
				inquiryElements.Add(new InquiryElement("c", "Use ranged weapon", null));
			}

			MultiSelectionInquiryData msid = new MultiSelectionInquiryData(
				eventTitle, // Title
				"While walking past some trees you notice something shiny high up in its branches.", // Description
				inquiryElements, // Options
				false, // Can close menu without selecting an option. Should always be false.
				1, // Force a single option to be selected. Should usually be true
				"Okay", // The text on the button that continues the event
				null, // The text to display on the "cancel" button, shouldn't ever need it.
				(elements) => // How to handle the selected option. Will only ever be a single element unless force single option is off.
				{
					if ((string)elements[0].Identifier == "a")
					{
						if (MBRandom.RandomFloat <= treeShakeChance)
						{
							// Success, calculate gold
							int goldGained = MBRandom.RandomInt(minGold, maxGold);
							Hero.MainHero.ChangeHeroGold(goldGained);

							InformationManager.ShowInquiry(new InquiryData(eventTitle, $"You eventually shake the shiny object free from the tree! It hits the ground with a heavy thunk. It turns out that it was a purse with {goldGained} gold inside.", true, false, "Done", null, null, null), true);
						}
						else
						{
							// Failure
							InformationManager.ShowInquiry(new InquiryData(eventTitle, $"Try as you might, you're unable to get dislodge the shiny object.", true, false, "Done", null, null, null), true);
						}
					}
					else if ((string)elements[0].Identifier == "b")
					{
						InformationManager.ShowInquiry(new InquiryData(eventTitle, $"You decide to leave the tree alone. Throughout the next few hours you can't help but wonder it was...", true, false, "Done", null, null, null), true);
					}
					else if ((string)elements[0].Identifier == "c")
					{
						//Get weapon
						SkillObject skillToUse = GetSkillObject();

						if (skillToUse == null)
						{
							InformationManager.ShowInquiry(new InquiryData(eventTitle, $"Something went wrong with selecting your weapon, what have you done?! Aborting event.", true, false, "Sorry", null, null, null), true);
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

							Hero.MainHero.AddSkillXp(skillToUse, Settings.GeneralSettings.GeneralLevelXpMultiplier * Hero.MainHero.GetSkillValue(skillToUse));

							InformationManager.ShowInquiry(new InquiryData(eventTitle, $"You manage to knock the shiny object out of the tree with (what you consider) a fantastic shot! Shame no one was there to see it. You notice that object was in fact a purse full of {goldGained} gold!", true, false, "Done", null, null, null), true);
						}
						else
						{
							// Failure

							ItemObject meat = MBObjectManager.Instance.GetObject<ItemObject>("meat");
							MobileParty.MainParty.ItemRoster.AddToCounts(meat, 1);

							InformationManager.ShowInquiry(new InquiryData(eventTitle, $"Shot after shot you attempt to knock down the object without success. At one stage a bird drops out of the tree. It's time to give up... At least you have dinner.", true, false, "Done", null, null, null), true);
						}

					}
					else
					{
						MessageBox.Show($"Error while selecting option for \"{this.RandomEventData.EventType}\"");
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
		public float treeShakeChance; // Chance player will successfully shake the gold out of the tree
		public float baseRangeChance; // Chance player will be able to get gold out of the tree with ranged weapon at minimum skill level
		public int minRangeLevel; // Below, the player will always miss
		public int maxRangeLevel; // At or above, the player will always succeed
		public int minGold;
		public int maxGold;

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
