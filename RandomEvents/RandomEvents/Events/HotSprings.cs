using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace CryingBuffalo.RandomEvents.Events
{
	public class HotSprings : BaseEvent
	{
		private int moraleGain;

		public HotSprings() : base(Settings.RandomEvents.HotSpringsData)
		{
			this.moraleGain = Settings.RandomEvents.HotSpringsData.moraleGain;
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

			List<TroopRosterElement> rosterAsList = MobileParty.MainParty.MemberRoster.ToList();

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

	public class HotSpringsData : RandomEventData
	{
		public int moraleGain;

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
