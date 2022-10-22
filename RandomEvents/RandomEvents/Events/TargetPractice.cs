using CryingBuffalo.RandomEvents.Helpers;
using System;
using System.Collections.Generic;
using System.Windows;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace CryingBuffalo.RandomEvents.Events
{
	public sealed class TargetPractice : BaseEvent
	{
		private readonly int minimumSoldiers;
		private readonly float percentageDifferenceOfCurrentTroop;

		private const string EventTitle = "Target Practice!";

		public TargetPractice() : base(Settings.Settings.RandomEvents.TargetPracticeData)
		{
			minimumSoldiers = Settings.Settings.RandomEvents.TargetPracticeData.minimumSoldiers;
			percentageDifferenceOfCurrentTroop = Settings.Settings.RandomEvents.TargetPracticeData.percentageDifferenceOfCurrentTroop;
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
			if (Settings.Settings.GeneralSettings.DebugMode)
			{
				InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}", RandomEventsSubmodule.TextColor));
			}

			float percentOffset = MBRandom.RandomFloatRanged(-percentageDifferenceOfCurrentTroop, percentageDifferenceOfCurrentTroop);

			int spawnCount = (int)(MobileParty.MainParty.MemberRoster.Count * ( 1 + percentOffset)) ;
			if (spawnCount < minimumSoldiers)
				spawnCount = minimumSoldiers;

			List<InquiryElement> inquiryElements = new List<InquiryElement>();
			inquiryElements.Add(new InquiryElement("a", "Let the soldiers have some fun!", null));
			inquiryElements.Add(new InquiryElement("b", "Do nothing.", null, true, "Think about the experience you're giving up!"));

			MultiSelectionInquiryData msid = new MultiSelectionInquiryData(
				EventTitle, // Title
				CalculateDescription(spawnCount), // Description
				inquiryElements, // Options
				false, // Can close menu without selecting an option. Should always be false.
				1, // Force a single option to be selected. Should usually be true
				"Okay", // The text on the button that continues the event
				null, // The text to display on the "cancel" button, shouldn't ever need it.
				(elements) => // How to handle the selected option. Will only ever be a single element unless force single option is off.
				{
					if ((string)elements[0].Identifier == "a")
					{
						InformationManager.ShowInquiry(new InquiryData(EventTitle, "The looters look terrified.", true, false, "Good", null, null, null), true);
						SpawnLooters(spawnCount);
					}
					else if ((string)elements[0].Identifier == "b")
					{
						InformationManager.ShowInquiry(new InquiryData(EventTitle, "The looters, seeing that you aren't about to attack, quickly scatter to the wind. Your soldiers grumble.", true, false, "Done", null, null, null), true);
					}
					else
					{
						MessageBox.Show($"Error while selecting option for \"{randomEventData.eventType}\"");
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

		private string CalculateDescription(int spawnCount)
		{
			string sizeDescription = "";
			if (spawnCount <= minimumSoldiers)
			{
				sizeDescription = "reasonable";
			}
			else if (spawnCount < minimumSoldiers * 1.5f)
			{
				sizeDescription = "large";
			}
			else
			{
				sizeDescription = "huge";
			}

			string description = $"You stumble upon a {sizeDescription} amount of looters! Your soldiers seem very eager to show you what they've learned";

			return description;
		}

		private static void SpawnLooters(int spawnCount)
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
		public readonly int minimumSoldiers;

		public readonly float percentageDifferenceOfCurrentTroop;

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
