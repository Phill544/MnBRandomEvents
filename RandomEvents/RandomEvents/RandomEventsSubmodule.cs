using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using Bannerlord.RandomEvents.Settings;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace Bannerlord.RandomEvents
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class RandomEventsSubmodule : MBSubModuleBase
    {
        //Mod Loaded Color
        public static readonly Color Ini_Color = Color.FromUint(7194750);
        
        public static readonly Color Msg_Color = Color.FromUint(11846692);
        
        //Green
        public static readonly Color Msg_Color_POS_Outcome = Color.FromUint(1999945);
        
        //Yellow
        public static readonly Color Msg_Color_MED_Outcome = Color.FromUint(13937677);
        
        //Orange
        public static readonly Color Msg_Color_NEG_Outcome = Color.FromUint(15092249);
        
        //Evil
        public static readonly Color Msg_Color_EVIL_Outcome = Color.FromUint(13840175);
        
        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {

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
