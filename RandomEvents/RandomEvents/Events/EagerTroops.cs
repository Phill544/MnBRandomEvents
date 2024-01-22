using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Bannerlord.RandomEvents.Helpers;
using Bannerlord.RandomEvents.Settings;
using Ini.Net;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace Bannerlord.RandomEvents.Events
{
	public sealed class EagerTroops : BaseEvent
	{
		private readonly bool eventDisabled;
		private readonly int minTroopGain;
		private readonly int maxTroopGain;

		public EagerTroops() : base(ModSettings.RandomEvents.EagerTroopsData)
		{
			var ConfigFile = new IniFile(ParseIniFile.GetTheConfigFile());
            
			eventDisabled = ConfigFile.ReadBoolean("EagerTroops", "EventDisabled");
			minTroopGain = ConfigFile.ReadInteger("EagerTroops", "MinTroopGain");
			maxTroopGain = ConfigFile.ReadInteger("EagerTroops", "MaxTroopGain");
		}

		public override void CancelEvent()
		{
		}
		
		private bool HasValidEventData()
		{
			if (eventDisabled == false)
			{
				if (minTroopGain != 0 || maxTroopGain != 0)
				{
					return true;
				}
			}
            
			return false;
		}

		public override bool CanExecuteEvent()
		{
			return HasValidEventData() && MobileParty.MainParty.Party.PartySizeLimit >= MobileParty.MainParty.MemberRoster.TotalHealthyCount + minTroopGain;
		}

		public override void StartEvent()
		{
			var realMaxTroopGain = Math.Min(MobileParty.MainParty.Party.PartySizeLimit - MobileParty.MainParty.MemberRoster.TotalHealthyCount, maxTroopGain);
			var numberToAdd = MBRandom.RandomInt(minTroopGain, realMaxTroopGain);

			var settlements = Settlement.FindAll(s => !s.IsHideout).ToList();
			var closestSettlement = settlements.MinBy(s => MobileParty.MainParty.GetPosition().DistanceSquared(s.GetPosition()));
			
			var eventTitle = new TextObject("{=EagerTroops_Title}Eager Troops!").ToString();
			
			var eventDescription = new TextObject("{=EagerTroops_Event_Desc}You come across {numberToAdd} troops that are eager for battle and glory. They want to join your ranks!")
				.SetTextVariable("numberToAdd", numberToAdd)
				.ToString();
			
			var eventOutcome1 = new TextObject("{=EagerTroops_Event_Text_2}Disappointed, the soldiers leave.")
				.ToString();
				
			var eventButtonText1 = new TextObject("{=EagerTroops_Event_Button_Text_1}Okay").ToString();
			var eventButtonText2 = new TextObject("{=EagerTroops_Event_Button_Text_2}Done").ToString();
			
			var eventOption1 = new TextObject("{=EagerTroops_Event_Option_1}Accept").ToString();
			var eventOption2 = new TextObject("{=EagerTroops_Event_Option_2}Decline").ToString();

			var inquiryElements = new List<InquiryElement>
			{
				new InquiryElement("a", eventOption1, null),
				new InquiryElement("b", eventOption2, null)
			};

			var msid = new MultiSelectionInquiryData(eventTitle, eventDescription, inquiryElements, false, 1, 1, eventButtonText1, null, 
				elements => 
				{
					switch ((string)elements[0].Identifier)
					{
						case "a":
						{
							var bandits = PartySetup.CreateBanditParty();
							bandits.MemberRoster.Clear();
							PartySetup.AddRandomCultureUnits(bandits, numberToAdd, closestSettlement.Culture);

							MobileParty.MainParty.MemberRoster.Add(bandits.MemberRoster);

							bandits.RemoveParty();
							break;
						}
						case "b":
							InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOutcome1, true, false, eventButtonText2, null, null, null), true);
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

	public class EagerTroopsData : RandomEventData
	{

		public EagerTroopsData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
		{
		}

		public override BaseEvent GetBaseEvent()
		{
			return new EagerTroops();
		}
	}
}
