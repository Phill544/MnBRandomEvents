using System;
using System.Windows;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace CryingBuffalo.RandomEvents.Events
{
	public sealed class Momentum : BaseEvent
	{
		private const string EventTitle = "Momentum";
		
		public Momentum() : base(Settings.ModSettings.RandomEvents.MomentumData)
		{
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
			bool isOnFoot = Hero.MainHero.CharacterObject.Equipment.Horse.IsEmpty;

			string dialogue;

			if (isOnFoot)
			{
				float xpToGive = Settings.ModSettings.GeneralSettings.GeneralLevelXpMultiplier * Hero.MainHero.GetSkillValue(DefaultSkills.Athletics);
				Hero.MainHero.AddSkillXp(DefaultSkills.Athletics, xpToGive);
				dialogue = "on foot";
			}
			else
			{
				float xpToGive = Settings.ModSettings.GeneralSettings.GeneralLevelXpMultiplier * Hero.MainHero.GetSkillValue(DefaultSkills.Riding);
				Hero.MainHero.AddSkillXp(DefaultSkills.Riding, xpToGive);
				dialogue = "riding";
			}

			InformationManager.ShowInquiry(
				new InquiryData(EventTitle,
					$"After spending so much time {dialogue} you gain a second wind!",
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

	public class MomentumData : RandomEventData
	{
		public MomentumData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
		{
		}

		public override BaseEvent GetBaseEvent()
		{
			return new Momentum();
		}
	}
}
