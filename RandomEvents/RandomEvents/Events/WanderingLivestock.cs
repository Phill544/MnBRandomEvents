using System;
using System.Collections.Generic;
using System.Windows;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace CryingBuffalo.RandomEvents.Events
{
	public sealed class WanderingLivestock : BaseEvent
	{
		private readonly int minFood;
		private readonly int maxFood;

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
				InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}", RandomEventsSubmodule.Dbg_Color));
			}
			
			var eventTitle = new TextObject("{=WanderingLivestock_Title}Free Range Meat").ToString();

			var eventOption1 = new TextObject("{=WanderingLivestock_Event_Option_1}Take them in").ToString();
			var eventOption2 = new TextObject("{=WanderingLivestock_Event_Option_2}Ignore them").ToString();

			var inquiryElements = new List<InquiryElement>
			{
				new InquiryElement("a", eventOption1, null),
				new InquiryElement("b", eventOption2, null)
			};
			
			var eventDescription = new TextObject("{=WanderingLivestock_Event_Desc}You come across some wandering livestock.")
				.ToString();
			
			var eventButtonText1 = new TextObject("{=WanderingLivestock_Event_Button_Text_1}Okay").ToString();
			var eventButtonText2 = new TextObject("{=WanderingLivestock_Event_Button_Text_2}Yum").ToString();
			var eventButtonText3 = new TextObject("{=WanderingLivestock_Event_Button_Text_3}Done").ToString();

			var msid = new MultiSelectionInquiryData(eventTitle, eventDescription, inquiryElements, false, 1, eventButtonText1, null, 
				elements => 
				{
					switch ((string)elements[0].Identifier)
					{
						case "a":
						{
							int totalCount = MBRandom.RandomInt(minFood, maxFood);

							var sheepCount = MBRandom.RandomInt(1, totalCount);
							var cowCount = totalCount - sheepCount;

							string cowText;
							
							var eventPluralEnd= new TextObject("{=WanderingLivestock_Event_Plural_End}s").ToString();
							
							if (cowCount > 0)
							{
								var cowPlural = "";
								if (cowCount > 1) cowPlural = eventPluralEnd;
								
								var eventCowText= new TextObject("{=WanderingLivestock_Event_Cow_Text}, and {cowCount} cow{cowPlural}.")
									.SetTextVariable("cowCount", cowCount)
									.SetTextVariable("cowPlural", cowPlural)
									.ToString();

								cowText = eventCowText;
							}
							else
							{
								cowText = ".";
							}

							ItemObject sheep = MBObjectManager.Instance.GetObject<ItemObject>("sheep");
							ItemObject cow = MBObjectManager.Instance.GetObject<ItemObject>("cow");

							MobileParty.MainParty.ItemRoster.AddToCounts(sheep, sheepCount);
							MobileParty.MainParty.ItemRoster.AddToCounts(cow, cowCount);
							
							var eventOptionAText = new TextObject("{=WanderingLivestock_Event_Choice_1}Who could say no to such a delicious -- I mean, reasonable proposition? You end up in possession of {sheepCount} sheep{cowText}")
								.SetTextVariable("sheepCount", sheepCount)
								.SetTextVariable("cowText", cowText)
								.ToString();
							

							InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionAText, true, false, eventButtonText2, null, null, null), true);
							break;
						}
						case "b":
							
							var eventOptionBText = new TextObject(
									"{=WanderingLivestock_Event_Choice_2}The last thing you need right now is to tend to livestock, so you leave them.")
								.ToString();
							
							InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionBText, true, false, eventButtonText3, null, null, null), true);
							break;
						default:
							MessageBox.Show($"Error while selecting option for \"{randomEventData.eventType}\"");
							break;
					}
				},
				null);

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
