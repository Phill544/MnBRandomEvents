using System;
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
	public sealed class HotSprings : BaseEvent
	{
		private readonly bool eventDisabled;
		private readonly int minMoraleGain;
		private readonly int maxMoraleGain;

		public HotSprings() : base(ModSettings.RandomEvents.HotSpringsData)
		{
			var ConfigFile = new IniFile(ParseIniFile.GetTheConfigFile());
            
			eventDisabled = ConfigFile.ReadBoolean("HotSprings", "EventDisabled");
			minMoraleGain = ConfigFile.ReadInteger("HotSprings", "MinMoraleGain");
			maxMoraleGain = ConfigFile.ReadInteger("HotSprings", "MaxMoraleGain");
		}

		public override void CancelEvent()
		{
		}
		
		private bool HasValidEventData()
		{
			if (eventDisabled == false)
			{
				if (minMoraleGain != 0 || maxMoraleGain != 0)
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
			var moraleGain = MBRandom.RandomInt(minMoraleGain, maxMoraleGain);
			
			MobileParty.MainParty.RecentEventsMorale += moraleGain;
			MobileParty.MainParty.MoraleExplained.Add(moraleGain);

			for (var i = 0; i < PartyBase.MainParty.MemberRoster.Count; i++)
			{
				var elementCopyAtIndex = PartyBase.MainParty.MemberRoster.GetElementCopyAtIndex(i);

				switch (elementCopyAtIndex.Character.IsHero)
				{
					case true:
						elementCopyAtIndex.Character.HeroObject.Heal(100);
						break;
					default:
						MobileParty.MainParty.Party.AddToMemberRosterElementAtIndex(i, 0, -PartyBase.MainParty.MemberRoster.GetElementWoundedNumber(i));
						break;
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
