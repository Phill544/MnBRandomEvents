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
	public sealed class Courier : BaseEvent
	{
		private readonly bool eventDisabled;
		private readonly int minMoraleGain;
		private readonly int maxMoraleGain;

		public Courier() : base(ModSettings.RandomEvents.CourierData)
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
			return HasValidEventData();
		}

		public override void StartEvent()
		{
			var heroName = Hero.MainHero.FirstName;

			var moraleGain = MBRandom.RandomInt(minMoraleGain, maxMoraleGain);

			MobileParty.MainParty.RecentEventsMorale += moraleGain;
			MobileParty.MainParty.MoraleExplained.Add(moraleGain);

			var eventTitle = new TextObject("{=Courier_Title}A Courier Arrives").ToString();
			
			var eventOption1 = new TextObject("{=Courier_Event_Text}A courier has arrived with a handful of letters for {heroName}'s party. The men seem quite excited, at least those who know how to read.")
				.SetTextVariable("heroName", heroName)
				.ToString();
		
				
			var eventButtonText = new TextObject("{=Courier_Event_Button_Text}Accept").ToString();

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

	public class CourierData : RandomEventData
	{

		public CourierData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
		{

		}

		public override BaseEvent GetBaseEvent()
		{
			return new Courier();
		}
	}
}
