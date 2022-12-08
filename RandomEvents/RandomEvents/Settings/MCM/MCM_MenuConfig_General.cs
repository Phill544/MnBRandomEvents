using System;
using MCM.Abstractions.Base.Global;
using MCM.Abstractions.FluentBuilder;
using MCM.Common;
using TaleWorlds.Localization;

namespace CryingBuffalo.RandomEvents.Settings.MCM
{
    public class MCM_ConfigMenu_General : IDisposable
    {
        private static MCM_ConfigMenu_General _instance;

        private FluentGlobalSettings globalSettings;

        #region Variables
        
        public bool GS_FirstRunDone { get; set; }
        public bool GS_DebugMode { get; private set; }
        public bool GS_Disable_Supernatural { get; private set; }
        public int GS_MinimumInGameHours { get; private set; }
        private int GS_MinimumRealMinutes { get; set; }
        public int GS_MaximumRealMinutes { get; private set; }
        public int GS_GeneralLevelXpMultiplier { get; private set; }
        public bool GS_DisableSkillChecks { get; private set; }

        #endregion


        public static MCM_ConfigMenu_General Instance
        {
            get { return _instance ??= new MCM_ConfigMenu_General(); }
        }

        public void Settings()
        {
            

            #region Strings

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
            var gs6_text = new TextObject("{=mcm_gs6_text}6. General Level XP Multiplier ").ToString();
            var gs6_hint = new TextObject("{=mcm_gs6_hint}The number used to define the XP multiplier. Higher number means higher XP.").ToString();
            var gs7_text = new TextObject("{=mcm_gs7_text}7. Disable Supernatural Events").ToString();
            var gs7_hint = new TextObject("{=mcm_gs7_hint}Check this if you want to disable all supernatural events. This is mostly to keep the game as canon as possible.").ToString();
            var gs8_text = new TextObject("{=mcm_gs8_text}8. Disable Skill Checks").ToString();
            var gs8_hint = new TextObject("{=mcm_gs8_hint}Some events have skill checks that open up new options. This disables that and makes all options available.").ToString();


            #endregion
            
            
            
            var builder = BaseSettingsBuilder.Create("RandomEvents4","4. Random Events - Settings")!
                .SetFormat("xml")
                .SetFolderName(RandomEventsSubmodule.FolderName)
                .SetSubFolder(RandomEventsSubmodule.ModName)
                
           #region Builder Modules
                
                
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
                        .SetHintText(gs5_hint))
                    .AddInteger("GS6", gs6_text,10,70, new ProxyRef<int>(() => GS_GeneralLevelXpMultiplier, o => GS_GeneralLevelXpMultiplier = o), integerBuilder => integerBuilder
                        .SetHintText(gs6_hint))
                    .AddBool("GS7", gs7_text, new ProxyRef<bool>(() => GS_Disable_Supernatural, o => GS_Disable_Supernatural = o), boolBuilder => boolBuilder
                        .SetHintText(gs7_hint))
                    .AddBool("GS8", gs8_text, new ProxyRef<bool>(() => GS_DisableSkillChecks, o => GS_DisableSkillChecks = o), boolBuilder => boolBuilder
                        .SetHintText(gs8_hint))
                    
                #endregion
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
            #region First Time Setup
            
            #region General Settings
            
            Instance.GS_FirstRunDone = true;
            Instance.GS_DebugMode = false;
            Instance.GS_MinimumInGameHours = 24;
            Instance.GS_MinimumRealMinutes = 5;
            Instance.GS_MaximumRealMinutes = 30;
            Instance.GS_GeneralLevelXpMultiplier = 40;
            Instance.GS_Disable_Supernatural = false;
            Instance.GS_DisableSkillChecks = false;

            #endregion

            #endregion
        }
        

        public void Dispose()
        {
            //NA
        }
    }
}