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
	public sealed class PerfectWeather : BaseEvent
	{
		private readonly bool eventDisabled;
		private readonly int minMoraleGain;
		private readonly int maxMoraleGain;

		public PerfectWeather() : base(ModSettings.RandomEvents.PerfectWeatherData)
		{
			var ConfigFile = new IniFile(ParseIniFile.GetTheConfigFile());
            
			eventDisabled = ConfigFile.ReadBoolean("PerfectWeather", "EventDisabled");
			minMoraleGain = ConfigFile.ReadInteger("PerfectWeather", "MinMoraleGain");
			maxMoraleGain = ConfigFile.ReadInteger("PerfectWeather", "MaxMoraleGain");
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
			if (GeneralSettings.DebugMode.IsActive())
			{
				InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}", RandomEventsSubmodule.Dbg_Color));
			}

			var moraleGain = MBRandom.RandomInt(minMoraleGain, maxMoraleGain);
			
			MobileParty.MainParty.RecentEventsMorale += moraleGain;
			MobileParty.MainParty.MoraleExplained.Add(moraleGain);
			
			var eventTitle = new TextObject("{=PerfectWeather_Title}Perfect Weather").ToString();
			
			var eventOption1 = new TextObject("{=PerfectWeather_Event_Text}The weather today is so perfect that everyone relaxes and the mood improves!")
				.ToString();
				
			var eventButtonText = new TextObject("{=PerfectWeather_Event_Button_Text}Done").ToString();

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

	public class PerfectWeatherData : RandomEventData
	{

		public PerfectWeatherData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
		{
		}

		public override BaseEvent GetBaseEvent()
		{
			return new PerfectWeather();
		}
	}
}
