using System;
using MCM.Abstractions.Base.Global;
using MCM.Abstractions.FluentBuilder;
using MCM.Common;
using TaleWorlds.Localization;

namespace CryingBuffalo.RandomEvents.Settings.MCM
{
    public class MCM_MenuConfig_Toggle : IDisposable
    {
        private static MCM_MenuConfig_Toggle _instance;

        private FluentGlobalSettings globalSettings;

        #region Variables

        public bool AFE_Disable { get; private set; }
        public bool AG_Disable { get; private set; }
        public bool AI_Disable { get; private set; }
        public bool AoT_Disable { get; private set; }
        public bool BA_Disable { get; private set; }
        public bool BK_Disable { get; private set; }
        public bool BM_Disable { get; private set; }
        public bool BB_Disable { get; private set; }
        public bool BS_Disable { get; private set; }
        public bool BP_Disable { get; private set; }
        public bool BU_Disable { get; private set; }
        public bool BC_Disable { get; private set; }
        public bool BoP_Disable { get; private set; }
        public bool CC_Disable { get; private set; }
        public bool CA_Disable { get; private set; }
        public bool CR_Disable { get; private set; }
        public bool DC_Disable { get; private set; }
        public bool DS_Disable { get; private set; }
        public bool DU_Disable { get; private set; }
        public bool DY_Disable { get; private set; }
        public bool ET_Disable { get; private set; }
        public bool ED_Disable { get; private set; }
        public bool FSF_Disable { get; private set; }
        public bool FF_Disable { get; private set; }
        public bool FE_Disable { get; private set; }
        public bool FS_Disable { get; private set; }
        public bool FoF_Disable { get; private set; }
        public bool GR_Disable { get; private set; }
        public bool HS_Disable { get; private set; }
        public bool HT_Disable { get; private set; }
        public bool LitS_Disable { get; private set; }
        public bool LS_Disable { get; private set; }
        public bool LU_Disable { get; private set; }
        public bool MG_Disable { get; private set; }
        public bool MO_Disable { get; private set; }
        public bool NotW_Disable { get; private set; }
        public bool OR_Disable { get; private set; }
        public bool PC_Disable { get; private set; }
        public bool PW_Disable { get; private set; }
        public bool PoWi_Disable { get; private set; }
        public bool PR_Disable { get; private set; }
        public bool PT_Disable { get; private set; }
        public bool RM_Disable { get; private set; }
        public bool RF_Disable { get; private set; }
        public bool RO_Disable { get; private set; }
        public bool RS_Disable { get; private set; }
        public bool SOS_Disable { get; private set; }
        public bool SS_Disable { get; private set; }
        public bool SR_Disable { get; private set; }
        public bool SD_Disable { get; private set; }
        public bool SuS_Disable { get; private set; }
        public bool SE_Disable { get; private set; }
        public bool TP_Disable { get; private set; }
        public bool TM_Disable { get; private set; }
        public bool TR_Disable { get; private set; }
        public bool UW_Disable { get; private set; }
        public bool UC_Disable { get; private set; }
        public bool VG_Disable { get; private set; }
        public bool WL_Disable { get; private set; }
        
        #endregion


        public static MCM_MenuConfig_Toggle Instance
        {
            get { return _instance ??= new MCM_MenuConfig_Toggle(); }
        }

        public void Settings()
        {
            
            #region Strings
            
            var toggle_text = new TextObject("{=mcm_toggle_text}Deactivate event").ToString();
            var toggle_hint = new TextObject("{=mcm_toggle_hint}If you dont want this event to show up you can deactivate it.").ToString();

            var afe_heading = new TextObject("{=mcm_afe_heading}A Flirtatious Encounter").ToString();
            var ag_heading = new TextObject("{=mcm_ag_heading}Army Games").ToString();
            var ai_heading = new TextObject("{=mcm_ai_heading}Army Invite").ToString();
            var aot_heading = new TextObject("{=mcm_aot_heading}Ahead of Time").ToString();
            var ba_heading = new TextObject("{=mcm_ba_heading}Bandit Ambush").ToString();
            var bk_heading = new TextObject("{=mcm_bk_heading}Bee Kind").ToString();
            var bm_heading = new TextObject("{=mcm_bm_heading}Bet Money").ToString();
            var bb_heading = new TextObject("{=mcm_bb_heading}Beggar Begging").ToString();
            var bs_heading = new TextObject("{=mcm_bs_heading}BirdSong").ToString();
            var bp_heading = new TextObject("{=mcm_bp_heading}Birthday Party").ToString();
            var bu_heading = new TextObject("{=mcm_bu_heading}Bottoms Up").ToString();
            var bc_heading = new TextObject("{=mcm_bc_heading}Bumper Crop").ToString();
            var bop_heading = new TextObject("{=mcm_bop_heading}Bunch of Prisoners").ToString();
            var cc_heading = new TextObject("{=mcm_cc_heading}Chatting Commanders").ToString();
            var ca_heading = new TextObject("{=mcm_ca_heading}Companion Admire").ToString();
            var cr_heading = new TextObject("{=mcm_cr_heading}Courier").ToString();
            var dc_heading = new TextObject("{=mcm_dc_heading}Diseased City").ToString();
            var ds_heading = new TextObject("{=mcm_ds_heading}Dreaded Sweats").ToString();
            var du_heading = new TextObject("{=mcm_du_heading}Duel").ToString();
            var dy_heading = new TextObject("{=mcm_dy_heading}Dysentery").ToString();
            var et_heading = new TextObject("{=mcm_et_heading}Eager Troops").ToString();
            var ed_heading = new TextObject("{=mcm_ed_heading}Exotic Drinks").ToString();
            var fsf_heading = new TextObject("{=mcm_fsf_heading}Fallen Soldier Family").ToString();
            var ff_heading = new TextObject("{=mcm_ff_heading}Fantastic Fighters").ToString();
            var fe_heading = new TextObject("{=mcm_fe_heading}Feast").ToString();
            var fs_heading = new TextObject("{=mcm_fs_heading}Fishing Spot").ToString();
            var fof_heading = new TextObject("{=mcm_fof_heading}Food Fight").ToString();
            var gr_heading = new TextObject("{=mcm_gr_heading}Granary Rats").ToString();
            var hs_heading = new TextObject("{=mcm_hs_heading}Hot Springs").ToString();
            var ht_heading = new TextObject("{=mcm_ht_heading}Hunting Trip").ToString();
            var lits_heading = new TextObject("{=mcm_lits_heading}Lights in the Skies").ToString();
            var ls_heading = new TextObject("{=mcm_ls_heading}Logging Site").ToString();
            var lu_heading = new TextObject("{=mcm_lu_heading}Look Up").ToString();
            var mg_heading = new TextObject("{=mcm_mg_heading}Mass Grave").ToString();
            var mo_heading = new TextObject("{=mcm_mo_heading}Momentum").ToString();
            var notw_heading = new TextObject("{=mcm_notw_heading}Not of this World").ToString();
            var or_heading = new TextObject("{=mcm_or_heading}Old Ruins").ToString();
            var pc_heading = new TextObject("{=mcm_pc_heading}Passing Comet").ToString();
            var pw_heading = new TextObject("{=mcm_pw_heading}Perfect Weather").ToString();
            var powi_heading = new TextObject("{=mcm_powi_heading}Poisoned Wine").ToString();
            var pr_heading = new TextObject("{=mcm_pr_heading}Prisoner Rebellion").ToString();
            var pt_heading = new TextObject("{=mcm_pt_heading}Prisoner Transfer").ToString();
            var rm_heading = new TextObject("{=mcm_rm_heading}Red Moon").ToString();
            var rf_heading = new TextObject("{=mcm_rf_heading}Refugees").ToString();
            var ro_heading = new TextObject("{=mcm_ro_heading}Robbery").ToString();
            var rs_heading = new TextObject("{=mcm_rs_heading}Runaway Son").ToString();
            var sos_heading = new TextObject("{=mcm_sos_heading}Secret of Steel").ToString();
            var ss_heading = new TextObject("{=mcm_ss_heading}Secret Singer").ToString();
            var sr_heading = new TextObject("{=mcm_sr_heading}Speedy Recovery").ToString();
            var sd_heading = new TextObject("{=mcm_sd_heading}Successful Deeds").ToString();
            var sus_heading = new TextObject("{=mcm_sus_heading}Sudden Storm").ToString();
            var se_heading = new TextObject("{=mcm_se_heading}Supernatural Encounter").ToString();
            var tp_heading = new TextObject("{=mcm_tp_heading}Target Practice").ToString();
            var tm_heading = new TextObject("{=mcm_tm_heading}Travelling Merchant").ToString();
            var tr_heading = new TextObject("{=mcm_tr_heading}Travellers").ToString();
            var uw_heading = new TextObject("{=mcm_uw_heading}Unexpected Wedding").ToString();
            var uc_heading = new TextObject("{=mcm_uc_heading}Undercooked").ToString();
            var vg_heading = new TextObject("{=mcm_vg_heading}Violated Girl").ToString();
            var wl_heading = new TextObject("{=mcm_wl_heading}Wandering Livestock").ToString();

            #endregion
            
            
            var builder = BaseSettingsBuilder.Create("RandomEvents2","2. Random Events - Event Toggle")!
                .SetFormat("xml")
                .SetFolderName(RandomEventsSubmodule.FolderName)
                .SetSubFolder(RandomEventsSubmodule.ModName)
                
                #region Builder Modules
                
                .CreateGroup(afe_heading, groupBuilder => groupBuilder
                    .AddBool("AFE", toggle_text, new ProxyRef<bool>(() => AFE_Disable, o => AFE_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(toggle_hint))
                )
                .CreateGroup(ag_heading, groupBuilder => groupBuilder
                    .AddBool("AG", toggle_text, new ProxyRef<bool>(() => AG_Disable, o => AG_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(toggle_hint))
                )
                .CreateGroup(ai_heading, groupBuilder => groupBuilder
                    .AddBool("AI", toggle_text, new ProxyRef<bool>(() => AI_Disable, o => AI_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(toggle_hint))
                )
                .CreateGroup(aot_heading, groupBuilder => groupBuilder
                    .AddBool("AoT", toggle_text, new ProxyRef<bool>(() => AoT_Disable, o => AoT_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(toggle_hint))
                )
                .CreateGroup(ba_heading, groupBuilder => groupBuilder
                    .AddBool("BA", toggle_text, new ProxyRef<bool>(() => BA_Disable, o => BA_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(toggle_hint))
                )
                .CreateGroup(bk_heading, groupBuilder => groupBuilder
                    .AddBool("BK", toggle_text, new ProxyRef<bool>(() => BK_Disable, o => BK_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(toggle_hint))
                )
                .CreateGroup(bm_heading, groupBuilder => groupBuilder
                    .AddBool("BM", toggle_text, new ProxyRef<bool>(() => BM_Disable, o => BM_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(toggle_hint))
                )
                .CreateGroup(bb_heading, groupBuilder => groupBuilder
                    .AddBool("BB", toggle_text, new ProxyRef<bool>(() => BB_Disable, o => BB_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(toggle_hint))
                )
                .CreateGroup(bs_heading, groupBuilder => groupBuilder
                    .AddBool("BS", toggle_text, new ProxyRef<bool>(() => BS_Disable, o => BS_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(toggle_hint))
                )
                .CreateGroup(bp_heading, groupBuilder => groupBuilder
                    .AddBool("BP", toggle_text, new ProxyRef<bool>(() => BP_Disable, o => BP_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(toggle_hint))
                )
                .CreateGroup(bu_heading, groupBuilder => groupBuilder
                    .AddBool("BU", toggle_text, new ProxyRef<bool>(() => BU_Disable, o => BU_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(toggle_hint))
                )
                .CreateGroup(bc_heading, groupBuilder => groupBuilder
                    .AddBool("BC", toggle_text, new ProxyRef<bool>(() => BC_Disable, o => BC_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(toggle_hint))
                )
                .CreateGroup(bop_heading, groupBuilder => groupBuilder
                    .AddBool("BoP", toggle_text, new ProxyRef<bool>(() => BoP_Disable, o => BoP_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(toggle_hint))
                )
                .CreateGroup(cc_heading, groupBuilder => groupBuilder
                    .AddBool("CC", toggle_text, new ProxyRef<bool>(() => CC_Disable, o => CC_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(toggle_hint))
                )
                .CreateGroup(ca_heading, groupBuilder => groupBuilder
                    .AddBool("CA", toggle_text, new ProxyRef<bool>(() => CA_Disable, o => CA_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(toggle_hint))
                )
                .CreateGroup(cr_heading, groupBuilder => groupBuilder
                    .AddBool("CR", toggle_text, new ProxyRef<bool>(() => CR_Disable, o => CR_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(toggle_hint))
                )
                .CreateGroup(dc_heading, groupBuilder => groupBuilder
                    .AddBool("DC", toggle_text, new ProxyRef<bool>(() => DC_Disable, o => DC_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(toggle_hint))
                )
                .CreateGroup(ds_heading, groupBuilder => groupBuilder
                    .AddBool("DS", toggle_text, new ProxyRef<bool>(() => DS_Disable, o => DS_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(toggle_hint))
                )
                .CreateGroup(du_heading, groupBuilder => groupBuilder
                    .AddBool("DU", toggle_text, new ProxyRef<bool>(() => DU_Disable, o => DU_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(toggle_hint))
                )
                .CreateGroup(dy_heading, groupBuilder => groupBuilder
                    .AddBool("DY", toggle_text, new ProxyRef<bool>(() => DY_Disable, o => DY_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(toggle_hint))
                )
                .CreateGroup(et_heading, groupBuilder => groupBuilder
                    .AddBool("ET", toggle_text, new ProxyRef<bool>(() => ET_Disable, o => ET_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(toggle_hint))
                )
                .CreateGroup(ed_heading, groupBuilder => groupBuilder
                    .AddBool("ED", toggle_text, new ProxyRef<bool>(() => ED_Disable, o => ED_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(toggle_hint))
                )
                .CreateGroup(fsf_heading, groupBuilder => groupBuilder
                    .AddBool("FSF", toggle_text, new ProxyRef<bool>(() => FSF_Disable, o => FSF_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(toggle_hint))
                )
                .CreateGroup(ff_heading, groupBuilder => groupBuilder
                    .AddBool("FF", toggle_text, new ProxyRef<bool>(() => FF_Disable, o => FF_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(toggle_hint))
                )
                .CreateGroup(fe_heading, groupBuilder => groupBuilder
                    .AddBool("FE", toggle_text, new ProxyRef<bool>(() => FF_Disable, o => FF_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(toggle_hint))
                )
                .CreateGroup(fs_heading, groupBuilder => groupBuilder
                    .AddBool("FS", toggle_text, new ProxyRef<bool>(() => FS_Disable, o => FS_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(toggle_hint))
                )
                .CreateGroup(fof_heading, groupBuilder => groupBuilder
                    .AddBool("FoF", toggle_text, new ProxyRef<bool>(() => FoF_Disable, o => FoF_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(toggle_hint))
                )
                .CreateGroup(gr_heading, groupBuilder => groupBuilder
                    .AddBool("GR", toggle_text, new ProxyRef<bool>(() => GR_Disable, o => GR_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(toggle_hint))
                )
                .CreateGroup(hs_heading, groupBuilder => groupBuilder
                    .AddBool("HS", toggle_text, new ProxyRef<bool>(() => HS_Disable, o => HS_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(toggle_hint))
                )
                .CreateGroup(ht_heading, groupBuilder => groupBuilder
                    .AddBool("HT", toggle_text, new ProxyRef<bool>(() => HT_Disable, o => HT_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(toggle_hint))
                )
                .CreateGroup(lits_heading, groupBuilder => groupBuilder
                    .AddBool("LITS", toggle_text, new ProxyRef<bool>(() => LitS_Disable, o => LitS_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(toggle_hint))
                )
                .CreateGroup(ls_heading, groupBuilder => groupBuilder
                    .AddBool("LS", toggle_text, new ProxyRef<bool>(() => LS_Disable, o => LS_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(toggle_hint))
                )
                .CreateGroup(lu_heading, groupBuilder => groupBuilder
                    .AddBool("LU", toggle_text, new ProxyRef<bool>(() => LU_Disable, o => LU_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(toggle_hint))
                )
                .CreateGroup(mg_heading, groupBuilder => groupBuilder
                    .AddBool("MG", toggle_text, new ProxyRef<bool>(() => MG_Disable, o => MG_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(toggle_hint))
                )
                .CreateGroup(mo_heading, groupBuilder => groupBuilder
                    .AddBool("MO", toggle_text, new ProxyRef<bool>(() => MO_Disable, o => MO_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(toggle_hint))
                )
                .CreateGroup(notw_heading, groupBuilder => groupBuilder
                    .AddBool("NotW", toggle_text, new ProxyRef<bool>(() => NotW_Disable, o => NotW_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(toggle_hint))
                )
                .CreateGroup(or_heading, groupBuilder => groupBuilder
                    .AddBool("OR", toggle_text, new ProxyRef<bool>(() => OR_Disable, o => OR_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(toggle_hint))
                )
                .CreateGroup(pc_heading, groupBuilder => groupBuilder
                    .AddBool("PC", toggle_text, new ProxyRef<bool>(() => PC_Disable, o => PC_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(toggle_hint))
                )
                .CreateGroup(pw_heading, groupBuilder => groupBuilder
                    .AddBool("PW", toggle_text, new ProxyRef<bool>(() => PW_Disable, o => PW_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(toggle_hint))
                )
                .CreateGroup(powi_heading, groupBuilder => groupBuilder
                    .AddBool("PoWi", toggle_text, new ProxyRef<bool>(() => PoWi_Disable, o => PoWi_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(toggle_hint))
                )
                .CreateGroup(pr_heading, groupBuilder => groupBuilder
                    .AddBool("PR", toggle_text, new ProxyRef<bool>(() => PR_Disable, o => PR_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(toggle_hint))
                )
                .CreateGroup(pt_heading, groupBuilder => groupBuilder
                    .AddBool("PT", toggle_text, new ProxyRef<bool>(() => PT_Disable, o => PT_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(toggle_hint))
                )
                .CreateGroup(rm_heading, groupBuilder => groupBuilder
                    .AddBool("RM", toggle_text, new ProxyRef<bool>(() => RM_Disable, o => RM_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(toggle_hint))
                )
                .CreateGroup(rf_heading, groupBuilder => groupBuilder
                    .AddBool("RF", toggle_text, new ProxyRef<bool>(() => RF_Disable, o => RF_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(toggle_hint))
                )
                .CreateGroup(ro_heading, groupBuilder => groupBuilder
                    .AddBool("RO", toggle_text, new ProxyRef<bool>(() => RO_Disable, o => RO_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(toggle_hint))
                )
                .CreateGroup(rs_heading, groupBuilder => groupBuilder
                    .AddBool("RS", toggle_text, new ProxyRef<bool>(() => RS_Disable, o => RS_Disable = o), boolBuilder => boolBuilder
                            .SetHintText(toggle_hint))
                )
                .CreateGroup(sos_heading, groupBuilder => groupBuilder
                    .AddBool("SOS", toggle_text, new ProxyRef<bool>(() => SOS_Disable, o => SOS_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(toggle_hint))
                )
                .CreateGroup(ss_heading, groupBuilder => groupBuilder
                    .AddBool("SS", toggle_text, new ProxyRef<bool>(() => SS_Disable, o => SS_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(toggle_hint))
                )
                .CreateGroup(sr_heading, groupBuilder => groupBuilder
                    .AddBool("SR", toggle_text, new ProxyRef<bool>(() => SR_Disable, o => SR_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(toggle_hint))
                )
                .CreateGroup(sd_heading, groupBuilder => groupBuilder
                    .AddBool("SD", toggle_text, new ProxyRef<bool>(() => SD_Disable, o => SD_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(toggle_hint))
                )
                .CreateGroup(sus_heading, groupBuilder => groupBuilder
                    .AddBool("SuS", toggle_text, new ProxyRef<bool>(() => SuS_Disable, o => SuS_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(toggle_hint))
                )
                .CreateGroup(se_heading, groupBuilder => groupBuilder
                    .AddBool("SE", toggle_text, new ProxyRef<bool>(() => SE_Disable, o => SE_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(toggle_hint))
                )
                .CreateGroup(tp_heading, groupBuilder => groupBuilder
                    .AddBool("TP", toggle_text, new ProxyRef<bool>(() => TP_Disable, o => TP_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(toggle_hint))
                )
                .CreateGroup(tm_heading, groupBuilder => groupBuilder
                    .AddBool("TM", toggle_text, new ProxyRef<bool>(() => TM_Disable, o => TM_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(toggle_hint))
                )
                .CreateGroup(tr_heading, groupBuilder => groupBuilder
                    .AddBool("TR", toggle_text, new ProxyRef<bool>(() => TR_Disable, o => TR_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(toggle_hint))
                )
                .CreateGroup(uw_heading, groupBuilder => groupBuilder
                    .AddBool("UW", toggle_text, new ProxyRef<bool>(() => UW_Disable, o => UW_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(toggle_hint))
                )
                .CreateGroup(uc_heading, groupBuilder => groupBuilder
                    .AddBool("UC", toggle_text, new ProxyRef<bool>(() => UC_Disable, o => UC_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(toggle_hint))
                )
                .CreateGroup(vg_heading, groupBuilder => groupBuilder
                    .AddBool("VG", toggle_text, new ProxyRef<bool>(() => VG_Disable, o => VG_Disable = o), boolBuilder => boolBuilder
                        .SetHintText(toggle_hint))
                )
                .CreateGroup(wl_heading, groupBuilder => groupBuilder
                        .AddBool("WL", toggle_text, new ProxyRef<bool>(() => WL_Disable, o => WL_Disable = o), boolBuilder => boolBuilder
                            .SetHintText(toggle_hint))
                    
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
            
            
            Instance.AFE_Disable = false;
            Instance.AG_Disable = false;
            Instance.AI_Disable = false;
            Instance.AoT_Disable = false;
            Instance.BA_Disable = false;
            Instance.BK_Disable = false;
            Instance.BM_Disable = false;
            Instance.BB_Disable = false;
            Instance.BS_Disable = false;
            Instance.BP_Disable = false;
            Instance.BU_Disable = false;
            Instance.BC_Disable = false;
            Instance.BoP_Disable = false;
            Instance.CC_Disable = false;
            Instance.CA_Disable = false;
            Instance.CR_Disable = false;
            Instance.DC_Disable = false;
            Instance.DS_Disable = false;
            Instance.DU_Disable = false;
            Instance.DY_Disable = false;
            Instance.ET_Disable = false;
            Instance.ED_Disable = false;
            Instance.FSF_Disable = false;
            Instance.FF_Disable = false;
            Instance.FE_Disable = false;
            Instance.FS_Disable = false;
            Instance.FoF_Disable = false;
            Instance.GR_Disable = false;
            Instance.HS_Disable = false;
            Instance.HT_Disable = false;
            Instance.LitS_Disable = false;
            Instance.LS_Disable = false;
            Instance.LU_Disable = false;
            Instance.MG_Disable = false;
            Instance.MO_Disable = false;
            Instance.NotW_Disable = false;
            Instance.OR_Disable = false;
            Instance.PC_Disable = false;
            Instance.PW_Disable = false;
            Instance.PoWi_Disable = false;
            Instance.PR_Disable = false;
            Instance.PT_Disable = true;
            Instance.RM_Disable = false;
            Instance.RF_Disable = false;
            Instance.RO_Disable = false;
            Instance.RS_Disable = false;
            Instance.SOS_Disable = false;
            Instance.SS_Disable = false;
            Instance.SR_Disable = false;
            Instance.SD_Disable = false;
            Instance.SuS_Disable = false;
            Instance.SE_Disable = false;
            Instance.TP_Disable = false;
            Instance.TM_Disable = false;
            Instance.TR_Disable = false;
            Instance.UW_Disable = false;
            Instance.UC_Disable = false;
            Instance.VG_Disable = false;
            Instance.WL_Disable = false;
            
            #endregion
        }
        

        public void Dispose()
        {
            //NA
        }
    }
}