using System;
using System.Collections.Generic;
using System.Windows;
using Bannerlord.RandomEvents.Helpers;
using Bannerlord.RandomEvents.Settings;
using Ini.Net;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace Bannerlord.RandomEvents.Events.CCEvents
{
    public sealed class RunawaySon : BaseEvent
    {
        private readonly bool eventDisabled;
        private readonly int minGold;
        private readonly int maxGold;
        private readonly int minRogueryLevel;

        public RunawaySon() : base(ModSettings.RandomEvents.RunawaySonData)
        {
            var ConfigFile = new IniFile(ParseIniFile.GetTheConfigFile());
            
            eventDisabled = ConfigFile.ReadBoolean("RunawaySon", "EventDisabled");
            minGold = ConfigFile.ReadInteger("RunawaySon", "MinGold");
            maxGold = ConfigFile.ReadInteger("RunawaySon", "MaxGold");
            minRogueryLevel = ConfigFile.ReadInteger("RunawaySon", "MinRogueryLevel");
        }

        public override void CancelEvent()
        {
        }

        private bool HasValidEventData()
        {
            if (eventDisabled == false)
            {
                if (minGold != 0 || maxGold != 0 || minRogueryLevel != 0)
                {
                    return true;
                }
            }
            
            return false;
        }

        public override bool CanExecuteEvent()
        {
            return HasValidEventData() && MobileParty.MainParty.CurrentSettlement == null;
        }

        public override void StartEvent()
        {
            var eventTitle = new TextObject("{=RunawaySon_Title}Runaway Son").ToString();
            
            var heroName = Hero.MainHero.FirstName;
            
            var goldLooted = MBRandom.RandomInt(minGold, maxGold);
            
            var closestSettlement = ClosestSettlements.GetClosestAny(MobileParty.MainParty).ToString();
            
            var heroRogueryLevel = Hero.MainHero.GetSkillValue(DefaultSkills.Roguery);

            var canKill = false;
            var rogueryAppendedText = "";
            
            if (GeneralSettings.SkillChecks.IsDisabled())
            {
                
                canKill = true;
                rogueryAppendedText = new TextObject("{=RunawaySon_Skill_Check_Disable_Appended_Text}**Skill checks are disabled**").ToString();

            }
            else
            {
                if (heroRogueryLevel >= minRogueryLevel)
                {
                    canKill = true;
                    
                    rogueryAppendedText = new TextObject("{=RunawaySon_Roguery_Appended_Text}[Roguery - lvl {minRogueryLevel}]")
                        .SetTextVariable("minRogueryLevel", minRogueryLevel)
                        .ToString();
                }
            }
            
            var eventDescription = new TextObject(
                    "{=RunawaySon_Event_Desc}As your party moves through the land near {closestSettlement}, you are approached by a young man. " +
                    "He explains that he ran away from the family farm after suffering abuse from his parents for years. He wants to join your party and he tells you he has some skills with weapons.")
                .SetTextVariable("closestSettlement", closestSettlement)
                .ToString();
            
            var eventOption1 = new TextObject("{=RunawaySon_Event_Option_1}Take him in and train him").ToString();
            var eventOption1Hover = new TextObject("{=RunawaySon_Event_Option_1_Hover}You could use the distraction of having someone to train").ToString();
            
            var eventOption2 = new TextObject("{=RunawaySon_Event_Option_2}Tell him he can tag along").ToString();
            var eventOption2Hover = new TextObject("{=RunawaySon_Event_Option_2_Hover}You really don't have time to babysit him").ToString();
            
            var eventOption3 = new TextObject("{=RunawaySon_Event_Option_3}Go away").ToString();
            var eventOption3Hover = new TextObject("{=RunawaySon_Event_Option_3_Hover}He needs to leave").ToString();
            
            var eventOption4 = new TextObject("{=RunawaySon_Event_Option_4}[Roguery] Kill him").ToString();
            var eventOption4Hover = new TextObject("{=RunawaySon_Event_Option_4_Hover}It's a cruel world.\n{rogueryAppendedText}").SetTextVariable("rogueryAppendedText",rogueryAppendedText).ToString();
            
            var eventButtonText1 = new TextObject("{=RunawaySon_Event_Button_Text_1}Okay").ToString();
            var eventButtonText2 = new TextObject("{=RunawaySon_Event_Button_Text_2}Done").ToString();
            
            var inquiryElements = new List<InquiryElement>();
            
            inquiryElements.Add(new InquiryElement("a", eventOption1, null, true, eventOption1Hover));
            inquiryElements.Add(new InquiryElement("b", eventOption2, null, true, eventOption2Hover));
            inquiryElements.Add(new InquiryElement("c", eventOption3, null, true, eventOption3Hover));
            
            if (canKill)
            {
                inquiryElements.Add(new InquiryElement("d", eventOption4, null, true, eventOption4Hover));
            }
            
            
            var eventOptionAText = new TextObject(
                    "{=RunawaySon_Event_Choice_1}You tell him he is welcome in your ranks and you will personally train him and make a fine solider of him.")
                .ToString();
            
            var eventOptionBText = new TextObject(
                    "{=RunawaySon_Event_Choice_2}You tell him he can tag along, but under no circumstance should he interfere in your affairs.")
                .ToString();
            
            var eventOptionCText = new TextObject(
                    "{=RunawaySon_Event_Choice_3}You tell him to get lost. The man turns around and promptly leaves.")
                .ToString();
            
            var eventOptionDText = new TextObject(
                    "{=RunawaySon_Event_Choice_4}You laugh as you hear his plea and your men soon join in on the laughter. You approach the man and thrust a " +
                    "dagger into his stomach. You watch him fall to the ground in a pool of blood, screaming in pain.\n" +
                    "You kneel down beside him and watch as the light soon leaves his eyes and he dies from his injury. " +
                    "You and some men decide to cut him open and hang his body from a tree as a warning but not before looting his body for {goldLooted} gold.")
                .SetTextVariable("goldLooted",goldLooted)
                .ToString();
            
            var eventMsg1 =new TextObject(
                    "{=RunawaySon_Event_Msg_1}{heroName} killed a young man and looted {goldLooted} from his corpse.")
                .SetTextVariable("heroName", heroName)
                .SetTextVariable("goldLooted", goldLooted)
                .ToString();

            var msid = new MultiSelectionInquiryData(eventTitle, eventDescription, inquiryElements, false, 1, 1, eventButtonText1, null,
                elements =>
                {
                    switch ((string)elements[0].Identifier)
                    {
                        case "a":
                            Hero.MainHero.AddSkillXp(DefaultSkills.Leadership, 30);
                            Hero.MainHero.AddSkillXp(DefaultSkills.Steward, 20);
                            
                            GiveOneRandomRecruitFromClosestCulture();
                            
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionAText, true, false, eventButtonText2, null, null, null), true);

                            break;
                        
                        case "b":
                            Hero.MainHero.AddSkillXp(DefaultSkills.Leadership, 20);
                            Hero.MainHero.AddSkillXp(DefaultSkills.Steward, 30);
                            
                            GiveOneRandomRecruitFromClosestCulture();
                            
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionBText, true, false, eventButtonText2, null, null, null), true);

                            break;
                        
                        case "c":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionCText, true, false, eventButtonText2, null, null, null), true);
                            break;
                        
                        case "d":
                            Hero.MainHero.AddSkillXp(DefaultSkills.Roguery, 150);
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionDText, true, false, eventButtonText2, null, null, null), true);
                            Hero.MainHero.ChangeHeroGold(goldLooted);
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg1, RandomEventsSubmodule.Msg_Color_EVIL_Outcome));
                            
                            
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

        private static void GiveOneRandomRecruitFromClosestCulture()
        {
            var closestSettlementCulture = ClosestSettlements.GetClosestAny(MobileParty.MainParty).Culture.ToString();

            var CultureDemonym = Demonym.GetTheDemonym(closestSettlementCulture, false);
            
            var troopRoster = TroopRoster.CreateDummyTroopRoster();

            // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
            foreach (var characterObject in CharacterObject.All)
            {
                if (characterObject.StringId.Contains("recruit") &&
                    !characterObject.StringId.Contains("vigla")
                    && characterObject.Culture.ToString() == closestSettlementCulture ||
                    (characterObject.StringId.Contains("footman") &&
                     !characterObject.StringId.Contains("vlandia")
                     && !characterObject.StringId.Contains("aserai") &&
                     characterObject.Culture.ToString() == closestSettlementCulture) ||
                    (characterObject.StringId.Contains("volunteer")
                     && characterObject.StringId.Contains("battanian") &&
                     characterObject.Culture.ToString() == closestSettlementCulture))
                {
                    troopRoster.AddToCounts(characterObject, 1);
                }
            }
            PartyScreenManager.OpenScreenAsReceiveTroops(troopRoster, leftPartyName: new TextObject("{CultureDemonym} Volunteer").SetTextVariable("CultureDemonym", CultureDemonym));
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


    public class RunawaySonData : RandomEventData
    {

        public RunawaySonData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
        {
        }

        public override BaseEvent GetBaseEvent()
        {
            return new RunawaySon();
        }
    }
}