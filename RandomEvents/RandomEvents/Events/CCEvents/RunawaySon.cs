using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using CryingBuffalo.RandomEvents.Helpers;
using CryingBuffalo.RandomEvents.Settings;
using CryingBuffalo.RandomEvents.Settings.MCM;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace CryingBuffalo.RandomEvents.Events.CCEvents
{
    public class RunawaySon : BaseEvent
    {
        private readonly int minGold;
        private readonly int maxGold;
        private readonly int minRogueryLevel;

        public RunawaySon() : base(ModSettings.RandomEvents.RunawaySonData)
        {
            minGold = MCM_MenuConfig_P_Z.Instance.RS_MinGoldGained;
            maxGold = MCM_MenuConfig_P_Z.Instance.RS_MaxGoldGained;
            minRogueryLevel = MCM_MenuConfig_P_Z.Instance.RS_minRogueryLevel;
        }

        public override void CancelEvent()
        {
        }

        public override bool CanExecuteEvent()
        {
            return MCM_MenuConfig_P_Z.Instance.RS_Disable == false && MobileParty.MainParty.CurrentSettlement == null;
        }

        public override void StartEvent()
        {
            if (MCM_ConfigMenu_General.Instance.GS_DebugMode)
            {
                InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}", RandomEventsSubmodule.Dbg_Color));
            }
            
            var eventTitle = new TextObject("{=RunawaySon_Title}Runaway Son").ToString();
            
            var heroName = Hero.MainHero.FirstName;
            
            var goldLooted = MBRandom.RandomInt(minGold, maxGold);
            
            var closestSettlement = ClosestSettlements.GetClosestAny(MobileParty.MainParty).ToString();
            
            var heroRogueryLevel = Hero.MainHero.GetSkillValue(DefaultSkills.Roguery);

            var canKill = false;
            var rogueryAppendedText = "";
            
            if (MCM_ConfigMenu_General.Instance.GS_DisableSkillChecks)
            {
                
                canKill = true;
                rogueryAppendedText = new TextObject("{=RunawaySon_Skill_Check_Disable_Appended_Text}**Skill checks are disabled**").ToString();

            }
            else
            {
                if (heroRogueryLevel >= minRogueryLevel)
                {
                    canKill = true;
                    
                    rogueryAppendedText = new TextObject("{=RunawaySon_Roguery_Appended_Text}[Roguery - lvl {rogueryLevel}]")
                        .SetTextVariable("rogueryLevel", heroRogueryLevel)
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
                    "dagger into his stomach. You watch him fall to the ground in a pool of blood, screaming in pain.\n " +
                    "You kneel down beside him and watch as the light soon leaves his eyes and he dies from his injury. " +
                    "You and some men decide to cut him open and hang his body from a tree as a warning but not before looting his body for {goldLooted} gold.")
                .SetTextVariable("goldLooted",goldLooted)
                .ToString();
            
            var eventMsg1 =new TextObject(
                    "{=RunawaySon_Event_Msg_1}{heroName} killed a young man and looted {goldLooted} from his corpse.")
                .SetTextVariable("heroName", heroName)
                .SetTextVariable("goldLooted", goldLooted)
                .ToString();

            var msid = new MultiSelectionInquiryData(eventTitle, eventDescription, inquiryElements, false, 1, eventButtonText1, null,
                elements =>
                {
                    switch ((string)elements[0].Identifier)
                    {
                        case "a":
                            Hero.MainHero.AddSkillXp(DefaultSkills.Leadership, 30);
                            Hero.MainHero.AddSkillXp(DefaultSkills.Steward, 20);
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionAText, true, false, eventButtonText2, null, null, null), true);

                            GainOneRecruit();
                            break;
                        
                        case "b":
                            Hero.MainHero.AddSkillXp(DefaultSkills.Leadership, 20);
                            Hero.MainHero.AddSkillXp(DefaultSkills.Steward, 30);
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionBText, true, false, eventButtonText2, null, null, null), true);
                            
                            GainOneRecruit();
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
                null);

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

        private static void GainOneRecruit()
        {
            var settlements = Settlement.FindAll(s => !s.IsHideout).ToList();
            var closestSettlement = settlements.MinBy(s => MobileParty.MainParty.GetPosition().DistanceSquared(s.GetPosition()));

            //Currently it gives just a random solider from the current culture.
            //PHILL
            
            var bandits = PartySetup.CreateBanditParty();
            bandits.MemberRoster.Clear();
            PartySetup.AddRandomCultureUnits(bandits, 1, closestSettlement.Culture);
            MobileParty.MainParty.MemberRoster.Add(bandits.MemberRoster);
            bandits.RemoveParty();
        }
    }


    public class RunawaySonData : RandomEventData
    {

        public RunawaySonData(string eventType, float chanceWeight) : base(eventType,
            chanceWeight)
        {
        }

        public override BaseEvent GetBaseEvent()
        {
            return new RunawaySon();
        }
    }
}