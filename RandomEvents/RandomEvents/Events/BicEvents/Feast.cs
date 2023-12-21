using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Bannerlord.RandomEvents.Helpers;
using Bannerlord.RandomEvents.Settings;
using Ini.Net;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace Bannerlord.RandomEvents.Events.BicEvents
{
	public sealed class Feast : BaseEvent
	{
		private readonly bool eventDisabled;

		public Feast() : base(ModSettings.RandomEvents.FeastData)
		{
			var ConfigFile = new IniFile(ParseIniFile.GetTheConfigFile());
            
			eventDisabled = ConfigFile.ReadBoolean("Feast", "EventDisabled");

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
			if (Settlement.CurrentSettlement == null)
				return false;
			var randomHero = Campaign.Current.AliveHeroes.Where(h => h.CurrentSettlement == Settlement.CurrentSettlement && h.IsLord && h != Hero.MainHero.Spouse && h.Clan != Clan.PlayerClan).OrderByDescending(h => MBRandom.RandomFloat).FirstOrDefault();

			return HasValidEventData() && randomHero != null && Clan.PlayerClan.Renown >= 500;
		}

		public override void StartEvent()
		{
			var currentSettlement = Settlement.CurrentSettlement.Name.ToString();

			#region Skill Checks
			var roguerySkill = Hero.MainHero.GetSkillValue(DefaultSkills.Roguery);
			var charmSkill = Hero.MainHero.GetSkillValue(DefaultSkills.Charm);

			var theftSuccess = false;
			var charmedNoble = false;
			var charmedNoble2 = false;
			var charmedNoble3 = false;
			var charmedNoble4 = false;
			var diffGender = true;

            if (roguerySkill >= 30)
			{
				theftSuccess = true;
			}
			if (charmSkill >= 120)
			{
				charmedNoble = true;
            }
			if (charmSkill >= 150)
            {
				charmedNoble2 = true;
            }
			if (charmSkill >= 300)
            {
				charmedNoble3 = true;
            }
			if (charmSkill >= 210)
			{
				charmedNoble4 = true;
			}
			#endregion

			#region Find Lord/Lady

			var randomHero = Campaign.Current.AliveHeroes.Where(h => h.CurrentSettlement == Settlement.CurrentSettlement && h.IsLord && h != Hero.MainHero.Spouse && h.Clan != Clan.PlayerClan).OrderByDescending(h => MBRandom.RandomFloat).FirstOrDefault();

			var targetLord = randomHero;
			var player = Hero.MainHero;
			var playerName = Hero.MainHero.Name;

			var targetLordIsFemale = targetLord != null && targetLord.IsFemale;
			var targetLordGender = targetLordIsFemale ? "female" : "male";
			var playerIsFemale = player.IsFemale;
			var playerGender = playerIsFemale ? "female" : "male";
			var targetLordGenderAdjective = GenderAssignment.GetTheGenderAssignment(targetLordGender, false, "adjective");
			var targetLordGenderSubjective = GenderAssignment.GetTheGenderAssignment(targetLordGender, false, "subjective");
			var targetLordGenderSubjectiveCAP = GenderAssignment.GetTheGenderAssignment(targetLordGender, true, "subjective");
			var targetLordGenderObjective = GenderAssignment.GetTheGenderAssignment(targetLordGender, false, "objective");
			var targetLordGenderTitle = GenderAssignment.GetTheGenderAssignment(targetLordGender, true, "title");
			var playerTitle = GenderAssignment.GetTheGenderAssignment(playerGender, true, "title");
			var playerGenderSubjective = GenderAssignment.GetTheGenderAssignment(playerGender, false, "subjective");
			#endregion

			#region Same Gender call
			if (targetLordGender.Equals(playerGender))
			{
				diffGender = false;
			}
			#endregion

			var relation = Hero.MainHero.GetRelation(targetLord);

			var eventTitle = new TextObject("{=Feast_Title}Feast Invitation").ToString();
			
			var eventDescription1 = new TextObject("{=Feast_Event_Description1}While in {settlement} you are approached by an envoy from {title} {targetLord} requesting your presence for a feast.")
				// ReSharper disable once PossibleNullReferenceException
				.SetTextVariable("targetLord", targetLord.Name)
				.SetTextVariable("settlement", currentSettlement)
				.SetTextVariable("gender", targetLordGenderObjective)
				.SetTextVariable("title", targetLordGenderTitle)
				.ToString();
				
			var eventButtonText1 = new TextObject("{=Feast_Event_Button_Text1}Choose").ToString();
			var eventButtonText2 = new TextObject("{=Feast_Event_Button_Text2}Done").ToString();

            #region Inquiry Elements
            //option A ---- Accept ----
            var eventOption1 = new TextObject("{=Feast_Event_Option_1}Accept").ToString();
			var eventOption1Hover = new TextObject("{=Feast_Event_Option_1_Hover}A feast sounds nice").ToString();
			//option B ---- Decline ---
			var eventOption2 = new TextObject("{=Feast_Event_Option_2}Decline").ToString();
			var eventOption2Hover = new TextObject("{=Feast_Event_Option_2_Hover}No time for this").ToString();
			var inquiryElements1 = new List<InquiryElement>
			{
				new InquiryElement("a", eventOption1, null, true, eventOption1Hover),
				new InquiryElement("b", eventOption2, null, true, eventOption2Hover),
			};
            #endregion

            var msid = new MultiSelectionInquiryData(eventTitle, eventDescription1, inquiryElements1, false, 1, 1, eventButtonText1, null,
			elements =>
			{
				switch ((string)elements[0].Identifier)
				{
					case "a"://Accept Invitation ===================================================
						 
                        #region Player OWN Settlement
                        //If Player Owns Settlement _____________________________
                        if (Settlement.CurrentSettlement.Owner == Hero.MainHero)
						{

							#region Good Relation
							if (relation > 34)
							{
								var eventDescription1a = new TextObject("{=Feast_Event_Description1a}[Good Relation] You arrive at your keep in {settlement} where {title} {targetLord} is waiting. Introductions are made and civil norms are met as per tradition before" +
									" finally taking a seat at the table, the sweet smell from the other room heightens your appetite. {targetLord} compliments your work here in {settlement}, noting the pristine condition of the keep, and gives thanks for " +
									"allowing {gender} to stay in such a fine area. A few minutes pass and small talk soon turns to laughter as you both discuss recent engagements and share grand stories matched only by that of" +
									" myths and legends. \n \n As the food finally arrives no time is wasted - both of you dig in to what can only be described as a meal fit for a king. ")
								.SetTextVariable("targetLord", targetLord.Name)
								.SetTextVariable("settlement", currentSettlement)
								.SetTextVariable("gender", targetLordGenderObjective)
								.SetTextVariable("title", targetLordGenderTitle)
								.ToString();


								#region Inquiry Elements 2
								//option A ---- Continue ----
								var eventOption1a = new TextObject("{=Feast_Event_Option_1a}Continue Feast").ToString();
								var eventOption1aHover = new TextObject("{=Feast_Event_Option_1a_Hover}We're having a great time").ToString();
								//option D ---- End ----
								var eventOption2a = new TextObject("{=Feast_Event_Option_2a}End Feast").ToString();
								var eventOption2aHover = new TextObject("{=Feast_Event_Option_2a_Hover}This is a good time to stop").ToString();



								var inquiryElements2 = new List<InquiryElement>
								{
									new InquiryElement("a", eventOption1a, null, true, eventOption1aHover),
										//new InquiryElement("b", eventOption2a, null, true, eventOption2aHover),
										//new InquiryElement("c", eventOption3a, null, true, eventOption3aHover),
									new InquiryElement("b", eventOption2a, null, true, eventOption2aHover),
								};
								#endregion

								var msid1a = new MultiSelectionInquiryData(eventTitle, eventDescription1a, inquiryElements2, false, 1, 1, eventButtonText1, null,
								elements =>
								{
									switch ((string)elements[0].Identifier)
									{
                                        #region Continue Feast 
                                        case "a": //Continue Feast ---------------------------


											var eventDescription1b = new TextObject("{=Feast_Event_Description1b}[Good Relation] After such a fantastic meal {targetLord} requests a new bottle of wine for the two of you to wash it all down. You signal" +
												" a servant to bring a bottle of your finest, as the occasion calls for nothing less. {targetLord} recalls {gender} most recent experience at the arena when {gender} friend drank so much they fell over the" +
												" wall and straight into the pit. But instead of crawling out, they decided to join in the fight! You both laugh hysterically, sharing nostalgic memories of times not forgotten.")
											.SetTextVariable("targetLord", targetLord.Name)
											.SetTextVariable("gender", targetLordGenderAdjective)
											.SetTextVariable("title", targetLordGenderTitle)
											.ToString();

											ChangeRelationAction.ApplyPlayerRelation(targetLord, 2);
											Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 2);
											//var eventMsg1 = new TextObject(
											//	"{=Feast_Event_Msg_1}Your relation has improved.").ToString();
											//InformationManager.DisplayMessage(new InformationMessage(eventMsg1, RandomEventsSubmodule.Msg_Color));

											#region Inquiry Elements 3
											//option D ---- End ----
											var eventOption2b = new TextObject("{=Feast_Event_Option_2b}Finish Feast").ToString();
											var eventOption2bHover = new TextObject("{=Feast_Event_Option_2b_Hover}Time to finish up").ToString();
											//option A ---- Skill: Charm different sex----
											var eventOption3b = new TextObject("{=Feast_Event_Option_3b}[Charm] Compliment").ToString();
											var eventOption3bHover = new TextObject("{=Feast_Event_Option_3b_Hover}Charm {gender} a little \n[Charm - lvl 120]")
											.SetTextVariable("gender", targetLordGenderObjective)
											.ToString();
											//option B ---- Skill: Charm same sex----
											var eventOption4b = new TextObject("{=Feast_Event_Option_4b}[Charm] Boast").ToString();
											var eventOption4bHover = new TextObject("{=Feast_Event_Option_4b_Hover}Boast their ego a bit \n[Charm - lvl 120]").ToString();
											//option C ---- Skill: Rogue ----
											var eventOption5b = new TextObject("{=Feast_Event_Option_5b}[Roguery] Theft").ToString();
											var eventOption5bHover = new TextObject("{=Feast_Event_Option_5b_Hover}Quite the coin purse.. \n[Roguery - lvl 60]")
											.SetTextVariable("gender", targetLordGenderSubjective)
											.ToString();

											var inquiryElements3 = new List<InquiryElement>();
											if (charmedNoble)
											{
												if (diffGender)
												{
													inquiryElements3.Add(new InquiryElement("a", eventOption3b, null, true, eventOption3bHover));
												}
												else if (diffGender == false)
												{
													inquiryElements3.Add(new InquiryElement("b", eventOption4b, null, true, eventOption4bHover));
												}
											}
											if (theftSuccess)
											{
												inquiryElements3.Add(new InquiryElement("c", eventOption5b, null, true, eventOption5bHover));
											}
											inquiryElements3.Add(new InquiryElement("d", eventOption2b, null, true, eventOption2bHover));


											#endregion

											var msid1b = new MultiSelectionInquiryData(eventTitle, eventDescription1b, inquiryElements3, false, 1, 1, eventButtonText1, null,
												elements =>
												{
														switch ((string)elements[0].Identifier)
														{
															#region Charm Diff Sex
															  case "a":// CHARM different sex

															var eventDescription1c = new TextObject("{=Feast_Event_Description1c}[Good Relation] Needless to say, the wine has made its way through the both of you and it seems there is quite a sense of attraction." +
																" You notice a beauty in {targetLord} you haven't been aware of before, something about {gender} eyes are pulling you in. As you smile {gender} eyes meet yours and {genderSUB} gives you a little" +
																" wink. You both know what is happening here..")
															.SetTextVariable("targetLord", targetLord.Name)
															.SetTextVariable("gender", targetLordGenderAdjective)
															.SetTextVariable("genderSUB", targetLordGenderSubjective)
															.ToString();


															ChangeRelationAction.ApplyPlayerRelation(targetLord, 5);
															Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 10);
															var eventMsgCharm1 = new TextObject(
																"{=Feast_Event_Msg_Charm1}You charm {targetLord}.")
															.SetTextVariable("targetLord", targetLord.Name)
															.ToString();
															InformationManager.DisplayMessage(new InformationMessage(eventMsgCharm1, RandomEventsSubmodule.Msg_Color));

															#region Inquiry Elements
															//option A ---- Hook up ----
															var eventOption1c = new TextObject("{=Feast_Event_Option_1c}[Charm] Go somewhere private").ToString();
															var eventOption1cHover = new TextObject("{=Feast_Event_Option_1c_Hover}Show {gender} around.. \n[Charm - lvl 150]")
															.SetTextVariable("gender", targetLordGenderObjective)
															.ToString();
															//option B ---- End ----
															var eventOption2c = new TextObject("{=Feast_Event_Option_2c}End Feast").ToString();
															var eventOption2cHover = new TextObject("{=Feast_Event_Option_2c_Hover}Let's not get carried away..").ToString();
                                                     
                                                            var inquiryElements4 = new List<InquiryElement>();
															if (charmedNoble2)
															{
																inquiryElements4.Add(new InquiryElement("a", eventOption1c, null, true, eventOption1cHover)); 
															}
															inquiryElements4.Add(new InquiryElement("b", eventOption2c, null, true, eventOption2cHover));
                                                            #endregion

                                                            var msid1c = new MultiSelectionInquiryData(eventTitle, eventDescription1c, inquiryElements4, false, 1, 1, eventButtonText1, null,
																elements =>
																{
																	switch ((string)elements[0].Identifier)
																	{
																		case "a":// Proceed to hook up

                                                                        #region Hook Up
                                                                            var eventDescription1d = new TextObject("{=Feast_Event_Description1d}[Good Relation] You stand from your chair and offer your hand to {title} {targetLord}, {genderSUB} accepts and raises as well." +
																				" The both of you walk into the other room and aren't seen again for a couple hours..")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.SetTextVariable("genderSUB", targetLordGenderSubjective)
																			.SetTextVariable("title", targetLordGenderTitle)
																			.ToString();

																			ChangeRelationAction.ApplyPlayerRelation(targetLord, 15);
																			Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 50);
																			var eventMsgCharm2 = new TextObject(
																				"{=Feast_Event_Msg_Charm2}You and {targetLord} have a great time.")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.ToString();
																			InformationManager.DisplayMessage(new InformationMessage(eventMsgCharm2, RandomEventsSubmodule.Msg_Color));

																			InformationManager.ShowInquiry(new InquiryData(eventTitle, eventDescription1d, true, false, eventButtonText2, null, null, null), true);

																			StopEvent();

																			break;
																		#endregion

																		#region Leave
																		case "b": // Leave

																			var eventDescription1e = new TextObject("{=Feast_Event_Description1e}[Good Relation] Although the tension is more than enough to write a story of its own - you decide it's best for the two of you to" +
																				" go your separate ways, for now.. {title} {targetLord} thanks you for such an exquisite meal and looks forward to staying in {settlement} for a while longer.")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.SetTextVariable("settlement", currentSettlement)
																			.SetTextVariable("title", targetLordGenderTitle)
																			.ToString();

																			ChangeRelationAction.ApplyPlayerRelation(targetLord, 5);
																			Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 10);
																			var eventMsgEnd = new TextObject(
																				"{=Feast_Event_Msg_End}The feast has ended.")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.ToString();
																			InformationManager.DisplayMessage(new InformationMessage(eventMsgEnd, RandomEventsSubmodule.Msg_Color));

																			InformationManager.ShowInquiry(new InquiryData(eventTitle, eventDescription1e, true, false, eventButtonText2, null, null, null), true);

																			StopEvent();

																		break;
                                                                            #endregion 

                                                                    }
                                                                }, null, null);

															MBInformationManager.ShowMultiSelectionInquiry(msid1c, true);

																StopEvent();
                                                            

                                                            break;
														    #endregion

														    #region Charm Same Sex
															case "b": // CHARM same sex

															var eventDescription1r = new TextObject("{=Feast_Event_Description1r}[Good Relation] A few more minutes of joyful talk go by and you recall {title} {targetLord}'s recent political feats. Claiming word of such events have made their" +
																" way all the way here to the halls of {settlement}. {genderSUBCAP} looks at you with a gleeful smile like that of a small child, then erupts once again into wine induced laughter as {genderSUB} proceeds to tell you all about the " +
																"event in great detail.  Starting from the very beginning, of course..")
															.SetTextVariable("targetLord", targetLord.Name)
															.SetTextVariable("genderSUB", targetLordGenderSubjective)
															.SetTextVariable("genderSUBCAP", targetLordGenderSubjectiveCAP)
															.SetTextVariable("title", targetLordGenderTitle)
															.SetTextVariable("settlement", currentSettlement)
															.ToString();

															ChangeRelationAction.ApplyPlayerRelation(targetLord, 5);
															var eventMsgCharm1a = new TextObject(
																"{=Feast_Event_Msg_Charm1}You boast {targetLord}.")
															.SetTextVariable("targetLord", targetLord.Name)
															.ToString();
															InformationManager.DisplayMessage(new InformationMessage(eventMsgCharm1a, RandomEventsSubmodule.Msg_Color));

															#region Inquiry Elements
															//option A ---- Continue ----
															var eventOption1r = new TextObject("{=Feast_Event_Option_1r}Continue Feast").ToString();
															var eventOption1rHover = new TextObject("{=Feast_Event_Option_1r_Hover}I'm having a great time")
															.SetTextVariable("gender", targetLordGenderObjective)
															.ToString();
															//option B ---- End ----
															var eventOption2r = new TextObject("{=Feast_Event_Option_2r}End Feast").ToString();
															var eventOption2rHover = new TextObject("{=Feast_Event_Option_2r_Hover}This is a good time to stop").ToString();

															var inquiryElements5 = new List<InquiryElement>
															{
																new InquiryElement("a", eventOption1r, null, true, eventOption1rHover),
																new InquiryElement("b", eventOption2r, null, true, eventOption2rHover),
															};
															#endregion

															var msid1d = new MultiSelectionInquiryData(eventTitle, eventDescription1r, inquiryElements5, false, 2, 2, eventButtonText1, null,
																elements =>
																{
																	switch ((string)elements[0].Identifier)
																	{
                                                                        #region Continue
                                                                        case "a":// Continue Feast

																			var eventDescription2r = new TextObject("{=Feast_Event_Description2r}[Good Relation] After finishing yet another round of cheerful story telling, you insist on the two of you continuing this epic feast in a more lively" +
																				" fashion. {targetLord} says a few of {gender} friends are probably at the tavern and would love to meet you. The two of you head to the tavern and aren't seen again for a couple hours..")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.SetTextVariable("genderSUB", targetLordGenderSubjective)
																			.SetTextVariable("gender", targetLordGenderAdjective)
																			.SetTextVariable("title", targetLordGenderTitle)
																			.ToString();

																			ChangeRelationAction.ApplyPlayerRelation(targetLord, 5);
																			Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 20);
																			var eventMsgCharm2 = new TextObject(
																				"{=Feast_Event_Msg_Charm2}You and {targetLord} have a great time.")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.ToString();
																			InformationManager.DisplayMessage(new InformationMessage(eventMsgCharm2, RandomEventsSubmodule.Msg_Color));

																			InformationManager.ShowInquiry(new InquiryData(eventTitle, eventDescription2r, true, false, eventButtonText2, null, null, null), true);
																			StopEvent();
																			
																			break;
                                                                        #endregion

                                                                        #region Leave
                                                                        case "b": // Leave

																			var eventDescription3r = new TextObject("{=Feast_Event_Description3r}[Good Relation] After finishing yet another round of cheerful story telling, {title} {targetLord} offers to make a toast in your honor, to which you accept. " +
																				"\n \n “To {Ptitle} {player}, an excellent leader, and an even better friend. May {playerSUB} live forever!”. \n \n {targetLord} then thanks you for such an exquisite meal and looks forward to staying in {settlement} " +
																				"for a while longer. And with that, the feast comes to and end..")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.SetTextVariable("settlement", currentSettlement)
																			.SetTextVariable("title", targetLordGenderTitle)
																			.SetTextVariable("Ptitle", playerTitle)
																			.SetTextVariable("player", playerName)
																			.SetTextVariable("playerSUB", playerGenderSubjective)
																			.ToString();


																			ChangeRelationAction.ApplyPlayerRelation(targetLord, 2);
																			Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 10);
																			var eventMsgEnd = new TextObject(
																				"{=Feast_Event_Msg_End}The feast has ended.")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.ToString();
																			InformationManager.DisplayMessage(new InformationMessage(eventMsgEnd, RandomEventsSubmodule.Msg_Color));


																			InformationManager.ShowInquiry(new InquiryData(eventTitle, eventDescription3r, true, false, eventButtonText2, null, null, null), true);

																			StopEvent();

																			break;
                                                                            #endregion
                                                                    }
                                                                }, null, null);

															MBInformationManager.ShowMultiSelectionInquiry(msid1d, true);

															StopEvent();
															break;
															#endregion

															#region Rogue
															case "c": // ROGUE

															var eventDescription1rr = new TextObject("{=Feast_Event_Description1r}[Good Relation] Amidst the joyful laughter, you signal one of your servants to bring more wine and make a scene. As the wine arrives your" +
																" servant trips and drops the bottle onto the table, spilling it into {targetLord}'s lap. You react quickly, grabbing a cloth from the table and reaching towards {genderOBJ}, attempting to wipe away the " +
																"still running wine from {genderADJ} shirt. When the moment is right you swipe {genderADJ} coin purse without {genderOBJ} ever noticing.")
															.SetTextVariable("targetLord", targetLord.Name)
															.SetTextVariable("genderADJ", targetLordGenderAdjective)
															.SetTextVariable("genderOBJ", targetLordGenderObjective)
															.SetTextVariable("genderSUB", targetLordGenderSubjective)
															.SetTextVariable("genderSUBCAP", targetLordGenderSubjectiveCAP)
															.SetTextVariable("title", targetLordGenderTitle)
															.SetTextVariable("settlement", currentSettlement)
															.ToString();

															var MoneyStole = MBRandom.RandomInt(1000 , 5000);
															Hero.MainHero.ChangeHeroGold(+MoneyStole);
															Hero.MainHero.AddSkillXp(DefaultSkills.Roguery, 50);
															var eventMsgRogue = new TextObject(
																"{=Feast_Event_Msg_Rogue}You steal the coin purse.").ToString();
															InformationManager.DisplayMessage(new InformationMessage(eventMsgRogue, RandomEventsSubmodule.Msg_Color));

															#region Inquiry Elements
															//option A ---- Continue ----
															var eventOption1rr = new TextObject("{=Feast_Event_Option_1r}Continue Feast").ToString();
															var eventOption1rrHover = new TextObject("{=Feast_Event_Option_1r_Hover}I'm having a great time")
															.SetTextVariable("gender", targetLordGenderObjective)
															.ToString();
															//option B ---- End ----
															var eventOption2rr = new TextObject("{=Feast_Event_Option_2r}End Feast").ToString();
															var eventOption2rrHover = new TextObject("{=Feast_Event_Option_2r_Hover}This is a good time to stop").ToString();

															var inquiryElements5r = new List<InquiryElement>
															{
																new InquiryElement("a", eventOption1rr, null, true, eventOption1rrHover),
																new InquiryElement("b", eventOption2rr, null, true, eventOption2rrHover),
															};
															#endregion

															var msid1dr = new MultiSelectionInquiryData(eventTitle, eventDescription1rr, inquiryElements5r, false, 2, 2, eventButtonText1, null,
																elements =>
																{
																	switch ((string)elements[0].Identifier)
																	{
																		#region Continue
																		case "a":// Continue Feast

																			var eventDescription2r = new TextObject("{=Feast_Event_Description2r}[Good Relation] After finishing yet another round of cheerful story telling, you insist on the two of you continuing this epic feast in a more lively" +
																				" fashion. {targetLord} says a few of {gender} friends are probably at the tavern and would love to meet you. The two of you head to the tavern and aren't seen again for a couple hours..")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.SetTextVariable("genderSUB", targetLordGenderSubjective)
																			.SetTextVariable("gender", targetLordGenderAdjective)
																			.SetTextVariable("title", targetLordGenderTitle)
																			.ToString();

																			ChangeRelationAction.ApplyPlayerRelation(targetLord, 4);
																			Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 20);
																			var eventMsgCharm2 = new TextObject(
																				"{=Feast_Event_Msg_Charm2}You and {targetLord} have a great time.")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.ToString();
																			InformationManager.DisplayMessage(new InformationMessage(eventMsgCharm2, RandomEventsSubmodule.Msg_Color));

																			InformationManager.ShowInquiry(new InquiryData(eventTitle, eventDescription2r, true, false, eventButtonText2, null, null, null), true);

																			StopEvent();

																			break;
																		#endregion

																		#region Leave
																		case "b": // Leave

																			var eventDescription3r = new TextObject("{=Feast_Event_Description3r}[Good Relation] After finishing yet another round of cheerful story telling, {title} {targetLord} offers to make a toast in your honor, to which you accept. " +
																				"\n \n “To {Ptitle} {player}, an excellent leader, and an even better friend. May {playerSUB} live forever!”. \n \n {targetLord} then thanks you for such an exquisite meal and looks forward to staying in {settlement} " +
																				"for a while longer. And with that, the feast comes to and end..")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.SetTextVariable("settlement", currentSettlement)
																			.SetTextVariable("title", targetLordGenderTitle)
																			.SetTextVariable("Ptitle", playerTitle)
																			.SetTextVariable("player", playerName)
																			.SetTextVariable("playerSUB", playerGenderSubjective)
																			.ToString();

																			ChangeRelationAction.ApplyPlayerRelation(targetLord, 3);
																			Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 10);
																			var eventMsgEnd = new TextObject(
																				"{=Feast_Event_Msg_End}The feast has ended.")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.ToString();
																			InformationManager.DisplayMessage(new InformationMessage(eventMsgEnd, RandomEventsSubmodule.Msg_Color));

																			InformationManager.ShowInquiry(new InquiryData(eventTitle, eventDescription3r, true, false, eventButtonText2, null, null, null), true);

																			StopEvent();

																			break;
																			#endregion
																	}
																}, null, null);

															MBInformationManager.ShowMultiSelectionInquiry(msid1dr, true);

															StopEvent();

															break;
															#endregion

															#region Finish Up
															case "d": // Finish Up


															var eventDescription3r = new TextObject("{=Feast_Event_Description3r}[Good Relation] As the laughter quiets down, {title} {targetLord} offers to make a toast in your honor, to which you accept. " +
																"\n \n “To {Ptitle} {player}, an excellent leader, and an even better friend. May {playerSUB} live forever!”. \n \n {targetLord} then thanks you for such an exquisite meal and looks forward to staying in {settlement} " +
																"for a while longer. And with that, the feast comes to and end..")
															.SetTextVariable("targetLord", targetLord.Name)
															.SetTextVariable("settlement", currentSettlement)
															.SetTextVariable("title", targetLordGenderTitle)
															.SetTextVariable("Ptitle", playerTitle)
															.SetTextVariable("player", playerName)
															.SetTextVariable("playerSUB", playerGenderSubjective)
															.ToString();

															ChangeRelationAction.ApplyPlayerRelation(targetLord, 3);
															Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 10);
															var eventMsgEnd = new TextObject(
																"{=Feast_Event_Msg_End}The feast has ended.")
															.SetTextVariable("targetLord", targetLord.Name)
															.ToString();
															InformationManager.DisplayMessage(new InformationMessage(eventMsgEnd, RandomEventsSubmodule.Msg_Color));

															InformationManager.ShowInquiry(new InquiryData(eventTitle, eventDescription3r, true, false, eventButtonText2, null, null, null), true);

															StopEvent();

															break;
															#endregion
													}
												}, null, null);

											MBInformationManager.ShowMultiSelectionInquiry(msid1b, true);

											StopEvent();
											break;

										#endregion

										#region End Feast
										case "b": //End Feast-------------

											var eventDescriptionEndFeast1 = new TextObject("{=Feast_Event_DescriptionEndFeast}[Good Relation] After finishing your meals {title} {targetLord} offers to make a toast in your honor, to which you accept. \n \n" +
												"“To {Ptitle} {player}, an excellent leader, and an even better friend. May {playerSUB} live forever!”. \n \n And with that, the feast comes to an end.")
											.SetTextVariable("targetLord", targetLord.Name)
											.SetTextVariable("settlement", currentSettlement)
											.SetTextVariable("title", targetLordGenderTitle)
											.SetTextVariable("Ptitle", playerTitle)
											.SetTextVariable("player", playerName)
											.SetTextVariable("playerSUB", playerGenderSubjective)

											.ToString();

											ChangeRelationAction.ApplyPlayerRelation(targetLord, 2);
											Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 10);
											var eventMsgEnd = new TextObject(
												"{=Feast_Event_Msg_End}The feast has ended.")
											.SetTextVariable("targetLord", targetLord.Name)
											.ToString();
											InformationManager.DisplayMessage(new InformationMessage(eventMsgEnd, RandomEventsSubmodule.Msg_Color));

											InformationManager.ShowInquiry(new InquiryData(eventTitle, eventDescriptionEndFeast1, true, false, eventButtonText2, null, null, null), true);

											StopEvent();

											break;
                                        #endregion
                                    }
                                }, null, null);

								MBInformationManager.ShowMultiSelectionInquiry(msid1a, true);

								StopEvent();
							}
							#endregion

							#region Bad Relation
							else if (relation < -9)
							{

								var eventDescription1u = new TextObject("{=Feast_Event_Description1u}[Bad Relation] You arrive at your keep in {settlement} where {title} {targetLord} is waiting. Introductions are made and civil norms are met as per tradition before" +
									" finally taking a seat at the table. {targetLord} makes a snarky remark regarding your work here in {settlement}, noting the poor condition of the keep, and gives a shallow thanks for " +
									"allowing {gender} to stay in such a 'fine' area. A few minutes pass and small talk builds tension as {targetLord} begins questioning your support of current political affairs. It is obvious {genderSUB} merely wishes to extort" +
									" some degree of support from you, which would explain this invitation..")
								.SetTextVariable("targetLord", targetLord.Name)
								.SetTextVariable("settlement", currentSettlement)
								.SetTextVariable("gender", targetLordGenderObjective)
								.SetTextVariable("genderSUB", targetLordGenderSubjective)
								.SetTextVariable("title", targetLordGenderTitle)
								.ToString();


								#region Inquiry Elements 2
								//option A ---- Continue ----
								var eventOption1u = new TextObject("{=Feast_Event_Option_1a}Continue Feast").ToString();
								var eventOption1uHover = new TextObject("{=Feast_Event_Option_1u_Hover}Let's continue..").ToString();
								//option D ---- End ----
								var eventOption2a = new TextObject("{=Feast_Event_Option_2a}End Feast").ToString();
								var eventOption2aHover = new TextObject("{=Feast_Event_Option_2a_Hover}This is a good time to stop").ToString();



								var inquiryElements2 = new List<InquiryElement>
								{
									new InquiryElement("a", eventOption1u, null, true, eventOption1uHover),
										//new InquiryElement("b", eventOption2a, null, true, eventOption2aHover),
										//new InquiryElement("c", eventOption3a, null, true, eventOption3aHover),
									new InquiryElement("b", eventOption2a, null, true, eventOption2aHover),
								};
								#endregion

								var msid1a = new MultiSelectionInquiryData(eventTitle, eventDescription1u, inquiryElements2, false, 1, 1, eventButtonText1, null,
								elements =>
								{
									switch ((string)elements[0].Identifier)
									{
										#region Continue Feast 
										case "a": //Continue Feast ---------------------------


											var eventDescription2u = new TextObject("{=Feast_Event_Description2u}[Bad Relation] After an agonizing meal {targetLord} requests a new bottle of wine for the two of you to wash it all down. You signal" +
												" a servant to bring a bottle, specifying 'nothing fancy'. {targetLord} continues questioning your stance on the current political climate, hoping to reach some sort of agreement on the issues. You " +
												"keep a neutral tone and blank stare while also nodding in agreeance as though to keep the discussion civil.")
											.SetTextVariable("targetLord", targetLord.Name)
											.SetTextVariable("gender", targetLordGenderAdjective)
											.SetTextVariable("title", targetLordGenderTitle)
											.ToString();

											ChangeRelationAction.ApplyPlayerRelation(targetLord, 1);
											Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 2);

											#region Inquiry Elements 3
											//option D ---- End ----
											var eventOption2b = new TextObject("{=Feast_Event_Option_2b}Finish Feast").ToString();
											var eventOption2bHover = new TextObject("{=Feast_Event_Option_2b_Hover}Time to finish up").ToString();
											//option A ---- Skill: Charm different sex----
											var eventOption3b = new TextObject("{=Feast_Event_Option_3b}[Charm] Compliment").ToString();
											var eventOption3bHover = new TextObject("{=Feast_Event_Option_3b_Hover}Charm {gender} a little \n[Charm - lvl 120]")
											.SetTextVariable("gender", targetLordGenderObjective)
											.ToString();
											//option B ---- Skill: Charm same sex----
											var eventOption4b = new TextObject("{=Feast_Event_Option_4b}[Charm] Boast").ToString();
											var eventOption4bHover = new TextObject("{=Feast_Event_Option_4b_Hover}Boast their ego a bit \n[Charm - lvl 120]").ToString();
											//option C ---- Skill: Rogue ----
											var eventOption5b = new TextObject("{=Feast_Event_Option_5b}[Roguery] Theft").ToString();
											var eventOption5bHover = new TextObject("{=Feast_Event_Option_5b_Hover}Quite the coin purse.. \n[Roguery - lvl 60]")
											.SetTextVariable("gender", targetLordGenderSubjective)
											.ToString();

											var inquiryElements3 = new List<InquiryElement>();
											if (charmedNoble)
											{
												if (diffGender)
												{
													inquiryElements3.Add(new InquiryElement("a", eventOption3b, null, true, eventOption3bHover));
												}
												else if (diffGender == false)
												{
													inquiryElements3.Add(new InquiryElement("b", eventOption4b, null, true, eventOption4bHover));
												}
											}
											if (theftSuccess)
											{
												inquiryElements3.Add(new InquiryElement("c", eventOption5b, null, true, eventOption5bHover));
											}
											inquiryElements3.Add(new InquiryElement("d", eventOption2b, null, true, eventOption2bHover));


											#endregion

											var msid1b = new MultiSelectionInquiryData(eventTitle, eventDescription2u, inquiryElements3, false, 1, 1, eventButtonText1, null,
												elements =>
												{
													switch ((string)elements[0].Identifier)
													{
														#region Charm Diff Sex
														case "a":// CHARM different sex

															var eventDescription3u = new TextObject("{=Feast_Event_Description3u}[Bad Relation] Despite the fact you both seem to despise one another, it seems there is quite a sense of attraction." +
																" Perhaps it's the wine, or perhaps you've had a long day, regardless.. You notice a beauty in {targetLord} you haven't been aware of before, something about {gender} eyes are " +
																"pulling you in. As you smile {gender} eyes meet yours and {genderSUB} gives you a little wink. You both know what is happening here..")
															.SetTextVariable("targetLord", targetLord.Name)
															.SetTextVariable("gender", targetLordGenderAdjective)
															.SetTextVariable("genderSUB", targetLordGenderSubjective)
															.ToString();

															ChangeRelationAction.ApplyPlayerRelation(targetLord, 2);
															Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 10);
															var eventMsgCharm1 = new TextObject(
																"{=Feast_Event_Msg_Charm1}You charm {targetLord}.")
															.SetTextVariable("targetLord", targetLord.Name)
															.ToString();
															InformationManager.DisplayMessage(new InformationMessage(eventMsgCharm1, RandomEventsSubmodule.Msg_Color));

															#region Inquiry Elements
															//option A ---- Hook up ----
															var eventOption2uc = new TextObject("{=Feast_Event_Option_1c}[Charm] Go somewhere private").ToString();
															var eventOption2ucHover = new TextObject("{=Feast_Event_Option_2uc_Hover}Show {gender} around.. \n[Charm - lvl 300]")
															.SetTextVariable("gender", targetLordGenderObjective)
															.ToString();
															//option B ---- End ----
															var eventOption2c = new TextObject("{=Feast_Event_Option_2c}End Feast").ToString();
															var eventOption2cHover = new TextObject("{=Feast_Event_Option_2c_Hover}Let's not get carried away..").ToString();

															var inquiryElements4 = new List<InquiryElement>();
															if (charmedNoble3)
															{
																inquiryElements4.Add(new InquiryElement("a", eventOption2uc, null, true, eventOption2ucHover));
															}
															inquiryElements4.Add(new InquiryElement("b", eventOption2c, null, true, eventOption2cHover));
															#endregion

															var msid1c = new MultiSelectionInquiryData(eventTitle, eventDescription3u, inquiryElements4, false, 1, 1, eventButtonText1, null,
																elements =>
																{
																	switch ((string)elements[0].Identifier)
																	{
                                                                        #region Hook up
                                                                        case "a":// Proceed to hook up

																			var eventDescription4u = new TextObject("{=Feast_Event_Description4u}[Bad Relation] You stand from your chair and offer your hand to {title} {targetLord}, {genderSUB} accepts and raises as well." +
																				" The both of you walk into the other room and aren't seen again for a couple hours..")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.SetTextVariable("genderSUB", targetLordGenderSubjective)
																			.SetTextVariable("title", targetLordGenderTitle)
																			.ToString();

																			ChangeRelationAction.ApplyPlayerRelation(targetLord, 5);
																			Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 50);
																			var eventMsgCharm2 = new TextObject(
																				"{=Feast_Event_Msg_Charm2}You and {targetLord} have a great time.")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.ToString();
																			InformationManager.DisplayMessage(new InformationMessage(eventMsgCharm2, RandomEventsSubmodule.Msg_Color));

																			InformationManager.ShowInquiry(new InquiryData(eventTitle, eventDescription4u, true, false, eventButtonText2, null, null, null), true);

																			StopEvent();

																			break;
                                                                        #endregion

                                                                        #region Leave
                                                                        case "b": // Leave

																			var eventDescription5u = new TextObject("{=Feast_Event_Description5u}[Bad Relation] Although the tension is more than enough to write a story of its own - you decide it's best for the two of you to" +
																				" go your separate ways, for now.. {title} {targetLord} looks quite displeased with the feast, and hopes to leave {settlement} as quickly as possible.")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.SetTextVariable("settlement", currentSettlement)
																			.SetTextVariable("title", targetLordGenderTitle)
																			.ToString();

																			ChangeRelationAction.ApplyPlayerRelation(targetLord, 1);
																			Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 10);
																			var eventMsgEnd = new TextObject(
																				"{=Feast_Event_Msg_End}The feast has ended.")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.ToString();
																			InformationManager.DisplayMessage(new InformationMessage(eventMsgEnd, RandomEventsSubmodule.Msg_Color));

																			InformationManager.ShowInquiry(new InquiryData(eventTitle, eventDescription5u, true, false, eventButtonText2, null, null, null), true);

																			StopEvent();

																			break;
																			#endregion

																	}
																}, null, null);

															MBInformationManager.ShowMultiSelectionInquiry(msid1c, true);

															StopEvent();


															break;
														#endregion

														#region Charm Same Sex
														case "b": // CHARM same sex

															var eventDescription1ru = new TextObject("{=Feast_Event_Description1ru}[Bad Relation] After what seems like an eternity of interrogation, you decide to play along and admit to {targetLord} that you believe" +
																" {genderADJ} plan is actually quite brilliant. Stating you didn't wish to state this before only because you were quite jealous that you hadn't thought of it first.")
															.SetTextVariable("targetLord", targetLord.Name)
															.SetTextVariable("genderADJ", targetLordGenderAdjective)
															.SetTextVariable("title", targetLordGenderTitle)
															.SetTextVariable("settlement", currentSettlement)
															.ToString();

															ChangeRelationAction.ApplyPlayerRelation(targetLord, 2);
															Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 10);
															var eventMsgCharm1a = new TextObject(
																"{=Feast_Event_Msg_Charm1}You charm {targetLord}.")
															.SetTextVariable("targetLord", targetLord.Name)
															.ToString();
															InformationManager.DisplayMessage(new InformationMessage(eventMsgCharm1a, RandomEventsSubmodule.Msg_Color));

															#region Inquiry Elements
															//option A ---- Continue ----
															var eventOption1ru = new TextObject("{=Feast_Event_Option_1a}Continue Feast").ToString();
															var eventOption1ruHover = new TextObject("{=Feast_Event_Option_1u_Hover}Let's continue..")
															.ToString();
															//option B ---- End ----
															var eventOption2r = new TextObject("{=Feast_Event_Option_2a}End Feast").ToString();
															var eventOption2rHover = new TextObject("{=Feast_Event_Option_2a_Hover}This is a good time to stop").ToString();

															var inquiryElements5 = new List<InquiryElement>
															{
																new InquiryElement("a", eventOption1ru, null, true, eventOption1ruHover),
																new InquiryElement("b", eventOption2r, null, true, eventOption2rHover),
															};
															#endregion

															var msid1d = new MultiSelectionInquiryData(eventTitle, eventDescription1ru, inquiryElements5, false, 2, 2, eventButtonText1, null,
																elements =>
																{
																	switch ((string)elements[0].Identifier)
																	{
																		#region Continue
																		case "a":// Continue Feast

																			var eventDescription2ru = new TextObject("{=Feast_Event_Description2ru}[Bad Relation] A few minutes of careful boasting leads to {title} {targetLord} feeling quite impressed by your words. {genderSUB} confesses" +
																				" that had {genderSUB} known you felt this way perhaps you could be friends. You offer to show {genderOBJ} around {settlement} while discussing further {targetLord}'s political agenda. To which" +
																				" {genderSUB} accepts.")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.SetTextVariable("genderSUB", targetLordGenderSubjective)
																			.SetTextVariable("genderOBJ", targetLordGenderObjective)
																			.SetTextVariable("gender", targetLordGenderAdjective)
																			.SetTextVariable("title", targetLordGenderTitle)
																			.SetTextVariable("settlement", currentSettlement)
																			.ToString();

																			ChangeRelationAction.ApplyPlayerRelation(targetLord, 2);
																			Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 20);
																			var eventMsgCharm2 = new TextObject(
																				"{=Feast_Event_Msg_End}The feast has ended.")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.ToString();
																			InformationManager.DisplayMessage(new InformationMessage(eventMsgCharm2, RandomEventsSubmodule.Msg_Color));

																			InformationManager.ShowInquiry(new InquiryData(eventTitle, eventDescription2ru, true, false, eventButtonText2, null, null, null), true);

																			StopEvent();

																			break;
																		#endregion

																		#region Leave
																		case "b": // Leave

																			var eventDescription3ru = new TextObject("{=Feast_Event_Description3ru}[Bad Relation] A few minutes of careful boasting leads to {title} {targetLord} feeling quite impressed by your words. {genderSUB} confesses" +
																				" that had {genderSUB} known you felt this way perhaps you could be friends. On that note you declare the feast is finished, giving {targetLord} a formal farewell before leaving the keep.")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.SetTextVariable("genderSUB", targetLordGenderSubjective)
																			.SetTextVariable("genderOBJ", targetLordGenderObjective)
																			.SetTextVariable("gender", targetLordGenderAdjective)
																			.SetTextVariable("title", targetLordGenderTitle)
																			.SetTextVariable("settlement", currentSettlement)
																			.ToString();

																			ChangeRelationAction.ApplyPlayerRelation(targetLord, 1);
																			Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 10);
																			var eventMsgCharm2a = new TextObject(
																				"{=Feast_Event_Msg_End}The feast has ended.")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.ToString();
																			InformationManager.DisplayMessage(new InformationMessage(eventMsgCharm2a, RandomEventsSubmodule.Msg_Color));

																			InformationManager.ShowInquiry(new InquiryData(eventTitle, eventDescription3ru, true, false, eventButtonText2, null, null, null), true);

																			StopEvent();

																			break;
																			#endregion
																	}
																}, null, null);

															MBInformationManager.ShowMultiSelectionInquiry(msid1d, true);

															StopEvent();
															break;
														#endregion

														#region Rogue
														case "c": // ROGUE

															var eventDescription1rq = new TextObject("{=Feast_Event_Description1rq}[Bad Relation] Amidst the dreadful debate, you signal one of your servants to bring more wine and make a scene. As the wine arrives your" +
																" servant trips and drops the bottle onto the table, spilling it into {targetLord}'s lap. You react quickly, grabbing a cloth from the table and reaching towards {genderOBJ}, attempting to wipe away the " +
																"still running wine from {genderADJ} shirt. When the moment is right you swipe {genderADJ} coin purse without {genderOBJ} ever noticing.")
															.SetTextVariable("targetLord", targetLord.Name)
															.SetTextVariable("genderADJ", targetLordGenderAdjective)
															.SetTextVariable("genderOBJ", targetLordGenderObjective)
															.ToString();

															var MoneyStole = MBRandom.RandomInt(1000, 5000);
															Hero.MainHero.ChangeHeroGold(+MoneyStole);
															Hero.MainHero.AddSkillXp(DefaultSkills.Roguery, 50);
															var eventMsgRogue = new TextObject(
																"{=Feast_Event_Msg_Rogue}You steal the coin purse.").ToString();
															InformationManager.DisplayMessage(new InformationMessage(eventMsgRogue, RandomEventsSubmodule.Msg_Color));

															#region Inquiry Elements
															//option A ---- Continue ----
															var eventOption1rq = new TextObject("{=Feast_Event_Option_1r}Continue Feast").ToString();
															var eventOption1rqHover = new TextObject("{=Feast_Event_Option_1u_Hover}Let's continue..")
															.ToString();
															//option B ---- End ----
															var eventOption2rr = new TextObject("{=Feast_Event_Option_2r}End Feast").ToString();
															var eventOption2rrHover = new TextObject("{=Feast_Event_Option_2r_Hover}This is a good time to stop").ToString();

															var inquiryElements5r = new List<InquiryElement>
															{
																new InquiryElement("a", eventOption1rq, null, true, eventOption1rqHover),
																new InquiryElement("b", eventOption2rr, null, true, eventOption2rrHover),
															};
															#endregion

															var msid1dr = new MultiSelectionInquiryData(eventTitle, eventDescription1rq, inquiryElements5r, false, 2, 2, eventButtonText1, null,
																elements =>
																{
																	switch ((string)elements[0].Identifier)
																	{
																		#region Continue
																		case "a":// Continue Feast

																			var eventDescription2rq = new TextObject("{=Feast_Event_Description2rq}[Bad Relation] A few minutes of careful word choice leads you to hint at the idea of supporting {targetLord} in {genderADJ} political agenda." +
																				" Although you think it's foolish, you just wish {genderSUB} would quiet down a bit. The feast continues for another couple hours until all the cheap wine has been drank.")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.SetTextVariable("genderADJ", targetLordGenderAdjective)
																			.SetTextVariable("genderSUB", targetLordGenderSubjective)
																			.ToString();

																			ChangeRelationAction.ApplyPlayerRelation(targetLord, 2);
																			Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 20);
																			var eventMsgCharm2a = new TextObject(
																				"{=Feast_Event_Msg_End}The feast has ended.")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.ToString();
																			InformationManager.DisplayMessage(new InformationMessage(eventMsgCharm2a, RandomEventsSubmodule.Msg_Color));

																			InformationManager.ShowInquiry(new InquiryData(eventTitle, eventDescription2rq, true, false, eventButtonText2, null, null, null), true);

																			StopEvent();

																			break;
																		#endregion

																		#region Leave
																		case "b": // Leave

																			var eventDescription3rq = new TextObject("{=Feast_Event_Description3rq}[Bad Relation] A few minutes of careful word choice leads to {title} {targetLord} feeling quite impressed by your position. Finally, the cheap wine" +
																				" has come to an end and on that note you declare the feast is finished, giving {targetLord} a formal farewell before leaving the keep.")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.SetTextVariable("title", targetLordGenderTitle)
																			.ToString();

																			ChangeRelationAction.ApplyPlayerRelation(targetLord, 1);
																			Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 20);
																			var eventMsgCharm2 = new TextObject(
																				"{=Feast_Event_Msg_End}The feast has ended.")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.ToString();
																			InformationManager.DisplayMessage(new InformationMessage(eventMsgCharm2, RandomEventsSubmodule.Msg_Color));

																			InformationManager.ShowInquiry(new InquiryData(eventTitle, eventDescription3rq, true, false, eventButtonText2, null, null, null), true);

																			StopEvent();

																			break;
																			#endregion
																	}
																}, null, null);

															MBInformationManager.ShowMultiSelectionInquiry(msid1dr, true);

															StopEvent();

															break;
														#endregion

														#region Finish Up
														case "d": // Finish Up


															var eventDescription4rq = new TextObject("{=Feast_Event_Description4rq}[Bad Relation] After what feels like an eternity of political discourse, you decide it's time to call this feast to an end. {title} {targetLord} seems" +
																" quite displeased with your lack of support and hopes to leave {settlement} as quickly as possible.")
															.SetTextVariable("targetLord", targetLord.Name)
															.SetTextVariable("settlement", currentSettlement)
															.SetTextVariable("title", targetLordGenderTitle)
															.ToString();

															ChangeRelationAction.ApplyPlayerRelation(targetLord, 1);
															Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 20);

															var eventMsgEnd = new TextObject(
															"{=Feast_Event_Msg_End}The feast has ended.").ToString();
															InformationManager.DisplayMessage(new InformationMessage(eventMsgEnd, RandomEventsSubmodule.Msg_Color));

															InformationManager.ShowInquiry(new InquiryData(eventTitle, eventDescription4rq, true, false, eventButtonText2, null, null, null), true);

															StopEvent();

															break;
															#endregion
													}
												}, null, null);

											MBInformationManager.ShowMultiSelectionInquiry(msid1b, true);

											StopEvent();
											break;

										#endregion

										#region End Feast
										case "b": //End Feast-------------

											var eventDescriptionEndFeast2 = new TextObject("{=Feast_Event_DescriptionEndFeast2}[Bad Relation] After finishing your meals you declare this feast come to an end. {title} {targetLord} seems displeased by your lack" +
												" of support and wishes to leave {settlement} as quickly as possible.")
											.SetTextVariable("targetLord", targetLord.Name)
											.SetTextVariable("settlement", currentSettlement)
											.SetTextVariable("title", targetLordGenderTitle)
											.ToString();

											//Hero.MainHero.SetPersonalRelation(targetLord, +1);
											Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 10);
											ChangeRelationAction.ApplyPlayerRelation(targetLord, 1);
											var eventMsgEnd = new TextObject(
												"{=Feast_Event_Msg_End}The feast has ended.")
											.SetTextVariable("targetLord", targetLord.Name)
											.ToString();
											InformationManager.DisplayMessage(new InformationMessage(eventMsgEnd, RandomEventsSubmodule.Msg_Color));

											InformationManager.ShowInquiry(new InquiryData(eventTitle, eventDescriptionEndFeast2, true, false, eventButtonText2, null, null, null), true);

											StopEvent();

											break;
											#endregion
									}
								}, null, null);

								MBInformationManager.ShowMultiSelectionInquiry(msid1a, true);

								StopEvent();

							}
							#endregion

							#region Neutral Relation
							else
							{
								var eventDescription1n = new TextObject("{=Feast_Event_Description1n}You arrive at your keep in {settlement} where {title} {targetLord} is waiting. Introductions are made and civil norms are met as per tradition before" +
								" finally taking a seat at the table. {targetLord} admires your work here in {settlement}, noting the acceptable condition of the keep, and gives thanks for " +
								"allowing {gender} to stay in such a fine area. A few minutes pass and small talk turns to laughter as you both discuss recent engagements and share stories of your travels. \n \n" +
								"As the food finally arrives the both of you dig in.")
								.SetTextVariable("targetLord", targetLord.Name)
								.SetTextVariable("settlement", currentSettlement)
								.SetTextVariable("gender", targetLordGenderObjective)
								.SetTextVariable("genderSUB", targetLordGenderSubjective)
								.SetTextVariable("title", targetLordGenderTitle)
								.ToString();

								#region Inquiry Elements 2
								//option A ---- Continue ----
								var eventOption1u = new TextObject("{=Feast_Event_Option_1a}Continue Feast").ToString();
								var eventOption1uHover = new TextObject("{=Feast_Event_Option_1a_Hover}Let's continue..").ToString();
								//option D ---- End ----
								var eventOption2a = new TextObject("{=Feast_Event_Option_2a}End Feast").ToString();
								var eventOption2aHover = new TextObject("{=Feast_Event_Option_2a_Hover}This is a good time to stop").ToString();



								var inquiryElements2 = new List<InquiryElement>
								{
									new InquiryElement("a", eventOption1u, null, true, eventOption1uHover),
										//new InquiryElement("b", eventOption2a, null, true, eventOption2aHover),
										//new InquiryElement("c", eventOption3a, null, true, eventOption3aHover),
									new InquiryElement("b", eventOption2a, null, true, eventOption2aHover),
								};
								#endregion

								var msid1a = new MultiSelectionInquiryData(eventTitle, eventDescription1n, inquiryElements2, false, 1, 1, eventButtonText1, null,
								elements =>
								{
									switch ((string)elements[0].Identifier)
									{
										#region Continue Feast 
										case "a": //Continue Feast ---------------------------


											var eventDescription2n = new TextObject("{=Feast_Event_Description2n}After an enjoyable meal {targetLord} requests a new bottle of wine for the two of you to wash it all down. You signal" +
												" a servant to bring a bottle from the cellar. {targetLord} begins questioning your stance on the current political climate, you answer vaguely as to not stir up any controversy. Playing off" +
												" of {targetLord}'s ideals more than your own. Between the civil discourse lay harmless jokes of noble affairs, you both share a laugh and continue drinking wine.")
											.SetTextVariable("targetLord", targetLord.Name)
											.SetTextVariable("gender", targetLordGenderAdjective)
											.SetTextVariable("title", targetLordGenderTitle)
											.ToString();

											Hero.MainHero.SetPersonalRelation(targetLord, +1);
											Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 2);

											#region Inquiry Elements 3
											//option D ---- End ----
											var eventOption2b = new TextObject("{=Feast_Event_Option_2b}Finish Feast").ToString();
											var eventOption2bHover = new TextObject("{=Feast_Event_Option_2b_Hover}Time to finish up").ToString();
											//option A ---- Skill: Charm different sex----
											var eventOption3b = new TextObject("{=Feast_Event_Option_3b}[Charm] Compliment").ToString();
											var eventOption3bHover = new TextObject("{=Feast_Event_Option_3b_Hover}Charm {gender} a little \n[Charm - lvl 120]")
											.SetTextVariable("gender", targetLordGenderObjective)
											.ToString();
											//option B ---- Skill: Charm same sex----
											var eventOption4b = new TextObject("{=Feast_Event_Option_4b}[Charm] Boast").ToString();
											var eventOption4bHover = new TextObject("{=Feast_Event_Option_4b_Hover}Boast their ego a bit \n[Charm - lvl 120]").ToString();
											//option C ---- Skill: Rogue ----
											var eventOption5b = new TextObject("{=Feast_Event_Option_5b}[Roguery] Theft").ToString();
											var eventOption5bHover = new TextObject("{=Feast_Event_Option_5b_Hover}Quite the coin purse.. \n[Roguery - lvl 60]")
											.SetTextVariable("gender", targetLordGenderSubjective)
											.ToString();

											var inquiryElements3 = new List<InquiryElement>();
											if (charmedNoble)
											{
												if (diffGender)
												{
													inquiryElements3.Add(new InquiryElement("a", eventOption3b, null, true, eventOption3bHover));
												}
												else if (diffGender == false)
												{
													inquiryElements3.Add(new InquiryElement("b", eventOption4b, null, true, eventOption4bHover));
												}
											}
											if (theftSuccess)
											{
												inquiryElements3.Add(new InquiryElement("c", eventOption5b, null, true, eventOption5bHover));
											}
											inquiryElements3.Add(new InquiryElement("d", eventOption2b, null, true, eventOption2bHover));


											#endregion

											var msid1b = new MultiSelectionInquiryData(eventTitle, eventDescription2n, inquiryElements3, false, 1, 1, eventButtonText1, null,
												elements =>
												{
													switch ((string)elements[0].Identifier)
													{
														#region Charm Diff Sex
														case "a":// CHARM different sex

															var eventDescription3n = new TextObject("{=Feast_Event_Description3n}As the feast goes on it seems there is quite a sense of attraction." +
																" Perhaps it's the wine, or perhaps you've never looked at {targetLord} this way, regardless.. You notice a beauty in {genderOBJ} you haven't been aware of before, something about {gender} eyes are " +
																"pulling you in. As you smile {gender} eyes meet yours and {genderSUB} gives you a little wink. You both know what is happening here..")
															.SetTextVariable("targetLord", targetLord.Name)
															.SetTextVariable("gender", targetLordGenderAdjective)
															.SetTextVariable("genderSUB", targetLordGenderSubjective)
															.SetTextVariable("genderOBJ", targetLordGenderObjective)
															.ToString();

															ChangeRelationAction.ApplyPlayerRelation(targetLord, 5);
															Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 10);
															var eventMsgCharm1 = new TextObject(
																"{=Feast_Event_Msg_Charm1}You charm {targetLord}.")
															.SetTextVariable("targetLord", targetLord.Name)
															.ToString();
															InformationManager.DisplayMessage(new InformationMessage(eventMsgCharm1, RandomEventsSubmodule.Msg_Color));

															#region Inquiry Elements
															//option A ---- Hook up ----
															var eventOption2uc = new TextObject("{=Feast_Event_Option_2uc}[Charm] Go somewhere private").ToString();
															var eventOption2nHover = new TextObject("{=Feast_Event_Option_2n_Hover}Show {gender} around.. \n[Charm - lvl 210]")
															.SetTextVariable("gender", targetLordGenderObjective)
															.ToString();
															//option B ---- End ----
															var eventOption2c = new TextObject("{=Feast_Event_Option_2c}End Feast").ToString();
															var eventOption2cHover = new TextObject("{=Feast_Event_Option_2c_Hover}Let's not get carried away..").ToString();

															var inquiryElements4 = new List<InquiryElement>();
															if (charmedNoble4)
															{
																inquiryElements4.Add(new InquiryElement("a", eventOption2uc, null, true, eventOption2nHover));
															}
															inquiryElements4.Add(new InquiryElement("b", eventOption2c, null, true, eventOption2cHover));
															#endregion

															var msid1c = new MultiSelectionInquiryData(eventTitle, eventDescription3n, inquiryElements4, false, 1, 1, eventButtonText1, null,
																elements =>
																{
																	switch ((string)elements[0].Identifier)
																	{
                                                                        #region Hook up
                                                                        case "a":// Proceed to hook up

																			var eventDescription4n = new TextObject("{=Feast_Event_Description4n}You stand from your chair and offer your hand to {title} {targetLord}, {genderSUB} accepts and raises as well." +
																				" The both of you walk into the other room and aren't seen again for a couple hours..")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.SetTextVariable("genderSUB", targetLordGenderSubjective)
																			.SetTextVariable("title", targetLordGenderTitle)
																			.ToString();

																			ChangeRelationAction.ApplyPlayerRelation(targetLord, 10);
																			Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 50);
																			var eventMsgCharm2 = new TextObject(
																				"{=Feast_Event_Msg_Charm2}You and {targetLord} have a great time.")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.ToString();
																			InformationManager.DisplayMessage(new InformationMessage(eventMsgCharm2, RandomEventsSubmodule.Msg_Color));

																			InformationManager.ShowInquiry(new InquiryData(eventTitle, eventDescription4n, true, false, eventButtonText2, null, null, null), true);

																			StopEvent();

																			break;
                                                                        #endregion

                                                                        #region Leave
                                                                        case "b": // Leave

																			var eventDescription5n = new TextObject("{=Feast_Event_Description5n}Although the tension is more than enough to write a story of its own - you decide it's best for the two of you to" +
																				" go your separate ways, for now.. {title} {targetLord} looks quite pleased with the feast, and hopes to stay in {settlement} for a while.")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.SetTextVariable("settlement", currentSettlement)
																			.SetTextVariable("title", targetLordGenderTitle)
																			.ToString();

																			ChangeRelationAction.ApplyPlayerRelation(targetLord, 2);
																			Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 10);
																			var eventMsgEnd = new TextObject(
																				"{=Feast_Event_Msg_End}The feast has ended.")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.ToString();
																			InformationManager.DisplayMessage(new InformationMessage(eventMsgEnd, RandomEventsSubmodule.Msg_Color));

																			InformationManager.ShowInquiry(new InquiryData(eventTitle, eventDescription5n, true, false, eventButtonText2, null, null, null), true);

																			StopEvent();

																			break;
																			#endregion

																	}
																}, null, null);

															MBInformationManager.ShowMultiSelectionInquiry(msid1c, true);

															StopEvent();


															break;
														#endregion

														#region Charm Same Sex
														case "b": // CHARM same sex

															var eventDescription1rn = new TextObject("{=Feast_Event_Description1rn}After a few laughs {title} {targetLord} requests another glass of wine, you accept another as well. With the mood set you recall the news of " +
																"{targetLord}'s recent achievements, noting there has been quite a lot of talk lately in regards. {genderSUBCAP} can't help but blush, trying to play it off as if it's no big deal. Needless to say {genderSUB} finds " +
																"great pleasure in knowing {genderADJ} name has spread throughout the land.")
															.SetTextVariable("targetLord", targetLord.Name)
															.SetTextVariable("genderSUB", targetLordGenderSubjective)
															.SetTextVariable("genderSUBCAP", targetLordGenderSubjectiveCAP)
															.SetTextVariable("genderADJ", targetLordGenderAdjective)
															.SetTextVariable("title", targetLordGenderTitle)
															.SetTextVariable("settlement", currentSettlement)
															.ToString();

															ChangeRelationAction.ApplyPlayerRelation(targetLord, 5);
															var eventMsgCharm2 = new TextObject(
																"{=Feast_Event_Msg_Charm1}You charm {targetLord}")
																.SetTextVariable("targetLord", targetLord.Name)
																.ToString();
															InformationManager.DisplayMessage(new InformationMessage(eventMsgCharm2, RandomEventsSubmodule.Msg_Color));

															#region Inquiry Elements
															//option A ---- Continue ----
															var eventOption1ru = new TextObject("{=Feast_Event_Option_1r}Continue Feast").ToString();
															var eventOption1ruHover = new TextObject("{=Feast_Event_Option_1u_Hover}Let's continue..")
															.ToString();
															//option B ---- End ----
															var eventOption2r = new TextObject("{=Feast_Event_Option_2r}End Feast").ToString();
															var eventOption2rHover = new TextObject("{=Feast_Event_Option_2r_Hover}This is a good time to stop").ToString();

															var inquiryElements5 = new List<InquiryElement>
															{
																new InquiryElement("a", eventOption1ru, null, true, eventOption1ruHover),
																new InquiryElement("b", eventOption2r, null, true, eventOption2rHover),
															};
															#endregion

															var msid1d = new MultiSelectionInquiryData(eventTitle, eventDescription1rn, inquiryElements5, false, 2, 2, eventButtonText1, null,
																elements =>
																{
																	switch ((string)elements[0].Identifier)
																	{
																		#region Continue
																		case "a":// Continue Feast

																			var eventDescription2rn = new TextObject("{=Feast_Event_Description2rn}A few minutes of careful boasting leads to {title} {targetLord} feeling quite impressed by your words. {genderSUB} confesses" +
																				" that your name has also been making its way around the noble tables. You smile and this civil discourse continues for a few hours until the wine runs dry. You offer to take {targetLord} around " +
																				"{settlement} for a full tour, to which they accept.")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.SetTextVariable("genderSUB", targetLordGenderSubjective)
																			.SetTextVariable("title", targetLordGenderTitle)
																			.SetTextVariable("settlement", currentSettlement)
																			.ToString();

																			//var currentRelationship = targetLord.GetRelation(Hero.MainHero);
																			//var newRelationship = (currentRelationship + 5);
																			//CharacterRelationManager.SetHeroRelation(targetLord, Hero.MainHero, newRelationship);

																			ChangeRelationAction.ApplyPlayerRelation(targetLord, 5);
																			Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 20);
																			var eventMsgCharm2 = new TextObject(
																				"{=Feast_Event_Msg_Charm2}You and {targetLord} have a great time.")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.ToString();
																			InformationManager.DisplayMessage(new InformationMessage(eventMsgCharm2, RandomEventsSubmodule.Msg_Color));

																			InformationManager.ShowInquiry(new InquiryData(eventTitle, eventDescription2rn, true, false, eventButtonText2, null, null, null), true);

																			StopEvent();

																			break;
																		#endregion

																		#region Leave
																		case "b": // Leave

																			var eventDescription3rn = new TextObject("{=Feast_Event_Description3rn}A few minutes of careful boasting leads to {title} {targetLord} feeling quite impressed by your words. {genderSUB} confesses" +
																				" that your name has also been making its way around the noble tables. With that, you declare the feast finished, to which {title} {targetLord} gives a formal farewell before leaving the keep.")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.SetTextVariable("genderSUB", targetLordGenderSubjective)
																			.SetTextVariable("genderOBJ", targetLordGenderObjective)
																			.SetTextVariable("gender", targetLordGenderAdjective)
																			.SetTextVariable("title", targetLordGenderTitle)
																			.SetTextVariable("settlement", currentSettlement)
																			.ToString();

																			ChangeRelationAction.ApplyPlayerRelation(targetLord, 2);
																			Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 10);
																			var eventMsgEnd = new TextObject(
																				"{=Feast_Event_Msg_End}The feast has ended.")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.ToString();
																			InformationManager.DisplayMessage(new InformationMessage(eventMsgEnd, RandomEventsSubmodule.Msg_Color));

																			InformationManager.ShowInquiry(new InquiryData(eventTitle, eventDescription3rn, true, false, eventButtonText2, null, null, null), true);

																			StopEvent();

																			break;
																			#endregion
																	}
																}, null, null);

															MBInformationManager.ShowMultiSelectionInquiry(msid1d, true);

															StopEvent();
															break;
														#endregion

														#region Rogue
														case "c": // ROGUE

															var eventDescription1rm = new TextObject("{=Feast_Event_Description1rm}With the mood set, you signal one of your servants to bring more wine and make a scene. As the wine arrives your" +
																" servant trips and drops the bottle onto the table, spilling it into {targetLord}'s lap. You react quickly, grabbing a cloth from the table and reaching towards {genderOBJ}, attempting to wipe away the " +
																"still running wine from {genderADJ} shirt. When the moment is right you swipe {genderADJ} coin purse without {genderOBJ} ever noticing.")
															.SetTextVariable("targetLord", targetLord.Name)
															.SetTextVariable("genderADJ", targetLordGenderAdjective)
															.SetTextVariable("genderOBJ", targetLordGenderObjective)
															.ToString();

															var MoneyStole = MBRandom.RandomInt(1000, 5000);
															Hero.MainHero.ChangeHeroGold(+MoneyStole);
															Hero.MainHero.AddSkillXp(DefaultSkills.Roguery, 50);
															var eventMsgRogue = new TextObject(
																"{=Feast_Event_Msg_Rogue}You steal the coin purse.").ToString();
															InformationManager.DisplayMessage(new InformationMessage(eventMsgRogue, RandomEventsSubmodule.Msg_Color));

															#region Inquiry Elements
															//option A ---- Continue ----
															var eventOption1rq = new TextObject("{=Feast_Event_Option_1r}Continue Feast").ToString();
															var eventOption1rqHover = new TextObject("{=Feast_Event_Option_1u_Hover}Let's continue..")
															.ToString();
															//option B ---- End ----
															var eventOption2rr = new TextObject("{=Feast_Event_Option_2r}End Feast").ToString();
															var eventOption2rrHover = new TextObject("{=Feast_Event_Option_2r_Hover}This is a good time to stop").ToString();

															var inquiryElements5r = new List<InquiryElement>
															{
																new InquiryElement("a", eventOption1rq, null, true, eventOption1rqHover),
																new InquiryElement("b", eventOption2rr, null, true, eventOption2rrHover),
															};
															#endregion

															var msid1dr = new MultiSelectionInquiryData(eventTitle, eventDescription1rm, inquiryElements5r, false, 2, 2, eventButtonText1, null,
																elements =>
																{
																	switch ((string)elements[0].Identifier)
																	{
																		#region Continue
																		case "a":// Continue Feast

																			var eventDescription2rm = new TextObject("{=Feast_Event_Description2rm}After cleaning the wine you offer {targetLord} another glass and the two of you continue talking about all sorts of interesting " +
																				"topics ranging from politics, to war, to the arena. You even manage to learn a thing or two in regards to stewardship. With the wine running low you offer to take {title} {targetLord} on a tour of {settlement}," +
																				"to which {genderSUB} accepts.")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.SetTextVariable("genderSUB", targetLordGenderSubjective)
																			.ToString();

																			ChangeRelationAction.ApplyPlayerRelation(targetLord, 5);
																			Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 20);
																			Hero.MainHero.AddSkillXp(DefaultSkills.Steward, 50);
																			var eventMsgCharm2 = new TextObject(
																				"{=Feast_Event_Msg_Charm2}You and {targetLord} have a great time.")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.ToString();
																			InformationManager.DisplayMessage(new InformationMessage(eventMsgCharm2, RandomEventsSubmodule.Msg_Color));

																			InformationManager.ShowInquiry(new InquiryData(eventTitle, eventDescription2rm, true, false, eventButtonText2, null, null, null), true);

																			StopEvent();

																			break;
																		#endregion

																		#region Leave
																		case "b": // Leave

																			var eventDescription3rm = new TextObject("{=Feast_Event_Description3rm}A few minutes of careful word choice leads to {title} {targetLord} feeling quite impressed by your position. Finally, the cheap wine" +
																				" has come to an end and on that note you declare the feast is finished, giving {targetLord} a formal farewell before leaving the keep.")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.SetTextVariable("title", targetLordGenderTitle)
																			.ToString();

																			ChangeRelationAction.ApplyPlayerRelation(targetLord, 2);
																			Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 10);
																			var eventMsgEnd = new TextObject(
																				"{=Feast_Event_Msg_End}The feast has ended.")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.ToString();
																			InformationManager.DisplayMessage(new InformationMessage(eventMsgEnd, RandomEventsSubmodule.Msg_Color));

																			InformationManager.ShowInquiry(new InquiryData(eventTitle, eventDescription3rm, true, false, eventButtonText2, null, null, null), true);

																			StopEvent();

																			break;
																			#endregion
																	}
																}, null, null);

															MBInformationManager.ShowMultiSelectionInquiry(msid1dr, true);

															StopEvent();

															break;
														#endregion

														#region Finish Up
														case "d": // Finish Up


															var eventDescription4rm = new TextObject("{=Feast_Event_Description4rm}As the wine comes to an end, you decide it's time to call this feast to a close. {title} {targetLord} seems" +
																" quite pleased with your excellent meal and hopes to stay in {settlement} for a while.")
															.SetTextVariable("targetLord", targetLord.Name)
															.SetTextVariable("settlement", currentSettlement)
															.SetTextVariable("title", targetLordGenderTitle)
															.ToString();

															ChangeRelationAction.ApplyPlayerRelation(targetLord, 1);
															Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 10);
															var eventMsgEnd = new TextObject(
																"{=Feast_Event_Msg_End}The feast has ended.")
															.SetTextVariable("targetLord", targetLord.Name)
															.ToString();
															InformationManager.DisplayMessage(new InformationMessage(eventMsgEnd, RandomEventsSubmodule.Msg_Color));

															InformationManager.ShowInquiry(new InquiryData(eventTitle, eventDescription4rm, true, false, eventButtonText2, null, null, null), true);

															StopEvent();

															break;
															#endregion
													}
												}, null, null);

											MBInformationManager.ShowMultiSelectionInquiry(msid1b, true);

											StopEvent();
											break;

										#endregion

										#region End Feast
										case "b": //End Feast-------------

											var eventDescriptionEndFeast3 = new TextObject("{=Feast_Event_DescriptionEndFeast3}After finishing your meals you declare this feast come to an end. {title} {targetLord} seems pleased by your acceptable" +
												" wine and wishes to stay in {settlement} for a while.")
											.SetTextVariable("targetLord", targetLord.Name)
											.SetTextVariable("settlement", currentSettlement)
											.SetTextVariable("title", targetLordGenderTitle)
											.ToString();

											ChangeRelationAction.ApplyPlayerRelation(targetLord, 1);
											Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 10);
											var eventMsgEnd = new TextObject(
												"{=Feast_Event_Msg_End}The feast has ended.")
											.SetTextVariable("targetLord", targetLord.Name)
											.ToString();
											InformationManager.DisplayMessage(new InformationMessage(eventMsgEnd, RandomEventsSubmodule.Msg_Color));

											InformationManager.ShowInquiry(new InquiryData(eventTitle, eventDescriptionEndFeast3, true, false, eventButtonText2, null, null, null), true);

											StopEvent();

											break;
											#endregion
									}
								}, null, null);

								MBInformationManager.ShowMultiSelectionInquiry(msid1a, true);

								StopEvent();
							}
							#endregion

						}
						#endregion

						#region Player NOT Settlement
						//If Player Owns Settlement _____________________________
						if (Settlement.CurrentSettlement.Owner != Hero.MainHero)
						{

							#region Good Relation
							if (relation > 34)
							{
								var eventDescription1v = new TextObject("{=Feast_Event_Description1v}[Good Relation] You arrive at the keep in {settlement} where {title} {targetLord} is waiting. Introductions are made and civil norms are met as per tradition before" +
									" finally taking a seat at the table, the sweet smell from the other room heightens your appetite. A few minutes pass and small talk soon turns to laughter as you both discuss recent engagements and share grand stories matched only by that of" +
									" myths and legends. \n \n As the food finally arrives no time is wasted - both of you dig in to what can only be described as a meal fit for a king.")
								.SetTextVariable("targetLord", targetLord.Name)
								.SetTextVariable("settlement", currentSettlement)
								.SetTextVariable("gender", targetLordGenderObjective)
								.SetTextVariable("title", targetLordGenderTitle)
								.ToString();


								#region Inquiry Elements 2
								//option A ---- Continue ----
								var eventOption1a = new TextObject("{=Feast_Event_Option_1a}Continue Feast").ToString();
								var eventOption1aHover = new TextObject("{=Feast_Event_Option_1a_Hover}We're having a great time").ToString();
								//option D ---- End ----
								var eventOption2a = new TextObject("{=Feast_Event_Option_2a}End Feast").ToString();
								var eventOption2aHover = new TextObject("{=Feast_Event_Option_2a_Hover}This is a good time to stop").ToString();



								var inquiryElements2 = new List<InquiryElement>
								{
									new InquiryElement("a", eventOption1a, null, true, eventOption1aHover),
										//new InquiryElement("b", eventOption2a, null, true, eventOption2aHover),
										//new InquiryElement("c", eventOption3a, null, true, eventOption3aHover),
									new InquiryElement("b", eventOption2a, null, true, eventOption2aHover),
								};
								#endregion

								var msid1a = new MultiSelectionInquiryData(eventTitle, eventDescription1v, inquiryElements2, false, 1, 1, eventButtonText1, null,
								elements =>
								{
									switch ((string)elements[0].Identifier)
									{
										#region Continue Feast 
										case "a": //Continue Feast ---------------------------


											var eventDescription2v = new TextObject("{=Feast_Event_Description2v}[Good Relation] After such a fantastic meal {targetLord} signals to a servant a new bottle of their finest wine for the two of you to wash it all down" +
												", as the occasion calls for nothing less. {targetLord} recalls {gender} most recent experience at the arena when {gender} friend drank so much they fell over the" +
												" wall and straight into the pit. But instead of crawling out, they decided to join in the fight! You both laugh hysterically, sharing nostalgic memories of times not forgotten.")
											.SetTextVariable("targetLord", targetLord.Name)
											.SetTextVariable("gender", targetLordGenderAdjective)
											.SetTextVariable("title", targetLordGenderTitle)
											.ToString();

											ChangeRelationAction.ApplyPlayerRelation(targetLord, 2);
											Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 2);

											#region Inquiry Elements 3
											//option D ---- End ----
											var eventOption2b = new TextObject("{=Feast_Event_Option_2b}Finish Feast").ToString();
											var eventOption2bHover = new TextObject("{=Feast_Event_Option_2b_Hover}Time to finish up").ToString();
											//option A ---- Skill: Charm different sex----
											var eventOption3b = new TextObject("{=Feast_Event_Option_3b}[Charm] Compliment").ToString();
											var eventOption3bHover = new TextObject("{=Feast_Event_Option_3b_Hover}Charm {gender} a little \n[Charm - lvl 120]")
											.SetTextVariable("gender", targetLordGenderObjective)
											.ToString();
											//option B ---- Skill: Charm same sex----
											var eventOption4b = new TextObject("{=Feast_Event_Option_4b}[Charm] Boast").ToString();
											var eventOption4bHover = new TextObject("{=Feast_Event_Option_4b_Hover}Boast their ego a bit \n[Charm - lvl 120]").ToString();
											//option C ---- Skill: Rogue ----
											var eventOption5b = new TextObject("{=Feast_Event_Option_5b}[Roguery] Theft").ToString();
											var eventOption5bHover = new TextObject("{=Feast_Event_Option_5b_Hover}Quite the coin purse.. \n[Roguery - lvl 60]")
											.SetTextVariable("gender", targetLordGenderSubjective)
											.ToString();

											var inquiryElements3 = new List<InquiryElement>();
											if (charmedNoble)
											{
												if (diffGender)
												{
													inquiryElements3.Add(new InquiryElement("a", eventOption3b, null, true, eventOption3bHover));
												}
												else if (diffGender == false)
												{
													inquiryElements3.Add(new InquiryElement("b", eventOption4b, null, true, eventOption4bHover));
												}
											}
											if (theftSuccess)
											{
												inquiryElements3.Add(new InquiryElement("c", eventOption5b, null, true, eventOption5bHover));
											}
											inquiryElements3.Add(new InquiryElement("d", eventOption2b, null, true, eventOption2bHover));


											#endregion

											var msid1b = new MultiSelectionInquiryData(eventTitle, eventDescription2v, inquiryElements3, false, 1, 1, eventButtonText1, null,
												elements =>
												{
													switch ((string)elements[0].Identifier)
													{
														#region Charm Diff Sex
														case "a":// CHARM different sex

															var eventDescription1c = new TextObject("{=Feast_Event_Description1c}[Good Relation] Needless to say, the wine has made its way through the both of you and it seems there is quite a sense of attraction." +
																" You notice a beauty in {targetLord} you haven't been aware of before, something about {gender} eyes are pulling you in. As you smile {gender} eyes meet yours and {genderSUB} gives you a little" +
																" wink. You both know what is happening here..")
															.SetTextVariable("targetLord", targetLord.Name)
															.SetTextVariable("gender", targetLordGenderAdjective)
															.SetTextVariable("genderSUB", targetLordGenderSubjective)
															.ToString();

															ChangeRelationAction.ApplyPlayerRelation(targetLord, 5);
															Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 10);
															var eventMsgCharm1 = new TextObject(
																"{=Feast_Event_Msg_Charm1}You charm {targetLord}.")
															.SetTextVariable("targetLord", targetLord.Name)
															.ToString();
															InformationManager.DisplayMessage(new InformationMessage(eventMsgCharm1, RandomEventsSubmodule.Msg_Color));

															#region Inquiry Elements
															//option A ---- Hook up ----
															var eventOption1c = new TextObject("{=Feast_Event_Option_1c}[Charm] Go somewhere private").ToString();
															var eventOption1cHover = new TextObject("{=Feast_Event_Option_1c_Hover}Show {gender} around.. \n[Charm - lvl 150]")
															.SetTextVariable("gender", targetLordGenderObjective)
															.ToString();
															//option B ---- End ----
															var eventOption2c = new TextObject("{=Feast_Event_Option_2c}End Feast").ToString();
															var eventOption2cHover = new TextObject("{=Feast_Event_Option_2c_Hover}Let's not get carried away..").ToString();

															var inquiryElements4 = new List<InquiryElement>();
															if (charmedNoble2)
															{
																inquiryElements4.Add(new InquiryElement("a", eventOption1c, null, true, eventOption1cHover));
															}
															inquiryElements4.Add(new InquiryElement("b", eventOption2c, null, true, eventOption2cHover));
															#endregion

															var msid1c = new MultiSelectionInquiryData(eventTitle, eventDescription1c, inquiryElements4, false, 1, 1, eventButtonText1, null,
																elements =>
																{
																	switch ((string)elements[0].Identifier)
																	{
                                                                        #region Hook up
                                                                        case "a":// Proceed to hook up

																			var eventDescription1d = new TextObject("{=Feast_Event_Description1d}[Good Relation] You stand from your chair and offer your hand to {title} {targetLord}, {genderSUB} accepts and raises as well." +
																				" The both of you walk into the other room and aren't seen again for a couple hours..")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.SetTextVariable("genderSUB", targetLordGenderSubjective)
																			.SetTextVariable("title", targetLordGenderTitle)
																			.ToString();

																			ChangeRelationAction.ApplyPlayerRelation(targetLord, 10);
																			Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 50);
																			var eventMsgCharm2 = new TextObject(
																				"{=Feast_Event_Msg_Charm2}You and {targetLord} have a great time.")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.ToString();
																			InformationManager.DisplayMessage(new InformationMessage(eventMsgCharm2, RandomEventsSubmodule.Msg_Color));


																			InformationManager.ShowInquiry(new InquiryData(eventTitle, eventDescription1d, true, false, eventButtonText2, null, null, null), true);

																			StopEvent();

																			break;
                                                                        #endregion

                                                                        #region Leave
                                                                        case "b": // Leave

																			var eventDescription3v = new TextObject("{=Feast_Event_Description3v}[Good Relation] Although the tension is more than enough to write a story of its own - you decide it's best for the two of you to" +
																				" go your separate ways, for now.. {title} {targetLord} thanks you for joining them in such an exquisite meal and looks forward to seeing you in {settlement} at a later time.")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.SetTextVariable("settlement", currentSettlement)
																			.SetTextVariable("title", targetLordGenderTitle)
																			.ToString();

																			ChangeRelationAction.ApplyPlayerRelation(targetLord, 2);
																			Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 10);
																			var eventMsgEnd = new TextObject(
																				"{=Feast_Event_Msg_End}The feast has ended.")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.ToString();
																			InformationManager.DisplayMessage(new InformationMessage(eventMsgEnd, RandomEventsSubmodule.Msg_Color));

																			InformationManager.ShowInquiry(new InquiryData(eventTitle, eventDescription3v, true, false, eventButtonText2, null, null, null), true);

																			StopEvent();

																			break;
                                                                            #endregion
                                                                    }
                                                                }, null, null);

															MBInformationManager.ShowMultiSelectionInquiry(msid1c, true);

															StopEvent();


															break;
														#endregion

														#region Charm Same Sex
														case "b": // CHARM same sex

															var eventDescription4v = new TextObject("{=Feast_Event_Description4v}[Good Relation] A few more minutes of joyful talk go by and you recall {title} {targetLord}'s recent political feats. Claiming word of such events have made their" +
																" way all the way to the farthest corners of Calradia. {genderSUBCAP} looks at you with a gleeful smile like that of a small child, then erupts once again into wine induced laughter as {genderSUB} proceeds to tell you all about the " +
																"event in great detail.  Starting from the very beginning, of course..")
															.SetTextVariable("targetLord", targetLord.Name)
															.SetTextVariable("genderSUB", targetLordGenderSubjective)
															.SetTextVariable("genderSUBCAP", targetLordGenderSubjectiveCAP)
															.SetTextVariable("title", targetLordGenderTitle)
															.SetTextVariable("settlement", currentSettlement)
															.ToString();

															ChangeRelationAction.ApplyPlayerRelation(targetLord, 5);
															Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 10);
															var eventMsgCharm1a = new TextObject(
																"{=Feast_Event_Msg_Charm1}You boast {targetLord}.")
															.SetTextVariable("targetLord", targetLord.Name)
															.ToString();
															InformationManager.DisplayMessage(new InformationMessage(eventMsgCharm1a, RandomEventsSubmodule.Msg_Color));

															#region Inquiry Elements
															//option A ---- Continue ----
															var eventOption1r = new TextObject("{=Feast_Event_Option_1r}Continue Feast").ToString();
															var eventOption1rHover = new TextObject("{=Feast_Event_Option_1r_Hover}I'm having a great time")
															.SetTextVariable("gender", targetLordGenderObjective)
															.ToString();
															//option B ---- End ----
															var eventOption2r = new TextObject("{=Feast_Event_Option_2r}End Feast").ToString();
															var eventOption2rHover = new TextObject("{=Feast_Event_Option_2r_Hover}This is a good time to stop").ToString();

															var inquiryElements5 = new List<InquiryElement>
															{
																new InquiryElement("a", eventOption1r, null, true, eventOption1rHover),
																new InquiryElement("b", eventOption2r, null, true, eventOption2rHover),
															};
															#endregion

															var msid1d = new MultiSelectionInquiryData(eventTitle, eventDescription4v, inquiryElements5, false, 2 ,2 , eventButtonText1, null,
																elements =>
																{
																	switch ((string)elements[0].Identifier)
																	{
																		#region Continue
																		case "a":// Continue Feast

																			var eventDescription2r = new TextObject("{=Feast_Event_Description2r}[Good Relation] After finishing yet another round of cheerful story telling, you insist on the two of you continuing this epic feast in a more lively" +
																				" fashion. {targetLord} says a few of {gender} friends are probably at the tavern and would love to meet you. The two of you head to the tavern and aren't seen again for a couple hours..")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.SetTextVariable("genderSUB", targetLordGenderSubjective)
																			.SetTextVariable("gender", targetLordGenderAdjective)
																			.SetTextVariable("title", targetLordGenderTitle)
																			.ToString();

																			ChangeRelationAction.ApplyPlayerRelation(targetLord, 3);
																			Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 20);
																			var eventMsgCharm2 = new TextObject(
																				"{=Feast_Event_Msg_Charm2}You and {targetLord} have a great time.")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.ToString();
																			InformationManager.DisplayMessage(new InformationMessage(eventMsgCharm2, RandomEventsSubmodule.Msg_Color));

																			InformationManager.ShowInquiry(new InquiryData(eventTitle, eventDescription2r, true, false, eventButtonText2, null, null, null), true);

																			StopEvent();

																			break;
																		#endregion

																		#region Leave
																		case "b": // Leave

																			var eventDescription5v = new TextObject("{=Feast_Event_Description5v}[Good Relation] After finishing yet another round of cheerful story telling, {title} {targetLord} offers to make a toast in your honor, to which you accept. " +
																				"\n \n “To {Ptitle} {player}, an excellent leader, and an even better friend. May {playerSUB} live forever!”. \n \n {targetLord} then thanks you for joining them in such an exquisite meal and looks forward to seeing you in {settlement} " +
																				"at a later time. And with that, the feast comes to and end..")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.SetTextVariable("settlement", currentSettlement)
																			.SetTextVariable("title", targetLordGenderTitle)
																			.SetTextVariable("Ptitle", playerTitle)
																			.SetTextVariable("player", playerName)
																			.SetTextVariable("playerSUB", playerGenderSubjective)
																			.ToString();

																			ChangeRelationAction.ApplyPlayerRelation(targetLord, 1);
																			Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 10);
																			var eventMsgEnd = new TextObject(
																				"{=Feast_Event_Msg_End}The feast has ended.")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.ToString();
																			InformationManager.DisplayMessage(new InformationMessage(eventMsgEnd, RandomEventsSubmodule.Msg_Color));

																			InformationManager.ShowInquiry(new InquiryData(eventTitle, eventDescription5v, true, false, eventButtonText2, null, null, null), true);

																			StopEvent();

																			break;
																			#endregion
																	}
																}, null, null);

															MBInformationManager.ShowMultiSelectionInquiry(msid1d, true);

															StopEvent();
															break;
														#endregion

														#region Rogue
														case "c": // ROGUE

															var eventDescription6v = new TextObject("{=Feast_Event_Description6v}[Good Relation] Amidst the joyful laughter, you signal one of your men to bring a bottle of your own wine and make a scene. As the wine arrives" +
																" he trips and drops the bottle onto the table, spilling it into {targetLord}'s lap. You react quickly, grabbing a cloth from the table and reaching towards {genderOBJ}, attempting to wipe away the " +
																"still running wine from {genderADJ} shirt. When the moment is right you swipe {genderADJ} coin purse without {genderOBJ} ever noticing.")
															.SetTextVariable("targetLord", targetLord.Name)
															.SetTextVariable("genderADJ", targetLordGenderAdjective)
															.SetTextVariable("genderOBJ", targetLordGenderObjective)
															.SetTextVariable("genderSUB", targetLordGenderSubjective)
															.SetTextVariable("genderSUBCAP", targetLordGenderSubjectiveCAP)
															.SetTextVariable("title", targetLordGenderTitle)
															.SetTextVariable("settlement", currentSettlement)
															.ToString();

															var MoneyStole = MBRandom.RandomInt(1000, 5000);
															Hero.MainHero.ChangeHeroGold(+MoneyStole);
															Hero.MainHero.AddSkillXp(DefaultSkills.Roguery, 50);
															var eventMsgRogue = new TextObject(
																"{=Feast_Event_Msg_Rogue}You steal the coin purse.").ToString();
															InformationManager.DisplayMessage(new InformationMessage(eventMsgRogue, RandomEventsSubmodule.Msg_Color));

															#region Inquiry Elements
															//option A ---- Continue ----
															var eventOption1rr = new TextObject("{=Feast_Event_Option_1r}Continue Feast").ToString();
															var eventOption1rrHover = new TextObject("{=Feast_Event_Option_1r_Hover}I'm having a great time")
															.SetTextVariable("gender", targetLordGenderObjective)
															.ToString();
															//option B ---- End ----
															var eventOption2rr = new TextObject("{=Feast_Event_Option_2r}End Feast").ToString();
															var eventOption2rrHover = new TextObject("{=Feast_Event_Option_2r_Hover}This is a good time to stop").ToString();

															var inquiryElements5r = new List<InquiryElement>
															{
																new InquiryElement("a", eventOption1rr, null, true, eventOption1rrHover),
																new InquiryElement("b", eventOption2rr, null, true, eventOption2rrHover),
															};
															#endregion

															var msid1dr = new MultiSelectionInquiryData(eventTitle, eventDescription6v, inquiryElements5r, false, 2, 2, eventButtonText1, null,
																elements =>
																{
																	switch ((string)elements[0].Identifier)
																	{
																		#region Continue
																		case "a":// Continue Feast

																			var eventDescription2r = new TextObject("{=Feast_Event_Description2r}[Good Relation] After finishing yet another round of cheerful story telling, you insist on the two of you continuing this epic feast in a more lively" +
																				" fashion. {targetLord} says a few of {gender} friends are probably at the tavern and would love to meet you. The two of you head to the tavern and aren't seen again for a couple hours..")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.SetTextVariable("genderSUB", targetLordGenderSubjective)
																			.SetTextVariable("gender", targetLordGenderAdjective)
																			.SetTextVariable("title", targetLordGenderTitle)
																			.ToString();


																			ChangeRelationAction.ApplyPlayerRelation(targetLord, 3);
																			Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 20);
																			var eventMsgCharm2 = new TextObject(
																				"{=Feast_Event_Msg_Charm2}You and {targetLord} have a great time.")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.ToString();
																			InformationManager.DisplayMessage(new InformationMessage(eventMsgCharm2, RandomEventsSubmodule.Msg_Color));

																			InformationManager.ShowInquiry(new InquiryData(eventTitle, eventDescription2r, true, false, eventButtonText2, null, null, null), true);

																			StopEvent();

																			break;
																		#endregion

																		#region Leave
																		case "b": // Leave

																			var eventDescription7v = new TextObject("{=Feast_Event_Description7v}[Good Relation] After finishing yet another round of cheerful story telling, {title} {targetLord} offers to make a toast in your honor, to which you accept. " +
																				"\n \n “To {Ptitle} {player}, an excellent leader, and an even better friend. May {playerSUB} live forever!”. \n \n {targetLord} then thanks you for joining them in such an exquisite meal and looks forward to seeing you in {settlement} " +
																				"at a later time. And with that, the feast comes to and end..")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.SetTextVariable("settlement", currentSettlement)
																			.SetTextVariable("title", targetLordGenderTitle)
																			.SetTextVariable("Ptitle", playerTitle)
																			.SetTextVariable("player", playerName)
																			.SetTextVariable("playerSUB", playerGenderSubjective)
																			.ToString();

																			ChangeRelationAction.ApplyPlayerRelation(targetLord, 1);
																			Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 10);
																			var eventMsgEnd = new TextObject(
																				"{=Feast_Event_Msg_End}The feast has ended.")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.ToString();
																			InformationManager.DisplayMessage(new InformationMessage(eventMsgEnd, RandomEventsSubmodule.Msg_Color));

																			InformationManager.ShowInquiry(new InquiryData(eventTitle, eventDescription7v, true, false, eventButtonText2, null, null, null), true);

																			StopEvent();

																			break;
																			#endregion
																	}
																}, null, null);

															MBInformationManager.ShowMultiSelectionInquiry(msid1dr, true);

															StopEvent();

															break;
														#endregion

														#region Finish Up
														case "d": // Finish Up


															var eventDescription8v = new TextObject("{=Feast_Event_Description7v}[Good Relation] After finishing yet another round of cheerful story telling, {title} {targetLord} offers to make a toast in your honor, to which you accept. " +
																"\n \n “To {Ptitle} {player}, an excellent leader, and an even better friend. May {playerSUB} live forever!”. \n \n {targetLord} then thanks you for joining them in such an exquisite meal and looks forward to seeing you in {settlement} " +
																"at a later time. And with that, the feast comes to and end..")
															.SetTextVariable("targetLord", targetLord.Name)
															.SetTextVariable("settlement", currentSettlement)
															.SetTextVariable("title", targetLordGenderTitle)
															.SetTextVariable("Ptitle", playerTitle)
															.SetTextVariable("player", playerName)
															.SetTextVariable("playerSUB", playerGenderSubjective)
															.ToString();

															ChangeRelationAction.ApplyPlayerRelation(targetLord, 3);
															Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 10);
															var eventMsgEnd = new TextObject(
																"{=Feast_Event_Msg_End}The feast has ended.")
															.SetTextVariable("targetLord", targetLord.Name)
															.ToString();
															InformationManager.DisplayMessage(new InformationMessage(eventMsgEnd, RandomEventsSubmodule.Msg_Color));

															InformationManager.ShowInquiry(new InquiryData(eventTitle, eventDescription8v, true, false, eventButtonText2, null, null, null), true);

															StopEvent();

															break;
															#endregion
													}
												}, null, null);

											MBInformationManager.ShowMultiSelectionInquiry(msid1b, true);

											StopEvent();
											break;

										#endregion

										#region End Feast
										case "b": //End Feast-------------

											var eventDescriptionEndFeast1 = new TextObject("{=Feast_Event_DescriptionEndFeast}[Good Relation] After finishing your meals {title} {targetLord} offers to make a toast in your honor, to which you accept. \n \n" +
												"“To {Ptitle} {player}, an excellent leader, and an even better friend. May {playerSUB} live forever!”. \n \n And with that, the feast comes to an end.")
											.SetTextVariable("targetLord", targetLord.Name)
											.SetTextVariable("settlement", currentSettlement)
											.SetTextVariable("title", targetLordGenderTitle)
											.SetTextVariable("Ptitle", playerTitle)
											.SetTextVariable("player", playerName)
											.SetTextVariable("playerSUB", playerGenderSubjective)

											.ToString();

											ChangeRelationAction.ApplyPlayerRelation(targetLord, 1);
											Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 10);
											var eventMsgEnd = new TextObject(
												"{=Feast_Event_Msg_End}The feast has ended.")
											.SetTextVariable("targetLord", targetLord.Name)
											.ToString();
											InformationManager.DisplayMessage(new InformationMessage(eventMsgEnd, RandomEventsSubmodule.Msg_Color));

											InformationManager.ShowInquiry(new InquiryData(eventTitle, eventDescriptionEndFeast1, true, false, eventButtonText2, null, null, null), true);

											StopEvent();

											break;
											#endregion
									}
								}, null, null);

								MBInformationManager.ShowMultiSelectionInquiry(msid1a, true);

								StopEvent();
							}
							#endregion

							#region Bad Relation
							else if (relation < -9)
							{

								var eventDescription1b = new TextObject("{=Feast_Event_Description1b}[Bad Relation] You arrive at the keep in {settlement} where {title} {targetLord} is waiting. Introductions are made and civil norms are met as per tradition before" +
									" finally taking a seat at the table. {targetLord} makes a snarky remark regarding your attire, and gives a shallow thanks for " +
									"joining {gender} for this occasion. A few minutes pass and small talk builds tension as {targetLord} begins questioning your support of current political affairs. It is obvious {genderSUB} merely wishes to extort" +
									" some degree of support from you, which would explain this invitation..")
								.SetTextVariable("targetLord", targetLord.Name)
								.SetTextVariable("settlement", currentSettlement)
								.SetTextVariable("gender", targetLordGenderObjective)
								.SetTextVariable("genderSUB", targetLordGenderSubjective)
								.SetTextVariable("title", targetLordGenderTitle)
								.ToString();

								
								Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 2);


								#region Inquiry Elements 2
								//option A ---- Continue ----
								var eventOption1u = new TextObject("{=Feast_Event_Option_1a}Continue Feast").ToString();
								var eventOption1uHover = new TextObject("{=Feast_Event_Option_1a_Hover}Let's continue..").ToString();
								//option D ---- End ----
								var eventOption2a = new TextObject("{=Feast_Event_Option_2a}End Feast").ToString();
								var eventOption2aHover = new TextObject("{=Feast_Event_Option_2a_Hover}This is a good time to stop").ToString();



								var inquiryElements2 = new List<InquiryElement>
								{
									new InquiryElement("a", eventOption1u, null, true, eventOption1uHover),
										//new InquiryElement("b", eventOption2a, null, true, eventOption2aHover),
										//new InquiryElement("c", eventOption3a, null, true, eventOption3aHover),
									new InquiryElement("b", eventOption2a, null, true, eventOption2aHover),
								};
								#endregion

								var msid1a = new MultiSelectionInquiryData(eventTitle, eventDescription1b, inquiryElements2, false, 1, 1, eventButtonText1, null,
								elements =>
								{
									switch ((string)elements[0].Identifier)
									{
										#region Continue Feast 
										case "a": //Continue Feast ---------------------------


											var eventDescription2b = new TextObject("{=Feast_Event_Description2b}[Bad Relation] After an agonizing meal {targetLord} signals a servant to bring a new bottle of wine for the two of you to wash it all down," +
												" specifying not to bring anything 'too fancy'. {targetLord} continues questioning your stance on the current political climate, hoping to reach some sort of agreement on the issues. You " +
												"keep a neutral tone and blank stare while also nodding in agreeance as though to keep the discussion civil.")
											.SetTextVariable("targetLord", targetLord.Name)
											.SetTextVariable("gender", targetLordGenderAdjective)
											.SetTextVariable("title", targetLordGenderTitle)
											.ToString();

											ChangeRelationAction.ApplyPlayerRelation(targetLord, 1);

											#region Inquiry Elements 3
											//option D ---- End ----
											var eventOption2b = new TextObject("{=Feast_Event_Option_2b}Finish Feast").ToString();
											var eventOption2bHover = new TextObject("{=Feast_Event_Option_2b_Hover}Time to finish up").ToString();
											//option A ---- Skill: Charm different sex----
											var eventOption3b = new TextObject("{=Feast_Event_Option_3b}[Charm] Compliment").ToString();
											var eventOption3bHover = new TextObject("{=Feast_Event_Option_3b_Hover}Charm {gender} a little \n[Charm - lvl 120]")
											.SetTextVariable("gender", targetLordGenderObjective)
											.ToString();
											//option B ---- Skill: Charm same sex----
											var eventOption4b = new TextObject("{=Feast_Event_Option_4b}[Charm] Boast").ToString();
											var eventOption4bHover = new TextObject("{=Feast_Event_Option_4b_Hover}Boast their ego a bit \n[Charm - lvl 120]").ToString();
											//option C ---- Skill: Rogue ----
											var eventOption5b = new TextObject("{=Feast_Event_Option_5b}[Roguery] Theft").ToString();
											var eventOption5bHover = new TextObject("{=Feast_Event_Option_5b_Hover}Quite the coin purse.. \n[Roguery - lvl 60]")
											.SetTextVariable("gender", targetLordGenderSubjective)
											.ToString();

											var inquiryElements3 = new List<InquiryElement>();
											if (charmedNoble)
											{
												if (diffGender)
												{
													inquiryElements3.Add(new InquiryElement("a", eventOption3b, null, true, eventOption3bHover));
												}
												else if (diffGender == false)
												{
													inquiryElements3.Add(new InquiryElement("b", eventOption4b, null, true, eventOption4bHover));
												}
											}
											if (theftSuccess)
											{
												inquiryElements3.Add(new InquiryElement("c", eventOption5b, null, true, eventOption5bHover));
											}
											inquiryElements3.Add(new InquiryElement("d", eventOption2b, null, true, eventOption2bHover));


											#endregion

											var msid1b = new MultiSelectionInquiryData(eventTitle, eventDescription2b, inquiryElements3, false, 1, 1, eventButtonText1, null,
												elements =>
												{
													switch ((string)elements[0].Identifier)
													{
														#region Charm Diff Sex
														case "a":// CHARM different sex

															var eventDescription3u = new TextObject("{=Feast_Event_Description3u}[Bad Relation] Despite the fact you both seem to despise one another, it seems there is quite a sense of attraction." +
																" Perhaps it's the wine, or perhaps you've had a long day, regardless.. You notice a beauty in {targetLord} you haven't been aware of before, something about {gender} eyes are " +
																"pulling you in. As you smile {gender} eyes meet yours and {genderSUB} gives you a little wink. You both know what is happening here..")
															.SetTextVariable("targetLord", targetLord.Name)
															.SetTextVariable("gender", targetLordGenderAdjective)
															.SetTextVariable("genderSUB", targetLordGenderSubjective)
															.ToString();

															ChangeRelationAction.ApplyPlayerRelation(targetLord, 1);
															Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 10);
															var eventMsgCharm1 = new TextObject(
																"{=Feast_Event_Msg_Charm1}You charm {targetLord}.")
															.SetTextVariable("targetLord", targetLord.Name)
															.ToString();
															InformationManager.DisplayMessage(new InformationMessage(eventMsgCharm1, RandomEventsSubmodule.Msg_Color));

															#region Inquiry Elements
															//option A ---- Hook up ----
															var eventOption2uc = new TextObject("{=Feast_Event_Option_2uc}[Charm] Go somewhere private").ToString();
															var eventOption2ucHover = new TextObject("{=Feast_Event_Option_2uc_Hover}Show {gender} around.. \n[Charm - lvl 300]")
															.SetTextVariable("gender", targetLordGenderObjective)
															.ToString();
															//option B ---- End ----
															var eventOption2c = new TextObject("{=Feast_Event_Option_2c}End Feast").ToString();
															var eventOption2cHover = new TextObject("{=Feast_Event_Option_2c_Hover}Let's not get carried away..").ToString();

															var inquiryElements4 = new List<InquiryElement>();
															if (charmedNoble3)
															{
																inquiryElements4.Add(new InquiryElement("a", eventOption2uc, null, true, eventOption2ucHover));
															}
															inquiryElements4.Add(new InquiryElement("b", eventOption2c, null, true, eventOption2cHover));
															#endregion

															var msid1c = new MultiSelectionInquiryData(eventTitle, eventDescription3u, inquiryElements4, false, 1, 1, eventButtonText1, null,
																elements =>
																{
																	switch ((string)elements[0].Identifier)
																	{
																		#region Hook Up
																		case "a":// Proceed to hook up

																			var eventDescription4u = new TextObject("{=Feast_Event_Description4u}[Bad Relation] You stand from your chair and offer your hand to {title} {targetLord}, {genderSUB} accepts and raises as well." +
																				" The both of you walk into the other room and aren't seen again for a couple hours..")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.SetTextVariable("genderSUB", targetLordGenderSubjective)
																			.SetTextVariable("title", targetLordGenderTitle)
																			.ToString();

																			ChangeRelationAction.ApplyPlayerRelation(targetLord, 5);

																			InformationManager.ShowInquiry(new InquiryData(eventTitle, eventDescription4u, true, false, eventButtonText2, null, null, null), true);

																			StopEvent();

																			break;
                                                                        #endregion

                                                                        #region Leave
                                                                        case "b": // Leave

																			var eventDescription3b = new TextObject("{=Feast_Event_Description3b}[Bad Relation] Although the tension is more than enough to write a story of its own - you decide it's best for the two of you to" +
																				" go your separate ways, for now.. {title} {targetLord} looks quite displeased with the feast, and hopes your stay in {settlement} ends with haste.")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.SetTextVariable("settlement", currentSettlement)
																			.SetTextVariable("title", targetLordGenderTitle)
																			.ToString();


																			InformationManager.ShowInquiry(new InquiryData(eventTitle, eventDescription3b, true, false, eventButtonText2, null, null, null), true);

																			StopEvent();

																			break;
                                                                            #endregion

                                                                    }
                                                                }, null, null);

															MBInformationManager.ShowMultiSelectionInquiry(msid1c, true);

															StopEvent();


															break;
														#endregion

														#region Charm Same Sex
														case "b": // CHARM same sex

															var eventDescription1ru = new TextObject("{=Feast_Event_Description1ru}[Bad Relation] After what seems like an eternity of interrogation, you decide to play along and admit to {targetLord} that you believe" +
																" {genderADJ} plan is actually quite brilliant. Stating you didn't wish to state this before only because you were quite jealous that you hadn't thought of it first.")
															.SetTextVariable("targetLord", targetLord.Name)
															.SetTextVariable("genderADJ", targetLordGenderAdjective)
															.SetTextVariable("title", targetLordGenderTitle)
															.SetTextVariable("settlement", currentSettlement)
															.ToString();

															ChangeRelationAction.ApplyPlayerRelation(targetLord, 2);
															Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 10);
															var eventMsgCharm1a = new TextObject(
																"{=Feast_Event_Msg_Charm1}You charm {targetLord}.")
															.SetTextVariable("targetLord", targetLord.Name)
															.ToString();
															InformationManager.DisplayMessage(new InformationMessage(eventMsgCharm1a, RandomEventsSubmodule.Msg_Color));

															#region Inquiry Elements
															//option A ---- Continue ----
															var eventOption1ru = new TextObject("{=Feast_Event_Option_1r}Continue Feast").ToString();
															var eventOption1ruHover = new TextObject("{=Feast_Event_Option_1r_Hover}Let's continue..")
															.ToString();
															//option B ---- End ----
															var eventOption2r = new TextObject("{=Feast_Event_Option_2r}End Feast").ToString();
															var eventOption2rHover = new TextObject("{=Feast_Event_Option_2r_Hover}This is a good time to stop").ToString();

															var inquiryElements5 = new List<InquiryElement>
															{
																new InquiryElement("a", eventOption1ru, null, true, eventOption1ruHover),
																new InquiryElement("b", eventOption2r, null, true, eventOption2rHover),
															};
															#endregion

															var msid1d = new MultiSelectionInquiryData(eventTitle, eventDescription1ru, inquiryElements5, false, 2, 2, eventButtonText1, null,
																elements =>
																{
																	switch ((string)elements[0].Identifier)
																	{
																		#region Continue
																		case "a":// Continue Feast

																			var eventDescription4b = new TextObject("{=Feast_Event_Description4b}[Bad Relation] A few minutes of careful boasting leads to {title} {targetLord} feeling quite impressed by your words. {genderSUB} confesses" +
																				" that had {genderSUB} known you felt this way perhaps you could be friends. You ask {targetLord} to show you around {settlement} while discussing further {targetLord}'s political agenda. To which" +
																				" {genderSUB} accepts.")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.SetTextVariable("genderSUB", targetLordGenderSubjective)
																			.SetTextVariable("genderOBJ", targetLordGenderObjective)
																			.SetTextVariable("gender", targetLordGenderAdjective)
																			.SetTextVariable("title", targetLordGenderTitle)
																			.SetTextVariable("settlement", currentSettlement)
																			.ToString();

																			ChangeRelationAction.ApplyPlayerRelation(targetLord, 1);

																			InformationManager.ShowInquiry(new InquiryData(eventTitle, eventDescription4b, true, false, eventButtonText2, null, null, null), true);

																			StopEvent();

																			break;
																		#endregion

																		#region Leave
																		case "b": // Leave

																			var eventDescription3ru = new TextObject("{=Feast_Event_Description3ru}[Bad Relation] A few minutes of careful boasting leads to {title} {targetLord} feeling quite impressed by your words. {genderSUB} confesses" +
																				" that had {genderSUB} known you felt this way perhaps you could be friends. On that note you declare the feast is finished, giving {targetLord} a formal farewell before leaving the keep.")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.SetTextVariable("genderSUB", targetLordGenderSubjective)
																			.SetTextVariable("genderOBJ", targetLordGenderObjective)
																			.SetTextVariable("gender", targetLordGenderAdjective)
																			.SetTextVariable("title", targetLordGenderTitle)
																			.SetTextVariable("settlement", currentSettlement)
																			.ToString();

																			InformationManager.ShowInquiry(new InquiryData(eventTitle, eventDescription3ru, true, false, eventButtonText2, null, null, null), true);

																			StopEvent();

																			break;
																			#endregion
																	}
																}, null, null);

															MBInformationManager.ShowMultiSelectionInquiry(msid1d, true);

															StopEvent();
															break;
														#endregion

														#region Rogue
														case "c": // ROGUE

															var eventDescription5b = new TextObject("{=Feast_Event_Description5b}[Bad Relation] Amidst the dreadful debate, you signal one of your men to bring a bottle of wine from your personal supply and make a scene. As the wine arrives your" +
																" he trips and drops the bottle onto the table, spilling it into {targetLord}'s lap. You react quickly, grabbing a cloth from the table and reaching towards {genderOBJ}, attempting to wipe away the " +
																"still running wine from {genderADJ} shirt. When the moment is right you swipe {genderADJ} coin purse without {genderOBJ} ever noticing.")
															.SetTextVariable("targetLord", targetLord.Name)
															.SetTextVariable("genderADJ", targetLordGenderAdjective)
															.SetTextVariable("genderOBJ", targetLordGenderObjective)
															.ToString();

															var MoneyStole = MBRandom.RandomInt(1000, 5000);
															Hero.MainHero.ChangeHeroGold(+MoneyStole);
															Hero.MainHero.AddSkillXp(DefaultSkills.Roguery, 50);
															var eventMsgRogue = new TextObject(
																"{=Feast_Event_Msg_Rogue}You steal the coin purse.").ToString();
															InformationManager.DisplayMessage(new InformationMessage(eventMsgRogue, RandomEventsSubmodule.Msg_Color));

															#region Inquiry Elements
															//option A ---- Continue ----
															var eventOption1rq = new TextObject("{=Feast_Event_Option_1r}Continue Feast").ToString();
															var eventOption1rqHover = new TextObject("{=Feast_Event_Option_1r_Hover}Let's continue..")
															.ToString();
															//option B ---- End ----
															var eventOption2rr = new TextObject("{=Feast_Event_Option_2r}End Feast").ToString();
															var eventOption2rrHover = new TextObject("{=Feast_Event_Option_2r_Hover}This is a good time to stop").ToString();

															var inquiryElements5r = new List<InquiryElement>
															{
																new InquiryElement("a", eventOption1rq, null, true, eventOption1rqHover),
																new InquiryElement("b", eventOption2rr, null, true, eventOption2rrHover),
															};
															#endregion

															var msid1dr = new MultiSelectionInquiryData(eventTitle, eventDescription5b, inquiryElements5r, false, 2, 2, eventButtonText1, null,
																elements =>
																{
																	switch ((string)elements[0].Identifier)
																	{
																		#region Continue
																		case "a":// Continue Feast

																			var eventDescription2rq = new TextObject("{=Feast_Event_Description2ru}[Bad Relation] A few minutes of careful word choice leads you to hint at the idea of supporting {targetLord} in {genderADJ} political agenda." +
																				" Although you think it's foolish, you just wish {genderSUB} would quiet down a bit. The feast continues for another couple hours until all the cheap wine has been drank.")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.SetTextVariable("genderADJ", targetLordGenderAdjective)
																			.SetTextVariable("genderSUB", targetLordGenderSubjective)
																			.ToString();

																			ChangeRelationAction.ApplyPlayerRelation(targetLord, 2);

																			InformationManager.ShowInquiry(new InquiryData(eventTitle, eventDescription2rq, true, false, eventButtonText2, null, null, null), true);

																			StopEvent();

																			break;
																		#endregion

																		#region Leave
																		case "b": // Leave

																			var eventDescription3rq = new TextObject("{=Feast_Event_Description3rq}[Bad Relation] A few minutes of careful word choice leads to {title} {targetLord} feeling quite impressed by your position. Finally, the cheap wine" +
																				" has come to an end and on that note you declare the feast is finished, giving {targetLord} a formal farewell before leaving the keep.")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.SetTextVariable("title", targetLordGenderTitle)
																			.ToString();

																			ChangeRelationAction.ApplyPlayerRelation(targetLord, 1);

																			InformationManager.ShowInquiry(new InquiryData(eventTitle, eventDescription3rq, true, false, eventButtonText2, null, null, null), true);

																			StopEvent();

																			break;
																			#endregion
																	}
																}, null, null);

															MBInformationManager.ShowMultiSelectionInquiry(msid1dr, true);

															StopEvent();

															break;
														#endregion

														#region Finish Up
														case "d": // Finish Up


															var eventDescription6b = new TextObject("{=Feast_Event_Description6b}[Bad Relation] After what feels like an eternity of political discourse, you decide it's time to call this feast to an end. {title} {targetLord} seems" +
																" quite displeased with your lack of support and hopes your stay in {settlement} comes to an end as quickly as possible.")
															.SetTextVariable("targetLord", targetLord.Name)
															.SetTextVariable("settlement", currentSettlement)
															.SetTextVariable("title", targetLordGenderTitle)
															.ToString();

															

															InformationManager.ShowInquiry(new InquiryData(eventTitle, eventDescription6b, true, false, eventButtonText2, null, null, null), true);

															StopEvent();

															break;
															#endregion
													}
												}, null, null);

											MBInformationManager.ShowMultiSelectionInquiry(msid1b, true);

											StopEvent();
											break;

										#endregion

										#region End Feast
										case "b": //End Feast-------------

											var eventDescriptionEndFeast7b = new TextObject("{=Feast_Event_DescriptionEndFeast7b}[Bad Relation] After finishing your meals you declare this feast come to an end. {title} {targetLord} seems displeased by your lack" +
												" of support and wishes your stay in {settlement} comes to an end as quickly as possible.")
											.SetTextVariable("targetLord", targetLord.Name)
											.SetTextVariable("settlement", currentSettlement)
											.SetTextVariable("title", targetLordGenderTitle)
											.ToString();

											Hero.MainHero.SetPersonalRelation(targetLord, +1);
											Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 10);
											var eventMsgEnd = new TextObject(
												"{=Feast_Event_Msg_End}The feast has ended.")
											.SetTextVariable("targetLord", targetLord.Name)
											.ToString();
											InformationManager.DisplayMessage(new InformationMessage(eventMsgEnd, RandomEventsSubmodule.Msg_Color));

											InformationManager.ShowInquiry(new InquiryData(eventTitle, eventDescriptionEndFeast7b, true, false, eventButtonText2, null, null, null), true);

											StopEvent();

											break;
											#endregion
									}
								}, null, null);

								MBInformationManager.ShowMultiSelectionInquiry(msid1a, true);

								StopEvent();

							}
							#endregion

							#region Neutral Relation
							else
							{
								var eventDescription1i = new TextObject("{=Feast_Event_Description1i}You arrive at your the in {settlement} where {title} {targetLord} is waiting. Introductions are made and civil norms are met as per tradition before" +
								" finally taking a seat at the table. {targetLord} admires your attire, noting your acceptable presentation, and gives thanks for " +
								"joining {gender} for such a fine feast. A few minutes pass and small talk turns to laughter as you both discuss recent engagements and share stories of your travels. \n \n" +
								"As the food finally arrives the both of you dig in.")
								.SetTextVariable("targetLord", targetLord.Name)
								.SetTextVariable("settlement", currentSettlement)
								.SetTextVariable("gender", targetLordGenderObjective)
								.SetTextVariable("genderSUB", targetLordGenderSubjective)
								.SetTextVariable("title", targetLordGenderTitle)
								.ToString();

								
								Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 2);

								#region Inquiry Elements 2
								//option A ---- Continue ----
								var eventOption1u = new TextObject("{=Feast_Event_Option_1a}Continue Feast").ToString();
								var eventOption1uHover = new TextObject("{=Feast_Event_Option_1a_Hover}Let's continue..").ToString();
								//option D ---- End ----
								var eventOption2a = new TextObject("{=Feast_Event_Option_2a}End Feast").ToString();
								var eventOption2aHover = new TextObject("{=Feast_Event_Option_2a_Hover}This is a good time to stop").ToString();



								var inquiryElements2 = new List<InquiryElement>
								{
									new InquiryElement("a", eventOption1u, null, true, eventOption1uHover),
										//new InquiryElement("b", eventOption2a, null, true, eventOption2aHover),
										//new InquiryElement("c", eventOption3a, null, true, eventOption3aHover),
									new InquiryElement("b", eventOption2a, null, true, eventOption2aHover),
								};
								#endregion

								var msid1a = new MultiSelectionInquiryData(eventTitle, eventDescription1i, inquiryElements2, false, 1, 1, eventButtonText1, null,
								elements =>
								{
									switch ((string)elements[0].Identifier)
									{
										#region Continue Feast 
										case "a": //Continue Feast ---------------------------


											var eventDescription2i = new TextObject("{=Feast_Event_Description2i}After an enjoyable meal {targetLord} requests a new bottle of wine for the two of you to wash it all down." +
												" {targetLord} begins questioning your stance on the current political climate, you answer vaguely as to not stir up any controversy. Playing off" +
												" of {targetLord}'s ideals more than your own. Between the civil discourse lay harmless jokes of noble affairs, you both share a laugh and continue drinking wine.")
											.SetTextVariable("targetLord", targetLord.Name)
											.SetTextVariable("gender", targetLordGenderAdjective)
											.SetTextVariable("title", targetLordGenderTitle)
											.ToString();

											ChangeRelationAction.ApplyPlayerRelation(targetLord, 2);
											Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 2);

											#region Inquiry Elements 3
											//option D ---- End ----
											var eventOption2b = new TextObject("{=Feast_Event_Option_2b}Finish Feast").ToString();
											var eventOption2bHover = new TextObject("{=Feast_Event_Option_2b_Hover}Time to finish up").ToString();
											//option A ---- Skill: Charm different sex----
											var eventOption3b = new TextObject("{=Feast_Event_Option_3b}[Charm] Compliment").ToString();
											var eventOption3bHover = new TextObject("{=Feast_Event_Option_3b_Hover}Charm {gender} a little \n[Charm - lvl 120]")
											.SetTextVariable("gender", targetLordGenderObjective)
											.ToString();
											//option B ---- Skill: Charm same sex----
											var eventOption4b = new TextObject("{=Feast_Event_Option_4b}[Charm] Boast").ToString();
											var eventOption4bHover = new TextObject("{=Feast_Event_Option_4b_Hover}Boast their ego a bit \n[Charm - lvl 120]").ToString();
											//option C ---- Skill: Rogue ----
											var eventOption5b = new TextObject("{=Feast_Event_Option_5b}[Roguery] Theft").ToString();
											var eventOption5bHover = new TextObject("{=Feast_Event_Option_5b_Hover}Quite the coin purse.. \n[Roguery - lvl 60]")
											.SetTextVariable("gender", targetLordGenderSubjective)
											.ToString();

											var inquiryElements3 = new List<InquiryElement>();
											if (charmedNoble)
											{
												if (diffGender)
												{
													inquiryElements3.Add(new InquiryElement("a", eventOption3b, null, true, eventOption3bHover));
												}
												else if (diffGender == false)
												{
													inquiryElements3.Add(new InquiryElement("b", eventOption4b, null, true, eventOption4bHover));
												}
											}
											if (theftSuccess)
											{
												inquiryElements3.Add(new InquiryElement("c", eventOption5b, null, true, eventOption5bHover));
											}
											inquiryElements3.Add(new InquiryElement("d", eventOption2b, null, true, eventOption2bHover));


											#endregion

											var msid1b = new MultiSelectionInquiryData(eventTitle, eventDescription2i, inquiryElements3, false, 1, 1, eventButtonText1, null,
												elements =>
												{
													switch ((string)elements[0].Identifier)
													{
														#region Charm Diff Sex
														case "a":// CHARM different sex

															var eventDescription3n = new TextObject("{=Feast_Event_Description3n}As the feast goes on it seems there is quite a sense of attraction." +
																" Perhaps it's the wine, or perhaps you've looked at {targetLord} this way, regardless.. You notice a beauty in {genderOBJ} you haven't been aware of before, something about {gender} eyes are " +
																"pulling you in. As you smile {gender} eyes meet yours and {genderSUB} gives you a little wink. You both know what is happening here..")
															.SetTextVariable("targetLord", targetLord.Name)
															.SetTextVariable("gender", targetLordGenderAdjective)
															.SetTextVariable("genderSUB", targetLordGenderSubjective)
															.SetTextVariable("genderOBJ", targetLordGenderObjective)
															.ToString();

															ChangeRelationAction.ApplyPlayerRelation(targetLord, 5);
															Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 10);
															var eventMsgCharm1 = new TextObject(
																"{=Feast_Event_Msg_Charm1}You charm {targetLord}.")
															.SetTextVariable("targetLord", targetLord.Name)
															.ToString();
															InformationManager.DisplayMessage(new InformationMessage(eventMsgCharm1, RandomEventsSubmodule.Msg_Color));

															#region Inquiry Elements
															//option A ---- Hook up ----
															var eventOption2uc = new TextObject("{=Feast_Event_Option_2uc}[Charm] Go somewhere private").ToString();
															var eventOption2nHover = new TextObject("{=Feast_Event_Option_2n_Hover}Show {gender} around.. \n[Charm - lvl 210]")
															.SetTextVariable("gender", targetLordGenderObjective)
															.ToString();
															//option B ---- End ----
															var eventOption2c = new TextObject("{=Feast_Event_Option_2c}End Feast").ToString();
															var eventOption2cHover = new TextObject("{=Feast_Event_Option_2c_Hover}Let's not get carried away..").ToString();

															var inquiryElements4 = new List<InquiryElement>();
															if (charmedNoble4)
															{
																inquiryElements4.Add(new InquiryElement("a", eventOption2uc, null, true, eventOption2nHover));
															}
															inquiryElements4.Add(new InquiryElement("b", eventOption2c, null, true, eventOption2cHover));
															#endregion

															var msid1c = new MultiSelectionInquiryData(eventTitle, eventDescription3n, inquiryElements4, false, 1, 1, eventButtonText1, null,
																elements =>
																{
																	switch ((string)elements[0].Identifier)
																	{
                                                                        #region Hook Up
                                                                        case "a":// Proceed to hook up

																			var eventDescription4n = new TextObject("{=Feast_Event_Description4n}You stand from your chair and offer your hand to {title} {targetLord}, {genderSUB} accepts and raises as well." +
																				" The both of you walk into the other room and aren't seen again for a couple hours..")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.SetTextVariable("genderSUB", targetLordGenderSubjective)
																			.SetTextVariable("title", targetLordGenderTitle)
																			.ToString();

																			ChangeRelationAction.ApplyPlayerRelation(targetLord, 10);
																			Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 50);
																			var eventMsgCharm2 = new TextObject(
																				"{=Feast_Event_Msg_Charm2}You and {targetLord} have a great time.")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.ToString();
																			InformationManager.DisplayMessage(new InformationMessage(eventMsgCharm2, RandomEventsSubmodule.Msg_Color));

																			InformationManager.ShowInquiry(new InquiryData(eventTitle, eventDescription4n, true, false, eventButtonText2, null, null, null), true);

																			StopEvent();

																			break;
                                                                        #endregion

                                                                        #region Leave
                                                                        case "b": // Leave

																			var eventDescription3i = new TextObject("{=Feast_Event_Description3i}Although the tension is more than enough to write a story of its own - you decide it's best for the two of you to" +
																				" go your separate ways, for now.. {title} {targetLord} looks quite pleased with the feast, and hopes to see you again in {settlement} soon.")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.SetTextVariable("settlement", currentSettlement)
																			.SetTextVariable("title", targetLordGenderTitle)
																			.ToString();

																			ChangeRelationAction.ApplyPlayerRelation(targetLord, 2);
																			Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 10);
																			var eventMsgEnd = new TextObject(
																				"{=Feast_Event_Msg_End}The feast has ended.")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.ToString();
																			InformationManager.DisplayMessage(new InformationMessage(eventMsgEnd, RandomEventsSubmodule.Msg_Color));

																			InformationManager.ShowInquiry(new InquiryData(eventTitle, eventDescription3i, true, false, eventButtonText2, null, null, null), true);

																			StopEvent();

																			break;
                                                                            #endregion

                                                                    }
                                                                }, null, null);

															MBInformationManager.ShowMultiSelectionInquiry(msid1c, true);

															StopEvent();


															break;
														#endregion

														#region Charm Same Sex
														case "b": // CHARM same sex

															var eventDescription1rn = new TextObject("{=Feast_Event_Description1rn}After a few laughs {title} {targetLord} requests another glass of wine, you accept another as well. With the mood set you recall the news of " +
																"{targetLord}'s recent achievements, noting there has been quite a lot of talk lately in regards. {genderSUBCAP} can't help but blush, trying to play it off as if it's no big deal. Needless to say {genderSUB} finds " +
																"great pleasure in knowing {genderADJ} name has spread throughout the land.")
															.SetTextVariable("targetLord", targetLord.Name)
															.SetTextVariable("genderSUB", targetLordGenderSubjective)
															.SetTextVariable("genderSUBCAP", targetLordGenderSubjectiveCAP)
															.SetTextVariable("genderADJ", targetLordGenderAdjective)
															.SetTextVariable("title", targetLordGenderTitle)
															.SetTextVariable("settlement", currentSettlement)
															.ToString();

															ChangeRelationAction.ApplyPlayerRelation(targetLord, 3);
															Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 10);
															var eventMsgCharm1a = new TextObject(
																"{=Feast_Event_Msg_Charm1}You charm {targetLord}.")
															.SetTextVariable("targetLord", targetLord.Name)
															.ToString();
															InformationManager.DisplayMessage(new InformationMessage(eventMsgCharm1a, RandomEventsSubmodule.Msg_Color));

															#region Inquiry Elements
															//option A ---- Continue ----
															var eventOption1ru = new TextObject("{=Feast_Event_Option_1r}Continue Feast").ToString();
															var eventOption1ruHover = new TextObject("{=Feast_Event_Option_1r_Hover}Let's continue..")
															.ToString();
															//option B ---- End ----
															var eventOption2r = new TextObject("{=Feast_Event_Option_2r}End Feast").ToString();
															var eventOption2rHover = new TextObject("{=Feast_Event_Option_2r_Hover}This is a good time to stop").ToString();

															var inquiryElements5 = new List<InquiryElement>
															{
																new InquiryElement("a", eventOption1ru, null, true, eventOption1ruHover),
																new InquiryElement("b", eventOption2r, null, true, eventOption2rHover),
															};
															#endregion

															var msid1d = new MultiSelectionInquiryData(eventTitle, eventDescription1rn, inquiryElements5, false, 2, 2, eventButtonText1, null,
																elements =>
																{
																	switch ((string)elements[0].Identifier)
																	{
																		#region Continue
																		case "a":// Continue Feast

																			var eventDescription4i = new TextObject("{=Feast_Event_Description4i}A few minutes of careful boasting leads to {title} {targetLord} feeling quite impressed by your words. {genderSUB} confesses" +
																				" that your name has also been making its way around the noble tables. You smile and this civil discourse continues for a few hours until the wine runs dry. {targetLord} offers to take you around " +
																				"{settlement} for a full tour, to which you accept.")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.SetTextVariable("genderSUB", targetLordGenderSubjective)
																			.SetTextVariable("title", targetLordGenderTitle)
																			.SetTextVariable("settlement", currentSettlement)
																			.ToString();


																			ChangeRelationAction.ApplyPlayerRelation(targetLord, 2);
																			Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 20);
																			var eventMsgCharm2 = new TextObject(
																				"{=Feast_Event_Msg_Charm2}You and {targetLord} have a great time.")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.ToString();
																			InformationManager.DisplayMessage(new InformationMessage(eventMsgCharm2, RandomEventsSubmodule.Msg_Color));

																			InformationManager.ShowInquiry(new InquiryData(eventTitle, eventDescription4i, true, false, eventButtonText2, null, null, null), true);

																			StopEvent();

																			break;
																		#endregion

																		#region Leave
																		case "b": // Leave

																			var eventDescription3rn = new TextObject("{=Feast_Event_Description3rn}A few minutes of careful boasting leads to {title} {targetLord} feeling quite impressed by your words. {genderSUB} confesses" +
																				" that your name has also been making its way around the noble tables. With that, you declare the feast finished, to which {title} {targetLord} gives a formal farewell before leaving the keep.")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.SetTextVariable("genderSUB", targetLordGenderSubjective)
																			.SetTextVariable("genderOBJ", targetLordGenderObjective)
																			.SetTextVariable("gender", targetLordGenderAdjective)
																			.SetTextVariable("title", targetLordGenderTitle)
																			.SetTextVariable("settlement", currentSettlement)
																			.ToString();

																			ChangeRelationAction.ApplyPlayerRelation(targetLord, 1);
																			Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 10);
																			var eventMsgEnd = new TextObject(
																				"{=Feast_Event_Msg_End}The feast has ended.")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.ToString();
																			InformationManager.DisplayMessage(new InformationMessage(eventMsgEnd, RandomEventsSubmodule.Msg_Color));

																			InformationManager.ShowInquiry(new InquiryData(eventTitle, eventDescription3rn, true, false, eventButtonText2, null, null, null), true);

																			StopEvent();

																			break;
																			#endregion
																	}
																}, null, null);

															MBInformationManager.ShowMultiSelectionInquiry(msid1d, true);

															StopEvent();
															break;
														#endregion

														#region Rogue
														case "c": // ROGUE

															var eventDescription5i = new TextObject("{=Feast_Event_Description5i}With the mood set, you signal one of your men to bring more wine from your personal supply and make a scene. As the wine arrives" +
																" he trips and drops the bottle onto the table, spilling it into {targetLord}'s lap. You react quickly, grabbing a cloth from the table and reaching towards {genderOBJ}, attempting to wipe away the " +
																"still running wine from {genderADJ} shirt. When the moment is right you swipe {genderADJ} coin purse without {genderOBJ} ever noticing.")
															.SetTextVariable("targetLord", targetLord.Name)
															.SetTextVariable("genderADJ", targetLordGenderAdjective)
															.SetTextVariable("genderOBJ", targetLordGenderObjective)
															.ToString();

															var MoneyStole = MBRandom.RandomInt(1000, 5000);
															Hero.MainHero.ChangeHeroGold(+MoneyStole);
															Hero.MainHero.AddSkillXp(DefaultSkills.Roguery, 50);
															var eventMsgRogue = new TextObject(
																"{=Feast_Event_Msg_Rogue}You steal the coin purse.").ToString();
															InformationManager.DisplayMessage(new InformationMessage(eventMsgRogue, RandomEventsSubmodule.Msg_Color));

															#region Inquiry Elements
															//option A ---- Continue ----
															var eventOption1rq = new TextObject("{=Feast_Event_Option_1r}Continue Feast").ToString();
															var eventOption1rqHover = new TextObject("{=Feast_Event_Option_1r_Hover}Let's continue..")
															.ToString();
															//option B ---- End ----
															var eventOption2rr = new TextObject("{=Feast_Event_Option_2r}End Feast").ToString();
															var eventOption2rrHover = new TextObject("{=Feast_Event_Option_2r_Hover}This is a good time to stop").ToString();

															var inquiryElements5r = new List<InquiryElement>
															{
																new InquiryElement("a", eventOption1rq, null, true, eventOption1rqHover),
																new InquiryElement("b", eventOption2rr, null, true, eventOption2rrHover),
															};
															#endregion

															var msid1dr = new MultiSelectionInquiryData(eventTitle, eventDescription5i, inquiryElements5r, false, 2, 2, eventButtonText1, null,
																elements =>
																{
																	switch ((string)elements[0].Identifier)
																	{
																		#region Continue
																		case "a":// Continue Feast

																			var eventDescription6i = new TextObject("{=Feast_Event_Description6i}After cleaning the wine you offer {targetLord} another glass and the two of you continue talking about all sorts of interesting " +
																				"topics ranging from politics, to war, to the arena. You even manage to learn a thing or two in regards to stewardship. With the wine running {targetLord} offer to take you on a tour of {settlement}," +
																				"to which you accept.")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.SetTextVariable("genderSUB", targetLordGenderSubjective)
																			.ToString();

																			ChangeRelationAction.ApplyPlayerRelation(targetLord, 3);
																			Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 20);
																			Hero.MainHero.AddSkillXp(DefaultSkills.Steward, 50);
																			var eventMsgCharm2 = new TextObject(
																				"{=Feast_Event_Msg_Charm2}You and {targetLord} have a great time.")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.ToString();
																			InformationManager.DisplayMessage(new InformationMessage(eventMsgCharm2, RandomEventsSubmodule.Msg_Color));

																			InformationManager.ShowInquiry(new InquiryData(eventTitle, eventDescription6i, true, false, eventButtonText2, null, null, null), true);

																			StopEvent();

																			break;
																		#endregion

																		#region Leave
																		case "b": // Leave

																			var eventDescription3rm = new TextObject("{=Feast_Event_Description3rm}[Bad Relation] A few minutes of careful word choice leads to {title} {targetLord} feeling quite impressed by your position. Finally, the cheap wine" +
																				" has come to an end and on that note you declare the feast is finished, giving {targetLord} a formal farewell before leaving the keep.")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.SetTextVariable("title", targetLordGenderTitle)
																			.ToString();

																			ChangeRelationAction.ApplyPlayerRelation(targetLord, 1);
																			Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 10);
																			var eventMsgEnd = new TextObject(
																				"{=Feast_Event_Msg_End}The feast has ended.")
																			.SetTextVariable("targetLord", targetLord.Name)
																			.ToString();
																			InformationManager.DisplayMessage(new InformationMessage(eventMsgEnd, RandomEventsSubmodule.Msg_Color));

																			InformationManager.ShowInquiry(new InquiryData(eventTitle, eventDescription3rm, true, false, eventButtonText2, null, null, null), true);

																			StopEvent();

																			break;
																			#endregion
																	}
																}, null, null);

															MBInformationManager.ShowMultiSelectionInquiry(msid1dr, true);

															StopEvent();

															break;
														#endregion

														#region Finish Up
														case "d": // Finish Up


															var eventDescription7i = new TextObject("{=Feast_Event_Description7i}As the wine comes to an end, you decide it's time to call this feast to a close. {title} {targetLord} seems" +
																" quite pleased with your excellent company and hopes to see you again in {settlement} soon.")
															.SetTextVariable("targetLord", targetLord.Name)
															.SetTextVariable("settlement", currentSettlement)
															.SetTextVariable("title", targetLordGenderTitle)
															.ToString();

															ChangeRelationAction.ApplyPlayerRelation(targetLord, 2);
															Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 10);
															var eventMsgEnd = new TextObject(
																"{=Feast_Event_Msg_End}The feast has ended.")
															.SetTextVariable("targetLord", targetLord.Name)
															.ToString();
															InformationManager.DisplayMessage(new InformationMessage(eventMsgEnd, RandomEventsSubmodule.Msg_Color));

															InformationManager.ShowInquiry(new InquiryData(eventTitle, eventDescription7i, true, false, eventButtonText2, null, null, null), true);

															StopEvent();

															break;
															#endregion
													}
												}, null, null);

											MBInformationManager.ShowMultiSelectionInquiry(msid1b, true);

											StopEvent();
											break;

										#endregion

										#region End Feast
										case "b": //End Feast-------------

											var eventDescriptionEndFeast8i = new TextObject("{=Feast_Event_DescriptionEndFeast8i}After finishing your meals you declare this feast come to an end. {title} {targetLord} seems pleased by your " +
												"company and wishes to see you again in {settlement} soon.")
											.SetTextVariable("targetLord", targetLord.Name)
											.SetTextVariable("settlement", currentSettlement)
											.SetTextVariable("title", targetLordGenderTitle)
											.ToString();

											Hero.MainHero.SetPersonalRelation(targetLord, +1);
											Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 10);
											var eventMsgEnd = new TextObject(
												"{=Feast_Event_Msg_End}The feast has ended.")
											.SetTextVariable("targetLord", targetLord.Name)
											.ToString();
											InformationManager.DisplayMessage(new InformationMessage(eventMsgEnd, RandomEventsSubmodule.Msg_Color));

											InformationManager.ShowInquiry(new InquiryData(eventTitle, eventDescriptionEndFeast8i, true, false, eventButtonText2, null, null, null), true);

											StopEvent();

											break;
											#endregion
									}
								}, null, null);

								MBInformationManager.ShowMultiSelectionInquiry(msid1a, true);

								StopEvent();
							}
							#endregion

						}
						#endregion

						break;

					case "b": //Decline Invitation =========================================================

						StopEvent();

					break;
					  
				}
			}, null, null);

			MBInformationManager.ShowMultiSelectionInquiry(msid, true);

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
