using System;
using System.Collections.Generic;
using System.Windows;
using Bannerlord.RandomEvents.Helpers;
using Bannerlord.RandomEvents.Settings;
using Ini.Net;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace Bannerlord.RandomEvents.Events.CCEvents
{
    public sealed class BirthdayParty : BaseEvent
    {
        private readonly bool eventDisabled;
        private readonly int minAttending;
        private readonly int maxAttending;
        private readonly int minYourMenAttending;
        private readonly int maxYourMenAttending;
        private readonly int minAge;
        private readonly int maxAge;
        private readonly int minBandits;
        private readonly int maxBandits;
        private readonly int minGoldGiven;
        private readonly int maxGoldGiven;
        private readonly int minInfluenceGain;
        private readonly int maxInfluenceGain;
        private readonly int minGoldLooted;
        private readonly int maxGoldLooted;
        private readonly int minRogueryLevel;

        public BirthdayParty() : base(ModSettings.RandomEvents.BirthdayPartyData)
        {
            
            var ConfigFile = new IniFile(ParseIniFile.GetTheConfigFile());
            
            eventDisabled = ConfigFile.ReadBoolean("BirthdayParty", "EventDisabled");
            minAttending = ConfigFile.ReadInteger("BirthdayParty", "MinAttending");
            maxAttending = ConfigFile.ReadInteger("BirthdayParty", "MaxAttending");
            minYourMenAttending = ConfigFile.ReadInteger("BirthdayParty", "MinYourMenAttending");
            maxYourMenAttending = ConfigFile.ReadInteger("BirthdayParty", "MaxYourMenAttending");
            minAge = ConfigFile.ReadInteger("BirthdayParty", "MinAge");
            maxAge = ConfigFile.ReadInteger("BirthdayParty", "MaxAge");
            minBandits = ConfigFile.ReadInteger("BirthdayParty", "MinBandits");
            maxBandits = ConfigFile.ReadInteger("BirthdayParty", "MaxBandits");
            minGoldGiven = ConfigFile.ReadInteger("BirthdayParty", "MinGoldGiven");
            maxGoldGiven = ConfigFile.ReadInteger("BirthdayParty", "MaxGoldGiven");
            minInfluenceGain = ConfigFile.ReadInteger("BirthdayParty", "MinInfluenceGain");
            maxInfluenceGain = ConfigFile.ReadInteger("BirthdayParty", "MaxInfluenceGain");
            minGoldLooted = ConfigFile.ReadInteger("BirthdayParty", "MinGoldLooted");
            maxGoldLooted = ConfigFile.ReadInteger("BirthdayParty", "MaxGoldLooted");
            minRogueryLevel = ConfigFile.ReadInteger("BirthdayParty", "MinRogueryLevel");

            //Overrides the min age.
            minAge = Math.Max(minAge, 16);
        }

        public override void CancelEvent()
        {
        }

        private bool HasValidEventData()
        {
            if (eventDisabled) return false;

            return minAttending != 0 || maxAttending != 0 || minYourMenAttending != 0 || maxYourMenAttending != 0 ||
                   minAge != 0 || maxAge != 0 || minBandits != 0 || maxBandits != 0 || minGoldGiven != 0 ||
                   maxGoldGiven != 0 || minInfluenceGain != 0 || maxInfluenceGain != 0 || minGoldLooted != 0 ||
                   maxGoldLooted != 0 || minRogueryLevel != 0;
        }

        public override bool CanExecuteEvent()
        {
            return HasValidEventData() && MobileParty.MainParty.MemberRoster.TotalRegulars >= maxYourMenAttending &&
                   Hero.MainHero.Gold > maxGoldGiven;
        }

        public override void StartEvent()
        {
            if (GeneralSettings.DebugMode.IsActive())
            {
                var debugMsg = new TextObject(
                        "Starting “{randomEvent}” with the current values:\n\n" +
                        "Min Attending : {minAttending}\n" +
                        "Max Attending : {maxAttending}\n" +
                        "Min Your Men Attending : {minYourMenAttending}\n" +
                        "Max Your Men Attending : {maxYourMenAttending}\n" +
                        "Min Age : {minAge}\n" +
                        "Max Age : {maxAge}\n" +
                        "Min Bandits : {minBandits}\n" +
                        "Max Bandits : {maxBandits}\n" +
                        "Min Gold Given : {minGoldGiven}\n" +
                        "Max Gold Given : {maxGoldGiven}\n" +
                        "Min Influence Gain : {minInfluenceGain}\n" +
                        "Max Influence Gain : {maxInfluenceGain}\n" +
                        "Min Gold Looted : {minGoldLooted}\n" +
                        "Max Gold Looted : {maxGoldLooted}\n" +
                        "Min Roguery Level : {minRogueryLevel}\n\n" +
                        "To disable these messages make sure you set the DebugMode = false in the ini settings\n\nThe ini file is located here : \n{path}"
                    )
                    .SetTextVariable("randomEvent", randomEventData.eventType)
                    .SetTextVariable("minAttending", minAttending)
                    .SetTextVariable("maxAttending", maxAttending)
                    .SetTextVariable("minYourMenAttending", minYourMenAttending)
                    .SetTextVariable("maxYourMenAttending", maxYourMenAttending)
                    .SetTextVariable("minAge", minAge)
                    .SetTextVariable("maxAge", maxAge)
                    .SetTextVariable("minBandits", minBandits)
                    .SetTextVariable("maxBandits", maxBandits)
                    .SetTextVariable("minGoldGiven", minGoldGiven)
                    .SetTextVariable("maxGoldGiven", maxGoldGiven)
                    .SetTextVariable("minInfluenceGain", minInfluenceGain)
                    .SetTextVariable("maxInfluenceGain", maxInfluenceGain)
                    .SetTextVariable("minGoldLooted", minGoldLooted)
                    .SetTextVariable("maxGoldLooted", maxGoldLooted)
                    .SetTextVariable("minRogueryLevel", minRogueryLevel)
                    .SetTextVariable("path", ParseIniFile.GetTheConfigFile())
                    .ToString();
                
                InformationManager.ShowInquiry(new InquiryData("Debug Info", debugMsg, true, false, "Start Event", null, null, null), true);
            }

            var heroName = Hero.MainHero.FirstName;

            var birthdayAge = MBRandom.RandomInt(minAge, maxAge);
            var yourMenAttending = MBRandom.RandomInt(minYourMenAttending, maxYourMenAttending);
            var peopleAttending = MBRandom.RandomInt(minAttending, maxAttending);
            var bandits = MBRandom.RandomInt(minBandits, maxBandits);
            var goldGiven = MBRandom.RandomInt(minGoldGiven, maxGoldGiven);
            var influenceGain = MBRandom.RandomInt(minInfluenceGain, maxInfluenceGain);
            var goldBase = MBRandom.RandomInt(minGoldLooted, maxGoldLooted);
            
            var goldLooted = goldBase * peopleAttending;
            
            var rogueryLevel = Hero.MainHero.GetSkillValue(DefaultSkills.Roguery);

            var closestSettlement = ClosestSettlements.GetClosestAny(MobileParty.MainParty).ToString();
            
            var canRaidWedding = false;

            var rogueryAppendedText = "";

            if (GeneralSettings.SkillChecks.IsDisabled())
            {
                
                canRaidWedding = true;
                rogueryAppendedText = new TextObject("{=BirthdayParty_Skill_Check_Disable_Appended_Text}**Skill checks are disabled**").ToString();

            }
            else
            {
                if (rogueryLevel >= minRogueryLevel)
                {
                    canRaidWedding = true;
                    
                    rogueryAppendedText = new TextObject("{=BirthdayParty_Roguery_Appended_Text}[Roguery - lvl {minRogueryLevel}]")
                        .SetTextVariable("minRogueryLevel", minRogueryLevel)
                        .ToString();
                }
            }


            var eventTitle = new TextObject("{=BirthdayParty_Title}The Birthday Party!").ToString();

            var eventDescription = new TextObject(
                "{=BirthdayParty_Event_Desc}As you and your party are traveling in the vicinity of {closestSettlement}, you come across {peopleAttending} " +
                "people in what seems to be a birthday party for a young girl. A couple of the guests invite you to join them in celebrating the girl's {birthdayAge}th " +
                "birthday! What should you do?")
                .SetTextVariable("closestSettlement", closestSettlement)
                .SetTextVariable("peopleAttending", peopleAttending)
                .SetTextVariable("birthdayAge", birthdayAge).ToString();

            var eventOption1 = new TextObject("{=BirthdayParty_Event_Option_1}Join them in celebration!").ToString();
            var eventOption1Hover = new TextObject("{=BirthdayParty_Event_Option_1_Hover}Not everyday you turn {birthdayAge} years old!").SetTextVariable("birthdayAge", birthdayAge).ToString();
            
            var eventOption2 = new TextObject("{=BirthdayParty_Event_Option_2}Give the girl some gold").ToString();
            var eventOption2Hover = new TextObject("{=BirthdayParty_Event_Option_2_Hover}You don't have time to stay but you can still be nice, right?").ToString();

            var eventOption3 = new TextObject("{=BirthdayParty_Event_Option_3}Leave").ToString();
            var eventOption3Hover = new TextObject("{=BirthdayParty_Event_Option_3_Hover}Don't have time").ToString();
            
            var eventOption4 = new TextObject("{=BirthdayParty_Event_Option_4}[Roguery] Raid the party").ToString();
            var eventOption4Hover = new TextObject("{=BirthdayParty_Event_Option_4_Hover}Have some fun.\n{rogueryAppendedText}").SetTextVariable("rogueryAppendedText", rogueryAppendedText).ToString();

            var eventButtonText = new TextObject("{=BirthdayParty_Event_Button_Text}Okay").ToString();
            
            var inquiryElements = new List<InquiryElement>();
            
            inquiryElements.Add(new InquiryElement("a", eventOption1, null, true, eventOption1Hover));
            inquiryElements.Add(new InquiryElement("b", eventOption2, null, true, eventOption2Hover));
            inquiryElements.Add(new InquiryElement("c", eventOption3, null, true, eventOption3Hover));
            
            if (canRaidWedding)
            {
                inquiryElements.Add(new InquiryElement("d", eventOption4, null, true, eventOption4Hover));
            }
            

            var eventOptionAText = new TextObject(
                "{=BirthdayParty_Event_Choice_1}You and {yourMenAttending} of your men decide to stay for the party " +
                "while the rest makes their way to {closestSettlement}. You approach the girl and give her {goldGiven} " +
                "gold as a gift. She gives you a hug and says thank you. You get yourself some beer and sit down to enjoy " +
                "the moment.\n\nSome time later, {bandits} bandits decide to crash the party. They go around from person" +
                " to person and takes everything of value. You order your men to stand down as you don't want to start" +
                " a fight with innocent people caught in the middle. After they have taken everything of value they" +
                " also try to take the young girl with them. This you will not stand for so you signal your men to" +
                " strike. You and your men make quick work in incapacitating the bandits. One of your men rides" +
                " to {closestSettlement} to fetch someone to throw these scum in the dungeon. The rest of the night" +
                " you are celebrated as a hero! You even get to dance with the birthday girl!")
                .SetTextVariable("yourMenAttending", yourMenAttending)
                .SetTextVariable("closestSettlement", closestSettlement)
                .SetTextVariable("goldGiven", goldGiven)
                .SetTextVariable("bandits", bandits)
                .ToString();
            
            var eventOptionBText = new TextObject(
                    "{=BirthdayParty_Event_Choice_2}You really don't have time to stay but you don't want to be rude" +
                    " either. You manage to scrape together {goldGiven} gold and give it to the girl as a gift. She " +
                    "seems grateful.\nYou say your goodbyes to the partygoers and leave in the direction of {closestSettlement}.")
                .SetTextVariable("closestSettlement", closestSettlement)
                .SetTextVariable("goldGiven", goldGiven)
                .ToString();
            
            var eventOptionCText = new TextObject(
                    "{=BirthdayParty_Event_Choice_3}You don't have time for this so you leave for {closestSettlement} but not before casting one last look at the party.")
                .SetTextVariable("closestSettlement", closestSettlement)
                .ToString();
            
            var eventOptionDText = new TextObject(
                    "{=BirthdayParty_Event_Choice_4}You decide that this is the perfect moment to let loose your" +
                    " somewhat evil side. You order your men you surround the party and you every guest to hand over" +
                    " everything of value. They refuse at the beginning but you have one of your men kill a random " +
                    "person. They all fall in line after that, handing over everything. Once you have gathered your " +
                    "loot, you and your men leave but not before tossing over 1 gold coin to the birthday girl who is " +
                    "clearly very upset. You are left with {goldLooted} gold.")
                .SetTextVariable("goldLooted", goldLooted)
                .ToString();
            
            var eventMsg1 =new TextObject(
                    "{=BirthdayParty_Event_Msg_1}{heroName} gave away {goldGiven} to the girl and gained {influenceGain} influence for slaying the bandits.")
                .SetTextVariable("heroName", heroName)
                .SetTextVariable("goldGiven", goldGiven)
                .SetTextVariable("influenceGain", influenceGain)
                .ToString();
            
            var eventMsg2 =new TextObject(
                    "{=BirthdayParty_Event_Msg_2}{heroName} gave away {goldGiven} to the girl.")
                .SetTextVariable("heroName", heroName)
                .SetTextVariable("goldGiven", goldGiven)
                .ToString();
            
            var eventMsg3 =new TextObject(
                    "{=BirthdayParty_Event_Msg_3}{heroName} looted {goldLooted} from the birthday party.")
                .SetTextVariable("heroName", heroName)
                .SetTextVariable("goldLooted", goldLooted)
                .ToString();

            

            var msid = new MultiSelectionInquiryData(eventTitle, eventDescription, inquiryElements, false, 1, 1, eventButtonText, null,
                elements =>
                {
                    switch ((string)elements[0].Identifier)
                    {
                        case "a":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionAText, true, false, eventButtonText, null, null, null), true);
                            Hero.MainHero.ChangeHeroGold(-goldGiven);
                            Hero.MainHero.AddInfluenceWithKingdom(influenceGain);
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg1, RandomEventsSubmodule.Msg_Color_POS_Outcome));
                            break;
                        
                        case "b":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionBText, true, false, eventButtonText, null, null, null), true);
                            Hero.MainHero.ChangeHeroGold(-goldGiven);
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg2, RandomEventsSubmodule.Msg_Color_POS_Outcome));
                            break;
                        
                        case "c":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle,eventOptionCText, true, false, eventButtonText, null, null, null), true);
                            break;
                        
                        case "d":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle,eventOptionDText, true, false, eventButtonText, null, null, null), true);
                            Hero.MainHero.ChangeHeroGold(+goldGiven);
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg3, RandomEventsSubmodule.Msg_Color_EVIL_Outcome));
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
    }


    public class BirthdayPartyData : RandomEventData
    {
        public BirthdayPartyData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
        {
        }

        public override BaseEvent GetBaseEvent()
        {
            return new BirthdayParty();
        }
    }
}