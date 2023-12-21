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

namespace Bannerlord.RandomEvents.Events
{
	public sealed class TargetPractice : BaseEvent
	{
		private readonly bool eventDisabled;
		private readonly int minimumSoldiers;
		private readonly float percentageDifferenceOfCurrentTroop;

		public TargetPractice() : base(ModSettings.RandomEvents.TargetPracticeData)
		{
			var ConfigFile = new IniFile(ParseIniFile.GetTheConfigFile());
            
			eventDisabled = ConfigFile.ReadBoolean("TargetPractice", "EventDisabled");
			minimumSoldiers = ConfigFile.ReadInteger("TargetPractice", "MinimumSoldiers");
			percentageDifferenceOfCurrentTroop = ConfigFile.ReadFloat("TargetPractice", "PercentageDifferenceOfCurrentTroop");
		}

		public override void CancelEvent()
		{
		}
		
		private bool HasValidEventData()
		{
			if (eventDisabled == false)
			{
				if (minimumSoldiers != 0 || percentageDifferenceOfCurrentTroop != 0)
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
			var percentOffset = MBRandom.RandomFloatRanged(-percentageDifferenceOfCurrentTroop, percentageDifferenceOfCurrentTroop);

			var spawnCount = (int)(MobileParty.MainParty.MemberRoster.Count * ( 1 + percentOffset));
			
			if (spawnCount < minimumSoldiers)
				spawnCount = minimumSoldiers;
			
			var eventTitle = new TextObject("{=TargetPractice_Title}Target Practice!").ToString();

			var eventOption1 = new TextObject("{=TargetPractice_Event_Option_1}Let the soldiers have some fun!").ToString();
			var eventOption2 = new TextObject("{=TargetPractice_Event_Option_2}Do nothing").ToString();
			var eventOption2Hover = new TextObject("{=TargetPractice_Event_Option_2_Hover}Think about the experience you're giving up!").ToString();
			
			var eventButtonText1 = new TextObject("{=TargetPractice_Event_Button_Text_1}Okay").ToString();
			var eventButtonText2 = new TextObject("{=TargetPractice_Event_Button_Text_2}Good").ToString();
			var eventButtonText3 = new TextObject("{=TargetPractice_Event_Button_Text_3}Done").ToString();


			var inquiryElements = new List<InquiryElement>
			{
				new InquiryElement("a", eventOption1, null),
				new InquiryElement("b", eventOption2, null, true, eventOption2Hover)
			};
			
			var eventOptionAText = new TextObject(
					"{=TargetPractice_Event_Choice_1}The looters look terrified.")
				.ToString();
            
			var eventOptionBText = new TextObject(
					"{=TargetPractice_Event_Choice_2}The looters, seeing that you aren't about to attack, quickly scatter to the wind. Your soldiers grumble.")
				.ToString();

			var msid = new MultiSelectionInquiryData(eventTitle, CalculateDescription(spawnCount), inquiryElements, false, 1, 1, eventButtonText1, null, 
				elements => 
				{
					switch ((string)elements[0].Identifier)
					{
						case "a":
							InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionAText, true, false, eventButtonText2, null, null, null), true);
							SpawnLooters(spawnCount);
							break;
						case "b":
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

		private string CalculateDescription(int spawnCount)
		{
			
			var eventSizeDesc1 = new TextObject("{=TargetPractice_Event_Size_Desc_1}reasonable").ToString();
			var eventSizeDesc2 = new TextObject("{=TargetPractice_Event_Size_Desc_2}large").ToString();
			var eventSizeDesc3 = new TextObject("{=TargetPractice_Event_Size_Desc_3}huge").ToString();
			
			
			string sizeDescription;
			if (spawnCount <= minimumSoldiers)
			{
				sizeDescription = eventSizeDesc1;
			}
			else if (spawnCount < minimumSoldiers * 1.5f)
			{
				sizeDescription = eventSizeDesc2;
			}
			else
			{
				sizeDescription = eventSizeDesc3;
			}

			var eventDescription = new TextObject(
					"{=TargetPractice_Event_Desc}You stumble upon a {sizeDescription} amount of looters! Your soldiers seem very eager to show you what they've learned.")
				.SetTextVariable("sizeDescription", sizeDescription)
				.ToString();


			string description = eventDescription;

			return description;
		}

		private static void SpawnLooters(int spawnCount)
		{
			MobileParty looterParty = PartySetup.CreateLooterParty();

			looterParty.MemberRoster.Clear();

			looterParty.Aggressiveness = 10f;
			looterParty.Ai.SetMoveEngageParty(MobileParty.MainParty);
			
			
			PartySetup.AddRandomCultureUnits(looterParty, spawnCount);
		}
	}

	public class TargetPracticeData : RandomEventData
	{
		public TargetPracticeData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
		{
		}

		public override BaseEvent GetBaseEvent()
		{
			return new TargetPractice();
		}
	}
}
