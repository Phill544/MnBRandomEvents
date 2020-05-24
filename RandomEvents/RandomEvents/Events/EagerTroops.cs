using CryingBuffalo.RandomEvents.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.TwoDimension;

namespace CryingBuffalo.RandomEvents.Events
{
	public class EagerTroops : BaseEvent
	{
		private int minTroopGain;
		private int maxTroopGain;

		private string eventTitle = "Des troupes désireuses!";

		public EagerTroops() : base(Settings.RandomEvents.EagerTroopsData)
		{
			minTroopGain = Settings.RandomEvents.EagerTroopsData.minTroopGain;
			maxTroopGain = Settings.RandomEvents.EagerTroopsData.maxTroopGain;
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
			if (Settings.GeneralSettings.DebugMode)
			{
				InformationManager.DisplayMessage(new InformationMessage($"Starting {this.RandomEventData.EventType}", RandomEventsSubmodule.textColor));
			}

			int realMaxTroopGain = (int)Mathf.Min(MobileParty.MainParty.Party.PartySizeLimit - MobileParty.MainParty.MemberRoster.TotalHealthyCount, maxTroopGain);
			int numberToAdd = MBRandom.RandomInt(minTroopGain, realMaxTroopGain);

			List<Settlement> settlements = Settlement.FindAll((s) => { return !s.IsHideout(); }).ToList();
			Settlement closestSettlement = settlements.MinBy((s) => { return MobileParty.MainParty.GetPosition().DistanceSquared(s.GetPosition()); });

			List<InquiryElement> inquiryElements = new List<InquiryElement>();
			inquiryElements.Add(new InquiryElement("a", "Accepté", null));
			inquiryElements.Add(new InquiryElement("b", "Décliné", null));

			MultiSelectionInquiryData msid = new MultiSelectionInquiryData(
				eventTitle, // Title
				$"Vous rencontrez {numberToAdd} guerrier avides de bataille et de gloire. Ils veulent rejoindre vos rangs!", // Description
				inquiryElements, // Options
				false, // Can close menu without selecting an option. Should always be false.
				true, // Force a single option to be selected. Should usually be true
				"Okay", // The text on the button that continues the event
				null, // The text to display on the "cancel" button, shouldn't ever need it.
				(elements) => // How to handle the selected option. Will only ever be a single element unless force single option is off.
				{
					if ((string)elements[0].Identifier == "a")
					{


						MobileParty bandits = PartySetup.CreateBanditParty();
						bandits.MemberRoster.Clear();
						PartySetup.AddRandomCultureUnits(bandits, numberToAdd, closestSettlement.Culture);

						MobileParty.MainParty.MemberRoster.Add(bandits.MemberRoster);

						bandits.RemoveParty();
					}
					else if ((string)elements[0].Identifier == "b")
					{
						InformationManager.ShowInquiry(new InquiryData(eventTitle, "Déçus, les soldats partent.", true, false, "Terminé", null, null, null), true);
					}
					else
					{
						MessageBox.Show($"Erreur lors de la sélection de l'option pour \"{this.RandomEventData.EventType}\"");
					}

				},
				null); // What to do on the "cancel" button, shouldn't ever need it.

			InformationManager.ShowMultiSelectionInquiry(msid, true);

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
	}

	public class EagerTroopsData : RandomEventData
	{
		public int minTroopGain;
		public int maxTroopGain;

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
