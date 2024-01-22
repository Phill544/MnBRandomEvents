using System;
using System.Collections.Generic;
using System.Windows;
using Bannerlord.RandomEvents.Helpers;
using Bannerlord.RandomEvents.Settings;
using Ini.Net;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace Bannerlord.RandomEvents.Events
{
	public sealed class ExoticDrinks : BaseEvent
	{
		private readonly bool eventDisabled;
		private readonly int minPrice;
		private readonly int maxPrice;
		private readonly float successChance;
		private readonly int minXp;
		private readonly int maxXp;

		public ExoticDrinks() : base(ModSettings.RandomEvents.ExoticDrinksData)
		{
			var ConfigFile = new IniFile(ParseIniFile.GetTheConfigFile());
            
			eventDisabled = ConfigFile.ReadBoolean("ExoticDrinks", "EventDisabled");
			minPrice = ConfigFile.ReadInteger("ExoticDrinks", "MinPrice");
			maxPrice = ConfigFile.ReadInteger("ExoticDrinks", "MaxPrice");
			successChance = ConfigFile.ReadFloat("ExoticDrinks", "SuccessChance");
			minXp = ConfigFile.ReadInteger("ExoticDrinks", "MinXp");
			maxXp = ConfigFile.ReadInteger("ExoticDrinks", "MaxXp");
		}

		public override void CancelEvent()
		{
		}

		private bool HasValidEventData()
		{
			if (eventDisabled == false)
			{
				if (minPrice != 0 || maxPrice != 0|| successChance != 0|| minXp != 0|| maxXp != 0)
				{
					return true;
				}
			}
            
			return false;
		}

		public override bool CanExecuteEvent()
		{
			return HasValidEventData() && Hero.MainHero.Gold >= maxPrice;
		}

		public override void StartEvent()
		{
			var eventTitle = new TextObject("{=ExoticDrinks_Title}Exotic Drinks").ToString();

			var price = MBRandom.RandomInt(minPrice, maxPrice);
			
			var eventDescription = new TextObject("{=ExoticDrinks_Event_Desc}You come across a vendor selling exotic drinks for {price}. He won't tell you how, but says that it will make you a better person.")
				.SetTextVariable("price", price)
				.ToString();
			
			var eventOutcome1 = new TextObject("{=ExoticDrinks_Event_Text_1}Wise choice the vendor says as he pours you a small cup with a weird, fizzy, yellow liquid in it. As you take a sip, you think to yourself that it smells like piss. Quickly you realise it tastes like it too.\n Hopefully that wasn't a mistake.")
				.ToString();
			
			var eventOutcome2 = new TextObject("{=ExoticDrinks_Event_Text_2}The vendor laughs. It's your loss, he claims.")
				.ToString();
			
			var eventNothingHappens = new TextObject("{=ExoticDrinks_Event_Nothing_Happens}Nothing happens. The vendor laughs maniacally. You're left to wonder what this was all about.")
				.ToString();
				
			var eventButtonText1 = new TextObject("{=ExoticDrinks_Event_Button_Text_1}Okay").ToString();
			var eventButtonText2 = new TextObject("{=ExoticDrinks_Event_Button_Text_2}Done").ToString();
			
			var eventOption1 = new TextObject("{=ExoticDrinks_Event_Option_1}Buy drink").ToString();
			var eventOption1Hover = new TextObject("{=ExoticDrinks_Event_Option_1_Hover}What could go wrong?").ToString();
			
			var eventOption2 = new TextObject("{=ExoticDrinks_Event_Option_2}Decline").ToString();
			var eventOption2Hover = new TextObject("{=ExoticDrinks_Event_Option_2_Hover}You'd have to be crazy to drink random liquid!").ToString();

			var inquiryElements = new List<InquiryElement>
			{
				new InquiryElement("a", eventOption1, null, true, eventOption1Hover),
				new InquiryElement("b", eventOption2, null, true, eventOption2Hover)
			};

			var msid = new MultiSelectionInquiryData(eventTitle, eventDescription, inquiryElements, false, 1, 1, eventButtonText1, null,
				elements => 
				{
					switch ((string)elements[0].Identifier)
					{
						case "a":
							InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOutcome1, true, false, eventButtonText2, null, null, null), true);
							
							Hero.MainHero.ChangeHeroGold(-price);

							float randFloat = MBRandom.RandomFloatRanged(0f, 1f);
							bool success = randFloat <= successChance;

							if (success)
								LiquidChanges();
							else
								InformationManager.ShowInquiry(new InquiryData(eventTitle, eventNothingHappens, true, false, eventButtonText2, null, null, null), true);
							
							break;
						case "b":
							InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOutcome2, true, false, eventButtonText2, null, null, null), true);
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
		
		void LiquidChanges()
		{
			int xpReceived = MBRandom.RandomInt(minXp, maxXp);

			var attributes = AttributeHelper.GetAllAttributes();

			var index = MBRandom.RandomInt(0, attributes.Count -1);

			//Set skill xp
			var skillIndex = MBRandom.RandomInt(0, attributes[index].Skills.Count - 1);
			Hero.MainHero.HeroDeveloper.AddSkillXp(attributes[index].Skills[skillIndex], xpReceived);
			InformationManager.DisplayMessage(new InformationMessage($"{attributes[index].Skills[skillIndex].Name} received {xpReceived.ToString()}xp!", RandomEventsSubmodule.Msg_Color_POS_Outcome));
		}
	}


	public class ExoticDrinksData : RandomEventData
	{

		public ExoticDrinksData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
		{
		}

		public override BaseEvent GetBaseEvent()
		{
			return new ExoticDrinks();
		}
	}
}
