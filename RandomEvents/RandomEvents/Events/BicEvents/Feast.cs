using System;
using System.Windows;
using CryingBuffalo.RandomEvents.Settings.MCM;
using CryingBuffalo.RandomEvents.Helpers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.CampaignSystem.Settlements;
using System.Linq;

namespace CryingBuffalo.RandomEvents.Events.BicEvents
{
	public sealed class Feast : BaseEvent
	{


        

		public Feast() : base(Settings.ModSettings.RandomEvents.BirdSongsData)
		{

		}

		public override void CancelEvent()
		{
        
		}

		public override bool CanExecuteEvent()
		{
			if (Settlement.CurrentSettlement == null)
				return false;

			//var notables = Settlement.CurrentSettlement.HeroesWithoutParty.ToList();
			//var Lords = notables.Where(character => character.IsCommander).ToList();
			// && Lords.Count != 0
			//MCM_MenuConfig_A_F.Instance.FE_Disable ==

			return  false;
		}

		public override void StartEvent()
		{
			if (MCM_ConfigMenu_General.Instance.GS_DebugMode)
			{
				InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}", RandomEventsSubmodule.Dbg_Color));
			}

			var currentSettlement = Settlement.CurrentSettlement.Name.ToString();

			var roguerySkill = Hero.MainHero.GetSkillValue(DefaultSkills.Roguery);
			var charmSkill = Hero.MainHero.GetSkillValue(DefaultSkills.Charm);

			var theftSuccess = false;
			var charmedNoble = false;

			if (roguerySkill >= 100)
			{
				theftSuccess = true;
			}
			else if (charmSkill >= 120)
			{
				charmedNoble = true;
            }


            //var notables = Settlement.CurrentSettlement.HeroesWithoutParty.ToList();
            //var Lords = notables.Where(character => character.IsLordParty).ToList();
            var notables = Settlement.CurrentSettlement.HeroesWithoutParty.ToList();
			var Lords = notables.Where(character => character.IsCommander).ToList();

			var nobleparty = Settlement.CurrentSettlement.Parties.ToList();
            var lordparty = nobleparty.Where(character => character.IsLordParty).ToList();

			//var characters = Lords.Concat(lordparty).Distinct().ToList();

			var random = new Random();
			var index = random.Next(Lords.Count);

			var targetLord = Lords[index];

			var targetLordIsFemale = targetLord.IsFemale;
			var targetLordGender = targetLordIsFemale ? "female" : "male";
			var targetLordGenderAdjective = GenderAssignment.GetTheGenderAssignment(targetLordGender, false, "adjective");
			var targetLordGenderSubjective = GenderAssignment.GetTheGenderAssignment(targetLordGender, false, "subjective");
			var targetLordGenderObjective = GenderAssignment.GetTheGenderAssignment(targetLordGender, false, "objective");
			var targetLordGenderTitle = GenderAssignment.GetTheGenderAssignment(targetLordGender, true, "title");

			var eventTitle = new TextObject("{=Feast_Title}Feast Invitation").ToString();
			
			var eventOption1 = new TextObject("{=Feast_Event_Description}While in {settlement} you are approached by an envoy from {title} {targetLord} requesting your presence for a feast.")
				.SetTextVariable("targetLord", targetLord?.Name)
				.SetTextVariable("settlement", currentSettlement)
				.SetTextVariable("gender", targetLordGenderObjective)
				.SetTextVariable("title", targetLordGenderTitle)
				.ToString();
				
			var eventButtonText = new TextObject("{=Feast_Event_Button_Text}Done").ToString();

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

	public class FeastData : RandomEventData
	{

		public FeastData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
		{

		}

		public override BaseEvent GetBaseEvent()
		{
			return new Feast();
		}
	}
}
