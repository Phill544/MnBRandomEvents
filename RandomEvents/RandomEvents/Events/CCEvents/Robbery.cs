using System;
using System.Linq;
using System.Windows;
using Bannerlord.RandomEvents.Helpers;
using Bannerlord.RandomEvents.Settings;
using Ini.Net;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace Bannerlord.RandomEvents.Events.CCEvents
{
    public sealed class Robbery : BaseEvent
    {
        private readonly bool eventDisabled;
        private readonly int minGoldLost;
        private readonly int maxGoldLost;
        private readonly int minRenownLost;
        private readonly int maxRenownLost;
        private readonly int minRoguerySkill;
        private readonly int minCharmSkill;
        private readonly int minOneHandedSkill;
        private readonly int minTwoHandedSkill;
            

        public Robbery() : base(ModSettings.RandomEvents.RobberyData)
        {
            var ConfigFile = new IniFile(ParseIniFile.GetTheConfigFile());
            
            eventDisabled = ConfigFile.ReadBoolean("Robbery", "EventDisabled");
            minGoldLost = ConfigFile.ReadInteger("Robbery", "minGoldLost");
            maxGoldLost = ConfigFile.ReadInteger("Robbery", "MaxGoldLost");
            minRenownLost = ConfigFile.ReadInteger("Robbery", "MinRenownLost");
            maxRenownLost = ConfigFile.ReadInteger("Robbery", "MaxRenownLost");
            minRoguerySkill = ConfigFile.ReadInteger("Robbery", "MinRoguerySkill");
            minCharmSkill = ConfigFile.ReadInteger("Robbery", "MinCharmSkill");
            minOneHandedSkill = ConfigFile.ReadInteger("Robbery", "MinOneHandedSkill");
            minTwoHandedSkill = ConfigFile.ReadInteger("Robbery", "MinTwoHandedSkill");
        }

        public override void CancelEvent()
        {
        }

        private bool HasValidEventData()
        {
            if (eventDisabled == false)
            {
                if (minGoldLost != 0 || maxGoldLost != 0 || minRenownLost != 0 || maxRenownLost != 0 || minRoguerySkill != 0 || minCharmSkill != 0 || minOneHandedSkill != 0 || minTwoHandedSkill != 0)
                {
                    return true;
                }
            }
            return false;
        }

        public override bool CanExecuteEvent()
        {
            if (Settlement.CurrentSettlement == null)
                return false;
            
            var notables = Settlement.CurrentSettlement.Notables.ToList();
            var gangLeaders = notables.Where(character => character.IsGangLeader).ToList();

            return HasValidEventData() && gangLeaders.Count != 0 && CurrentTimeOfDay.IsNight;
        }

        public override void StartEvent()
        {
            if (GeneralSettings.DebugMode.IsActive())
            {
                var debugMsg = new TextObject(
                        "Starting “{randomEvent}” with the current values:\n\n" +
                        "Min Gold Lost : {minGoldLost}\n" +
                        "Max Gold Lst : {maxGoldLost}\n" +
                        "Min Renown Lost : {minRenownLost}\n" +
                        "Max Renown Lost : {maxRenownLost}\n" +
                        "Min Roguery Skill : {minRoguerySkill}\n" +
                        "Min Charm Skill : {minCharmSkill}\n" +
                        "Min One Handed Skill : {minOneHandedSkill}\n" +
                        "Min Two Handed Skill : {minTwoHandedSkill}\n\n" +
                        "To disable these messages make sure you set the DebugMode = false in the ini settings\n\nThe ini file is located here : \n{path}"
                    )
                    .SetTextVariable("randomEvent", randomEventData.eventType)
                    .SetTextVariable("minGoldLost", minGoldLost)
                    .SetTextVariable("maxGoldLost", maxGoldLost)
                    .SetTextVariable("minRenownLost", minRenownLost)
                    .SetTextVariable("maxRenownLost", maxRenownLost)
                    .SetTextVariable("minRoguerySkill", minRoguerySkill)
                    .SetTextVariable("minCharmSkill", minCharmSkill)
                    .SetTextVariable("minOneHandedSkill", minOneHandedSkill)
                    .SetTextVariable("minTwoHandedSkill", minTwoHandedSkill)
                    .SetTextVariable("path", ParseIniFile.GetTheConfigFile())
                    .ToString();
                
                InformationManager.ShowInquiry(new InquiryData("Debug Info", debugMsg, true, false, "Start Event", null, null, null), true);
            }

            var currentSettlement = Settlement.CurrentSettlement.Name.ToString();

            var thugs = MBRandom.RandomInt(5, 30);
            
            var goldLost = MBRandom.RandomInt(minGoldLost, maxGoldLost);

            var renownLost = MBRandom.RandomInt(minRenownLost, maxRenownLost);

            var clanRenown = Clan.PlayerClan.Renown;

            var roguerySkill = Hero.MainHero.GetSkillValue(DefaultSkills.Roguery);
            var charmSkill = Hero.MainHero.GetSkillValue(DefaultSkills.Charm);
            var oneHandedSkill = Hero.MainHero.GetSkillValue(DefaultSkills.OneHanded);
            var twoHandedSkill = Hero.MainHero.GetSkillValue(DefaultSkills.TwoHanded);

            var intimidatedThugs = false;
            var charmedThugs = false;
            var convincedThugs = false;
            var gangLeaderGoodRelation = false;

            if (GeneralSettings.SkillChecks.IsDisabled())
            {
                convincedThugs = true;
                charmedThugs = true;
                intimidatedThugs = true;
            }
            else
            {
                if (roguerySkill >= minRoguerySkill)
                {
                    convincedThugs = true;
                }
                else if (charmSkill >= minCharmSkill)
                {
                    charmedThugs = true;
                }
                else if (oneHandedSkill >= minOneHandedSkill && twoHandedSkill >= minTwoHandedSkill)
                {
                    intimidatedThugs = true;
                }
            }

            var notables = Settlement.CurrentSettlement.Notables.ToList();
            
            var gangLeaders = notables.Where(character => character.IsGangLeader).ToList();

            var random = new Random();
            var index = random.Next(gangLeaders.Count);

            var gangLeader = gangLeaders[index];

            var gangLeaderIsFemale = gangLeader.IsFemale;

            var gangLeaderGender = gangLeaderIsFemale ? "female" : "male";

            var gangLeaderGenderAdjective = GenderAssignment.GetTheGenderAssignment(gangLeaderGender, false, "adjective");
            var gangLeaderGenderSubjective = GenderAssignment.GetTheGenderAssignment(gangLeaderGender, false, "subjective");

            var gangLeaderName = gangLeader.Name.ToString();

            var gangLeaderRelation = gangLeader.GetBaseHeroRelation(Hero.MainHero);

            if (gangLeaderRelation >= 50)
            {
                gangLeaderGoodRelation = true;
            }

            var eventTitle = new TextObject("{=Robbery_Title}Robbery").ToString();

            var eventText_Bad_Outcome =new TextObject(
                    "{=Robbery_Event_Text_Bad_Outcome}One evening, while leaving the tavern in {currentSettlement} late " +
                    "at night you decide to take a shortcut back through an alley. You quickly realize that this was a bad " +
                    "idea as you are surrounded by {thugs} thugs who start threatening you. Their leader, {gangLeader}, " +
                    "tells you to hand over any valuables you have on you. Knowing you are outnumbered, you comply and " +
                    "hand over {goldLost} gold to {gangLeader} and {Adjective} thugs.\n\n{gangLeader} tells {Adjective} thugs to teach you " +
                    "a lesson that will keep you out of their alley next time. The next few minutes are a blur as you are " +
                    "punched, kicked, thrown, and intimidated.\nYou black out\n\nYou awake some time later only to realize " +
                    "the thugs have dumped you in a pigsty. You stumble to your feet and make haste back to where you are staying.")
                .SetTextVariable("currentSettlement", currentSettlement)
                .SetTextVariable("thugs", thugs)
                .SetTextVariable("Adjective", gangLeaderGenderAdjective)
                .SetTextVariable("gangLeader", gangLeaderName)
                .SetTextVariable("goldLost", goldLost)
                .ToString();
            
            var eventText_Convinced_Outcome =new TextObject(
                    "{=Robbery_Event_Text_Convinced_Outcome}One evening, while leaving the tavern in {currentSettlement} " +
                    "late at night you decide to take a shortcut back through an alley. You quickly realize that this was " +
                    "a bad idea as you are surrounded by {thugs} thugs who start threatening you. Their leader, {gangLeader}, " +
                    "tells you to hand over any valuables you have on you. You ask {gangLeader} if they are crazy and " +
                    "haven't recognized you. {gangLeader} and {Adjective} thugs look at you one more time before realization sets " +
                    "in. {gangLeader} apologizes and withdraws {Adjective} thugs. They let you pass through their alley unhindered. " +
                    "The next person however will be in for quite a surprise.")
                .SetTextVariable("currentSettlement", currentSettlement)
                .SetTextVariable("gangLeader", gangLeaderName)
                .SetTextVariable("Adjective", gangLeaderGenderAdjective)
                .ToString();
            
            var eventText_Charmed_Outcome =new TextObject(
                    "{=Robbery_Event_Text_Charmed_Outcome}One evening, while leaving the tavern in {currentSettlement} " +
                    "late at night you decide to take a shortcut back through an alley. You quickly realize that this " +
                    "was a bad idea as you are surrounded by {thugs} thugs who start threatening you. Their leader, {gangLeader}, " +
                    "tells you to hand over any valuables you have on you. Knowing you are outnumbered, you do what you " +
                    "do best, talk. You strike up a somewhat friendly conversation with {gangLeader} and {Subjective} actually " +
                    "seem interested in the conversation. You tell {Adjective} some gossip you've heard about a rich guest " +
                    "visiting this city and so on. {gangLeader} takes one long look at you before ordering {Adjective} thugs to " +
                    "let you go. You hand over 100 gold to {gangLeader} and bids farewell.")
                .SetTextVariable("currentSettlement", currentSettlement)
                .SetTextVariable("gangLeader", gangLeaderName)
                .SetTextVariable("Adjective", gangLeaderGenderAdjective)
                .SetTextVariable("Subjective", gangLeaderGenderSubjective)
                .ToString();
            
            var eventText_Intimidated_Outcome =new TextObject(
                    "{=Robbery_Event_Text_Intimidated_Outcome}One evening, while leaving the tavern in {currentSettlement} " +
                    "late at night you decide to take a shortcut back through an alley. You quickly realize that this " +
                    "was a bad idea as you are surrounded by {thugs} thugs who start threatening you. Their leader, {gangLeader}, " +
                    "tells you to hand over any valuables you have on you. You immediately burst out in laughter, much " +
                    "to the surprise of {gangLeader} and {Adjective} thugs. You go on to tell them the deeds you have done on the " +
                    "battlefields of Calradia, about all the looters and bandits who have died by your hand. You can see " +
                    "the thugs are getting nervous. You ask {gangLeader} if {Subjective} really think {thugs} thugs are gonna pose " +
                    "any threat to you. The thugs put away their weapons and {gangLeader} tells you that you may leave. " +
                    "You leave the alley but not before suggesting to the thugs that justice will soon catch up to them.")
                .SetTextVariable("currentSettlement", currentSettlement)
                .SetTextVariable("thugs", thugs)
                .SetTextVariable("gangLeader", gangLeaderName)
                .SetTextVariable("Adjective", gangLeaderGenderAdjective)
                .SetTextVariable("Subjective", gangLeaderGenderSubjective)
                .ToString();
            
            var eventText_Good_Relation_Outcome =new TextObject(
                    "{=Robbery_Event_Text_Good_Relation_Outcome}One evening, while leaving the tavern in {currentSettlement} " +
                    "late at night you decide to take a shortcut back through an alley. You quickly realize that this " +
                    "was a bad idea as you are surrounded by {thugs} thugs who start threatening you. Their leader, {gangLeader}, " +
                    "tells you to hand over any valuables you have on you. You ask {gangLeader} if {Subjective} don't recognize you. " +
                    "{gangLeader} then approaches you and gives you a big hug while laughing. You and {gangLeader} go way back, " +
                    "back to the time before the turmoil the world is in. The two of you stand there talking for a few minutes " +
                    "and catch up on the old and new. Eventually you tell {gangLeader} that it's time for you to get some rest. " +
                    "You say your goodbyes and embrace one last time.")
                .SetTextVariable("currentSettlement", currentSettlement)
                .SetTextVariable("thugs", thugs)
                .SetTextVariable("gangLeader", gangLeaderName)
                .SetTextVariable("Subjective", gangLeaderGenderSubjective)
                .ToString();
            
            var eventMsg1 =new TextObject(
                    "{=Robbery_Event_Msg_1}Clan {playerClan} lost {renownLost} renown after word spread that {heroName} was robbed by {gangLeader} and {Adjective} thugs!")
                .SetTextVariable("playerClan", Clan.PlayerClan.Name.ToString())
                .SetTextVariable("renownLost", renownLost)
                .SetTextVariable("heroName", Hero.MainHero.FirstName.ToString())
                .SetTextVariable("gangLeader", gangLeaderName)
                .SetTextVariable("Adjective", gangLeaderGenderAdjective)
                .ToString();
            
            var eventMsg2=new TextObject(
                    "{=Robbery_Event_Msg_2}{heroName} managed to convince {gangLeader} and {Adjective} thugs that they were not worth it.")
                .SetTextVariable("heroName", Hero.MainHero.FirstName.ToString())
                .SetTextVariable("gangLeader", gangLeaderName)
                .SetTextVariable("Adjective", gangLeaderGenderAdjective)
                .ToString();
            
            var eventMsg3=new TextObject(
                    "{=Robbery_Event_Msg_3}{heroName} managed to talk their way out of a robbery by {gangLeader}.")
                .SetTextVariable("heroName", Hero.MainHero.FirstName.ToString())
                .SetTextVariable("gangLeader", gangLeaderName)
                .ToString();
            
            var eventMsg4=new TextObject(
                    "{=Robbery_Event_Msg_4}{heroName} managed to intimidate {gangLeader} and {Adjective} thugs.")
                .SetTextVariable("heroName", Hero.MainHero.FirstName.ToString())
                .SetTextVariable("gangLeader", gangLeaderName)
                .SetTextVariable("Adjective", gangLeaderGenderAdjective)
                .ToString();


            var eventButtonText = new TextObject("{=Robbery_Event_Button_Text}Continue").ToString();

            if (charmedThugs == false && convincedThugs == false && intimidatedThugs == false && gangLeaderGoodRelation == false)
            {
                InformationManager.ShowInquiry(new InquiryData(eventTitle, eventText_Bad_Outcome, true, false, eventButtonText, null, null, null), true);
                
                Clan.PlayerClan.Renown = clanRenown - renownLost;
                Hero.MainHero.ChangeHeroGold(-goldLost);
                InformationManager.DisplayMessage(new InformationMessage(eventMsg1, RandomEventsSubmodule.Msg_Color_NEG_Outcome));
            }
            else if (convincedThugs)
            {
                InformationManager.ShowInquiry(new InquiryData(eventTitle, eventText_Convinced_Outcome, true, false, eventButtonText, null, null, null), true);
                InformationManager.DisplayMessage(new InformationMessage(eventMsg2, RandomEventsSubmodule.Msg_Color_MED_Outcome));
            }
            else if (charmedThugs)
            {
                InformationManager.ShowInquiry(new InquiryData(eventTitle, eventText_Charmed_Outcome, true, false, eventButtonText, null, null, null), true);
                Hero.MainHero.ChangeHeroGold(-100);
                InformationManager.DisplayMessage(new InformationMessage(eventMsg3, RandomEventsSubmodule.Msg_Color_MED_Outcome));
            }
            else if (intimidatedThugs)
            {
                InformationManager.ShowInquiry(new InquiryData(eventTitle, eventText_Intimidated_Outcome, true, false, eventButtonText, null, null, null), true);
                InformationManager.DisplayMessage(new InformationMessage(eventMsg4, RandomEventsSubmodule.Msg_Color_MED_Outcome));
            }
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            else if (gangLeaderGoodRelation)
            {
                InformationManager.ShowInquiry(new InquiryData(eventTitle, eventText_Good_Relation_Outcome, true, false, eventButtonText, null, null, null), true);
            }

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


    public class RobberyData : RandomEventData
    {
        public RobberyData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
        {
        }

        public override BaseEvent GetBaseEvent()
        {
            return new Robbery();
        }
    }
}