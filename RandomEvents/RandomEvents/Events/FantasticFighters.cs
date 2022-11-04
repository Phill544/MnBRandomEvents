using System;
using System.Windows;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace CryingBuffalo.RandomEvents.Events
{
	public sealed class FantasticFighters : BaseEvent
	{
		private readonly int renownGain;

		public FantasticFighters() : base(Settings.ModSettings.RandomEvents.FantasticFightersData)
		{
			renownGain = Settings.ModSettings.RandomEvents.FantasticFightersData.renownGain;
		}

		public override void CancelEvent()
		{
		}

		public override bool CanExecuteEvent()
		{
			return Hero.MainHero.Clan != null;
		}

		public override void StartEvent()
		{
			try
			{
				Hero.MainHero.Clan.Renown += renownGain;
				
				var eventTitle = new TextObject("{=FantasticFighters_Title}Fantastic Fighters?").ToString();
			
				var eventOption1 = new TextObject("{=FantasticFighters_Event_Text}A rumor spreads that your clan managed to decisively win a battle when outnumbered 10-1.")
					.ToString();
				
				var eventButtonText = new TextObject("{=FantasticFighters_Event_Button_Text}Done").ToString();

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

	public class FantasticFightersData : RandomEventData
	{
		public readonly int renownGain;

		public FantasticFightersData(string eventType, float chanceWeight, int renownGain) : base(eventType, chanceWeight)
		{
			this.renownGain = renownGain;
		}

		public override BaseEvent GetBaseEvent()
		{
			return new FantasticFighters();
		}
	}
}
