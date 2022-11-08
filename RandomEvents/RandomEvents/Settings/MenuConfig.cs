using System;
using System.Reflection;
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
            var ModName = Assembly.GetExecutingAssembly().GetName().Name;
                
            var version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            var builder = BaseSettingsBuilder.Create("RandomEvents", $"{ModName} - {version}")!
                .SetFormat("xml")
                .SetFolderName(RandomEventsSubmodule.ModuleFolderName)
                .SetSubFolder(RandomEventsSubmodule.ModName)
                
                .CreateGroup("1. Beggar Begging", groupBuilder => groupBuilder
                    .AddInteger("BB1", "1.Min gold to give.",5,150, new ProxyRef<int>(() => MinGoldToBeggar, o => MinGoldToBeggar = o), boolBuilder => boolBuilder
                        .SetHintText("Minimum amount of gold to give to the beggar."))
                    .AddInteger("BB2", "2.Max gold to give.",5,150, new ProxyRef<int>(() => MaxGoldToBeggar, o => MaxGoldToBeggar = o), boolBuilder => boolBuilder
                        .SetHintText("Maximum amount of gold to give to the beggar."))
                    .AddInteger("BB3", "3.Min renown gained.",5,25, new ProxyRef<int>(() => MinRenownGain, o => MinRenownGain = o), boolBuilder => boolBuilder
                        .SetHintText("Minimum amount of renown to gain."))
                    .AddInteger("BB4", "4.Max renown gained.",5,25, new ProxyRef<int>(() => MaxRenownGain, o => MaxRenownGain = o), boolBuilder => boolBuilder
                        .SetHintText("Maximum amount of renown to gain."))
                    )
                
                .CreateGroup("General Settings", groupBuilder => groupBuilder
                    .AddBool("firstrun", "Initial Setup", new ProxyRef<bool>(() => FirstRunDone, o => FirstRunDone = o), boolBuilder => boolBuilder
                    .SetHintText("Uncheck this to re-run the Initial Setup and set all values back to the original")
                    .SetRequireRestart(true)));



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