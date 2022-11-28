using System;
using MCM.Abstractions.Base.Global;
using MCM.Abstractions.FluentBuilder;
using MCM.Common;
using TaleWorlds.Localization;

namespace CryingBuffalo.RandomEvents.Settings.MCM
{
    public class MCM_MenuConfig_G_O : IDisposable
    {
        private static MCM_MenuConfig_G_O _instance;

        private FluentGlobalSettings globalSettings;

        #region Variables

        #region Granary Rats - Variables
        
        public bool GR_Disable { get; private set; }
        public float GR_MinFoodLoss { get; private set; }
        public float GR_MaxFoodLoss { get; private set; }
        
        #endregion
        
        #region Hot Springs - Variables
        
        public bool HS_Disable { get; private set; }
        public int HS_MinMoraleGain { get; private set; }
        public int HS_MaxMoraleGain { get; private set; }
        
        #endregion

        #region Hunting Trip - Variables

        public bool HT_Disable { get; private set; }
        public int HT_MinSoldiersToGo { get; private set; }
        public int HT_MaxSoldiersToGo { get; private set; }
        public int HT_MaxCatch { get; private set; }
        public int HT_MinMoraleGain { get; private set; }
        public int HT_MaxMoraleGain { get; private set; }
        public int HT_MinYieldMultiplier { get; private set; }
        public int HT_MaxYieldMultiplier { get; private set; }
        

        #endregion
        
        #region Lights in the Skies - Variables

        public bool LitS_Disable { get; private set; }

        #endregion
        
        #region Logging Site - Variables

        public bool LS_Disable { get; private set; }
        public int LS_MinSoldiersToGo { get; private set; }
        public int LS_MaxSoldiersToGo { get; private set; }
        public int LS_MinYield { get; private set; }
        public int LS_MaxYield { get; private set; }
        public int LS_MinYieldMultiplier { get; private set; }
        public int LS_MaxYieldMultiplier { get; private set; }
        

        #endregion
        
        #region Look Up - Variables

        public bool LU_Disable { get; private set; }
        public float LU_TreeShakeChance { get; private set; }
        public float LU_BaseRangeChance { get; private set; }
        public int LU_MinRangeLevel { get; private set; }
        public int LU_MaxRangeLevel { get; private set; }
        public int LU_MinGold { get; private set; }
        public int LU_MaxGold { get; private set; }
        

        #endregion
        
        #region Mass Grave - Variables

        public bool MG_Disable { get; private set; }
        public int MG_MinSoldiers{ get; private set; }
        public int MG_MaxSoldiers { get; private set; }
        public int MG_MinBodies { get; private set; }
        public int MG_MaxBodies { get; private set; }
        public int MG_MinBaseMoraleLoss { get; private set; }
        public int MG_MaxBaseMoraleLoss { get; private set; }
        

        #endregion
        
        #region Momentum - Variables

        public bool MO_Disable { get; private set; }

        #endregion
        
        #region Not Of This World - Variables

        public bool NotW_Disable { get; private set; }
        public int NotW_MinSoldiersGone{ get; private set; }
        public int NotW_MaxSoldiersGone { get; private set; }
        
        #endregion
        
        #region Old Ruins - Variables

        public bool OR_Disable { get; private set; }
        public int OR_MinSoldiers { get; private set; }
        public int OR_MaxSoldiers { get; private set; }
        public float OR_MenToKill { get; private set; }
        public int OR_MinGoldFound { get; private set; }
        public int OR_MaxGoldFound { get; private set; }
        
        #endregion

        #endregion


        public static MCM_MenuConfig_G_O Instance
        {
            get { return _instance ??= new MCM_MenuConfig_G_O(); }
        }

        public void Settings()
        {


            #region Strings
            
            #region Granary Rats - Strings
            
            var gr_heading = new TextObject("{=mcm_gr_heading}Granary Rats").ToString();
            var gr1_text = new TextObject("{=mcm_gr1_text}1. Min Food loss percent").ToString();
            var gr1_hint = new TextObject("{=mcm_gr1_hint}Minimum amount of food lost during this event.").ToString();
            var gr2_text = new TextObject("{=mcm_gr2_text}2. Max Food loss percent").ToString();
            var gr2_hint = new TextObject("{=mcm_gr2_hint}Maximum amount of food lost during this event.").ToString();
            var gr3_text = new TextObject("{=mcm_gr3_text}3. Deactivate event").ToString();
            var gr3_hint = new TextObject("{=mcm_gr3_hint}If you dont want this event to show up you can deactivate it.").ToString();
            
            #endregion
            
            #region Hot Springs - Strings
            
            var hs_heading = new TextObject("{=mcm_hs_heading}Hot Springs").ToString();
            var hs1_text = new TextObject("{=mcm_hs1_text}1. Min Morale Gain").ToString();
            var hs1_hint = new TextObject("{=mcm_hs1_hint}Minimum amount of morale gained during this event.").ToString();
            var hs2_text = new TextObject("{=mcm_hs2_text}2. Max Morale Gain").ToString();
            var hs2_hint = new TextObject("{=mcm_hs2_hint}Maximum amount morale gained lost during this event.").ToString();
            var hs3_text = new TextObject("{=mcm_hs3_text}3. Deactivate event").ToString();
            var hs3_hint = new TextObject("{=mcm_hs3_hint}If you dont want this event to show up you can deactivate it.").ToString();
            
            #endregion
            
            #region Hunting Trip - Strings

            var ht_heading = new TextObject("{=mcm_ht_heading}Hunting Trip").ToString();
            var ht1_text = new TextObject("{=mcm_ht1_text}1. Min Soldiers").ToString();
            var ht1_hint = new TextObject("{=mcm_ht1_hint}Minimum soldiers to go hunting.").ToString();
            var ht2_text = new TextObject("{=mcm_ht2_text}2. Max Soldiers").ToString();
            var ht2_hint = new TextObject("{=mcm_ht2_hint}Maximum soldiers to go hunting.").ToString();
            var ht3_text = new TextObject("{=mcm_ht3_text}3. Max Catch").ToString();
            var ht3_hint = new TextObject("{=mcm_ht3_hint}Minimum amount of prey to catch.").ToString();
            var ht4_text = new TextObject("{=mcm_ht4_text}4. Min Morale Gained").ToString();
            var ht4_hint = new TextObject("{=mcm_ht4_hint}Minimum amount of morale gained.").ToString();
            var ht5_text = new TextObject("{=mcm_ht5_text}5. Max Morale Gained").ToString();
            var ht5_hint = new TextObject("{=mcm_ht5_hint}Maximum amount of morale gained.").ToString();
            var ht6_text = new TextObject("{=mcm_ht6_text}6. Min Yield Multiplier").ToString();
            var ht6_hint = new TextObject("{=mcm_ht6_hint}The amount of prey you catch is multiplied by this to get meat resources.").ToString();
            var ht7_text = new TextObject("{=mcm_ht7_text}7. Max Yield Multiplier").ToString();
            var ht7_hint = new TextObject("{=mcm_ht7_hint}The amount of prey you catch is multiplied by this to get meat resources.").ToString();
            var ht8_text = new TextObject("{=mcm_ht8_text}8. Deactivate event").ToString();
            var ht8_hint = new TextObject("{=mcm_ht8_hint}If you dont want this event to show up you can deactivate it.").ToString();

            #endregion
            
            #region Lights in the Sky - Strings
            
            var lits_heading = new TextObject("{=mcm_lits_heading}Lights in the Skies").ToString();
            var lits1_text = new TextObject("{=mcm_lits1_text}1. Deactivate event").ToString();
            var lits1_hint = new TextObject("{=mcm_lits1_hint}If you dont want this event to show up you can deactivate it.").ToString();
            
            #endregion
            
            #region Logging Site - Strings

            var ls_heading = new TextObject("{=mcm_ls_heading}Logging Site").ToString();
            var ls1_text = new TextObject("{=mcm_ls1_text}1. Min Soldiers").ToString();
            var ls1_hint = new TextObject("{=mcm_ls1_hint}Minimum soldiers to go logging.").ToString();
            var ls2_text = new TextObject("{=mcm_ls2_text}2. Max Soldiers").ToString();
            var ls2_hint = new TextObject("{=mcm_ls2_hint}Maximum soldiers to go logging.").ToString();
            var ls3_text = new TextObject("{=mcm_ls3_text}3. Min Yield").ToString();
            var ls3_hint = new TextObject("{=mcm_ls3_hint}Minimum amount of trees to chop.").ToString();
            var ls4_text = new TextObject("{=mcm_ls4_text}4. Max Yield").ToString();
            var ls4_hint = new TextObject("{=mcm_ls4_hint}Maximum amount of trees to chop").ToString();
            var ls5_text = new TextObject("{=mcm_ls5_text}5. Min Yield Multiplier").ToString();
            var ls5_hint = new TextObject("{=mcm_ls5_hint}The amount of trees you chop is multiplied by this to get hardwood resources.").ToString();
            var ls6_text = new TextObject("{=mcm_ls6_text}6. Max Yield Multiplier").ToString();
            var ls6_hint = new TextObject("{=mcm_ls6_hint}The amount of trees you chop is multiplied by this to get hardwood resources.").ToString();
            var ls7_text = new TextObject("{=mcm_ls7_text}7. Deactivate event").ToString();
            var ls7_hint = new TextObject("{=mcm_ls7_hint}If you dont want this event to show up you can deactivate it.").ToString();

            #endregion
            
            #region Look Up - Strings

            var lu_heading = new TextObject("{=mcm_lu_heading}Look Up").ToString();
            var lu1_text = new TextObject("{=mcm_lu1_text}1. Tree Shake Chance").ToString();
            var lu1_hint = new TextObject("{=mcm_lu1_hint}The chance player will successfully shake the gold out of the tree.").ToString();
            var lu2_text = new TextObject("{=mcm_lu2_text}2. Base Range Chance").ToString();
            var lu2_hint = new TextObject("{=mcm_lu2_hint}The chance player will be able to get gold out of the tree with ranged weapon at minimum skill level").ToString();
            var lu3_text = new TextObject("{=mcm_lu3_text}3. Min Ranged Level").ToString();
            var lu3_hint = new TextObject("{=mcm_lu3_hint}Below, the player will always miss.").ToString();
            var lu4_text = new TextObject("{=mcm_lu4_text}4. Max Ranged Level").ToString();
            var lu4_hint = new TextObject("{=mcm_lu4_hint}At or above, the player will always succeed.").ToString();
            var lu5_text = new TextObject("{=mcm_lu5_text}5. Min Gold Gained").ToString();
            var lu5_hint = new TextObject("{=mcm_lu5_hint}Minimum amount of gold gained from this event.").ToString();
            var lu6_text = new TextObject("{=mcm_lu6_text}6. Max Gold Gained").ToString();
            var lu6_hint = new TextObject("{=mcm_lu6_hint}Maximum amount of gold gained from this event").ToString();
            var lu7_text = new TextObject("{=mcm_lu7_text}7. Deactivate event").ToString();
            var lu7_hint = new TextObject("{=mcm_lu7_hint}If you dont want this event to show up you can deactivate it.").ToString();

            #endregion
            
            #region Mass Grave - Strings

            var mg_heading = new TextObject("{=mcm_mg_heading}Mass Grave").ToString();
            var mg1_text = new TextObject("{=mcm_mg1_text}1. Min Soldiers").ToString();
            var mg1_hint = new TextObject("{=mcm_mg1_hint}The minimum amount of men who will make the discovery.").ToString();
            var mg2_text = new TextObject("{=mcm_mg2_text}2. Max Soldiers").ToString();
            var mg2_hint = new TextObject("{=mcm_mg2_hint}The maximum amount of men who will make the discovery.").ToString();
            var mg3_text = new TextObject("{=mcm_mg3_text}3. Min Bodies").ToString();
            var mg3_hint = new TextObject("{=mcm_mg3_hint}The minimum amount of bodies in the mass grave.").ToString();
            var mg4_text = new TextObject("{=mcm_mg4_text}4. Max Bodies").ToString();
            var mg4_hint = new TextObject("{=mcm_mg4_hint}The maximum amount of bodies in the mass grave.").ToString();
            var mg5_text = new TextObject("{=mcm_mg5_text}5. Min Base Morale Loss").ToString();
            var mg5_hint = new TextObject("{=mcm_mg5_hint}Minimum amount of morale that can be lost.").ToString();
            var mg6_text = new TextObject("{=mcm_mg6_text}6. Max Base Morale Loss").ToString();
            var mg6_hint = new TextObject("{=mcm_mg6_hint}Maximum amount of morale that can be lost.").ToString();
            var mg7_text = new TextObject("{=mcm_mg7_text}7. Deactivate event").ToString();
            var mg7_hint = new TextObject("{=mcm_mg7_hint}If you dont want this event to show up you can deactivate it.").ToString();

            #endregion
            
            #region Momentum - Strings

            var mo_heading = new TextObject("{=mcm_mo_heading}Momentum").ToString();
            var mo1_text = new TextObject("{=mcm_mo1_text}1. Deactivate event").ToString();
            var mo1_hint = new TextObject("{=mcm_mo1_hint}If you dont want this event to show up you can deactivate it.").ToString();

            #endregion
            
            #region Not Of This World - Strings

            var notw_heading = new TextObject("{=mcm_notw_heading}Not of this World").ToString();
            var notw1_text = new TextObject("{=mcm_notw1_text}1. Min Soldiers To Disappear").ToString();
            var notw1_hint = new TextObject("{=mcm_notw1_hint}The minimum amount of men who will disappear.").ToString();
            var notw2_text = new TextObject("{=mcm_notw2_text}2. Max Soldiers To Disappear").ToString();
            var notw2_hint = new TextObject("{=mcm_notw2_hint}The maximum amount of men who will disappear.").ToString();
            var notw3_text = new TextObject("{=mcm_notw3_text}3. Deactivate event").ToString();
            var notw3_hint = new TextObject("{=mcm_notw3_hint}If you dont want this event to show up you can deactivate it.").ToString();

            #endregion
            
            #region Old Ruins - Strings

            var or_heading = new TextObject("{=mcm_or_heading}Old Ruins").ToString();
            var or1_text = new TextObject("{=mcm_or1_text}1. Min Soldiers").ToString();
            var or1_hint = new TextObject("{=mcm_or1_hint}The minimum amount of men who come with you.").ToString();
            var or2_text = new TextObject("{=mcm_or2_text}2. Max Soldiers").ToString();
            var or2_hint = new TextObject("{=mcm_or2_hint}The maximum amount of men who come with you.").ToString();
            var or3_text = new TextObject("{=mcm_or3_text}3. Men Who Will Die").ToString();
            var or3_hint = new TextObject("{=mcm_or3_hint}The % of men who will die in this event if the circumstances align.").ToString();
            var or4_text = new TextObject("{=mcm_or4_text}4. Min Gold Found").ToString();
            var or4_hint = new TextObject("{=mcm_or4_hint}The minimum amount of gold to be found.").ToString();
            var or5_text = new TextObject("{=mcm_or5_text}5. Max Gold Found").ToString();
            var or5_hint = new TextObject("{=mcm_or5_hint}The maximum amount of gold to be found.").ToString();
            var or6_text = new TextObject("{=mcm_or6_text}6. Deactivate event").ToString();
            var or6_hint = new TextObject("{=mcm_or6_hint}If you dont want this event to show up you can deactivate it.").ToString();

            #endregion

            #endregion
            
            
            
            var builder = BaseSettingsBuilder.Create("RandomEvents2","2. Random Events - G to O")!
                .SetFormat("xml")
                .SetFolderName(RandomEventsSubmodule.FolderName)
                .SetSubFolder(RandomEventsSubmodule.ModName)
                
           #region Builder Modules
                
                #region Granary Rats - Builder
                
                .CreateGroup(gr_heading, groupBuilder => groupBuilder
                    .AddFloatingInteger("GR1", gr1_text,0.05f,0.90f, new ProxyRef<float>(() => GR_MinFoodLoss, o => GR_MinFoodLoss = o), floatBuilder => floatBuilder
                        .SetHintText(gr1_hint))
                    .AddFloatingInteger("GR2", gr2_text,0.05f,0.90f, new ProxyRef<float>(() => GR_MaxFoodLoss, o => GR_MaxFoodLoss = o), floatBuilder => floatBuilder
                        .SetHintText(gr2_hint))
                    .AddBool("GR3", gr3_text, new ProxyRef<bool>(() => GR_Disable, o => GR_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(gr3_hint))
                    )
                
                
                #endregion
                
                #region Hot Springs - Builder
                
                .CreateGroup(hs_heading, groupBuilder => groupBuilder
                        .AddInteger("HS1", hs1_text,5,30, new ProxyRef<int>(() => HS_MinMoraleGain, o => HS_MinMoraleGain = o), integerBuilder => integerBuilder
                            .SetHintText(hs1_hint))
                        .AddInteger("HS2", hs2_text,5,30, new ProxyRef<int>(() => HS_MaxMoraleGain, o => HS_MaxMoraleGain = o), integerBuilder => integerBuilder
                            .SetHintText(hs2_hint))
                        .AddBool("HS3", hs3_text, new ProxyRef<bool>(() => HS_Disable, o => HS_Disable = o), boolBuilder => boolBuilder
                            .SetHintText(hs3_hint))
                    )
                
                
                #endregion
                
                #region Hunting Trip - Builder
                
                .CreateGroup(ht_heading, groupBuilder => groupBuilder
                    .AddInteger("HT1", ht1_text,2,15, new ProxyRef<int>(() => HT_MinSoldiersToGo, o => HT_MinSoldiersToGo = o), integerBuilder => integerBuilder
                        .SetHintText(ht1_hint))
                    .AddInteger("HT2", ht2_text,2,15, new ProxyRef<int>(() => HT_MaxSoldiersToGo, o => HT_MaxSoldiersToGo = o), integerBuilder => integerBuilder
                        .SetHintText(ht2_hint))
                    .AddInteger("HT3", ht3_text,10,30, new ProxyRef<int>(() => HT_MaxCatch, o => HT_MaxCatch = o), integerBuilder => integerBuilder
                        .SetHintText(ht3_hint))
                    .AddInteger("HT4", ht4_text,5,30, new ProxyRef<int>(() => HT_MinMoraleGain, o => HT_MinMoraleGain = o), integerBuilder => integerBuilder
                        .SetHintText(ht4_hint))
                    .AddInteger("HT5", ht5_text,5,30, new ProxyRef<int>(() => HT_MaxMoraleGain, o => HT_MaxMoraleGain = o), integerBuilder => integerBuilder
                        .SetHintText(ht5_hint))
                    .AddInteger("HT6", ht6_text,2,7, new ProxyRef<int>(() => HT_MinYieldMultiplier, o => HT_MinYieldMultiplier = o), integerBuilder => integerBuilder
                        .SetHintText(ht6_hint))
                    .AddInteger("HT7", ht7_text,2,7, new ProxyRef<int>(() => HT_MaxYieldMultiplier, o => HT_MaxYieldMultiplier = o), integerBuilder => integerBuilder
                        .SetHintText(ht7_hint))
                    .AddBool("HT8", ht8_text, new ProxyRef<bool>(() => HT_Disable, o => HT_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(ht8_hint))
                    )

                #endregion
                
                #region Lights in the Skies - Builder
                
                .CreateGroup(lits_heading, groupBuilder => groupBuilder
                    .AddBool("LITS", lits1_text, new ProxyRef<bool>(() => LitS_Disable, o => LitS_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(lits1_hint))
                )

                #endregion
                
                #region Logging Site - Builder
                
                .CreateGroup(ls_heading, groupBuilder => groupBuilder
                        .AddInteger("LS1", ls1_text,5,25, new ProxyRef<int>(() => LS_MinSoldiersToGo, o => LS_MinSoldiersToGo = o), integerBuilder => integerBuilder
                            .SetHintText(ls1_hint))
                        .AddInteger("LS2", ls2_text,5,25, new ProxyRef<int>(() => LS_MaxSoldiersToGo, o => LS_MaxSoldiersToGo = o), integerBuilder => integerBuilder
                            .SetHintText(ls2_hint))
                        .AddInteger("LS3", ls3_text,5,15, new ProxyRef<int>(() => LS_MinYield, o => LS_MinYield = o), integerBuilder => integerBuilder
                            .SetHintText(ls3_hint))
                        .AddInteger("LS4", ls4_text,5,15, new ProxyRef<int>(() => LS_MaxYield, o => LS_MaxYield = o), integerBuilder => integerBuilder
                            .SetHintText(ls4_hint))
                        .AddInteger("LS5", ls5_text,5,15, new ProxyRef<int>(() => LS_MinYieldMultiplier, o => LS_MinYieldMultiplier = o), integerBuilder => integerBuilder
                            .SetHintText(ls5_hint))
                        .AddInteger("LS6", ls6_text,5,15, new ProxyRef<int>(() => LS_MaxYieldMultiplier, o => LS_MaxYieldMultiplier = o), integerBuilder => integerBuilder
                            .SetHintText(ls6_hint))
                        .AddBool("LS7", ls7_text, new ProxyRef<bool>(() => LS_Disable, o => LS_Disable = o), boolBuilder => boolBuilder
                            .SetHintText(ls7_hint))
                    )

                #endregion
                
                #region Look Up - Builder
                
                .CreateGroup(lu_heading, groupBuilder => groupBuilder
                        .AddFloatingInteger("LU1", lu1_text,0.1f,0.75f, new ProxyRef<float>(() => LU_TreeShakeChance, o => LU_TreeShakeChance = o), floatBuilder => floatBuilder
                            .SetHintText(lu1_hint))
                        .AddFloatingInteger("LU2", lu2_text,0.1f,0.5f, new ProxyRef<float>(() => LU_BaseRangeChance, o => LU_BaseRangeChance = o), floatBuilder => floatBuilder
                            .SetHintText(lu2_hint))
                        .AddInteger("LU3", lu3_text,5,75, new ProxyRef<int>(() => LU_MinRangeLevel, o => LU_MinRangeLevel = o), integerBuilder => integerBuilder
                            .SetHintText(lu3_hint))
                        .AddInteger("LU4", lu4_text,5,75, new ProxyRef<int>(() => LU_MaxRangeLevel, o => LU_MaxRangeLevel = o), integerBuilder => integerBuilder
                            .SetHintText(lu4_hint))
                        .AddInteger("LU5", lu5_text,250,5000, new ProxyRef<int>(() => LU_MinGold, o => LU_MinGold = o), integerBuilder => integerBuilder
                            .SetHintText(lu5_hint))
                        .AddInteger("LU6", lu6_text,250,5000, new ProxyRef<int>(() => LU_MaxGold, o => LU_MaxGold = o), integerBuilder => integerBuilder
                            .SetHintText(lu6_hint))
                        .AddBool("LU7", lu7_text, new ProxyRef<bool>(() => LU_Disable, o => LU_Disable = o), boolBuilder => boolBuilder
                            .SetHintText(lu7_hint))
                    )

                    #endregion
                    
                #region Mass Grave - Builder
                
                .CreateGroup(mg_heading, groupBuilder => groupBuilder
                    .AddInteger("MG1", mg1_text,2,10, new ProxyRef<int>(() => MG_MinSoldiers, o => MG_MinSoldiers = o), integerBuilder => integerBuilder
                        .SetHintText(mg1_hint))
                    .AddInteger("MG2", mg2_text,2,10, new ProxyRef<int>(() => MG_MaxSoldiers, o => MG_MaxSoldiers = o), integerBuilder => integerBuilder
                        .SetHintText(mg2_hint))
                    .AddInteger("MG3", mg3_text,10,60, new ProxyRef<int>(() => MG_MinBodies, o => MG_MinBodies = o), integerBuilder => integerBuilder
                        .SetHintText(mg3_hint))
                    .AddInteger("MG4", mg4_text,10,60, new ProxyRef<int>(() => MG_MaxBodies, o => MG_MaxBodies = o), integerBuilder => integerBuilder
                        .SetHintText(mg4_hint))
                    .AddInteger("MG5", mg5_text,10,30, new ProxyRef<int>(() => MG_MinBaseMoraleLoss, o => MG_MinBaseMoraleLoss = o), integerBuilder => integerBuilder
                        .SetHintText(mg5_hint))
                    .AddInteger("MG6", mg6_text,10,30, new ProxyRef<int>(() => MG_MaxBaseMoraleLoss, o => MG_MaxBaseMoraleLoss = o), integerBuilder => integerBuilder
                        .SetHintText(mg6_hint))
                    .AddBool("MG7", mg7_text, new ProxyRef<bool>(() => MG_Disable, o => MG_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(mg7_hint))
                    )

                #endregion
                
                #region Momentum - Builder
                
                .CreateGroup(mo_heading, groupBuilder => groupBuilder
                        .AddBool("MO1", mo1_text, new ProxyRef<bool>(() => MO_Disable, o => MO_Disable = o), boolBuilder => boolBuilder
                            .SetHintText(mo1_hint))
                        )

                #endregion
                    
                #region Not Of This World - Builder
                
                .CreateGroup(notw_heading, groupBuilder => groupBuilder
                    .AddInteger("NotW1", notw1_text,2,20, new ProxyRef<int>(() => NotW_MinSoldiersGone, o => NotW_MinSoldiersGone = o), integerBuilder => integerBuilder
                        .SetHintText(notw1_hint))
                    .AddInteger("NotW2", notw2_text,2,20, new ProxyRef<int>(() => NotW_MaxSoldiersGone, o => NotW_MinSoldiersGone = o), integerBuilder => integerBuilder
                        .SetHintText(notw2_hint))
                    .AddBool("NotW3", notw3_text, new ProxyRef<bool>(() => NotW_Disable, o => NotW_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(notw3_hint))
                    )

                #endregion
                
                #region Old Ruins - Builder
                
                .CreateGroup(or_heading, groupBuilder => groupBuilder
                        .AddInteger("OR1", or1_text,5,15, new ProxyRef<int>(() => OR_MinSoldiers, o => OR_MinSoldiers = o), integerBuilder => integerBuilder
                            .SetHintText(or1_hint))
                        .AddInteger("OR2", or2_text,5,15, new ProxyRef<int>(() => OR_MaxSoldiers, o => OR_MaxSoldiers = o), integerBuilder => integerBuilder
                            .SetHintText(or2_hint))
                        .AddFloatingInteger("OR3", or3_text,20,90, new ProxyRef<float>(() => OR_MenToKill, o => OR_MenToKill = o), floatBuilder => floatBuilder
                            .SetHintText(or3_hint))
                        .AddInteger("OR4", or4_text,100,5000, new ProxyRef<int>(() => OR_MinGoldFound, o => OR_MinGoldFound = o), integerBuilder => integerBuilder
                            .SetHintText(or4_hint))
                        .AddInteger("OR5", or5_text,100,5000, new ProxyRef<int>(() => OR_MaxGoldFound, o => OR_MaxGoldFound = o), integerBuilder => integerBuilder
                            .SetHintText(or5_hint))
                        .AddBool("OR6", or6_text, new ProxyRef<bool>(() => OR_Disable, o => OR_Disable = o), boolBuilder => boolBuilder
                            .SetHintText(or6_hint))

                    #endregion    

                    #endregion
                );



            globalSettings = builder.BuildAsGlobal();
            globalSettings.Register();

            if (!MCM_ConfigMenu_General.Instance.GS_FirstRunDone)
            {
                Perform_First_Time_Setup();
            }
        }

        private static void Perform_First_Time_Setup()
        {
            #region First Time Setup

            #region Granary Rats

            Instance.GR_Disable = false;
            Instance.GR_MinFoodLoss = 0.10f;
            Instance.GR_MaxFoodLoss = 0.25f;

            #endregion
            
            #region Hot Springs

            Instance.HS_Disable = false;
            Instance.HS_MinMoraleGain = 10;
            Instance.HS_MaxMoraleGain = 25;

            #endregion
            
            #region Hunting Trip
            
            Instance.HT_Disable = false;
            Instance.HT_MinSoldiersToGo = 3;
            Instance.HT_MaxSoldiersToGo = 12;
            Instance.HT_MaxCatch = 20;
            Instance.HT_MinMoraleGain = 7;
            Instance.HT_MaxMoraleGain = 20;
            Instance.HT_MinYieldMultiplier = 3;
            Instance.HT_MaxYieldMultiplier = 6;

            #endregion
            
            #region Logging Site
            
            Instance.LS_Disable = false;
            Instance.LS_MinSoldiersToGo = 10;
            Instance.LS_MaxSoldiersToGo = 20;
            Instance.LS_MinYield = 5;
            Instance.LS_MaxYield = 15;
            Instance.LS_MinYieldMultiplier = 10;
            Instance.LS_MaxYieldMultiplier = 15;

            #endregion
            
            #region Look Up
            
            Instance.LU_Disable = false;
            Instance.LU_TreeShakeChance = 0.25f;
            Instance.LU_BaseRangeChance = 0.1f;
            Instance.LU_MinRangeLevel = 10;
            Instance.LU_MaxRangeLevel = 60;
            Instance.LU_MinGold = 500;
            Instance.LU_MaxGold = 2500;

            #endregion
            
            #region Mass Grave
            
            Instance.MG_Disable = false;
            Instance.MG_MinSoldiers = 3;
            Instance.MG_MaxSoldiers = 8;
            Instance.MG_MinBodies = 20;
            Instance.MG_MaxBodies = 40;
            Instance.MG_MinBaseMoraleLoss = 15;
            Instance.MG_MaxBaseMoraleLoss = 25;

            #endregion
            
            #region Momentum
            
            Instance.MO_Disable = false;

            #endregion
            
            #region Not of this World
            
            Instance.NotW_Disable = false;
            Instance.NotW_MinSoldiersGone = 3;
            Instance.NotW_MaxSoldiersGone = 8;

            #endregion
            
            #region Old Ruins
            
            Instance.OR_Disable = false;
            Instance.OR_MinSoldiers = 6;
            Instance.OR_MaxSoldiers = 12;
            Instance.OR_MenToKill = 70.0f;
            Instance.OR_MinGoldFound = 250;
            Instance.OR_MaxGoldFound = 5000;

            #endregion
            
            #endregion
        }
        

        public void Dispose()
        {
            //NA
        }
    }
}