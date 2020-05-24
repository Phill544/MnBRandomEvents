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

namespace CryingBuffalo.RandomEvents.Events
{
	public class BeeKind : BaseEvent
	{
		private int damage;
		private int reactionDamage;
		private float reactionChance;

		public BeeKind() : base(Settings.RandomEvents.BeeKindData)
		{
			this.damage = Settings.RandomEvents.BeeKindData.damage;
			this.reactionDamage = Settings.RandomEvents.BeeKindData.reactionDamage;
			this.reactionChance = Settings.RandomEvents.BeeKindData.reactionChance;
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
			string extraDialogue = "";
			int damageToInflict = damage;

			if (MBRandom.RandomFloatRanged(0.0f,1.0f) <= reactionChance)
			{
				extraDialogue = "Votre corps réagit douloureusement à la piqûre. ";
				damageToInflict = reactionDamage;
			}

			Hero.MainHero.HitPoints -= damageToInflict;

			InformationManager.ShowInquiry(
				new InquiryData("Bee Kind",
					$"Lorsque vous vous asseyez à côté de quelques fleurs, vous vous faites piquer par une abeille! {extraDialogue}Pourquoi la nature est-elle si cruelle?",
					true,
					false,
					"Aie.",
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
				MessageBox.Show($"Erreur lors de l'arrêt de \"{this.RandomEventData.EventType}\" event :\n\n {ex.Message} \n\n { ex.StackTrace}");
			}
		}
	}
	

	public class BeeKindData : RandomEventData
	{
		public int damage;
		public int reactionDamage;
		public float reactionChance;

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
