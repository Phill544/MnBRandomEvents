using System;
using System.Windows;
using CryingBuffalo.RandomEvents.Settings.MCM;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace CryingBuffalo.RandomEvents.Events.BicEvents
{
	public sealed class Dysentery : BaseEvent
	{
		private readonly int minMoraleLoss;
		private readonly int maxMoraleLoss;
		private readonly int minvictim;
        private readonly int maxvictim;

        

		public Dysentery() : base(Settings.ModSettings.RandomEvents.DysenteryData)
		{

			minMoraleLoss = MCM_MenuConfig_A_M.Instance.DY_minMoraleLoss;
			maxMoraleLoss = MCM_MenuConfig_A_M.Instance.DY_maxMoraleLoss;
			minvictim = MCM_MenuConfig_A_M.Instance.DY_minvictim;
			maxvictim = MCM_MenuConfig_A_M.Instance.DY_maxvictim;

		}

		public override void CancelEvent()
		{
		}

		public override bool CanExecuteEvent()
		{
			return MCM_MenuConfig_A_M.Instance.DY_Disable == false && MobileParty.MainParty.MemberRoster.TotalHealthyCount >= 10 && MobileParty.MainParty.CurrentSettlement == null;
		}

		public override void StartEvent()
		{
			if (MCM_ConfigMenu_General.Instance.GS_DebugMode)
			{
				InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}", RandomEventsSubmodule.Dbg_Color));
			}

			var partysize = MobileParty.MainParty.MemberRoster.TotalHealthyCount;
			var moraleLoss = MBRandom.RandomInt(minMoraleLoss, maxMoraleLoss);
			var victims = MBRandom.RandomInt(minvictim, maxvictim);
			var totalvictims = partysize / 10 + victims;
			Hero.MainHero.AddSkillXp(DefaultSkills.Medicine, 5);
			MobileParty.MainParty.RecentEventsMorale -= moraleLoss;
			MobileParty.MainParty.MoraleExplained.Add(-moraleLoss);
			MobileParty.MainParty.MemberRoster.WoundNumberOfTroopsRandomly(totalvictims);


			string eventTitle = new TextObject("{=Dysentery_Title}Dysentery").ToString();
			
			string eventOption1 = new TextObject("{=Dysentery_Event_Text}An illness of the stomach has taken hold of your troops. You inspect the food reserves but there are no signs of rot or spoil," +
				" perhaps it has come from a nearby creek? Or rather a divine sign of retribution? Regardless, the vile smell and debilitating condition surely call for worry.")
				.ToString();
				
			string eventButtonText = new TextObject("{=Dysentery_Event_Button_Text}Done").ToString();

			InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOption1, true, false, eventButtonText, null, null, null), true);

            string eventMsg1 = new TextObject("{=Dysentery_Event_Msg_1}{totalvictims} troops have dysentery!")
			.SetTextVariable("totalvictims", totalvictims)
			.ToString();

            InformationManager.DisplayMessage(new InformationMessage(eventMsg1, RandomEventsSubmodule.Msg_Color));
            
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

	public class DysenteryData : RandomEventData
	{

		public DysenteryData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
		{


	}

		public override BaseEvent GetBaseEvent()
		{
			return new Dysentery();
		}
	}
}
