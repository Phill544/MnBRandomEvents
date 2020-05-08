using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.ObjectSystem;

namespace CryingBuffalo.RandomEvents.Events
{
	public class WanderingLivestock : BaseEvent
	{
		private int minFood;
		private int maxFood;

		private string eventTitle = "Free Range Meat";

		public WanderingLivestock() : base(Settings.RandomEvents.WanderingLivestockData)
		{
			this.minFood = Settings.RandomEvents.WanderingLivestockData.minFood;
			this.maxFood = Settings.RandomEvents.WanderingLivestockData.maxFood;
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
				InformationManager.DisplayMessage(new InformationMessage($"Starting {this.RandomEventData.EventType}", RandomEventsSubmodule.Instance.textColor));
			}

			List<InquiryElement> inquiryElements = new List<InquiryElement>();
			inquiryElements.Add(new InquiryElement("a", "Take them in", null));
			inquiryElements.Add(new InquiryElement("b", "Ignore them", null));

			MultiSelectionInquiryData msid = new MultiSelectionInquiryData(
				eventTitle, // Title
				$"You come across some wandering livestock.", // Description
				inquiryElements, // Options
				false, // Can close menu without selecting an option. Should always be false.
				true, // Force a single option to be selected. Should usually be true
				"Okay", // The text on the button that continues the event
				null, // The text to display on the "cancel" button, shouldn't ever need it.
				(elements) => // How to handle the selected option. Will only ever be a single element unless force single option is off.
				{
					if ((string)elements[0].Identifier == "a")
					{
						int sheepCount = 0;
						int cowCount = 0;

						int totalCount = MBRandom.RandomInt(minFood, maxFood);

						sheepCount = MBRandom.RandomInt(1, totalCount);
						cowCount = totalCount - sheepCount;

						string cowText = "";

						if (cowCount > 0)
						{
							string cowPlural = "";
							if (cowCount > 1) cowPlural = "s";

							cowText = $", and {cowCount} cow{cowPlural}.";
						}
						else
						{
							cowText = ".";
						}

						ItemObject sheep = MBObjectManager.Instance.GetObject<ItemObject>("sheep");
						ItemObject cow = MBObjectManager.Instance.GetObject<ItemObject>("cow");

						MobileParty.MainParty.ItemRoster.AddToCounts(sheep, sheepCount);
						MobileParty.MainParty.ItemRoster.AddToCounts(cow, cowCount);

						InformationManager.ShowInquiry(new InquiryData(eventTitle, $"Who could say no to such a delicious -- I mean, reasonable proposition? You end up in possession of {sheepCount} sheep{cowText}", true, false, "Yum", null, null, null), true);
					}
					else if ((string)elements[0].Identifier == "b")
					{
						InformationManager.ShowInquiry(new InquiryData(eventTitle, "The last thing you need right now is to tend to livestock, so you leave them.", true, false, "Done", null, null, null), true);
					}
					else
					{
						MessageBox.Show($"Error while selecting option for \"{this.RandomEventData.EventType}\"");
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

	public class WanderingLivestockData : RandomEventData
	{
		public int minFood;
		public int maxFood;

		public WanderingLivestockData(RandomEventType eventType, float chanceWeight, int minFood, int maxFood) : base(eventType, chanceWeight)
		{
			this.minFood = minFood;
			this.maxFood = maxFood;
		}
	}
}
