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

namespace Bannerlord.RandomEvents.Events.BicEvents
{
	public sealed class Dysentery : BaseEvent
	{
		private readonly bool eventDisabled;
		private readonly int minMoraleLoss;
		private readonly int maxMoraleLoss;
		private readonly int minVictims;
        private readonly int maxVictims;

        

		public Dysentery() : base(ModSettings.RandomEvents.DysenteryData)
		{
			var ConfigFile = new IniFile(ParseIniFile.GetTheConfigFile());
			
			eventDisabled = ConfigFile.ReadBoolean("Dysentery", "EventDisabled");
			minMoraleLoss = ConfigFile.ReadInteger("Dysentery", "MinMoraleLoss");
			maxMoraleLoss = ConfigFile.ReadInteger("Dysentery", "MaxMoraleLoss");
			minVictims = ConfigFile.ReadInteger("Dysentery", "MinVictims");
			maxVictims = ConfigFile.ReadInteger("Dysentery", "MaxVictims");
		}

		public override void CancelEvent()
		{
		}
		
		private bool HasValidEventData()
		{
			if (eventDisabled == false)
			{
				if (minMoraleLoss != 0 || maxMoraleLoss != 0 || minVictims != 0 || maxVictims != 0)
				{
					return true;
				}
			}
			return false;
		}

		public override bool CanExecuteEvent()
		{
			return HasValidEventData() &&  MobileParty.MainParty.MemberRoster.TotalHealthyCount >= 10 && MobileParty.MainParty.CurrentSettlement == null;
		}

		public override void StartEvent()
		{ 
			var partySize = MobileParty.MainParty.MemberRoster.TotalHealthyCount;
			
			var moraleLoss = MBRandom.RandomInt(minMoraleLoss, maxMoraleLoss);
			
			var victims = MBRandom.RandomInt(minVictims, maxVictims);
			
			var totalvictims = partySize / 10 + victims;
			
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

            var eventMsg1 = new TextObject("{=Dysentery_Event_Msg_1}{totalvictims} troops have dysentery!")
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
