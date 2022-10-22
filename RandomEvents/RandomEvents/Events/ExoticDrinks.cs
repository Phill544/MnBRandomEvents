using System;
using System.Collections.Generic;
using System.Windows;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace CryingBuffalo.RandomEvents.Events
{
	public sealed class ExoticDrinks : BaseEvent
	{
		private readonly int price;

		private const string EventTitle = "Exotic Drinks";

		public ExoticDrinks() : base(Settings.Settings.RandomEvents.ExoticDrinksData)
		{
			price = Settings.Settings.RandomEvents.ExoticDrinksData.price;
		}

		public override void CancelEvent()
		{
		}

		public override bool CanExecuteEvent()
		{
			return Hero.MainHero.Gold >= price;
		}

		public override void StartEvent()
		{
			if (Settings.Settings.GeneralSettings.DebugMode)
			{
				InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}", RandomEventsSubmodule.TextColor));
			}

			var inquiryElements = new List<InquiryElement>
			{
				new InquiryElement("a", "Buy drink", null, true, "What could go wrong?"),
				new InquiryElement("b", "Decline", null, true, "You'd have to be crazy to drink random liquid!")
			};

			var msid = new MultiSelectionInquiryData(
				EventTitle, // Title
				$"You come across a vendor selling exotic drinks for {price}. He won't tell you how, but says that it will make you a better person.", // Description
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
							Hero.MainHero.ChangeHeroGold(-price);

							InformationManager.ShowInquiry(new InquiryData(EventTitle, "\"Wise choice.\" The vendor pours you a small cup with a weird, fizzy, yellow liquid in it. As you take a sip, you think to yourself that it smells like piss. Quickly you realise it tastes like it too.\n Hopefully that wasn't a mistake.", true, false, "Done", null, null, null), true);
							NothingHappens();
							break;
						case "b":
							InformationManager.ShowInquiry(new InquiryData(EventTitle, "\"Hehehehehe\" the vendor laughs. \"It's your loss.\"", true, false, "Done", null, null, null), true);
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

		private void NothingHappens()
		{
			InformationManager.ShowInquiry(new InquiryData(EventTitle, "\"Nothing happens.\" The vendor laughs maniacally. \"You're left to wonder what this was all about.\"", true, false, "Done", null, null, null), true);
		}
	}


	public class ExoticDrinksData : RandomEventData
	{
		public readonly int price;

		public ExoticDrinksData(string eventType, float chanceWeight, int price) : base(eventType, chanceWeight)
		{
			this.price = price;
		}

		public override BaseEvent GetBaseEvent()
		{
			return new ExoticDrinks();
		}
	}
}
