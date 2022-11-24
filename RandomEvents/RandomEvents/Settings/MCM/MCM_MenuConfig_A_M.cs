using System;
using MCM.Abstractions.Base.Global;
using MCM.Abstractions.FluentBuilder;
using MCM.Common;
using TaleWorlds.Localization;

namespace CryingBuffalo.RandomEvents.Settings.MCM
{
    public class MCM_MenuConfig_A_M : IDisposable
    {
        private static MCM_MenuConfig_A_M _instance;

        private FluentGlobalSettings globalSettings;

        #region Variables

        #region A Flirtatious Encounter - Variables
        
        public bool AFE_Disable { get; private set; }
        public int AFE_minWomanAge { get; private set; }
        public int AFE_maxWomanAge { get; private set; }
        public float AFE_minRelationshipIncrease { get; private set; }
        public float AFE_maxRelationshipIncrease { get; private set; }

        #endregion

        #region Army Games - Variables

        public bool AG_Disable { get; private set; }
        public float AG_CohesionGain { get; private set; }
        public int AG_minMoraleGain { get; private set; }
        public int AG_maxMoraleGain { get; private set; }

        #endregion

        #region Ahead of Time - Variables

        public bool AoT_Disable { get; private set; }

        #endregion

        #region Bandit Ambush - Variables
        
        public bool BA_Disable { get; private set; }
        public float BA_MoneyMinPercent { get; private set; }
        public float BA_MoneyMaxPercent { get; private set; }
        public int BA_TroopScareCount { get; private set; }
        public int BA_BanditCap { get; private set; }
        
        #endregion
        
        #region Bee Kind - Variables
        
        public bool BK_Disable { get; private set; }
        public int BK_damage { get; private set; }
        public float BK_Reaction_Chance { get; private set; }
        public int BK_Add_Damage { get; private set; }
        
        #endregion
        
        #region Bet Money - Variables
        
        public bool BM_Disable { get; private set; }
        public float BM_Money_Percent { get; private set; }
        
        #endregion

        #region Beggar Begging - Variables
        
        public bool BB_Disable { get; private set; }
        public int BB_MinGoldToBeggar { get; private set; }
        public int BB_MaxGoldToBeggar { get; private set; }
        public int BB_MinRenownGain { get; private set; }
        public int BB_MaxRenownGain { get; private set; }
        
        #endregion

        #region BirdSongs - Variables

        public bool BS_Disable { get; private set; }
        public int BS_minMoraleGain { get; private set; }
        public int BS_maxMoraleGain { get; private set; }

        #endregion
        
        #region Birthday Party - Variables
        
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
        
        #endregion
        
        #region Bottoms Up - Variables
        
        public bool BU_Disable { get; private set; }
        public int BU_minMoraleGain { get; private set; }
        public int BU_maxMoraleGain { get; private set; }
        public int BU_minGold { get; private set; }
        public int BU_maxGold { get; private set; }

        #endregion
        
        #region Bumper Crop - Variables
        
        public bool BC_Disable { get; private set; }
        public float BC_CropGainPercent { get; private set; }
        
        #endregion
        
        #region Bunch of Prisoners - Variables
        
        public bool BoP_Disable { get; private set; }
        public int BoP_MinPrisonerGain { get; private set; }
        public int BoP_MaxPrisonerGain { get; private set; }
        
        #endregion

        #region Chatting Commanders - Variables
        
        public bool CC_Disable { get; private set; }
        public float CC_CohesionGain { get; private set; }
        
        #endregion
        
        #region Courier - Variables

        public bool CR_Disable { get; private set; }
        public int CR_minMoraleGain { get; private set; }
        public int CR_maxMoraleGain { get; private set; }
        #endregion

        #region Diseased City - Variables

        public bool DC_Disable { get; private set; }
        public float DC_BaseSuccessChance{ get; private set; }
        public float DC_HighMedicineChance{ get; private set; }
        public int DC_HighMedicineLevel{ get; private set; }
        public float DC_PercentLoss{ get; private set; }

        #endregion
        
        #region Dreaded Sweats - Variables

        public bool DS_Disable { get; private set; }
        public int DS_minMoraleLoss { get; private set; }
        public int DS_maxMoraleLoss { get; private set; }
        public int DS_minvictim { get; private set; }
        public int DS_maxvictim { get; private set; }

        #endregion
        
        #region Dysentery - Variables

        public bool DY_Disable { get; private set; }
        public int DY_minMoraleLoss { get; private set; }
        public int DY_maxMoraleLoss { get; private set; }
        public int DY_minvictim { get; private set; }
        public int DY_maxvictim { get; private set; }

        #endregion
        
        #region Eager Troops - Variables

        public bool ET_Disable { get; private set; }
        public int ET_MinTroopGain{ get; private set; }
        public int ET_MaxTroopGain{ get; private set; }

        #endregion
        
        #region ExoticDrinks - Variables

        public bool ED_Disable { get; private set; }
        public int ED_MinPrice{ get; private set; }
        public int ED_MaxPrice{ get; private set; }

        #endregion

        #region Fallen Soldier Family - Variables

        public bool FSF_Disable { get; private set; }
        public int FSF_MinFamilyCompensation { get; private set; }
        public int FSF_MaxFamilyCompensation { get; private set; }
        public int FSF_MinGoldLooted { get; private set; }
        public int FSF_MaxGoldLooted { get; private set; }

        #endregion
        
        #region Fantastic Fighters - Variables
        
        public bool FF_Disable { get; private set; }
        public int FF_MinRenownGain { get; private set; }
        public int FF_MaxRenownGain { get; private set; }
        
        #endregion

        #region Fishing Spot - Variables

        public bool FS_Disable { get; private set; }
        public int FS_MinSoldiersToGo { get; private set; }
        public int FS_MaxSoldiersToGo { get; private set; }
        public int FS_MaxFishCatch { get; private set; }
        public int FS_MinMoraleGain { get; private set; }
        public int FS_MaxMoraleGain { get; private set; }

        #endregion

        #region Food Fight - Variables

        public bool FoF_Disable { get; private set; }
        public int FoF_MinFoodLoss { get; private set; }
        public int FoF_MaxFoodLoss { get; private set; }
        public int FoF_MinMoraleLoss { get; private set; }
        public int FoF_MaxMoraleLoss { get; private set; }

        #endregion
        
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

        #endregion


        public static MCM_MenuConfig_A_M Instance
        {
            get { return _instance ??= new MCM_MenuConfig_A_M(); }
        }

        public void Settings()
        {
            

            #region Strings

            #region A Flirtatious Encounter - Strings
            
            var afe_heading = new TextObject("{=mcm_afe_heading}A Flirtatious Encounter").ToString();
            var afe1_text = new TextObject("{=mcm_afe1_text}1. Min Age").ToString();
            var afe1_hint = new TextObject("{=mcm_afe1_hint}The minimum age the target must be.").ToString();
            var afe2_text = new TextObject("{=mcm_afe2_text}2. Max Age").ToString();
            var afe2_hint = new TextObject("{=mcm_afe2_hint}The maximum age the target must be.").ToString();
            var afe3_text = new TextObject("{=mcm_afe3_text}3. Min Relationship Gain").ToString();
            var afe3_hint = new TextObject("{=mcm_afe3_hint}The minimum amount of relationship increase.").ToString();
            var afe4_text = new TextObject("{=mcm_afe4_text}4. Max Relationship Gain").ToString();
            var afe4_hint = new TextObject("{=mcm_afe4_hint}The maximum amount of relationship increase.").ToString();
            var afe5_text = new TextObject("{=mcm_afe5_text}5. Deactivate event").ToString();
            var afe5_hint = new TextObject("{=mcm_afe5_hint}If you dont want this event to show up you can deactivate it.").ToString();

            #endregion

            #region Army games - Strings

            var ag_heading = new TextObject("{=mcm_ag_heading}Army Games").ToString();
            var ag1_text = new TextObject("{=mcm_ag1_text}1. Crop % gain").ToString();
            var ag1_hint = new TextObject("{=mcm_ag1_hint}The amount of % the cohesion is increased.").ToString();
            var ag2_text = new TextObject("{=mcm_ag1_text}1. Min amount of morale gain").ToString();
            var ag2_hint = new TextObject("{=mcm_ag1_hint}The minimum amount of morale the party gains.").ToString();
            var ag3_text = new TextObject("{=mcm_ag2_text}2. Max amount of morale gain").ToString();
            var ag3_hint = new TextObject("{=mcm_ag2_hint}The maximum amount of morale the party gains.").ToString();
            var ag4_text = new TextObject("{=mcm_ag3_text}4. Deactivate event").ToString();
            var ag4_hint = new TextObject("{=mcm_ag3_hint}If you dont want this event to show up you can deactivate it.").ToString();

            #endregion

            #region Ahead of Time - Strings

            var aot_heading = new TextObject("{=mcm_aot_heading}Ahead of Time").ToString();
            var aot1_text = new TextObject("{=mcm_aot1_text}1. Deactivate event").ToString();
            var aot1_hint = new TextObject("{=mcm_aot1_hint}If you dont want this event to show up you can deactivate it.").ToString();
            
            #endregion
            
            #region Bandit Ambush - Strings
            
            var ba_heading = new TextObject("{=mcm_ba_heading}Bandit Ambush").ToString();
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
            
            #endregion
            
            #region Bee Kind - Strings
            
            var bk_heading = new TextObject("{=mcm_bk_heading}Bee Kind").ToString();
            var bk1_text = new TextObject("{=mcm_bk1_text}1. Damage to inflict").ToString();
            var bk1_hint = new TextObject("{=mcm_bk1_hint}The amount of damage the player gets.").ToString();
            var bk2_text = new TextObject("{=mcm_bk2_text}2. Reaction chance").ToString();
            var bk2_hint = new TextObject("{=mcm_bk2_hint}The chance in % that the player manages to react.").ToString();
            var bk3_text = new TextObject("{=mcm_bk3_text}3. Additional damage").ToString();
            var bk3_hint = new TextObject("{=mcm_bk3_hint}The amount of additional damage the player gets if they fail to react.").ToString();
            var bk4_text = new TextObject("{=mcm_bk4_text}4. Deactivate event").ToString();
            var bk4_hint = new TextObject("{=mcm_bk4_hint}If you dont want this event to show up you can deactivate it.").ToString();
            
            #endregion
            
            #region Bet Money - Strings
            
            var bm_heading = new TextObject("{=mcm_bm_heading}Bet Money").ToString();
            var bm1_text = new TextObject("{=mcm_bm1_text}1. Percent of money to bet").ToString();
            var bm1_hint = new TextObject("{=mcm_bm1_hint}The amount of money in percent to bet.").ToString();
            var bm2_text = new TextObject("{=mcm_bm2_text}2. Deactivate event").ToString();
            var bm2_hint = new TextObject("{=mcm_bm2_hint}If you dont want this event to show up you can deactivate it.").ToString();
            
            #endregion
            
            #region Beggar Begging - Strings
            
            var bb_heading = new TextObject("{=mcm_bb_heading}Beggar Begging").ToString();
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
            
            #endregion
            
            #region Birdsong - Strings
            
            var bs_heading = new TextObject("{=mcm_bs_heading}BirdSong").ToString();
            var bs1_text = new TextObject("{=mcm_bs1_text}1. Min amount of morale gain").ToString();
            var bs1_hint = new TextObject("{=mcm_bs1_hint}The minimum amount of morale the party gains.").ToString();
            var bs2_text = new TextObject("{=mcm_bs2_text}2. Max amount of morale gain").ToString();
            var bs2_hint = new TextObject("{=mcm_bs2_hint}The maximum amount of morale the party gains.").ToString();
            var bs3_text = new TextObject("{=mcm_bs3_text}3. Deactivate event").ToString();
            var bs3_hint = new TextObject("{=mcm_bs3_hint}If you dont want this event to show up you can deactivate it.").ToString();
            
            #endregion
            
            #region Birthday Party - Strings
            
            var bp_heading = new TextObject("{=mcm_bp_heading}Birthday Party").ToString();
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
            
            #endregion
            
            #region Bottoms Up - Strings
            
            var bu_heading = new TextObject("{=mcm_bu_heading}Bottoms Up").ToString();
            var bu1_text = new TextObject("{=mcm_bu_text}1.Min Morale Gain").ToString();
            var bu1_hint = new TextObject("{=mcm_bu_text}The minimum morale gain.").ToString();
            var bu2_text = new TextObject("{=mcm_bu_text}2.Max Morale Gain").ToString();
            var bu2_hint = new TextObject("{=mcm_bu_text}The maximum morale gain.").ToString();
            var bu3_text = new TextObject("{=mcm_bu_text}3.Min gold").ToString();
            var bu3_hint = new TextObject("{=mcm_bu_text}The minimum gold reward.").ToString();
            var bu4_text = new TextObject("{=mcm_bu_text}4.Max gold").ToString();
            var bu4_hint = new TextObject("{=mcm_bu_text}The maximum gold reward.").ToString();
            var bu5_text = new TextObject("{=mcm_bu_text}3. Deactivate event").ToString();
            var bu5_hint = new TextObject("{=mcm_bu_text}If you dont want this event to show up you can deactivate it.").ToString();
            
            #endregion
            
            #region Bumper Crop - Strings
            
            var bc_heading = new TextObject("{=mcm_bc_heading}Bumper Crop").ToString();
            var bc1_text = new TextObject("{=mcm_bc1_text}1. Crop % gain").ToString();
            var bc1_hint = new TextObject("{=mcm_bc1_hint}The amount of % the crop yield is increased.").ToString();
            var bc2_text = new TextObject("{=mcm_bc2_text}2. Deactivate event").ToString();
            var bc2_hint = new TextObject("{=mcm_bc2_hint}If you dont want this event to show up you can deactivate it.").ToString();
            
            #endregion
            
            #region Bunch of Prisoners - Strings
            
            var bop_heading = new TextObject("{=mcm_bop_heading}Bumper Crop").ToString();
            var bop1_text = new TextObject("{=mcm_bop1_text}1. Min amount of prisoners").ToString();
            var bop1_hint = new TextObject("{=mcm_bop1_hint}The minimum amount of prisoners the settlement gains.").ToString();
            var bop2_text = new TextObject("{=mcm_bop2_text}2. Max amount of prisoners").ToString();
            var bop2_hint = new TextObject("{=mcm_bop2_hint}The maximum amount of prisoners the settlement gains.").ToString();
            var bop3_text = new TextObject("{=mcm_bop3_text}3. Deactivate event").ToString();
            var bop3_hint = new TextObject("{=mcm_bop3_hint}If you dont want this event to show up you can deactivate it.").ToString();
            
            #endregion
            
            #region Chatting Commanders - Strings
            
            var cc_heading = new TextObject("{=mcm_cc_heading}Chatting Commanders").ToString();
            var cc1_text = new TextObject("{=mcm_cc1_text}1. Crop % gain").ToString();
            var cc1_hint = new TextObject("{=mcm_cc1_hint}The amount of % the cohesion is increased.").ToString();
            var cc2_text = new TextObject("{=mcm_cc2_text}2. Deactivate event").ToString();
            var cc2_hint = new TextObject("{=mcm_cc2_hint}If you dont want this event to show up you can deactivate it.").ToString();
            
            #endregion
            
            #region Courier - Strings

            var cr_heading = new TextObject("{=mcm_cr_heading}Courier").ToString();
            var cr1_text = new TextObject("{=mcm_bu_text}1.Min Morale Gain").ToString();
            var cr1_hint = new TextObject("{=mcm_bu_text}The minimum morale gain.").ToString();
            var cr2_text = new TextObject("{=mcm_bu_text}2.Max Morale Gain").ToString();
            var cr2_hint = new TextObject("{=mcm_bu_text}The maximum morale gain.").ToString();
            var cr3_text = new TextObject("{=mcm_bu_text}3. Deactivate event").ToString();
            var cr3_hint = new TextObject("{=mcm_bu_text}If you dont want this event to show up you can deactivate it.").ToString();

            #endregion

            #region Diseased City - Strings

            var dc_heading = new TextObject("{=mcm_dc_heading}Diseased City").ToString();
            var dc1_text = new TextObject("{=mcm_dc1_text}1. Base Success Chance").ToString();
            var dc1_hint = new TextObject("{=mcm_dc1_hint}The base success chance that this event will have a positive outcome.").ToString();
            var dc2_text = new TextObject("{=mcm_dc2_text}2. High Medicine Chance").ToString();
            var dc3_text = new TextObject("{=mcm_dc3_text}3. High Medicine Level").ToString();
            var dc4_text = new TextObject("{=mcm_dc4_text}4. Percentage Lost").ToString();
            var dc4_hint = new TextObject("{=mcm_dc4_hint}The amount of men lost at the stricken settlement.").ToString();
            var dc5_text = new TextObject("{=mcm_dc5_text}5. Deactivate event").ToString();
            var dc5_hint = new TextObject("{=mcm_dc5_hint}If you dont want this event to show up you can deactivate it.").ToString();

            #endregion
            
            #region Dreaded Sweats - Strings
            
            var ds_heading = new TextObject("{=mcm_ds_heading}Dreaded Sweats").ToString();
            var ds1_text = new TextObject("{=mcm_ds_text}1.Min Morale Loss").ToString();
            var ds1_hint = new TextObject("{=mcm_ds_text}The minimum morale loss.").ToString();
            var ds2_text = new TextObject("{=mcm_ds_text}2.Max Morale Loss").ToString();
            var ds2_hint = new TextObject("{=mcm_ds_text}The maximum morale loss.").ToString();
            var ds3_text = new TextObject("{=mcm_ds_text}3.Min victims").ToString();
            var ds3_hint = new TextObject("{=mcm_ds_text}The minimum amount of victims.").ToString();
            var ds4_text = new TextObject("{=mcm_ds_text}4.Max victims").ToString();
            var ds4_hint = new TextObject("{=mcm_ds_text}The maximum amount of victims.").ToString();
            var ds5_text = new TextObject("{=mcm_ds_text}3. Deactivate event").ToString();
            var ds5_hint = new TextObject("{=mcm_ds_text}If you dont want this event to show up you can deactivate it.").ToString();
            
            #endregion
            
            #region Dysentery - Strings
            
            var dy_heading = new TextObject("{=mcm_dy_heading}Dysentery").ToString();
            var dy1_text = new TextObject("{=mcm_dy_text}1.Min Morale Loss").ToString();
            var dy1_hint = new TextObject("{=mcm_dy_text}The minimum morale loss.").ToString();
            var dy2_text = new TextObject("{=mcm_dy_text}2.Max Morale Loss").ToString();
            var dy2_hint = new TextObject("{=mcm_dy_text}The maximum morale loss.").ToString();
            var dy3_text = new TextObject("{=mcm_dy_text}3.Min victims").ToString();
            var dy3_hint = new TextObject("{=mcm_dy_text}The minimum amount of victims.").ToString();
            var dy4_text = new TextObject("{=mcm_dy_text}4.Max victims").ToString();
            var dy4_hint = new TextObject("{=mcm_dy_text}The maximum amount of victims.").ToString();
            var dy5_text = new TextObject("{=mcm_dy_text}3. Deactivate event").ToString();
            var dy5_hint = new TextObject("{=mcm_dy_text}If you dont want this event to show up you can deactivate it.").ToString();

            #endregion
            
            #region Eager Troops - Strings

            var et_heading = new TextObject("{=mcm_et_heading}Eager Troops").ToString();
            var et1_text = new TextObject("{=mcm_et1_text}1. Min Troops Gained").ToString();
            var et1_hint = new TextObject("{=mcm_et1_hint}Minimum amount of troops gained.").ToString();
            var et2_text = new TextObject("{=mcm_et2_text}2. Max Troops Gained").ToString();
            var et2_hint = new TextObject("{=mcm_et2_hint}Maximum amount of troops gained.").ToString();
            var et3_text = new TextObject("{=mcm_et3_text}3. Deactivate event").ToString();
            var et3_hint = new TextObject("{=mcm_et3_hint}If you dont want this event to show up you can deactivate it.").ToString();

            #endregion
            
            #region Exotic Drinks - Strings

            var ed_heading = new TextObject("{=mcm_ed_heading}Exotic Drinks").ToString();
            var ed1_text = new TextObject("{=mcm_ed1_text}1. Min Price").ToString();
            var ed1_hint = new TextObject("{=mcm_ed1_hint}Minimum price for the drink.").ToString();
            var ed2_text = new TextObject("{=mcm_ed2_text}2. Max Price").ToString();
            var ed2_hint = new TextObject("{=mcm_ed2_hint}Maximum price for the drink.").ToString();
            var ed3_text = new TextObject("{=mcm_ed3_text}3. Deactivate event").ToString();
            var ed3_hint = new TextObject("{=mcm_ed3_hint}If you dont want this event to show up you can deactivate it.").ToString();

            #endregion
            
            #region Fallen Soldier Family - Strings
            
            var fsf_heading = new TextObject("{=mcm_fsf_heading}Fallen Soldier Family").ToString();
            var fsf1_text = new TextObject("{=mcm_fsf1_text}1. Min Compensation").ToString();
            var fsf1_hint = new TextObject("{=mcm_fsf1_hint}Minimum amount compensation to the family.").ToString();
            var fsf2_text = new TextObject("{=mcm_fsf2_text}2. Max Compensation").ToString();
            var fsf2_hint = new TextObject("{=mcm_fsf2_hint}Maximum amount compensation to the family").ToString();
            var fsf3_text = new TextObject("{=mcm_fsf3_text}3. Min Loot").ToString();
            var fsf3_hint = new TextObject("{=mcm_fsf3_hint}Minimum amount of gold looted.").ToString();
            var fsf4_text = new TextObject("{=mcm_fsf4_text}4. Max Loot").ToString();
            var fsf4_hint = new TextObject("{=mcm_fsf4_hint}Maximum amount of gold looted.").ToString();
            var fsf5_text = new TextObject("{=mcm_fsf5_text}5. Deactivate event").ToString();
            var fsf5_hint = new TextObject("{=mcm_fsf5_hint}If you dont want this event to show up you can deactivate it.").ToString();
            
            #endregion
            
            #region Fantastic Fighters - Strings

            var ff_heading = new TextObject("{=mcm_ff_heading}Fantastic Fighters").ToString();
            var ff1_text = new TextObject("{=mcm_ff1_text}1. Min Renown Gained").ToString();
            var ff1_hint = new TextObject("{=mcm_ff1_hint}Minimum amount of renown gained.").ToString();
            var ff2_text = new TextObject("{=mcm_ff2_text}2. Max Renown Gained").ToString();
            var ff2_hint = new TextObject("{=mcm_ff2_hint}Maximum amount of renown gained.").ToString();
            var ff3_text = new TextObject("{=mcm_ff3_text}3. Deactivate event").ToString();
            var ff3_hint = new TextObject("{=mcm_ff3_hint}If you dont want this event to show up you can deactivate it.").ToString();

            #endregion
            
            #region Fishing Spot - Strings

            var fs_heading = new TextObject("{=mcm_fs_heading}Fishing Spot").ToString();
            var fs1_text = new TextObject("{=mcm_fs1_text}1. Min Soldiers").ToString();
            var fs1_hint = new TextObject("{=mcm_fs1_hint}Minimum soldiers to go fishing.").ToString();
            var fs2_text = new TextObject("{=mcm_fs2_text}2. Max Soldiers").ToString();
            var fs2_hint = new TextObject("{=mcm_fs2_hint}Maximum soldiers to go fishing.").ToString();
            var fs3_text = new TextObject("{=mcm_fs3_text}3. Max Fish Catch").ToString();
            var fs3_hint = new TextObject("{=mcm_fs3_hint}Minimum amount of fish to catch.").ToString();
            var fs4_text = new TextObject("{=mcm_fs4_text}4. Min Morale Gained").ToString();
            var fs4_hint = new TextObject("{=mcm_fs4_hint}Minimum amount of morale gained.").ToString();
            var fs5_text = new TextObject("{=mcm_fs5_text}5. Max Morale Gained").ToString();
            var fs5_hint = new TextObject("{=mcm_fs5_hint}Maximum amount of morale gained.").ToString();
            var fs6_text = new TextObject("{=mcm_fs6_text}6. Deactivate event").ToString();
            var fs6_hint = new TextObject("{=mcm_fs6_hint}If you dont want this event to show up you can deactivate it.").ToString();

            #endregion
            
            #region Food Fight - Strings

            var fof_heading = new TextObject("{=mcm_fof_heading}Food Fight").ToString();
            var fof1_text = new TextObject("{=mcm_fof1_text}1. Min Soldiers").ToString();
            var fof1_hint = new TextObject("{=mcm_fof1_hint}Minimum soldiers to go fishing.").ToString();
            var fof2_text = new TextObject("{=mcm_fof2_text}2. Max Soldiers").ToString();
            var fof2_hint = new TextObject("{=mcm_fof2_hint}Maximum soldiers to go fishing.").ToString();
            var fof3_text = new TextObject("{=mcm_fof3_text}3. Max Fish Catch").ToString();
            var fof3_hint = new TextObject("{=mcm_fof3_hint}Minimum amount of fish to catch.").ToString();
            var fof4_text = new TextObject("{=mcm_fof4_text}4. Min Morale Gained").ToString();
            var fof4_hint = new TextObject("{=mcm_fof4_hint}Minimum amount of morale gained.").ToString();
            var fof5_text = new TextObject("{=mcm_fof5_text}5. Deactivate event").ToString();
            var fof5_hint = new TextObject("{=mcm_fof5_hint}If you dont want this event to show up you can deactivate it.").ToString();

            #endregion
            
            #region Granary Rats - Strings
            
            var gr_heading = new TextObject("{=mcm_gr_heading}Bee Kind").ToString();
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

            #endregion
            
            
            
            var builder = BaseSettingsBuilder.Create("RandomEvents1","1. Random Events - A to M")!
                .SetFormat("xml")
                .SetFolderName(RandomEventsSubmodule.FolderName)
                .SetSubFolder(RandomEventsSubmodule.ModName)
                
           #region Builder Modules

                #region A Flirtatious Encounter - Builder
                
                .CreateGroup(afe_heading, groupBuilder => groupBuilder
                    .AddInteger("AFE1", afe1_text,18,50, new ProxyRef<int>(() => AFE_minWomanAge, o => AFE_minWomanAge = o), integerBuilder => integerBuilder
                        .SetHintText(afe1_hint))
                    .AddInteger("AFE2", afe2_text,18,50, new ProxyRef<int>(() => AFE_maxWomanAge, o => AFE_maxWomanAge = o), integerBuilder => integerBuilder
                        .SetHintText(afe2_hint))
                    .AddFloatingInteger("AFE3", afe3_text,3,65, new ProxyRef<float>(() => AFE_minRelationshipIncrease, o => AFE_minRelationshipIncrease = o), floatBuilder => floatBuilder
                        .SetHintText(afe3_hint))
                    .AddFloatingInteger("AFE4", afe4_text,3,65, new ProxyRef<float>(() => AFE_maxRelationshipIncrease, o => AFE_maxRelationshipIncrease = o), floatBuilder => floatBuilder
                        .SetHintText(afe4_hint))
                    .AddBool("AFE7", afe5_text, new ProxyRef<bool>(() => AFE_Disable, o => AFE_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(afe5_hint))
                )

            #endregion

                #region Army Games - Builder

               .CreateGroup(ag_heading, groupBuilder => groupBuilder
                    .AddFloatingInteger("AG1", ag1_text, 5, 100, new ProxyRef<float>(() => AG_CohesionGain, o => AG_CohesionGain = o), floatBuilder => floatBuilder
                        .SetHintText(ag1_hint))
                    .AddInteger("AG2", ag2_text, 1, 30, new ProxyRef<int>(() => AG_minMoraleGain, o => AG_minMoraleGain = o), integerBuilder => integerBuilder
                        .SetHintText(ag2_hint))
                    .AddInteger("AG3", ag3_text, 1, 30, new ProxyRef<int>(() => AG_maxMoraleGain, o => AG_maxMoraleGain = o), integerBuilder => integerBuilder
                        .SetHintText(ag3_hint))
                    .AddBool("AG4", ag4_text, new ProxyRef<bool>(() => AG_Disable, o => AG_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(ag4_hint))
                    )
                #endregion

                #region Ahead of Time - Builder

                .CreateGroup(aot_heading, groupBuilder => groupBuilder
                    .AddBool("AoT", aot1_text, new ProxyRef<bool>(() => AoT_Disable, o => AoT_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(aot1_hint))
                    )
                
                #endregion
                
                #region Bandit Ambush - Builder
                
                .CreateGroup(ba_heading, groupBuilder => groupBuilder
                    .AddFloatingInteger("BA1", ba1_text,0.01f,0.50f, new ProxyRef<float>(() => BA_MoneyMinPercent, o => BA_MoneyMinPercent = o), floatBuilder => floatBuilder
                        .SetHintText(ba1_hint))
                    .AddFloatingInteger("BA2", ba2_text,0.01f,0.90f, new ProxyRef<float>(() => BA_MoneyMaxPercent, o => BA_MoneyMaxPercent = o), floatBuilder => floatBuilder
                        .SetHintText(ba2_hint))
                    .AddInteger("BA3", ba3_text,25,100, new ProxyRef<int>(() => BA_TroopScareCount, o => BA_TroopScareCount = o), integerBuilder => integerBuilder
                        .SetHintText(ba3_hint))
                    .AddInteger("BA4", ba4_text,5,25, new ProxyRef<int>(() => BA_BanditCap, o => BA_BanditCap = o), integerBuilder => integerBuilder
                        .SetHintText(ba4_hint))
                    .AddBool("BA5", ba5_text, new ProxyRef<bool>(() => BA_Disable, o => BA_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(ba5_hint))
                )
                
                #endregion
                
                #region Bee Kind - Builder
                
                .CreateGroup(bk_heading, groupBuilder => groupBuilder
                    .AddInteger("BK1", bk1_text,5,50, new ProxyRef<int>(() => BK_damage, o => BK_damage = o), floatBuilder => floatBuilder
                        .SetHintText(bk1_hint))
                    .AddFloatingInteger("BK2", bk2_text,0.01f,0.50f, new ProxyRef<float>(() => BK_Reaction_Chance, o => BK_Reaction_Chance = o), floatBuilder => floatBuilder
                        .SetHintText(bk2_hint))
                    .AddInteger("BK3", bk3_text,10,25, new ProxyRef<int>(() => BK_Add_Damage, o => BK_Add_Damage = o), integerBuilder => integerBuilder
                        .SetHintText(bk3_hint))
                    .AddBool("BK4", bk4_text, new ProxyRef<bool>(() => BK_Disable, o => BK_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(bk4_hint))
                )
                
                #endregion
                
                #region Bet Money - Builder
                
                .CreateGroup(bm_heading, groupBuilder => groupBuilder
                    .AddFloatingInteger("BM1", bm1_text,0.01f,0.9f, new ProxyRef<float>(() => BM_Money_Percent, o => BM_Money_Percent = o), floatBuilder => floatBuilder
                        .SetHintText(bm1_hint))
                    .AddBool("BM2", bm2_text, new ProxyRef<bool>(() => BM_Disable, o => BM_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(bm2_hint))
                )
                
                #endregion
                
                #region Beggar Begging - Builder
                
                .CreateGroup(bb_heading, groupBuilder => groupBuilder
                .AddInteger("BB1", bb1_text,2,50, new ProxyRef<int>(() => BB_MinGoldToBeggar, o => BB_MinGoldToBeggar = o), integerBuilder => integerBuilder
                    .SetHintText(bb1_hint))
                .AddInteger("BB2", bb2_text,2,50, new ProxyRef<int>(() => BB_MaxGoldToBeggar, o => BB_MaxGoldToBeggar = o), integerBuilder => integerBuilder
                    .SetHintText(bb2_hint))
                .AddInteger("BB3", bb3_text,5,20, new ProxyRef<int>(() => BB_MinRenownGain, o => BB_MinRenownGain = o), integerBuilder => integerBuilder
                    .SetHintText(bb3_hint))
                .AddInteger("BB4", bb4_text,5,20, new ProxyRef<int>(() => BB_MaxRenownGain, o => BB_MaxRenownGain = o), integerBuilder => integerBuilder
                    .SetHintText(bb4_hint))
                .AddBool("BB5", bb5_text, new ProxyRef<bool>(() => BB_Disable, o => BB_Disable = o), boolBuilder => boolBuilder
                    .SetHintText(bb5_hint))
                )
                
                #endregion
                
                #region Bird Songs - Builder

                .CreateGroup(bs_heading, groupBuilder => groupBuilder
                    .AddInteger("BS1", bs1_text, 1, 30, new ProxyRef<int>(() => BS_minMoraleGain, o => BS_minMoraleGain = o), integerBuilder => integerBuilder
                        .SetHintText(bs1_hint))
                    .AddInteger("BS2", bs2_text, 1, 30, new ProxyRef<int>(() => BS_maxMoraleGain, o => BS_maxMoraleGain = o), integerBuilder => integerBuilder
                        .SetHintText(bs2_hint))
                    .AddBool("BS3", bs3_text, new ProxyRef<bool>(() => BS_Disable, o => BS_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(bs3_hint))

                )
                #endregion
                
                #region Birthday Party - Builder
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
                )
                #endregion
                
                #region Bottoms Up - Builder
                
                .CreateGroup(bu_heading, groupBuilder => groupBuilder
                    .AddInteger("BU1", bu1_text, 1, 30, new ProxyRef<int>(() => BU_minMoraleGain, o => BU_minMoraleGain = o), integerBuilder => integerBuilder
                        .SetHintText(bu1_hint))
                    .AddInteger("BU2", bu2_text, 1, 30, new ProxyRef<int>(() => BU_maxMoraleGain, o => BU_maxMoraleGain = o), integerBuilder => integerBuilder
                        .SetHintText(bu2_hint))
                    .AddInteger("BU3", bu3_text, 1, 30, new ProxyRef<int>(() => BU_minGold, o => BU_minGold = o), integerBuilder => integerBuilder
                        .SetHintText(bu3_hint))
                    .AddInteger("BU4", bu4_text, 1, 300, new ProxyRef<int>(() => BU_maxGold, o => BU_maxGold = o), integerBuilder => integerBuilder
                        .SetHintText(bu4_hint))
                    .AddBool("BU5", bu5_text, new ProxyRef<bool>(() => BU_Disable, o => BU_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(bu5_hint))
                )
                
                #endregion
                
                #region Bumper Crop - Builder
                
                .CreateGroup(bc_heading, groupBuilder => groupBuilder
                    .AddFloatingInteger("BC1", bc1_text,0.1f,0.90f, new ProxyRef<float>(() => BC_CropGainPercent, o => BC_CropGainPercent = o), floatBuilder => floatBuilder
                        .SetHintText(bc1_hint))
                    .AddBool("BC2", bc2_text, new ProxyRef<bool>(() => BC_Disable, o => BC_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(bc2_hint))
                )
                #endregion
                
                #region Bunch of Prisoners - Builder
                
                .CreateGroup(bop_heading, groupBuilder => groupBuilder
                    .AddInteger("BoP1", bop1_text,5,75, new ProxyRef<int>(() => BoP_MinPrisonerGain, o => BoP_MinPrisonerGain = o), integerBuilder => integerBuilder
                        .SetHintText(bop1_hint))
                    .AddInteger("BoP2", bop2_text,5, 75, new ProxyRef<int>(() => BoP_MaxPrisonerGain, o => BoP_MaxPrisonerGain = o), integerBuilder => integerBuilder
                        .SetHintText(bop2_hint))
                    .AddBool("BoP3", bop3_text, new ProxyRef<bool>(() => BoP_Disable, o => BoP_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(bop3_hint))
                    )
                #endregion
                
                #region Chatting Commanders - Builder
                
                .CreateGroup(cc_heading, groupBuilder => groupBuilder
                        .AddFloatingInteger("CC1", cc1_text,5,75, new ProxyRef<float>(() => CC_CohesionGain, o => CC_CohesionGain = o), floatBuilder => floatBuilder
                            .SetHintText(cc1_hint))
                        .AddBool("CC2", cc2_text, new ProxyRef<bool>(() => CC_Disable, o => CC_Disable = o), boolBuilder => boolBuilder
                            .SetHintText(cc2_hint))
                    )
                #endregion               
                
                #region Courier - Builder
                
                .CreateGroup(cr_heading, groupBuilder => groupBuilder
                    .AddInteger("CR1", cr1_text, 1, 30, new ProxyRef<int>(() => CR_minMoraleGain, o => CR_minMoraleGain = o), integerBuilder => integerBuilder
                        .SetHintText(cr1_hint))
                    .AddInteger("CR2", cr2_text, 1, 30, new ProxyRef<int>(() => CR_maxMoraleGain, o => CR_maxMoraleGain = o), integerBuilder => integerBuilder
                        .SetHintText(cr2_hint))
                    .AddBool("CR3", cr3_text, new ProxyRef<bool>(() => CR_Disable, o => CR_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(cr3_hint))
                )
                
                #endregion
                
                #region Diseased City - Builder
                
                .CreateGroup(dc_heading, groupBuilder => groupBuilder
                        .AddFloatingInteger("DC1", dc1_text,0.1f,0.75f, new ProxyRef<float>(() => DC_BaseSuccessChance, o => DC_BaseSuccessChance = o), floatBuilder => floatBuilder
                            .SetHintText(dc1_hint))
                        .AddFloatingInteger("DC2", dc2_text,0.25f,0.90f, new ProxyRef<float>(() => DC_HighMedicineChance, o => DC_HighMedicineChance = o), floatBuilder => floatBuilder
                            .SetHintText(""))
                        .AddInteger("DC3", dc3_text,40,90, new ProxyRef<int>(() => DC_HighMedicineLevel, o => DC_HighMedicineLevel = o), integerBuilder => integerBuilder
                            .SetHintText(""))
                        .AddFloatingInteger("DC4", dc4_text,0.1f,0.50f, new ProxyRef<float>(() => DC_PercentLoss, o => DC_PercentLoss = o), floatBuilder => floatBuilder
                            .SetHintText(dc4_hint))
                        .AddBool("DC5", dc5_text, new ProxyRef<bool>(() => DC_Disable, o => DC_Disable = o), boolBuilder => boolBuilder
                            .SetHintText(dc5_hint))
                    )
                    
                    #endregion
                
                #region Dreaded Sweats - Builder
                
                .CreateGroup(ds_heading, groupBuilder => groupBuilder
                    .AddInteger("DS1", ds1_text, 1, 30, new ProxyRef<int>(() => DS_minMoraleLoss, o => DS_minMoraleLoss = o), integerBuilder => integerBuilder
                        .SetHintText(ds1_hint))
                    .AddInteger("DS2", ds2_text, 1, 30, new ProxyRef<int>(() => DS_maxMoraleLoss, o => DS_maxMoraleLoss = o), integerBuilder => integerBuilder
                        .SetHintText(ds2_hint))
                    .AddInteger("DS3", ds3_text, 1, 30, new ProxyRef<int>(() => DS_minvictim, o => DS_minvictim = o), integerBuilder => integerBuilder
                        .SetHintText(ds3_hint))
                    .AddInteger("DS4", ds4_text, 1, 30, new ProxyRef<int>(() => DS_maxvictim, o => DS_maxvictim = o), integerBuilder => integerBuilder
                        .SetHintText(ds4_hint))
                    .AddBool("DS5", ds5_text, new ProxyRef<bool>(() => DS_Disable, o => DS_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(ds5_hint))
                )

                #endregion
                
                #region Dysentery - Builder
                
                .CreateGroup(dy_heading, groupBuilder => groupBuilder
                    .AddInteger("DY1", dy1_text, 1, 30, new ProxyRef<int>(() => DY_minMoraleLoss, o => DY_minMoraleLoss = o), integerBuilder => integerBuilder
                        .SetHintText(dy1_hint))
                    .AddInteger("DY2", dy2_text, 1, 30, new ProxyRef<int>(() => DY_maxMoraleLoss, o => DY_maxMoraleLoss = o), integerBuilder => integerBuilder
                        .SetHintText(dy2_hint))
                    .AddInteger("DY3", dy3_text, 1, 30, new ProxyRef<int>(() => DY_minvictim, o => DY_minvictim = o), integerBuilder => integerBuilder
                        .SetHintText(dy3_hint))
                    .AddInteger("DY4", dy4_text, 1, 30, new ProxyRef<int>(() => DY_maxvictim, o => DY_maxvictim = o), integerBuilder => integerBuilder
                        .SetHintText(dy4_hint))
                    .AddBool("DY5", dy5_text, new ProxyRef<bool>(() => DY_Disable, o => DY_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(dy5_hint))
                )

                #endregion
                
                #region Eager Troops - Builder
                
                .CreateGroup(et_heading, groupBuilder => groupBuilder
                        .AddInteger("ET1", et1_text,5,50, new ProxyRef<int>(() => ET_MinTroopGain, o => ET_MinTroopGain = o), integerBuilder => integerBuilder
                            .SetHintText(et1_hint))
                        .AddInteger("ET2", et2_text,5,50, new ProxyRef<int>(() => ET_MaxTroopGain, o => ET_MaxTroopGain = o), integerBuilder => integerBuilder
                            .SetHintText(et2_hint))
                        .AddBool("ET3", et3_text, new ProxyRef<bool>(() => ET_Disable, o => ET_Disable = o), boolBuilder => boolBuilder
                            .SetHintText(et3_hint))
                    )

                    #endregion
                
                #region Exotic Drinks - Builder
                
                .CreateGroup(ed_heading, groupBuilder => groupBuilder
                        .AddInteger("ED1", ed1_text,2500,7500, new ProxyRef<int>(() => ED_MinPrice, o => ED_MinPrice = o), integerBuilder => integerBuilder
                            .SetHintText(ed1_hint))
                        .AddInteger("ED2", ed2_text,2500,7500, new ProxyRef<int>(() => ED_MaxPrice, o => ED_MaxPrice = o), integerBuilder => integerBuilder
                            .SetHintText(ed2_hint))
                        .AddBool("ED3", ed3_text, new ProxyRef<bool>(() => ED_Disable, o => ED_Disable = o), boolBuilder => boolBuilder
                            .SetHintText(ed3_hint))
                    )

                #endregion
                
                #region Fallen Soldier Family - Builder
                
                .CreateGroup(fsf_heading, groupBuilder => groupBuilder
                        .AddInteger("FSF1", fsf1_text,500,2500, new ProxyRef<int>(() => FSF_MinFamilyCompensation, o => FSF_MinFamilyCompensation = o), integerBuilder => integerBuilder
                            .SetHintText(fsf1_hint))
                        .AddInteger("FSF2", fsf2_text,500,2500, new ProxyRef<int>(() => FSF_MaxFamilyCompensation, o => FSF_MaxFamilyCompensation = o), integerBuilder => integerBuilder
                            .SetHintText(fsf2_hint))
                        .AddInteger("FSF3", fsf3_text,500,1500, new ProxyRef<int>(() => FSF_MinGoldLooted, o => FSF_MinGoldLooted = o), integerBuilder => integerBuilder
                            .SetHintText(fsf3_hint))
                        .AddInteger("FSF4", fsf4_text,500,1500, new ProxyRef<int>(() => FSF_MaxGoldLooted, o => FSF_MaxGoldLooted = o), integerBuilder => integerBuilder
                            .SetHintText(fsf4_hint))
                        .AddBool("FSF5", fsf5_text, new ProxyRef<bool>(() => FSF_Disable, o => FSF_Disable = o), boolBuilder => boolBuilder
                            .SetHintText(fsf5_hint))
                    )

                #endregion
                
                #region Fantastic Fighters - Builder
                
                .CreateGroup(ff_heading, groupBuilder => groupBuilder
                    .AddInteger("FF1", ff1_text,10,100, new ProxyRef<int>(() => FF_MinRenownGain, o => FF_MinRenownGain = o), integerBuilder => integerBuilder
                        .SetHintText(ff1_hint))
                    .AddInteger("FF2", ff2_text,10,100, new ProxyRef<int>(() => FF_MaxRenownGain, o => FF_MaxRenownGain = o), integerBuilder => integerBuilder
                        .SetHintText(ff2_hint))
                    .AddBool("FF3", ff3_text, new ProxyRef<bool>(() => FF_Disable, o => FF_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(ff3_hint))
                    )

                #endregion
                
                #region Fishing Spot - Builder
                
                .CreateGroup(fs_heading, groupBuilder => groupBuilder
                    .AddInteger("FS1", fs1_text,2,15, new ProxyRef<int>(() => FS_MinSoldiersToGo, o => FS_MinSoldiersToGo = o), integerBuilder => integerBuilder
                        .SetHintText(fs1_hint))
                    .AddInteger("FS2", fs2_text,2,15, new ProxyRef<int>(() => FS_MaxSoldiersToGo, o => FS_MaxSoldiersToGo = o), integerBuilder => integerBuilder
                        .SetHintText(fs2_hint))
                    .AddInteger("FS3", fs3_text,10,30, new ProxyRef<int>(() => FS_MaxFishCatch, o => FS_MaxFishCatch = o), integerBuilder => integerBuilder
                        .SetHintText(fs3_hint))
                    .AddInteger("FS4", fs4_text,5,30, new ProxyRef<int>(() => FS_MinMoraleGain, o => FS_MinMoraleGain = o), integerBuilder => integerBuilder
                        .SetHintText(fs4_hint))
                    .AddInteger("FS5", fs5_text,5,30, new ProxyRef<int>(() => FS_MaxMoraleGain, o => FS_MaxMoraleGain = o), integerBuilder => integerBuilder
                        .SetHintText(fs5_hint))
                    .AddBool("FS6", fs6_text, new ProxyRef<bool>(() => FS_Disable, o => FS_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(fs6_hint))
                    )
                    
                #endregion
                
                #region Food Fight - Builder
                
                .CreateGroup(fof_heading, groupBuilder => groupBuilder
                        .AddInteger("FoF1", fof1_text,2,15, new ProxyRef<int>(() => FoF_MinFoodLoss, o => FoF_MinFoodLoss = o), integerBuilder => integerBuilder
                            .SetHintText(fof1_hint))
                        .AddInteger("FoF2", fof2_text,2,15, new ProxyRef<int>(() => FoF_MaxFoodLoss, o => FoF_MaxFoodLoss = o), integerBuilder => integerBuilder
                            .SetHintText(fof2_hint))
                        .AddInteger("FoF3", fof3_text,5,30, new ProxyRef<int>(() => FoF_MinMoraleLoss, o => FoF_MinMoraleLoss = o), integerBuilder => integerBuilder
                            .SetHintText(fof3_hint))
                        .AddInteger("FoF4", fof4_text,5,30, new ProxyRef<int>(() => FoF_MaxMoraleLoss, o => FoF_MaxMoraleLoss = o), integerBuilder => integerBuilder
                            .SetHintText(fof4_hint))
                        .AddBool("FoF5", fof5_text, new ProxyRef<bool>(() => FoF_Disable, o => FoF_Disable = o), boolBuilder => boolBuilder
                            .SetHintText(fof5_hint))
                    )
                    
                    #endregion
                    
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
            
            
            #region A Flirtatious Encounter
            
            Instance.AFE_Disable = false;
            Instance.AFE_minWomanAge = 18;
            Instance.AFE_maxWomanAge = 45;
            Instance.AFE_minRelationshipIncrease = 15.0f;
            Instance.AFE_maxRelationshipIncrease = 50.0f;

            #endregion

            #region Army Games

            Instance.AG_Disable = false;
            Instance.AG_CohesionGain = 30f;
            Instance.AG_minMoraleGain = 10;
            Instance.AG_maxMoraleGain = 30;

            #endregion

            #region Ahead of Time

            Instance.AoT_Disable = false;
            
            #endregion
            
            #region Bandit Ambush
            
            Instance.BA_Disable = false;
            Instance.BA_MoneyMinPercent = 0.05f;
            Instance.BA_MoneyMaxPercent = 0.15f;
            Instance.BA_TroopScareCount = 50;
            Instance.BA_BanditCap = 50;
            
            #endregion
            
            #region Bee Kind
            
            Instance.BK_Disable = false;
            Instance.BK_damage = 10;
            Instance.BK_Reaction_Chance = 0.15f;
            Instance.BK_Add_Damage = 15;
            
            #endregion
            
            #region Bet Money
            
            Instance.BM_Disable = false;
            Instance.BM_Money_Percent = 0.10f;
            
            #endregion
            
            #region Beggar Begging
            
            Instance.BB_MinGoldToBeggar = 25;
            Instance.BB_MaxGoldToBeggar = 75;
            Instance.BB_MinRenownGain = 10;
            Instance.BB_MaxRenownGain = 20;
            Instance.BB_Disable = false;

            #endregion

            #region Bird Songs
            Instance.BS_Disable = false;
            Instance.BS_minMoraleGain = 15;
            Instance.BS_maxMoraleGain = 30;
            #endregion

            #region Birthday Party

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

            #endregion

            #region Bottoms Up
            Instance.BU_Disable = false;
            Instance.BU_minMoraleGain = 10;
            Instance.BU_maxMoraleGain = 25;
            Instance.BU_minGold = 50;
            Instance.BU_maxGold = 300;
            #endregion

            #region Bumper Crop

            Instance.BC_Disable = false;
            Instance.BC_CropGainPercent = 0.66f;
            
            #endregion
            
            #region Bunch of Prisoners
            
            Instance.BoP_Disable = false;
            Instance.BoP_MinPrisonerGain = 15;
            Instance.BoP_MaxPrisonerGain = 50;

            #endregion
            
            #region Chatting Commanders
            
            Instance.CC_Disable = false;
            Instance.CC_CohesionGain = 30f;

            #endregion

            #region Courier
            Instance.CR_Disable = false;
            Instance.CR_minMoraleGain = 15;
            Instance.CR_maxMoraleGain = 30;
            #endregion

            #region Diseased City

            Instance.DC_Disable = false;
            Instance.DC_BaseSuccessChance = 0.50f;
            Instance.DC_HighMedicineChance = 0.75f;
            Instance.DC_HighMedicineLevel = 75;
            Instance.DC_PercentLoss = 0.20f;

            #endregion

            #region Dreaded Sweats
            Instance.DS_Disable = false;
            Instance.DS_minMoraleLoss = 10;
            Instance.DS_maxMoraleLoss = 25;
            Instance.DS_minvictim = 3;
            Instance.DS_maxvictim = 6;
            #endregion

            #region Dysentery
            Instance.DS_Disable = false;
            Instance.DS_minMoraleLoss = 10;
            Instance.DS_maxMoraleLoss = 25;
            Instance.DS_minvictim = 3;
            Instance.DS_maxvictim = 6;
            #endregion

            #region Eager Troops

            Instance.ET_Disable = false;
            Instance.ET_MinTroopGain = 10;
            Instance.ET_MaxTroopGain = 35;

            #endregion
            
            #region Exotic Drinks
            
            Instance.ED_Disable = false;
            Instance.ED_MinPrice = 3000;
            Instance.ED_MaxPrice = 6000;

            #endregion
            
            #region Fallen Soldier Family
            
            Instance.FSF_Disable = false;
            Instance.FSF_MinFamilyCompensation = 750;
            Instance.FSF_MaxFamilyCompensation = 1750;
            Instance.FSF_MinGoldLooted = 750;
            Instance.FSF_MaxGoldLooted = 1500;

            #endregion
            
            #region Fantastic Fighters
            
            Instance.FF_Disable = false;
            Instance.FF_MinRenownGain = 25;
            Instance.FF_MaxRenownGain = 75;

            #endregion
            
            #region Fishing Spot
            
            Instance.FS_Disable = false;
            Instance.FS_MinSoldiersToGo = 3;
            Instance.FS_MaxSoldiersToGo = 12;
            Instance.FS_MaxFishCatch = 20;
            Instance.FS_MinMoraleGain = 7;
            Instance.FS_MaxMoraleGain = 20;

            #endregion
            
            #region Food Fight
            
            Instance.FoF_Disable = false;
            Instance.FoF_MinFoodLoss = 5;
            Instance.FoF_MaxFoodLoss = 30;
            Instance.FoF_MinMoraleLoss = 5;
            Instance.FoF_MaxMoraleLoss = 20;

            #endregion

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
            
            #endregion
        }
        

        public void Dispose()
        {
            //NA
        }
    }
}