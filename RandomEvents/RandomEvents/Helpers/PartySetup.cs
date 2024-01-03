using System;
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

namespace Bannerlord.RandomEvents.Helpers
{
	public static class PartySetup
	{
		public static MobileParty CreateLooterParty(string partyName = null)
		{
			MobileParty banditParty = null;

			try
			{
				var hideouts = Settlement.FindAll(s => s.IsHideout).ToList();
				var closestHideout = hideouts.MinBy(s => MobileParty.MainParty.GetPosition().DistanceSquared(s.GetPosition()));

				var banditCultureObject = MBObjectManager.Instance.GetObject<CultureObject>("looters");

				partyName ??= $"{banditCultureObject.Name} (Random Event)";
				
				var partyTemplate = MBObjectManager.Instance.GetObject<PartyTemplateObject>($"{banditCultureObject.StringId}_template");
				
				banditParty = BanditPartyComponent.CreateLooterParty(
					$"randomevent_{banditCultureObject.StringId}_{MBRandom.RandomInt(int.MaxValue)}",
					closestHideout.OwnerClan,
					closestHideout,
					false);
				
				var partyNameTextObject = new TextObject(partyName);
				
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
			    var hideouts = Settlement.FindAll(s => s.IsHideout).ToList();
		        var closestHideout = hideouts.MinBy(s => MobileParty.MainParty.GetPosition().DistanceSquared(s.GetPosition()));

		        var banditCultureObject = cultureObjectId != null ? MBObjectManager.Instance.GetObject<CultureObject>(cultureObjectId) : closestHideout.Culture;

		        partyName ??= $"{banditCultureObject.Name} (Random Event)";

		        var partyTemplate = MBObjectManager.Instance.GetObject<PartyTemplateObject>($"{banditCultureObject.StringId}_template");

		        var matchedClan = Clan.BanditFactions.FirstOrDefault(clan => clan.DefaultPartyTemplate == partyTemplate);

		        // If no matching clan is found, select a random culture from the specified list
		        if (matchedClan == null)
		        {
		            var cultures = new List<string> { "Vlandia", "westernEmpire", "easternEmpire", "northernEmpire", "Khuzait", "Aserai", "Sturgia", "Battania" };
		            
		            var randomCulture = cultures[MBRandom.RandomInt(cultures.Count)];
		            
		            banditCultureObject = MBObjectManager.Instance.GetObject<CultureObject>(randomCulture);
		            partyTemplate = MBObjectManager.Instance.GetObject<PartyTemplateObject>($"{banditCultureObject.StringId}_template");
		            matchedClan = Clan.BanditFactions.FirstOrDefault(clan => clan.DefaultPartyTemplate == partyTemplate);

		            if (matchedClan == null)
		            {
		                MessageBox.Show($"Error: No matching clan found even with '{randomCulture}' culture.");
		                return null;
		            }
		        }

		        banditParty = BanditPartyComponent.CreateBanditParty(
		            $"randomevent_{banditCultureObject.StringId}_{MBRandom.RandomInt(int.MaxValue)}",
		            matchedClan,
		            closestHideout.Hideout, false);

		        var partyNameTextObject = new TextObject(partyName);

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
			try
			{
				// Get culture
				var partyCultureObject = overrideCulture ?? party.Party.Culture;

				// Get possible units to create
				var characterObjectList = partyCultureObject.IsBandit ? GetBanditCharacters(partyCultureObject) : GetMainCultureCharacters(partyCultureObject);

				// Split spawn based on number to add
				var spawnNumbers = new int[characterObjectList.Count];
				var currentSpawned = 0;

				while (currentSpawned < numberToAdd)
				{
					var randomInt = MBRandom.RandomInt(0, spawnNumbers.Length);
					spawnNumbers[randomInt]++;
					currentSpawned++;
				}

				for (var i = 0; i < characterObjectList.Count; i++)
				{
					var characterObject = characterObjectList[i];
					if (characterObject != null)
						party.AddElementToMemberRoster(characterObject, spawnNumbers[i]);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error while trying to add random culture units :\n\n {ex.Message} \n\n { ex.StackTrace}");
			}
		}

		private static List<CharacterObject> GetBanditCharacters(CultureObject partyCultureObject)
		{
			var characterObjectList = new List<CharacterObject>();

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
			var characterObjectList = new List<CharacterObject>();

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

		private static void CollectFromTroopTree(CharacterObject co, ICollection<CharacterObject> characterObjectList)
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
