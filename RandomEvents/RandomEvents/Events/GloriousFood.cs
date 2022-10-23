using System;
using System.Collections.Generic;
using System.Windows;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

namespace CryingBuffalo.RandomEvents.Events
{
	/// <summary>
	/// KNOWN BUG -- IF GAME SAVED DURING EVENT PLAYER WILL NOT BE ABLE TO MOVE (MobileParty.MainParty.IsActive = true)
	/// Also, using that value to stop movement means that the player cannot be interacted with.
	/// Disabled until it can be reimplemented
	/// </summary>
	public sealed class GloriousFood : BaseEvent
	{
		private readonly int minFoodAmount;
		private readonly int maxFoodAmount;
		private readonly int forageHours;

		private int currentForageHours;

		private const string EventTitle = "Food, Glorious Food";

		private MBCampaignEvent hourlyTickEvent;

		public GloriousFood(int minFoodAmount, int maxFoodAmount, int forageHours) : base(null)
		{
			this.minFoodAmount = minFoodAmount;
			this.maxFoodAmount = maxFoodAmount;
			this.forageHours = forageHours;
		}

		public override void CancelEvent()
		{
			hourlyTickEvent.Unregister(this);
			hourlyTickEvent = null;
			MobileParty.MainParty.IsActive = true;
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

			List<InquiryElement> inquiryElements = new List<InquiryElement>
			{
				new InquiryElement("a", "Order the men to gather some food.", null),
				new InquiryElement("b", "There's no time.", null)
			};

			MultiSelectionInquiryData msid = new MultiSelectionInquiryData(
				EventTitle, // Title
				"While traveling you come across a large meadow with grazing deer surrounded by grape vines. If you have some spare time, perhaps you could collect some food.", // Description
				inquiryElements, // Options
				false, // Can close menu without selecting an option. Should always be false.
				1, // Force a single option to be selected. Should usually be true
				"Okay", // The text on the button that continues the event
				null, // The text to display on the "cancel" button, shouldn't ever need it.
				elements => // How to handle the selected option. Will only ever be a single element unless force single option is off.
				{
					switch ((string)elements[0].Identifier)
					{
						case "a":
							Campaign.Current.TimeControlMode = CampaignTimeControlMode.UnstoppableFastForwardForPartyWaitTime;

							MobileParty.MainParty.IsActive = false;

							waitPos = MobileParty.MainParty.Position2D;

							//hourlyTickEvent = CampaignEvents.CreatePeriodicEvent(1f, 0f);
							hourlyTickEvent = CampaignPeriodicEventManager.CreatePeriodicEvent(CampaignTime.HoursFromNow(1f), CampaignTime.Zero);
							hourlyTickEvent.AddHandler(HourlyTick);
							break;
						case "b":
							StopEvent();
							break;
						default:
							MessageBox.Show($"Error while selecting option for \"{randomEventData.eventType}\"");
							break;
					}
				},
				null); // What to do on the "cancel" button, shouldn't ever need it.

			MBInformationManager.ShowMultiSelectionInquiry(msid, true);
		}

		private Vec2 waitPos;

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

		private void HourlyTick(MBCampaignEvent campaignevent, object[] delegateparams)
		{
			currentForageHours++;
			if (currentForageHours < forageHours)
			{
				return;
			}

			int gatheredMeat = MBRandom.RandomInt(minFoodAmount, maxFoodAmount);
			int gatheredGrapes = MBRandom.RandomInt(minFoodAmount, maxFoodAmount);

			ItemObject grape = MBObjectManager.Instance.GetObject<ItemObject>("grape");
			ItemObject meat = MBObjectManager.Instance.GetObject<ItemObject>("meat");

			MobileParty.MainParty.ItemRoster.AddToCounts(grape, gatheredGrapes);
			MobileParty.MainParty.ItemRoster.AddToCounts(meat, gatheredMeat);

			Campaign.Current.TimeControlMode = CampaignTimeControlMode.Stop;

			InformationManager.ShowInquiry(new InquiryData(EventTitle, $"Your troop managed to forage {gatheredMeat} slabs of meat and {gatheredGrapes} baskets of grapes!", true, false, "Done", null, null, null), true);

			hourlyTickEvent.Unregister(this);
			hourlyTickEvent = null;

			MobileParty.MainParty.IsActive = true;

			StopEvent();
		}
	}
}
