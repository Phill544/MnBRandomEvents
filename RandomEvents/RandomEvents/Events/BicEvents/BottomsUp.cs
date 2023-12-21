using System;
using System.Collections.Generic;
using System.Windows;
using Bannerlord.RandomEvents.Helpers;
using Bannerlord.RandomEvents.Settings;
using Ini.Net;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace Bannerlord.RandomEvents.Events.BicEvents
{
	public sealed class BottomsUp : BaseEvent
	{
		private readonly bool eventDisabled;
		private readonly int minMoraleGain;
		private readonly int maxMoraleGain;
		private readonly int minGold;
		private readonly int maxGold;


		public BottomsUp() : base(ModSettings.RandomEvents.BottomsUpData)
		{
			var ConfigFile = new IniFile(ParseIniFile.GetTheConfigFile());
            
			eventDisabled = ConfigFile.ReadBoolean("BottomsUp", "EventDisabled");
			minMoraleGain = ConfigFile.ReadInteger("BottomsUp", "MinMoraleGain");
			maxMoraleGain = ConfigFile.ReadInteger("BottomsUp", "MaxMoraleGain");
			minGold = ConfigFile.ReadInteger("BottomsUp", "MinGold");
			maxGold = ConfigFile.ReadInteger("BottomsUp", "MaxGold");
		}

		public override void CancelEvent()
		{
		}
		
		private bool HasValidEventData()
		{
			if (eventDisabled == false)
			{
				if (minMoraleGain != 0 || maxMoraleGain != 0 || minGold != 0 || maxGold != 0)
				{
					return true;
				}
			}
			return false;
		}

		public override bool CanExecuteEvent()
		{
			return HasValidEventData() && MobileParty.MainParty.CurrentSettlement != null && (MobileParty.MainParty.CurrentSettlement.IsTown && CurrentTimeOfDay.IsNight && Clan.PlayerClan.Renown >= 500||
				 HasValidEventData() && MobileParty.MainParty.CurrentSettlement != null && MobileParty.MainParty.CurrentSettlement.IsVillage) && CurrentTimeOfDay.IsNight && Clan.PlayerClan.Renown >= 500;
		}

		public override void StartEvent()
		{
			var goldGain = MBRandom.RandomInt(minGold, maxGold);
			var moraleGain = MBRandom.RandomInt(minMoraleGain, maxMoraleGain);
			var currentSettlement = MobileParty.MainParty.CurrentSettlement.Name;

			var eventTitle = new TextObject("{=BottomsUp_Title}Bottoms Up").ToString();

			var eventDescription = new TextObject("{=BottomsUp_Event_Text}While in {currentSettlement} you decide to " +
			                                      "make your way to the tavern with a few of your men for a night of " +
			                                      "fun and games. After a few drinks a young man begins boasting his " +
			                                      "drinking abilities, declaring himself the king of this tavern. 'No " +
			                                      "man can out-drink me!' he declares.  Your men turn to you and smile, " +
			                                      "wondering if you will take this boy up on his claim.")
				.SetTextVariable("currentSettlement", currentSettlement)
				.ToString();

			//option A ---- Challenge ----
			var eventOption1 = new TextObject("{=BottomsUp_Event_Option_1}Challenge").ToString();
			var eventOption1Hover = new TextObject("{=BottomsUp_Event_Option_1_Hover}Show this boy who's boss").ToString();

			var eventOption2 = new TextObject("{=BottomsUp_Event_Option_2}Decline").ToString();
			var eventOption2Hover = new TextObject("{=BottomsUp_Event_Option_2_Hover}Best not to interfere").ToString();

			var inquiryElements = new List<InquiryElement>
			{
				new InquiryElement("a", eventOption1, null, true, eventOption1Hover),
				new InquiryElement("b", eventOption2, null, true, eventOption2Hover),
			};

			//Bottom Left Game Messages ____________________________________
			var eventMsg1 = new TextObject(
					"{=BottomsUp_Event_Msg_1}You won! Earning {gold} gold!")
				.SetTextVariable("gold", goldGain)
				.ToString();

			// AFTER Challenge -------------------------------
			var eventOptionAText = new TextObject(
				   "{=BottomsUp_Event_Choice_1}You walk over to the boy and place a large mug on the table.  His eyes " +
				   "light up, followed by a cold hard stare as he grabs the handle and clears his throat. You hear " +
				   "patrons begin placing bets on who will win, adding to a victor's pot - winner takes 20%. A young " +
				   "maiden comes to your side and begins counting down. \n\nThree!  Two!  One!  Drink! \n\nThe boy " +
				   "raises his mug quickly and begins chugging as fast as he can, while you just stand there smiling, " +
				   "looking around. You can hear your men laughing in the background as you boast your lack of urgency. " +
				   "When the boy is about half way finished you finally take your mug and begin drinking. Within seconds " +
				   "you slam the mug back on the table, empty. The young man realizes you have already finished and " +
				   "gasps for air. His cheeks turn red as the whole tavern erupts in laughter. You see the shame in his " +
				   "eyes, so you grab his hand and raise it to the air, ensuring no hard feelings as you buy him another drink.")
			   .ToString();

			// AFTER Decline -------------------------------
			var eventOptionBText = new TextObject(
				   "{=BottomsUp_Event_Choice_2}You look up at your men and give a little smirk, knowing full well you " +
				   "could have crushed this boy's dreams but sometimes it's best to let others have their moment.")
			   .ToString();

			var eventButtonText1 = new TextObject("{=BottomsUp_Event_Button_Text_1}Choose").ToString();
			var eventButtonText2 = new TextObject("{=BottomsUp_Event_Button_Text_2}Done").ToString();

			var msid = new MultiSelectionInquiryData(eventTitle, eventDescription, inquiryElements, false, 1, 1, eventButtonText1, null,
				elements =>
                {
					switch ((string)elements[0].Identifier)
                    {
						case "a":
							InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionAText, true, false, eventButtonText2, null, null, null), true);
							
							Hero.MainHero.ChangeHeroGold(goldGain);
							
							MobileParty.MainParty.RecentEventsMorale += moraleGain;
							MobileParty.MainParty.MoraleExplained.Add(+moraleGain);

							InformationManager.DisplayMessage(new InformationMessage(eventMsg1, RandomEventsSubmodule.Msg_Color_POS_Outcome));

							break;

						case "b":
							InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionBText, true, false, eventButtonText2, null, null, null), true);

							break;
					}
				},
				null, null);

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

	public class BottomsUpData : RandomEventData
	{

		public BottomsUpData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
		{

		}

		public override BaseEvent GetBaseEvent()
		{
			return new BottomsUp();
		}
	}
}
