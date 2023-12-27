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
                return minStewardLevel != 0 || minRogueryLevel != 0;
            }
            
            return false;
        }

        public override bool CanExecuteEvent()
        {
           
            return HasValidEventData() && MobileParty.MainParty.CurrentSettlement != null && (MobileParty.MainParty.CurrentSettlement.IsTown || MobileParty.MainParty.CurrentSettlement.IsVillage);
        }

        public override void StartEvent()
        {
            var mainHero = Hero.MainHero;

            var heroName = mainHero.FirstName;

            var eventTitle = new TextObject(EventTextHandler.GetRandomEventTitle()).ToString();

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

            /*
             * her or him
             */
            var genderAssignmentObjective = GenderAssignment.GetTheGenderAssignment(beggarGender, false, "objective");
            
            /*
             * her or his
             */
            var genderAssignmentAdjective = GenderAssignment.GetTheGenderAssignment(beggarGender, false, "adjective");
            
            /*
             * she or he
             */
            var genderAssignmentSubjective = GenderAssignment.GetTheGenderAssignment(beggarGender, false, "subjective");
            var genderAssignmentSubjectiveCap = GenderAssignment.GetTheGenderAssignment(beggarGender, true, "subjective");
            

            var eventDescription = new TextObject(EventTextHandler.GetRandomEventDescription())
                .SetTextVariable("currentSettlement", currentSettlement)
                .SetTextVariable("beggarAge", beggarAge)
                .SetTextVariable("beggarGender", beggarGender)
                .SetTextVariable("genderAssignmentSubjective", genderAssignmentSubjective)
                .SetTextVariable("genderAssignmentAdjective", genderAssignmentAdjective)
                .SetTextVariable("genderAssignmentObjective", genderAssignmentObjective)
                .SetTextVariable("genderAssignmentSubjectiveCap", genderAssignmentSubjectiveCap)
                .ToString();

            var eventOption1 = new TextObject("{=BeggarBegging_Event_Option_1}Give {genderAssignmentObjective} nothing")
                .SetTextVariable("genderAssignmentObjective", genderAssignmentObjective).ToString();
            var eventOption1Hover = new TextObject("{=BeggarBegging_Event_Option_1_Hover}Filthy beggar!").ToString();

            var eventOption2 = new TextObject("{=BeggarBegging_Event_Option_2}Give {genderAssignmentObjective} 5 gold")
                .SetTextVariable("genderAssignmentObjective", genderAssignmentObjective).ToString();
            var eventOption2Hover = new TextObject("{=BeggarBegging_Event_Option_2_Hover}It's something.").ToString();

            var eventOption3 = new TextObject("{=BeggarBegging_Event_Option_3}[Steward] Give {genderAssignmentObjective} 100 gold")
                .SetTextVariable("genderAssignmentObjective", genderAssignmentObjective).ToString();
            var eventOption3Hover = new TextObject("{=BeggarBegging_Event_Option_3_Hover}You can spare it.\n{stewardAppendedText}")
                .SetTextVariable("stewardAppendedText", stewardAppendedText).ToString();

            var eventOption4 = new TextObject("{=BeggarBegging_Event_Option_4}[Steward] Give {genderAssignmentObjective} a warm meal")
                .SetTextVariable("genderAssignmentObjective", genderAssignmentObjective).ToString();
            var eventOption4Hover = new TextObject("{=BeggarBegging_Event_Option_4_Hover}Take them to the tavern.\n{stewardAppendedText}")
                .SetTextVariable("stewardAppendedText", stewardAppendedText).ToString();

            var eventOption5 = new TextObject("{=BeggarBegging_Event_Option_5}[Roguery] Kill {genderAssignmentObjective}")
                .SetTextVariable("genderAssignmentObjective", genderAssignmentObjective).ToString();
            var eventOption5Hover = new TextObject("{=BeggarBegging_Event_Option_5_Hover}You really hate beggars!\n{rogueryAppendedText}")
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

            var eventOptionAText = new TextObject(EventTextHandler.GetRandomEventChoice1())
                .SetTextVariable("genderAssignmentSubjective", genderAssignmentSubjective)
                .SetTextVariable("genderAssignmentAdjective", genderAssignmentAdjective)
                .SetTextVariable("genderAssignmentObjective", genderAssignmentObjective)
                .SetTextVariable("genderAssignmentSubjectiveCap", genderAssignmentSubjectiveCap)
                .ToString();

            var eventOptionBText = new TextObject(EventTextHandler.GetRandomEventChoice2())
                .SetTextVariable("genderAssignmentSubjective", genderAssignmentSubjective)
                .SetTextVariable("genderAssignmentAdjective", genderAssignmentAdjective)
                .SetTextVariable("genderAssignmentObjective", genderAssignmentObjective)
                .SetTextVariable("genderAssignmentSubjectiveCap", genderAssignmentSubjectiveCap)
                .ToString();

            var eventOptionCText = new TextObject(EventTextHandler.GetRandomEventChoice3())
                .SetTextVariable("genderAssignmentSubjective", genderAssignmentSubjective)
                .SetTextVariable("genderAssignmentAdjective", genderAssignmentAdjective)
                .SetTextVariable("genderAssignmentObjective", genderAssignmentObjective)
                .SetTextVariable("genderAssignmentSubjectiveCap", genderAssignmentSubjectiveCap)
                .ToString();

            var eventOptionDText = new TextObject(EventTextHandler.GetRandomEventChoice4())
                .SetTextVariable("genderAssignmentSubjective", genderAssignmentSubjective)
                .SetTextVariable("genderAssignmentAdjective", genderAssignmentAdjective)
                .SetTextVariable("genderAssignmentObjective", genderAssignmentObjective)
                .SetTextVariable("genderAssignmentSubjectiveCap", genderAssignmentSubjectiveCap)
                .ToString();

            var eventOptionEText = new TextObject(EventTextHandler.GetRandomEventChoice5())
                .SetTextVariable("genderAssignmentSubjective", genderAssignmentSubjective)
                .SetTextVariable("genderAssignmentAdjective", genderAssignmentAdjective)
                .SetTextVariable("genderAssignmentObjective", genderAssignmentObjective)
                .SetTextVariable("genderAssignmentSubjectiveCap", genderAssignmentSubjectiveCap)
                .ToString();

            var eventMsg1 = new TextObject(EventTextHandler.GetRandomEventMessage1())
                .SetTextVariable("heroName", heroName)
                .ToString();

            var eventMsg2 = new TextObject(EventTextHandler.GetRandomEventMessage2())
                .SetTextVariable("heroName", heroName)
                .ToString();

            var eventMsg3 = new TextObject(EventTextHandler.GetRandomEventMessage3())
                .SetTextVariable("heroName", heroName)
                .ToString();

            var eventMsg4 = new TextObject(EventTextHandler.GetRandomEventMessage4())
                .SetTextVariable("heroName", heroName)
                .ToString();

            var eventMsg5 = new TextObject(EventTextHandler.GetRandomEventMessage5())
                .SetTextVariable("heroName", heroName)
                .ToString();

            var msid = new MultiSelectionInquiryData(eventTitle, eventDescription, inquiryElements, false, 1, 1,
                eventButtonText1, null,
                elements =>
                {
                    switch ((string)elements[0].Identifier)
                    {
                        case "a":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionAText, true, false, eventButtonText2, null, null, null), true);

                            InformationManager.DisplayMessage(new InformationMessage(eventMsg1, RandomEventsSubmodule.Msg_Color_NEG_Outcome));
                            break;
                        case "b":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionBText, true, false, eventButtonText2, null, null, null), true);

                            Hero.MainHero.ChangeHeroGold(-5);
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg2, RandomEventsSubmodule.Msg_Color_MED_Outcome));
                            break;
                        case "c":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionCText, true, false, eventButtonText2, null, null, null), true);

                            Hero.MainHero.ChangeHeroGold(-100);
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg3, RandomEventsSubmodule.Msg_Color_POS_Outcome));

                            break;
                        case "d":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionDText, true, false, eventButtonText2, null, null, null), true);

                            Hero.MainHero.ChangeHeroGold(-100);
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg4, RandomEventsSubmodule.Msg_Color_POS_Outcome));

                            break;
                        case "e":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionEText, true, false, eventButtonText2, null, null, null), true);

                            InformationManager.DisplayMessage(new InformationMessage(eventMsg5, RandomEventsSubmodule.Msg_Color_EVIL_Outcome));

                            break;
                        default:
                            MessageBox.Show($"Error while selecting option for \"{randomEventData.eventType}\"");
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
                MessageBox.Show(
                    $"Error while stopping \"{randomEventData.eventType}\" event :\n\n {ex.Message} \n\n {ex.StackTrace}");
            }
        }
        
        private static class EventTextHandler
        {
            private static readonly Random random = new Random();
            
            private static readonly List<string> eventTitles = new List<string>
            {
                "{=BeggarBegging_Title_A}Beggar's Lament",
                "{=BeggarBegging_Title_B}Pleading Pauper",
                "{=BeggarBegging_Title_C}Begging Hands",
                "{=BeggarBegging_Title_D}Street Appeal",
                "{=BeggarBegging_Title_E}Alms Seeker",
                "{=BeggarBegging_Title_F}Beggar's Request",
                "{=BeggarBegging_Title_G}Poverty's Plea",
                "{=BeggarBegging_Title_H}Street Beggar's Cry"
            };
            
            private static readonly List<string> eventDescriptions = new List<string>
            {
                //Event Description A
                "{=BeggarBegging_Event_Desc_A}While taking a break in {currentSettlement}, you're approached by a " +
                "{beggarAge} {beggarGender} beggar. The individual stands before you, {genderAssignmentSubjective} eyes " +
                "conveying hope and resignation. In a humble voice, {genderAssignmentSubjective} ask if you could spare" +
                " some gold. Observing {genderAssignmentSubjective}, you can't help but ponder {genderAssignmentAdjective} " +
                "story and the circumstances that led {genderAssignmentObjective} to this moment. You find yourself" +
                " contemplating how to respond, considering the impact of your decision on both {genderAssignmentObjective} " +
                "life and yours.",
                
                //Event Description B
                "{=BeggarBegging_Event_Desc_B}As you relax in the bustling environment of {currentSettlement}, your attention " +
                "is drawn to a {beggarAge} {beggarGender} beggar who approaches you with a hesitant step. The beggar's eyes, " +
                "weary yet hopeful, meet yours as {genderAssignmentSubjective} extend a trembling hand, asking for some " +
                "gold. The moment stretches out as you consider {genderAssignmentAdjective} plight, the hardships " +
                "{genderAssignmentSubjective} must have faced, and the difference a simple act of kindness could" +
                " make in {genderAssignmentObjective} life.",

                //Event Description C
                "{=BeggarBegging_Event_Desc_C}During your leisure time in {currentSettlement}, you are approached by " +
                "a {beggarAge} {beggarGender} beggar, whose presence interrupts your thoughts. {genderAssignmentSubjectiveCap} " +
                "stand there, a figure of need and desperation, softly asking for whatever gold you can spare. " +
                "The encounter prompts you to reflect on {genderAssignmentAdjective} life story, the struggles " +
                "{genderAssignmentSubjective} may have endured, and the moral implications of your next action.",

                //Event Description D
                "{=BeggarBegging_Event_Desc_D}While enjoying a moment of peace in {currentSettlement}, your solitude " +
                "is broken by the arrival of a {beggarAge} {beggarGender} beggar. With a look that speaks volumes of " +
                "{genderAssignmentAdjective} life's challenges, {genderAssignmentSubjective} ask you for some gold. " +
                "This unexpected meeting makes you pause and think about the larger picture of society, " +
                "the unseen struggles of its less fortunate members, and the role you play in this intricate tapestry.",

                //Event Description E
                "{=BeggarBegging_Event_Desc_E}In the midst of your relaxation in {currentSettlement}, a {beggarAge} "+
                "{beggarGender} beggar timidly makes {genderAssignmentAdjective} way towards you. {genderAssignmentSubjectiveCap} " +
                "ask for assistance, a reflection of the broader societal issues and the personal stories of struggle. " +
                "You ponder how a small gesture from you could potentially add a glimmer of hope to " +
                "{genderAssignmentAdjective} challenging journey."

            };
            
            private static readonly List<string> eventChoice1 = new List<string>
            {
                //Event Choice 1A
                "{=BeggarBegging_Event_Choice_1A}You tell {genderAssignmentObjective} to f**ck off and that you don't " +
                "have any gold to spare. {genderAssignmentSubjectiveCap} apologises for troubling you and wishes " +
                "you a good day. You watch as {genderAssignmentSubjective} disappears into the crowd in search of gold.",
                
                //Event Choice 1B
                "{=BeggarBegging_Event_Choice_1B}You brusquely inform {genderAssignmentObjective} that you have no gold to give. " +
                "{genderAssignmentSubjectiveCap} offers a quick apology and bids you farewell. You observe " +
                "{genderAssignmentSubjective} as {genderAssignmentSubjective} blends back into the busy streets.",
                
                //Event Choice 1C
                "{=BeggarBegging_Event_Choice_1C}You curtly reject {genderAssignmentObjective}, stating your lack of spare gold. " +
                "{genderAssignmentSubjectiveCap} nods understandingly and departs quietly. You see {genderAssignmentSubjective} " +
                "weaving through the crowd, continuing {genderAssignmentAdjective} quest.",
                
                //Event Choice 1D
                "{=BeggarBegging_Event_Choice_1D}You dismiss {genderAssignmentObjective} harshly, claiming you have nothing " +
                "to offer. {genderAssignmentSubjectiveCap} expresses regret for the disturbance and moves on. You glance as " +
                "{genderAssignmentSubjective} vanishes among the bustling throng.",
                
                //Event Choice 1E
                "{=BeggarBegging_Event_Choice_1E}You sharply tell {genderAssignmentObjective} that you can't help. " +
                "{genderAssignmentSubjectiveCap} apologizes and leaves respectfully. Watching, you notice " +
                "{genderAssignmentSubjective} merge into the sea of people, still seeking assistance."
            };
            
            private static readonly List<string> eventChoice2 = new List<string>
            {
                //Event Choice 2A
                "{=BeggarBegging_Event_Choice_2A}You hand {genderAssignmentObjective} 5 gold from your pocket. " +
                "{genderAssignmentSubjectiveCap} shakes your hand and thanks you for this humble gift and wishes " +
                "you a blessed day. You watch as {genderAssignmentSubjective} disappears into the crowd in " +
                "search of more gold.",
                
                //Event Choice 2B
                "{=BeggarBegging_Event_Choice_2B}You give {genderAssignmentObjective} 5 gold coins. " +
                "{genderAssignmentSubjectiveCap} gratefully clasps your hand, expressing heartfelt " +
                "thanks for your generosity and blessing your day. You see {genderAssignmentSubjective} " +
                "blend into the crowd, still seeking aid.",
                
                //Event Choice 2C
                "{=BeggarBegging_Event_Choice_2C}You pass 5 gold pieces to {genderAssignmentObjective}. " +
                "{genderAssignmentSubjectiveCap} warmly thanks you for your kindness, offering blessings for " +
                "your journey ahead. You observe {genderAssignmentSubjective} as {genderAssignmentSubjective} " +
                "merges back into the throngs of people.",
                
                //Event Choice 2D
                "{=BeggarBegging_Event_Choice_2D}You slip 5 gold into {genderAssignmentObjective}'s hand. " +
                "{genderAssignmentSubjectiveCap} offers a thankful handshake and bestows well wishes upon you. " +
                "You watch {genderAssignmentSubjective} vanish into the crowd, continuing " +
                "{genderAssignmentAdjective} plight.",
                
                //Event Choice 2E
                "\"{=BeggarBegging_Event_Choice_2E}You extend 5 gold to {genderAssignmentObjective}. " +
                "{genderAssignmentSubjectiveCap} gratefully acknowledges your gift with a " +
                "handshake and kind words for a good day. You glance as {genderAssignmentSubjective} " +
                "fades into the bustling crowd."
                
            };
            
            private static readonly List<string> eventChoice3 = new List<string>
            {
                //Event Choice 3A
                "{=BeggarBegging_Event_Choice_3A}You are feeling generous today so you hand {genderAssignmentObjective} " +
                "100 gold. {genderAssignmentSubjectiveCap} says {genderAssignmentSubjective} can't accept this, but you " +
                "assure {genderAssignmentObjective} that it's okay and that you have more than enough for yourself. " +
                "{genderAssignmentSubjectiveCap} begins to cry as you wrap {genderAssignmentObjective} in a hug and comfort " +
                "{genderAssignmentObjective}. {genderAssignmentSubjectiveCap} tells you that {genderAssignmentSubjective}" +
                " now has enough for a warm meal and a bed for a couple of days. Something {genderAssignmentSubjective} " +
                "has not had for many days. {genderAssignmentSubjectiveCap} thanks you yet again before disappearing " +
                "towards the tavern to get some warm food.",
                
                //Event Choice 3B
                "{=BeggarBegging_Event_Choice_3B}Moved by generosity, you offer {genderAssignmentObjective} 100 gold. " +
                "Initially hesitant, {genderAssignmentSubjectiveCap} eventually accepts after your reassurance. " +
                "Overwhelmed, {genderAssignmentSubjectiveCap} breaks down in tears. You comfort {genderAssignmentObjective} " +
                "with a warm embrace. {genderAssignmentSubjectiveCap} gratefully shares how this means a hot meal and a " +
                "place to sleep, luxuries {genderAssignmentSubjective} hasn't had in a while. After heartfelt thanks, " +
                "{genderAssignmentSubjective} heads off to the tavern.",
                
                //Event Choice 3C
                "{=BeggarBegging_Event_Choice_3C}In a generous mood, you give {genderAssignmentObjective} 100 gold. " +
                "{genderAssignmentSubjectiveCap} is initially reluctant, but you insist it's no burden. Tears well " +
                "up in {genderAssignmentSubjective}'s eyes as you offer a comforting hug. {genderAssignmentSubjectiveCap} " +
                "explains this gift means a few days of food and shelter, a rarity for {genderAssignmentSubjective}. " +
                "With renewed gratitude, {genderAssignmentSubjective} heads towards the tavern for a much-needed meal.",
                
                //Event Choice 3D
                "{=BeggarBegging_Event_Choice_3D}Feeling kind, you present {genderAssignmentObjective} with 100 gold. " +
                "{genderAssignmentSubjectiveCap} tries to refuse, but your reassurance prevails. " +
                "{genderAssignmentSubjectiveCap} tearfully embraces you, grateful for the chance at a warm meal and " +
                "shelter, things {genderAssignmentSubjective} has missed greatly. After thanking you profusely, " +
                "{genderAssignmentSubjective} makes {genderAssignmentAdjective} way to the tavern.",
                
                //Event Choice 3E
                "{=BeggarBegging_Event_Choice_3E}With a generous heart, you hand over 100 gold to {genderAssignmentObjective}. " +
                "{genderAssignmentSubjectiveCap} hesitates, but your encouragement convinces {genderAssignmentObjective}. " +
                "{genderAssignmentSubjectiveCap}'s tears flow as you give a comforting hug. {genderAssignmentSubjectiveCap} " +
                "shares how this means a rare few days of comfort and food. Expressing deep gratitude, " +
                "{genderAssignmentSubjective} sets off for the tavern."
            };
            
            private static readonly List<string> eventChoice4= new List<string>
            {
                //Event Choice 4A
                "{=BeggarBegging_Event_Choice_4A}You are feeling generous today so you tell {genderAssignmentObjective} that " +
                "if {genderAssignmentSubjective} wants to, you will take {genderAssignmentObjective} to the tavern and buy " +
                "{genderAssignmentObjective} some food and something to drink instead of giving gold. " +
                "{genderAssignmentSubjectiveCap} accepts your proposal, and the two of you make your way towards the " +
                "tavern.\n\nOnce there, you are immediately told by the owner that beggars aren't welcome. You then proceed " +
                "to tell the owner that {genderAssignmentSubjective} is with you. The owner nods in agreement. Once seated, " +
                "you order some hot food and a large drink for yourself and the beggar. As you sit there, the two of you " +
                "naturally begin to chat about various subjects. You learn {genderAssignmentSubjective}'s story and why " +
                "{genderAssignmentSubjective} is in the situation {genderAssignmentSubjective} is currently in. After a " +
                "few minutes, you must depart as you have other matters to attend to. The two of you say your goodbyes, and " +
                "you tell the owner that the beggar is to be allowed to finish {genderAssignmentObjective} meal and you will " +
                "pay, so {genderAssignmentSubjective} will have one night to sleep. The owner agrees, and you leave.",
                
                //Event Choice 4B
                "{=BeggarBegging_Event_Choice_4B}Feeling charitable, you offer to treat {genderAssignmentObjective} to a meal" +
                " and drink at the tavern, rather than giving gold. {genderAssignmentSubjectiveCap} gratefully accepts, and " +
                "together you head to the tavern.\n\nUpon arrival, the tavern owner initially objects to the beggar's presence " +
                "but relents when you explain {genderAssignmentSubjective} is your guest. After ordering a hearty meal and drinks, " +
                "you engage in conversation, learning about {genderAssignmentSubjective}'s life and current predicament. Time " +
                "flies, and soon you have to leave for other commitments. Before departing, you ensure the tavern owner allows " +
                "the beggar to complete the meal and stay the night, at your expense.",
                
                //Event Choice 4C
                "{=BeggarBegging_Event_Choice_4C}Today, your generosity leads you to offer {genderAssignmentObjective} a meal " +
                "at the tavern instead of gold. {genderAssignmentSubjectiveCap} happily agrees, and you both set off.\n\nAt " +
                "the tavern, the owner hesitates to welcome a beggar, but you assert that {genderAssignmentSubjective} is " +
                "your companion. Settled in, you order warm meals and drinks, and begin to converse, uncovering " +
                "{genderAssignmentSubjective}'s backstory and struggles. As time to leave nears, you instruct the tavern " +
                "owner to let the beggar finish the meal and provide a night's lodging, all on your tab.",
                
                //Event Choice 4D
                "{=BeggarBegging_Event_Choice_4D}In a generous mood, you suggest taking {genderAssignmentObjective} for " +
                "a meal at the tavern, offering companionship over gold. {genderAssignmentSubjectiveCap} eagerly agrees, " +
                "and you both go to the tavern.\n\nInitially, the tavern owner disapproves of the beggar's presence, but " +
                "you persuade them that {genderAssignmentSubjective} is with you. While dining and drinking, you delve " +
                "into {genderAssignmentSubjective}'s story, understanding more about {genderAssignmentSubjective}'s hardships. " +
                "When it's time to leave, you arrange with the owner for the beggar to finish their meal and spend the " +
                "night, assuring payment for both.",
                
                //Event Choice 4E
                "{=BeggarBegging_Event_Choice_4E}With a heart full of kindness, you decide to provide " +
                "{genderAssignmentObjective} with a tavern meal instead of gold. {genderAssignmentSubjectiveCap} " +
                "accepts with gratitude, and you head to the local tavern.\n\nThe tavern owner initially balks at " +
                "a beggar's entry but acquiesces when you intervene. Over a meal and drinks, you engage in meaningful " +
                "dialogue, learning about {genderAssignmentSubjective}'s circumstances. As you prepare to leave, you speak " +
                "to the tavern owner, ensuring that the beggar can finish their meal and have a place to sleep for " +
                "the night, all covered by you."
            };
            
            private static readonly List<string> eventChoice5= new List<string>
            {
                //Event Choice 5A
                "{=BeggarBegging_Event_Choice_5}You tell {genderAssignmentObjective} that following you will result in " +
                "receiving 150 gold. Initially, {genderAssignmentSubjectiveCap} regards you with suspicion but eventually " +
                "acquiesces, intrigued by the offer. Leading {genderAssignmentObjective} into a deserted alley, you ensure " +
                "no one is around to witness what follows. With a swift and powerful punch, you knock the beggar to the " +
                "ground. {genderAssignmentSubjectiveCap} curls up, trying to protect {genderAssignmentObjective}self, as " +
                "you unleash a flurry of punches, kicks, and shoves. The assault is relentless and merciless, continuing " +
                "for several long, brutal minutes. When you finally cease, {genderAssignmentSubjective} is lifeless. As " +
                "you prepare to leave the scene, you're met by a group of five guards. They question your actions, to which " +
                "you reply candidly, expressing your disdain for beggars and ordering them to dispose of the body. The guards, " +
                "showing visible reluctance and discomfort, comply with your order. One of them hoists the lifeless body over " +
                "their shoulder. As you step out of the alley, a sense of exhilaration from your actions courses through you, " +
                "mixed with a sense of power and control.",
                
                //Event Choice 4B
                "{=BeggarBegging_Event_Choice_5B}You cleverly persuade {genderAssignmentObjective} to accompany you with " +
                "the promise of 150 gold coins. Despite {genderAssignmentSubjective}'s initial hesitation and suspicion, " +
                "the allure of the gold sways {genderAssignmentSubjective}. You lead {genderAssignmentObjective} into a " +
                "quiet, secluded alley, making sure no one is nearby to intervene. Once isolated, you launch a sudden and " +
                "vicious attack on the unsuspecting beggar. {genderAssignmentSubjectiveCap} falls to the ground, " +
                "struggling to defend {genderAssignmentObjective}self from your relentless and ferocious onslaught. Your " +
                "fists and feet move in a blur, each blow delivered with calculated brutality. After several agonizing " +
                "minutes, the beggar lies motionless on the ground, a victim of your unfettered rage. As you exit the alley, " +
                "you are immediately confronted by a patrol of five guards. Without hesitation, you admit to your actions " +
                "and express your desire to rid the town of beggars. You coldly command them to dispose of the body. Despite " +
                "their visible discomfort and moral conflict, they obey your orders. Watching the guard carry the body " +
                "away, you feel a rush of adrenaline and a sense of twisted satisfaction.",
                
                //Event Choice 4C
                "{=BeggarBegging_Event_Choice_5}You tell {genderAssignmentObjective} that following you will result in " +
                "receiving 150 gold. Initially, {genderAssignmentSubjectiveCap} regards you with suspicion but eventually " +
                "acquiesces, intrigued by the offer. Leading {genderAssignmentObjective} into a deserted alley, you " +
                "ensure no one is around to witness what follows. With a swift and powerful punch, you knock the beggar " +
                "to the ground. {genderAssignmentSubjectiveCap} curls up, trying to protect {genderAssignmentObjective}self, " +
                "as you unleash a flurry of punches, kicks, and shoves. The assault is relentless and merciless, continuing " +
                "for several long, brutal minutes. When you finally cease, {genderAssignmentSubjective} is lifeless. As you " +
                "prepare to leave the scene, you're met by a group of five guards. They question your actions, to which you " +
                "reply candidly, expressing your disdain for beggars and ordering them to dispose of the body. The guards, " +
                "showing visible reluctance and discomfort, comply with your order. One of them hoists the lifeless body over " +
                "their shoulder. As you step out of the alley, a sense of exhilaration from your actions courses through you, " +
                "mixed with a sense of power and control.",
                
                //Event Choice 4D
                "{=BeggarBegging_Event_Choice_5D}With a manipulative charm, you lure {genderAssignmentObjective} with the " +
                "lucrative offer of 150 gold coins. Despite {genderAssignmentSubjective}'s initial mistrust, the prospect " +
                "of such wealth proves too tempting, and {genderAssignmentSubjectiveCap} cautiously follows you. You lead " +
                "{genderAssignmentObjective} to an obscure alley, far from prying eyes, where you swiftly turn violent. You " +
                "strike the beggar with a powerful blow that sends {genderAssignmentObjective} crashing to the ground. As " +
                "{genderAssignmentSubjectiveCap} tries to shield {genderAssignmentObjective}self, you continue your assault " +
                "with an unbridled ferocity. The beggar's feeble attempts to protect {genderAssignmentObjective}self are no " +
                "match for your ruthless aggression. After several minutes of continuous abuse, the beggar lies still, life " +
                "extinguished by your relentless onslaught. As you exit the alley, a group of five guards confronts you, " +
                "inquiring about the disturbance. You openly admit to your actions, declaring your distaste for beggars and " +
                "commanding the guards to dispose of the body. Despite their apparent unease, the guards comply with your order, " +
                "one of them shouldering the responsibility of carrying the deceased. You leave the alley, a complex mix of " +
                "exhilaration and power washing over you, your actions leaving an indelible mark on your psyche.",
                
                //Event Choice 4E
                "{=BeggarBegging_Event_Choice_5E}With a blend of cunning and generosity, you deceive {genderAssignmentObjective} " +
                "with an offer of 150 gold, convincing {genderAssignmentSubjective} to follow you. {genderAssignmentSubjectiveCap} " +
                "cautiously agrees, lured by the promise of the gold. You take {genderAssignmentObjective} to a remote alley, " +
                "ensuring no witnesses are present. Once isolated, you unleash a brutal assault on the beggar, delivering a " +
                "crushing punch that sends {genderAssignmentObjective} reeling to the ground. {genderAssignmentSubjectiveCap} " +
                "tries in vain to defend {genderAssignmentObjective}self as you continue your merciless beating. The beggar's " +
                "struggles gradually cease as your blows continue unabated. When the violence ends, {genderAssignmentSubjective} " +
                "lies lifeless on the alley floor. As you step out, you're met by a group of five guards who question your actions." +
                " Unabashedly, you confess to the murder, explaining your intention to rid the town of beggars. You then command " +
                "the guards to remove the body, asserting your authority. The guards, visibly disturbed, follow your orders. One of " +
                "them solemnly lifts the lifeless body, carrying it away. As you walk away from the alley, you feel an intense " +
                "rush of power and dominance, the act of taking a life leaving a profound and dark impact on you."
            };
            
            private static readonly List<string> eventMsg1 = new List<string>
            {
                "{=BeggarBegging_Event_Msg_1A}{heroName} told a beggar to f**k off.",
                "{=BeggarBegging_Event_Msg_1B}{heroName} harshly dismissed a beggar with a curt 'f**k off.'",
                "{=BeggarBegging_Event_Msg_1C}{heroName} bluntly sent a beggar away, telling them to f**k off.",
                "{=BeggarBegging_Event_Msg_1D}{heroName} brusquely told a beggar to f**k off, refusing any interaction.",
                "{=BeggarBegging_Event_Msg_1E}{heroName} had no patience for a beggar and curtly told them to f**k off."

            };
            
            private static readonly List<string> eventMsg2 = new List<string>
            {
                "{=BeggarBegging_Event_Msg_2A}{heroName} gave the beggar 5 gold.",
                "{=BeggarBegging_Event_Msg_2B}{heroName} handed 5 gold to the beggar.",
                "{=BeggarBegging_Event_Msg_2C}{heroName} generously gave 5 gold coins to the beggar.",
                "{=BeggarBegging_Event_Msg_2D}{heroName} offered the beggar a gift of 5 gold.",
                "{=BeggarBegging_Event_Msg_2E}{heroName} presented 5 gold to the grateful beggar."
            };
            
            private static readonly List<string> eventMsg3 = new List<string>
            {
                "{=BeggarBegging_Event_Msg_3A}{heroName} gave the beggar 100 gold.",
                "{=BeggarBegging_Event_Msg_3B}{heroName} generously bestowed 100 gold upon the beggar.",
                "{=BeggarBegging_Event_Msg_3C}{heroName} handed over 100 gold coins to the beggar.",
                "{=BeggarBegging_Event_Msg_3D}{heroName} presented the beggar with a sum of 100 gold.",
                "{=BeggarBegging_Event_Msg_3E}{heroName} graciously offered 100 gold to the beggar."
            };
            
            private static readonly List<string> eventMsg4 = new List<string>
            { 
                "{=BeggarBegging_Event_Msg_4A}{heroName} took the beggar to get some food.",
                "{=BeggarBegging_Event_Msg_4B}{heroName} escorted the beggar to a meal.",
                "{=BeggarBegging_Event_Msg_4C}{heroName} led the beggar to dine.",
                "{=BeggarBegging_Event_Msg_4D}{heroName} accompanied the beggar for a food treat.",
                "{=BeggarBegging_Event_Msg_4E}{heroName} guided the beggar to a place for food."
            };
            
            private static readonly List<string> eventMsg5 = new List<string>
            { 
                "{=BeggarBegging_Event_Msg_5A}{heroName} killed an innocent beggar.",
                "{=BeggarBegging_Event_Msg_5B}{heroName} took the life of a defenseless beggar.",
                "{=BeggarBegging_Event_Msg_5C}{heroName} ended the life of a hapless beggar.",
                "{=BeggarBegging_Event_Msg_5D}{heroName} fatally attacked an unsuspecting beggar.",
                "{=BeggarBegging_Event_Msg_5E}{heroName} committed the act of killing a beggar."
            };

            
            public static string GetRandomEventTitle()
            {
                var index = random.Next(eventTitles.Count);
                return eventTitles[index];
            }
            
            public static string GetRandomEventDescription()
            {
                var index = random.Next(eventDescriptions.Count);
                return eventDescriptions[index];
            }
            
            public static string GetRandomEventChoice1()
            {
                var index = random.Next(eventChoice1.Count);
                return eventChoice1[index];
            }
            
            public static string GetRandomEventChoice2()
            {
                var index = random.Next(eventChoice2.Count);
                return eventChoice2[index];
            }
            
            public static string GetRandomEventChoice3()
            {
                var index = random.Next(eventChoice3.Count);
                return eventChoice3[index];
            }
            
            public static string GetRandomEventChoice4()
            {
                var index = random.Next(eventChoice4.Count);
                return eventChoice4[index];
            }
            
            public static string GetRandomEventChoice5()
            {
                var index = random.Next(eventChoice5.Count);
                return eventChoice5[index];
            }
            
            public static string GetRandomEventMessage1()
            {
                var index = random.Next(eventMsg1.Count);
                return eventMsg1[index];
            }
            
            public static string GetRandomEventMessage2()
            {
                var index = random.Next(eventMsg2.Count);
                return eventMsg2[index];
            }
            
            public static string GetRandomEventMessage3()
            {
                var index = random.Next(eventMsg3.Count);
                return eventMsg3[index];
            }
            
            public static string GetRandomEventMessage4()
            {
                var index = random.Next(eventMsg4.Count);
                return eventMsg4[index];
            }
            
            public static string GetRandomEventMessage5()
            {
                var index = random.Next(eventMsg5.Count);
                return eventMsg5[index];
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