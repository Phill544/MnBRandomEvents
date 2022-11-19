﻿using System;
using System.Linq;
using System.Windows;
using CryingBuffalo.RandomEvents.Settings;
using CryingBuffalo.RandomEvents.Settings.MCM;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace CryingBuffalo.RandomEvents.Events
{
	public sealed class DiseasedCity : BaseEvent
	{
		private readonly float baseSuccessChance;
		private readonly float highMedicineChance;
		private readonly int highMedicineLevel;
		private readonly float percentLoss;

		public DiseasedCity() : base(ModSettings.RandomEvents.DiseasedCityData)
		{
			baseSuccessChance = MCM_MenuConfig_A_M.Instance.DC_BaseSuccessChance;
			highMedicineChance = MCM_MenuConfig_A_M.Instance.DC_HighMedicineChance;
			highMedicineLevel = MCM_MenuConfig_A_M.Instance.DC_HighMedicineLevel;
			percentLoss = MCM_MenuConfig_A_M.Instance.DC_PercentLoss;
		}

		public override void CancelEvent()
		{
		}

		public override bool CanExecuteEvent()
		{
			return MCM_MenuConfig_A_M.Instance.DC_Disable == false && Hero.MainHero.Clan.Settlements.Any();
		}

		public override void StartEvent()
		{
			if (MCM_ConfigMenu_General.Instance.GS_DebugMode)
			{
				InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}", RandomEventsSubmodule.Dbg_Color));
			}
			try
			{
				
				var plaguedSettlement = GetPlaguedSettlement();

				Hero highestMedicineHero = null;

				if (plaguedSettlement.Town.Governor != null)
				{
					highestMedicineHero = plaguedSettlement.Town.Governor;
				}

				if (MobileParty.MainParty.CurrentSettlement == plaguedSettlement)
				{
					var surgeonSkill = MobileParty.MainParty.EffectiveSurgeon.GetSkillValue(DefaultSkills.Medicine);
					if (highestMedicineHero != null)
					{
						if (surgeonSkill >= highestMedicineHero.GetSkillValue(DefaultSkills.Medicine))
						{
							highestMedicineHero = MobileParty.MainParty.EffectiveSurgeon;
						}
					}
					else
					{
						highestMedicineHero = MobileParty.MainParty.EffectiveSurgeon;
					}
				}

				var useSkill = highestMedicineHero != null && highestMedicineHero.GetSkillValue(DefaultSkills.Medicine) >= highMedicineLevel;

				var plagueKills = PlagueKills(useSkill);

				ResolvePlague(plagueKills, plaguedSettlement , highestMedicineHero);
				
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error while running \"{randomEventData.eventType}\" event :\n\n {ex.Message} \n\n { ex.StackTrace}");
			}

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

		private static Settlement GetPlaguedSettlement()
		{
			if (MobileParty.MainParty.CurrentSettlement != null)
			{
				// If the player is in a settlement, use this one for the event so there's a higher chance a medic will help
				return MobileParty.MainParty.CurrentSettlement;
			}

			var eligibleSettlements = Hero.MainHero.Clan.Settlements.Where(s => s.IsTown || s.IsCastle).ToList();

			// Randomly pick one of the eligible settlements
			var index = MBRandom.RandomInt(0, eligibleSettlements.Count);

			return eligibleSettlements[index];
		}

		private bool PlagueKills(bool useSkill)
		{
			if (useSkill)
			{
				return MBRandom.RandomFloatRanged(0,1) >= highMedicineChance;
			}

			return MBRandom.RandomFloatRanged(0, 1) >= baseSuccessChance;
		}

		private void ResolvePlague(bool plagueKills, Settlement plaguedSettlement, Hero highestMedicineHero)
		{

			var highestMedicine = highestMedicineHero;
			var eventTitle = new TextObject("{=DiseasedCity_Title}It's Airborne!").ToString();
			
			var eventOutcome1 = new TextObject("{=DiseasedCity_Event_Choice_1}{plaguedSettlement} has suffered a devastating plague. Although {highestMedicine} tried their best to save the population, it wasn't enough...")
				.SetTextVariable("plaguedSettlement", plaguedSettlement.Name)
				.SetTextVariable("highestHero", highestMedicine.Name)
				.ToString();
			
			var eventOutcome2 = new TextObject("{=DiseasedCity_Event_Choice_2}{plaguedSettlement} has suffered a devastating plague. As there wasn't anyone able to provide assistance to the population, the sickness cut through the population without mercy.")
				.SetTextVariable("plaguedSettlement", plaguedSettlement.Name)
				.ToString();
			
			var eventOutcome3 = new TextObject("{=DiseasedCity_Event_Choice_3}Although the telltale signs of an emerging plague started to appear in {plaguedSettlement}, because of {highestMedicine}'s expertise, measures were put in place that saved the settlement from unnecessary death.")
				.SetTextVariable("plaguedSettlement", plaguedSettlement.Name)
				.SetTextVariable("highestHero", highestMedicine.Name)
				.ToString();
			
			var eventOutcome4 = new TextObject("{=DiseasedCity_Event_Choice_4}Although the telltale signs of an emerging plague starting to appear in {plaguedSettlement}, as luck would have it, nothing ever came of it. Those that were ill recovered, and the fears of a deadly pandemic can be laid to rest... For now.")
				.SetTextVariable("plaguedSettlement", plaguedSettlement.Name)
				.ToString();
			
			var eventButtonText = new TextObject("{=DiseasedCity_Event_Button_Text}Done").ToString();
			
			if (plagueKills)
			{
				if (highestMedicineHero != null)
				{
                    // Hero tried their best

                    // Kill some of their Garrison
                    plaguedSettlement.Town.GarrisonParty?.MemberRoster.KillNumberOfMenRandomly((int)(plaguedSettlement.Town.GarrisonParty.MemberRoster.TotalManCount * percentLoss), false);

                    // Kill some of their Militia
					plaguedSettlement.MilitiaPartyComponent.Party.MemberRoster.KillNumberOfMenRandomly((int)(plaguedSettlement.MilitiaPartyComponent.Party.MemberRoster.TotalManCount * percentLoss), false);

					// Drop some loyalty
					plaguedSettlement.Town.Loyalty *= 1 - percentLoss;

					// Give the hero half xp for trying
					var xpToGive = MCM_ConfigMenu_General.Instance.GS_GeneralLevelXpMultiplier * highestMedicineHero.GetSkillValue(DefaultSkills.Medicine) * 0.5f;

					highestMedicineHero.AddSkillXp(DefaultSkills.Medicine, xpToGive);

					InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOutcome1, true, false, eventButtonText, null, null, null), true);
				}
				else
				{
                    // Kill some of their Garrison
                    plaguedSettlement.Town.GarrisonParty?.MemberRoster.KillNumberOfMenRandomly((int)(plaguedSettlement.Town.GarrisonParty.MemberRoster.TotalManCount * percentLoss), false);

                    // Kill some of their Militia
					plaguedSettlement.MilitiaPartyComponent.Party.MemberRoster.KillNumberOfMenRandomly((int)(plaguedSettlement.MilitiaPartyComponent.Party.MemberRoster.TotalManCount * percentLoss), false);

					// Drop some loyalty
					plaguedSettlement.Town.Loyalty *= 1 - percentLoss;

					// Lack of Hero meant they were left to fend for themselves
					InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOutcome2, true, false, eventButtonText, null, null, null), true);
				}
			}
			else
			{
				if (highestMedicineHero != null)
				{
					// Give the hero xp for saving the settlement
					var xpToGive = MCM_ConfigMenu_General.Instance.GS_GeneralLevelXpMultiplier * highestMedicineHero.GetSkillValue(DefaultSkills.Medicine);

					highestMedicineHero.AddSkillXp(DefaultSkills.Medicine, xpToGive);

					// Thanks to hero's efforts lives were saved
					InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOutcome3, true, false, eventButtonText, null, null, null), true);
				}
				else
				{
					// Although there was no one to help, the population managed to subdue the sickness.
					InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOutcome4, true, false, eventButtonText, null, null, null), true);
				}
			}
		}
	}

	public class DiseasedCityData : RandomEventData
	{
		public DiseasedCityData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
		{
		}

		public override BaseEvent GetBaseEvent()
		{
			return new DiseasedCity();
		}
	}
}
 