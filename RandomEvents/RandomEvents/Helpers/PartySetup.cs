using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace CryingBuffalo.RandomEvents.Helpers
{
	public static class PartySetup
	{

		public static MobileParty CreateBanditParty(string cultureObjectID = null, string partyName = null)
		{
			MobileParty banditParty = null;

			try
			{
				List<Settlement> hideouts = Settlement.FindAll((s) => { return s.IsHideout(); }).ToList();
				Settlement closestHideout = hideouts.MinBy((s) => { return MobileParty.MainParty.GetPosition().DistanceSquared(s.GetPosition()); });

				CultureObject banditCultureObject = null;
				if (cultureObjectID != null)
				{
					banditCultureObject = MBObjectManager.Instance.GetObject<CultureObject>(cultureObjectID);
				}
				else
				{
					banditCultureObject = closestHideout.Culture;
				}

				if (partyName == null)
				{
					partyName = $"{banditCultureObject.Name} (Random Event)";
				}

				PartyTemplateObject partyTemplate = MBObjectManager.Instance.GetObject<PartyTemplateObject>($"{banditCultureObject.StringId}_template");
				partyTemplate.IncrementNumberOfCreated();
				banditParty = MBObjectManager.Instance.CreateObject<MobileParty>($"randomevent_{banditCultureObject.StringId}_{partyTemplate.NumberOfCreated}");
				TextObject partyNameTextObject = new TextObject(partyName, null);
				Clan banditClan = Clan.BanditFactions.FirstOrDefault(clan => clan.StringId == banditCultureObject.StringId);
				banditParty.InitializeMobileParty(partyNameTextObject, partyTemplate, MobileParty.MainParty.Position2D, 0.2f, 0.1f);

				banditParty.HomeSettlement = closestHideout;

				banditParty.Party.Owner = banditClan.Heroes.ToList()[0];
				banditParty.Party.Owner.Clan = banditClan;
				banditParty.ChangePartyLeader(banditClan.Leader.CharacterObject);
				banditClan.AddParty(banditParty.Party);				
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
			CultureObject partyCultureObject = null;
			if (overrideCulture != null)
				partyCultureObject = overrideCulture;
			else
				partyCultureObject = party.Party.Culture;

			// Get possible units to create
			List<CharacterObject> characterObjectList = null;
			if (partyCultureObject.IsBandit)
			{
				characterObjectList = GetBanditCharacters(partyCultureObject);
			}
			else
			{
				characterObjectList = GetMainCultureCharacters(partyCultureObject);
			}
			
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
				CharacterObject characterObject = (CharacterObject)characterObjectList[i];
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
