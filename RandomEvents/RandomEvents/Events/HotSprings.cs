using System;
using System.Windows;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Library;

namespace CryingBuffalo.RandomEvents.Events
{
	public sealed class HotSprings : BaseEvent
	{
		private readonly int moraleGain;

		public HotSprings() : base(Settings.Settings.RandomEvents.HotSpringsData)
		{
			moraleGain = Settings.Settings.RandomEvents.HotSpringsData.moraleGain;
		}

		public override void CancelEvent()
		{
		}

		public override bool CanExecuteEvent()
		{
			return MobileParty.MainParty.CurrentSettlement == null;
		}

		public override void StartEvent()
		{
			MobileParty.MainParty.RecentEventsMorale += moraleGain;
			MobileParty.MainParty.MoraleExplained.Add(moraleGain, new TaleWorlds.Localization.TextObject("Random Event"));

			for (int i = 0; i < PartyBase.MainParty.MemberRoster.Count; i++)
			{
				TroopRosterElement elementCopyAtIndex = PartyBase.MainParty.MemberRoster.GetElementCopyAtIndex(i);
				if (elementCopyAtIndex.Character.IsHero)
				{
					elementCopyAtIndex.Character.HeroObject.Heal(PartyBase.MainParty, 100, false);
				}
				else
				{
					MobileParty.MainParty.Party.AddToMemberRosterElementAtIndex(i, 0, -PartyBase.MainParty.MemberRoster.GetElementWoundedNumber(i));
				}
			}

			InformationManager.ShowInquiry(
				new InquiryData("The Hot Springs",
					$"You stumble upon some beautiful hot springs. After bathing with your soldiers you feel fantastic!",
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

	public class HotSpringsData : RandomEventData
	{
		public readonly int moraleGain;

		public HotSpringsData(string eventType, float chanceWeight, int moraleGain) : base(eventType, chanceWeight)
		{
			this.moraleGain = moraleGain;
		}

		public override BaseEvent GetBaseEvent()
		{
			return new HotSprings();
		}
	}
}
