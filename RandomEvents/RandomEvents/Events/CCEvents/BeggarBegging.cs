using System;
using System.Collections.Generic;
using System.Windows;
using Bannerlord.RandomEvents.Helpers;
using Bannerlord.RandomEvents.Settings;
using Ini.Net;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace Bannerlord.RandomEvents.Events.CCEvents
{
    public sealed class BeggarBegging : BaseEvent
    {
        private readonly bool eventDisabled;
        private readonly int minStewardLevel;
        private readonly int minRogueryLevel;

        public BeggarBegging() : base(ModSettings.RandomEvents.BeggarBeggingData)
        {
            var ConfigFile = new IniFile(ParseIniFile.GetTheConfigFile());

            eventDisabled = ConfigFile.ReadBoolean("BeggarBegging", "EventDisabled");
            minStewardLevel = ConfigFile.ReadInteger("BeggarBegging", "MinStewardLevel");
            minRogueryLevel = ConfigFile.ReadInteger("BeggarBegging", "MaxStewardLevel");
            
        }

        public override void CancelEvent()
        {
        }

        private bool HasValidEventData()
        {
            if (eventDisabled == false)
            {
                if (minStewardLevel != 0 || minRogueryLevel != 0 )
                {
                    return true;
                }
            }
            
            return false;
        }

        public override bool CanExecuteEvent()
        {
           
            return HasValidEventData() && MobileParty.MainParty.CurrentSettlement != null && (MobileParty.MainParty.CurrentSettlement.IsTown || MobileParty.MainParty.CurrentSettlement.IsVillage);
        }

        public override void StartEvent()
        {
            if (GeneralSettings.DebugMode.IsActive())
            {
                var debugMsg = new TextObject(
                        "Starting “{randomEvent}” with the current values:\n\n" +
                        "Min Steward Level : {minStewardLevel}\n" +
                        "Max Roguery Level : {minRogueryLevel}\n\n" +
                        "To disable these messages make sure you set the DebugMode = false in the ini settings\n\nThe ini file is located here : \n{path}"
                    )
                    .SetTextVariable("randomEvent", randomEventData.eventType)
                    .SetTextVariable("minStewardLevel", minStewardLevel)
                    .SetTextVariable("minRogueryLevel", minRogueryLevel)
                    .SetTextVariable("path", ParseIniFile.GetTheConfigFile())
                    .ToString();
                
                InformationManager.ShowInquiry(new InquiryData("Debug Info", debugMsg, true, false, "Start Event", null, null, null), true);
            }

            var mainHero = Hero.MainHero;

            var heroName = mainHero.FirstName;

            var eventTitle = new TextObject("{=BeggarBegging_Title}Beggar").ToString();

            var currentSettlement = MobileParty.MainParty.CurrentSettlement.Name;

            var currentSettlementOwner = Settlement.CurrentSettlement.Owner;

            var rogueryLevel = Hero.MainHero.GetSkillValue(DefaultSkills.Roguery);
            var stewardLevel = Hero.MainHero.GetSkillValue(DefaultSkills.Steward);

            var ownedSettlement = false;
            if (currentSettlementOwner == mainHero)
            {
                ownedSettlement = true;
            }

            var canGiveMoreGold = false;
            var canOfferFood = false;
            var canKillBeggar = false;

            var stewardAppendedText = "";
            var rogueryAppendedText = "";

            if (GeneralSettings.SkillChecks.IsDisabled())
            {
                canGiveMoreGold = true;
                canKillBeggar = true;
                canOfferFood = true;

                stewardAppendedText =
                    new TextObject("{=BeggarBegging_Skill_Check_Disable_Appended_Text}**Skill checks are disabled**")
                        .ToString();
                rogueryAppendedText =
                    new TextObject("{=BeggarBegging_Skill_Check_Disable_Appended_Text}**Skill checks are disabled**")
                        .ToString();
            }
            else
            {
                if (stewardLevel >= minStewardLevel)
                {
                    canGiveMoreGold = true;

                    stewardAppendedText =
                        new TextObject("{=BeggarBegging_Steward_Appended_Text}[Steward - lvl {minStewardLevel}]")
                            .SetTextVariable("minStewardLevel", minStewardLevel)
                            .ToString();
                }

                if (stewardLevel >= minStewardLevel + 50)
                {
                    canOfferFood = true;

                    stewardAppendedText =
                        new TextObject("{=BeggarBegging_Steward_Appended_Text}[Steward - lvl {minStewardLevel}]")
                            .SetTextVariable("minStewardLevel", minStewardLevel + 50)
                            .ToString();
                }

                if (rogueryLevel >= minRogueryLevel && ownedSettlement)
                {
                    canKillBeggar = true;

                    rogueryAppendedText =
                        new TextObject("{=BeggarBegging_Roguery_Appended_Text}[Roguery - lvl {minRogueryLevel}]")
                            .SetTextVariable("minRogueryLevel", minRogueryLevel)
                            .ToString();
                }
            }

            var gender = new List<string>();
            var age = new List<string>();

            string[] genders =
            {
                new TextObject("{=BeggarBegging_Beggar_Gender_Male}male").ToString(),
                new TextObject("{=BeggarBegging_Beggar_Gender_Female}female").ToString()
            };

            string[] ages =
            {
                new TextObject("{=BeggarBegging_Beggar_Age_Young}a young").ToString(),
                new TextObject("{=BeggarBegging_Beggar_Age_Middle-Aged}a middle-aged").ToString(),
                new TextObject("{=BeggarBegging_Beggar_Age_Old}an old").ToString()
            };

            gender.AddRange(genders);
            age.AddRange(ages);

            var randomGender = new Random();
            var indexGender = randomGender.Next(gender.Count);
            var beggarGender = gender[indexGender];

            var randomAge = new Random();
            var indexAge = randomAge.Next(age.Count);
            var beggarAge = age[indexAge];

            var genderAssignmentObjective = GenderAssignment.GetTheGenderAssignment(beggarGender, false, "objective");
            var genderAssignmentAdjective = GenderAssignment.GetTheGenderAssignment(beggarGender, false, "adjective");
            var genderAssignmentSubjective = GenderAssignment.GetTheGenderAssignment(beggarGender, false, "subjective");
            var genderAssignmentSubjectiveCap =
                GenderAssignment.GetTheGenderAssignment(beggarGender, true, "subjective");

            var eventDescription = new TextObject(
                    "{=BeggarBegging_Event_Desc}While you are relaxing in {currentSettlement} you are approached by {beggarAge} {beggarGender} beggar who asks if you can spare any gold. " +
                    "You wonder what you should do.")
                .SetTextVariable("currentSettlement", currentSettlement)
                .SetTextVariable("beggarAge", beggarAge)
                .SetTextVariable("beggarGender", beggarGender)
                .ToString();

            var eventOption1 = new TextObject("{=BeggarBegging_Event_Option_1}Give {genderAssignmentObjective} nothing")
                .SetTextVariable("genderAssignmentObjective", genderAssignmentObjective).ToString();
            var eventOption1Hover = new TextObject("{=BeggarBegging_Event_Option_1_Hover}Filthy beggar!").ToString();

            var eventOption2 = new TextObject("{=BeggarBegging_Event_Option_2}Give {genderAssignmentObjective} 5 gold")
                .SetTextVariable("genderAssignmentObjective", genderAssignmentObjective).ToString();
            var eventOption2Hover = new TextObject("{=BeggarBegging_Event_Option_2_Hover}It's something.").ToString();

            var eventOption3 =
                new TextObject("{=BeggarBegging_Event_Option_3}[Steward] Give {genderAssignmentObjective} 100 gold")
                    .SetTextVariable("genderAssignmentObjective", genderAssignmentObjective).ToString();
            var eventOption3Hover =
                new TextObject("{=BeggarBegging_Event_Option_3_Hover}You can spare it.\n{stewardAppendedText}")
                    .SetTextVariable("stewardAppendedText", stewardAppendedText).ToString();

            var eventOption4 =
                new TextObject("{=BeggarBegging_Event_Option_4}[Steward] Give {genderAssignmentObjective} a warm meal")
                    .SetTextVariable("genderAssignmentObjective", genderAssignmentObjective).ToString();
            var eventOption4Hover =
                new TextObject("{=BeggarBegging_Event_Option_4_Hover}Take them to the tavern.\n{stewardAppendedText}")
                    .SetTextVariable("stewardAppendedText", stewardAppendedText).ToString();

            var eventOption5 =
                new TextObject("{=BeggarBegging_Event_Option_5}[Roguery] Kill {genderAssignmentObjective}")
                    .SetTextVariable("genderAssignmentObjective", genderAssignmentObjective).ToString();
            var eventOption5Hover =
                new TextObject("{=BeggarBegging_Event_Option_5_Hover}You really hate beggars!\n{rogueryAppendedText}")
                    .SetTextVariable("rogueryAppendedText", rogueryAppendedText).ToString();


            var eventButtonText1 = new TextObject("{=BeggarBegging_Event_Button_Text_1}Choose").ToString();
            var eventButtonText2 = new TextObject("{=BeggarBegging_Event_Button_Text_2}Done").ToString();

            var inquiryElements = new List<InquiryElement>();

            inquiryElements.Add(new InquiryElement("a", eventOption1, null, true, eventOption1Hover));
            inquiryElements.Add(new InquiryElement("b", eventOption2, null, true, eventOption2Hover));
            if (canGiveMoreGold)
            {
                inquiryElements.Add(new InquiryElement("c", eventOption3, null, true, eventOption3Hover));
            }

            if (canOfferFood)
            {
                inquiryElements.Add(new InquiryElement("d", eventOption4, null, true, eventOption4Hover));
            }

            if (canKillBeggar)
            {
                inquiryElements.Add(new InquiryElement("e", eventOption5, null, true, eventOption5Hover));
            }

            var eventOptionAText = new TextObject(
                    "{=BeggarBegging_Event_Choice_1}You tell {genderAssignmentObjective} to f**ck off and that you don't have any gold to spare. {genderAssignmentSubjectiveCap} apologises for troubling you and wishes " +
                    "you a good day. You watch as {genderAssignmentSubjective} disappears into the crowd in search of gold.")
                .SetTextVariable("genderAssignmentObjective", genderAssignmentObjective)
                .SetTextVariable("genderAssignmentSubjectiveCap", genderAssignmentSubjectiveCap)
                .SetTextVariable("genderAssignmentSubjective", genderAssignmentSubjective)
                .ToString();

            var eventOptionBText = new TextObject(
                    "{=BeggarBegging_Event_Choice_2}You hand {genderAssignmentObjective} 5 gold from your pocket. {genderAssignmentSubjectiveCap} shakes you hand and thanks you for this humble gift and wishes you a blessed " +
                    "day. You watch as {genderAssignmentSubjective} disappears into the crowd in search of more gold.")
                .SetTextVariable("genderAssignmentObjective", genderAssignmentObjective)
                .SetTextVariable("genderAssignmentSubjectiveCap", genderAssignmentSubjectiveCap)
                .SetTextVariable("genderAssignmentSubjective", genderAssignmentSubjective)
                .ToString();

            var eventOptionCText = new TextObject(
                    "{=BeggarBegging_Event_Choice_3}You are feeling generous today so you hand {genderAssignmentObjective} 100 gold. {genderAssignmentSubjectiveCap} says {genderAssignmentSubjective} can't accept this, but you " +
                    "assure {genderAssignmentObjective} that it's okay and that you have more than enough for yourself. {genderAssignmentSubjectiveCap} begins to cry as you wrap {genderAssignmentObjective} in a hug and " +
                    "comforts {genderAssignmentObjective}. {genderAssignmentSubjectiveCap} tells you that {genderAssignmentSubjective} now has enough for a warm meal and a bed for a couple of days. Something {genderAssignmentSubjective} " +
                    "has not had for many days. {genderAssignmentSubjectiveCap} thanks you yet again before disappearing towards the tavern to get som warm food.")
                .SetTextVariable("genderAssignmentObjective", genderAssignmentObjective)
                .SetTextVariable("genderAssignmentSubjectiveCap", genderAssignmentSubjectiveCap)
                .SetTextVariable("genderAssignmentSubjective", genderAssignmentSubjective)
                .ToString();

            var eventOptionDText = new TextObject(
                    "{=BeggarBegging_Event_Choice_4}You are feeling generous today so you tell {genderAssignmentObjective} that if {genderAssignmentSubjective} wants to you will take {genderAssignmentObjective} to the " +
                    "tavern and buy {genderAssignmentObjective} some food and something to drink instead of giving gold. {genderAssignmentSubjectiveCap} accepts you proposal and the two of you make your way towards the " +
                    "tavern.\n\nOnce there you are immediately told by the owner that beggars aren't welcome. You then proceed to tell the owner that {genderAssignmentSubjective} is with you. The owner nods in agreement. " +
                    "Once seated you order som hot food and a large drink for yourself and the beggar. As you sit there the two of you naturally begin to chat about various subjects. You learn {genderAssignmentAdjective} " +
                    "story and why {genderAssignmentSubjective} is in the situation {genderAssignmentSubjective}'s currently in. After a few minutes you must depart as you have other matters to attend to. The two of " +
                    "you say your goodbyes and you tell the owner that the beggar is to be allowed to finish {genderAssignmentAdjective} meal and you will pay so {genderAssignmentSubjective} will have one night to sleep. " +
                    "The owner agrees and you leave.")
                .SetTextVariable("genderAssignmentObjective", genderAssignmentObjective)
                .SetTextVariable("genderAssignmentAdjective", genderAssignmentAdjective)
                .SetTextVariable("genderAssignmentSubjectiveCap", genderAssignmentSubjectiveCap)
                .SetTextVariable("genderAssignmentSubjective", genderAssignmentSubjective)
                .ToString();

            var eventOptionEText = new TextObject(
                    "{=BeggarBegging_Event_Choice_5}You tell {genderAssignmentObjective} that if {genderAssignmentSubjective} follows you, you will give {genderAssignmentObjective} 150 gold. {genderAssignmentSubjectiveCap} " +
                    "looks at you suspiciously but ultimately goes with you. You guide {genderAssignmentObjective} into a alley where you make sure that there is no one around. You then proceed to knock the beggar down with " +
                    "one punch. {genderAssignmentSubjectiveCap} falls to the ground and cover {genderAssignmentObjective} head. You punch, kick, drag and push {genderAssignmentObjective} around the alley for a few minutes. " +
                    "When you finally stop {genderAssignmentSubjective} is finally dead.\n\nYou turn to leave but 5 guards approaches you and ask what is going on. You tell the guards the truth. That you don't want filthy " +
                    "beggars in your town and you then order your guards to dispose of the body. The guards reluctantly agrees and one of the guards lifts the body over the shoulder. You emerge from the alley feeling the rush of just " +
                    "killing an innocent person.")
                .SetTextVariable("genderAssignmentObjective", genderAssignmentObjective)
                .SetTextVariable("genderAssignmentSubjectiveCap", genderAssignmentSubjectiveCap)
                .SetTextVariable("genderAssignmentSubjective", genderAssignmentSubjective)
                .ToString();

            var eventMsg1 = new TextObject(
                    "{=BeggarBegging_Event_Msg_1}{heroName} told a beggar to f**k off.")
                .SetTextVariable("heroName", heroName)
                .ToString();

            var eventMsg2 = new TextObject(
                    "{=BeggarBegging_Event_Msg_2}{heroName} gave the beggar 5 gold.")
                .SetTextVariable("heroName", heroName)
                .ToString();

            var eventMsg3 = new TextObject(
                    "{=BeggarBegging_Event_Msg_3}{heroName} gave the beggar 100 gold.")
                .SetTextVariable("heroName", heroName)
                .ToString();

            var eventMsg4 = new TextObject(
                    "{=BeggarBegging_Event_Msg_4}{heroName} took the beggar to get some food.")
                .SetTextVariable("heroName", heroName)
                .ToString();

            var eventMsg5 = new TextObject(
                    "{=BeggarBegging_Event_Msg_5}{heroName} killed an innocent beggar.")
                .SetTextVariable("heroName", heroName)
                .ToString();

            var msid = new MultiSelectionInquiryData(eventTitle, eventDescription, inquiryElements, false, 1, 1,
                eventButtonText1, null,
                elements =>
                {
                    switch ((string)elements[0].Identifier)
                    {
                        case "a":
                            InformationManager.ShowInquiry(
                                new InquiryData(eventTitle, eventOptionAText, true, false, eventButtonText2, null, null,
                                    null), true);

                            InformationManager.DisplayMessage(new InformationMessage(eventMsg1,
                                RandomEventsSubmodule.Msg_Color_NEG_Outcome));
                            break;
                        case "b":
                            InformationManager.ShowInquiry(
                                new InquiryData(eventTitle, eventOptionBText, true, false, eventButtonText2, null, null,
                                    null), true);

                            Hero.MainHero.ChangeHeroGold(-5);
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg2,
                                RandomEventsSubmodule.Msg_Color_MED_Outcome));
                            break;
                        case "c":
                            InformationManager.ShowInquiry(
                                new InquiryData(eventTitle, eventOptionCText, true, false, eventButtonText2, null, null,
                                    null), true);

                            Hero.MainHero.ChangeHeroGold(-100);
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg3,
                                RandomEventsSubmodule.Msg_Color_POS_Outcome));

                            break;
                        case "d":
                            InformationManager.ShowInquiry(
                                new InquiryData(eventTitle, eventOptionDText, true, false, eventButtonText2, null, null,
                                    null), true);

                            Hero.MainHero.ChangeHeroGold(-100);
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg4,
                                RandomEventsSubmodule.Msg_Color_POS_Outcome));

                            break;
                        case "e":
                            InformationManager.ShowInquiry(
                                new InquiryData(eventTitle, eventOptionEText, true, false, eventButtonText2, null, null,
                                    null), true);

                            InformationManager.DisplayMessage(new InformationMessage(eventMsg5,
                                RandomEventsSubmodule.Msg_Color_EVIL_Outcome));

                            break;
                        default:
                            MessageBox.Show($"Error while selecting option for \"{randomEventData.eventType}\"");
                            break;
                    }
                },
                null, null);

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
                MessageBox.Show(
                    $"Error while stopping \"{randomEventData.eventType}\" event :\n\n {ex.Message} \n\n {ex.StackTrace}");
            }
        }
    }


    public class BeggarBeggingData : RandomEventData
    {
        public BeggarBeggingData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
        {
        }

        public override BaseEvent GetBaseEvent()
        {
            return new BeggarBegging();
        }
    }
}