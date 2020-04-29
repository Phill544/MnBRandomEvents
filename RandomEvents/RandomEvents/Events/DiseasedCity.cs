using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace CryingBuffalo.RandomEvents.Events
{
	public class DiseasedCity : BaseEvent
	{
		private float baseSuccessChance;
		private float highMedicineChance;
		private int highMedicineLevel;
		private float percentLoss;

		private string eventTitle = "It’s Airborne!";
		public DiseasedCity() : base(Settings.RandomEvents.DiseasedCityData)
		{
			baseSuccessChance = Settings.RandomEvents.DiseasedCityData.baseSuccessChance;
			highMedicineChance = Settings.RandomEvents.DiseasedCityData.highMedicineChance;
			highMedicineLevel = Settings.RandomEvents.DiseasedCityData.highMedicineLevel;
			percentLoss = Settings.RandomEvents.DiseasedCityData.percentLoss;
		}

		public override void CancelEvent()
		{
		}

		public override bool CanExecuteEvent()
		{
			if (Hero.MainHero.Clan.Settlements.Count() > 0)
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
			try
			{
				// The name of the settlement that receives the food
				string plaguedSettlementName = "";

				Settlement plaguedSettlement = GetPlaguedSettlement();

				Hero highestMedicineHero = null;

				if (plaguedSettlement.Town.Governor != null)
				{
					highestMedicineHero = plaguedSettlement.Town.Governor;
				}

				if (MobileParty.MainParty.CurrentSettlement == plaguedSettlement)
				{
					int surgeonSkill = MobileParty.MainParty.EffectiveSurgeon.GetSkillValue(DefaultSkills.Medicine);
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

				bool useSkill = false;
				if (highestMedicineHero != null && highestMedicineHero.GetSkillValue(DefaultSkills.Medicine) >= highMedicineLevel)
					useSkill = true;

				bool plagueKills = PlagueKills(useSkill);

				ResolvePlague(plagueKills, plaguedSettlement , highestMedicineHero);

				// set the name to display
				plaguedSettlementName = plaguedSettlement.Name.ToString();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error while running \"{this.RandomEventData.EventType}\" event :\n\n {ex.Message} \n\n { ex.StackTrace}");
			}

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

		private Settlement GetPlaguedSettlement()
		{
			if (MobileParty.MainParty.CurrentSettlement != null)
			{
				// If the player is in a settlement, use this one for the event so there's a higher chance a medic will help
				return MobileParty.MainParty.CurrentSettlement;
			}
			else
			{
				List<Settlement> eligibleSettlements = new List<Settlement>();

				foreach (Settlement s in Hero.MainHero.Clan.Settlements)
				{
					if (s.IsTown || s.IsCastle)
					{
						eligibleSettlements.Add(s);
					}
				}

				// Randomly pick one of the eligible settlements
				int index = MBRandom.RandomInt(0, eligibleSettlements.Count);

				return eligibleSettlements[index];
			}
		}

		private bool PlagueKills(bool useSkill)
		{
			if (useSkill)
			{
				if (MBRandom.RandomFloatRanged(0,1) >= highMedicineChance)
				{
					// Plague kills
					return true;
				}
				else
				{
					// Plague doesn't kill
					return false;
				}
			}
			else
			{
				if (MBRandom.RandomFloatRanged(0, 1) >= baseSuccessChance)
				{
					// Plague kills
					return true;
				}
				else
				{
					// Plague doesn't kill
					return false;
				}
			}
		}

		private void ResolvePlague(bool plagueKills, Settlement plaguedSettlement, Hero highestMedicineHero)
		{
			if (plagueKills)
			{
				if (highestMedicineHero != null)
				{
					// Hero tried their best

					// Kill some of their Garrison
					plaguedSettlement.Town.GarrisonParty.MemberRoster.KillNumberOfMenRandomly((int)(plaguedSettlement.Town.GarrisonParty.MemberRoster.TotalManCount * this.percentLoss), false);

					// Kill some of their Militia
					plaguedSettlement.Town.MilitiaParty.MemberRoster.KillNumberOfMenRandomly((int)(plaguedSettlement.Town.MilitiaParty.MemberRoster.TotalManCount * this.percentLoss), false);

					// Drop some loyalty
					plaguedSettlement.Town.Loyalty *= 1 - this.percentLoss;

					// Give the hero half xp for trying
					float xpToGive = Settings.GeneralSettings.GeneralLevelXpMultiplier * highestMedicineHero.GetSkillValue(DefaultSkills.Medicine) * 0.5f;

					highestMedicineHero.AddSkillXp(DefaultSkills.Medicine, xpToGive);

					InformationManager.ShowInquiry(
						new InquiryData(eventTitle,
							$"{plaguedSettlement} has suffered a devastating plague. Although {highestMedicineHero.Name} tried their best to save the population, it wasn't enough...",
							true,
							false,
							"Done",
							null,
							null,
							null
							), 
						true);
				}
				else
				{
					// Kill some of their Garrison
					plaguedSettlement.Town.GarrisonParty.MemberRoster.KillNumberOfMenRandomly((int)(plaguedSettlement.Town.GarrisonParty.MemberRoster.TotalManCount * this.percentLoss), false);

					// Kill some of their Militia
					plaguedSettlement.Town.MilitiaParty.MemberRoster.KillNumberOfMenRandomly((int)(plaguedSettlement.Town.MilitiaParty.MemberRoster.TotalManCount * this.percentLoss), false);

					// Drop some loyalty
					plaguedSettlement.Town.Loyalty *= 1 - this.percentLoss;

					// Lack of Hero meant they were left to fend for themselves
					InformationManager.ShowInquiry(
						new InquiryData(eventTitle,
							$"{plaguedSettlement} has suffered a devastating plague. As there wasn't anyone able to provide assistance to the population, the sickness cut through the population without mercy.",
							true,
							false,
							"Done",
							null,
							null,
							null
							),
						true);
				}
			}
			else
			{
				if (highestMedicineHero != null)
				{
					// Give the hero xp for saving the settlement
					float xpToGive = Settings.GeneralSettings.GeneralLevelXpMultiplier * highestMedicineHero.GetSkillValue(DefaultSkills.Medicine);

					highestMedicineHero.AddSkillXp(DefaultSkills.Medicine, xpToGive);

					// Thanks to hero's efforts lives were saved
					InformationManager.ShowInquiry(
						new InquiryData(eventTitle,
							$"Although the telltale signs of an emerging plague started to appear in {plaguedSettlement}, because of {highestMedicineHero.Name}'s expertise, measures were put in place that saved the settlement from unnecessary death.",
							true,
							false,
							"Done",
							null,
							null,
							null
							),
						true);
				}
				else
				{
					// Although there was no one to help, the population managed to subdue the sickness.
					InformationManager.ShowInquiry(
						new InquiryData(eventTitle,
							$"Although the telltale signs of an emerging plague starting to appear in {plaguedSettlement}, as luck would have it, nothing ever came of it. Those that were ill recovered, and the fears of a deadly pandemic can be laid to rest... For now.",
							true,
							false,
							"Done",
							null,
							null,
							null
							),
						true);
				}
			}
		}
	}

	public class DiseasedCityData : RandomEventData
	{
		public float baseSuccessChance;
		public float highMedicineChance;
		public int highMedicineLevel;
		public float percentLoss;
		public DiseasedCityData(RandomEventType eventType, float chanceWeight, float baseSuccessChance, float highMedicineChance, int highMedicineLevel, float percentLoss) : base(eventType, chanceWeight)
		{
			this.baseSuccessChance = baseSuccessChance;
			this.highMedicineChance = highMedicineChance;
			this.highMedicineLevel = highMedicineLevel;
			this.percentLoss = percentLoss;
		}
	}
}
