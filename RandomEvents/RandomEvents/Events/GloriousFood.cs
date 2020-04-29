using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace CryingBuffalo.RandomEvents.Events
{
	/// <summary>
	/// KNOWN BUG -- IF GAME SAVED DURING EVENT PLAYER WILL NOT BE ABLE TO MOVE (MobileParty.MainParty.IsActive = true)
	/// Also, using that value to stop movement means that the player cannot be interacted with.
	/// Disabled until it can be reimplemented
	/// </summary>
	public class GloriousFood : BaseEvent
	{
		private int minFoodAmount;
		private int maxFoodAmount;
		private int forageHours;

		private int currentForageHours;

		private string eventTitle = "Food, Glorious Food";

		private MBCampaignEvent hourlyTickEvent = null;

		public GloriousFood() : base(Settings.RandomEvents.GloriousFoodData)
		{
			minFoodAmount = Settings.RandomEvents.GloriousFoodData.minFoodAmount;
			maxFoodAmount = Settings.RandomEvents.GloriousFoodData.maxFoodAmount;
			forageHours = Settings.RandomEvents.GloriousFoodData.forageHours;
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
			if (Settings.GeneralSettings.DebugMode)
			{
				InformationManager.DisplayMessage(new InformationMessage($"Starting {this.RandomEventData.EventType}", RandomEventsSubmodule.Instance.textColor));
			}

			List<InquiryElement> inquiryElements = new List<InquiryElement>();
			inquiryElements.Add(new InquiryElement("a", "Order the men to gather some food.", null));
			inquiryElements.Add(new InquiryElement("b", "There's no time.", null));

			MultiSelectionInquiryData msid = new MultiSelectionInquiryData(
				eventTitle, // Title
				"While traveling you come across a large meadow with grazing deer surrounded by grape vines. If you have some spare time, perhaps you could collect some food.", // Description
				inquiryElements, // Options
				false, // Can close menu without selecting an option. Should always be false.
				true, // Force a single option to be selected. Should usually be true
				"Okay", // The text on the button that continues the event
				null, // The text to display on the "cancel" button, shouldn't ever need it.
				(elements) => // How to handle the selected option. Will only ever be a single element unless force single option is off.
				{
					if ((string)elements[0].Identifier == "a")
					{
						Campaign.Current.TimeControlMode = CampaignTimeControlMode.UnstoppableFastForwardForPartyWaitTime;

						MobileParty.MainParty.IsActive = false;

						waitPos = MobileParty.MainParty.Position2D;

						hourlyTickEvent = CampaignEvents.CreatePeriodicEvent(1f, 0f);
						hourlyTickEvent.AddHandler(new MBCampaignEvent.CampaignEventDelegate(HourlyTick));
					}
					else if ((string)elements[0].Identifier == "b")
					{
						StopEvent();
					}
					else
					{
						MessageBox.Show($"Error while selecting option for \"{this.RandomEventData.EventType}\"");
					}

				},
				null); // What to do on the "cancel" button, shouldn't ever need it.

			InformationManager.ShowMultiSelectionInquiry(msid, true);
		}

		private Vec2 waitPos;

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

			InformationManager.ShowInquiry(new InquiryData(eventTitle, $"Your troop managed to forage {gatheredMeat} slabs of meat and {gatheredGrapes} baskets of grapes!", true, false, "Done", null, null, null), true);

			hourlyTickEvent.Unregister(this);
			hourlyTickEvent = null;

			MobileParty.MainParty.IsActive = true;

			StopEvent();
		}
	}

	public class GloriousFoodData : RandomEventData
	{
		public int minFoodAmount;
		public int maxFoodAmount;
		public int forageHours;

		public GloriousFoodData(RandomEventType eventType, float chanceWeight, int minFoodAmount, int maxFoodAmount, int forageHours) : base(eventType, chanceWeight)
		{
			this.minFoodAmount = minFoodAmount;
			this.maxFoodAmount = maxFoodAmount;
			this.forageHours = forageHours;
		}
	}
}
