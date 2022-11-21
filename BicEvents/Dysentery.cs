using System;
using System.Windows;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ObjectSystem;
using TaleWorlds.GauntletUI.Data;
using TaleWorlds.Engine;
using TaleWorlds.CampaignSystem;

namespace CryingBuffalo.RandomEvents.Events
{
	public sealed class Dysentery : BaseEvent
	{
		private readonly int minMoraleLoss;
		private readonly int maxMoraleLoss;
		private readonly int minvictim;
        private readonly int maxvictim;

        

		public Dysentery() : base(Settings.ModSettings.RandomEvents.DysenteryData)
		{
			minMoraleLoss = Settings.ModSettings.RandomEvents.DysenteryData.minMoraleLoss;
			maxMoraleLoss = Settings.ModSettings.RandomEvents.DysenteryData.maxMoraleLoss;
			minvictim = Settings.ModSettings.RandomEvents.DysenteryData.minvictim;
			maxvictim = Settings.ModSettings.RandomEvents.DysenteryData.maxvictim;
		}

		public override void CancelEvent()
		{
		}

		public override bool CanExecuteEvent()
		{
			return MobileParty.MainParty.MemberRoster.TotalHealthyCount >= 10;
		}

		public override void StartEvent()
		{

			var partysize = MobileParty.MainParty.MemberRoster.TotalHealthyCount;
			var moraleLoss = MBRandom.RandomInt(minMoraleLoss, maxMoraleLoss);
			var victims = MBRandom.RandomInt(minvictim, maxvictim);
			var totalvictims = partysize / 10 + victims;
			Hero.MainHero.AddSkillXp(DefaultSkills.Medicine, 5);
			MobileParty.MainParty.RecentEventsMorale -= moraleLoss;
			MobileParty.MainParty.MoraleExplained.Add(-moraleLoss);
			MobileParty.MainParty.MemberRoster.WoundNumberOfTroopsRandomly(totalvictims);


			var eventTitle = new TextObject("{=Dysentery_Title}Dysentery").ToString();
			
			var eventOption1 = new TextObject("{=Dysentery_Event_Text}An illness of the stomach has taken hold of your troops.  You inspect the food reserves but there are no signs of rot or spoil," +
				" perhaps it has come from a nearby creek?  Or rather a divine sign of retribution?  Regardless, the vile smell and debilitating condition surely call for worry.")
				.ToString();
				
			var eventButtonText = new TextObject("{=Dysentery_Event_Button_Text}Done").ToString();

			InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOption1, true, false, eventButtonText, null, null, null), true);

            string eventMsg1 = new TextObject(
		"{=Refugees_Event_Msg_1}{totalvictims} troops have dysentery!")
			.SetTextVariable("totalvictims", totalvictims)
			.ToString();

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
		public readonly int minMoraleLoss;
		public readonly int maxMoraleLoss;
		public readonly int minvictim;
		public readonly int maxvictim;


		public DysenteryData(string eventType, float chanceWeight, int minMoraleLoss, int maxMoraleLoss, int minvictim, int maxvictim) : base(eventType, chanceWeight)
		{
			this.minMoraleLoss = minMoraleLoss;
			this.maxMoraleLoss = maxMoraleLoss;
			this.minvictim = minvictim;
			this.maxvictim = maxvictim;

	}

		public override BaseEvent GetBaseEvent()
		{
			return new Dysentery();
		}
	}
}
