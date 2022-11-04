using System;
using System.Windows;
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

		public SpeedyRecovery() : base(Settings.ModSettings.RandomEvents.SpeedyRecoveryData)
		{
			minTroopsToHeal = Settings.ModSettings.RandomEvents.SpeedyRecoveryData.minTroopsToHeal;
			maxTroopsToHeal = Settings.ModSettings.RandomEvents.SpeedyRecoveryData.maxTroopsToHeal;
		}

		public override void CancelEvent()
		{
		}

		public override bool CanExecuteEvent()
		{
			return MobileParty.MainParty.MemberRoster.TotalWoundedRegulars >= minTroopsToHeal;
		}

		public override void StartEvent()
		{
			try
			{
				int totalToHeal = MBRandom.RandomInt(minTroopsToHeal, Math.Min(maxTroopsToHeal, MobileParty.MainParty.MemberRoster.TotalWoundedRegulars));
				int totalHealed = 0;

				while (totalHealed < totalToHeal)
				{
					int randomElement = MBRandom.RandomInt(MobileParty.MainParty.MemberRoster.Count);
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
		public readonly int minTroopsToHeal;
		public readonly int maxTroopsToHeal;

		public SpeedyRecoveryData(string eventType, float chanceWeight, int minTroopsToHeal, int maxTroopsToHeal) : base(eventType, chanceWeight)
		{
			this.minTroopsToHeal = minTroopsToHeal;
			this.maxTroopsToHeal = maxTroopsToHeal;
		}

		public override BaseEvent GetBaseEvent()
		{
			return new SpeedyRecovery();
		}
	}
}
