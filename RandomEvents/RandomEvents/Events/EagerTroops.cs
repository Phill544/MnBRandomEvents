using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using CryingBuffalo.RandomEvents.Helpers;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace CryingBuffalo.RandomEvents.Events
{
	public sealed class EagerTroops : BaseEvent
	{
		private readonly int minTroopGain;
		private readonly int maxTroopGain;

		private const string EventTitle = "Eager Troops!";

		public EagerTroops() : base(Settings.ModSettings.RandomEvents.EagerTroopsData)
		{
			minTroopGain = Settings.ModSettings.RandomEvents.EagerTroopsData.minTroopGain;
			maxTroopGain = Settings.ModSettings.RandomEvents.EagerTroopsData.maxTroopGain;
		}

		public override void CancelEvent()
		{
		}

		public override bool CanExecuteEvent()
		{
			return MobileParty.MainParty.Party.PartySizeLimit >= MobileParty.MainParty.MemberRoster.TotalHealthyCount + minTroopGain;
		}

		public override void StartEvent()
		{
			if (Settings.ModSettings.GeneralSettings.DebugMode)
			{
				InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}", RandomEventsSubmodule.Dbg_Color));
			}

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

			var msid = new MultiSelectionInquiryData(eventTitle, eventDescription, inquiryElements, false, 1, eventButtonText1, null, 
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

	public class EagerTroopsData : RandomEventData
	{
		public readonly int minTroopGain;
		public readonly int maxTroopGain;

		public EagerTroopsData(string eventType, float chanceWeight, int minTroopGain, int maxTroopGain) : base(eventType, chanceWeight)
		{
			this.minTroopGain = minTroopGain;
			this.maxTroopGain = maxTroopGain;
		}

		public override BaseEvent GetBaseEvent()
		{
			return new EagerTroops();
		}
	}
}
