using System;
using MCM.Abstractions.Base.Global;
using MCM.Abstractions.FluentBuilder;
using MCM.Common;
using TaleWorlds.Localization;

namespace CryingBuffalo.RandomEvents.Settings.MCM
{
    public class MCM_MenuConfig_Chances : IDisposable
    {
        private static MCM_MenuConfig_Chances _instance;

        private FluentGlobalSettings globalSettings;
        
        public float A_Flirtatious_Encounter_Chance{ get; private set; }
        public float Ahead_Of_Time_Chance{ get; private set; }
        public float Army_Games_Chance { get; private set; }
        public float ArmyInvite_Chance { get; private set; }
        public float Bandit_Ambush_Chance{ get; private set; }
        public float Bee_Kind_Chance{ get; private set; }
        public float Bet_Money_Chance{ get; private set; }
        public float Beggar_Begging_Chance{ get; private set; }
        public float Bird_Songs_Chance { get; private set; }
        public float Birthday_Party_Chance{ get; private set; }
        public float Bottoms_Up_Chance { get; private set; }
        public float Bumper_Crop_Chance{ get; private set; }
        public float Bunch_Of_Prisoners_Chance{ get; private set; }
        public float Chatting_Commanders_Chance{ get; private set; }
        public float Companion_Admire_Chance { get; private set; }
        public float Courier_Chance { get; private set; }
        public float Diseased_City_Chance{ get; private set; }
        public float Dreaded_Sweats_Chance { get; private set; }
        public float Duel_Chance { get; private set; }
        public float Dysentery_Chance { get; private set; }
        public float Eager_Troops_Chance{ get; private set; }
        public float Exotic_Drinks_Chance{ get; private set; }
        public float Fallen_Soldier_Family_Chance{ get; private set; }
        public float Fantastic_Fighters_Chance{ get; private set; }
        public float Feast_Chance { get; private set; }
        public float Fishing_Spot_Chance{ get; private set; }
        public float Food_Fight_Chance{ get; private set; }
        public float Granary_Rats_Chance{ get; private set; }
        public float Hot_Springs_Chance{ get; private set; }
        public float Hunting_Trip_Chance{ get; private set; }
        public float Lights_In_the_Skies_Chance{ get; private set; }
        public float Logging_Site_Chance{ get; private set; }
        public float Look_Up_Chance{ get; private set; }
        public float Mass_Grave_Chance{ get; private set; }
        public float Momentum_Chance{ get; private set; }
        public float Not_Of_This_World_Chance{ get; private set; }
        public float Old_Ruins_Chance{ get; private set; }
        public float Passing_Comet_Chance{ get; private set; }
        public float Perfect_Weather_Chance{ get; private set; }
        public float Prisoner_Rebellion_Chance{ get; private set; }
        public float Prisoner_Transfer_Chance{ get; private set; }
        public float Red_Moon_Chance{ get; private set; }
        public float Refugees_Chance { get; private set; }
        public float Robbery_Chance { get; private set; }
        public float Runaway_Son_Chance{ get; private set; }
        public float Secret_Singer_Chance{ get; private set; }
        public float Speedy_Recovery_Chance{ get; private set; }
        public float Successful_Deeds_Chance{ get; private set; }
        public float Sudden_Storm_Chance{ get; private set; }
        public float Supernatural_Encounter_Chance{ get; private set; }
        public float Target_Practice_Chance{ get; private set; }
        public float Travelling_Merchant_Chance { get; private set; }
        public float Travellers_Chance { get; private set; }
        public float Unexpected_Wedding_Chance{ get; private set; }
        public float Undercooked_Chance{ get; private set; }
        public float Violated_Girl_Chance{ get; private set; }
        public float Wandering_Livestock_Chance{ get; private set; }
        private bool CHANCES_FirstRunDone { get; set; }
        


        public static MCM_MenuConfig_Chances Instance
        {
            get { return _instance ??= new MCM_MenuConfig_Chances(); }
        }

        public void Settings()
        {
            

            #region Strings

            var adv_heading = new TextObject("{=mcm_adv_heading}Encounter Chance Settings").ToString();
            var adv_hint = new TextObject("{=mcm_adv1_hint}The chance to see this event will increase the higher the number. Please note that if all events are equal, they will all have the same chance.").ToString();
            var adv1_text = new TextObject("{=mcm_adv1_text}A Flirtatious Encounter").ToString();
            var adv2_text = new TextObject("{=mcm_adv2_text}Ahead of Time").ToString();
            var adv3_text = new TextObject("{=mcm_adv3_text}Bandit Ambush").ToString();
            var adv4_text = new TextObject("{=mcm_adv4_text}Bee Kind").ToString();
            var adv5_text = new TextObject("{=mcm_adv5_text}Bet Money").ToString();
            var adv6_text = new TextObject("{=mcm_adv6_text}Beggar").ToString();
            var adv7_text = new TextObject("{=mcm_adv7_text}Birthday Party").ToString();
            var adv8_text = new TextObject("{=mcm_adv8_text}Bumper Crop").ToString();
            var adv9_text = new TextObject("{=mcm_adv9_text}Bunch of Prisoners").ToString();
            var adv10_text = new TextObject("{=mcm_adv10_text}Chatting Commanders").ToString();
            var adv11_text = new TextObject("{=mcm_adv11_text}Diseased City").ToString();
            var adv12_text = new TextObject("{=mcm_adv12_text}Eager Troops").ToString();
            var adv13_text = new TextObject("{=mcm_adv13_text}Exotic Drinks").ToString();
            var adv14_text = new TextObject("{=mcm_adv14_text}Fallen Soldier Family").ToString();
            var adv15_text = new TextObject("{=mcm_adv15_text}Fantastic Fighters").ToString();
            var adv16_text = new TextObject("{=mcm_adv16_text}Fishing Spot").ToString();
            var adv17_text = new TextObject("{=mcm_adv17_text}Food Fight").ToString();
            var adv18_text = new TextObject("{=mcm_adv18_text}Granary Rats").ToString();
            var adv19_text = new TextObject("{=mcm_adv19_text}Hot Springs").ToString();
            var adv20_text = new TextObject("{=mcm_adv20_text}Hunting Trip").ToString();
            var adv21_text = new TextObject("{=mcm_adv21_text}Logging Site").ToString();
            var adv22_text = new TextObject("{=mcm_adv22_text}Look Up").ToString();
            var adv23_text = new TextObject("{=mcm_adv23_text}Mass Grave").ToString();
            var adv24_text = new TextObject("{=mcm_adv24_text}Momentum").ToString();
            var adv25_text = new TextObject("{=mcm_adv25_text}Not of this World").ToString();
            var adv26_text = new TextObject("{=mcm_adv26_text}Old Ruins").ToString();
            var adv27_text = new TextObject("{=mcm_adv27_text}Passing Comet").ToString();
            var adv28_text = new TextObject("{=mcm_adv28_text}Perfect Weather").ToString();
            var adv29_text = new TextObject("{=mcm_adv29_text}Prisoner Rebellion").ToString();
            var adv30_text = new TextObject("{=mcm_adv30_text}Red Moon").ToString();
            var adv31_text = new TextObject("{=mcm_adv31_text}Runaway Son").ToString();
            var adv32_text = new TextObject("{=mcm_adv32_text}Secret Singer").ToString();
            var adv33_text = new TextObject("{=mcm_adv33_text}Speedy Recovery").ToString();
            var adv34_text = new TextObject("{=mcm_adv34_text}Successful Deeds").ToString();
            var adv35_text = new TextObject("{=mcm_adv35_text}Sudden Storm").ToString();
            var adv36_text = new TextObject("{=mcm_adv36_text}Supernatural Encounter").ToString();
            var adv37_text = new TextObject("{=mcm_adv37_text}Target Practice").ToString();
            var adv38_text = new TextObject("{=mcm_adv38_text}Unexpected Wedding").ToString();
            var adv39_text = new TextObject("{=mcm_adv39_text}Undercooked").ToString();
            var adv40_text = new TextObject("{=mcm_adv40_text}Violated Girl").ToString();
            var adv41_text = new TextObject("{=mcm_adv41_text}Wandering Livestock").ToString();
            var adv42_text = new TextObject("{=mcm_adv42_text}1. Initial Setup").ToString();
            var adv42_hint = new TextObject("{=mcm_adv42_hint}Uncheck this to re-run the Initial Setup and set all values back to the original.").ToString();
            var adv43_text = new TextObject("{=mcm_adv43_text}Prisoner Transfer").ToString();
            var adv44_text = new TextObject("{=mcm_adv44_text}Robbery").ToString();
            var adv45_text = new TextObject("{=mcm_adv45_text}Lights in the Skies").ToString();
            var adv46_text = new TextObject("{=mcm_adv46_text}Bird Songs").ToString();
            var adv47_text = new TextObject("{=mcm_adv47_text}Courier").ToString();
            var adv48_text = new TextObject("{=mcm_adv48_text}Dysentery").ToString();
            var adv49_text = new TextObject("{=mcm_adv49_text}Refugees").ToString();
            var adv50_text = new TextObject("{=mcm_adv50_text}Bottoms Up").ToString();
            var adv51_text = new TextObject("{=mcm_adv51_text}Dreaded Sweats").ToString();
            var adv52_text = new TextObject("{=mcm_adv52_text}Travelling Merchant").ToString();
            var adv53_text = new TextObject("{=mcm_adv53_text}Army Games").ToString();
            var adv54_text = new TextObject("{=mcm_adv54_text}Companion Admire").ToString();
            var adv55_text = new TextObject("{=mcm_adv55_text}Army Invite").ToString();
            var adv56_text = new TextObject("{=mcm_adv56_text}Feast").ToString();
            var adv57_text = new TextObject("{=mcm_adv57_text}Travellers").ToString();
            var adv58_text = new TextObject("{=mcm_adv58_text}Duel").ToString();

            #endregion



            var builder = BaseSettingsBuilder.Create("RandomEvents5","5. Random Events - Chances")!
                .SetFormat("xml")
                .SetFolderName(RandomEventsSubmodule.FolderName)
                .SetSubFolder(RandomEventsSubmodule.ModName)
                
           #region Builder Modules

                .CreateGroup(adv_heading, groupBuilder => groupBuilder
                        .AddFloatingInteger ("ADV1", adv1_text,5,100, new ProxyRef<float>(() => A_Flirtatious_Encounter_Chance, o => A_Flirtatious_Encounter_Chance = o), floatBuilder => floatBuilder
                            .SetHintText(adv_hint))
                        .AddFloatingInteger ("ADV2", adv2_text,5,100, new ProxyRef<float>(() => Ahead_Of_Time_Chance, o => Ahead_Of_Time_Chance = o), floatBuilder => floatBuilder
                            .SetHintText(adv_hint))
                        .AddFloatingInteger ("ADV3", adv3_text,5,100, new ProxyRef<float>(() => Bandit_Ambush_Chance, o => Bandit_Ambush_Chance = o), floatBuilder => floatBuilder
                            .SetHintText(adv_hint))
                        .AddFloatingInteger ("ADV4", adv4_text,5,100, new ProxyRef<float>(() => Bee_Kind_Chance, o => Bee_Kind_Chance = o), floatBuilder => floatBuilder
                            .SetHintText(adv_hint))
                        .AddFloatingInteger ("ADV5", adv5_text,5,100, new ProxyRef<float>(() => Bet_Money_Chance, o => Bet_Money_Chance = o), floatBuilder => floatBuilder
                            .SetHintText(adv_hint))
                        .AddFloatingInteger ("ADV6", adv6_text,5,100, new ProxyRef<float>(() => Beggar_Begging_Chance, o => Beggar_Begging_Chance = o), floatBuilder => floatBuilder
                            .SetHintText(adv_hint))
                        .AddFloatingInteger ("ADV7", adv7_text,5,100, new ProxyRef<float>(() => Birthday_Party_Chance, o => Birthday_Party_Chance = o), floatBuilder => floatBuilder
                            .SetHintText(adv_hint))
                        .AddFloatingInteger ("ADV8", adv8_text,5,100, new ProxyRef<float>(() => Bumper_Crop_Chance, o => Bumper_Crop_Chance = o), floatBuilder => floatBuilder
                            .SetHintText(adv_hint))
                        .AddFloatingInteger ("ADV9", adv9_text,5,100, new ProxyRef<float>(() => Bunch_Of_Prisoners_Chance, o => Bunch_Of_Prisoners_Chance = o), floatBuilder => floatBuilder
                            .SetHintText(adv_hint))
                        .AddFloatingInteger ("ADV10", adv10_text,5,100, new ProxyRef<float>(() => Chatting_Commanders_Chance, o => Chatting_Commanders_Chance = o), floatBuilder => floatBuilder
                            .SetHintText(adv_hint))
                        .AddFloatingInteger ("ADV11", adv11_text,5,100, new ProxyRef<float>(() => Diseased_City_Chance, o => Diseased_City_Chance = o), floatBuilder => floatBuilder
                            .SetHintText(adv_hint))
                        .AddFloatingInteger ("ADV12", adv12_text,5,100, new ProxyRef<float>(() => Eager_Troops_Chance, o => Eager_Troops_Chance = o), floatBuilder => floatBuilder
                            .SetHintText(adv_hint))
                        .AddFloatingInteger ("ADV13", adv13_text,5,100, new ProxyRef<float>(() => Exotic_Drinks_Chance, o => Exotic_Drinks_Chance = o), floatBuilder => floatBuilder
                            .SetHintText(adv_hint))
                        .AddFloatingInteger ("ADV14", adv14_text,5,100, new ProxyRef<float>(() => Fallen_Soldier_Family_Chance, o => Fallen_Soldier_Family_Chance = o), floatBuilder => floatBuilder
                            .SetHintText(adv_hint))
                        .AddFloatingInteger ("ADV15", adv15_text,5,100, new ProxyRef<float>(() => Fantastic_Fighters_Chance, o => Fantastic_Fighters_Chance = o), floatBuilder => floatBuilder
                            .SetHintText(adv_hint))
                        .AddFloatingInteger ("ADV16", adv16_text,5,100, new ProxyRef<float>(() => Fishing_Spot_Chance, o => Fishing_Spot_Chance = o), floatBuilder => floatBuilder
                            .SetHintText(adv_hint))
                        .AddFloatingInteger ("ADV17", adv17_text,5,100, new ProxyRef<float>(() => Food_Fight_Chance, o => Food_Fight_Chance = o), floatBuilder => floatBuilder
                            .SetHintText(adv_hint))
                        .AddFloatingInteger ("ADV18", adv18_text,5,100, new ProxyRef<float>(() => Granary_Rats_Chance, o => Granary_Rats_Chance = o), floatBuilder => floatBuilder
                            .SetHintText(adv_hint))
                        .AddFloatingInteger ("ADV19", adv19_text,5,100, new ProxyRef<float>(() => Hot_Springs_Chance, o => Hot_Springs_Chance = o), floatBuilder => floatBuilder
                            .SetHintText(adv_hint))
                        .AddFloatingInteger ("ADV20", adv20_text,5,100, new ProxyRef<float>(() => Hunting_Trip_Chance, o => Hunting_Trip_Chance = o), floatBuilder => floatBuilder
                            .SetHintText(adv_hint))
                        .AddFloatingInteger ("ADV21", adv21_text,5,100, new ProxyRef<float>(() => Logging_Site_Chance, o => Logging_Site_Chance = o), floatBuilder => floatBuilder
                            .SetHintText(adv_hint))
                        .AddFloatingInteger ("ADV22", adv22_text,5,100, new ProxyRef<float>(() => Look_Up_Chance, o => Look_Up_Chance = o), floatBuilder => floatBuilder
                            .SetHintText(adv_hint))
                        .AddFloatingInteger ("ADV23", adv23_text,5,100, new ProxyRef<float>(() => Mass_Grave_Chance, o => Mass_Grave_Chance = o), floatBuilder => floatBuilder
                            .SetHintText(adv_hint))
                        .AddFloatingInteger ("ADV24", adv24_text,5,100, new ProxyRef<float>(() => Momentum_Chance, o => Momentum_Chance = o), floatBuilder => floatBuilder
                            .SetHintText(adv_hint))
                        .AddFloatingInteger ("ADV25", adv25_text,5,100, new ProxyRef<float>(() => Not_Of_This_World_Chance, o => Not_Of_This_World_Chance = o), floatBuilder => floatBuilder
                            .SetHintText(adv_hint))
                        .AddFloatingInteger ("ADV26", adv26_text,5,100, new ProxyRef<float>(() => Old_Ruins_Chance, o => Old_Ruins_Chance = o), floatBuilder => floatBuilder
                            .SetHintText(adv_hint))
                        .AddFloatingInteger ("ADV27", adv27_text,5,100, new ProxyRef<float>(() => Passing_Comet_Chance, o => Passing_Comet_Chance = o), floatBuilder => floatBuilder
                            .SetHintText(adv_hint))
                        .AddFloatingInteger ("ADV28", adv28_text,5,100, new ProxyRef<float>(() => Perfect_Weather_Chance, o => Perfect_Weather_Chance = o), floatBuilder => floatBuilder
                            .SetHintText(adv_hint))
                        .AddFloatingInteger ("ADV29", adv29_text,5,100, new ProxyRef<float>(() => Prisoner_Rebellion_Chance, o => Prisoner_Rebellion_Chance = o), floatBuilder => floatBuilder
                            .SetHintText(adv_hint))
                        .AddFloatingInteger ("ADV30", adv30_text,5,100, new ProxyRef<float>(() => Red_Moon_Chance, o => Red_Moon_Chance = o), floatBuilder => floatBuilder
                            .SetHintText(adv_hint))
                        .AddFloatingInteger ("ADV31", adv31_text,5,100, new ProxyRef<float>(() => Runaway_Son_Chance, o => Runaway_Son_Chance = o), floatBuilder => floatBuilder
                            .SetHintText(adv_hint))
                        .AddFloatingInteger ("ADV32", adv32_text,5,100, new ProxyRef<float>(() => Secret_Singer_Chance, o => Secret_Singer_Chance = o), floatBuilder => floatBuilder
                            .SetHintText(adv_hint))
                        .AddFloatingInteger ("ADV33", adv33_text,5,100, new ProxyRef<float>(() => Speedy_Recovery_Chance, o => Speedy_Recovery_Chance = o), floatBuilder => floatBuilder
                            .SetHintText(adv_hint))
                        .AddFloatingInteger ("ADV34", adv34_text,5,100, new ProxyRef<float>(() => Successful_Deeds_Chance, o => Successful_Deeds_Chance = o), floatBuilder => floatBuilder
                            .SetHintText(adv_hint))
                        .AddFloatingInteger ("ADV35", adv35_text,5,100, new ProxyRef<float>(() => Sudden_Storm_Chance, o => Sudden_Storm_Chance = o), floatBuilder => floatBuilder
                            .SetHintText(adv_hint))
                        .AddFloatingInteger ("ADV36", adv36_text,5,100, new ProxyRef<float>(() => Supernatural_Encounter_Chance, o => Supernatural_Encounter_Chance = o), floatBuilder => floatBuilder
                            .SetHintText(adv_hint))
                        .AddFloatingInteger ("ADV37", adv37_text,5,100, new ProxyRef<float>(() => Target_Practice_Chance, o => Target_Practice_Chance = o), floatBuilder => floatBuilder
                            .SetHintText(adv_hint))
                        .AddFloatingInteger ("ADV38", adv38_text,5,100, new ProxyRef<float>(() => Unexpected_Wedding_Chance, o => Unexpected_Wedding_Chance = o), floatBuilder => floatBuilder
                            .SetHintText(adv_hint))
                        .AddFloatingInteger ("ADV39", adv39_text,5,100, new ProxyRef<float>(() => Undercooked_Chance, o => Undercooked_Chance = o), floatBuilder => floatBuilder
                            .SetHintText(adv_hint))
                        .AddFloatingInteger ("ADV40", adv40_text,5,100, new ProxyRef<float>(() => Violated_Girl_Chance, o => Violated_Girl_Chance = o), floatBuilder => floatBuilder
                            .SetHintText(adv_hint))
                        .AddFloatingInteger ("ADV41", adv41_text,5,100, new ProxyRef<float>(() => Wandering_Livestock_Chance, o => Wandering_Livestock_Chance = o), floatBuilder => floatBuilder
                            .SetHintText(adv_hint))
                        .AddBool("ADV42", adv42_text, new ProxyRef<bool>(() => CHANCES_FirstRunDone, o => CHANCES_FirstRunDone = o), boolBuilder => boolBuilder
                            .SetHintText(adv42_hint)
                            .SetRequireRestart(true))
                        .AddFloatingInteger ("ADV43", adv43_text,5,100, new ProxyRef<float>(() => Prisoner_Transfer_Chance, o => Prisoner_Transfer_Chance = o), floatBuilder => floatBuilder
                            .SetHintText(adv_hint))
                        .AddFloatingInteger ("ADV44", adv44_text,5,100, new ProxyRef<float>(() => Robbery_Chance, o => Robbery_Chance = o), floatBuilder => floatBuilder
                            .SetHintText(adv_hint))
                        .AddFloatingInteger ("ADV45", adv45_text,5,100, new ProxyRef<float>(() => Lights_In_the_Skies_Chance, o => Lights_In_the_Skies_Chance = o), floatBuilder => floatBuilder
                            .SetHintText(adv_hint))
                        .AddFloatingInteger ("ADV46", adv46_text,5,100, new ProxyRef<float>(() => Bird_Songs_Chance, o => Bird_Songs_Chance = o), floatBuilder => floatBuilder
                            .SetHintText(adv_hint))
                        .AddFloatingInteger ("ADV47", adv47_text,5, 100, new ProxyRef<float>(() => Courier_Chance, o => Courier_Chance = o), floatBuilder => floatBuilder
                            .SetHintText(adv_hint))
                        .AddFloatingInteger ("ADV48", adv48_text,5, 100, new ProxyRef<float>(() => Dysentery_Chance, o => Dysentery_Chance = o), floatBuilder => floatBuilder
                            .SetHintText(adv_hint))
                        .AddFloatingInteger ("ADV49", adv49_text,5, 100, new ProxyRef<float>(() => Refugees_Chance, o => Refugees_Chance = o), floatBuilder => floatBuilder
                            .SetHintText(adv_hint))
                        .AddFloatingInteger ("ADV50", adv50_text,5, 100, new ProxyRef<float>(() => Bottoms_Up_Chance, o => Bottoms_Up_Chance = o), floatBuilder => floatBuilder
                            .SetHintText(adv_hint))
                        .AddFloatingInteger ("ADV51", adv51_text,5, 100, new ProxyRef<float>(() => Dreaded_Sweats_Chance, o => Dreaded_Sweats_Chance = o), floatBuilder => floatBuilder
                            .SetHintText(adv_hint))
                        .AddFloatingInteger ("ADV52", adv52_text,5, 100, new ProxyRef<float>(() => Travelling_Merchant_Chance, o => Travelling_Merchant_Chance = o), floatBuilder => floatBuilder
                            .SetHintText(adv_hint))
                        .AddFloatingInteger ("ADV53", adv53_text,5, 100, new ProxyRef<float>(() => Army_Games_Chance, o => Army_Games_Chance = o), floatBuilder => floatBuilder
                            .SetHintText(adv_hint))
                        .AddFloatingInteger ("ADV54", adv54_text,5, 100, new ProxyRef<float>(() => Companion_Admire_Chance, o => Companion_Admire_Chance = o), floatBuilder => floatBuilder
                            .SetHintText(adv_hint))
                        .AddFloatingInteger ("ADV55", adv55_text, 5, 100, new ProxyRef<float>(() => ArmyInvite_Chance, o => ArmyInvite_Chance = o), floatBuilder => floatBuilder
                            .SetHintText(adv_hint))
                        .AddFloatingInteger ("ADV56", adv56_text, 5, 100, new ProxyRef<float>(() => Feast_Chance, o => Feast_Chance = o), floatBuilder => floatBuilder
                           .SetHintText(adv_hint))
                        .AddFloatingInteger ("ADV57", adv57_text, 5, 100, new ProxyRef<float>(() => Travellers_Chance, o => Travellers_Chance = o), floatBuilder => floatBuilder
                            .SetHintText(adv_hint))
                        .AddFloatingInteger ("ADV58", adv58_text, 5, 100, new ProxyRef<float>(() => Duel_Chance, o => Duel_Chance = o), floatBuilder => floatBuilder
                            .SetHintText(adv_hint))

                #endregion

                );



            globalSettings = builder.BuildAsGlobal();
            globalSettings.Register();

            if (!CHANCES_FirstRunDone)
            {
                Perform_First_Time_Setup();
            }
        }

        private static void Perform_First_Time_Setup()
        {
            
            Instance.CHANCES_FirstRunDone = true;
            Instance.A_Flirtatious_Encounter_Chance = 60.0f;
            Instance.Ahead_Of_Time_Chance = 50.0f;
            Instance.Army_Games_Chance = 60.0f;
            Instance.ArmyInvite_Chance = 60.0f;
            Instance.Bandit_Ambush_Chance = 50.0f;
            Instance.Bee_Kind_Chance = 50.0f;
            Instance.Bet_Money_Chance = 50.0f;
            Instance.Beggar_Begging_Chance = 60.0f;
            Instance.Bird_Songs_Chance = 50.0f;
            Instance.Birthday_Party_Chance = 50.0f;
            Instance.Bottoms_Up_Chance = 50.0f;
            Instance.Bunch_Of_Prisoners_Chance = 50.0f;
            Instance.Bumper_Crop_Chance = 50.0f;
            Instance.Chatting_Commanders_Chance = 60.0f;
            Instance.Courier_Chance = 50.0f;
            Instance.Companion_Admire_Chance = 20.0f;
            Instance.Diseased_City_Chance = 50.0f;
            Instance.Dreaded_Sweats_Chance = 35.0f;
            Instance.Duel_Chance = 35.0f;
            Instance.Dysentery_Chance = 35.0f;
            Instance.Eager_Troops_Chance = 50.0f;
            Instance.Exotic_Drinks_Chance = 50.0f;
            Instance.Fallen_Soldier_Family_Chance = 60.0f;
            Instance.Fantastic_Fighters_Chance = 50.0f;
            Instance.Feast_Chance = 60.0f;
            Instance.Fishing_Spot_Chance = 50.0f;
            Instance.Food_Fight_Chance = 50.0f;
            Instance.Granary_Rats_Chance = 50.0f;
            Instance.Hot_Springs_Chance = 50.0f;
            Instance.Hunting_Trip_Chance = 50.0f;
            Instance.Lights_In_the_Skies_Chance = 10.0f;
            Instance.Logging_Site_Chance = 50.0f;
            Instance.Look_Up_Chance = 50.0f;
            Instance.Mass_Grave_Chance = 50.0f;
            Instance.Momentum_Chance = 50.0f;
            Instance.Not_Of_This_World_Chance = 10.0f;
            Instance.Old_Ruins_Chance = 50.0f;
            Instance.Passing_Comet_Chance = 15.0f;
            Instance.Perfect_Weather_Chance = 50.0f;
            Instance.Prisoner_Rebellion_Chance = 50.0f;
            Instance.Prisoner_Transfer_Chance = 50.0f;
            Instance.Red_Moon_Chance = 25.0f;
            Instance.Refugees_Chance = 30.0f;
            Instance.Robbery_Chance = 60.0f;
            Instance.Runaway_Son_Chance = 50.0f;
            Instance.Secret_Singer_Chance = 50.0f;
            Instance.Speedy_Recovery_Chance = 50.0f;
            Instance.Successful_Deeds_Chance = 50.0f;
            Instance.Supernatural_Encounter_Chance = 10.0f;
            Instance.Target_Practice_Chance = 50.0f;
            Instance.Travelling_Merchant_Chance = 30.0f;
            Instance.Travellers_Chance = 50.0f;
            Instance.Unexpected_Wedding_Chance = 50.0f;
            Instance.Undercooked_Chance = 50.0f;
            Instance.Violated_Girl_Chance = 50.0f;
            Instance.Sudden_Storm_Chance = 50.0f;
            Instance.Wandering_Livestock_Chance = 50.0f;
        }
        

        public void Dispose()
        {
            //NA
        }
    }
}