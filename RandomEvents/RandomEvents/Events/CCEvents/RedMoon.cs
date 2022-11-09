﻿using System;
using System.Collections.Generic;
using System.Windows;
using CryingBuffalo.RandomEvents.Helpers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace CryingBuffalo.RandomEvents.Events.CCEvents
{
	public sealed class RedMoon : BaseEvent
	{
		private readonly int minGoldLost;
		private readonly int maxGoldLost;
		private readonly int minMenLost;
		private readonly int maxMenLost;

		public RedMoon() : base(Settings.ModSettings.RandomEvents.RedMoonData)
		{
			minGoldLost = Settings.ModSettings.RandomEvents.RedMoonData.minGoldLost;
			maxGoldLost = Settings.ModSettings.RandomEvents.RedMoonData.maxGoldLost;
			minMenLost = Settings.ModSettings.RandomEvents.RedMoonData.minMenLost;
			maxMenLost = Settings.ModSettings.RandomEvents.RedMoonData.maxMenLost;
		}

		public override void CancelEvent()
		{
		}

		public override bool CanExecuteEvent()
		{
			return true;
		}

		public override void StartEvent()
		{
			if (Settings.ModSettings.GeneralSettings.DebugMode)
			{
				InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}", RandomEventsSubmodule.Dbg_Color));
			}
			
			var eventTitle = new TextObject("{=RedMoon_Title}A Coming Apocalypse?").ToString();

			var heroName = Hero.MainHero.FirstName;

			var goldLostToReligion = MBRandom.RandomInt(minGoldLost, maxGoldLost);

			var menLostToReligion = MBRandom.RandomInt(minMenLost, maxMenLost);
			
			var closestSettlement = ClosestSettlements.GetClosestTownOrVillage(MobileParty.MainParty).ToString();
			
			
			var eventDescription =new TextObject(
					"{=RedMoon_Event_Desc}You are in your tent late one night when you are awoken by your men starting a commotion. Annoyed, you go out and tell them to quiet down " +
					"but as soon as you step foot outside your tent you see what the commotion is about. The moon has become blood red. Your men are panicking and running around in all " +
					"directions and proclaiming this to be the end of days. The only question you can ask yourself is what are you going to do about this?") 
				.ToString();
			
			var eventOption1 = new TextObject("{=RedMoon_Event_Option_1}Panic with your men").ToString();
			var eventOption1Hover = new TextObject("{=RedMoon_Event_Option_1_Hover}This really **is** the end!").ToString();
            
			var eventOption2 = new TextObject("{=RedMoon_Event_Option_2}Call your men to you").ToString();
			var eventOption2Hover = new TextObject("{=RedMoon_Event_Option_2_Hover}Let's talk instead of acting like idiots").ToString();
            
			var eventOption3 = new TextObject("{=RedMoon_Event_Option_3}Order your men to stop").ToString();
			var eventOption3Hover = new TextObject("{=RedMoon_Event_Option_3_Hover}This is embarrassing!").ToString();
            
			var eventOption4 = new TextObject("{=RedMoon_Event_Option_4}Ignore everything").ToString();
			var eventOption4Hover = new TextObject("{=RedMoon_Event_Option_4_Hover}Just head back to your tent").ToString();
            
			var eventButtonText1 = new TextObject("{=RedMoon_Event_Button_Text_1}Choose").ToString();
			var eventButtonText2 = new TextObject("{=RedMoon_Event_Button_Text_2}Done").ToString();
			
			var inquiryElements = new List<InquiryElement>
			{
				new InquiryElement("a", eventOption1, null, true, eventOption1Hover),
				new InquiryElement("b", eventOption2, null, true, eventOption2Hover),
				new InquiryElement("c", eventOption3, null, true, eventOption3Hover),
				new InquiryElement("d", eventOption4, null, true, eventOption4Hover)
			};
			
			var eventOptionAText = new TextObject(
                    "{=RedMoon_Event_Choice_1}You fall to your knees and start praying to the gods. Several of your men join you in prayer. \n \n" +
                    "After praying for almost 10 minutes you realize that this won't help. You order your men to give you {goldLostToReligion} gold that you will rush to the " +
                    "nearest chapel. Hopefully the priests can help you. You mount your steed and ride of. You ride like a madman towards {closestSettlement} as you know the " +
                    "settlement has a chapel. As your steed jumps over a fence you fall off and black out.\n" +
                    "When you wake up it's morning you make your way back to camp. Only after you arrived back did you think of the chest of gold you lost.")
				.SetTextVariable("goldLostToReligion", goldLostToReligion)
				.SetTextVariable("closestSettlement", closestSettlement)
				.ToString();
            
            var eventOptionBText = new TextObject(
                    "{=RedMoon_Event_Choice_2}You call your men to you. Many are panicking and you see genuine fear in their faces. You tell to stop and calm down but to no avail. They start " +
                    "running around like headless chickens again. You decide not to waste your time and let the fanatics do what they want so you retire to your tent.\n \n" +
                    "When you wake up the following morning you learned that {menLostToReligion} of your men left your party in the direction of {closestSettlement}. They also inform you they " +
                    "took {goldLostToReligion} gold from the treasury as an offering to the church. At least the fanatics are gone!")
	            .SetTextVariable("menLostToReligion", menLostToReligion)
	            .SetTextVariable("closestSettlement", closestSettlement)
	            .SetTextVariable("goldLostToReligion", goldLostToReligion)
	            .ToString();
            
            var eventOptionCText = new TextObject(
                    "{=RedMoon_Event_Choice_3}You order your men to you! Most fall in line immediately, but some still act like idiots. You tell your men that what you are witnessing is a " +
                    "natural occuring phenomenon since you have witnessed this before. You explain how you saw the exact same thing a few years ago and nothing bad came out of it. You men seem to be " +
                    "happy to hear this. There are still some idiots praying in the back but you kick them from the party claiming you don't need mentally unstable warriors. You have several " +
                    "of your guards kick them out of camp!")
                .ToString();
            
            var eventOptionDText = new TextObject(
                    "{=RedMoon_Event_Choice_4}Nope...\n \nYou turn around and go back into your tent and straight back to bed.\n \n" +
                    "When you wake up the following morning you learned that {menLostToReligion} of your men left your party in the direction of {closestSettlement}. They also inform you they " +
                    "took {goldLostToReligion} gold from the treasury as an offering to the church. At least the fanatics are gone!")
	            .SetTextVariable("closestSettlement", closestSettlement)
	            .SetTextVariable("goldLostToReligion", goldLostToReligion)
                .ToString();
            
            var eventMsg1 = new TextObject(
		            "{=RedMoon_Event_Msg_1}{heroName} lost {goldLostToReligion} gold due to a... unfortunate accident.")
	            .SetTextVariable("goldLostToReligion", goldLostToReligion)
	            .SetTextVariable("heroName", heroName)
	            .ToString();
            
            var eventMsg2 = new TextObject(
		            "{=RedMoon_Event_Msg_2}{heroName} lost {menLostToReligion} men and {goldLostToReligion} gold after the red moon.")
	            .SetTextVariable("goldLostToReligion", goldLostToReligion)
	            .SetTextVariable("menLostToReligion", menLostToReligion)
	            .SetTextVariable("heroName", heroName)
	            .ToString();
            
            var eventMsg3 = new TextObject(
		            "{=RedMoon_Event_Msg_3}{heroName} threw {menLostToReligion} men out of his party for being unworthy.")
	            .SetTextVariable("menLostToReligion", menLostToReligion)
	            .SetTextVariable("heroName", heroName)
	            .ToString();
            
            var eventMsg4 = new TextObject(
		            "{=RedMoon_Event_Msg_4}{heroName} lost {menLostToReligion} men and {goldLostToReligion} gold after ignoring the men last night.")
	            .SetTextVariable("goldLostToReligion", goldLostToReligion)
	            .SetTextVariable("menLostToReligion", menLostToReligion)
	            .SetTextVariable("heroName", heroName)
	            .ToString();
            
                        var msid = new MultiSelectionInquiryData(eventTitle, eventDescription, inquiryElements, false, 1, eventButtonText1, null,
                elements =>
                {
                    switch ((string)elements[0].Identifier)
                    {
                        case "a":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionAText, true, false, eventButtonText2, null, null, null), true);
                            
                            Hero.MainHero.ChangeHeroGold(-goldLostToReligion);
                            
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg1, RandomEventsSubmodule.Msg_Color));
                            
                            break;
                        case "b":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionBText, true, false, eventButtonText2, null, null, null), true);
                            
                            Hero.MainHero.ChangeHeroGold(-goldLostToReligion);
                            MobileParty.MainParty.MemberRoster.KillNumberOfMenRandomly(menLostToReligion, false);
                            
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg2, RandomEventsSubmodule.Msg_Color));
                            
                            break;
                        case "c":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionCText, true, false, eventButtonText2, null, null, null), true);
                            
                            MobileParty.MainParty.MemberRoster.KillNumberOfMenRandomly(menLostToReligion, false);
                            
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg3, RandomEventsSubmodule.Msg_Color));
                            
                            break;
                        case "d":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionDText, true, false, eventButtonText2, null, null, null), true);
                            
                            Hero.MainHero.ChangeHeroGold(-goldLostToReligion);
                            MobileParty.MainParty.MemberRoster.KillNumberOfMenRandomly(menLostToReligion, false);
                            
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg4, RandomEventsSubmodule.Msg_Color));
                            
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
	

	public class RedMoonData : RandomEventData
	{
		public readonly int minGoldLost;
		public readonly int maxGoldLost;
		public readonly int minMenLost;
		public readonly int maxMenLost;

		public RedMoonData(string eventType, float chanceWeight, int minGoldLost, int maxGoldLost, int minMenLost, int maxMenLost) : base(eventType, chanceWeight)
		{
			this.minGoldLost = minGoldLost;
			this.maxGoldLost = maxGoldLost;
			this.minMenLost = minMenLost;
			this.maxMenLost = maxMenLost;
		}

		public override BaseEvent GetBaseEvent()
		{
			return new RedMoon();
		}
	}
}
