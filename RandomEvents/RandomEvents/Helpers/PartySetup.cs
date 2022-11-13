﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace CryingBuffalo.RandomEvents.Helpers
{
	public static class PartySetup
	{
		public static MobileParty CreateLooterParty(string partyName = null)
		{
			MobileParty banditParty = null;

			try
			{
				List<Settlement> hideouts = Settlement.FindAll((s) => s.IsHideout).ToList();
				Settlement closestHideout = hideouts.MinBy((s) => MobileParty.MainParty.GetPosition().DistanceSquared(s.GetPosition()));

				var banditCultureObject = MBObjectManager.Instance.GetObject<CultureObject>("looters");

				if (partyName == null)
					partyName = $"{banditCultureObject.Name} (Random Event)";
				
				PartyTemplateObject partyTemplate = MBObjectManager.Instance.GetObject<PartyTemplateObject>($"{banditCultureObject.StringId}_template");
				banditParty = BanditPartyComponent.CreateLooterParty(
					$"randomevent_{banditCultureObject.StringId}_{MBRandom.RandomInt(int.MaxValue)}",
					closestHideout.OwnerClan,
					closestHideout,
					false);
				TextObject partyNameTextObject = new TextObject(partyName);
				banditParty.InitializeMobilePartyAroundPosition(partyTemplate, MobileParty.MainParty.Position2D, 0.2f, 0.1f, 20);
				banditParty.SetCustomName(partyNameTextObject);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error while trying to create a mobile bandit party :\n\n {ex.Message} \n\n { ex.StackTrace}");
			}

			return banditParty;
		}

		public static MobileParty CreateBanditParty(string cultureObjectId = null, string partyName = null)
		{
			MobileParty banditParty = null;

			try
			{
				List<Settlement> hideouts = Settlement.FindAll((s) => s.IsHideout).ToList();
				Settlement closestHideout = hideouts.MinBy((s) => MobileParty.MainParty.GetPosition().DistanceSquared(s.GetPosition()));

				var banditCultureObject = cultureObjectId != null ? MBObjectManager.Instance.GetObject<CultureObject>(cultureObjectId) : closestHideout.Culture;

				if (partyName == null)
				{
					partyName = $"{banditCultureObject.Name} (Random Event)";
				}

				PartyTemplateObject partyTemplate = MBObjectManager.Instance.GetObject<PartyTemplateObject>($"{banditCultureObject.StringId}_template");
				banditParty = BanditPartyComponent.CreateBanditParty(
					$"randomevent_{banditCultureObject.StringId}_{MBRandom.RandomInt(int.MaxValue)}",
					Clan.BanditFactions.First(clan => clan.DefaultPartyTemplate == partyTemplate),
					closestHideout.Hideout,
					false);
				TextObject partyNameTextObject = new TextObject(partyName);
				banditParty.InitializeMobilePartyAroundPosition(partyTemplate, MobileParty.MainParty.Position2D, 0.2f, 0.1f, 20);
				banditParty.SetCustomName(partyNameTextObject);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error while trying to create a mobile bandit party :\n\n {ex.Message} \n\n { ex.StackTrace}");
			}

			return banditParty;
		}

		public static void AddRandomCultureUnits(MobileParty party, int numberToAdd, CultureObject overrideCulture = null)
		{
			// Get culture
			var partyCultureObject = overrideCulture ?? party.Party.Culture;

			// Get possible units to create
			var characterObjectList = partyCultureObject.IsBandit ? GetBanditCharacters(partyCultureObject) : GetMainCultureCharacters(partyCultureObject);

			// Split spawn based on number to add
			int[] spawnNumbers = new int[characterObjectList.Count];
			int currentSpawned = 0;

			while (currentSpawned < numberToAdd)
			{
				int randomInt = MBRandom.RandomInt(0, spawnNumbers.Length);
				spawnNumbers[randomInt]++;
				currentSpawned++;
			}

			for (int i = 0; i < characterObjectList.Count; i++)
			{
				CharacterObject characterObject = characterObjectList[i];
				party.AddElementToMemberRoster(characterObject, spawnNumbers[i]);
			}
		}

		private static List<CharacterObject> GetBanditCharacters(CultureObject partyCultureObject)
		{
			List<CharacterObject> characterObjectList = new List<CharacterObject>();

			if (partyCultureObject.StringId == "looters")
			{
				// We have to treat looters differently as they only have a single unit type compared to the other bandits.
				characterObjectList.Add(partyCultureObject.BasicTroop);
			}
			else
			{
				characterObjectList.Add(partyCultureObject.BanditBandit);
				characterObjectList.Add(partyCultureObject.BanditRaider);
				characterObjectList.Add(partyCultureObject.BanditChief);
			}

			return characterObjectList;
		}

		private static List<CharacterObject> GetMainCultureCharacters(CultureObject partyCultureObject)
		{
			List<CharacterObject> characterObjectList = new List<CharacterObject>();

			// Add basic troop
			if (partyCultureObject.BasicTroop != null)
			{
				characterObjectList.Add(partyCultureObject.BasicTroop);
			}
			else
			{
				return characterObjectList;
			}

			foreach (var upgradeTarget in partyCultureObject.BasicTroop.UpgradeTargets)
			{
				CollectFromTroopTree(upgradeTarget, characterObjectList);
			}

			return characterObjectList;
		}

		private static void CollectFromTroopTree(CharacterObject co, List<CharacterObject> characterObjectList)
		{
			if (co.UpgradeTargets == null || co.UpgradeTargets.Length == 0)
				return;

			foreach (var upgradeTarget in co.UpgradeTargets)
			{
				if(!characterObjectList.Contains(upgradeTarget))
					characterObjectList.Add(upgradeTarget);

				CollectFromTroopTree(upgradeTarget, characterObjectList);
			}
		}

	}
}
