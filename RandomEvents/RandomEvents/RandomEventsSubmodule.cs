using System;
using System.Windows;
using CryingBuffalo.RandomEvents.Settings;
using CryingBuffalo.RandomEvents.Settings.MCM;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace CryingBuffalo.RandomEvents
{
    public class RandomEventsSubmodule : MBSubModuleBase
    {
        public static readonly Color Ini_Color = Color.FromUint(7194750);
        public static readonly Color Dbg_Color = Color.FromUint(16005134);
        public static readonly Color Msg_Color = Color.FromUint(11846692);
        
        //Green
        public static readonly Color Msg_Color_POS_Outcome = Color.FromUint(1999945);
        
        //Yellow
        public static readonly Color Msg_Color_MED_Outcome = Color.FromUint(13937677);
        
        //Red
        public static readonly Color Msg_Color_NEG_Outcome = Color.FromUint(11549230);
        
        //MCM Base Settings
        public const string FolderName = "RandomEvents";
        public const string ModName = "RandomEvents";

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            MCM_MenuConfig_A_M.Instance.Settings();
            MCM_MenuConfig_N_Z.Instance.Settings();
            MCM_MenuConfig_Chances.Instance.Settings();
            MCM_ConfigMenu_General.Instance.Settings();

            ModSettings.LoadGeneralSettings();
            ModSettings.LoadRandomEventSettings();
            
            //Many mods use this. Nice way to tell if a mod is loaded correctly
            InformationManager.DisplayMessage(new InformationMessage("Successfully loaded 'RandomEvents'.", Ini_Color));
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            base.OnGameStart(game, gameStarterObject);

            if (!(game.GameType is Campaign)) return;
            CampaignGameStarter gameInitializer = (CampaignGameStarter)gameStarterObject;
            try
            {
                gameInitializer.AddBehavior(new RandomEventBehavior());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while initialising RandomEvents :\n\n {ex.Message} \n\n { ex.StackTrace}");
            }
        }
    }
}
