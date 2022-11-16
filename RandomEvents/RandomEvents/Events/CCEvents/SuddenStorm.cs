﻿using System;
using System.Collections.Generic;
using System.Windows;
using CryingBuffalo.RandomEvents.Helpers;
using CryingBuffalo.RandomEvents.Settings;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Inventory;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace CryingBuffalo.RandomEvents.Events.CCEvents
{
    public class SuddenStorm : BaseEvent
    {
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
            
            minHorsesLost = MCM_MenuConfig_N_Z.Instance.SuS_MinHorsesLost;
            maxHorsesLost = MCM_MenuConfig_N_Z.Instance.SuS_MaxHorsesLost;
            minMenDied = MCM_MenuConfig_N_Z.Instance.SuS_MinMenDied;
            maxMenDied = MCM_MenuConfig_N_Z.Instance.SuS_MaxMenDied;
            minMenWounded = MCM_MenuConfig_N_Z.Instance.SuS_MinMenWounded;
            maxMenWounded = MCM_MenuConfig_N_Z.Instance.SuS_MaxMenWounded;
            minMeatFromHorse = MCM_MenuConfig_N_Z.Instance.SuS_MinMeatFromHorse;
            maxMeatFromHorse = MCM_MenuConfig_N_Z.Instance.SuS_MaxMeatFromHorse;
            
        }

        public override void CancelEvent()
        {
        }

        public override bool CanExecuteEvent()
        {
            return MCM_MenuConfig_N_Z.Instance.SuS_Disable == false && Settlement.CurrentSettlement == null && MobileParty.MainParty.MemberRoster.TotalRegulars >= maxMenDied;

        }

        public override void StartEvent()
        {
            if (MCM_ConfigMenu_General.Instance.GS_DebugMode)
            {
                InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}",
                    RandomEventsSubmodule.Dbg_Color));
            }

            var eventTitle = new TextObject("{=SuddenStorm_Title}A Sudden Storm").ToString();

            var closestSettlement = ClosestSettlements.GetClosestAny(MobileParty.MainParty).ToString();
            
            var horsesLost = MBRandom.RandomInt(minHorsesLost, maxHorsesLost);
            var menDied = MBRandom.RandomInt(minMenDied, maxMenDied);
            var menWounded = MBRandom.RandomInt(minMenWounded, maxMenWounded);

            var meatFromHorseMultiplier = MBRandom.RandomInt(minMeatFromHorse, maxMeatFromHorse);

            var meatFromHorse = horsesLost * meatFromHorseMultiplier;
            
            var meat = MBObjectManager.Instance.GetObject<ItemObject>("meat");

            var eventDescription = new TextObject(
                    "{=SuddenStorm_Event_Desc}Your party is traveling though the near {closestSettlement} when you are suddenly caught in a massive hail and thunderstorm. The hails are massive, the rain is pouring, " +
                    "the wind is intense and there are lightning strikes all around you. You waste no time telling your men what do to.")
                .SetTextVariable("closestSettlement", closestSettlement)
                .ToString();
            
            var eventOption1 = new TextObject("{=SuddenStorm_Event_Option_1}Run for cover in the forest!").ToString();
            var eventOption1Hover = new TextObject("{=SuddenStorm_Event_Option_1_Hover}It offers some protection").ToString();

            var eventOption2 = new TextObject("{=SuddenStorm_Event_Option_2}Hide underneath the wagons!").ToString();
            var eventOption2Hover = new TextObject("{=SuddenStorm_Event_Option_2_Hover}Not all are going to fit").ToString();

            var eventOption3 = new TextObject("{=SuddenStorm_Event_Option_3}Press on a bit further").ToString();
            var eventOption3Hover = new TextObject("{=SuddenStorm_Event_Option_3_Hover}Try to find better shelter").ToString();

            var eventOption4 = new TextObject("{=SuddenStorm_Event_Option_4}The storm won't stop us").ToString();
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
                    "{=SuddenStorm_Event_Choice_1}You order your men to run into the forest to the right. The men drop what they can and makes for the woods. When you make it to the woods you look back and see " +
                    "that there are already some dead horses and wounded men on the ground. You cannot do anything for them now so you and your men huddle together in a group and wait for the storm to pass.\n\n" +
                    "Luckily the storm passes just as quickly as it came. You and your men start to inspect the damage and help those who are wounded. You decide to take the wounded men to {closestSettlement} " +
                    "to get help there. All in all {horsesLost} horses and {menDied} men died from the storm. {menWounded} men were taken to {closestSettlement} to get help. You bury your deceased men in a " +
                    "single grave and you manage to get {meatFromHorse} meat from the dead horses.")
                .SetTextVariable("closestSettlement", closestSettlement)
                .SetTextVariable("horsesLost", horsesLost)
                .SetTextVariable("menDied", menDied)
                .SetTextVariable("menWounded", menWounded)
                .SetTextVariable("meatFromHorse", meatFromHorse)
                .ToString();

            var eventOptionBText = new TextObject(
                    "{=SuddenStorm_Event_Choice_2}You order your men to hide underneath the wagons, but some men are running into the forest nearby. While you lay on the muddy ground beneath a wagon with some other " +
                    "you can hear the sound of the giant hail destroying some of your equipment as well as killing some horses. \n\nSuddenly a bolt of lightning hit the forest where some of your men had run into. Over " +
                    "the deafening sound of the wind you can hear some of them crying out for help.You cannot do anything for them now so you and your men wait for the storm to pass.\n\n" +
                    "Luckily the storm passes just as quickly as it came. You and your men start to inspect the damage and help those who are wounded in the forest. You decide to take the wounded men to " +
                    "{closestSettlement} to get help there. All in all {horsesLost} horses and {menDied} men died from the storm. {menWounded} men were taken to {closestSettlement} to get help. You bury your deceased men in a " +
                    "single grave and you manage to get {meatFromHorse} meat from the dead horses.")
                .SetTextVariable("closestSettlement", closestSettlement)
                .SetTextVariable("horsesLost", horsesLost)
                .SetTextVariable("menDied", menDied)
                .SetTextVariable("menWounded", menWounded)
                .SetTextVariable("meatFromHorse", meatFromHorse)
                .ToString();

            var eventOptionCText = new TextObject(
                    "{=SuddenStorm_Event_Choice_3}You order your men to press on through the storm in an attempt to find better shelter. The men reluctantly follow your lead towards a rock face not to far away. " +
                    "When you reach it you order your men to dismount and stand close to the wall. The men all do as you say and you all stand there for what seams like forever in waiting for the storm to pass. " +
                    "A lightning bolt rips through the sky and the deafening boom that follows spooks the horses so they start to run in all directions. Some of your mn start to run after the horses but you manage " +
                    "to stop them by saying the we all look for the horses after the storm passes.\n\n" +
                    "The storm passes relatively quick. You and your men start to inspect the damage to your gear and help those who have been wounded. You decide to take the wounded men to " +
                    "{closestSettlement} to get help there. After a few hours you have found all the surviving horses. " +
                    "All in all {horsesLost} horses died and {menWounded} men were wounded and they were taken to {closestSettlement} to get help. You manage to get {meatFromHorse} meat from the dead horses.")
                .SetTextVariable("closestSettlement", closestSettlement)
                .SetTextVariable("horsesLost", horsesLost)
                .SetTextVariable("menWounded", menWounded)
                .SetTextVariable("meatFromHorse", meatFromHorse)
                .ToString();

            var eventOptionDText = new TextObject(
                    "{=SuddenStorm_Event_Choice_4}You tell your men that no storm shall stop you so you order them to keep going though the storm. After a few minutes you hear them men begging you to stop saying " +
                    "that some of your men already have been killed by the giant hail. Hearing this you agree to stop and you and your men starting to take cover. As you jump of your horse you suddenly feel an " +
                    "intense pain in the back of your head before the world fades to darkness.\n\n" +
                    "You are awoken by several of your men saying your name and pouring water in your face. They help you to your feet. The sun is shining, the birds are singing and there is no sign of the storm " +
                    "except distant rumbling. One of your men shows you the helmet you were wearing. You are shocked when you see it has a massive dent in the back from the hail. If you weren't wearing this " +
                    "there is no doubt you would be dead.\nYou and your men start to inspect the damage and help those who are wounded. You decide to take the wounded men to {closestSettlement} " +
                    "to get help there. All in all {horsesLost} horses and {menDied} men died from the storm, but you cannot help thinking that number would have been lower if you had stopped sooner. " +
                    "{menWounded} men were taken to {closestSettlement} to get help. You bury your deceased men in a single grave and you manage to get {meatFromHorse} meat from the dead horses.")
                .SetTextVariable("closestSettlement", closestSettlement)
                .SetTextVariable("horsesLost", horsesLost)
                .SetTextVariable("menDied", menDied)
                .SetTextVariable("menWounded", menWounded)
                .SetTextVariable("meatFromHorse", meatFromHorse)
                .ToString();
                
            var eventMsg1 = new TextObject(
                    "{=SuddenStorm_Event_Msg_1}{heroName} lost {horsesLost} horses and {menDied} men to a sudden storm. He also got {meatFromHorse} meat from butchering the dead horses.")
                .SetTextVariable("heroName", Hero.MainHero.FirstName.ToString())
                .SetTextVariable("horsesLost", horsesLost)
                .SetTextVariable("menDied", menDied)
                .SetTextVariable("meatFromHorse", meatFromHorse)
                .ToString();
                
            var eventMsg2 = new TextObject(
                    "{=SuddenStorm_Event_Msg_2}{heroName} lost {horsesLost} horses and {menDied} men to a sudden storm. He also got {meatFromHorse} meat from butchering the dead horses.")
                .SetTextVariable("heroName", Hero.MainHero.FirstName.ToString())
                .SetTextVariable("horsesLost", horsesLost)
                .SetTextVariable("menDied", menDied)
                .SetTextVariable("meatFromHorse", meatFromHorse)
                .ToString();
                
            var eventMsg3 = new TextObject(
                    "{=SuddenStorm_Event_Msg_3}{heroName} lost {horsesLost} horses to a sudden storm. He got {meatFromHorse} meat from butchering the dead horses.")
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
            
            var msid = new MultiSelectionInquiryData(eventTitle, eventDescription, inquiryElements, false, 1,
                eventButtonText1, null,
                elements =>
                   {
                       switch ((string)elements[0].Identifier)
                       { 
                           case "a":
                                InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionAText, true, false, eventButtonText2, null, null, null), true);
                                
                                MobileParty.MainParty.ItemRoster.AddToCounts(meat, meatFromHorse);
                                MobileParty.MainParty.MemberRoster.KillNumberOfMenRandomly(menDied, false);
                                MobileParty.MainParty.MemberRoster.WoundNumberOfTroopsRandomly(menWounded);
                                
                                InformationManager.DisplayMessage(new InformationMessage(eventMsg1, RandomEventsSubmodule.Msg_Color));
                                break;
                            case "b":
                                InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionBText, true, false, eventButtonText2, null, null, null), true);
                                
                                MobileParty.MainParty.ItemRoster.AddToCounts(meat, meatFromHorse);
                                MobileParty.MainParty.MemberRoster.KillNumberOfMenRandomly(menDied, false);
                                MobileParty.MainParty.MemberRoster.WoundNumberOfTroopsRandomly(menWounded);
                                
                                InformationManager.DisplayMessage(new InformationMessage(eventMsg2, RandomEventsSubmodule.Msg_Color));
                                break;
                            case "c":
                                InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionCText, true, false, eventButtonText2, null, null, null), true);
                                
                                MobileParty.MainParty.ItemRoster.AddToCounts(meat, meatFromHorse);
                                MobileParty.MainParty.MemberRoster.WoundNumberOfTroopsRandomly(menWounded);
                                
                                InformationManager.DisplayMessage(new InformationMessage(eventMsg3, RandomEventsSubmodule.Msg_Color));
                                
                                break;
                            case "d":
                                InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionDText, true, false, eventButtonText2, null, null, null), true);
                                
                                MobileParty.MainParty.ItemRoster.AddToCounts(meat, meatFromHorse);
                                MobileParty.MainParty.MemberRoster.KillNumberOfMenRandomly(menDied, false);
                                MobileParty.MainParty.MemberRoster.WoundNumberOfTroopsRandomly(menWounded);
                                
                                InformationManager.DisplayMessage(new InformationMessage(eventMsg4, RandomEventsSubmodule.Msg_Color));
                                
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