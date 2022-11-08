using System;
using System.Windows;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace CryingBuffalo.RandomEvents.Events
{
	public sealed class BeeKind : BaseEvent
	{
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
			
			var eventTitle = new TextObject("{=BeeKind_Title}Bee Kind").ToString();

			var eventExtraDialogue = new TextObject("{=BeeKind_Event_Extra_Dialogue}Your body reacts painfully to the sting. ").ToString();
			
			if (MBRandom.RandomFloatRanged(0.0f,1.0f) <= reactionChance)
			{
				extraDialogue = eventExtraDialogue;
				damageToInflict = reactionDamage;
			}
			
			var eventText = new TextObject("{=BeeKind_Event_Text}As you sit down next to some flowers you get stung by a bee! {extraDialogue}Why is nature so cruel?")
				.SetTextVariable("extraDialogue", extraDialogue)
				.ToString();

			var eventButtonText = new TextObject("{=BeeKind_Event_Button_Text}Ouch").ToString();

			Hero.MainHero.HitPoints -= damageToInflict;

			InformationManager.ShowInquiry(new InquiryData(eventTitle, eventText, true, false, eventButtonText, null, null, null), true);

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
