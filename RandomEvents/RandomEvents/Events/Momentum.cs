using System;
using System.Windows;
using CryingBuffalo.RandomEvents.Settings;
using CryingBuffalo.RandomEvents.Settings.MCM;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace CryingBuffalo.RandomEvents.Events
{
	public sealed class Momentum : BaseEvent
	{
		public Momentum() : base(ModSettings.RandomEvents.MomentumData)
		{
		}

		public override void CancelEvent()
		{
		}

		public override bool CanExecuteEvent()
		{
			return MCM_MenuConfig_A_M.Instance.MO_Disable == false && MobileParty.MainParty.CurrentSettlement == null;
		}

		public override void StartEvent()
		{
			if (MCM_ConfigMenu_General.Instance.GS_DebugMode)
			{
				InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}", RandomEventsSubmodule.Dbg_Color));
			}
			
			bool isOnFoot = Hero.MainHero.CharacterObject.Equipment.Horse.IsEmpty;

			string dialogue;
			
			var eventTitle = new TextObject("{=Momentum_Title}Momentum").ToString();

			var eventDialogue1 = new TextObject("{=Momentum_OnFoot}on foot").ToString();
			
			var eventDialogue2 = new TextObject("{=Momentum_Riding}riding").ToString();

			var eventButtonText = new TextObject("{=Momentum_Event_Button_Text}Done").ToString();

			if (isOnFoot)
			{
				float xpToGive = MCM_ConfigMenu_General.Instance.GS_GeneralLevelXpMultiplier * Hero.MainHero.GetSkillValue(DefaultSkills.Athletics);
				Hero.MainHero.AddSkillXp(DefaultSkills.Athletics, xpToGive);
				dialogue = eventDialogue1;
			}
			else
			{
				float xpToGive = MCM_ConfigMenu_General.Instance.GS_GeneralLevelXpMultiplier * Hero.MainHero.GetSkillValue(DefaultSkills.Riding);
				Hero.MainHero.AddSkillXp(DefaultSkills.Riding, xpToGive);
				dialogue = eventDialogue2;
			}
			
			var eventOption1 = new TextObject("{=Momentum_Event_Text}After spending so much time {dialogue} you gain a second wind!")
				.SetTextVariable("dialogue", dialogue)
				.ToString();

			InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOption1, true, false, eventButtonText, null, null, null), true);

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
