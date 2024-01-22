using System;
using System.Collections.Generic;
using System.Windows;
using Bannerlord.RandomEvents.Helpers;
using Bannerlord.RandomEvents.Settings;
using Ini.Net;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace Bannerlord.RandomEvents.Events
{
	public sealed class WanderingLivestock : BaseEvent
	{
		private readonly bool eventDisabled;
		private readonly int minFood;
		private readonly int maxFood;

		public WanderingLivestock() : base(ModSettings.RandomEvents.WanderingLivestockData)
		{
			var ConfigFile = new IniFile(ParseIniFile.GetTheConfigFile());
            
			eventDisabled = ConfigFile.ReadBoolean("WanderingLivestock", "EventDisabled");
			minFood = ConfigFile.ReadInteger("WanderingLivestock", "MinFood");
			maxFood = ConfigFile.ReadInteger("WanderingLivestock", "MaxFood");
		}

		public override void CancelEvent()
		{
		}
		
		private bool HasValidEventData()
		{
			if (eventDisabled == false)
			{
				if (minFood != 0 || maxFood != 0)
				{
					return true;
				}
			}
            
			return false;
		}

		public override bool CanExecuteEvent()
		{
			return HasValidEventData() && MobileParty.MainParty.CurrentSettlement == null;
		}

		public override void StartEvent()
		{
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

			var msid = new MultiSelectionInquiryData(eventTitle, eventDescription, inquiryElements, false, 1, 1, eventButtonText1, null, 
				elements => 
				{
					switch ((string)elements[0].Identifier)
					{
						case "a":
						{
							var totalCount = MBRandom.RandomInt(minFood, maxFood);

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

							var sheep = MBObjectManager.Instance.GetObject<ItemObject>("sheep");
							var cow = MBObjectManager.Instance.GetObject<ItemObject>("cow");

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
				}, null, null);

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
		public WanderingLivestockData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
		{
		}

		public override BaseEvent GetBaseEvent()
		{
			return new WanderingLivestock();
		}
	}
}
