using System;
using System.Windows;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace CryingBuffalo.RandomEvents
{
    public class RandomEventsSubmodule : MBSubModuleBase
    {
        public static readonly Color TextColor = Color.FromUint(6750401U);

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            Settings.Settings.LoadGeneralSettings();
            Settings.Settings.LoadRandomEventSettings();
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();

            InformationManager.DisplayMessage(new InformationMessage("Successfully loaded 'RandomEvents'.", TextColor));
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
