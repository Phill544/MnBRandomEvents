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
using TaleWorlds.ObjectSystem;

namespace Bannerlord.RandomEvents.Events.CCEvents
{
    public sealed class SuddenStorm : BaseEvent
    {
        private readonly bool eventDisabled;
        private readonly int minHorsesLost;
        private readonly int maxHorsesLost;
        private readonly int minMenDied;
        private readonly int maxMenDied;
        private readonly int minMenWounded;
        private readonly int maxMenWounded;
        private readonly int minMeatFromHorse;
        private readonly int maxMeatFromHorse;

        public SuddenStorm() : base(ModSettings.RandomEvents.SuddenStormData)
        {
            var ConfigFile = new IniFile(ParseIniFile.GetTheConfigFile());
            
            eventDisabled = ConfigFile.ReadBoolean("SuddenStorm", "EventDisabled");
            minHorsesLost = ConfigFile.ReadInteger("SuddenStorm", "MinHorsesLost");
            maxHorsesLost = ConfigFile.ReadInteger("SuddenStorm", "MaxHorsesLost");
            minMenDied = ConfigFile.ReadInteger("SuddenStorm", "MinMenDied");
            maxMenDied = ConfigFile.ReadInteger("SuddenStorm", "MaxMenDied");
            minMenWounded = ConfigFile.ReadInteger("SuddenStorm", "MinMenWounded");
            maxMenWounded = ConfigFile.ReadInteger("SuddenStorm", "MaxMenWounded");
            minMeatFromHorse = ConfigFile.ReadInteger("SuddenStorm", "MinMeatFromHorse");
            maxMeatFromHorse = ConfigFile.ReadInteger("SuddenStorm", "MaxMeatFromHorse");
            
        }

        public override void CancelEvent()
        {
        }

        private bool HasValidEventData()
        {
            if (eventDisabled == false)
            {
                if (minHorsesLost != 0 || maxHorsesLost != 0 || minMenDied != 0 || maxMenDied != 0 || minMenWounded != 0 || maxMenWounded != 0 || minMeatFromHorse != 0 || maxMeatFromHorse != 0)
                {
                    return true;
                }
            }
            return false;
        }

        public override bool CanExecuteEvent()
        {
            return HasValidEventData() && Settlement.CurrentSettlement == null && MobileParty.MainParty.MemberRoster.TotalRegulars >= maxMenDied;

        }

        public override void StartEvent()
        {
            var eventTitle = new TextObject("{=SuddenStorm_Title}A Sudden Storm").ToString();

            var closestSettlement = ClosestSettlements.GetClosestAny(MobileParty.MainParty).ToString();
            
            var horsesLost = MBRandom.RandomInt(minHorsesLost, maxHorsesLost);
            var menDied = MBRandom.RandomInt(minMenDied, maxMenDied);
            var menWounded = MBRandom.RandomInt(minMenWounded, maxMenWounded);

            var meatFromHorseMultiplier = MBRandom.RandomInt(minMeatFromHorse, maxMeatFromHorse);

            var meatFromHorse = horsesLost * meatFromHorseMultiplier;
            
            var meat = MBObjectManager.Instance.GetObject<ItemObject>("meat");

            var eventDescription = new TextObject(
                    "{=SuddenStorm_Event_Desc}Your party is traveling near {closestSettlement} when you are suddenly " +
                    "caught in a massive storm. The hailstones are massive, the rain is pouring, the wind -- intense, " +
                    "and there is lightning strikes all around. You waste no time telling your men what do to.")
                .SetTextVariable("closestSettlement", closestSettlement)
                .ToString();
            
            var eventOption1 = new TextObject("{=SuddenStorm_Event_Option_1}Run for cover in the forest!").ToString();
            var eventOption1Hover = new TextObject("{=SuddenStorm_Event_Option_1_Hover}It offers some protection").ToString();

            var eventOption2 = new TextObject("{=SuddenStorm_Event_Option_2}Hide underneath the wagons!").ToString();
            var eventOption2Hover = new TextObject("{=SuddenStorm_Event_Option_2_Hover}Not all are going to fit").ToString();

            var eventOption3 = new TextObject("{=SuddenStorm_Event_Option_3}Press on a bit further").ToString();
            var eventOption3Hover = new TextObject("{=SuddenStorm_Event_Option_3_Hover}Try to find better shelter").ToString();

            var eventOption4 = new TextObject("{=SuddenStorm_Event_Option_4}The storm won't stop us!").ToString();
            var eventOption4Hover = new TextObject("{=SuddenStorm_Event_Option_4_Hover}You force your men to continue").ToString();

            var eventButtonText1 = new TextObject("{=SuddenStorm_Event_Button_Text_1}Choose").ToString();
            var eventButtonText2 = new TextObject("{=SuddenStorm_Event_Button_Text_2}Done").ToString();
            

            var inquiryElements = new List<InquiryElement>
            {
                new InquiryElement("a", eventOption1, null, true, eventOption1Hover),
                new InquiryElement("b", eventOption2, null, true, eventOption2Hover),
                new InquiryElement("c", eventOption3, null, true, eventOption3Hover),
                new InquiryElement("d", eventOption4, null, true, eventOption4Hover),
            };

            var eventOptionAText = new TextObject(
                    "{=SuddenStorm_Event_Choice_1}You order your men to run into the forest to the right. The men drop " +
                    "what they can and makes for the woods. Once inside, you look back and see the corpses of men and " +
                    "horses that weren't so quick. You cannot do anything for them now so you huddle together with your " +
                    "men and wait for the storm to pass.\n\nLuckily the storm passes just as quickly as it came. You then " +
                    "start to inspect the damage and help those who are wounded, deciding to take the wounded men to " +
                    "{closestSettlement} to get help there. All in all {horsesLost} horses and {menDied} men died from " +
                    "the storm. {menWounded} men were taken to {closestSettlement} to get help. You bury your deceased " +
                    "men in a single grave and manage to salvage {meatFromHorse} meat from the dead horses.")
                .SetTextVariable("closestSettlement", closestSettlement)
                .SetTextVariable("horsesLost", horsesLost)
                .SetTextVariable("menDied", menDied)
                .SetTextVariable("menWounded", menWounded)
                .SetTextVariable("meatFromHorse", meatFromHorse)
                .ToString();

            var eventOptionBText = new TextObject(
                    "{=SuddenStorm_Event_Choice_2}You order your men to hide underneath the wagons, but some men are " +
                    "running into the forest nearby. While you lay on the muddy ground you can hear the sound of the " +
                    "giant hail destroying some of your equipment as well as killing some horses. \n\nSuddenly a bolt " +
                    "of lightning hit the forest where some of your men had run. Over the deafening sound of the wind " +
                    "you can hear some of them crying out for help. You cannot do anything for them now so you and your " +
                    "men wait.\n\nLuckily the storm passes just as quickly as it came. You and your men start to inspect " +
                    "the damage and help those who are wounded in the forest, deciding to take the wounded men to " +
                    "{closestSettlement} to get help there. All in all {horsesLost} horses and {menDied} men died from " +
                    "the storm. {menWounded} men were taken to {closestSettlement} to get help. You bury your deceased " +
                    "men in a single grave and you manage to salvage {meatFromHorse} meat from the dead horses.")
                .SetTextVariable("closestSettlement", closestSettlement)
                .SetTextVariable("horsesLost", horsesLost)
                .SetTextVariable("menDied", menDied)
                .SetTextVariable("menWounded", menWounded)
                .SetTextVariable("meatFromHorse", meatFromHorse)
                .ToString();

            var eventOptionCText = new TextObject(
                    "{=SuddenStorm_Event_Choice_3}You order your men to press on through the storm in an attempt to find " +
                    "better shelter. The men reluctantly follow your lead towards a rock face not to far away. When you " +
                    "reach it you order your men to dismount and stand close to the wall. The men all do as you say and " +
                    "you all stand there for what seams like forever in waiting for the storm to pass. A lightning bolt " +
                    "rips through the sky and the deafening boom that follows spooks the horses so they start to run in " +
                    "all directions. Some of your men start to run after the horses but you manage to convince them to " +
                    "stay by saying that we will search once the storm has passed.\n\nThe storm passes relatively quickly. " +
                    "You and your men start to inspect the damage to your gear and help those who have been wounded. " +
                    "You decide to take the wounded men to {closestSettlement} to get help there. After a few hours you " +
                    "find all the surviving horses. All in all {horsesLost} horses died and {menWounded} men were wounded " +
                    "and they were taken to {closestSettlement} to get help. You manage to salvage {meatFromHorse} meat " +
                    "from the dead horses.")
                .SetTextVariable("closestSettlement", closestSettlement)
                .SetTextVariable("horsesLost", horsesLost)
                .SetTextVariable("menWounded", menWounded)
                .SetTextVariable("meatFromHorse", meatFromHorse)
                .ToString();

            var eventOptionDText = new TextObject(
                    "{=SuddenStorm_Event_Choice_4}You tell your men that no storm shall stop you and order them to march " +
                    "though the storm. After a few minutes you hear the men begging you to stop, telling you that some of " +
                    "your men already have been killed by the giant hail. Hearing this you agree to stop and everyone takes " +
                    "cover. As you jump off your horse you suddenly feel an intense pain in the back of your head moments " +
                    "before the world fades to darkness.\n\nYou are awoken by several of your men uttering your name and " +
                    "pouring water in your face. They help you to your feet. The sun is shining, the birds are singing and " +
                    "there is no sign of the storm except distant rumbling. One of your men shows you the helmet you were " +
                    "wearing. You are shocked when you see it has a massive dent in the back from the hail. If you weren't " +
                    "wearing this there is no doubt you would be dead.\nYou and your men start to inspect the damage and help " +
                    "those who are wounded. You decide to take the wounded men to {closestSettlement} to get help there. " +
                    "All in all {horsesLost} horses and {menDied} men died from the storm, but you cannot help thinking " +
                    "that number would have been lower if you had stopped sooner. {menWounded} men were taken to " +
                    "{closestSettlement} to get help. You bury your deceased men in a single grave and you manage to " +
                    "salvage {meatFromHorse} meat from the dead horses.")
                .SetTextVariable("closestSettlement", closestSettlement)
                .SetTextVariable("horsesLost", horsesLost)
                .SetTextVariable("menDied", menDied)
                .SetTextVariable("menWounded", menWounded)
                .SetTextVariable("meatFromHorse", meatFromHorse)
                .ToString();
                
            var eventMsg1 = new TextObject(
                    "{=SuddenStorm_Event_Msg_1}{heroName} lost {horsesLost} horses and {menDied} men to a sudden storm. He also received {meatFromHorse} meat from butchering the dead horses.")
                .SetTextVariable("heroName", Hero.MainHero.FirstName.ToString())
                .SetTextVariable("horsesLost", horsesLost)
                .SetTextVariable("menDied", menDied)
                .SetTextVariable("meatFromHorse", meatFromHorse)
                .ToString();
                
            var eventMsg2 = new TextObject(
                    "{=SuddenStorm_Event_Msg_2}{heroName} lost {horsesLost} horses and {menDied} men to a sudden storm. He also received {meatFromHorse} meat from butchering the dead horses.")
                .SetTextVariable("heroName", Hero.MainHero.FirstName.ToString())
                .SetTextVariable("horsesLost", horsesLost)
                .SetTextVariable("menDied", menDied)
                .SetTextVariable("meatFromHorse", meatFromHorse)
                .ToString();
                
            var eventMsg3 = new TextObject(
                    "{=SuddenStorm_Event_Msg_3}{heroName} lost {horsesLost} horses to a sudden storm. He received {meatFromHorse} meat from butchering the dead horses.")
                .SetTextVariable("heroName", Hero.MainHero.FirstName.ToString())
                .SetTextVariable("horsesLost", horsesLost)
                .SetTextVariable("meatFromHorse", meatFromHorse)
                .ToString();
                
            var eventMsg4 = new TextObject(
                    "{=SuddenStorm_Event_Msg_4}In refusing his men shelter from the storm, {heroName} lost {horsesLost} horses and {menDied} men to a sudden storm.")
                .SetTextVariable("heroName", Hero.MainHero.FirstName.ToString())
                .SetTextVariable("horsesLost", horsesLost)
                .SetTextVariable("menDied", menDied)
                .SetTextVariable("meatFromHorse", meatFromHorse)
                .ToString();
            
            var msid = new MultiSelectionInquiryData(eventTitle, eventDescription, inquiryElements, false, 1, 1,
                eventButtonText1, null,
                elements =>
                   {
                       switch ((string)elements[0].Identifier)
                       { 
                           case "a":
                                InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionAText, true, false, eventButtonText2, null, null, null), true);
                                
                                MobileParty.MainParty.ItemRoster.AddToCounts(meat, meatFromHorse);
                                MobileParty.MainParty.MemberRoster.KillNumberOfNonHeroTroopsRandomly(menDied);
                                MobileParty.MainParty.MemberRoster.WoundNumberOfTroopsRandomly(menWounded);
                                
                                InformationManager.DisplayMessage(new InformationMessage(eventMsg1, RandomEventsSubmodule.Msg_Color_MED_Outcome));
                                break;
                            case "b":
                                InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionBText, true, false, eventButtonText2, null, null, null), true);
                                
                                MobileParty.MainParty.ItemRoster.AddToCounts(meat, meatFromHorse);
                                MobileParty.MainParty.MemberRoster.KillNumberOfNonHeroTroopsRandomly(menDied);
                                MobileParty.MainParty.MemberRoster.WoundNumberOfTroopsRandomly(menWounded);
                                
                                InformationManager.DisplayMessage(new InformationMessage(eventMsg2, RandomEventsSubmodule.Msg_Color_MED_Outcome));
                                break;
                            case "c":
                                InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionCText, true, false, eventButtonText2, null, null, null), true);
                                
                                MobileParty.MainParty.ItemRoster.AddToCounts(meat, meatFromHorse);
                                MobileParty.MainParty.MemberRoster.WoundNumberOfTroopsRandomly(menWounded);
                                
                                InformationManager.DisplayMessage(new InformationMessage(eventMsg3, RandomEventsSubmodule.Msg_Color_MED_Outcome));
                                
                                break;
                            case "d":
                                InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionDText, true, false, eventButtonText2, null, null, null), true);
                                
                                MobileParty.MainParty.ItemRoster.AddToCounts(meat, meatFromHorse);
                                MobileParty.MainParty.MemberRoster.KillNumberOfNonHeroTroopsRandomly(menDied);
                                MobileParty.MainParty.MemberRoster.WoundNumberOfTroopsRandomly(menWounded);
                                
                                InformationManager.DisplayMessage(new InformationMessage(eventMsg4, RandomEventsSubmodule.Msg_Color_NEG_Outcome));
                                
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


    public class SuddenStormData : RandomEventData
    {
        public SuddenStormData(string eventType, float chanceWeight) : base(eventType, chanceWeight)
        {
        }

        public override BaseEvent GetBaseEvent()
        {
            return new SuddenStorm();
        }
    }
}