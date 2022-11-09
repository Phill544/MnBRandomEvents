using System;
using System.Windows;
using CryingBuffalo.RandomEvents.Settings;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace CryingBuffalo.RandomEvents.Events
{
	public sealed class BeeKind : BaseEvent
	{
		private readonly int damage;
		private readonly float reactionChance;
		private readonly int reactionDamage;
		

		public BeeKind() : base(ModSettings.RandomEvents.BeeKindData)
		{
			damage = MenuConfig.Instance.BK_damage;
			reactionChance = MenuConfig.Instance.BK_Reaction_Chance;
			reactionDamage = MenuConfig.Instance.BK_Add_Damage;
		}

		public override void CancelEvent()
		{
		}

		public override bool CanExecuteEvent()
		{
			return MenuConfig.Instance.BK_Disable == false;
		}

		public override void StartEvent()
		{
			if (MenuConfig.Instance.GS_DebugMode)
			{
				InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}", RandomEventsSubmodule.Dbg_Color));
			}
			
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

		public BeeKindData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
		{
		}

		public override BaseEvent GetBaseEvent()
		{
			return new BeeKind();
		}
	}
}
