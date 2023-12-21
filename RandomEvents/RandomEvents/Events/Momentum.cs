using System;
using System.Windows;
using Bannerlord.RandomEvents.Helpers;
using Bannerlord.RandomEvents.Settings;
using Ini.Net;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace Bannerlord.RandomEvents.Events
{
	public sealed class Momentum : BaseEvent
	{
		private readonly bool eventDisabled;
		
		public Momentum() : base(ModSettings.RandomEvents.MomentumData)
		{
			var ConfigFile = new IniFile(ParseIniFile.GetTheConfigFile());
            
			eventDisabled = ConfigFile.ReadBoolean("Momentum", "EventDisabled");
		}

		public override void CancelEvent()
		{
		}
		
		private bool HasValidEventData()
		{
			return eventDisabled == false;
		}

		public override bool CanExecuteEvent()
		{
			return HasValidEventData() && MobileParty.MainParty.CurrentSettlement == null;
		}

		public override void StartEvent()
		{
			bool isOnFoot = Hero.MainHero.CharacterObject.Equipment.Horse.IsEmpty;

			string dialogue;
			
			var eventTitle = new TextObject("{=Momentum_Title}Momentum").ToString();

			var eventDialogue1 = new TextObject("{=Momentum_OnFoot}on foot").ToString();
			
			var eventDialogue2 = new TextObject("{=Momentum_Riding}riding").ToString();

			var eventButtonText = new TextObject("{=Momentum_Event_Button_Text}Done").ToString();

			if (isOnFoot)
			{
				float xpToGive = GeneralSettings.Basic.GetLevelXpMultiplier() * Hero.MainHero.GetSkillValue(DefaultSkills.Athletics);
				Hero.MainHero.AddSkillXp(DefaultSkills.Athletics, xpToGive);
				dialogue = eventDialogue1;
			}
			else
			{
				float xpToGive = GeneralSettings.Basic.GetLevelXpMultiplier() * Hero.MainHero.GetSkillValue(DefaultSkills.Riding);
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
