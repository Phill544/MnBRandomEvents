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
using TaleWorlds.ObjectSystem;

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

		private string eventTitle = "Nourriture, glorieuse nourriture";

		private MBCampaignEvent hourlyTickEvent = null;

		public GloriousFood() : base(null/*Settings.RandomEvents.GloriousFoodData*/)
		{
			//minFoodAmount = Settings.RandomEvents.GloriousFoodData.minFoodAmount;
			//maxFoodAmount = Settings.RandomEvents.GloriousFoodData.maxFoodAmount;
			//forageHours = Settings.RandomEvents.GloriousFoodData.forageHours;
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
				InformationManager.DisplayMessage(new InformationMessage($"Starting {this.RandomEventData.EventType}", RandomEventsSubmodule.textColor));
			}

			List<InquiryElement> inquiryElements = new List<InquiryElement>();
			inquiryElements.Add(new InquiryElement("a", "Ordonne aux hommes de ramasser de la nourriture.", null));
			inquiryElements.Add(new InquiryElement("b", "Il n'y a pas de temps.", null));

			MultiSelectionInquiryData msid = new MultiSelectionInquiryData(
				eventTitle, // Title
				"En voyageant, vous rencontrez une grande prairie avec des cerfs en pâturage entourés de vignes. Si vous avez du temps libre, vous pourriez peut-être ramasser de la nourriture.", // Description
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
						MessageBox.Show($"Erreur lors de la sélection de l'option pour \"{this.RandomEventData.EventType}\"");
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

			ItemObject grape = MBObjectManager.Instance.GetObject<ItemObject>("Raisin");
			ItemObject meat = MBObjectManager.Instance.GetObject<ItemObject>("Viande");

			MobileParty.MainParty.ItemRoster.AddToCounts(grape, gatheredGrapes);
			MobileParty.MainParty.ItemRoster.AddToCounts(meat, gatheredMeat);

			Campaign.Current.TimeControlMode = CampaignTimeControlMode.Stop;

			InformationManager.ShowInquiry(new InquiryData(eventTitle, $"Vos troupe récolte {gatheredMeat} morceaux de viande et {gatheredGrapes} paniers de raisins!", true, false, "Terminé", null, null, null), true);

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

		public GloriousFoodData(string eventType, float chanceWeight, int minFoodAmount, int maxFoodAmount, int forageHours) : base(eventType, chanceWeight)
		{
			this.minFoodAmount = minFoodAmount;
			this.maxFoodAmount = maxFoodAmount;
			this.forageHours = forageHours;
		}

		public override BaseEvent GetBaseEvent()
		{
			return new GloriousFood();
		}
	}
}
