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
	public class ExoticDrinks : BaseEvent
	{
		private int price;

		private string eventTitle = "Exotic Drinks";

		public ExoticDrinks() : base(Settings.RandomEvents.ExoticDrinksData)
		{
			this.price = Settings.RandomEvents.ExoticDrinksData.price;
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
			if (Settings.GeneralSettings.DebugMode)
			{
				InformationManager.DisplayMessage(new InformationMessage($"Starting {this.RandomEventData.EventType}", RandomEventsSubmodule.textColor));
			}

			List<InquiryElement> inquiryElements = new List<InquiryElement>();
			inquiryElements.Add(new InquiryElement("a", "Buy drink", null, true, "What could go wrong?"));
			inquiryElements.Add(new InquiryElement("b", "Decline", null, true, "You'd have to be crazy to drink random liquid!"));

			MultiSelectionInquiryData msid = new MultiSelectionInquiryData(
				eventTitle, // Title
				$"You come across a vendor selling exotic drinks for {price}. He won't tell you how, but says that it will make you a better person.", // Description
				inquiryElements, // Options
				false, // Can close menu without selecting an option. Should always be false.
				true, // Force a single option to be selected. Should usually be true
				"Okay", // The text on the button that continues the event
				null, // The text to display on the "cancel" button, shouldn't ever need it.
				(elements) => // How to handle the selected option. Will only ever be a single element unless force single option is off.
				{
					if ((string)elements[0].Identifier == "a")
					{
						GiveRandomSkillXP();
						Hero.MainHero.ChangeHeroGold(-price);

						InformationManager.ShowInquiry(new InquiryData(eventTitle, "\"Wise choice.\" The vendor pours you a small cup with a weird, fizzy, yellow liquid in it. As you take a sip, you think to yourself that it smells like piss. Quickly you realise it tastes like it too.\n Hopefully that wasn't a mistake.", true, false, "Done", null, null, null), true);
					}
					else if ((string)elements[0].Identifier == "b")
					{
						InformationManager.ShowInquiry(new InquiryData(eventTitle, "\"Hehehehehe\" the vendor laughs. \"It's your loss.\"", true, false, "Done", null, null, null), true);
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

		private void GiveRandomSkillXP()
		{
			List<SkillObject> allSkills = DefaultSkills.GetAllSkills().ToList();
			int index = MBRandom.RandomInt(allSkills.Count);

			float xpToGive = Settings.GeneralSettings.GeneralLevelXpMultiplier * Hero.MainHero.GetSkillValue(allSkills[index]);
			Hero.MainHero.AddSkillXp(allSkills[index], xpToGive);
		}
	}


	public class ExoticDrinksData : RandomEventData
	{
		public int price;

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
