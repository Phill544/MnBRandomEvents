using System;
using System.Linq;
using System.Windows;
using Bannerlord.RandomEvents.Helpers;
using Bannerlord.RandomEvents.Settings;
using Ini.Net;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace Bannerlord.RandomEvents.Events
{
	public sealed class DiseasedCity : BaseEvent
	{
		private readonly bool eventDisabled;
		private readonly float baseSuccessChance;
		private readonly float highMedicineChance;
		private readonly int highMedicineLevel;
		private readonly float percentLoss;

		public DiseasedCity() : base(ModSettings.RandomEvents.DiseasedCityData)
		{
			var ConfigFile = new IniFile(ParseIniFile.GetTheConfigFile());
            
			eventDisabled = ConfigFile.ReadBoolean("DiseasedCity", "EventDisabled");
			baseSuccessChance = ConfigFile.ReadFloat("DiseasedCity", "BaseSuccessChance");
			highMedicineChance = ConfigFile.ReadFloat("DiseasedCity", "HighMedicineChance");
			highMedicineLevel = ConfigFile.ReadInteger("DiseasedCity", "HighMedicineLevel");
			percentLoss = ConfigFile.ReadFloat("DiseasedCity", "PercentLoss");
		}

		public override void CancelEvent()
		{
		}
		
		private bool HasValidEventData()
		{
			if (eventDisabled == false)
			{
				if (baseSuccessChance != 0 || highMedicineChance != 0|| highMedicineLevel != 0|| percentLoss != 0)
				{
					return true;
				}
			}
            
			return false;
		}

		public override bool CanExecuteEvent()
		{
			return HasValidEventData() && Hero.MainHero.Clan.Settlements.Any();
		}

		public override void StartEvent()
		{
			try
			{
				// The name of the settlement that receives the food

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
			
			var eventOutcome1 = new TextObject("{=DiseasedCity_Event_Choice_1}{plaguedSettlement} has suffered a devastating plague. Although {highestHero} tried their best to save the population, it wasn't enough...")
				.SetTextVariable("plaguedSettlement", plaguedSettlement.Name)
				.SetTextVariable("highestHero", highestMedicine?.Name)
				.ToString();
			
			var eventOutcome2 = new TextObject("{=DiseasedCity_Event_Choice_2}{plaguedSettlement} has suffered a devastating plague. As there wasn't anyone able to provide assistance to the population, the sickness cut through the population without mercy.")
				.SetTextVariable("plaguedSettlement", plaguedSettlement.Name)
				.ToString();
			
			var eventOutcome3 = new TextObject("{=DiseasedCity_Event_Choice_3}Although the telltale signs of an emerging plague started to appear in {plaguedSettlement}, because of {highestHero}'s expertise, measures were put in place that saved the settlement from unnecessary death.")
				.SetTextVariable("plaguedSettlement", plaguedSettlement.Name)
				.SetTextVariable("highestHero", highestMedicine?.Name)
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
                    plaguedSettlement.Town.GarrisonParty?.MemberRoster.KillNumberOfNonHeroTroopsRandomly((int)(plaguedSettlement.Town.GarrisonParty.MemberRoster.TotalManCount * percentLoss));

                    // Kill some of their Militia
					plaguedSettlement.MilitiaPartyComponent.Party.MemberRoster.KillNumberOfNonHeroTroopsRandomly((int)(plaguedSettlement.MilitiaPartyComponent.Party.MemberRoster.TotalManCount * percentLoss));

					// Drop some loyalty
					plaguedSettlement.Town.Loyalty *= 1 - percentLoss;

					// Give the hero half xp for trying
					var xpToGive = GeneralSettings.Basic.GetLevelXpMultiplier() * highestMedicineHero.GetSkillValue(DefaultSkills.Medicine) * 0.5f;

					highestMedicineHero.AddSkillXp(DefaultSkills.Medicine, xpToGive);

					InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOutcome1, true, false, eventButtonText, null, null, null), true);
				}
				else
				{
                    // Kill some of their Garrison
                    plaguedSettlement.Town.GarrisonParty?.MemberRoster.KillNumberOfNonHeroTroopsRandomly((int)(plaguedSettlement.Town.GarrisonParty.MemberRoster.TotalManCount * percentLoss));

                    // Kill some of their Militia
					plaguedSettlement.MilitiaPartyComponent.Party.MemberRoster.KillNumberOfNonHeroTroopsRandomly((int)(plaguedSettlement.MilitiaPartyComponent.Party.MemberRoster.TotalManCount * percentLoss));

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
					var xpToGive = GeneralSettings.Basic.GetLevelXpMultiplier() * highestMedicineHero.GetSkillValue(DefaultSkills.Medicine);

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
 