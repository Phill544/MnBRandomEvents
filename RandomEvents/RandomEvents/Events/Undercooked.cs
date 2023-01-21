using System;
using System.Windows;
using CryingBuffalo.RandomEvents.Helpers;
using CryingBuffalo.RandomEvents.Settings;
using Ini.Net;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace CryingBuffalo.RandomEvents.Events
{
	public sealed class Undercooked : BaseEvent
	{
		private readonly bool eventDisabled;
		private readonly int minTroopsToInjure;
		private readonly int maxTroopsToInjure;

		public Undercooked() : base(ModSettings.RandomEvents.UndercookedData)
		{
			var ConfigFile = new IniFile(ParseIniFile.GetTheConfigFile());
            
			eventDisabled = ConfigFile.ReadBoolean("Undercooked", "EventDisabled");
			minTroopsToInjure = ConfigFile.ReadInteger("Undercooked", "MinTroopsToInjure");
			maxTroopsToInjure = ConfigFile.ReadInteger("Undercooked", "MaxTroopsToInjure");
		}

		public override void CancelEvent()
		{
		}
		
		private bool EventCanRun()
		{
			if (eventDisabled == false)
			{
				if (minTroopsToInjure != 0 || maxTroopsToInjure != 0)
				{
					return true;
				}
			}
            
			return false;
		}

		public override bool CanExecuteEvent()
		{
			return EventCanRun() && (MobileParty.MainParty.MemberRoster.TotalRegulars - MobileParty.MainParty.MemberRoster.TotalWoundedRegulars) >= minTroopsToInjure;
		}

		public override void StartEvent()
		{
			if (GeneralSettings.DebugMode.IsActive())
			{
				InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}", RandomEventsSubmodule.Dbg_Color));
			}
			
			try
			{
				var numberToInjure = MBRandom.RandomInt(minTroopsToInjure, maxTroopsToInjure);
				
				numberToInjure = Math.Min(numberToInjure, maxTroopsToInjure);

				MobileParty.MainParty.MemberRoster.WoundNumberOfTroopsRandomly(numberToInjure);
				
				var eventTitle = new TextObject("{=Undercooked_Title}Undercooked").ToString();
			
				var eventOption1 = new TextObject("{=Undercooked_Event_Text}Some of your troops fall ill to bad food, although you're unsure of what caused it, you're glad it wasn't you.")
					.ToString();
				
				var eventButtonText = new TextObject("{=Undercooked_Event_Button_Text}Done").ToString();

				InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOption1, true, false, eventButtonText, null, null, null), true);

				StopEvent();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error while playing \"{randomEventData.eventType}\" event :\n\n {ex.Message} \n\n { ex.StackTrace}");
			}
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

	public class UndercookedData : RandomEventData
	{
		public UndercookedData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
		{
		}

		public override BaseEvent GetBaseEvent()
		{
			return new Undercooked();
		}
	}
}
