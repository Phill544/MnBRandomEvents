using System;
using System.Linq;
using System.Windows;
using CryingBuffalo.RandomEvents.Helpers;
using CryingBuffalo.RandomEvents.Settings.MCM;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace CryingBuffalo.RandomEvents.Events.BicEvents
{
	public sealed class CompanionAdmire : BaseEvent
	{

		//private readonly int low = 1;
		//private readonly int high = 4;



		public CompanionAdmire() : base(Settings.ModSettings.RandomEvents.CompanionAdmireData)
        {


		}

		public override void CancelEvent()
		{
        
		}

		public override bool CanExecuteEvent()
		{
			return MCM_MenuConfig_A_M.Instance.CA_Disable == false && MobileParty.MainParty.CurrentSettlement == null && MobileParty.MainParty.EffectiveScout != Hero.MainHero
				&& MobileParty.MainParty.IsDisorganized == false && Clan.PlayerClan.Renown >= 500 ||
				   MCM_MenuConfig_A_M.Instance.CA_Disable == false && MobileParty.MainParty.CurrentSettlement == null && MobileParty.MainParty.EffectiveQuartermaster != Hero.MainHero
				&& MobileParty.MainParty.IsDisorganized == false && Clan.PlayerClan.Renown >= 500 ||
				   MCM_MenuConfig_A_M.Instance.CA_Disable == false && MobileParty.MainParty.CurrentSettlement == null && MobileParty.MainParty.EffectiveSurgeon != Hero.MainHero
				&& MobileParty.MainParty.IsDisorganized == false && Clan.PlayerClan.Renown >= 500 ||
				   MCM_MenuConfig_A_M.Instance.CA_Disable == false && MobileParty.MainParty.CurrentSettlement == null && MobileParty.MainParty.EffectiveEngineer != Hero.MainHero
				&& MobileParty.MainParty.IsDisorganized == false && Clan.PlayerClan.Renown >= 500;
		}

		public override void StartEvent()
		{
			if (MCM_ConfigMenu_General.Instance.GS_DebugMode)
			{
				InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}", RandomEventsSubmodule.Dbg_Color));
			}

			int dice = MBRandom.RandomInt(1, 4);



			//Profession----------
			var scout = MobileParty.MainParty.EffectiveScout;
			var quartermaster = MobileParty.MainParty.EffectiveQuartermaster;
			var engineer = MobileParty.MainParty.EffectiveEngineer;
			var surgeon = MobileParty.MainParty.EffectiveSurgeon;
			//Profession Name ----
			var scoutName = MobileParty.MainParty.EffectiveScout.Name;
			var quartermasterName = MobileParty.MainParty.EffectiveQuartermaster.Name;
			var engineerName = MobileParty.MainParty.EffectiveEngineer.Name;
			var surgeonName = MobileParty.MainParty.EffectiveSurgeon.Name;
			//--------------------

			var clanname = Clan.PlayerClan.Name;
			var closestSettlement = ClosestSettlements.GetClosestAny(MobileParty.MainParty).ToString();
			Hero.MainHero.AddSkillXp(DefaultSkills.Leadership, 50);
			Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 30);

			string eventButtonText = new TextObject("{=CompanionAdmire_Event_Button_Text}Done").ToString();

			//Event Messages___---___---=
			string eventMsg1 = new TextObject("{=CompanionAdmire_Event_Msg_1}Your relationship with {scout) has improved.")
			.SetTextVariable("scout", scoutName)
			.ToString();
			string eventMsg2 = new TextObject("{=CompanionAdmire_Event_Msg_2}Your relationship with {quartermaster) has improved.")
			.SetTextVariable("quartermaster", quartermasterName)
			.ToString();
			string eventMsg3 = new TextObject("{=CompanionAdmire_Event_Msg_3}Your relationship with {surgeon) has improved.")
			.SetTextVariable("surgeon", surgeonName)
			.ToString();
			string eventMsg4 = new TextObject("{=CompanionAdmire_Event_Msg_4}Your relationship with {engineer) has improved.")
			.SetTextVariable("engineer", engineerName)
			.ToString();



			//----------------------------------------------- 1 ------------------------------
			if (dice == 1)
			{
				#region Scout
				if (MobileParty.MainParty.EffectiveScout != Hero.MainHero)
				{

					var CompanionIsFemale = scout.IsFemale;
					var CompanionGender = CompanionIsFemale ? "female" : "male";
					var gender = GenderAssignment.GetTheGenderAssignment(CompanionGender, false, "subjective");

					var scoutrelation = Hero.MainHero.GetRelation(MobileParty.MainParty.EffectiveScout);
					Hero.MainHero.SetPersonalRelation(MobileParty.MainParty.EffectiveScout, scoutrelation + 15);

					string eventTitle = new TextObject("{=CompanionAdmire_Title}Companion Admiration").ToString();

					string eventOption1 = new TextObject("{=CompanionAdmire_Event_Text}While travelling near {closestsettlement} your scout, {scout}, approaches you for a chat. Casual conversation soon turns to a more serious matter in regards" +
						" to the party and its future. \n\n{scout} wants you to know {gender} admires your leadership and looks forward to celebrating the many future victories along your side. Lastly noting {gender} is determined to bring honor" +
						" to the {clan} clan in any way {gender} can.")
						.SetTextVariable("closestsettlement", closestSettlement)
						.SetTextVariable("clan", clanname)
						.SetTextVariable("scout", scoutName)
						.SetTextVariable("gender", gender)
						.ToString();

					InformationManager.DisplayMessage(new InformationMessage(eventMsg1, RandomEventsSubmodule.Msg_Color));

					InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOption1, true, false, eventButtonText, null, null, null), true);
					StopEvent();
					
				}
				#endregion
				//--------------------------
				#region Quartermaster
				else if (MobileParty.MainParty.EffectiveQuartermaster != Hero.MainHero)
				{

					var CompanionIsFemale = quartermaster.IsFemale;
					var CompanionGender = CompanionIsFemale ? "female" : "male";
					var gender = GenderAssignment.GetTheGenderAssignment(CompanionGender, false, "subjective");

					var relation = Hero.MainHero.GetRelation(MobileParty.MainParty.EffectiveQuartermaster);
					Hero.MainHero.SetPersonalRelation(MobileParty.MainParty.EffectiveQuartermaster, relation + 15);

					string eventTitle2 = new TextObject("{=CompanionAdmire_Title}Companion Admiration").ToString();

					string eventOption2 = new TextObject("{=CompanionAdmire_Event_Text}While travelling near {closestsettlement} your quartermaster, {quartermaster}, approaches you for a chat. Casual conversation soon turns to a more serious matter in regards" +
						" to the party and its future. \n\n{quartermaster} wants you to know {gender} admires your leadership and looks forward to celebrating the many future victories along your side. Lastly noting {gender} is determined to bring honor" +
						" to the {clan} clan in any way {gender} can.")
						.SetTextVariable("closestsettlement", closestSettlement)
						.SetTextVariable("clan", clanname)
						.SetTextVariable("quartermaster", quartermasterName)
						.SetTextVariable("gender", gender)
						.ToString();

					InformationManager.DisplayMessage(new InformationMessage(eventMsg2, RandomEventsSubmodule.Msg_Color));
					InformationManager.ShowInquiry(new InquiryData(eventTitle2, eventOption2, true, false, eventButtonText, null, null, null), true);
					StopEvent();


				}
				#endregion
				//--------------------------
				#region Surgeon
				else if (MobileParty.MainParty.EffectiveSurgeon != Hero.MainHero)
				{
					var CompanionIsFemale = surgeon.IsFemale;
					var CompanionGender = CompanionIsFemale ? "female" : "male";
					var gender = GenderAssignment.GetTheGenderAssignment(CompanionGender, false, "subjective");

					var relation = Hero.MainHero.GetRelation(MobileParty.MainParty.EffectiveSurgeon);
					Hero.MainHero.SetPersonalRelation(MobileParty.MainParty.EffectiveSurgeon, relation + 15);

					string eventTitle3 = new TextObject("{=CompanionAdmire_Title}Companion Admiration").ToString();

					string eventOption3 = new TextObject("{=CompanionAdmire_Event_Text}While travelling near {closestsettlement} your surgeon, {surgeon}, approaches you for a chat. Casual conversation soon turns to a more serious matter in regards" +
						" to the party and its future. \n\n{surgeon} wants you to know {gender} admires your leadership and looks forward to celebrating the many future victories along your side. Lastly noting {gender} is determined to bring honor" +
						" to the {clan} clan in any way {gender} can.")
						.SetTextVariable("closestsettlement", closestSettlement)
						.SetTextVariable("clan", clanname)
						.SetTextVariable("surgeon", surgeonName)
						.SetTextVariable("gender", gender)
						.ToString();

					InformationManager.DisplayMessage(new InformationMessage(eventMsg3, RandomEventsSubmodule.Msg_Color));
					InformationManager.ShowInquiry(new InquiryData(eventTitle3, eventOption3, true, false, eventButtonText, null, null, null), true);
					StopEvent();


				}
				#endregion
				//-------------------------
				#region Engineer
				else if (MobileParty.MainParty.EffectiveEngineer != Hero.MainHero)
				{
					var CompanionIsFemale = engineer.IsFemale;
					var CompanionGender = CompanionIsFemale ? "female" : "male";
					var gender = GenderAssignment.GetTheGenderAssignment(CompanionGender, false, "subjective");

					var relation = Hero.MainHero.GetRelation(MobileParty.MainParty.EffectiveEngineer);
					Hero.MainHero.SetPersonalRelation(MobileParty.MainParty.EffectiveEngineer, relation + 15);

					string eventTitle4 = new TextObject("{=CompanionAdmire_Title}Companion Admiration").ToString();

					string eventOption4 = new TextObject("{=CompanionAdmire_Event_Text}While travelling near {closestsettlement} your engineer, {engineer}, approaches you for a chat. Casual conversation soon turns to a more serious matter in regards" +
						" to the party and its future. \n\n{engineer} wants you to know {gender} admires your leadership and looks forward to celebrating the many future victories along your side. Lastly noting {gender} is determined to bring honor" +
						" to the {clan} clan in any way {gender} can.")
						.SetTextVariable("closestsettlement", closestSettlement)
						.SetTextVariable("clan", clanname)
						.SetTextVariable("engineer", engineerName)
						.SetTextVariable("gender", gender)
						.ToString();

					InformationManager.DisplayMessage(new InformationMessage(eventMsg4, RandomEventsSubmodule.Msg_Color));
					InformationManager.ShowInquiry(new InquiryData(eventTitle4, eventOption4, true, false, eventButtonText, null, null, null), true);
					StopEvent();
				}
				#endregion

			}
            //----------------------------------------------- 2 ------------------------------
            else if (dice == 2)
            {
				#region Quartermaster
				if (MobileParty.MainParty.EffectiveQuartermaster != Hero.MainHero)
				{
					var CompanionIsFemale = quartermaster.IsFemale;
					var CompanionGender = CompanionIsFemale ? "female" : "male";
					var gender = GenderAssignment.GetTheGenderAssignment(CompanionGender, false, "subjective");

					var relation = Hero.MainHero.GetRelation(MobileParty.MainParty.EffectiveQuartermaster);
					Hero.MainHero.SetPersonalRelation(MobileParty.MainParty.EffectiveQuartermaster, relation + 15);

					string eventTitle2 = new TextObject("{=CompanionAdmire_Title}Companion Admiration").ToString();

					string eventOption2 = new TextObject("{=CompanionAdmire_Event_Text}While travelling near {closestsettlement} your quartermaster, {quartermaster}, approaches you for a chat. Casual conversation soon turns to a more serious matter in regards" +
						" to the party and its future. \n\n{quartermaster} wants you to know {gender} admires your leadership and looks forward to celebrating the many future victories along your side. Lastly noting {gender} is determined to bring honor" +
						" to the {clan} clan in any way {gender} can.")
						.SetTextVariable("closestsettlement", closestSettlement)
						.SetTextVariable("clan", clanname)
						.SetTextVariable("quartermaster", quartermasterName)
						.SetTextVariable("gender", gender)
						.ToString();

					InformationManager.DisplayMessage(new InformationMessage(eventMsg2, RandomEventsSubmodule.Msg_Color));
					InformationManager.ShowInquiry(new InquiryData(eventTitle2, eventOption2, true, false, eventButtonText, null, null, null), true);
					StopEvent();


				}
				#endregion

				#region Surgeon
				else if (MobileParty.MainParty.EffectiveSurgeon != Hero.MainHero)
				{
					var CompanionIsFemale = surgeon.IsFemale;
					var CompanionGender = CompanionIsFemale ? "female" : "male";
					var gender = GenderAssignment.GetTheGenderAssignment(CompanionGender, false, "subjective");

					var relation = Hero.MainHero.GetRelation(MobileParty.MainParty.EffectiveSurgeon);
					Hero.MainHero.SetPersonalRelation(MobileParty.MainParty.EffectiveSurgeon, relation + 15);

					string eventTitle3 = new TextObject("{=CompanionAdmire_Title}Companion Admiration").ToString();

					string eventOption3 = new TextObject("{=CompanionAdmire_Event_Text}While travelling near {closestsettlement} your surgeon, {surgeon}, approaches you for a chat. Casual conversation soon turns to a more serious matter in regards" +
						" to the party and its future. \n\n{surgeon} wants you to know {gender} admires your leadership and looks forward to celebrating the many future victories along your side. Lastly noting {gender} is determined to bring honor" +
						" to the {clan} clan in any way {gender} can.")
						.SetTextVariable("closestsettlement", closestSettlement)
						.SetTextVariable("clan", clanname)
						.SetTextVariable("surgeon", surgeonName)
						.SetTextVariable("gender", gender)
						.ToString();

					InformationManager.DisplayMessage(new InformationMessage(eventMsg3, RandomEventsSubmodule.Msg_Color));
					InformationManager.ShowInquiry(new InquiryData(eventTitle3, eventOption3, true, false, eventButtonText, null, null, null), true);
					StopEvent();
				}
				#endregion

				#region Engineer
				else if (MobileParty.MainParty.EffectiveEngineer != Hero.MainHero)
				{
					var CompanionIsFemale = engineer.IsFemale;
					var CompanionGender = CompanionIsFemale ? "female" : "male";
					var gender = GenderAssignment.GetTheGenderAssignment(CompanionGender, false, "subjective");

					var relation = Hero.MainHero.GetRelation(MobileParty.MainParty.EffectiveEngineer);
					Hero.MainHero.SetPersonalRelation(MobileParty.MainParty.EffectiveEngineer, relation + 15);

					string eventTitle4 = new TextObject("{=CompanionAdmire_Title}Companion Admiration").ToString();

					string eventOption4 = new TextObject("{=CompanionAdmire_Event_Text}While travelling near {closestsettlement} your engineer, {engineer}, approaches you for a chat. Casual conversation soon turns to a more serious matter in regards" +
						" to the party and its future. \n\n{engineer} wants you to know {gender} admires your leadership and looks forward to celebrating the many future victories along your side. Lastly noting {gender} is determined to bring honor" +
						" to the {clan} clan in any way {gender} can.")
						.SetTextVariable("closestsettlement", closestSettlement)
						.SetTextVariable("clan", clanname)
						.SetTextVariable("engineer", engineerName)
						.SetTextVariable("gender", gender)
						.ToString();

					InformationManager.DisplayMessage(new InformationMessage(eventMsg4, RandomEventsSubmodule.Msg_Color));
					InformationManager.ShowInquiry(new InquiryData(eventTitle4, eventOption4, true, false, eventButtonText, null, null, null), true);
					StopEvent();

				}
				#endregion

				#region Scout
				else if (MobileParty.MainParty.EffectiveScout != Hero.MainHero)
				{

					var CompanionIsFemale = scout.IsFemale;
					var CompanionGender = CompanionIsFemale ? "female" : "male";
					var gender = GenderAssignment.GetTheGenderAssignment(CompanionGender, false, "subjective");

					var scoutrelation = Hero.MainHero.GetRelation(MobileParty.MainParty.EffectiveScout);
					Hero.MainHero.SetPersonalRelation(MobileParty.MainParty.EffectiveScout, scoutrelation + 15);

					string eventTitle = new TextObject("{=CompanionAdmire_Title}Companion Admiration").ToString();

					string eventOption1 = new TextObject("{=CompanionAdmire_Event_Text}While travelling near {closestsettlement} your scout, {scout}, approaches you for a chat. Casual conversation soon turns to a more serious matter in regards" +
						" to the party and its future. \n\n{scout} wants you to know {gender} admires your leadership and looks forward to celebrating the many future victories along your side. Lastly noting {gender} is determined to bring honor" +
						" to the {clan} clan in any way {gender} can.")
						.SetTextVariable("closestsettlement", closestSettlement)
						.SetTextVariable("clan", clanname)
						.SetTextVariable("scout", scoutName)
						.SetTextVariable("gender", gender)
						.ToString();

					InformationManager.DisplayMessage(new InformationMessage(eventMsg1, RandomEventsSubmodule.Msg_Color));

					InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOption1, true, false, eventButtonText, null, null, null), true);
					StopEvent();

				}
				#endregion

			}
			//----------------------------------------------- 3 ------------------------------
			else if (dice == 3)
            {
				#region Surgeon
				if (MobileParty.MainParty.EffectiveSurgeon != Hero.MainHero)
				{
					var CompanionIsFemale = surgeon.IsFemale;
					var CompanionGender = CompanionIsFemale ? "female" : "male";
					var gender = GenderAssignment.GetTheGenderAssignment(CompanionGender, false, "subjective");

					var relation = Hero.MainHero.GetRelation(MobileParty.MainParty.EffectiveSurgeon);
					Hero.MainHero.SetPersonalRelation(MobileParty.MainParty.EffectiveSurgeon, relation + 15);

					string eventTitle3 = new TextObject("{=CompanionAdmire_Title}Companion Admiration").ToString();

					string eventOption3 = new TextObject("{=CompanionAdmire_Event_Text}While travelling near {closestsettlement} your surgeon, {surgeon}, approaches you for a chat. Casual conversation soon turns to a more serious matter in regards" +
						" to the party and its future. \n\n{surgeon} wants you to know {gender} admires your leadership and looks forward to celebrating the many future victories along your side. Lastly noting {gender} is determined to bring honor" +
						" to the {clan} clan in any way {gender} can.")
						.SetTextVariable("closestsettlement", closestSettlement)
						.SetTextVariable("clan", clanname)
						.SetTextVariable("surgeon", surgeonName)
						.SetTextVariable("gender", gender)
						.ToString();

					InformationManager.DisplayMessage(new InformationMessage(eventMsg3, RandomEventsSubmodule.Msg_Color));
					InformationManager.ShowInquiry(new InquiryData(eventTitle3, eventOption3, true, false, eventButtonText, null, null, null), true);
					StopEvent();

				}
				#endregion

				#region Engineer
				else if (MobileParty.MainParty.EffectiveEngineer != Hero.MainHero)
				{
					var CompanionIsFemale = engineer.IsFemale;
					var CompanionGender = CompanionIsFemale ? "female" : "male";
					var gender = GenderAssignment.GetTheGenderAssignment(CompanionGender, false, "subjective");

					var relation = Hero.MainHero.GetRelation(MobileParty.MainParty.EffectiveEngineer);
					Hero.MainHero.SetPersonalRelation(MobileParty.MainParty.EffectiveEngineer, relation + 15);

					string eventTitle4 = new TextObject("{=CompanionAdmire_Title}Companion Admiration").ToString();

					string eventOption4 = new TextObject("{=CompanionAdmire_Event_Text}While travelling near {closestsettlement} your engineer, {engineer}, approaches you for a chat. Casual conversation soon turns to a more serious matter in regards" +
						" to the party and its future. \n\n{engineer} wants you to know {gender} admires your leadership and looks forward to celebrating the many future victories along your side. Lastly noting {gender} is determined to bring honor" +
						" to the {clan} clan in any way {gender} can.")
						.SetTextVariable("closestsettlement", closestSettlement)
						.SetTextVariable("clan", clanname)
						.SetTextVariable("engineer", engineerName)
						.SetTextVariable("gender", gender)
						.ToString();

					InformationManager.DisplayMessage(new InformationMessage(eventMsg4, RandomEventsSubmodule.Msg_Color));
					InformationManager.ShowInquiry(new InquiryData(eventTitle4, eventOption4, true, false, eventButtonText, null, null, null), true);
					StopEvent();
				}
				#endregion

				#region Scout
				else if (MobileParty.MainParty.EffectiveScout != Hero.MainHero)
				{
					var CompanionIsFemale = scout.IsFemale;
					var CompanionGender = CompanionIsFemale ? "female" : "male";
					var gender = GenderAssignment.GetTheGenderAssignment(CompanionGender, false, "subjective");

					var scoutrelation = Hero.MainHero.GetRelation(MobileParty.MainParty.EffectiveScout);
					Hero.MainHero.SetPersonalRelation(MobileParty.MainParty.EffectiveScout, scoutrelation + 15);

					string eventTitle = new TextObject("{=CompanionAdmire_Title}Companion Admiration").ToString();

					string eventOption1 = new TextObject("{=CompanionAdmire_Event_Text}While travelling near {closestsettlement} your scout, {scout}, approaches you for a chat. Casual conversation soon turns to a more serious matter in regards" +
						" to the party and its future. \n\n{scout} wants you to know {gender} admires your leadership and looks forward to celebrating the many future victories along your side. Lastly noting {gender} is determined to bring honor" +
						" to the {clan} clan in any way {gender} can.")
						.SetTextVariable("closestsettlement", closestSettlement)
						.SetTextVariable("clan", clanname)
						.SetTextVariable("scout", scoutName)
						.SetTextVariable("gender", gender)
						.ToString();

					InformationManager.DisplayMessage(new InformationMessage(eventMsg1, RandomEventsSubmodule.Msg_Color));

					InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOption1, true, false, eventButtonText, null, null, null), true);
					StopEvent();
				}
				#endregion

				#region Quartermaster
				else if (MobileParty.MainParty.EffectiveQuartermaster != Hero.MainHero)
				{
					var CompanionIsFemale = quartermaster.IsFemale;
					var CompanionGender = CompanionIsFemale ? "female" : "male";
					var gender = GenderAssignment.GetTheGenderAssignment(CompanionGender, false, "subjective");

					var relation = Hero.MainHero.GetRelation(MobileParty.MainParty.EffectiveQuartermaster);
					Hero.MainHero.SetPersonalRelation(MobileParty.MainParty.EffectiveQuartermaster, relation + 15);

					string eventTitle2 = new TextObject("{=CompanionAdmire_Title}Companion Admiration").ToString();

					string eventOption2 = new TextObject("{=CompanionAdmire_Event_Text}While travelling near {closestsettlement} your quartermaster, {quartermaster}, approaches you for a chat. Casual conversation soon turns to a more serious matter in regards" +
						" to the party and its future. \n\n{quartermaster} wants you to know {gender} admires your leadership and looks forward to celebrating the many future victories along your side. Lastly noting {gender} is determined to bring honor" +
						" to the {clan} clan in any way {gender} can.")
						.SetTextVariable("closestsettlement", closestSettlement)
						.SetTextVariable("clan", clanname)
						.SetTextVariable("quartermaster", quartermasterName)
						.SetTextVariable("gender", gender)
						.ToString();

					InformationManager.DisplayMessage(new InformationMessage(eventMsg2, RandomEventsSubmodule.Msg_Color));
					InformationManager.ShowInquiry(new InquiryData(eventTitle2, eventOption2, true, false, eventButtonText, null, null, null), true);
					StopEvent();

				}
				#endregion

			}
			//----------------------------------------------- 4 ------------------------------
			else if (dice == 4)
            {
				#region Engineer
				if (MobileParty.MainParty.EffectiveEngineer != Hero.MainHero)
				{
					var CompanionIsFemale = engineer.IsFemale;
					var CompanionGender = CompanionIsFemale ? "female" : "male";
					var gender = GenderAssignment.GetTheGenderAssignment(CompanionGender, false, "subjective");

					var relation = Hero.MainHero.GetRelation(MobileParty.MainParty.EffectiveEngineer);
					Hero.MainHero.SetPersonalRelation(MobileParty.MainParty.EffectiveEngineer, relation + 15);

					string eventTitle4 = new TextObject("{=CompanionAdmire_Title}Companion Admiration").ToString();

					string eventOption4 = new TextObject("{=CompanionAdmire_Event_Text}While travelling near {closestsettlement} your engineer, {engineer}, approaches you for a chat. Casual conversation soon turns to a more serious matter in regards" +
						" to the party and its future. \n\n{engineer} wants you to know {gender} admires your leadership and looks forward to celebrating the many future victories along your side. Lastly noting {gender} is determined to bring honor" +
						" to the {clan} clan in any way {gender} can.")
						.SetTextVariable("closestsettlement", closestSettlement)
						.SetTextVariable("clan", clanname)
						.SetTextVariable("engineer", engineerName)
						.SetTextVariable("gender", gender)
						.ToString();

					InformationManager.DisplayMessage(new InformationMessage(eventMsg4, RandomEventsSubmodule.Msg_Color));
					InformationManager.ShowInquiry(new InquiryData(eventTitle4, eventOption4, true, false, eventButtonText, null, null, null), true);
					StopEvent();
				}
				#endregion

				#region Scout
				else if (MobileParty.MainParty.EffectiveScout != Hero.MainHero)
				{

					var CompanionIsFemale = scout.IsFemale;
					var CompanionGender = CompanionIsFemale ? "female" : "male";
					var gender = GenderAssignment.GetTheGenderAssignment(CompanionGender, false, "subjective");

					var scoutrelation = Hero.MainHero.GetRelation(MobileParty.MainParty.EffectiveScout);
					Hero.MainHero.SetPersonalRelation(MobileParty.MainParty.EffectiveScout, scoutrelation + 15);

					string eventTitle = new TextObject("{=CompanionAdmire_Title}Companion Admiration").ToString();

					string eventOption1 = new TextObject("{=CompanionAdmire_Event_Text}While travelling near {closestsettlement} your scout, {scout}, approaches you for a chat. Casual conversation soon turns to a more serious matter in regards" +
						" to the party and its future. \n\n{scout} wants you to know {gender} admires your leadership and looks forward to celebrating the many future victories along your side. Lastly noting {gender} is determined to bring honor" +
						" to the {clan} clan in any way {gender} can.")
						.SetTextVariable("closestsettlement", closestSettlement)
						.SetTextVariable("clan", clanname)
						.SetTextVariable("scout", scoutName)
						.SetTextVariable("gender", gender)
						.ToString();

					InformationManager.DisplayMessage(new InformationMessage(eventMsg1, RandomEventsSubmodule.Msg_Color));

					InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOption1, true, false, eventButtonText, null, null, null), true);
					StopEvent();
				}
				#endregion

				#region Quartermaster
				else if (MobileParty.MainParty.EffectiveQuartermaster != Hero.MainHero)
				{
					var CompanionIsFemale = quartermaster.IsFemale;
					var CompanionGender = CompanionIsFemale ? "female" : "male";
					var gender = GenderAssignment.GetTheGenderAssignment(CompanionGender, false, "subjective");

					var relation = Hero.MainHero.GetRelation(MobileParty.MainParty.EffectiveQuartermaster);
					Hero.MainHero.SetPersonalRelation(MobileParty.MainParty.EffectiveQuartermaster, relation + 15);

					string eventTitle2 = new TextObject("{=CompanionAdmire_Title}Companion Admiration").ToString();

					string eventOption2 = new TextObject("{=CompanionAdmire_Event_Text}While travelling near {closestsettlement} your quartermaster, {quartermaster}, approaches you for a chat. Casual conversation soon turns to a more serious matter in regards" +
						" to the party and its future. \n\n{quartermaster} wants you to know {gender} admires your leadership and looks forward to celebrating the many future victories along your side. Lastly noting {gender} is determined to bring honor" +
						" to the {clan} clan in any way {gender} can.")
						.SetTextVariable("closestsettlement", closestSettlement)
						.SetTextVariable("clan", clanname)
						.SetTextVariable("quartermaster", quartermasterName)
						.SetTextVariable("gender", gender)
						.ToString();

					InformationManager.DisplayMessage(new InformationMessage(eventMsg2, RandomEventsSubmodule.Msg_Color));
					InformationManager.ShowInquiry(new InquiryData(eventTitle2, eventOption2, true, false, eventButtonText, null, null, null), true);
					StopEvent();

				}
				#endregion

				#region Surgeon
				else if (MobileParty.MainParty.EffectiveSurgeon != Hero.MainHero)
				{
					var CompanionIsFemale = surgeon.IsFemale;
					var CompanionGender = CompanionIsFemale ? "female" : "male";
					var gender = GenderAssignment.GetTheGenderAssignment(CompanionGender, false, "subjective");

					var relation = Hero.MainHero.GetRelation(MobileParty.MainParty.EffectiveSurgeon);
					Hero.MainHero.SetPersonalRelation(MobileParty.MainParty.EffectiveSurgeon, relation + 15);

					string eventTitle3 = new TextObject("{=CompanionAdmire_Title}Companion Admiration").ToString();

					string eventOption3 = new TextObject("{=CompanionAdmire_Event_Text}While travelling near {closestsettlement} your surgeon, {surgeon}, approaches you for a chat. Casual conversation soon turns to a more serious matter in regards" +
						" to the party and its future. \n\n{surgeon} wants you to know {gender} admires your leadership and looks forward to celebrating the many future victories along your side. Lastly noting {gender} is determined to bring honor" +
						" to the {clan} clan in any way {gender} can.")
						.SetTextVariable("closestsettlement", closestSettlement)
						.SetTextVariable("clan", clanname)
						.SetTextVariable("surgeon", surgeonName)
						.SetTextVariable("gender", gender)
						.ToString();

					InformationManager.DisplayMessage(new InformationMessage(eventMsg3, RandomEventsSubmodule.Msg_Color));
					InformationManager.ShowInquiry(new InquiryData(eventTitle3, eventOption3, true, false, eventButtonText, null, null, null), true);
					StopEvent();

				}
				#endregion

			}


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

	public class CompanionAdmireData : RandomEventData
	{

		public CompanionAdmireData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
		{

		}

		public override BaseEvent GetBaseEvent()
		{
			return new CompanionAdmire();
		}
	}
}
