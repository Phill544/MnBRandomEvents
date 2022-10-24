using System;
using System.Windows;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace CryingBuffalo.RandomEvents.Events
{
	public sealed class Undercooked : BaseEvent
	{
		private readonly int minTroopsToInjure;
		private readonly int maxTroopsToInjure;

		public Undercooked() : base(Settings.Settings.RandomEvents.UndercookedData)
		{
			minTroopsToInjure = Settings.Settings.RandomEvents.UndercookedData.minTroopsToInjure;
			maxTroopsToInjure = Settings.Settings.RandomEvents.UndercookedData.maxTroopsToInjure;
		}

		public override void CancelEvent()
		{
		}

		public override bool CanExecuteEvent()
		{
			return (MobileParty.MainParty.MemberRoster.TotalRegulars - MobileParty.MainParty.MemberRoster.TotalWoundedRegulars) >= minTroopsToInjure;
		}

		public override void StartEvent()
		{
			try
			{
				int numberToInjure = MBRandom.RandomInt(minTroopsToInjure, maxTroopsToInjure);
				numberToInjure = Math.Min(numberToInjure, maxTroopsToInjure);

				MobileParty.MainParty.MemberRoster.WoundNumberOfTroopsRandomly(numberToInjure);

				InformationManager.ShowInquiry(
					new InquiryData("Undercooked",
						"Some of your troops fall ill to bad food, although you're unsure of what caused it, you're glad it wasn't you.",
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
		public readonly int minTroopsToInjure;
		public readonly int maxTroopsToInjure;

		public UndercookedData(string eventType, float chanceWeight, int minTroopsToInjure, int maxTroopsToInjure) : base(eventType, chanceWeight)
		{
			this.minTroopsToInjure = minTroopsToInjure;
			this.maxTroopsToInjure = maxTroopsToInjure;
		}

		public override BaseEvent GetBaseEvent()
		{
			return new Undercooked();
		}
	}
}
