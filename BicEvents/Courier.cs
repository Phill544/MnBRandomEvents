using CryingBuffalo.RandomEvents.Helpers;
using System;
using System.Collections.Generic;
using System.Windows;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace CryingBuffalo.RandomEvents.Events
{
	public sealed class Courier : BaseEvent
	{
		private readonly int minMoraleGain;
		private readonly int maxMoraleGain;

		public Courier() : base(Settings.ModSettings.RandomEvents.CourierData)
		{
			minMoraleGain = Settings.ModSettings.RandomEvents.CourierData.minMoraleGain;
			maxMoraleGain = Settings.ModSettings.RandomEvents.CourierData.maxMoraleGain;
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
			var closestSettlement = ClosestSettlements.GetClosestAny(MobileParty.MainParty).ToString();
			var heroName = Hero.MainHero.FirstName;

			var moraleGain = MBRandom.RandomInt(minMoraleGain, maxMoraleGain);

			MobileParty.MainParty.RecentEventsMorale += moraleGain;
			MobileParty.MainParty.MoraleExplained.Add(moraleGain);

			var eventTitle = new TextObject("{=Courier_Title}A Courier Arrives").ToString();
			
			var eventOption1 = new TextObject("{=Courier_Event_Text}A courier near {closestSettlement} has arrived with a handful of letters for { heroName }'s party.  The men seem quite excited, at least those who know how to read.")
								.SetTextVariable("closestSettlement", closestSettlement)
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
		public readonly int minMoraleGain;
		public readonly int maxMoraleGain;

		public CourierData(string eventType, float chanceWeight, int minMoraleGain, int maxMoraleGain) : base(eventType, chanceWeight)
		{
			this.minMoraleGain = minMoraleGain;
			this.maxMoraleGain = maxMoraleGain;
		}

		public override BaseEvent GetBaseEvent()
		{
			return new Courier();
		}
	}
}
