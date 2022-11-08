using System;
using System.Windows;
using CryingBuffalo.RandomEvents.Settings;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace CryingBuffalo.RandomEvents
{
    public class RandomEventsSubmodule : MBSubModuleBase
    {
        public static readonly Color TextColor = Color.FromUint(6750401U);
        public static readonly Color MsgColor = Color.FromUint(11846692);
        
        //MCM Settings
        public static readonly string ModuleFolderName = "RandomEvents";
        public static readonly string ModName = "RandomEvents";

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            ModSettings.LoadGeneralSettings();
            ModSettings.LoadRandomEventSettings();
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            MenuConfig.Instance.Settings();
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
