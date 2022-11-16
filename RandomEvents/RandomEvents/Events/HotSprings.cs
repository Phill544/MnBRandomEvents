using System;
using System.Windows;
using CryingBuffalo.RandomEvents.Settings;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace CryingBuffalo.RandomEvents.Events
{
	public sealed class HotSprings : BaseEvent
	{
		private readonly int minMoraleGain;
		private readonly int maxMoraleGain;

		public HotSprings() : base(ModSettings.RandomEvents.HotSpringsData)
		{
			minMoraleGain = MCM_MenuConfig_A_M.Instance.HS_MinMoraleGain;
			maxMoraleGain = MCM_MenuConfig_A_M.Instance.HS_MaxMoraleGain;
		}

		public override void CancelEvent()
		{
		}

		public override bool CanExecuteEvent()
		{
			return MCM_MenuConfig_A_M.Instance.HS_Disable == false && MobileParty.MainParty.CurrentSettlement == null;
		}

		public override void StartEvent()
		{
			if (MCM_ConfigMenu_General.Instance.GS_DebugMode)
			{
				InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}", RandomEventsSubmodule.Dbg_Color));
			}

			var moraleGain = MBRandom.RandomInt(minMoraleGain, maxMoraleGain);
			
			MobileParty.MainParty.RecentEventsMorale += moraleGain;
			MobileParty.MainParty.MoraleExplained.Add(moraleGain);
			

			for (int i = 0; i < PartyBase.MainParty.MemberRoster.Count; i++)
			{
				TroopRosterElement elementCopyAtIndex = PartyBase.MainParty.MemberRoster.GetElementCopyAtIndex(i);
				if (elementCopyAtIndex.Character.IsHero)
				{
					elementCopyAtIndex.Character.HeroObject.Heal(100);
				}
				else
				{
					MobileParty.MainParty.Party.AddToMemberRosterElementAtIndex(i, 0, -PartyBase.MainParty.MemberRoster.GetElementWoundedNumber(i));
				}
			}
			
			var eventTitle = new TextObject("{=HotSprings_Title}The Hot Springs").ToString();
			
			var eventOption1 = new TextObject("{=HotSprings_Event_Text}You stumble upon some beautiful hot springs. After bathing with your soldiers you feel fantastic!")
				.ToString();
				
			var eventButtonText = new TextObject("{=HotSprings_Event_Button_Text}Done").ToString();

			InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOption1, true, false, eventButtonText, null, null, null), true);

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

	public class HotSpringsData : RandomEventData
	{

		public HotSpringsData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
		{
		}

		public override BaseEvent GetBaseEvent()
		{
			return new HotSprings();
		}
	}
}
