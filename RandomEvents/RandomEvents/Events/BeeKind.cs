using System;
using System.Windows;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace CryingBuffalo.RandomEvents.Events
{
	public sealed class BeeKind : BaseEvent
	{
		private const string EventTitle = "Bee Kind";
		
		private readonly int damage;
		private readonly int reactionDamage;
		private readonly float reactionChance;

		public BeeKind() : base(Settings.ModSettings.RandomEvents.BeeKindData)
		{
			damage = Settings.ModSettings.RandomEvents.BeeKindData.damage;
			reactionDamage = Settings.ModSettings.RandomEvents.BeeKindData.reactionDamage;
			reactionChance = Settings.ModSettings.RandomEvents.BeeKindData.reactionChance;
		}

		public override void CancelEvent()
		{
		}

		public override bool CanExecuteEvent()
		{
			return true;
		}

		public override void StartEvent()
		{
			var extraDialogue = "";
			var damageToInflict = damage;

			if (MBRandom.RandomFloatRanged(0.0f,1.0f) <= reactionChance)
			{
				extraDialogue = "Your body reacts painfully to the sting. ";
				damageToInflict = reactionDamage;
			}

			Hero.MainHero.HitPoints -= damageToInflict;

			InformationManager.ShowInquiry(
				new InquiryData(EventTitle,
					$"As you sit down next to some flowers you get stung by a bee! {extraDialogue}Why is nature so cruel?",
					true,
					false,
					"Ouch.",
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
	

	public class BeeKindData : RandomEventData
	{
		public readonly int damage;
		public readonly int reactionDamage;
		public readonly float reactionChance;

		public BeeKindData(string eventType, float chanceWeight, int damage, int reactionDamage, float reactionChance) : base(eventType, chanceWeight)
		{
			this.damage = damage;
			this.reactionDamage = reactionDamage;
			this.reactionChance = reactionChance;
		}

		public override BaseEvent GetBaseEvent()
		{
			return new BeeKind();
		}
	}
}
