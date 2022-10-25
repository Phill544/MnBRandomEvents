using System;
using System.Windows;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Library;

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
			MobileParty.MainParty.MoraleExplained.Add(moraleGain, new TaleWorlds.Localization.TextObject("Random Event"));

			InformationManager.ShowInquiry(
				new InquiryData("Secret Singer!",
					"You discover one of your party members is an extremely good singer!",
					true,
					false,
					"Done",
					null,
					null,
					null
					),
				true);

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
