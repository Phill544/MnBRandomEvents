using System;
using MCM.Abstractions.Base.Global;
using MCM.Abstractions.FluentBuilder;
using MCM.Common;
using TaleWorlds.Localization;

namespace CryingBuffalo.RandomEvents.Settings.MCM
{
    public class MCM_MenuConfig_N_Z : IDisposable
    {
        private static MCM_MenuConfig_N_Z _instance;

        private FluentGlobalSettings globalSettings;

        #region Variables
        
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
        
        #region Passing Comet - Variables

        public bool PC_Disable { get; private set; }

        #endregion
        
        #region Perfect Weather - Variables
        
        public bool PW_Disable { get; private set; }
        public int PW_MinMoraleGain { get; private set; }
        public int PW_MaxMoraleGain { get; private set; }
        
        #endregion
        
        #region Prisoner Rebellion - Variables
        
        public bool PR_Disable { get; private set; }
        public int PR_MinPrisoners { get; private set; }

        #endregion
        
        #region Prisoner Transfer - Variables
        
        public bool PT_Disable { get; private set; }
        public int PT_MinPrisoners { get; private set; }
        public int PT_MaxPrisoners { get; private set; }
        public int PT_MinPricePrPrisoner { get; private set; }
        public int PT_MaxPricePrPrisoner { get; private set; }

        #endregion
        
        #region Red Moon - Variables
        
        public bool RM_Disable { get; private set; }
        public int RM_MinGoldLost { get; private set; }
        public int RM_MaxGoldLost { get; private set; }
        public int RM_MinMenLost { get; private set; }
        public int RM_MaxMenLost { get; private set; }

        #endregion
        
        #region Refugees - Variables

        public bool RF_Disable { get; private set; }
        public int RF_minSoldiers { get; private set; }
        public int RF_maxSoldiers { get; private set; }
        public int RF_minFood { get; private set; }
        public int RF_maxFood { get; private set; }
        public int RF_minCaptive { get; private set; }
        public int RF_maxCaptive { get; private set; }

        #endregion
        
        #region Robbery - Variables
        
        public bool RO_Disable { get; private set; }
        public int RO_MinGoldLost { get; private set; }
        public int RO_MaxGoldLost { get; private set; }
        public int RO_MinRenownLost { get; private set; }
        public int RO_MaxRenownLost { get; private set; }

        #endregion

        #region Runaway Son - Variables
        
        public bool RS_Disable { get; private set; }
        public int RS_MinGoldGained { get; private set; }
        public int RS_MaxGoldGained { get; private set; }
        
        #endregion
        
        #region Secret Singer - Variables
        
        public bool SS_Disable { get; private set; }
        public int SS_MinMoraleGained { get; private set; }
        public int SS_MaxMoraleGained { get; private set; }
        
        #endregion
        
        #region Speedy Recovery - Variables
        
        public bool SR_Disable { get; private set; }
        public int SR_MinMenToRecover { get; private set; }
        public int SR_MaxMenToRecover { get; private set; }
        
        #endregion
        
        #region Successful Deeds - Variables
        
        public bool SD_Disable { get; private set; }
        public int SD_MinInfluenceGained { get; private set; }
        public int SD_MaxInfluenceGained { get; private set; }
        
        #endregion
        
        #region Sudden Storm - Variables
        
        public bool SuS_Disable { get; private set; }
        public int SuS_MinHorsesLost { get; private set; }
        public int SuS_MaxHorsesLost { get; private set; }
        public int SuS_MinMenDied { get; private set; }
        public int SuS_MaxMenDied { get; private set; }
        public int SuS_MinMenWounded { get; private set; }
        public int SuS_MaxMenWounded { get; private set; }
        public int SuS_MinMeatFromHorse { get; private set; }
        public int SuS_MaxMeatFromHorse { get; private set; }
        
        #endregion
        
        #region Supernatural Encounter - Variables

        public bool SE_Disable { get; private set; }

        #endregion
        
        #region Target Practice - Variables
        
        public bool TP_Disable { get; private set; }
        public int TP_MinSoldiers { get; private set; }
        public float TP_PercentageDifferenceOfCurrentTroop { get; private set; }
        
        #endregion
        
        #region Travelling Merchant - Variables

        public bool TM_Disable { get; private set; }
        public int TM_minloot { get; private set; }
        public int TM_maxloot { get; private set; }

        #endregion
        
        #region Unexcpected Wedding - Variables
        
        public bool UW_Disable { get; private set; }
        public int UW_MinGoldToDonate { get; private set; }
        public int UW_MaxGoldToDonate { get; private set; }
        public int UW_MinPeopleInWedding { get; private set; }
        public int UW_MaxPeopleInWedding { get; private set; }
        public int UW_EmbarrassedSoliderMaxGold { get; private set; }
        public int UW_MinGoldRaided { get; private set; }
        public int UW_MaxGoldRaided { get; private set; }
        
        #endregion
        
        #region Undercooked - Variables
        
        public bool UC_Disable { get; private set; }
        public int UC_MinSoldiersToInjure { get; private set; }
        public int UC_MaxSoldiersToInjure { get; private set; }
        
        #endregion
        
        #region Violated Girl - Variables
        
        public bool VG_Disable { get; private set; }
        public int VG_MinCompensation { get; private set; }
        public int VG_MaxCompensation { get; private set; }
        
        #endregion
        
        #region Wandering Livestock - Variables
        
        public bool WL_Disable { get; private set; }
        public int WL_MinFood { get; private set; }
        public int WL_MaxFood { get; private set; }
        
        #endregion

        #endregion


        public static MCM_MenuConfig_N_Z Instance
        {
            get { return _instance ??= new MCM_MenuConfig_N_Z(); }
        }

        public void Settings()
        {
            

            #region Strings

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
            
            #region Passing Comet - Strings

            var pc_heading = new TextObject("{=mcm_pc_heading}Passing Comet").ToString();
            var pc1_text = new TextObject("{=mcm_pc1_text}1. Deactivate event").ToString();
            var pc1_hint = new TextObject("{=mcm_pc1_hint}If you dont want this event to show up you can deactivate it.").ToString();

            #endregion
            
            #region Perfect Weather - Strings
            
            var pw_heading = new TextObject("{=mcm_pw_heading}Perfect Weather").ToString();
            var pw1_text = new TextObject("{=mcm_pw1_text}1. Min Morale Gain").ToString();
            var pw1_hint = new TextObject("{=mcm_pw1_hint}Minimum amount of morale gained during this event.").ToString();
            var pw2_text = new TextObject("{=mcm_pw2_text}2. Max Morale Gain").ToString();
            var pw2_hint = new TextObject("{=mcm_pw2_hint}Maximum amount morale gained lost during this event.").ToString();
            var pw3_text = new TextObject("{=mcm_pw3_text}3. Deactivate event").ToString();
            var pw3_hint = new TextObject("{=mcm_pw3_hint}If you dont want this event to show up you can deactivate it.").ToString();
            
            #endregion
            
            #region Prisoner Rebellion - Strings
            
            var pr_heading = new TextObject("{=mcm_pr_heading}Prisoner Rebellion").ToString();
            var pr1_text = new TextObject("{=mcm_pr1_text}1. Min Prisoners").ToString();
            var pr1_hint = new TextObject("{=mcm_pr1_hint}Minimum amount of prisoners needed for the event to trigger.").ToString();
            var pr2_text = new TextObject("{=mcm_pr2_text}2. Deactivate event").ToString();
            var pr2_hint = new TextObject("{=mcm_pr2_hint}If you dont want this event to show up you can deactivate it.").ToString();
            
            #endregion
            
            #region Prisoner Transfer - Strings
            
            var pt_heading = new TextObject("{=mcm_pt_heading}Prisoner Transfer").ToString();
            var pt1_text = new TextObject("{=mcm_pt1_text}1. Min Prisoners").ToString();
            var pt1_hint = new TextObject("{=mcm_pt1_hint}Minimum amount of prisoners to transfer.").ToString();
            var pt2_text = new TextObject("{=mcm_pt2_text}2. Min Prisoners").ToString();
            var pt2_hint = new TextObject("{=mcm_pt2_hint}Maximum amount of prisoners to transfer.").ToString();
            var pt3_text = new TextObject("{=mcm_pt3_text}3. Min Price for each prisoner").ToString();
            var pt3_hint = new TextObject("{=mcm_pt3_hint}Minimum price for each prisoner.").ToString();
            var pt4_text = new TextObject("{=mcm_pt4_text}4. Max Price for each prisoner").ToString();
            var pt4_hint = new TextObject("{=mcm_pt4_hint}Maximum price for each prisoner.").ToString();
            var pt5_text = new TextObject("{=mcm_pt5_text}5. Deactivate event").ToString();
            var pt5_hint = new TextObject("{=mcm_pt5_hint}If you dont want this event to show up you can deactivate it.").ToString();
            
            #endregion
            
            #region Red Moon - Strings
            
            var rm_heading = new TextObject("{=mcm_rm_heading}Prisoner Rebellion").ToString();
            var rm1_text = new TextObject("{=mcm_rm1_text}1. Min Gold Lost").ToString();
            var rm1_hint = new TextObject("{=mcm_rm1_hint}Minimum amount of gold that can be lost during this event.").ToString();
            var rm2_text = new TextObject("{=mcm_rm2_text}2. Max Gold Lost").ToString();
            var rm2_hint = new TextObject("{=mcm_rm2_hint}Maximum amount of gold that can be lost during this event.").ToString();
            var rm3_text = new TextObject("{=mcm_rm3_text}3. Min Men Lost").ToString();
            var rm3_hint = new TextObject("{=mcm_rm3_hint}Minimum amount of men that can be lost during this event.").ToString();
            var rm4_text = new TextObject("{=mcm_rm4_text}4. Max Men Lost").ToString();
            var rm4_hint = new TextObject("{=mcm_rm4_hint}Maximum amount of gold that can be lost during this event.").ToString();
            var rm5_text = new TextObject("{=mcm_rm5_text}5. Deactivate event").ToString();
            var rm5_hint = new TextObject("{=mcm_rm5_hint}If you dont want this event to show up you can deactivate it.").ToString();
            
            #endregion
            
            #region Refugees - Strings
            
            var rf_heading = new TextObject("{=mcm_rf_heading}Refugees").ToString();
            var rf1_text = new TextObject("{=mcm_rf_text}1.Min Soldiers").ToString();
            var rf1_hint = new TextObject("{=mcm_rf_text}The minimum amount of recruits.").ToString();
            var rf2_text = new TextObject("{=mcm_rf_text}2.Max Soldiers").ToString();
            var rf2_hint = new TextObject("{=mcm_rf_text}The maximum amount of recruits.").ToString();
            var rf3_text = new TextObject("{=mcm_rf_text}3.Min Food").ToString();
            var rf3_hint = new TextObject("{=mcm_rf_text}The minimum amount of Food.").ToString();
            var rf4_text = new TextObject("{=mcm_rf_text}4.Max Food").ToString();
            var rf4_hint = new TextObject("{=mcm_rf_text}The maximum amount of Food.").ToString();
            var rf5_text = new TextObject("{=mcm_rf_text}5.Min Captives").ToString();
            var rf5_hint = new TextObject("{=mcm_rf_text}The minimum amount of Captives.").ToString();
            var rf6_text = new TextObject("{=mcm_rf_text}6.Max Captives").ToString();
            var rf6_hint = new TextObject("{=mcm_rf_text}The maximum amount of Captives.").ToString();
            var rf7_text = new TextObject("{=mcm_rf_text}7. Deactivate event").ToString();
            var rf7_hint = new TextObject("{=mcm_rf_text}If you dont want this event to show up you can deactivate it.").ToString();

            #endregion
            
            #region Robbery - Strings
            
            var ro_heading = new TextObject("{=mcm_ro_heading}Robbery").ToString();
            var ro1_text = new TextObject("{=mcm_ro1_text}1. Min Gold Lost").ToString();
            var ro1_hint = new TextObject("{=mcm_ro1_hint}Minimum amount of gold that can be lost during this event.").ToString();
            var ro2_text = new TextObject("{=mcm_ro2_text}2. Max Gold Lost").ToString();
            var ro2_hint = new TextObject("{=mcm_ro2_hint}Maximum amount of gold that can be lost during this event.").ToString();
            var ro3_text = new TextObject("{=mcm_ro3_text}3. Min Renown Lost").ToString();
            var ro3_hint = new TextObject("{=mcm_ro3_hint}Minimum amount of renown that can be lost during this event.").ToString();
            var ro4_text = new TextObject("{=mcm_ro4_text}4. Max Renown Lost").ToString();
            var ro4_hint = new TextObject("{=mcm_ro4_hint}Maximum amount of renown that can be lost during this event.").ToString();
            var ro5_text = new TextObject("{=mcm_ro5_text}5. Deactivate event").ToString();
            var ro5_hint = new TextObject("{=mcm_ro5_hint}If you dont want this event to show up you can deactivate it.").ToString();
            
            #endregion
            
            #region Runaway Son - Strings
            
            var rs_heading = new TextObject("{=mcm_rs_heading}Runaway Son").ToString();
            var rs1_text = new TextObject("{=mcm_rs1_text}1. Min Gold Gained").ToString();
            var rs1_hint = new TextObject("{=mcm_rs1_hint}Minimum amount of gold that can gained during this event.").ToString();
            var rs2_text = new TextObject("{=mcm_rs2_text}2. Max Gold Gain").ToString();
            var rs2_hint = new TextObject("{=mcm_rs2_hint}Maximum amount of gold that can gained during this event.").ToString();
            var rs3_text = new TextObject("{=mcm_rs3_text}3. Deactivate event").ToString();
            var rs3_hint = new TextObject("{=mcm_rs3_hint}If you dont want this event to show up you can deactivate it.").ToString();
            
            #endregion
            
            #region Secret Singer - Strings
            
            var ss_heading = new TextObject("{=mcm_ss_heading}Secret Singer").ToString();
            var ss1_text = new TextObject("{=mcm_ss1_text}1. Min Morale Gained").ToString();
            var ss1_hint = new TextObject("{=mcm_ss1_hint}Minimum amount of morale that can gained during this event.").ToString();
            var ss2_text = new TextObject("{=mcm_ss2_text}2. Max Morale Gain").ToString();
            var ss2_hint = new TextObject("{=mcm_ss2_hint}Maximum amount of morale that can gained during this event.").ToString();
            var ss3_text = new TextObject("{=mcm_ss3_text}3. Deactivate event").ToString();
            var ss3_hint = new TextObject("{=mcm_ss3_hint}If you dont want this event to show up you can deactivate it.").ToString();
            
            #endregion
            
            #region Speedy Recover - Strings
            
            var sr_heading = new TextObject("{=mcm_sr_heading}Speedy Recovery").ToString();
            var sr1_text = new TextObject("{=mcm_sr1_text}1. Min Men To Heal").ToString();
            var sr1_hint = new TextObject("{=mcm_sr1_hint}Minimum amount of men that can be healed during this event.").ToString();
            var sr2_text = new TextObject("{=mcm_sr2_text}2. Max Men To Heal").ToString();
            var sr2_hint = new TextObject("{=mcm_sr2_hint}Maximum amount of of men that can be healed during this event.").ToString();
            var sr3_text = new TextObject("{=mcm_sr3_text}3. Deactivate event").ToString();
            var sr3_hint = new TextObject("{=mcm_sr3_hint}If you dont want this event to show up you can deactivate it.").ToString();
            
            #endregion
            
            #region Successful Deeds - Strings
            
            var sd_heading = new TextObject("{=mcm_sd_heading}Successful Deeds").ToString();
            var sd1_text = new TextObject("{=mcm_sd1_text}1. Min Influence Gained").ToString();
            var sd1_hint = new TextObject("{=mcm_sd1_hint}Minimum amount of influence that can be gained during this event.").ToString();
            var sd2_text = new TextObject("{=mcm_sd2_text}2. Max Influence Gained").ToString();
            var sd2_hint = new TextObject("{=mcm_sd2_hint}Maximum amount of of influence that can be gained during this event.").ToString();
            var sd3_text = new TextObject("{=mcm_sd3_text}3. Deactivate event").ToString();
            var sd3_hint = new TextObject("{=mcm_sd3_hint}If you dont want this event to show up you can deactivate it.").ToString();
            
            #endregion
            
            #region Sudden Storm - Strings
            
            var sus_heading = new TextObject("{=mcm_sus_heading}Sudden Storm").ToString();
            var sus1_text = new TextObject("{=mcm_sus1_text}1. Min Horses Killed").ToString();
            var sus1_hint = new TextObject("{=mcm_sus1_hint}Minimum amount of horses killed during this event.").ToString();
            var sus2_text = new TextObject("{=mcm_sus2_text}2. Max Horses Killed").ToString();
            var sus2_hint = new TextObject("{=mcm_sus2_hint}Maximum amount of of horses killed during this event.").ToString();
            var sus3_text = new TextObject("{=mcm_sus3_text}3. Min Men Killed").ToString();
            var sus3_hint = new TextObject("{=mcm_sus3_hint}Minimum amount of men killed during this event.").ToString();
            var sus4_text = new TextObject("{=mcm_sus4_text}4. Max Men Killed").ToString();
            var sus4_hint = new TextObject("{=mcm_sus4_hint}Maximum amount of men killed during this event.").ToString();
            var sus5_text = new TextObject("{=mcm_sus5_text}5. Min Men Wounded").ToString();
            var sus5_hint = new TextObject("{=mcm_sus5_hint}Minimum amount of men wounded during this event.").ToString();
            var sus6_text = new TextObject("{=mcm_sus6_text}6. Max Men Wounded").ToString();
            var sus6_hint = new TextObject("{=mcm_sus6_hint}Maximum amount of men wounded gained during this event.").ToString();
            var sus7_text = new TextObject("{=mcm_sus7_text}7. Min Meat Multiplier").ToString();
            var sus7_hint = new TextObject("{=mcm_sus7_hint}Minimum meat multiplier used to calculate the amount of meat yielded during this event.").ToString();
            var sus8_text = new TextObject("{=mcm_sus8_text}8. Max Meat Multiplier").ToString();
            var sus8_hint = new TextObject("{=mcm_sus8_hint}Maximum meat multiplier used to calculate the amount of meat yielded during this event.").ToString();
            var sus9_text = new TextObject("{=mcm_sus9_text}9. Deactivate event").ToString();
            var sus9_hint = new TextObject("{=mcm_sus9_hint}If you dont want this event to show up you can deactivate it.").ToString();
            
            #endregion
            
            #region Supernatural Encounter - Strings

            var se_heading = new TextObject("{=mcm_se_heading}Supernatural Encounter").ToString();
            var se1_text = new TextObject("{=mcm_se1_text}1. Deactivate event").ToString();
            var se1_hint = new TextObject("{=mcm_se1_hint}If you dont want this event to show up you can deactivate it.").ToString();

            #endregion
            
            #region Target Practice - Strings
            
            var tp_heading = new TextObject("{=mcm_tp_heading}Target Practice").ToString();
            var tp1_text = new TextObject("{=mcm_tp1_text}1. Min Soldiers").ToString();
            var tp2_text = new TextObject("{=mcm_tp2_text}2. % Difference Of Current Troop").ToString();
            var tp3_text = new TextObject("{=mcm_tp3_text}3. Deactivate event").ToString();
            var tp3_hint = new TextObject("{=mcm_tp3_hint}If you dont want this event to show up you can deactivate it.").ToString();
            
            #endregion
            
            #region Travelling Merchant - Strings

            var tm_heading = new TextObject("{=mcm_tm_heading}Travelling Merchant").ToString();
            var tm1_text = new TextObject("{=mcm_tm1_text}1. Min Loot").ToString();
            var tm1_hint = new TextObject("{=mcm_tm1_hint}Minimum Amount of Loot").ToString();
            var tm2_text = new TextObject("{=mcm_tm2_text}2. Max Loot").ToString();
            var tm2_hint = new TextObject("{=mcm_tm2_hint}Maximum Amount of Loot").ToString();
            var tm3_text = new TextObject("{=mcm_tm3_text}3. Deactivate event").ToString();
            var tm3_hint = new TextObject("{=mcm_tm3_hint}If you dont want this event to show up you can deactivate it.").ToString();

            #endregion
            
            #region Unexpected Wedding - Strings
            
            var uw_heading = new TextObject("{=mcm_uw_heading}Unexpected Wedding").ToString();
            var uw1_text = new TextObject("{=mcm_uw1_text}1. Min Gold To Give").ToString();
            var uw1_hint = new TextObject("{=mcm_uw1_hint}Minimum amount of gold you give as a gift.").ToString();
            var uw2_text = new TextObject("{=mcm_uw2_text}2. Max Gold To Give").ToString();
            var uw2_hint = new TextObject("{=mcm_uw2_hint}Maximum amount of gold you give as a gift.").ToString();
            var uw3_text = new TextObject("{=mcm_uw3_text}3. Min People In Wedding").ToString();
            var uw3_hint = new TextObject("{=mcm_uw3_hint}Minimum amount of people in the wedding.").ToString();
            var uw4_text = new TextObject("{=mcm_uw4_text}4. Max People In Wedding").ToString();
            var uw4_hint = new TextObject("{=mcm_uw4_hint}Maximum amount of people in the wedding.").ToString();
            var uw5_text = new TextObject("{=mcm_uw5_text}5. Max Gold Soldier Must Give").ToString();
            var uw5_hint = new TextObject("{=mcm_uw5_hint}Maximum amount of gold a solider is forced to give if the event requires it.").ToString();
            var uw6_text = new TextObject("{=mcm_uw6_text}6. Min Gold To Raid").ToString();
            var uw6_hint = new TextObject("{=mcm_uw6_hint}Minimum amount of of gold that can be raided during this event.").ToString();
            var uw7_text = new TextObject("{=mcm_uw7_text}7. Max Gold To Raid").ToString();
            var uw7_hint = new TextObject("{=mcm_uw7_hint}Maximum amount of gold that can be raided during this event.").ToString();
            var uw8_text = new TextObject("{=mcm_uw8_text}8. Deactivate event").ToString();
            var uw8_hint = new TextObject("{=mcm_uw8_hint}If you dont want this event to show up you can deactivate it.").ToString();
            
            #endregion
            
            #region Undercooked- Strings
            
            var uc_heading = new TextObject("{=mcm_uc_heading}Undercooked").ToString();
            var uc1_text = new TextObject("{=mcm_uc1_text}1. Min Soldiers To Injure").ToString();
            var uc1_hint = new TextObject("{=mcm_uc1_hint}The minimum amount of soldiers to get injured during this event.").ToString();
            var uc2_text = new TextObject("{=mcm_uc2_text}2. Max Soldiers To Injure").ToString();
            var uc2_hint = new TextObject("{=mcm_uc2_hint}The maximum amount of soldiers to get injured during this event.").ToString();
            var uc3_text = new TextObject("{=mcm_uc3_text}3. Deactivate event").ToString();
            var uc3_hint = new TextObject("{=mcm_uc3_hint}If you dont want this event to show up you can deactivate it.").ToString();
            
            #endregion
            
            #region Violated Girl - Strings
            
            var vg_heading = new TextObject("{=mcm_vg_heading}Violated Girl").ToString();
            var vg1_text = new TextObject("{=mcm_vg1_text}1. Min Gold Compensation").ToString();
            var vg1_hint = new TextObject("{=mcm_vg1_hint}The minimum amount of compensation the girl gets during this event.").ToString();
            var vg2_text = new TextObject("{=mcm_vg2_text}2. Max Gold Compensation").ToString();
            var vg2_hint = new TextObject("{=mcm_vg2_hint}The maximum amount of compensation the girl gets during this event.").ToString();
            var vg3_text = new TextObject("{=mcm_vg3_text}3. Deactivate event").ToString();
            var vg3_hint = new TextObject("{=mcm_vg3_hint}If you dont want this event to show up you can deactivate it.").ToString();
            
            #endregion
            
            #region Wandering Livestock - Strings
            
            var wl_heading = new TextObject("{=mcm_wl_heading}Wandering Livestock").ToString();
            var wl1_text = new TextObject("{=mcm_wl1_text}1. Min Food").ToString();
            var wl1_hint = new TextObject("{=mcm_wl1_hint}The minimum amount of food to get during this event.").ToString();
            var wl2_text = new TextObject("{=mcm_wl2_text}2. Max Food").ToString();
            var wl2_hint = new TextObject("{=mcm_wl2_hint}The maximum amount of food to get during this event.").ToString();
            var wl3_text = new TextObject("{=mcm_wl3_text}3. Deactivate event").ToString();
            var wl3_hint = new TextObject("{=mcm_wl3_hint}If you dont want this event to show up you can deactivate it.").ToString();
            
            #endregion

            #endregion
            
            
            
            var builder = BaseSettingsBuilder.Create("RandomEvents2","2. Random Events - N to Z")!
                .SetFormat("xml")
                .SetFolderName(RandomEventsSubmodule.FolderName)
                .SetSubFolder(RandomEventsSubmodule.ModName)
                
           #region Builder Modules
                
                
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
                    )

                    #endregion
                
                #region Passing Comet - Builder
                
                .CreateGroup(pc_heading, groupBuilder => groupBuilder
                    .AddBool("PC1", pc1_text, new ProxyRef<bool>(() => PC_Disable, o => PC_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(pc1_hint))
                    )

                #endregion
                
                #region Perfect Weather - Builder
                
                .CreateGroup(pw_heading, groupBuilder => groupBuilder
                    .AddInteger("PW1", pw1_text,5,30, new ProxyRef<int>(() => PW_MinMoraleGain, o => PW_MinMoraleGain = o), integerBuilder => integerBuilder
                        .SetHintText(pw1_hint))
                    .AddInteger("PW2", pw2_text,5,30, new ProxyRef<int>(() => PW_MaxMoraleGain, o => PW_MaxMoraleGain = o), integerBuilder => integerBuilder
                        .SetHintText(pw2_hint))
                    .AddBool("PW3", pw3_text, new ProxyRef<bool>(() => PW_Disable, o => PW_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(pw3_hint))
                    )


                #endregion
                
                #region Prisoner Rebellion - Builder
                
                .CreateGroup(pr_heading, groupBuilder => groupBuilder
                        .AddInteger("PR1", pr1_text,10,60, new ProxyRef<int>(() => PR_MinPrisoners, o => PR_MinPrisoners = o), integerBuilder => integerBuilder
                            .SetHintText(pr1_hint))
                        .AddBool("PR2", pr2_text, new ProxyRef<bool>(() => PR_Disable, o => PR_Disable = o), boolBuilder => boolBuilder
                            .SetHintText(pr2_hint))
                    )
                
                #endregion
                
                #region Prisoner Transfer - Builder
                
                .CreateGroup(pt_heading, groupBuilder => groupBuilder
                    .AddInteger("PT1", pt1_text,10,60, new ProxyRef<int>(() => PT_MinPrisoners, o => PT_MinPrisoners = o), integerBuilder => integerBuilder
                        .SetHintText(pt1_hint))
                    .AddInteger("PT2", pt2_text,10,60, new ProxyRef<int>(() => PT_MaxPrisoners, o => PT_MaxPrisoners = o), integerBuilder => integerBuilder
                        .SetHintText(pt2_hint))
                    .AddInteger("PT3", pt3_text,10,60, new ProxyRef<int>(() => PT_MinPricePrPrisoner, o => PT_MinPricePrPrisoner = o), integerBuilder => integerBuilder
                        .SetHintText(pt3_hint))
                    .AddInteger("PT4", pt4_text,10,60, new ProxyRef<int>(() => PT_MaxPricePrPrisoner, o => PT_MaxPricePrPrisoner = o), integerBuilder => integerBuilder
                        .SetHintText(pt4_hint))
                    .AddBool("PT5", pt5_text, new ProxyRef<bool>(() => PT_Disable, o => PT_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(pt5_hint))
                )
                
                #endregion
                
                #region Red Moon - Builder
                
                .CreateGroup(rm_heading, groupBuilder => groupBuilder
                    .AddInteger("RM1", rm1_text,500,5000, new ProxyRef<int>(() => RM_MinGoldLost, o => RM_MinGoldLost = o), integerBuilder => integerBuilder
                        .SetHintText(rm1_hint))
                    .AddInteger("RM2", rm2_text,500,5000, new ProxyRef<int>(() => RM_MaxGoldLost, o => RM_MaxGoldLost = o), integerBuilder => integerBuilder
                        .SetHintText(rm2_hint))
                    .AddInteger("RM3", rm3_text,10,100, new ProxyRef<int>(() => RM_MinMenLost, o => RM_MinMenLost = o), integerBuilder => integerBuilder
                        .SetHintText(rm3_hint))
                    .AddInteger("RM4", rm4_text,10,100, new ProxyRef<int>(() => RO_MaxGoldLost, o => RO_MaxGoldLost = o), integerBuilder => integerBuilder
                        .SetHintText(rm4_hint))
                    .AddBool("RM5", rm5_text, new ProxyRef<bool>(() => RM_Disable, o => RM_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(rm5_hint))
                )
                            
                #endregion
                
                #region Refugees - Builder
                .CreateGroup(rf_heading, groupBuilder => groupBuilder
                    .AddInteger("RF1", rf1_text, 1, 30, new ProxyRef<int>(() => RF_minSoldiers, o => RF_minSoldiers = o), integerBuilder => integerBuilder
                        .SetHintText(rf1_hint))
                    .AddInteger("RF2", rf2_text, 1, 30, new ProxyRef<int>(() => RF_maxSoldiers, o => RF_maxSoldiers = o), integerBuilder => integerBuilder
                        .SetHintText(rf2_hint))
                    .AddInteger("RF3", rf3_text, 1, 20, new ProxyRef<int>(() => RF_minFood, o => RF_minFood = o), integerBuilder => integerBuilder
                        .SetHintText(rf3_hint))
                    .AddInteger("RF4", rf4_text, 1, 20, new ProxyRef<int>(() => RF_maxFood, o => RF_maxFood = o), integerBuilder => integerBuilder
                        .SetHintText(rf4_hint))
                    .AddInteger("RF5", rf5_text, 1, 30, new ProxyRef<int>(() => RF_minCaptive, o => RF_minCaptive = o), integerBuilder => integerBuilder
                        .SetHintText(rf5_hint))
                    .AddInteger("RF6", rf6_text, 1, 30, new ProxyRef<int>(() => RF_maxCaptive, o => RF_maxCaptive = o), integerBuilder => integerBuilder
                        .SetHintText(rf6_hint))
                    .AddBool("RF7", rf7_text, new ProxyRef<bool>(() => RF_Disable, o => RF_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(rf7_hint))

                )
                #endregion

                #region Robbery - Builder
                
                .CreateGroup(ro_heading, groupBuilder => groupBuilder
                    .AddInteger("RO1", ro1_text,500,5000, new ProxyRef<int>(() => RO_MinGoldLost, o => RO_MinGoldLost = o), integerBuilder => integerBuilder
                        .SetHintText(ro1_hint))
                    .AddInteger("RO2", ro2_text,500,5000, new ProxyRef<int>(() => RO_MaxGoldLost, o => RO_MaxGoldLost = o), integerBuilder => integerBuilder
                        .SetHintText(ro2_hint))
                    .AddInteger("RO3", ro3_text,10,100, new ProxyRef<int>(() => RO_MinRenownLost, o => RO_MinRenownLost = o), integerBuilder => integerBuilder
                        .SetHintText(ro3_hint))
                    .AddInteger("RO4", ro4_text,10,100, new ProxyRef<int>(() => RO_MaxRenownLost, o => RO_MaxRenownLost = o), integerBuilder => integerBuilder
                        .SetHintText(ro4_hint))
                    .AddBool("RO5", ro5_text, new ProxyRef<bool>(() => RO_Disable, o => RO_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(ro5_hint))
                )
                            
                #endregion

                #region Runaway Son - Builder
                
                .CreateGroup(rs_heading, groupBuilder => groupBuilder
                        .AddInteger("RS1", rs1_text,5,250, new ProxyRef<int>(() => RS_MinGoldGained, o => RS_MinGoldGained = o), integerBuilder => integerBuilder
                            .SetHintText(rs1_hint))
                        .AddInteger("RS2", rs2_text,5,250, new ProxyRef<int>(() => RS_MaxGoldGained, o => RS_MaxGoldGained = o), integerBuilder => integerBuilder
                            .SetHintText(rs2_hint))
                        .AddBool("RS3", rs3_text, new ProxyRef<bool>(() => RS_Disable, o => RS_Disable = o), boolBuilder => boolBuilder
                            .SetHintText(rs3_hint))
                    )
                    
                #endregion
                
                #region Secret Singer - Builder
                
                .CreateGroup(ss_heading, groupBuilder => groupBuilder
                        .AddInteger("SS1", ss1_text,5,100, new ProxyRef<int>(() => SS_MinMoraleGained, o => SS_MinMoraleGained = o), integerBuilder => integerBuilder
                            .SetHintText(ss1_hint))
                        .AddInteger("SS2", ss2_text,5,100, new ProxyRef<int>(() => SS_MaxMoraleGained, o => SS_MaxMoraleGained = o), integerBuilder => integerBuilder
                            .SetHintText(ss2_hint))
                        .AddBool("SS3", ss3_text, new ProxyRef<bool>(() => SS_Disable, o => SS_Disable = o), boolBuilder => boolBuilder
                            .SetHintText(ss3_hint))
                    )
                    
                    #endregion
                    
                #region Speedy Recovery - Builder
                
                .CreateGroup(sr_heading, groupBuilder => groupBuilder
                        .AddInteger("SR1", sr1_text,2,50, new ProxyRef<int>(() => SR_MinMenToRecover, o => SR_MinMenToRecover = o), integerBuilder => integerBuilder
                            .SetHintText(sr1_hint))
                        .AddInteger("SR2", sr2_text,2,50, new ProxyRef<int>(() => SR_MaxMenToRecover, o => SR_MaxMenToRecover = o), integerBuilder => integerBuilder
                            .SetHintText(sr2_hint))
                        .AddBool("SR3", sr3_text, new ProxyRef<bool>(() => SR_Disable, o => SR_Disable = o), boolBuilder => boolBuilder
                            .SetHintText(sr3_hint))
                    )
                    
                #endregion
                
                #region Successful Deeds - Builder
                
                .CreateGroup(sd_heading, groupBuilder => groupBuilder
                        .AddInteger("SD1", sd1_text,10,500, new ProxyRef<int>(() => SD_MinInfluenceGained, o => SD_MinInfluenceGained = o), integerBuilder => integerBuilder
                            .SetHintText(sd1_hint))
                        .AddInteger("SD2", sd2_text,10,500, new ProxyRef<int>(() => SD_MaxInfluenceGained, o => SD_MaxInfluenceGained = o), integerBuilder => integerBuilder
                            .SetHintText(sd2_hint))
                        .AddBool("SD3", sd3_text, new ProxyRef<bool>(() => SD_Disable, o => SD_Disable = o), boolBuilder => boolBuilder
                            .SetHintText(sd3_hint))
                    )
                    
                #endregion
                
                #region Sudden Storm - Builder
                
                .CreateGroup(sus_heading, groupBuilder => groupBuilder
                    .AddInteger("SuS1", sus1_text,5,20, new ProxyRef<int>(() => SuS_MinHorsesLost, o => SuS_MinHorsesLost = o), integerBuilder => integerBuilder
                        .SetHintText(sus1_hint))
                    .AddInteger("SuS2", sus2_text,5,20, new ProxyRef<int>(() => SuS_MaxHorsesLost, o => SuS_MaxHorsesLost = o), integerBuilder => integerBuilder
                        .SetHintText(sus2_hint))
                    .AddInteger("SuS3", sus3_text,5,50, new ProxyRef<int>(() => SuS_MinMenDied, o => SuS_MinMenDied = o), integerBuilder => integerBuilder
                        .SetHintText(sus3_hint))
                    .AddInteger("SuS4", sus4_text,5,50, new ProxyRef<int>(() => SuS_MaxMenDied, o => SuS_MaxMenDied = o), integerBuilder => integerBuilder
                        .SetHintText(sus4_hint))
                    .AddInteger("SuS5", sus5_text,5,25, new ProxyRef<int>(() => SuS_MinMenWounded, o => SuS_MinMenWounded = o), integerBuilder => integerBuilder
                        .SetHintText(sus5_hint))
                    .AddInteger("SuS6", sus6_text,5,25, new ProxyRef<int>(() => SuS_MaxMenWounded, o => SuS_MaxMenWounded = o), integerBuilder => integerBuilder
                        .SetHintText(sus6_hint))
                    .AddInteger("SuS7", sus7_text,3,10, new ProxyRef<int>(() => SuS_MinMeatFromHorse, o => SuS_MinMeatFromHorse = o), integerBuilder => integerBuilder
                        .SetHintText(sus7_hint))
                    .AddInteger("SuS8", sus8_text,3,10, new ProxyRef<int>(() => SuS_MaxMeatFromHorse, o => SuS_MaxMeatFromHorse = o), integerBuilder => integerBuilder
                        .SetHintText(sus8_hint))
                    .AddBool("SuS9", sus9_text, new ProxyRef<bool>(() => SuS_Disable, o => SuS_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(sus9_hint))
                )
                #endregion
                
                #region Supernatural Encounter - Builder
                
                .CreateGroup(se_heading, groupBuilder => groupBuilder
                    .AddBool("SE1", se1_text, new ProxyRef<bool>(() => SE_Disable, o => SE_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(se1_hint))
                    )
                

                #endregion
                
                #region Target Practice- Builder
                
                .CreateGroup(tp_heading, groupBuilder => groupBuilder
                        .AddInteger("TP1", tp1_text,20,60, new ProxyRef<int>(() => TP_MinSoldiers, o => TP_MinSoldiers = o), integerBuilder => integerBuilder
                            .SetHintText(""))
                        .AddFloatingInteger("TP2", tp2_text,0.1f,1.0f, new ProxyRef<float>(() => TP_PercentageDifferenceOfCurrentTroop, o => TP_PercentageDifferenceOfCurrentTroop = o), floatBuilder => floatBuilder
                            .SetHintText(""))
                        .AddBool("TP3", tp3_text, new ProxyRef<bool>(() => TP_Disable, o => TP_Disable = o), boolBuilder => boolBuilder
                            .SetHintText(tp3_hint))
                        )

                #endregion
                
                #region Travelling Merchant - Builder

                .CreateGroup(tm_heading, groupBuilder => groupBuilder
                    .AddInteger("TM1", tm1_text, 1, 3500, new ProxyRef<int>(() => TM_minloot, o => TM_minloot = o), integerBuilder => integerBuilder
                        .SetHintText(tm1_hint))
                    .AddInteger("TM2", tm2_text, 1, 20000, new ProxyRef<int>(() => TM_maxloot, o => TM_maxloot = o), integerBuilder => integerBuilder
                        .SetHintText(tm2_hint))
                    .AddBool("TM3", tm3_text, new ProxyRef<bool>(() => TM_Disable, o => TM_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(tm3_hint))
                )

                #endregion
                
                #region Unexpected Wedding - Builder
                
                .CreateGroup(uw_heading, groupBuilder => groupBuilder
                        .AddInteger("UW1", uw1_text,20,500, new ProxyRef<int>(() => UW_MinGoldToDonate, o => UW_MinGoldToDonate = o), integerBuilder => integerBuilder
                            .SetHintText(uw1_hint))
                        .AddInteger("UW2", uw2_text,20,500, new ProxyRef<int>(() => UW_MaxGoldToDonate, o => UW_MaxGoldToDonate = o), integerBuilder => integerBuilder
                            .SetHintText(uw2_hint))
                        .AddInteger("UW3", uw3_text,15,75, new ProxyRef<int>(() => UW_MinPeopleInWedding, o => UW_MinPeopleInWedding = o), integerBuilder => integerBuilder
                            .SetHintText(uw3_hint))
                        .AddInteger("UW4", uw4_text,15,75, new ProxyRef<int>(() => UW_MaxPeopleInWedding, o => UW_MaxPeopleInWedding = o), integerBuilder => integerBuilder
                            .SetHintText(uw4_hint))
                        .AddInteger("UW5", uw5_text,50,200, new ProxyRef<int>(() => UW_EmbarrassedSoliderMaxGold, o => UW_EmbarrassedSoliderMaxGold = o), integerBuilder => integerBuilder
                            .SetHintText(uw5_hint))
                        .AddInteger("UW6", uw6_text,250,1500, new ProxyRef<int>(() => UW_MinGoldRaided, o => UW_MinGoldRaided = o), integerBuilder => integerBuilder
                            .SetHintText(uw6_hint))
                        .AddInteger("UW7", uw7_text,250,1500, new ProxyRef<int>(() => UW_MaxGoldRaided, o => UW_MaxGoldRaided = o), integerBuilder => integerBuilder
                            .SetHintText(uw7_hint))
                        .AddBool("UW8", uw8_text, new ProxyRef<bool>(() => UW_Disable, o => UW_Disable = o), boolBuilder => boolBuilder
                            .SetHintText(uw8_hint))
                    )
                            
                #endregion
                
                #region Undercooked - Builder
                
                .CreateGroup(uc_heading, groupBuilder => groupBuilder
                    .AddInteger("UC1", uc1_text,5,50, new ProxyRef<int>(() => UC_MinSoldiersToInjure, o => UC_MinSoldiersToInjure = o), integerBuilder => integerBuilder
                        .SetHintText(uc1_hint))
                    .AddInteger("UC2", uc2_text,5,50, new ProxyRef<int>(() => UC_MaxSoldiersToInjure, o => UC_MaxSoldiersToInjure = o), integerBuilder => integerBuilder
                        .SetHintText(uc2_hint))
                    .AddBool("UC3", uc3_text, new ProxyRef<bool>(() => UC_Disable, o => UC_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(uc3_hint))
                    )
                

                #endregion
                
                #region Violated Girl -  Builder
                
                .CreateGroup(vg_heading, groupBuilder => groupBuilder
                        .AddInteger("VG1", vg1_text,250,7500, new ProxyRef<int>(() => VG_MinCompensation, o => VG_MinCompensation = o), integerBuilder => integerBuilder
                            .SetHintText(vg1_hint))
                        .AddInteger("VG2", vg2_text,250,7500, new ProxyRef<int>(() => VG_MaxCompensation, o => VG_MaxCompensation = o), integerBuilder => integerBuilder
                            .SetHintText(vg2_hint))
                        .AddBool("VG3", vg3_text, new ProxyRef<bool>(() => VG_Disable, o => VG_Disable = o), boolBuilder => boolBuilder
                            .SetHintText(vg3_hint))
                    )
                

                #endregion
                
                #region Wandering Livestock -  Builder
                
                .CreateGroup(wl_heading, groupBuilder => groupBuilder
                        .AddInteger("WL1", wl1_text,5,20, new ProxyRef<int>(() => WL_MinFood, o => WL_MinFood = o), integerBuilder => integerBuilder
                            .SetHintText(wl1_hint))
                        .AddInteger("WL2", wl2_text,5,20, new ProxyRef<int>(() => WL_MaxFood, o => WL_MaxFood = o), integerBuilder => integerBuilder
                            .SetHintText(wl2_hint))
                        .AddBool("WL3", wl3_text, new ProxyRef<bool>(() => WL_Disable, o => WL_Disable = o), boolBuilder => boolBuilder
                            .SetHintText(wl3_hint))


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
            
            #region Passing Comet
            
            Instance.PC_Disable = false;

            #endregion
            
            #region Perfect Weather

            Instance.PW_Disable = false;
            Instance.PW_MinMoraleGain = 10;
            Instance.PW_MaxMoraleGain = 25;

            #endregion
            
            #region Prisoner Rebellion

            Instance.PR_Disable = false;
            Instance.PR_MinPrisoners = 30;

            #endregion
            
            #region Prisoner Transfer

            Instance.PT_Disable = true;
            Instance.PT_MinPrisoners = 10;
            Instance.PT_MaxPrisoners = 50;
            Instance.PT_MinPricePrPrisoner = 50;
            Instance.PT_MaxPricePrPrisoner = 400;

            #endregion
            
            #region Red Moon

            Instance.RM_Disable = false;
            Instance.RM_MinGoldLost = 700;
            Instance.RM_MaxGoldLost = 4000;
            Instance.RM_MinMenLost = 15;
            Instance.RM_MaxMenLost = 50;

            #endregion
            
            #region Refugees
            
            Instance.RF_Disable = false;
            Instance.RF_minSoldiers = 8;
            Instance.RF_maxSoldiers = 15;
            Instance.RF_minFood = 3;
            Instance.RF_maxFood = 5;
            Instance.RF_minCaptive = 8;
            Instance.RF_maxCaptive = 15;

            #endregion
            
            #region Robbery

            Instance.RO_Disable = false;
            Instance.RO_MinGoldLost = 500;
            Instance.RO_MaxGoldLost = 5000;
            Instance.RO_MinRenownLost = 10;
            Instance.RO_MaxRenownLost = 150;

            #endregion
            
            #region Runaway Son

            Instance.RS_Disable = false;
            Instance.RS_MinGoldGained = 50;
            Instance.RS_MaxGoldGained = 150;

            #endregion
            
            #region Secret Singer

            Instance.SS_Disable = false;
            Instance.SS_MinMoraleGained = 10;
            Instance.SS_MaxMoraleGained = 75;

            #endregion
            
            #region Speedy Recovery

            Instance.SR_Disable = false;
            Instance.SR_MinMenToRecover = 5;
            Instance.SR_MaxMenToRecover = 25;

            #endregion
            
            #region Successful Deeds

            Instance.SD_Disable = false;
            Instance.SD_MinInfluenceGained = 20;
            Instance.SD_MaxInfluenceGained = 100;

            #endregion
            
            #region Sudden Storm

            Instance.SuS_Disable = false;
            Instance.SuS_MinHorsesLost = 5;
            Instance.SuS_MaxHorsesLost = 12;
            Instance.SuS_MinMenDied = 10;
            Instance.SuS_MaxMenDied = 20;
            Instance.SuS_MinMenWounded = 10;
            Instance.SuS_MaxMenWounded = 25;
            Instance.SuS_MinMeatFromHorse = 4;
            Instance.SuS_MaxMeatFromHorse = 9;

            #endregion
            
            #region Supernatural Encounter
            
            Instance.SE_Disable = false;

            #endregion
            
            #region Target Practice

            Instance.TP_Disable = false;
            Instance.TP_MinSoldiers = 50;
            Instance.TP_PercentageDifferenceOfCurrentTroop = 0.5f;

            #endregion
            
            #region Travelling Merchant

            Instance.TM_Disable = false;
            Instance.TM_minloot = 1000;
            Instance.TM_maxloot = 6000;

            #endregion

            
            #region Unexpected Wedding

            Instance.UW_Disable = false;
            Instance.UW_MinGoldToDonate = 200;
            Instance.UW_MaxGoldToDonate = 750;
            Instance.UW_MinPeopleInWedding = 20;
            Instance.UW_MaxPeopleInWedding = 50;
            Instance.UW_EmbarrassedSoliderMaxGold = 150;
            Instance.UW_MinGoldRaided = 500;
            Instance.UW_MaxGoldRaided = 1250;

            #endregion
            
            #region Undercooked

            Instance.UC_Disable = false;
            Instance.UC_MinSoldiersToInjure = 8;
            Instance.UC_MaxSoldiersToInjure = 30;

            #endregion
            
            #region Violated Girl

            Instance.VG_Disable = false;
            Instance.VG_MinCompensation = 2000;
            Instance.VG_MaxCompensation = 5000;

            #endregion
            
            #region Wandering Livestock

            Instance.WL_Disable = false;
            Instance.WL_MinFood = 10;
            Instance.WL_MaxFood = 20;

            #endregion
            
            #endregion
        }
        

        public void Dispose()
        {
            //NA
        }
    }
}