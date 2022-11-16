using System;
using System.Windows;
using CryingBuffalo.RandomEvents.Settings;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace CryingBuffalo.RandomEvents.Events
{
	public sealed class Undercooked : BaseEvent
	{
		private readonly int minTroopsToInjure;
		private readonly int maxTroopsToInjure;

		public Undercooked() : base(ModSettings.RandomEvents.UndercookedData)
		{
			minTroopsToInjure = MCM_MenuConfig_N_Z.Instance.UC_MinSoldiersToInjure;
			maxTroopsToInjure = MCM_MenuConfig_N_Z.Instance.UC_MaxSoldiersToInjure;
		}

		public override void CancelEvent()
		{
		}

		public override bool CanExecuteEvent()
		{
			return MCM_MenuConfig_N_Z.Instance.UC_Disable == false && (MobileParty.MainParty.MemberRoster.TotalRegulars - MobileParty.MainParty.MemberRoster.TotalWoundedRegulars) >= minTroopsToInjure;
		}

		public override void StartEvent()
		{
			if (MCM_ConfigMenu_General.Instance.GS_DebugMode)
			{
				InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}", RandomEventsSubmodule.Dbg_Color));
			}
			
			try
			{
				int numberToInjure = MBRandom.RandomInt(minTroopsToInjure, maxTroopsToInjure);
				numberToInjure = Math.Min(numberToInjure, maxTroopsToInjure);

				MobileParty.MainParty.MemberRoster.WoundNumberOfTroopsRandomly(numberToInjure);
				
				var eventTitle = new TextObject("{=Undercooked_Title}Undercooked").ToString();
			
				var eventOption1 = new TextObject("{=Undercooked_Event_Text}Some of your troops fall ill to bad food, although you're unsure of what caused it, you're glad it wasn't you.")
					.ToString();
				
				var eventButtonText = new TextObject("{=Undercooked_Event_Button_Text}Done").ToString();

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

	public class UndercookedData : RandomEventData
	{
		public UndercookedData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
		{
		}

		public override BaseEvent GetBaseEvent()
		{
			return new Undercooked();
		}
	}
}
