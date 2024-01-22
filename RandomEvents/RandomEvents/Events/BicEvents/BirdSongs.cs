using System;
using System.Windows;
using Bannerlord.RandomEvents.Helpers;
using Bannerlord.RandomEvents.Settings;
using Ini.Net;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace Bannerlord.RandomEvents.Events.BicEvents
{
	public sealed class BirdSongs : BaseEvent
	{
		private readonly bool eventDisabled;
		private readonly int minMoraleGain;
		private readonly int maxMoraleGain;

		public BirdSongs() : base(ModSettings.RandomEvents.BirdSongsData)
		{
			var ConfigFile = new IniFile(ParseIniFile.GetTheConfigFile());
            
			eventDisabled = ConfigFile.ReadBoolean("BirdSongs", "EventDisabled");
			minMoraleGain = ConfigFile.ReadInteger("BirdSongs", "MinMoraleGain");
			maxMoraleGain = ConfigFile.ReadInteger("BirdSongs", "MaxMoraleGain");

		}

		public override void CancelEvent()
		{
		}
		
		private bool HasValidEventData()
		{
			if (eventDisabled == false)
			{
				if (minMoraleGain != 0 || maxMoraleGain != 0 )
				{
					return true;
				}
			}
			return false;
		}

		public override bool CanExecuteEvent()
		{
			return HasValidEventData() && MobileParty.MainParty.CurrentSettlement == null && CurrentTimeOfDay.IsMorning;
		}

		public override void StartEvent()
		{
			var moraleGain = MBRandom.RandomInt(minMoraleGain, maxMoraleGain);

			MobileParty.MainParty.RecentEventsMorale += moraleGain;
			MobileParty.MainParty.MoraleExplained.Add(moraleGain);

			var eventTitle = new TextObject("{=BirdSongs_Title}Bird Songs").ToString();
			
			var eventOption1 = new TextObject("{=BirdSongs_Event_Text}This day has been blessed by the beautiful melodies of birds singing songs. Silence falls over your ranks as the relaxing sounds of " + 
			    "nature's choir brings a sense of joy to your men. This will surely boost their morale.")
				.ToString();
				
			var eventButtonText = new TextObject("{=BirdSongs_Event_Button_Text}Done").ToString();

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

	public class BirdSongsData : RandomEventData
	{

		public BirdSongsData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
		{

		}

		public override BaseEvent GetBaseEvent()
		{
			return new BirdSongs();
		}
	}
}
