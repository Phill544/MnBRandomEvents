using System;
using System.Windows;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace CryingBuffalo.RandomEvents.Events
{
	public sealed class SecretSinger : BaseEvent
	{
		private readonly int moraleGain;

		public SecretSinger() : base(Settings.ModSettings.RandomEvents.SecretSingerData)
		{
			moraleGain = Settings.ModSettings.RandomEvents.SecretSingerData.moraleGain;
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
			MobileParty.MainParty.RecentEventsMorale += moraleGain;
			MobileParty.MainParty.MoraleExplained.Add(moraleGain);
			
			var eventTitle = new TextObject("{=SecretSinger_Title}Secret Singer!").ToString();
			
			var eventOption1 = new TextObject("{=SecretSinger_Event_Text}You discover one of your party members is an extremely good singer!")
				.ToString();
				
			var eventButtonText = new TextObject("{=SecretSinger_Event_Button_Text}Done").ToString();

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

	public class SecretSingerData : RandomEventData
	{
		public readonly int moraleGain;

		public SecretSingerData(string eventType, float chanceWeight, int moraleGain) : base(eventType, chanceWeight)
		{
			this.moraleGain = moraleGain;
		}

		public override BaseEvent GetBaseEvent()
		{
			return new SecretSinger();
		}
	}
}
