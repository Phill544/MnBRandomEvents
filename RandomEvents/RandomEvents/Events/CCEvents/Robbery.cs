using System;
using System.Linq;
using System.Windows;
using CryingBuffalo.RandomEvents.Settings;
using CryingBuffalo.RandomEvents.Settings.MCM;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace CryingBuffalo.RandomEvents.Events.CCEvents
{
    public class Robbery : BaseEvent
    {
        private readonly int minGoldLost;
        private readonly int maxGoldLost;
        private readonly int minRenownLost;
        private readonly int maxRenownLost;

        public Robbery() : base(ModSettings.RandomEvents.RobberyData)
        {
            minGoldLost = MCM_MenuConfig_N_Z.Instance.RO_MinGoldLost;
            maxGoldLost = MCM_MenuConfig_N_Z.Instance.RO_MaxGoldLost;
            minRenownLost = MCM_MenuConfig_N_Z.Instance.RO_MinRenownLost;
            maxRenownLost = MCM_MenuConfig_N_Z.Instance.RO_MaxRenownLost;
        }

        public override void CancelEvent()
        {
        }

        public override bool CanExecuteEvent()
        {
            if (Settlement.CurrentSettlement == null)
                return false;
            
            var notables = Settlement.CurrentSettlement.Notables.ToList();
            var gangLeaders = notables.Where(character => character.IsGangLeader).ToList();

            return gangLeaders.Count != 0 && CampaignTime.Now.IsNightTime;
        }

        public override void StartEvent()
        {
            if (MCM_ConfigMenu_General.Instance.GS_DebugMode)
            {
                InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}",
                    RandomEventsSubmodule.Dbg_Color));
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

            if (roguerySkill >= 150)
            {
                convincedThugs = true;
            }
            else if (charmSkill >= 200)
            {
                charmedThugs = true;
            }
            else if (oneHandedSkill >= 150 && twoHandedSkill >= 200)
            {
                intimidatedThugs = true;
            }
            
            var notables = Settlement.CurrentSettlement.Notables.ToList();
            
            var gangLeaders = notables.Where(character => character.IsGangLeader).ToList();

            var random = new Random();
            var index = random.Next(gangLeaders.Count);

            var gangLeader = gangLeaders[index];

            var gangLeaderName = gangLeader.Name.ToString();

            var gangLeaderRelation = gangLeader.GetBaseHeroRelation(Hero.MainHero);

            if (gangLeaderRelation >= 50)
            {
                gangLeaderGoodRelation = true;
            }

            var eventTitle = new TextObject("{=Robbery_Title}Robbery").ToString();

            var eventText_Bad_Outcome =new TextObject(
                    "{=Robbery_Event_Text_Bad_Outcome}One evening, while leaving the tavern in {currentSettlement} late at night you decide to take a shortcut back through an alley. You quickly realize that this was a bad idea " +
                    "as you are surrounded by {thugs} thugs who start threatening you. Their leader, {gangLeader}, tells you to hand over any valuables you have on you. Knowing you are outnumbered, you comply and hand " +
                    "over {goldLost} gold to {gangLeader} and the thugs.\n\n{gangLeader} tells the thugs to teach you a lesson that will keep you out of their alley next time. The next few minutes are a blur as you are " +
                    "punched, kicked, thrown, and intimidated.\nYou black out\n\nYou awake some time later only to realize the thugs have dumped you in a pigsty. You stumble to your feet and make " +
                    "haste back to where you are staying.")
                .SetTextVariable("currentSettlement", currentSettlement)
                .SetTextVariable("thugs", thugs)
                .SetTextVariable("gangLeader", gangLeaderName)
                .SetTextVariable("goldLost", goldLost)
                .ToString();
            
            var eventText_Convinced_Outcome =new TextObject(
                    "{=Robbery_Event_Text_Convinced_Outcome}One evening, while leaving the tavern in {currentSettlement} late at night you decide to take a shortcut back through an alley. You quickly realize that this was a bad idea " +
                    "as you are surrounded by {thugs} thugs who start threatening you. Their leader, {gangLeader}, tells you to hand over any valuables you have on you. You ask {gangLeader} if they are crazy and not recognize you. " +
                    "{gangLeader} and the thugs look at you one more time before they remember you. {gangLeader} apologized and withdraw the thugs. They let you pass through their alley unhindered. The next person however will " +
                    "be in for quite a surprise.")
                .SetTextVariable("currentSettlement", currentSettlement)
                .SetTextVariable("gangLeader", gangLeaderName)
                .ToString();
            
            var eventText_Charmed_Outcome =new TextObject(
                    "{=Robbery_Event_Text_Charmed_Outcome}One evening, while leaving the tavern in {currentSettlement} late at night you decide to take a shortcut back through an alley. You quickly realize that this was a bad idea " +
                    "as you are surrounded by {thugs} thugs who start threatening you. Their leader, {gangLeader}, tells you to hand over any valuables you have on you. Knowing you are outnumbered, you do what you do best, talk." +
                    "You strike up a somewhat friendly conversation with {gangLeader} and they actually seem interested in the conversation. You tell them som gossip you've heard about a rich guest visiting this city and so on. " +
                    "{gangLeader} takes one long look at you before ordering the thugs to let you go. You hand over 100 gold to {gangLeader} and bids farewell.")
                .SetTextVariable("currentSettlement", currentSettlement)
                .SetTextVariable("gangLeader", gangLeaderName)
                .ToString();
            
            var eventText_Intimidated_Outcome =new TextObject(
                    "{=Robbery_Event_Text_Intimidated_Outcome}One evening, while leaving the tavern in {currentSettlement} late at night you decide to take a shortcut back through an alley. You quickly realize that this was a bad idea " +
                    "as you are surrounded by {thugs} thugs who start threatening you. Their leader, {gangLeader}, tells you to hand over any valuables you have on you. You immediately burst out in laughter, much to " +
                    "the surprise of {gangLeader} and the thugs. You go on to tell them the deeds you have done on the battlefields of Calradia, about all the looters and bandits who have died by your hand. You can see " +
                    "the thugs are getting nervous. You ask {gangLeader} if they really think {thugs} thugs are gonna pose any threat to you. The thugs put away their weapons and {gangLeader} tells you that you " +
                    "may leave. You leave the alley but not before suggesting to the thugs that justice will soon catch up to them.")
                .SetTextVariable("currentSettlement", currentSettlement)
                .SetTextVariable("thugs", thugs)
                .SetTextVariable("gangLeader", gangLeaderName)
                .ToString();
            
            var eventText_Good_Relation_Outcome =new TextObject(
                    "{=Robbery_Event_Text_Good_Relation_Outcome}One evening, while leaving the tavern in {currentSettlement} late at night you decide to take a shortcut back through an alley. You quickly realize that this was a bad idea " +
                    "as you are surrounded by {thugs} thugs who start threatening you. Their leader, {gangLeader}, tells you to hand over any valuables you have on you. You ask {gangLeader} if they don't recognize you. " +
                    "{gangLeader} then approaches you and gives you a big hug while laughing. You and {gangLeader} go way back, back to the time before the turmoil the world is in. The two of you stand there talking " +
                    "for a few minutes and catches up on old and new. Eventually you tell {gangLeader} that it's time for you to get some rest. You say your goodbyes and embrace one last time.")
                .SetTextVariable("currentSettlement", currentSettlement)
                .SetTextVariable("thugs", thugs)
                .SetTextVariable("gangLeader", gangLeaderName)
                .ToString();
            
            var eventMsg1 =new TextObject(
                    "{=Robbery_Event_Msg_1}Clan {playerClan} lost {renownLost} renown after word spread that {heroName} was robbed by {gangLeader}'s thugs!")
                .SetTextVariable("playerClan", Clan.PlayerClan.Name.ToString())
                .SetTextVariable("renownLost", renownLost)
                .SetTextVariable("heroName", Hero.MainHero.FirstName.ToString())
                .SetTextVariable("gangLeader", gangLeaderName)
                .ToString();
            
            var eventMsg2=new TextObject(
                    "{=Robbery_Event_Msg_2}{heroName} managed to convince {gangLeader} and the thugs that they were not worth it.")
                .SetTextVariable("heroName", Hero.MainHero.FirstName.ToString())
                .SetTextVariable("gangLeader", gangLeaderName)
                .ToString();
            
            var eventMsg3=new TextObject(
                    "{=Robbery_Event_Msg_3}{heroName} managed to talk their way of a robbery by {gangLeader}.")
                .SetTextVariable("heroName", Hero.MainHero.FirstName.ToString())
                .SetTextVariable("gangLeader", gangLeaderName)
                .ToString();
            
            var eventMsg4=new TextObject(
                    "{=Robbery_Event_Msg_4}{heroName} managed to intimidate {gangLeader} and their thugs.")
                .SetTextVariable("heroName", Hero.MainHero.FirstName.ToString())
                .SetTextVariable("gangLeader", gangLeaderName)
                .ToString();


            var eventButtonText = new TextObject("{=Robbery_Event_Button_Text}Continue").ToString();

            if (charmedThugs == false && convincedThugs == false && intimidatedThugs == false && gangLeaderGoodRelation == false)
            {
                InformationManager.ShowInquiry(new InquiryData(eventTitle, eventText_Bad_Outcome, true, false, eventButtonText, null, null, null), true);
                
                Clan.PlayerClan.Renown = clanRenown - renownLost;
                Hero.MainHero.ChangeHeroGold(-goldLost);
                InformationManager.DisplayMessage(new InformationMessage(eventMsg1, RandomEventsSubmodule.Msg_Color));
            }
            else if (convincedThugs)
            {
                InformationManager.ShowInquiry(new InquiryData(eventTitle, eventText_Convinced_Outcome, true, false, eventButtonText, null, null, null), true);
                InformationManager.DisplayMessage(new InformationMessage(eventMsg2, RandomEventsSubmodule.Msg_Color));
            }
            else if (charmedThugs)
            {
                InformationManager.ShowInquiry(new InquiryData(eventTitle, eventText_Charmed_Outcome, true, false, eventButtonText, null, null, null), true);
                Hero.MainHero.ChangeHeroGold(-100);
                InformationManager.DisplayMessage(new InformationMessage(eventMsg3, RandomEventsSubmodule.Msg_Color));
            }
            else if (intimidatedThugs)
            {
                InformationManager.ShowInquiry(new InquiryData(eventTitle, eventText_Intimidated_Outcome, true, false, eventButtonText, null, null, null), true);
                InformationManager.DisplayMessage(new InformationMessage(eventMsg4, RandomEventsSubmodule.Msg_Color));
            }
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