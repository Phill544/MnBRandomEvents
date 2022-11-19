using System;
using System.Windows;
using CryingBuffalo.RandomEvents.Settings;
using CryingBuffalo.RandomEvents.Settings.MCM;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace CryingBuffalo.RandomEvents.Events
{
	public sealed class SpeedyRecovery : BaseEvent
	{
		private readonly int minTroopsToHeal;
		private readonly int maxTroopsToHeal;

		public SpeedyRecovery() : base(ModSettings.RandomEvents.SpeedyRecoveryData)
		{
			minTroopsToHeal = MCM_MenuConfig_N_Z.Instance.SR_MinMenToRecover;
			maxTroopsToHeal = MCM_MenuConfig_N_Z.Instance.SR_MaxMenToRecover;
		}

		public override void CancelEvent()
		{
		}

		public override bool CanExecuteEvent()
		{
			return MCM_MenuConfig_N_Z.Instance.SR_Disable == false && MobileParty.MainParty.MemberRoster.TotalWoundedRegulars >= minTroopsToHeal;
		}

		public override void StartEvent()
		{
			if (MCM_ConfigMenu_General.Instance.GS_DebugMode)
			{
				InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}", RandomEventsSubmodule.Dbg_Color));
			}
			
			try
			{
				var totalToHeal = MBRandom.RandomInt(minTroopsToHeal, Math.Min(maxTroopsToHeal, MobileParty.MainParty.MemberRoster.TotalWoundedRegulars));
				
				var totalHealed = 0;

				while (totalHealed < totalToHeal)
				{
					var randomElement = MBRandom.RandomInt(MobileParty.MainParty.MemberRoster.Count);
					
					while (MobileParty.MainParty.MemberRoster.GetElementWoundedNumber(randomElement) == 0 || !MobileParty.MainParty.MemberRoster.GetCharacterAtIndex(randomElement).IsRegular)
					{
						randomElement++;

						if (randomElement == MobileParty.MainParty.MemberRoster.Count)
						{
							randomElement = 0;
						}
					}

					MobileParty.MainParty.MemberRoster.AddToCountsAtIndex(randomElement, 0, -1);
					totalHealed++;
				}
				
				var eventTitle = new TextObject("{=SpeedyRecovery_Title}Speedy Recovery!").ToString();
			
				var eventOption1 = new TextObject("{=SpeedyRecovery_Event_Text}You receive word that a group of your troops are feeling better, and are ready for combat.")
					.ToString();
				
				var eventButtonText = new TextObject("{=SpeedyRecovery_Event_Button_Text}Done").ToString();

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
	

	public class SpeedyRecoveryData : RandomEventData
	{
		public SpeedyRecoveryData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
		{
		}

		public override BaseEvent GetBaseEvent()
		{
			return new SpeedyRecovery();
		}
	}
}
