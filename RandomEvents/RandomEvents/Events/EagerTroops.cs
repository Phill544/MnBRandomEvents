using CryingBuffalo.RandomEvents.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace CryingBuffalo.RandomEvents.Events
{
	public sealed class EagerTroops : BaseEvent
	{
		private readonly int minTroopGain;
		private readonly int maxTroopGain;

		private const string EventTitle = "Eager Troops!";

		public EagerTroops() : base(Settings.Settings.RandomEvents.EagerTroopsData)
		{
			minTroopGain = Settings.Settings.RandomEvents.EagerTroopsData.minTroopGain;
			maxTroopGain = Settings.Settings.RandomEvents.EagerTroopsData.maxTroopGain;
		}

		public override void CancelEvent()
		{
		}

		public override bool CanExecuteEvent()
		{
			return MobileParty.MainParty.Party.PartySizeLimit >= MobileParty.MainParty.MemberRoster.TotalHealthyCount + minTroopGain;
		}

		public override void StartEvent()
		{
			if (Settings.Settings.GeneralSettings.DebugMode)
			{
				InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}", RandomEventsSubmodule.TextColor));
			}

			var realMaxTroopGain = Math.Min(MobileParty.MainParty.Party.PartySizeLimit - MobileParty.MainParty.MemberRoster.TotalHealthyCount, maxTroopGain);
			var numberToAdd = MBRandom.RandomInt(minTroopGain, realMaxTroopGain);

			var settlements = Settlement.FindAll((s) => !s.IsHideout).ToList();
			var closestSettlement = settlements.MinBy((s) => MobileParty.MainParty.GetPosition().DistanceSquared(s.GetPosition()));

			var inquiryElements = new List<InquiryElement>();
			inquiryElements.Add(new InquiryElement("a", "Accept", null));
			inquiryElements.Add(new InquiryElement("b", "Decline", null));

			var msid = new MultiSelectionInquiryData(
				EventTitle, // Title
				$"You come across {numberToAdd} troops that are eager for battle and glory. They want to join your ranks!", // Description
				inquiryElements, // Options
				false, // Can close menu without selecting an option. Should always be false.
				1, // Force a single option to be selected. Should usually be true
				"Okay", // The text on the button that continues the event
				null, // The text to display on the "cancel" button, shouldn't ever need it.
				(elements) => // How to handle the selected option. Will only ever be a single element unless force single option is off.
				{
					switch ((string)elements[0].Identifier)
					{
						case "a":
						{
							var bandits = PartySetup.CreateBanditParty();
							bandits.MemberRoster.Clear();
							PartySetup.AddRandomCultureUnits(bandits, numberToAdd, closestSettlement.Culture);

							MobileParty.MainParty.MemberRoster.Add(bandits.MemberRoster);

							bandits.RemoveParty();
							break;
						}
						case "b":
							InformationManager.ShowInquiry(new InquiryData(EventTitle, "Disappointed, the soldiers leave.", true, false, "Done", null, null, null), true);
							break;
						default:
							MessageBox.Show($"Error while selecting option for \"{randomEventData.eventType}\"");
							break;
					}
				},
				null); // What to do on the "cancel" button, shouldn't ever need it.

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
	}

	public class EagerTroopsData : RandomEventData
	{
		public readonly int minTroopGain;
		public readonly int maxTroopGain;

		public EagerTroopsData(string eventType, float chanceWeight, int minTroopGain, int maxTroopGain) : base(eventType, chanceWeight)
		{
			this.minTroopGain = minTroopGain;
			this.maxTroopGain = maxTroopGain;
		}

		public override BaseEvent GetBaseEvent()
		{
			return new EagerTroops();
		}
	}
}
