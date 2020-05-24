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
	public class TargetPractice : BaseEvent
	{
		private int minimumSoldiers;
		private float percentageDifferenceOfCurrentTroop;

		private string eventTitle = "Cible d'entrainement!";

		public TargetPractice() : base(Settings.RandomEvents.TargetPracticeData)
		{
			minimumSoldiers = Settings.RandomEvents.TargetPracticeData.minimumSoldiers;
			percentageDifferenceOfCurrentTroop = Settings.RandomEvents.TargetPracticeData.percentageDifferenceOfCurrentTroop;
		}

		public override void CancelEvent()
		{
		}

		public override bool CanExecuteEvent()
		{
			return MobileParty.MainParty.CurrentSettlement == null;
		}

		public override void StartEvent()
		{
			if (Settings.GeneralSettings.DebugMode)
			{
				InformationManager.DisplayMessage(new InformationMessage($"Starting {this.RandomEventData.EventType}", RandomEventsSubmodule.textColor));
			}

			float percentOffset = MBRandom.RandomFloatRanged(-percentageDifferenceOfCurrentTroop, percentageDifferenceOfCurrentTroop);

			int spawnCount = (int)(MobileParty.MainParty.MemberRoster.Count * ( 1 + percentOffset)) ;
			if (spawnCount < minimumSoldiers)
				spawnCount = minimumSoldiers;

			List<InquiryElement> inquiryElements = new List<InquiryElement>();
			inquiryElements.Add(new InquiryElement("a", "Laissez les soldats s'amuser!", null));
			inquiryElements.Add(new InquiryElement("b", "Ne rien faire.", null, true, "Pensez à l'expérience que vous abandonnez!"));

			MultiSelectionInquiryData msid = new MultiSelectionInquiryData(
				eventTitle, // Title
				CalculateDescription(spawnCount), // Description
				inquiryElements, // Options
				false, // Can close menu without selecting an option. Should always be false.
				true, // Force a single option to be selected. Should usually be true
				"Okay", // The text on the button that continues the event
				null, // The text to display on the "cancel" button, shouldn't ever need it.
				(elements) => // How to handle the selected option. Will only ever be a single element unless force single option is off.
				{
					if ((string)elements[0].Identifier == "a")
					{
						InformationManager.ShowInquiry(new InquiryData(eventTitle, "Les pillards ont l'air terrifiés.", true, false, "Bien", null, null, null), true);
						SpawnLooters(spawnCount);
					}
					else if ((string)elements[0].Identifier == "b")
					{
						InformationManager.ShowInquiry(new InquiryData(eventTitle, "Les pillards, voyant que vous n'êtes pas sur le point d'attaquer, se dispersent rapidement au quatre vents. Vos soldats se plaignent.", true, false, "Done", null, null, null), true);
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
				MessageBox.Show($"Erreur lors de l'arrêt \"{this.RandomEventData.EventType}\" event :\n\n {ex.Message} \n\n { ex.StackTrace}");
			}
		}

		private string CalculateDescription(int spawnCount)
		{
			string sizeDescription = "";
			if (spawnCount <= minimumSoldiers)
			{
				sizeDescription = "résonable";
			}
			else if (spawnCount < minimumSoldiers * 1.5f)
			{
				sizeDescription = "important";
			}
			else
			{
				sizeDescription = "énorme";
			}

			string description = $"Vous tombez sur un nombre {sizeDescription} de pillards! Vos soldats semblent très désireux de vous montrer ce qu'ils ont appris";

			return description;
		}

		private void SpawnLooters(int spawnCount)
		{
			MobileParty looterParty = PartySetup.CreateBanditParty("looters");

			looterParty.MemberRoster.Clear();

			looterParty.Aggressiveness = 10f;
			looterParty.SetMoveEngageParty(MobileParty.MainParty);

			PartySetup.AddRandomCultureUnits(looterParty, spawnCount);
		}
	}

	public class TargetPracticeData : RandomEventData
	{
		public int minimumSoldiers;

		public float percentageDifferenceOfCurrentTroop;

		public TargetPracticeData(string eventType, float chanceWeight, float percentageDifferenceOfCurrentTroop, int minimumSoldiers) : base(eventType, chanceWeight)
		{
			this.percentageDifferenceOfCurrentTroop = percentageDifferenceOfCurrentTroop;
			this.minimumSoldiers = minimumSoldiers;
		}

		public override BaseEvent GetBaseEvent()
		{
			return new TargetPractice();
		}
	}
}
