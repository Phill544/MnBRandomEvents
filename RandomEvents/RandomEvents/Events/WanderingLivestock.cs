using System;
using System.Collections.Generic;
using System.Windows;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

namespace CryingBuffalo.RandomEvents.Events
{
	public sealed class WanderingLivestock : BaseEvent
	{
		private readonly int minFood;
		private readonly int maxFood;

		private const string EventTitle = "Free Range Meat";

		public WanderingLivestock() : base(Settings.ModSettings.RandomEvents.WanderingLivestockData)
		{
			minFood = Settings.ModSettings.RandomEvents.WanderingLivestockData.minFood;
			maxFood = Settings.ModSettings.RandomEvents.WanderingLivestockData.maxFood;
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
			if (Settings.ModSettings.GeneralSettings.DebugMode)
			{
				InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}", RandomEventsSubmodule.TextColor));
			}

			List<InquiryElement> inquiryElements = new List<InquiryElement>
			{
				new InquiryElement("a", "Take them in", null),
				new InquiryElement("b", "Ignore them", null)
			};

			MultiSelectionInquiryData msid = new MultiSelectionInquiryData(
				EventTitle, // Title
				"You come across some wandering livestock.", // Description
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
						{
							int totalCount = MBRandom.RandomInt(minFood, maxFood);

							var sheepCount = MBRandom.RandomInt(1, totalCount);
							var cowCount = totalCount - sheepCount;

							string cowText;

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

							InformationManager.ShowInquiry(new InquiryData(EventTitle, $"Who could say no to such a delicious -- I mean, reasonable proposition? You end up in possession of {sheepCount} sheep{cowText}", true, false, "Yum", null, null, null), true);
							break;
						}
						case "b":
							InformationManager.ShowInquiry(new InquiryData(EventTitle, "The last thing you need right now is to tend to livestock, so you leave them.", true, false, "Done", null, null, null), true);
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

	public class WanderingLivestockData : RandomEventData
	{
		public readonly int minFood;
		public readonly int maxFood;

		public WanderingLivestockData(string eventType, float chanceWeight, int minFood, int maxFood) : base(eventType, chanceWeight)
		{
			this.minFood = minFood;
			this.maxFood = maxFood;
		}

		public override BaseEvent GetBaseEvent()
		{
			return new WanderingLivestock();
		}
	}
}
