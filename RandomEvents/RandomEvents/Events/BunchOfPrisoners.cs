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
	public sealed class BunchOfPrisoners : BaseEvent
	{
		private readonly bool eventDisabled;
		private readonly int minPrisonerGain;
		private readonly int maxPrisonerGain;

		public BunchOfPrisoners() : base(ModSettings.RandomEvents.BunchOfPrisonersData)
		{
			var ConfigFile = new IniFile(ParseIniFile.GetTheConfigFile());
            
			eventDisabled = ConfigFile.ReadBoolean("BunchOfPrisoners", "EventDisabled");
			minPrisonerGain = ConfigFile.ReadInteger("BunchOfPrisoners", "MinPrisonerGain");
			maxPrisonerGain = ConfigFile.ReadInteger("BunchOfPrisoners", "MaxPrisonerGain");
		}

		public override void CancelEvent()
		{
		}
		
		private bool HasValidEventData()
		{
			if (eventDisabled == false)
			{
				if (minPrisonerGain != 0 || maxPrisonerGain != 0)
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
			var prisonerAmount = MBRandom.RandomInt(minPrisonerGain, maxPrisonerGain);
			var settlement = GetRandomSettlement();

			var prisoners = PartySetup.CreateBanditParty();
			prisoners.MemberRoster.Clear();
			PartySetup.AddRandomCultureUnits(prisoners, prisonerAmount, GetCultureToSpawn());

			settlement.Party.AddPrisoners(prisoners.MemberRoster);
				
			prisoners.RemoveParty();

			var settlementName = settlement.Name;
			
			var eventTitle = new TextObject("{=BunchOfPrisoners_Title}Bunch of Prisoners").ToString();
			
			var eventOption1 = new TextObject("{=BunchOfPrisoners_Event_Text}You receive word that your guards have expertly stopped a force inciting violence at {settlementName}, they have been put in cells.")
				.SetTextVariable("settlementName", settlementName)
				.ToString();
				
			var eventButtonText = new TextObject("{=BunchOfPrisoners_Event_Button_Text}Done").ToString();

			InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOption1, true, false, eventButtonText, null, null, null), true);

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

		private static Settlement GetRandomSettlement()
		{
			var eligibleSettlements = Hero.MainHero.Clan.Settlements.Where(s => s.IsTown || s.IsCastle).ToList();
			
			var index = MBRandom.RandomInt(0, eligibleSettlements.Count);

			return eligibleSettlements[index];
		}

		private static CultureObject GetCultureToSpawn()
		{
			var factionsAtWar = Campaign.Current.Factions.Where(faction => Hero.MainHero.Clan.IsAtWarWith(faction) && !faction.IsBanditFaction).ToList();

			if (factionsAtWar.Count != 0) return RandomSelection<IFaction>.GetRandomElement(factionsAtWar).Culture;
			// The player isn't at war with anyone, we'll spawn bandits.
			var hideouts = Settlement.FindAll(s => s.IsHideout).ToList();
			var closestHideout = hideouts.MinBy(s => MobileParty.MainParty.GetPosition().DistanceSquared(s.GetPosition()));
			return closestHideout.Culture;
		}
	}

	public class BunchOfPrisonersData : RandomEventData
	{
		public BunchOfPrisonersData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
		{
		}

		public override BaseEvent GetBaseEvent()
		{
			return new BunchOfPrisoners();
		}
	}
}
