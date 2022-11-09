using System;
using MCM.Abstractions.Base.Global;
using MCM.Abstractions.FluentBuilder;
using MCM.Common;
using TaleWorlds.Localization;

namespace CryingBuffalo.RandomEvents.Settings
{
    public class MenuConfig : IDisposable
    {
        private static MenuConfig _instance;

        private FluentGlobalSettings globalSettings;

        public bool GS_FirstRunDone { get; set; }
        public bool GS_DebugMode { get; private set; }
        public int GS_MinimumInGameHours { get; private set; }
        public int GS_MinimumRealMinutes { get; private set; }
        public int GS_MaximumRealMinutes { get; private set; }
        
        //AheadOfTime
        public bool AoT_Disable { get; private set; }
        
        //BanditAmbush
        public bool BA_Disable { get; private set; }
        public float BA_MoneyMinPercent { get; private set; }
        public float BA_MoneyMaxPercent { get; private set; }
        public int BA_TroopScareCount { get; private set; }
        public int BA_BanditCap { get; private set; }
        
        //BeeKind
        public bool BK_Disable { get; private set; }
        public int BK_damage { get; private set; }
        public float BK_Reaction_Chance { get; private set; }
        public int BK_Add_Damage { get; private set; }
        
        //BetMoney
        public bool BM_Disable { get; private set; }
        public float BM_Money_Percent { get; private set; }

        //BeggarBegging
        public bool BB_Disable { get; private set; }
        public int BB_MinGoldToBeggar { get; private set; }
        public int BB_MaxGoldToBeggar { get; private set; }
        public int BB_MinRenownGain { get; private set; }
        public int BB_MaxRenownGain { get; private set; }
        
        //BirthdayParty
        public bool BP_Disable { get; private set; }
        public int BP_MinAttending{ get; private set; }
        public int BP_MaxAttending{ get; private set; }
        public int BP_MinYourMenAttending{ get; private set; }
        public int BP_MaxYourMenAttending{ get; private set; }
        public int BP_MinAge{ get; private set; }
        public int BP_MaxAge{ get; private set; }
        public int BP_MinBandits{ get; private set; }
        public int BP_MaxBandits{ get; private set; }
        public int BP_MinGoldGiven{ get; private set; }
        public int BP_MaxGoldGiven{ get; private set; }
        public int BP_MinRenownGain{ get; private set; }
        public int BP_MaxRenownGain{ get; private set; }
        public int BP_MinGoldLooted{ get; private set; }
        public int BP_MaxGoldLooted{ get; private set; }


        public static MenuConfig Instance
        {
            get { return _instance ??= new MenuConfig(); }
        }

        public void Settings()
        {
            
        //General Settings - Strings
            var gs_heading = new TextObject("{=mcm_gs_heading}1. General Settings").ToString();
            var gs1_text = new TextObject("{=mcm_gs1_text}1. Initial Setup").ToString();
            var gs1_hint = new TextObject("{=mcm_gs1_hint}Uncheck this to re-run the Initial Setup and set all values back to the original.").ToString();
            var gs2_text = new TextObject("{=mcm_gs2_text}2. Enable Debug mode").ToString();
            var gs2_hint = new TextObject("{=mcm_gs2_hint}Displays more info in the logs.").ToString();
            var gs3_text = new TextObject("{=mcm_gs3_text}3. In-game hours before events").ToString();
            var gs3_hint = new TextObject("{=mcm_gs3_hint}Minimum amount of in-game hours that must pass before the random events begin to occur.").ToString();
            var gs4_text = new TextObject("{=mcm_gs4_text}4. Min minutes between events").ToString();
            var gs4_hint = new TextObject("{=mcm_gs4_hint}Minimum amount of minutes in between events.").ToString();
            var gs5_text = new TextObject("{=mcm_gs5_text}5. Max minutes between events").ToString();
            var gs5_hint = new TextObject("{=mcm_gs5_hint}Maximum amount of minutes in between events.").ToString();
            
            //Ahead of Time - Strings
            var aot_heading = new TextObject("{=mcm_aot_heading}2. Ahead of Time").ToString();
            var aot1_text = new TextObject("{=mcm_aot1_text}1. Deactivate event").ToString();
            var aot1_hint = new TextObject("{=mcm_aot1_hint}If you dont want this event to show up you can deactivate it.").ToString();
            
            //Bandit Ambush - Strings
            var ba_heading = new TextObject("{=mcm_ba_heading}3. Bandit Ambush").ToString();
            var ba1_text = new TextObject("{=mcm_ba1_text}1. Min % gold loss").ToString();
            var ba1_hint = new TextObject("{=mcm_ba1_hint}The minimum amount of gold loss that can occur from this event.").ToString();
            var ba2_text = new TextObject("{=mcm_ba2_text}2. Max % gold loss").ToString();
            var ba2_hint = new TextObject("{=mcm_ba2_hint}The maximum amount of gold loss that can occur from this event.").ToString();
            var ba3_text = new TextObject("{=mcm_ba3_text}3. Troop scare count").ToString();
            var ba3_hint = new TextObject("{=mcm_ba3_hint}How many troops are need to scare the bandits.").ToString();
            var ba4_text = new TextObject("{=mcm_ba4_text}4. Max amount of bandits").ToString();
            var ba4_hint = new TextObject("{=mcm_ba4_hint}Maximum amount of bandits to spawn.").ToString();
            var ba5_text = new TextObject("{=mcm_ba5_text}5. Deactivate event").ToString();
            var ba5_hint = new TextObject("{=mcm_ba5_hint}If you dont want this event to show up you can deactivate it.").ToString();
            
            //Bee Kind - Strings
            var bk_heading = new TextObject("{=mcm_bk_heading}4. Bee Kind").ToString();
            var bk1_text = new TextObject("{=mcm_bk1_text}1. Damage to inflict").ToString();
            var bk1_hint = new TextObject("{=mcm_bk1_hint}The amount of damage the player gets.").ToString();
            var bk2_text = new TextObject("{=mcm_bk2_text}2. Reaction chance").ToString();
            var bk2_hint = new TextObject("{=mcm_bk2_hint}The chance in % that the player manages to react.").ToString();
            var bk3_text = new TextObject("{=mcm_bk3_text}3. Additional damage").ToString();
            var bk3_hint = new TextObject("{=mcm_bk3_hint}The amount of additional damage the player gets if they fail to react.").ToString();
            var bk4_text = new TextObject("{=mcm_bk4_text}4. Deactivate event").ToString();
            var bk4_hint = new TextObject("{=mcm_bk4_hint}If you dont want this event to show up you can deactivate it.").ToString();
            
            //Bet Money - Strings
            var bm_heading = new TextObject("{=mcm_bm_heading}5. Bet Money").ToString();
            var bm1_text = new TextObject("{=mcm_bm1_text}1. Percent of money to bet").ToString();
            var bm1_hint = new TextObject("{=mcm_bm1_hint}The amount of money in percent to bet.").ToString();
            var bm2_text = new TextObject("{=mcm_bm2_text}2. Deactivate event").ToString();
            var bm2_hint = new TextObject("{=mcm_bm2_hint}If you dont want this event to show up you can deactivate it.").ToString();
            
            //Beggar Begging - Strings
            var bb_heading = new TextObject("{=mcm_bb_heading}6. Beggar Begging").ToString();
            var bb1_text = new TextObject("{=mcm_bb1_text}1. Min gold to give").ToString();
            var bb1_hint = new TextObject("{=mcm_bb1_hint}Minimum amount of gold to give to the beggar.").ToString();
            var bb2_text = new TextObject("{=mcm_bb2_text}2. Max gold to give").ToString();
            var bb2_hint = new TextObject("{=mcm_bb2_hint}Maximum amount of gold to give to the beggar.").ToString();
            var bb3_text = new TextObject("{=mcm_bb3_text}3. Min renown gained").ToString();
            var bb3_hint = new TextObject("{=mcm_bb3_hint}Minimum amount of renown to gain.").ToString();
            var bb4_text = new TextObject("{=mcm_bb4_text}4. Max renown gained.").ToString();
            var bb4_hint = new TextObject("{=mcm_bb4_hint}Maximum amount of renown to gain.").ToString();
            var bb5_text = new TextObject("{=mcm_bb5_text}5. Deactivate event").ToString();
            var bb5_hint = new TextObject("{=mcm_bb5_hint}If you dont want this event to show up you can deactivate it.").ToString();
            
            //Birthday Party - Strings
            var bp_heading = new TextObject("{=mcm_bp_heading}7. Birthday Party").ToString();
            var bp1_text = new TextObject("{=mcm_bp1_text}1. Min Attending").ToString();
            var bp1_hint = new TextObject("{=mcm_bp1_hint}Minimum amount of guests attending.").ToString();
            var bp2_text = new TextObject("{=mcm_bp2_text}2. Max Attending").ToString();
            var bp2_hint = new TextObject("{=mcm_bp2_hint}Maximum amount of guests attending.").ToString();
            var bp3_text = new TextObject("{=mcm_bp3_text}3. Min your men").ToString();
            var bp3_hint = new TextObject("{=mcm_bp3_hint}Minimum amount of your men attending.").ToString();
            var bp4_text = new TextObject("{=mcm_bp4_text}4. Max your men.").ToString();
            var bp4_hint = new TextObject("{=mcm_bp4_hint}Maximum amount of your men attending.").ToString();
            var bp5_text = new TextObject("{=mcm_bp5_text}5. Min Age").ToString();
            var bp5_hint = new TextObject("{=mcm_bp5_hint}The girls minimum age.").ToString();
            var bp6_text = new TextObject("{=mcm_bp6_text}6. Max Age").ToString();
            var bp6_hint = new TextObject("{=mcm_bp6_hint}The girls maximum age.").ToString();
            var bp7_text = new TextObject("{=mcm_bp7_text}7. Min Bandits").ToString();
            var bp7_hint = new TextObject("{=mcm_bp7_hint}Minimum amount of bandits that can appear.").ToString();
            var bp8_text = new TextObject("{=mcm_bp8_text}8. Max Bandits").ToString();
            var bp8_hint = new TextObject("{=mcm_bp8_hint}Maximum amount of bandits that can appear.").ToString();
            var bp9_text = new TextObject("{=mcm_bp9_text}9. Min gold given").ToString();
            var bp9_hint = new TextObject("{=mcm_bp9_hint}Minimum amount of gold given to the girl.").ToString();
            var bp10_text = new TextObject("{=mcm_bp10_text}10. Max gold given").ToString();
            var bp10_hint = new TextObject("{=mcm_bp10_hint}Maximum amount of gold given to the girl.").ToString();
            var bp11_text = new TextObject("{=mcm_bp11_text}11. Min renown gain").ToString();
            var bp11_hint = new TextObject("{=mcm_bp11_hint}Minimum amount of renown you can gain.").ToString();
            var bp12_text = new TextObject("{=mcm_bp12_text}12. Max renown gain").ToString();
            var bp12_hint = new TextObject("{=mcm_bp12_hint}Maximum amount of renown you can gain.").ToString();
            var bp13_text = new TextObject("{=mcm_bp13_text}13. Min gold to loot").ToString();
            var bp13_hint = new TextObject("{=mcm_bp13_hint}Minimum amount of gold you can loot from the party.").ToString();
            var bp14_text = new TextObject("{=mcm_bp14_text}14. Max gold to loot").ToString();
            var bp14_hint = new TextObject("{=mcm_bp14_hint}Minimum amount of gold you can loot from the party.").ToString();
            var bp15_text = new TextObject("{=mcm_bp15_text}15. Deactivate event").ToString();
            var bp15_hint = new TextObject("{=mcm_bp15_hint}If you dont want this event to show up you can deactivate it.").ToString();
            
            
            
            var builder = BaseSettingsBuilder.Create("RandomEvents","Random Events")!
                .SetFormat("xml")
                .SetFolderName(RandomEventsSubmodule.FolderName)
                .SetSubFolder(RandomEventsSubmodule.ModName)
                
                //General Settings
                .CreateGroup(gs_heading, groupBuilder => groupBuilder
                    .AddBool("GS1", gs1_text, new ProxyRef<bool>(() => GS_FirstRunDone, o => GS_FirstRunDone = o), boolBuilder => boolBuilder
                        .SetHintText(gs1_hint)
                        .SetRequireRestart(true))
                    .AddBool("GS2", gs2_text, new ProxyRef<bool>(() => GS_DebugMode, o => GS_DebugMode = o), boolBuilder => boolBuilder
                        .SetHintText(gs2_hint))
                    .AddInteger("GS3", gs3_text,5,360, new ProxyRef<int>(() => GS_MinimumInGameHours, o => GS_MinimumInGameHours = o), integerBuilder => integerBuilder
                        .SetHintText(gs3_hint))
                    .AddInteger("GS4", gs4_text,5,60, new ProxyRef<int>(() => GS_MinimumRealMinutes, o => GS_MinimumRealMinutes = o), integerBuilder => integerBuilder
                        .SetHintText(gs4_hint))
                    .AddInteger("GS5", gs5_text,5,60, new ProxyRef<int>(() => GS_MaximumRealMinutes, o => GS_MaximumRealMinutes = o), integerBuilder => integerBuilder
                        .SetHintText(gs5_hint)))
                
                //Event - Ahead Of Time
                .CreateGroup(aot_heading, groupBuilder => groupBuilder
                    .AddBool("AoT1", aot1_text, new ProxyRef<bool>(() => AoT_Disable, o => AoT_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(aot1_hint))
                    )
                
                //Event - Bandit Ambush
                .CreateGroup(ba_heading, groupBuilder => groupBuilder
                    .AddFloatingInteger("BA1", ba1_text,3,25, new ProxyRef<float>(() => BA_MoneyMinPercent, o => BA_MoneyMinPercent = o), floatBuilder => floatBuilder
                        .SetHintText(ba1_hint))
                    .AddFloatingInteger("BA2", ba2_text,3,25, new ProxyRef<float>(() => BA_MoneyMaxPercent, o => BA_MoneyMaxPercent = o), floatBuilder => floatBuilder
                        .SetHintText(ba2_hint))
                    .AddInteger("BA3", ba3_text,25,100, new ProxyRef<int>(() => BA_TroopScareCount, o => BA_TroopScareCount = o), integerBuilder => integerBuilder
                        .SetHintText(ba3_hint))
                    .AddInteger("BA4", ba4_text,5,25, new ProxyRef<int>(() => BA_BanditCap, o => BA_BanditCap = o), integerBuilder => integerBuilder
                        .SetHintText(ba4_hint))
                    .AddBool("BA5", ba5_text, new ProxyRef<bool>(() => BA_Disable, o => BA_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(ba5_hint))
                )
                
                //Event - Bee Kind
                .CreateGroup(bk_heading, groupBuilder => groupBuilder
                    .AddInteger("BK1", bk1_text,5,50, new ProxyRef<int>(() => BK_damage, o => BK_damage = o), floatBuilder => floatBuilder
                        .SetHintText(bk1_hint))
                    .AddFloatingInteger("BK2", bk2_text,3,25, new ProxyRef<float>(() => BK_Reaction_Chance, o => BK_Reaction_Chance = o), floatBuilder => floatBuilder
                        .SetHintText(bk2_hint))
                    .AddInteger("BK3", bk3_text,10,25, new ProxyRef<int>(() => BK_Add_Damage, o => BK_Add_Damage = o), integerBuilder => integerBuilder
                        .SetHintText(bk3_hint))
                    .AddBool("BK4", bk4_text, new ProxyRef<bool>(() => BK_Disable, o => BK_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(bk4_hint))
                )
                
                //Event - Bet Money
                .CreateGroup(bm_heading, groupBuilder => groupBuilder
                    .AddFloatingInteger("BM1", bm1_text,1,90, new ProxyRef<float>(() => BM_Money_Percent, o => BM_Money_Percent = o), floatBuilder => floatBuilder
                        .SetHintText(bm1_hint))
                    .AddBool("BM2", bm2_text, new ProxyRef<bool>(() => BM_Disable, o => BM_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(bm2_hint))
                )
            
                //Event - Birthday Party
                .CreateGroup(bp_heading, groupBuilder => groupBuilder
                .AddInteger("BP1", bp1_text,10,25, new ProxyRef<int>(() => BP_MinAttending, o => BP_MinAttending = o), integerBuilder => integerBuilder
                    .SetHintText(bp1_hint))
                .AddInteger("BP2", bp2_text,10,25, new ProxyRef<int>(() => BP_MaxAttending, o => BP_MaxAttending = o), integerBuilder => integerBuilder
                    .SetHintText(bp2_hint))
                .AddInteger("BP3", bp3_text,2,20, new ProxyRef<int>(() => BP_MinYourMenAttending, o => BP_MinYourMenAttending = o), integerBuilder => integerBuilder
                    .SetHintText(bp3_hint))
                .AddInteger("BP4", bp4_text,2,20, new ProxyRef<int>(() => BP_MaxYourMenAttending, o => BP_MaxYourMenAttending = o), integerBuilder => integerBuilder
                    .SetHintText(bp4_hint))
                .AddInteger("BP5", bp5_text,14,25, new ProxyRef<int>(() => BP_MinAge, o => BP_MinAge = o), integerBuilder => integerBuilder
                    .SetHintText(bp5_hint))
                .AddInteger("BP6", bp6_text,14,25, new ProxyRef<int>(() => BP_MaxAge, o => BP_MaxAge = o), integerBuilder => integerBuilder
                    .SetHintText(bp6_hint))
                .AddInteger("BP7", bp7_text,3,20, new ProxyRef<int>(() => BP_MinBandits, o => BP_MinBandits = o), integerBuilder => integerBuilder
                    .SetHintText(bp7_hint))
                .AddInteger("BP8", bp8_text,3,20, new ProxyRef<int>(() => BP_MaxBandits, o => BP_MaxBandits = o), integerBuilder => integerBuilder
                    .SetHintText(bp8_hint))
                .AddInteger("BP9", bp9_text,50,200, new ProxyRef<int>(() => BP_MinGoldGiven, o => BP_MinGoldGiven = o), integerBuilder => integerBuilder
                    .SetHintText(bp9_hint))
                .AddInteger("BP10", bp10_text,50,200, new ProxyRef<int>(() => BP_MaxGoldGiven, o => BP_MaxGoldGiven = o), integerBuilder => integerBuilder
                    .SetHintText(bp10_hint))
                .AddInteger("BP11", bp11_text,10,30, new ProxyRef<int>(() => BP_MinRenownGain, o => BP_MinRenownGain = o), integerBuilder => integerBuilder
                    .SetHintText(bp11_hint))
                .AddInteger("BP12", bp12_text,10,30, new ProxyRef<int>(() => BP_MaxRenownGain, o => BP_MaxRenownGain = o), integerBuilder => integerBuilder
                    .SetHintText(bp12_hint))
                .AddInteger("BP13", bp13_text,250,6500, new ProxyRef<int>(() => BP_MinGoldLooted, o => BP_MinGoldLooted = o), integerBuilder => integerBuilder
                    .SetHintText(bp13_hint))
                .AddInteger("BP14", bp14_text,250,6500, new ProxyRef<int>(() => BP_MaxGoldLooted, o => BP_MaxGoldLooted = o), integerBuilder => integerBuilder
                    .SetHintText(bp14_hint))
                .AddBool("BP15", bp15_text, new ProxyRef<bool>(() => BP_Disable, o => BP_Disable = o), boolBuilder => boolBuilder
                    .SetHintText(bp15_hint))
                );



            globalSettings = builder.BuildAsGlobal();
            globalSettings.Register();

            if (!GS_FirstRunDone)
            {
                Perform_First_Time_Setup();
            }
        }

        private static void Perform_First_Time_Setup()
        {
            Instance.GS_FirstRunDone = true;
            Instance.GS_DebugMode = false;
            Instance.GS_MinimumInGameHours = 24;
            Instance.GS_MinimumRealMinutes = 5;
            Instance.GS_MaximumRealMinutes = 30;
            
            //AheadOfTime 
            Instance.AoT_Disable = false;
            
            //BanditAmbush
            Instance.BA_Disable = false;
            Instance.BA_MoneyMinPercent = 0.05f;
            Instance.BA_MoneyMaxPercent = 0.15f;
            Instance.BA_TroopScareCount = 25;
            Instance.BA_BanditCap = 50;
            
            //BeeKind
            Instance.BK_Disable = false;
            Instance.BK_damage = 10;
            Instance.BK_Reaction_Chance = 0.33f;
            Instance.BK_Add_Damage = 15;
            
            //BetMoney 
            Instance.BM_Disable = false;
            Instance.BM_Money_Percent = 0.15f;
            
            //BeggarBegging
            Instance.BB_MinGoldToBeggar = 25;
            Instance.BB_MaxGoldToBeggar = 75;
            Instance.BB_MinRenownGain = 10;
            Instance.BB_MaxRenownGain = 20;
            Instance.BB_Disable = false;
            
            //BirthdayParty
            Instance.BP_Disable = false;
            Instance.BP_MinAttending = 20;
            Instance.BP_MaxAttending = 60;
            Instance.BP_MinYourMenAttending = 3;
            Instance.BP_MaxYourMenAttending = 12;
            Instance.BP_MinAge = 14;
            Instance.BP_MaxAge = 22;
            Instance.BP_MinBandits = 5;
            Instance.BP_MaxBandits = 10;
            Instance.BP_MinGoldGiven = 50;
            Instance.BP_MaxGoldGiven = 200;
            Instance.BP_MinRenownGain = 15;
            Instance.BP_MaxRenownGain = 30;
            Instance.BP_MinGoldLooted = 500;
            Instance.BP_MaxGoldLooted = 1500;
        }

        public void Dispose()
        {
            //NA
        }
    }
}