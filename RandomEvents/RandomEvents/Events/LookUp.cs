using System;
using System.Collections.Generic;
using System.Windows;
using CryingBuffalo.RandomEvents.Helpers;
using CryingBuffalo.RandomEvents.Settings;
using CryingBuffalo.RandomEvents.Settings.MCM;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
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

		public LookUp() : base(ModSettings.RandomEvents.LookUpData)
		{
			treeShakeChance = MCM_MenuConfig_G_O.Instance.LU_TreeShakeChance;
			baseRangeChance = MCM_MenuConfig_G_O.Instance.LU_BaseRangeChance;
			minRangeLevel = MCM_MenuConfig_G_O.Instance.LU_MinRangeLevel;
			maxRangeLevel = MCM_MenuConfig_G_O.Instance.LU_MaxRangeLevel;
			minGold = MCM_MenuConfig_G_O.Instance.LU_MinGold;
			maxGold = MCM_MenuConfig_G_O.Instance.LU_MaxGold;
		}

		public override void CancelEvent()
		{
		}

		public override bool CanExecuteEvent()
		{
			return MCM_MenuConfig_G_O.Instance.LU_Disable == false && MobileParty.MainParty.CurrentSettlement == null;
		}

		public override void StartEvent()
		{
			if (MCM_ConfigMenu_General.Instance.GS_DebugMode)
			{
				InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}", RandomEventsSubmodule.Dbg_Color));
			}
			
			var eventTitle = new TextObject("{=LookUp_Title}Look up!").ToString();
			
			var eventDescription = new TextObject("{=LookUp_Event_Desc}While walking past some trees you notice something shiny high up in its branches.")
				.ToString();
			
			var eventOption1 = new TextObject("{=LookUp_Event_Option_1}Shake the tree").ToString();
			var eventOption2 = new TextObject("{=LookUp_Event_Option_2}Leave it be").ToString();
			var eventOption3 = new TextObject("{=LookUp_Event_Option_3}Use ranged weapon").ToString();

			var eventButtonText1 = new TextObject("{=LookUp_Event_Button_Text_1}Okay").ToString();
			var eventButtonText2 = new TextObject("{=LookUp_Event_Button_Text_2}Done").ToString();
			var eventButtonText3 = new TextObject("{=LookUp_Event_Button_Text_3}Sorry").ToString();
			
			var inquiryElements = new List<InquiryElement>
			{
				new InquiryElement("a", eventOption1, null),
				new InquiryElement("b", eventOption2, null)
			};

			if (PlayerStatus.HasRangedWeaponEquipped())
			{
				inquiryElements.Add(new InquiryElement("c", eventOption3, null));
			}

			var msid = new MultiSelectionInquiryData(eventTitle, eventDescription, inquiryElements, false, 1, eventButtonText1, null, 
				elements => 
				{
					switch ((string)elements[0].Identifier)
					{
						case "a" when MBRandom.RandomFloat <= treeShakeChance:
						{
							// Success, calculate gold
							var goldGained = MBRandom.RandomInt(minGold, maxGold);
							Hero.MainHero.ChangeHeroGold(goldGained);
							
							var eventOutcome1 = new TextObject("{=LookUp_Event_Text_1}You eventually shake the shiny object free from the tree! It hits the ground with a heavy thunk. It turns out that it was a purse with {goldGained} gold inside.")
								.SetTextVariable("goldGained", goldGained)
								.ToString();

							InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOutcome1, true, false, eventButtonText2, null, null, null), true);
							break;
						}
						case "a":
							// Failure
							
							var eventOutcome2 = new TextObject("{=LookUp_Event_Text_2}Try as you might, you're unable to get dislodge the shiny object.")
								.ToString();
							
							InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOutcome2, true, false, eventButtonText2, null, null, null), true);
							break;
						case "b":
							var eventOutcome3 = new TextObject("{=LookUp_Event_Text_3}You decide to leave the tree alone. Throughout the next few hours you can't help but wonder it was...")
								.ToString();
							InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOutcome3, true, false, eventButtonText2, null, null, null), true);
							break;
						case "c":
						{
							//Get weapon
							SkillObject skillToUse = GetSkillObject();

							if (skillToUse == null)
							{
								var eventOutcome4 = new TextObject("{=LookUp_Event_Text_4}Something went wrong with selecting your weapon, what have you done?! Aborting event.")
									.ToString();
								
								InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOutcome4, true, false, eventButtonText3, null, null, null), true);
								return;
							}

							//Check for success
							float chancePercent;

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
								var goldGained = MBRandom.RandomInt(minGold, maxGold);
								Hero.MainHero.ChangeHeroGold(goldGained);

								Hero.MainHero.AddSkillXp(skillToUse, MCM_ConfigMenu_General.Instance.GS_GeneralLevelXpMultiplier * Hero.MainHero.GetSkillValue(skillToUse));

								
								var eventOutcome5 = new TextObject("{=LookUp_Event_Text_5}You manage to knock the shiny object out of the tree with (what you consider) a fantastic shot! Shame no one was there to see it. You notice that object was in fact a purse full of {goldGained} gold!")
									.SetTextVariable("goldGained", goldGained)
									.ToString();
								
								InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOutcome5, true, false, eventButtonText2, null, null, null), true);
							}
							else
							{
								// Failure

								ItemObject meat = MBObjectManager.Instance.GetObject<ItemObject>("meat");
								MobileParty.MainParty.ItemRoster.AddToCounts(meat, 1);
								
								var eventOutcome6 = new TextObject("{=LookUp_Event_Text_6}Shot after shot you attempt to knock down the object without success. At one stage a bird drops out of the tree. It's time to give up... At least you have dinner.")
									.ToString();

								InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOutcome6, true, false, eventButtonText2, null, null, null), true);
							}

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

		/// <summary>
		/// Gets which skill to use for the event
		/// </summary>
		private static SkillObject GetSkillObject()
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

				if (item != null && item.Type == ItemObject.ItemTypeEnum.Bow)
				{
					return DefaultSkills.Bow;
				}
				if  (item != null && item.Type == ItemObject.ItemTypeEnum.Crossbow)
				{
					return DefaultSkills.Crossbow;
				}
			}
			return null;
		}
	}

	public class LookUpData : RandomEventData
	{

		public LookUpData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
		{
		}

		public override BaseEvent GetBaseEvent()
		{
			return new LookUp();
		}
	}
}
