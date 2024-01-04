using System;
using System.Collections.Generic;
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
                if (minGoldLost != 0 || maxGoldLost != 0 ||  minRoguerySkill != 0 || minCharmSkill != 0 || minOneHandedSkill != 0 || minTwoHandedSkill != 0)
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
            var currentSettlement = Settlement.CurrentSettlement.Name.ToString();

            var heroName = Hero.MainHero.FirstName.ToString();

            var thugs = MBRandom.RandomInt(5, 30);
            
            var goldLost = MBRandom.RandomInt(minGoldLost, maxGoldLost);

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

            var eventTitle = new TextObject(EventTextHandler.GetRandomEventTitle()).ToString();

            var eventText_Bad_Outcome =new TextObject(EventTextHandler.GetRandomEventOutcomeBad())
                .SetTextVariable("currentSettlement", currentSettlement)
                .SetTextVariable("thugs", thugs)
                .SetTextVariable("Adjective", gangLeaderGenderAdjective)
                .SetTextVariable("gangLeader", gangLeaderName)
                .SetTextVariable("goldLost", goldLost)
                .ToString();
            
            var eventText_Convinced_Outcome =new TextObject(EventTextHandler.GetRandomEventOutcomeConvinced())
                .SetTextVariable("currentSettlement", currentSettlement)
                .SetTextVariable("gangLeader", gangLeaderName)
                .SetTextVariable("Adjective", gangLeaderGenderAdjective)
                .ToString();
            
            var eventText_Charmed_Outcome =new TextObject(EventTextHandler.GetRandomEventOutcomeCharmed())
                .SetTextVariable("currentSettlement", currentSettlement)
                .SetTextVariable("gangLeader", gangLeaderName)
                .SetTextVariable("Adjective", gangLeaderGenderAdjective)
                .SetTextVariable("Subjective", gangLeaderGenderSubjective)
                .ToString();
            
            var eventText_Intimidated_Outcome =new TextObject(EventTextHandler.GetRandomEventOutcomeIntimidated())
                .SetTextVariable("currentSettlement", currentSettlement)
                .SetTextVariable("thugs", thugs)
                .SetTextVariable("gangLeader", gangLeaderName)
                .SetTextVariable("Adjective", gangLeaderGenderAdjective)
                .SetTextVariable("Subjective", gangLeaderGenderSubjective)
                .ToString();
            
            var eventText_Good_Relation_Outcome =new TextObject(EventTextHandler.GetRandomEventOutcomeGoodRelations())
                .SetTextVariable("currentSettlement", currentSettlement)
                .SetTextVariable("thugs", thugs)
                .SetTextVariable("gangLeader", gangLeaderName)
                .SetTextVariable("Subjective", gangLeaderGenderSubjective)
                .ToString();
            
            var eventMsg1 =new TextObject(EventTextHandler.GetRandomEventMessageBadOutcome())
                .SetTextVariable("heroName", heroName)
                .SetTextVariable("gangLeader", gangLeaderName)
                .SetTextVariable("goldLost", goldLost)
                .SetTextVariable("currentSettlement", currentSettlement)
                .ToString();
            
            var eventMsg2=new TextObject(EventTextHandler.GetRandomEventMessageConvincedOutcome())
                .SetTextVariable("heroName", heroName)
                .SetTextVariable("gangLeader", gangLeaderName)
                .SetTextVariable("currentSettlement", currentSettlement)
                .ToString();
            
            var eventMsg3=new TextObject(EventTextHandler.GetRandomEventMessageCharmedOutcome())
                .SetTextVariable("heroName", heroName)
                .SetTextVariable("gangLeader", gangLeaderName)
                .SetTextVariable("currentSettlement", currentSettlement)
                .ToString();
            
            var eventMsg4=new TextObject(EventTextHandler.GetRandomEventMessageIntimidatedOutcome())
                .SetTextVariable("heroName", heroName)
                .SetTextVariable("gangLeader", gangLeaderName)
                .SetTextVariable("currentSettlement", currentSettlement)
                .ToString();
            
            var eventMsg5=new TextObject(EventTextHandler.GetRandomEventMessageGoodRelationOutcome())
                .SetTextVariable("heroName", heroName)
                .SetTextVariable("gangLeader", gangLeaderName)
                .SetTextVariable("currentSettlement", currentSettlement)
                .ToString();


            var eventButtonText = new TextObject("{=Robbery_Event_Button_Text}Continue").ToString();

            if (charmedThugs == false && convincedThugs == false && intimidatedThugs == false && gangLeaderGoodRelation == false)
            {
                InformationManager.ShowInquiry(new InquiryData(eventTitle, eventText_Bad_Outcome, true, false, eventButtonText, null, null, null), true);
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
                InformationManager.DisplayMessage(new InformationMessage(eventMsg5, RandomEventsSubmodule.Msg_Color_POS_Outcome));
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
        
        private static class EventTextHandler
        {
            private static readonly Random random = new Random();
            
            private static readonly List<string> eventTitles = new List<string>
            {
                "{=Robbery_Title_A}City Heist",
                "{=Robbery_Title_B}Urban Theft",
                "{=Robbery_Title_C}Streets of Deceit",
                "{=Robbery_Title_D}The City's Underbelly",
                "{=Robbery_Title_E}Metropolitan Mugging",
                "{=Robbery_Title_F}Backstreet Banditry",
                "{=Robbery_Title_G}Thieves in the Shadows",
                "{=Robbery_Title_H}Urban Pickpocketing"

            };
            
            private static readonly List<string> eventOutcomeBad = new List<string>
            {
                //Event Outcome Bad A
                "{=Robbery_Event_Text_Bad_Outcome_A}On a late-night departure from the tavern in {currentSettlement}, you " +
                "opt for an alley as a shortcut. Suddenly, you find yourself encircled by {thugs} menacing figures. The " +
                "group's leader, {gangLeader}, demands your valuables. Outnumbered, you relinquish {goldLost} gold to " +
                "{gangLeader} and {Adjective} accomplices. {gangLeader} instructs {Adjective} followers to give you a " +
                "harsh reminder to avoid their territory. The ensuing moments are chaotic as you endure punches, kicks, " +
                "and threats. Consciousness fades. When you come to, you're lying in a pigsty. Gathering your strength, " +
                "you hurriedly return to your lodgings.",
                
                //Event Outcome Bad B
                "{=Robbery_Event_Text_Bad_Outcome_B}Strolling back from the tavern in {currentSettlement} via an alleyway " +
                "proved perilous when {thugs} thugs, led by {gangLeader}, corner you. With no way to resist, you hand over " +
                "{goldLost} gold to {gangLeader} and {Adjective} gang. 'Remember this place,' {gangLeader} sneers, as {Adjective} " +
                "henchmen rough you up. After a flurry of blows and jeers, you lose consciousness. You later awaken in a " +
                "filthy pigsty, barely able to stand, and hasten back to your accommodation." ,
                
                //Event Outcome Bad C
                "{=Robbery_Event_Text_Bad_Outcome_C}Late at night in {currentSettlement}, an alley shortcut turns into a " +
                "nightmare. Surrounded by {thugs} of {gangLeader}'s gang, you're coerced into giving up {goldLost} gold. " +
                "{gangLeader} signals {Adjective} thugs to 'leave a mark,' leading to a brutal beating. The world spins and " +
                "goes dark. You eventually come to in a disgusting pigsty, bruised and battered. You quickly make your way " +
                "back to your temporary home.",
                
                //Event Outcome Bad D
                "{=Robbery_Event_Text_Bad_Outcome_D}Leaving the tavern in {currentSettlement}, you decide on an alley for a " +
                "quick return. But {thugs} thugs, led by {gangLeader}, soon encircle you. Faced with no choice, you give " +
                "{gangLeader} and {Adjective} cronies {goldLost} gold. 'Make sure they remember,' {gangLeader} orders, and " +
                "{Adjective} goons unleash a barrage of violence upon you. When you regain consciousness, you're in a pigsty, " +
                "dazed and hurting. You make a swift return to safety.",
                
                //Event Outcome Bad E
                "{=Robbery_Event_Text_Bad_Outcome_E}Your walk back from the tavern in {currentSettlement} takes a dire turn " +
                "in an alley, where {thugs} of {gangLeader}'s gang trap you. Reluctantly, you hand over {goldLost} gold to " +
                "{gangLeader} and {Adjective} gang members. 'Teach them not to trespass,' {gangLeader} commands. A brutal assault " +
                "follows, leaving you unconscious. Awakening in a pigsty, you muster the strength to stagger back to your lodgings."

            };
            
            private static readonly List<string> eventOutcomeConvinced = new List<string>
            {
                //Event Outcome Convinced A
                "{=Robbery_Event_Text_Convinced_Outcome_A}Exiting the tavern in {currentSettlement} late at night, you venture " +
                "through an alley as a shortcut. Suddenly, you're surrounded by {thugs} thugs. The gang's leader, {gangLeader}, " +
                "demands your valuables. In a bold move, you question {gangLeader}'s sanity for not recognizing who you are. A " +
                "moment of tension passes before {gangLeader} and {Adjective} gang recognize you, leading to an apology from " +
                "{gangLeader} and a quick dispersal of {Adjective} thugs. They let you continue, leaving the alley clear " +
                "for you. Someone else, though, might not be so lucky next time.",
                
                //Event Outcome Convinced B
                "{=Robbery_Event_Text_Convinced_Outcome_B}As you leave a tavern in {currentSettlement} via an alleyway, {thugs} " +
                "thugs encircle you, led by {gangLeader} who demands your treasures. You retort, asking if {gangLeader} has " +
                "lost their mind not to know you. After a second look, {gangLeader} and {Adjective} thugs recognize you, hastily " +
                "apologizing and retreating. You're allowed to pass, but you can't help but think of the next unsuspecting passerby.",
                
                //Event Outcome Convinced C
                "{=Robbery_Event_Text_Convinced_Outcome_C}On a late-night stroll from the tavern in {currentSettlement}, an " +
                "alley shortcut lands you amidst {thugs} thugs. Their leader, {gangLeader}, orders you to hand over your goods. " +
                "You challenge {gangLeader}, questioning their recognition of you. Recognition dawns on {gangLeader} and " +
                "{Adjective} thugs, leading to a swift apology and your unobstructed passage, leaving the alley's danger " +
                "to the next traveler.",
                
                //Event Outcome Convinced D
                "{=Robbery_Event_Text_Convinced_Outcome_D}Navigating an alley after leaving a tavern in {currentSettlement}, " +
                "you're ambushed by {thugs} thugs under {gangLeader}. Confronted for valuables, you question {gangLeader}'s " +
                "awareness of your identity. A moment of realization strikes {gangLeader} and {Adjective} thugs, prompting a " +
                "quick apology and your safe passage, though the alley remains a peril for others.",
                
                //Event Outcome Convinced E
                "{=Robbery_Event_Text_Convinced_Outcome_E}Taking an alleyway post-tavern visit in {currentSettlement}, you're " +
                "cornered by {thugs} thugs, with {gangLeader} demanding your valuables. You confront {gangLeader} about their " +
                "failure to recognize you. After a brief pause, recognition hits {gangLeader} and {Adjective} thugs, leading " +
                "to an apologetic retreat and your free passage, leaving the alley's next victim to fate."

            };
            
            private static readonly List<string> eventOutcomeCharmed = new List<string>
            {
                //Event Outcome Charmed A
                "{=Robbery_Event_Text_Charmed_Outcome_A}Exiting the tavern in {currentSettlement} late, you choose an alley " +
                "shortcut and are soon surrounded by {thugs} thugs. Confronted by their leader, {gangLeader}, you're asked " +
                "for your valuables. Outnumbered, you engage {gangLeader} in conversation, piquing {Subjective} interest with " +
                "tales of a wealthy visitor in town. After a thoughtful pause, {gangLeader} orders {Adjective} thugs to release " +
                "you. You part with 100 gold and bid farewell, relieved.",
                
                //Event Outcome Charmed B
                "{=Robbery_Event_Text_Charmed_Outcome_B}Leaving the tavern in {currentSettlement} at night, an alleyway " +
                "shortcut leads you into a trap set by {thugs} thugs. {gangLeader} demands your valuables, but you deftly " +
                "start a chat, intriguing {Subjective} with rumors of a wealthy visitor. Intrigued, {gangLeader} decides " +
                "to let you go after you hand over 100 gold, leaving you to continue your night.",
                
                //Event Outcome Charmed C
                "{=Robbery_Event_Text_Charmed_Outcome_C}After leaving a tavern in {currentSettlement} at night, your alley " +
                "shortcut results in an encounter with {thugs} thugs. {gangLeader} wants your valuables, but your quick wit " +
                "leads to a conversation about a rumored affluent guest. {gangLeader}, intrigued, releases you after taking " +
                "100 gold, a small price for your safety.",
                
                //Event Outcome Charmed D
                "{=Robbery_Event_Text_Charmed_Outcome_D}Your late-night shortcut through an alley in {currentSettlement} ends " +
                "with {thugs} thugs demanding valuables. You talk your way out, capturing {gangLeader}'s attention with gossip " +
                "of a rich visitor. Captivated, {gangLeader} frees you after receiving 100 gold, allowing you to depart unharmed.",
                
                //Event Outcome Charmed E
                "{=Robbery_Event_Text_Charmed_Outcome_E}In {currentSettlement} at night, an alley shortcut traps you with {thugs} " +
                "thugs. Faced with {gangLeader}'s demands, you distract {Subjective} with stories of a wealthy city visitor. " +
                "{gangLeader}, intrigued, lets you go for 100 gold, sparing you further trouble."
                
            };
            
            private static readonly List<string> eventOutcomeIntimidated = new List<string>
            {
                //Event Outcome Intimidated A
                "{=Robbery_Event_Text_Intimidated_Outcome_A}In {currentSettlement} at night, an alleyway shortcut leads you to " +
                "a confrontation with {thugs} thugs. The leader, {gangLeader}, demands your valuables. You respond with laughter, " +
                "recounting your formidable battlefield exploits and the bandits who've fallen by your hand. This visibly " +
                "unnerves the thugs. Challenging {gangLeader} about their threat, you see their resolve waver. {gangLeader} " +
                "permits your departure, as you warn them of impending justice.",
                
                //Event Outcome Intimidated B
                "{=Robbery_Event_Text_Intimidated_Outcome_B}After leaving a tavern in {currentSettlement}, you're ambushed in " +
                "an alley by {thugs} thugs. {gangLeader} asks for valuables, but you laugh, boasting of your martial prowess and " +
                "past victories over bandits. The impact of your tales is clear on the thugs' faces. Questioning {gangLeader}'s " +
                "confidence, they back down, allowing you to leave with a final warning of justice's reach.",
                
                //Event Outcome Intimidated C
                "{=Robbery_Event_Text_Intimidated_Outcome_C}A late-night shortcut in {currentSettlement} puts you face-to-face " +
                "with {thugs} thugs. {gangLeader}'s demand for loot is met with your derisive laughter and tales of battlefield " +
                "glory. The thugs' confidence falters upon hearing of your feats. You confront {gangLeader} about their " +
                "threat, leading them to stand down, as you leave a cautionary note about justice.",
                
                //Event Outcome Intimidated D
                "{=Robbery_Event_Text_Intimidated_Outcome_D}Surrounded by {thugs} thugs in an alley in {currentSettlement}, " +
                "you defy {gangLeader}'s demand for valuables with laughter, sharing your fearsome battlefield " +
                "accomplishments. Your words sow doubt among the thugs. Challenging their threat, they lower their " +
                "weapons, and {gangLeader} allows your exit, heedful of your warning about justice.",
                
                //Event Outcome Intimidated E
                "{=Robbery_Event_Text_Intimidated_Outcome_E}Navigating an alley in {currentSettlement}, {thugs} thugs, " +
                "led by {gangLeader}, corner you for valuables. You mock them with laughter, narrating your victories and " +
                "the fate of past foes. The thugs grow anxious. Your challenge to {gangLeader} causes them to retract, " +
                "letting you pass with a foreboding hint about justice's arrival."

            };
            
            private static readonly List<string> eventOutcomeGood= new List<string>
            {
                //Event Outcome Good Relation A
                "{=Robbery_Event_Text_Good_Relation_Outcome_A}Exiting the tavern in {currentSettlement} at night, your alley " +
                "shortcut leads to an encounter with {thugs} thugs. Their leader, {gangLeader}, demands your valuables, but you " +
                "question if {Subjective} recognizes you. Recognizing an old friend, {gangLeader} greets you with laughter and " +
                "a warm hug. You reminisce about times before the world's chaos, sharing stories and catching up. After a " +
                "heartfelt conversation, you part ways with {gangLeader}, exchanging goodbyes and a final embrace.",
                
                //Event Outcome Good Relation B
                "{=Robbery_Event_Text_Good_Relation_Outcome_B}At night in {currentSettlement}, an alleyway shortcut unexpectedly " +
                "reunites you with {thugs} thugs, led by an old acquaintance, {gangLeader}. Upon realizing your identity, " +
                "{gangLeader} shares a hearty laugh and a friendly hug. You both reminisce about the past and update each " +
                "other on recent events. With a fond farewell and a last embrace, you continue on your way.",
                
                //Event Outcome Good Relation C
                "{=Robbery_Event_Text_Good_Relation_Outcome_C}In {currentSettlement} late at night, your alleyway detour " +
                "brings you face to face with {thugs} thugs and their leader, {gangLeader}. Recognizing an old comrade, " +
                "{gangLeader} greets you with jovial laughter and an affectionate hug. You spend a few moments exchanging " +
                "tales of the past and present before parting with a warm goodbye and a final embrace.",
                
                //Event Outcome Good Relation D
                "{=Robbery_Event_Text_Good_Relation_Outcome_D}Walking through an alley in {currentSettlement} at night, " +
                "you're stopped by {thugs} thugs. Their leader, {gangLeader}, initially demands valuables but then joyfully " +
                "recognizes you. A reunion filled with laughter and a friendly hug ensues, as you both reminisce over old " +
                "times. Eventually, you bid {gangLeader} farewell, embracing before parting ways.",
                
                //Event Outcome Good Relation E
                "{=Robbery_Event_Text_Good_Relation_Outcome_E}A late-night shortcut in {currentSettlement} turns into a " +
                "surprising reunion with {thugs} thugs and their chief, {gangLeader}. Realizing your long-standing connection, " +
                "{gangLeader} greets you with laughter and a heartfelt hug. You catch up on old memories and current " +
                "affairs, then say goodbye with a final warm embrace."

            };
            
            private static readonly List<string> eventMsgBadOutcome = new List<string>
            {
                "{=Robbery_Event_Text_Bad_Outcome_Msg_A}{heroName} loses {goldLost} gold to {gangLeader}'s thugs in {currentSettlement}, waking up later in a pigsty.",
                "{=Robbery_Event_Text_Bad_Outcome_Msg_B}Cornered by {gangLeader}'s gang in {currentSettlement}, {heroName} is robbed of {goldLost} gold and left in a pigsty.",
                "{=Robbery_Event_Text_Bad_Outcome_Msg_C}Ambushed in a {currentSettlement} alley, {heroName} hands {goldLost} gold to {gangLeader}'s thugs and wakes in a pigsty.",
                "{=Robbery_Event_Text_Bad_Outcome_Msg_D}In {currentSettlement}, {heroName} gives {gangLeader}'s thugs {goldLost} gold and regains consciousness in a pigsty.",
                "{=Robbery_Event_Text_Bad_Outcome_Msg_E}{heroName}, trapped by {gangLeader}'s gang in {currentSettlement}, loses {goldLost} gold and wakes up in a pigsty."
            };
            
            private static readonly List<string> eventMsgConvincedOutcome = new List<string>
            {
                "{=Robbery_Event_Text_Convinced_Msg_A}In a {currentSettlement} alley, {heroName} is confronted by {gangLeader}'s thugs but convinces them to leave, escaping unharmed.",
                "{=Robbery_Event_Text_Convinced_Msg_B}{heroName} talks their way out of a robbery by {gangLeader}'s gang in {currentSettlement}, leaving the alley safely.",
                "{=Robbery_Event_Text_Convinced_Msg_C}Cornered in {currentSettlement}, {heroName} avoids a mugging by {gangLeader}'s thugs through quick wit.",
                "{=Robbery_Event_Text_Convinced_Msg_D}{heroName} faces down {gangLeader}'s thugs in a {currentSettlement} alley and escapes without harm after being recognized.",
                "{=Robbery_Event_Text_Convinced_Msg_E}In {currentSettlement}, {heroName} is cornered by {gangLeader}'s thugs but evades trouble by revealing their identity."
            };
            
            private static readonly List<string> eventMsgCharmedOutcome = new List<string>
            {
                "{=Robbery_Event_Text_Charmed_Msg_A}In a {currentSettlement} alley, {heroName} diverts {gangLeader}'s attention with a tale, parting with 100 gold for a safe exit.",
                "{=Robbery_Event_Text_Charmed_Msg_B}{heroName} avoids a mugging in {currentSettlement} by intriguing {gangLeader} with rumors, losing 100 gold but gaining freedom.",
                "{=Robbery_Event_Text_Charmed_Msg_C}Cornered by {gangLeader}'s thugs, {heroName} spins a story of a rich visitor in {currentSettlement}, escaping with a 100 gold loss.",
                "{=Robbery_Event_Text_Charmed_Msg_D}Facing {gangLeader}'s thugs in {currentSettlement}, {heroName} talks of wealthy strangers, exchanging 100 gold for safety.",
                "{=Robbery_Event_Text_Charmed_Msg_E}{heroName} in {currentSettlement} distracts {gangLeader} with tales of affluence, trading 100 gold for an unharmed departure."
            };
            
            private static readonly List<string> eventMsgIntimidatedOutcome = new List<string>
            { 
                "{=Robbery_Event_Text_Intimidated_Msg_A}In {currentSettlement}, {heroName} thwarts {gangLeader}'s thugs with tales of battle, leaving unscathed with a warning of justice.",
                "{=Robbery_Event_Text_Intimidated_Msg_B}{heroName} laughs off {gangLeader}'s ambush in {currentSettlement}, intimidating the thugs into retreat with tales of martial prowess.",
                "{=Robbery_Event_Text_Intimidated_Msg_C}{heroName}'s mockery and battlefield stories unnerve {gangLeader}'s thugs in {currentSettlement}, allowing a safe departure.",
                "{=Robbery_Event_Text_Intimidated_Msg_D}Faced with {gangLeader}'s thugs in {currentSettlement}, {heroName} uses fear, recounting past victories to secure an escape.",
                "{=Robbery_Event_Text_Intimidated_Msg_E}In an alley of {currentSettlement}, {heroName} intimidates {gangLeader}'s gang with daunting tales, leaving a stern warning behind."
            };
            
            private static readonly List<string> eventMsgGoodRelationOutcome = new List<string>
            { 
                "{=Robbery_Event_Text_Good_Relation_Msg_A}In {currentSettlement}, {heroName}'s chance meeting with {gangLeader}'s thugs turns into a friendly reunion, reminiscing old times.",
                "{=Robbery_Event_Text_Good_Relation_Msg_B}{heroName} encounters old friend {gangLeader} in {currentSettlement}, leading to laughter, stories, and a warm farewell.",
                "{=Robbery_Event_Text_Good_Relation_Msg_C}An alley in {currentSettlement} brings {heroName} and {gangLeader} together, sharing fond memories before parting ways.",
                "{=Robbery_Event_Text_Good_Relation_Msg_D}{heroName} runs into {gangLeader} and thugs in {currentSettlement}, turning a potential mugging into a joyful catch-up.",
                "{=Robbery_Event_Text_Good_Relation_Msg_E}A surprise reunion with {gangLeader} in {currentSettlement} transforms {heroName}'s night into a nostalgic exchange of tales and goodbyes."
            };

            
            public static string GetRandomEventTitle()
            {
                var index = random.Next(eventTitles.Count);
                return eventTitles[index];
            }
            
            public static string GetRandomEventOutcomeBad()
            {
                var index = random.Next(eventOutcomeBad.Count);
                return eventOutcomeBad[index];
            }
            
            public static string GetRandomEventOutcomeConvinced()
            {
                var index = random.Next(eventOutcomeConvinced.Count);
                return eventOutcomeConvinced[index];
            }
            
            public static string GetRandomEventOutcomeCharmed()
            {
                var index = random.Next(eventOutcomeCharmed.Count);
                return eventOutcomeCharmed[index];
            }
            
            public static string GetRandomEventOutcomeIntimidated()
            {
                var index = random.Next(eventOutcomeIntimidated.Count);
                return eventOutcomeIntimidated[index];
            }
            
            public static string GetRandomEventOutcomeGoodRelations()
            {
                var index = random.Next(eventOutcomeGood.Count);
                return eventOutcomeGood[index];
            }
            
            public static string GetRandomEventMessageBadOutcome()
            {
                var index = random.Next(eventMsgBadOutcome.Count);
                return eventMsgBadOutcome[index];
            }
            
            public static string GetRandomEventMessageConvincedOutcome()
            {
                var index = random.Next(eventMsgConvincedOutcome.Count);
                return eventMsgConvincedOutcome[index];
            }
            
            public static string GetRandomEventMessageCharmedOutcome()
            {
                var index = random.Next(eventMsgCharmedOutcome.Count);
                return eventMsgCharmedOutcome[index];
            }
            
            public static string GetRandomEventMessageIntimidatedOutcome()
            {
                var index = random.Next(eventMsgIntimidatedOutcome.Count);
                return eventMsgIntimidatedOutcome[index];
            }
            
            public static string GetRandomEventMessageGoodRelationOutcome()
            {
                var index = random.Next(eventMsgGoodRelationOutcome.Count);
                return eventMsgGoodRelationOutcome[index];
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