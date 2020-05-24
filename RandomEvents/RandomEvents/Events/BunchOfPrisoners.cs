using CryingBuffalo.RandomEvents.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace CryingBuffalo.RandomEvents.Events
{
	public class BunchOfPrisoners : BaseEvent
	{
		private int minPrisonerGain;
		private int maxPrisonerGain;

		public BunchOfPrisoners() : base(Settings.RandomEvents.BunchOfPrisonersData)
		{
			this.minPrisonerGain = Settings.RandomEvents.BunchOfPrisonersData.minPrisonerGain;
			this.maxPrisonerGain = Settings.RandomEvents.BunchOfPrisonersData.maxPrisonerGain;
		}

		public override void CancelEvent()
		{
		}

		public override bool CanExecuteEvent()
		{
			return PlayerStatus.HasSettlement();
		}

		public override void StartEvent()
		{
			if (Settings.GeneralSettings.DebugMode)
			{
				InformationManager.DisplayMessage(new InformationMessage($"Starting {this.RandomEventData.EventType}", RandomEventsSubmodule.textColor));
			}

			int prisonerAmount = MBRandom.RandomInt(minPrisonerGain, maxPrisonerGain);
			Settlement settlement = GetRandomSettlement();

			MobileParty prisoners = PartySetup.CreateBanditParty();
			prisoners.MemberRoster.Clear();
			PartySetup.AddRandomCultureUnits(prisoners, prisonerAmount, GetCultureToSpawn());

			settlement.Party.AddPrisoners(prisoners.MemberRoster.ToFlattenedRoster());
				
			prisoners.RemoveParty();

			InformationManager.ShowInquiry(
				new InquiryData("Bunch of Prisoners",
					$"Vous recevez un message selon lequel vos gardes ont expertement arrêté une force incitant à la violence à {settlement.Name}, ils ont été placés dans des cellules",
					true,
					false,
					"Terminé",
					null,
					null,
					null
					),
				true);

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
				MessageBox.Show($"Erreur lors de l'arrêt \"{this.RandomEventData.EventType}\" event :\n\n {ex.Message} \n\n { ex.StackTrace}");
			}
		}

		private Settlement GetRandomSettlement()
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

		private CultureObject GetCultureToSpawn()
		{
			List<IFaction> factionsAtWar = new List<IFaction>();

			foreach (IFaction faction in Campaign.Current.Factions)
			{
				if (Hero.MainHero.Clan.IsAtWarWith(faction))
				{
					factionsAtWar.Add(faction);
				}
			}

			if (factionsAtWar.Count == 0)
			{
				// The player isn't at war with anyone, we'll spawn bandits.
				List<Settlement> hideouts = Settlement.FindAll((s) => { return s.IsHideout(); }).ToList();
				Settlement closestHideout = hideouts.MinBy((s) => { return MobileParty.MainParty.GetPosition().DistanceSquared(s.GetPosition()); });
				return closestHideout.Culture;
			}
			else
			{
				// Pick one of the factions to spawn prisoners of
				return RandomSelection<IFaction>.GetRandomElement(factionsAtWar).Culture;
			}
		}
	}

	public class BunchOfPrisonersData : RandomEventData
	{
		public int minPrisonerGain;
		public int maxPrisonerGain;

		public BunchOfPrisonersData(string eventType, float chanceWeight, int minPrisonerGain, int maxPrisonerGain) : base(eventType, chanceWeight)
		{
			this.minPrisonerGain = minPrisonerGain;
			this.maxPrisonerGain = maxPrisonerGain;
		}

		public override BaseEvent GetBaseEvent()
		{
			return new BunchOfPrisoners();
		}
	}
}
