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

		public static MobileParty CreateBanditParty(string cultureObjectID = null)
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
				
				PartyTemplateObject partyTemplate = new PartyTemplateObject();
				partyTemplate = banditCultureObject.BanditBossPartyTemplate;
				partyTemplate.IncrementNumberOfCreated();
				banditParty = MBObjectManager.Instance.CreateObject<MobileParty>($"randomevent_{banditCultureObject.StringId}_{partyTemplate.NumberOfCreated}");
				TextObject partyName = new TextObject("Bandits (Random Event)", null);
				Clan banditClan = Clan.BanditFactions.FirstOrDefault(clan => clan.StringId == banditCultureObject.StringId);
				banditParty.InitializeMobileParty(partyName, partyTemplate, MobileParty.MainParty.Position2D, 0.6f, 0.4f);

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
				MessageBox.Show($"Adding random culture units currently only supports bandits!");
			}

			// Split spawn based on number to add
			int[] spawnNumbers = new int[characterObjectList.Count];
			int currentSpawned = 0;
			for (int i = 0; i < spawnNumbers.Length; i++)
			{
				int randomInt = MBRandom.RandomInt(0, numberToAdd - currentSpawned);
				spawnNumbers[i] = randomInt;
				currentSpawned += randomInt;
			}
			spawnNumbers[0] += numberToAdd - currentSpawned;

			for (int i = 0; i < characterObjectList.Count; i++)
			{
				CharacterObject characterObject = (CharacterObject)characterObjectList[i];
				party.AddElementToMemberRoster(characterObject, spawnNumbers[i]);
			}
		}

		private static List<CharacterObject> GetBanditCharacters(CultureObject partyCultureObject)
		{
			List<CharacterObject> characterObjectList = new List<CharacterObject>();

			characterObjectList.Add(partyCultureObject.BanditBandit);
			characterObjectList.Add(partyCultureObject.BanditRaider);
			characterObjectList.Add(partyCultureObject.BanditChief);

			return characterObjectList;
		}

	}
}
