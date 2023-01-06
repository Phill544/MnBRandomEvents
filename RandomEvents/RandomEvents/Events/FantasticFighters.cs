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
	public sealed class FantasticFighters : BaseEvent
	{
		private readonly int minRenownGain;
		private readonly int maxRenownGain;

		public FantasticFighters() : base(ModSettings.RandomEvents.FantasticFightersData)
		{
			minRenownGain = 25;
			maxRenownGain = 75;
		}

		public override void CancelEvent()
		{
		}

		public override bool CanExecuteEvent()
		{
			return MCM_MenuConfig_Toggle.Instance.FF_Disable == false && Hero.MainHero.Clan != null;
		}

		public override void StartEvent()
		{
			if (MCM_ConfigMenu_General.Instance.GS_DebugMode)
			{
				InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}", RandomEventsSubmodule.Dbg_Color));
			}
			try
			{

				var renownGain = MBRandom.RandomInt(minRenownGain, maxRenownGain);
				
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

		public FantasticFightersData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
		{
		}

		public override BaseEvent GetBaseEvent()
		{
			return new FantasticFighters();
		}
	}
}
