using System;
using System.Windows;
using CryingBuffalo.RandomEvents.Settings.MCM;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace CryingBuffalo.RandomEvents.Events.BicEvents
{
	public sealed class Courier : BaseEvent
	{
		private readonly int minMoraleGain;
		private readonly int maxMoraleGain;

		public Courier() : base(Settings.ModSettings.RandomEvents.CourierData)
		{
			minMoraleGain = MCM_MenuConfig_A_F.Instance.CR_minMoraleGain;
			maxMoraleGain = MCM_MenuConfig_A_F.Instance.CR_maxMoraleGain;
		}

		public override void CancelEvent()
		{
		}

		public override bool CanExecuteEvent()
		{
			return MCM_MenuConfig_A_F.Instance.CR_Disable == false;
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
