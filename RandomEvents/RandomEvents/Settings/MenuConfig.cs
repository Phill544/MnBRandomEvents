using System;
using MCM.Abstractions.Base.Global;
using MCM.Abstractions.FluentBuilder;
using MCM.Common;

namespace CryingBuffalo.RandomEvents.Settings
{
    public class MenuConfig : IDisposable
    {
        private static MenuConfig _instance;

        private FluentGlobalSettings globalSettings;
        
        public bool FirstRunDone { get; set; }
        public int MinimumInGameHours { get;  set; }
        public int MinimumRealMinutes { get;  set; }
        public int MaximumRealMinutes { get;  set; }
        public int MinGoldToBeggar { get; set; }
        public int MaxGoldToBeggar { get; set; }
        public int MinRenownGain { get; set; }
        public int MaxRenownGain { get; set; }

        public static MenuConfig Instance
        {
            get { return _instance ??= new MenuConfig(); }
        }

        public void Settings()
        {

            var builder = BaseSettingsBuilder.Create("RandomEvents","Random Events")!
                .SetFormat("xml")
                .SetFolderName(RandomEventsSubmodule.FolderName)
                .SetSubFolder(RandomEventsSubmodule.ModName)
                .CreateGroup("1. General Settings", groupBuilder => groupBuilder
                    .AddBool("GS1", "1. Initial Setup", new ProxyRef<bool>(() => FirstRunDone, o => FirstRunDone = o), boolBuilder => boolBuilder
                        .SetHintText("Uncheck this to re-run the Initial Setup and set all values back to the original")
                        .SetRequireRestart(true))
                    .AddInteger("GS2", "2. In-game hours before events",5,360, new ProxyRef<int>(() => MinimumInGameHours, o => MinimumInGameHours = o), integerBuilder => integerBuilder
                        .SetHintText("Minimum amount of in-game hours that must pass before the random events begin to occur."))
                    .AddInteger("GS3", "3. Min minutes between events",5,60, new ProxyRef<int>(() => MinimumRealMinutes, o => MinimumRealMinutes = o), integerBuilder => integerBuilder
                        .SetHintText("Minimum amount of minutes in between events."))
                    .AddInteger("GS4", "4. Max minutes between events",5,60, new ProxyRef<int>(() => MaximumRealMinutes, o => MaximumRealMinutes = o), integerBuilder => integerBuilder
                        .SetHintText("Maximum amount of minutes in between events.")))
                
                .CreateGroup("2. Beggar Begging", groupBuilder => groupBuilder
                    .AddInteger("BB1", "1. Min gold to give.",5,150, new ProxyRef<int>(() => MinGoldToBeggar, o => MinGoldToBeggar = o), integerBuilder => integerBuilder
                        .SetHintText("Minimum amount of gold to give to the beggar."))
                    .AddInteger("BB2", "2. Max gold to give.",5,150, new ProxyRef<int>(() => MaxGoldToBeggar, o => MaxGoldToBeggar = o), integerBuilder => integerBuilder
                        .SetHintText("Maximum amount of gold to give to the beggar."))
                    .AddInteger("BB3", "3. Min renown gained.",5,25, new ProxyRef<int>(() => MinRenownGain, o => MinRenownGain = o), integerBuilder => integerBuilder
                        .SetHintText("Minimum amount of renown to gain."))
                    .AddInteger("BB4", "4. Max renown gained.",5,25, new ProxyRef<int>(() => MaxRenownGain, o => MaxRenownGain = o), integerBuilder => integerBuilder
                        .SetHintText("Maximum amount of renown to gain."))
                    );



            globalSettings = builder.BuildAsGlobal();
            globalSettings.Register();

            if (!FirstRunDone)
            {
                Perform_First_Time_Setup();
                
            }
        }

        private void Perform_First_Time_Setup()
        {
            Instance.FirstRunDone = true;
            Instance.MinimumInGameHours = 24;
            Instance.MinimumRealMinutes = 5;
            Instance.MaximumRealMinutes = 30;
            Instance.MinGoldToBeggar = 25;
            Instance.MaxGoldToBeggar = 75;
            Instance.MinRenownGain = 10;
            Instance.MaxRenownGain = 20;
        }

        public void Dispose()
        {
            //MenuConfig.Unregister();
        }
    }
}