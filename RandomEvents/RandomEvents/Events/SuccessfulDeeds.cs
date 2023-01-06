using System;
using System.Windows;
using CryingBuffalo.RandomEvents.Settings;
using CryingBuffalo.RandomEvents.Settings.MCM;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace CryingBuffalo.RandomEvents.Events
{
	public sealed class SuccessfulDeeds : BaseEvent
	{

		private readonly int minInfluenceGain;
		private readonly int maxInfluenceGain;

		public SuccessfulDeeds() : base(ModSettings.RandomEvents.SuccessfulDeedsData)
		{
			minInfluenceGain = 20;
			maxInfluenceGain = 100;
		}

		public override void CancelEvent()
		{
		}

		public override bool CanExecuteEvent()
		{
			return MCM_MenuConfig_Toggle.Instance.SD_Disable == false && Hero.MainHero.Clan.Kingdom != null;
		}

		public override void StartEvent()
		{
			if (MCM_ConfigMenu_General.Instance.GS_DebugMode)
			{
				InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}", RandomEventsSubmodule.Dbg_Color));
			}
			
			try
			{

				var influenceGain = MBRandom.RandomInt(minInfluenceGain,maxInfluenceGain);
				
				Hero.MainHero.AddInfluenceWithKingdom(influenceGain);
				
				var eventTitle = new TextObject("{=SuccessfulDeeds_Title}Successful Deeds!").ToString();
			
				var eventOption1 = new TextObject("{=SuccessfulDeeds_Event_Text}Some of your deeds have reached other members of the kingdom.")
					.ToString();
				
				var eventButtonText = new TextObject("{=SuccessfulDeeds_Event_Button_Text}Done").ToString();

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

	public class SuccessfulDeedsData : RandomEventData
	{

		public SuccessfulDeedsData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
		{
		}

		public override BaseEvent GetBaseEvent()
		{
			return new SuccessfulDeeds();
		}
	}
}
