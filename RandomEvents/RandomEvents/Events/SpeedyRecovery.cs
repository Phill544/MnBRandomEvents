using CryingBuffalo.RandomEvents;
using CryingBuffalo.RandomEvents.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace CryingBuffalo.RandomEvents.Events
{
	public class SpeedyRecovery : BaseEvent
	{
		private int minTroopsToHeal;
		private int maxTroopsToHeal;

		public SpeedyRecovery() : base(Settings.RandomEvents.SpeedyRecoveryData)
		{
			this.minTroopsToHeal = Settings.RandomEvents.SpeedyRecoveryData.minTroopsToHeal;
			this.maxTroopsToHeal = Settings.RandomEvents.SpeedyRecoveryData.maxTroopsToHeal;
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

					MobileParty.MainParty.MemberRoster.AddToCountsAtIndex(randomElement, 0, -1, 0, true);
					totalHealed++;
				}

				InformationManager.ShowInquiry(
					new InquiryData("Speedy Recovery!",
						$"You receive word that a group of your troops are feeling better, and are ready for combat.",
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
			catch (Exception ex)
			{
				MessageBox.Show($"Error while playing \"{this.RandomEventData.EventType}\" event :\n\n {ex.Message} \n\n { ex.StackTrace}");
			}
		}

		public override void StopEvent()
		{
			try
			{
				OnEventCompleted.Invoke();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error while stopping \"{this.RandomEventData.EventType}\" event :\n\n {ex.Message} \n\n { ex.StackTrace}");
			}
		}
	}
	

	public class SpeedyRecoveryData : RandomEventData
	{
		public int minTroopsToHeal;
		public int maxTroopsToHeal;

		public SpeedyRecoveryData(RandomEventType eventType, float chanceWeight, int minTroopsToHeal, int maxTroopsToHeal) : base(eventType, chanceWeight)
		{
			this.minTroopsToHeal = minTroopsToHeal;
			this.maxTroopsToHeal = maxTroopsToHeal;
		}
	}
}
