using System;
using System.Windows;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ObjectSystem;
using TaleWorlds.GauntletUI.Data;
using TaleWorlds.Engine;
using TaleWorlds.CampaignSystem;

namespace CryingBuffalo.RandomEvents.Events
{
	public sealed class BirdSongs : BaseEvent
	{
		private readonly int minMoraleGain;
		private readonly int maxMoraleGain;

        

		public BirdSongs() : base(Settings.ModSettings.RandomEvents.BirdSongsData)
		{
			minMoraleGain = Settings.ModSettings.RandomEvents.BirdSongsData.minMoraleGain;
			maxMoraleGain = Settings.ModSettings.RandomEvents.BirdSongsData.maxMoraleGain;
		}

		public override void CancelEvent()
		{
        
		}

		public override bool CanExecuteEvent()
		{
			return true;
		}

		public override void StartEvent()
		{


			var moraleGain = MBRandom.RandomInt(minMoraleGain, maxMoraleGain);

			MobileParty.MainParty.RecentEventsMorale += moraleGain;
			MobileParty.MainParty.MoraleExplained.Add(moraleGain);

			var eventTitle = new TextObject("{=BirdSongs_Title}Bird Songs").ToString();
			
			var eventOption1 = new TextObject("{=BirdSongs_Event_Text}This day has been blessed by the beautiful melodies of birds singing song.  Silence takes over your ranks as the relaxing sounds of nature's choir bring a sense of joy to your men.  This will surely boost their morale.")
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
		public readonly int minMoraleGain;
		public readonly int maxMoraleGain;

		public BirdSongsData(string eventType, float chanceWeight, int minMoraleGain, int maxMoraleGain) : base(eventType, chanceWeight)
		{
			this.minMoraleGain = minMoraleGain;
			this.maxMoraleGain = maxMoraleGain;
		}

		public override BaseEvent GetBaseEvent()
		{
			return new BirdSongs();
		}
	}
}
