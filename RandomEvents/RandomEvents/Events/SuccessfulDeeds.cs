using System;
using System.Windows;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace CryingBuffalo.RandomEvents.Events
{
	public sealed class SuccessfulDeeds : BaseEvent
	{

		private readonly float influenceGain;

		public SuccessfulDeeds() : base(Settings.ModSettings.RandomEvents.SuccessfulDeedsData)
		{
			influenceGain = Settings.ModSettings.RandomEvents.SuccessfulDeedsData.influenceGain;
		}

		public override void CancelEvent()
		{
		}

		public override bool CanExecuteEvent()
		{
			return Hero.MainHero.Clan.Kingdom != null;
		}

		public override void StartEvent()
		{
			if (Settings.ModSettings.GeneralSettings.DebugMode)
			{
				InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}", RandomEventsSubmodule.Dbg_Color));
			}
			
			try
			{
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
		public readonly float influenceGain;

		public SuccessfulDeedsData(string eventType, float chanceWeight, float influenceGain) : base(eventType, chanceWeight)
		{
			this.influenceGain = influenceGain;
		}

		public override BaseEvent GetBaseEvent()
		{
			return new SuccessfulDeeds();
		}
	}
}
