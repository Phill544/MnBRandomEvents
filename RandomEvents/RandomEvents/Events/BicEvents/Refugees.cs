﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using CryingBuffalo.RandomEvents.Settings.MCM;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace CryingBuffalo.RandomEvents.Events.BicEvents
{
    public class Refugees : BaseEvent
    {
        // The letters correspond to the inquiry element ids
        private readonly int moraleGainA = 5;
        private readonly int moraleGainB = 5;
        private readonly int moraleLossC = 0;
        private readonly int moraleLossD = 0;
        // min max values
        private readonly int minSoldiers;
        private readonly int maxSoldiers;
        private readonly int minFood;
        private readonly int maxFood;
        private readonly int minCaptive;
        private readonly int maxCaptive;

        public Refugees() : base(Settings.ModSettings.RandomEvents.RefugeesData)
        {
            minSoldiers = Settings.ModSettings.RandomEvents.RefugeesData.minSoldiers;
            maxSoldiers = Settings.ModSettings.RandomEvents.RefugeesData.maxSoldiers;
            minFood = Settings.ModSettings.RandomEvents.RefugeesData.minFood;
            maxFood = Settings.ModSettings.RandomEvents.RefugeesData.maxFood;
            minCaptive = Settings.ModSettings.RandomEvents.RefugeesData.minCaptive;
            maxCaptive = Settings.ModSettings.RandomEvents.RefugeesData.maxCaptive;
        }

        public override void CancelEvent()
        {
        }

        public override bool CanExecuteEvent()
        {
            
            return true;
        }

        public override void StartEvent()
        {
            if (MCM_ConfigMenu_General.Instance.GS_DebugMode)
            {
                InformationManager.DisplayMessage(new InformationMessage($"Starting {randomEventData.eventType}", RandomEventsSubmodule.Dbg_Color));
            }
            
            //All the Var String ---------------
            var eventTitle = new TextObject("{=Refugees_Title}Refugees").ToString();
            var settlements = Settlement.FindAll(s => s.IsTown || s.IsCastle || s.IsVillage).ToList();
            var closestSettlement = settlements.MinBy(s => MobileParty.MainParty.GetPosition().DistanceSquared(s.GetPosition()));
            string cultureclass = closestSettlement.Culture.ToString();
            


            //Random Values __________________________________________________
            var soldiers = MBRandom.RandomInt(minSoldiers, maxSoldiers);
            var food = MBRandom.RandomInt(minFood, maxFood);
            var captives = MBRandom.RandomInt(minCaptive, maxCaptive);
        //________________________________________________________________    




            //Main Text ---------------------------------------------------------------------------
            var eventDescription = new TextObject(
                    "{=Refugees_Event_Desc} While travelling near {closestSettlement} you are met by a small band of refugees.  They look as though they haven't eaten in days, " +
                    "probably been on the run for quite some time enduring the harsh environment with very little clothing or food.  Despite their weakened state you figure " +
                    "the majority of these men would make fine soldiers if given a hot meal and good night's sleep.  Or.. perhaps more fitting as manual labor?")
                .SetTextVariable("closestSettlement", closestSettlement?.Name)
                .SetTextVariable("soldiers", soldiers)
                .ToString();
            
            //option A ---- Give Food ----
            var eventOption1 = new TextObject("{=Refugees_Event_Option_1}Give Food").ToString();
            var eventOption1Hover = new TextObject("{=Refugees_Event_Option_1_Hover}Help those in need").ToString();
            
            //option B ---- Recruit Into Party ----
            var eventOption2 = new TextObject("{=Refugees_Event_Option_2}Recruit").ToString();
            var eventOption2Hover = new TextObject("{=Refugees_Event_Option_2_Hover}They will make fine soldiers").ToString();
            
            //option C ---- Capture ----
            var eventOption3 = new TextObject("{=Refugees_Event_Option_3}Capture").ToString();
            var eventOption3Hover = new TextObject("{=Refugees_Event_Option_3_Hover}Refugees are criminals!").ToString();
            
            //option D ---- Ignore ----
            var eventOption4 = new TextObject("{=Refugees_Event_Option_4}Leave them").ToString();
            var eventOption4Hover = new TextObject("{=Refugees_Event_Option_4_Hover}Not your problem").ToString();
            
            //Finish ---------------____
            var eventButtonText1 = new TextObject("{=Refugees_Event_Button_Text_1}Choose").ToString();
            var eventButtonText2 = new TextObject("{=Refugees_Event_Button_Text_2}Done").ToString();
            
            var inquiryElements = new List<InquiryElement>
            {
                new InquiryElement("a", eventOption1, null, true, eventOption1Hover),
                new InquiryElement("b", eventOption2, null, true, eventOption2Hover),
                new InquiryElement("c", eventOption3, null, true, eventOption3Hover),
                new InquiryElement("d", eventOption4, null, true, eventOption4Hover)
            };
            

            // AFTER GIVE FOOD -------------------------------
            var eventOptionAText = new TextObject(
                   "{=Refugees_Event_Choice_1}You stop the refugees and signal a few of your men to grab some food from the rear of the formation.  Their eyes light up as they haven't had a full meal in days.")
                .ToString();

            // AFTER GIVE FOOD NO FOOD------------------------
            var eventOptionA2Text = new TextObject(
                    "{=Refugees_Event_Choice_1}Sadly, you do not have enough food to spare.  The least you can do is point them in the right direction and offer them a more efficient path to their destination.")
                .ToString();

            // AFTER RECRUIT INTO PARTY ----------------------
            var eventOptionBText = new TextObject(
                    "{=Refugees_Event_Choice_2}You stop the refugees and offer them a chance to join your ranks.  Most of the men volunteer as the rest continue their long journey home.")
                .ToString();
            
            // AFTER CAPTURE ---------------------------------
            var eventOptionCText = new TextObject(
                    "{=Refugees_Event_Choice_3}You stop the refugees and order your men to subdue them.  A few flee into the wilderness but you manage to capture most without a fight" +
                    " as they seem far too exhausted to give any effort towards conflict.")
                .ToString();
            
            // AFTER LEAVE IGNORE ----------------------------
            var eventOptionDText = new TextObject(
                    "{=Refugees_Event_Choice_4}You pass by the refugees without so much as a single word, and notice a few of your men throw bits of scrap for them to eat.  War is hell.")
                .ToString();


            //Bottom Left Game Messages ____________________________________
            var eventMsg1 =new TextObject(
                    "{=Refugees_Event_Msg_1}You offered food to the refugees.")
                .SetTextVariable("moraleGainA", moraleGainA)
                .ToString();
            
            var eventMsg2=new TextObject(
                    "{=Refugees_Event_Msg_2}You have recruited refugees.")
                .SetTextVariable("moraleGainB", moraleGainB)
                .ToString();
            
            var eventMsg3 =new TextObject(
                    "{=Refugees_Event_Msg_3} You have taken the refugees prisoner.")
                .SetTextVariable("moraleLossC", moraleLossC)
                .ToString();
            
            var eventMsg4 =new TextObject(
                    "{=Refugees_Event_Msg_4}You have ignored the refugees.")
                .SetTextVariable("moraleLossD", moraleLossD)
                .ToString();
            //________________________________________________________________

            var msid = new MultiSelectionInquiryData(eventTitle, eventDescription, inquiryElements, false, 1, eventButtonText1, null,
                elements =>
                {
                switch ((string)elements[0].Identifier)
                {


                    //Selected Option for Food -----------------------------------------------------------
                    case "a":
                        //-----all food types-----
                        var grain = MBObjectManager.Instance.GetObject<ItemObject>("grain");
                        var grainStorage = MobileParty.MainParty.ItemRoster.GetItemNumber(grain);
                        var fish = MBObjectManager.Instance.GetObject<ItemObject>("fish");
                        var fishStorage = MobileParty.MainParty.ItemRoster.GetItemNumber(fish);
                        var cheese = MBObjectManager.Instance.GetObject<ItemObject>("cheese");
                        var cheeseStorage = MobileParty.MainParty.ItemRoster.GetItemNumber(cheese);
                        var butter = MBObjectManager.Instance.GetObject<ItemObject>("butter");
                        var butterStorage = MobileParty.MainParty.ItemRoster.GetItemNumber(butter);
                        var meat = MBObjectManager.Instance.GetObject<ItemObject>("meat");
                        var meatStorage = MobileParty.MainParty.ItemRoster.GetItemNumber(meat);
                        var grapes = MBObjectManager.Instance.GetObject<ItemObject>("grape");
                        var grapesStorage = MobileParty.MainParty.ItemRoster.GetItemNumber(grapes);
                        var olives = MBObjectManager.Instance.GetObject<ItemObject>("olives");
                        var olivesStorage = MobileParty.MainParty.ItemRoster.GetItemNumber(olives);
                        var beer = MBObjectManager.Instance.GetObject<ItemObject>("beer");
                        var beerStorage = MobileParty.MainParty.ItemRoster.GetItemNumber(beer);
                        var datefruit = MBObjectManager.Instance.GetObject<ItemObject>("date_fruit");
                        var datefruitStorage = MobileParty.MainParty.ItemRoster.GetItemNumber(datefruit);

                        //Attempt to give the refugees food
                        int foodNeededToFeed = food;
                        bool providedEnoughFood = true;
                        if (grainStorage > 0 && foodNeededToFeed > 0)
                        {
                            MobileParty.MainParty.ItemRoster.AddToCounts(grain, -MathF.Min(grainStorage, foodNeededToFeed));
                            foodNeededToFeed -= grainStorage;
                        }

                        if (fishStorage > 0 && foodNeededToFeed > 0)
                        {
                            MobileParty.MainParty.ItemRoster.AddToCounts(fish, -MathF.Min(fishStorage, foodNeededToFeed));
                            foodNeededToFeed -= fishStorage;
                        }

                        if (cheeseStorage > 0 && foodNeededToFeed > 0)
                        {
                            MobileParty.MainParty.ItemRoster.AddToCounts(cheese, -MathF.Min(cheeseStorage, foodNeededToFeed));
                            foodNeededToFeed -= cheeseStorage;
                        }

                        if (butterStorage > 0 && foodNeededToFeed > 0)
                        {
                            MobileParty.MainParty.ItemRoster.AddToCounts(butter, -MathF.Min(butterStorage, foodNeededToFeed));
                            foodNeededToFeed -= butterStorage;
                        }

                        if (meatStorage > 0 && foodNeededToFeed > 0)
                        {
                            MobileParty.MainParty.ItemRoster.AddToCounts(meat, -MathF.Min(meatStorage, foodNeededToFeed));
                            foodNeededToFeed -= meatStorage;
                        }

                        if (grapesStorage > 0 && foodNeededToFeed > 0)
                        {
                            MobileParty.MainParty.ItemRoster.AddToCounts(grapes, -MathF.Min(grapesStorage, foodNeededToFeed));
                            foodNeededToFeed -= grapesStorage;
                        }

                        if (olivesStorage > 0 && foodNeededToFeed > 0)
                        {
                            MobileParty.MainParty.ItemRoster.AddToCounts(olives, -MathF.Min(olivesStorage, foodNeededToFeed));
                            foodNeededToFeed -= olivesStorage;
                        }

                        if (beerStorage > 0 && foodNeededToFeed > 0)
                        {
                            MobileParty.MainParty.ItemRoster.AddToCounts(beer, -MathF.Min(beerStorage, foodNeededToFeed));
                            foodNeededToFeed -= beerStorage;
                        }

                        if (datefruitStorage > 0 && foodNeededToFeed > 0)
                        {
                            MobileParty.MainParty.ItemRoster.AddToCounts(datefruit, -MathF.Min(datefruitStorage, foodNeededToFeed));
                            foodNeededToFeed -= datefruitStorage;
                        }

                        if (foodNeededToFeed > 0) //If the player hasn't give enough food to the refugees
                        {
                            InformationManager.ShowInquiry(
                                new InquiryData(eventTitle, eventOptionA2Text, true, false, eventButtonText2, null,
                                    null, null), true);
                            Hero.MainHero.AddSkillXp(DefaultSkills.Leadership, 30);
                            Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 20);
                            providedEnoughFood = false;
                        }

                        // If the player is able to provide the refugees with food, give the player a big boon
                        if (providedEnoughFood)
                        {
                            InformationManager.ShowInquiry(
                                new InquiryData(eventTitle, eventOptionAText, true, false, eventButtonText2, null, null,
                                    null), true);
                            MobileParty.MainParty.RecentEventsMorale += moraleGainA;
                            MobileParty.MainParty.MoraleExplained.Add(+moraleGainA);
                            Hero.MainHero.AddSkillXp(DefaultSkills.Leadership, 150);
                            Hero.MainHero.AddSkillXp(DefaultSkills.Charm, 50);
                        }

                        break;


                    //Selected Option for Recruitment----------------------------------------------------
                    case "b":
                        InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionBText, true, false, eventButtonText2, null, null, null), true);

                        MobileParty.MainParty.RecentEventsMorale += moraleGainB;
                        MobileParty.MainParty.MoraleExplained.Add(+moraleGainB);
                        Hero.MainHero.AddSkillXp(DefaultSkills.Steward, 150);
                        Hero.MainHero.AddSkillXp(DefaultSkills.Leadership, 50);

                            //Add Recruits
                        TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
                        for (int i = 0; i < CharacterObject.All.Count; i++)
                        {
                                CharacterObject characterObject = CharacterObject.All[i];

                                if (characterObject.StringId.Contains("recruit") && !characterObject.StringId.Contains("vigla") && characterObject.Culture.ToString() == cultureclass ||
                                            (characterObject.StringId.Contains("footman") && !characterObject.StringId.Contains("vlandia") && !characterObject.StringId.Contains("aserai") 
                                            && characterObject.Culture.ToString() == cultureclass) || (characterObject.StringId.Contains("volunteer")
                                            && (characterObject.StringId.Contains("battanian") && characterObject.Culture.ToString() == cultureclass)))
                                {
                                    troopRoster.AddToCounts(characterObject, soldiers);
                                }
                        }                      

                        PartyScreenManager.OpenScreenAsReceiveTroops(troopRoster, leftPartyName: new TextObject("{cultureclass} Refugees").SetTextVariable("cultureclass", cultureclass), null);

                        InformationManager.DisplayMessage(new InformationMessage(eventMsg2, RandomEventsSubmodule.Msg_Color_POS_Outcome));
                            
                        break;
                            //_______________________________________________________________________________________________________
                          

                            //Selected Option for Capture -------------------------------------------
                        case "c":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionCText, true, false, eventButtonText2, null, null, null), true);
                            
                            MobileParty.MainParty.RecentEventsMorale -= moraleLossC;
                            MobileParty.MainParty.MoraleExplained.Add(-moraleLossC);
                            Hero.MainHero.ChangeHeroGold(food * 5);
                            Hero.MainHero.AddSkillXp(DefaultSkills.Roguery, 135);


                            TroopRoster troopRoster2 = TroopRoster.CreateDummyTroopRoster();
                            for (int i = 0; i < CharacterObject.All.Count; i++)
                            {
                                CharacterObject characterObject = CharacterObject.All[i];

                                    if (characterObject.StringId.Contains("recruit") && !characterObject.StringId.Contains("vigla") 
                                                && characterObject.Culture.ToString() == cultureclass || (characterObject.StringId.Contains("footman") && !characterObject.StringId.Contains("vlandia")
                                                && !characterObject.StringId.Contains("aserai") && characterObject.Culture.ToString() == cultureclass) || (characterObject.StringId.Contains("volunteer") 
                                                && (characterObject.StringId.Contains("battanian") && characterObject.Culture.ToString() == cultureclass)))
                                    {
                                         troopRoster2.AddToCounts(characterObject, soldiers);
                                    }
                            }

                            TroopRoster emptyTroopRoster = TroopRoster.CreateDummyTroopRoster();

                            PartyScreenManager.OpenScreenAsLoot(emptyTroopRoster, troopRoster2, new TextObject("{cultureclass} Refugees").SetTextVariable("cultureclass", cultureclass), 20, null);

                            InformationManager.DisplayMessage(new InformationMessage(eventMsg3, RandomEventsSubmodule.Msg_Color_MED_Outcome));

                            break;
                            //___________________________________________________________________________________


                            //Selected Option for Leave Alone----------------------------------------
                        case "d":
                            InformationManager.ShowInquiry(new InquiryData(eventTitle, eventOptionDText, true, false, eventButtonText2, null, null, null), true);
                            
                            MobileParty.MainParty.RecentEventsMorale -= moraleLossD;
                            MobileParty.MainParty.MoraleExplained.Add(-moraleLossD);
                            
                            InformationManager.DisplayMessage(new InformationMessage(eventMsg4, RandomEventsSubmodule.Msg_Color_MED_Outcome));
                            
                            break;
                        default:
                            MessageBox.Show($"Error while selecting option for \"{randomEventData.eventType}\"");
                            break;
                            //___________________________________________________________________________________
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


    public class RefugeesData : RandomEventData
    {
        public readonly int minSoldiers;
        public readonly int maxSoldiers;
        public readonly int minFood;
        public readonly int maxFood;
        public readonly int minCaptive;
        public readonly int maxCaptive;

        public RefugeesData(string eventType, float chanceWeight, int minSoldiers, int maxSoldiers, int minFood, int maxFood, int minCaptive, int maxCaptive) : base(eventType,
            chanceWeight)
        {
            this.minSoldiers = minSoldiers;
            this.maxSoldiers = maxSoldiers;
            this.minFood = minFood;
            this.maxFood = maxFood;
            this.minCaptive = minCaptive;
            this.maxCaptive = maxCaptive;
        }

        public override BaseEvent GetBaseEvent()
        {
            return new Refugees();
        }
    }
}