using System;
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
	public sealed class DreadedSweats : BaseEvent
	{
		private readonly bool eventDisabled;
		private readonly int minMoraleLoss;
		private readonly int maxMoraleLoss;
		private readonly int minVictims;
        private readonly int maxVictims;
        

		public DreadedSweats() : base(ModSettings.RandomEvents.DreadedSweatsData)
		{
			var ConfigFile = new IniFile(ParseIniFile.GetTheConfigFile());
			
			eventDisabled = ConfigFile.ReadBoolean("DreadedSweats", "EventDisabled");
			minMoraleLoss = ConfigFile.ReadInteger("DreadedSweats", "MinMoraleLoss");
			maxMoraleLoss = ConfigFile.ReadInteger("DreadedSweats", "MaxMoraleLoss");
			minVictims = ConfigFile.ReadInteger("DreadedSweats", "MinVictims");
			maxVictims = ConfigFile.ReadInteger("DreadedSweats", "MaxVictims");

		}

		public override void CancelEvent()
		{
		}
		
		private bool HasValidEventData()
		{
			if (eventDisabled == false)
			{
				if (minMoraleLoss != 0 || maxMoraleLoss != 0 || minVictims != 0 || maxVictims != 0)
				{
					return true;
				}
			}
			return false;
		}

		public override bool CanExecuteEvent()
		{
			return HasValidEventData() && MobileParty.MainParty.MemberRoster.TotalHealthyCount >= 10;
		}

		public override void StartEvent()
		{
			var partySize = MobileParty.MainParty.MemberRoster.TotalHealthyCount;
            
			var moraleLoss = MBRandom.RandomInt(minMoraleLoss, maxMoraleLoss);
			
			var victims = MBRandom.RandomInt(minVictims, maxVictims);
			
			var totalVictims = partySize / 10 + victims;
			
			Hero.MainHero.AddSkillXp(DefaultSkills.Medicine, 5);
			
			MobileParty.MainParty.SetDisorganized(true);
			
			MobileParty.MainParty.RecentEventsMorale -= moraleLoss;
			MobileParty.MainParty.MoraleExplained.Add(-moraleLoss);
			
			MobileParty.MainParty.MemberRoster.WoundNumberOfTroopsRandomly(totalVictims);

			
			var eventTitle2 = new TextObject("{=DreadedSweats_Title}The Dreaded Sweat").ToString();

			var eventOption2 = new TextObject("{=DreadedSweats_Event_Text}There has been an outbreak of some sort of " +
			                                  "illness among the troops. Fever, debilitating aches and pains, a few of " +
			                                  "the men can barely stand. All the symptoms point towards this being what " +
			                                  "many call 'the Dreaded Sweats'.  You order your men to wash up in the " +
			                                  "creak before moving out, hopefully this won't become something too serious.")
					.ToString();

			var eventButtonText = new TextObject("{=DreadedSweats_Event_Button_Text}Done").ToString();

			InformationManager.ShowInquiry(new InquiryData(eventTitle2, eventOption2, true, false, eventButtonText, null, null, null), true);

			var eventMsg1 = new TextObject(
				"{=DreadedSweats_Event_Msg_1}{totalvictims} troops have the Dreaded Sweats!")
				.SetTextVariable("totalvictims", totalVictims)
				.ToString();
				
			InformationManager.DisplayMessage(new InformationMessage(eventMsg1, RandomEventsSubmodule.Msg_Color_POS_Outcome));
			
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

	public class DreadedSweatsData : RandomEventData
	{

		public DreadedSweatsData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
		{
		}

		public override BaseEvent GetBaseEvent()
		{
			return new DreadedSweats();
		}
	}
}
