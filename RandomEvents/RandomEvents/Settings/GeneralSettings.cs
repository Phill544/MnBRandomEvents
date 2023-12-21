using Bannerlord.RandomEvents.Helpers;
using Ini.Net;

namespace Bannerlord.RandomEvents.Settings
{
    public abstract class GeneralSettings
    {
        public abstract class SkillChecks
        {
            public static bool IsDisabled()
            {
                var ConfigFile = new IniFile(ParseIniFile.GetTheConfigFile());

                var skillChecks = ConfigFile.ReadBoolean("GeneralSettings", "DisableSkillChecks");

                return skillChecks;
            }
        }
        
        public abstract class SupernaturalEvents
        {
            public static bool IsDisabled()
            {
                var ConfigFile = new IniFile(ParseIniFile.GetTheConfigFile());

                var SupernaturalEvents = ConfigFile.ReadBoolean("GeneralSettings", "DisableSupernatural");

                return SupernaturalEvents;
            }
        }
        
        public abstract class Basic
        {
            public static int GetMinimumInGameHours()
            {
                var ConfigFile = new IniFile(ParseIniFile.GetTheConfigFile());

                var MinimumInGameHours = ConfigFile.ReadInteger("GeneralSettings", "MinimumInGameHours");

                return MinimumInGameHours != 0 ? MinimumInGameHours : 24;
            }
            
            public static int GetMinimumRealMinutes()
            {
                var ConfigFile = new IniFile(ParseIniFile.GetTheConfigFile());

                var MinimumRealMinutes = ConfigFile.ReadInteger("GeneralSettings", "MinimumRealMinutes");

                return MinimumRealMinutes != 0 ? MinimumRealMinutes : 5;
            }
            
            public static int GetMaximumRealMinutes()
            {
                var ConfigFile = new IniFile(ParseIniFile.GetTheConfigFile());

                var MaximumRealMinutes = ConfigFile.ReadInteger("GeneralSettings", "MaximumRealMinutes");

                return MaximumRealMinutes != 0 ? MaximumRealMinutes : 30;
            }
            
            public static int GetLevelXpMultiplier()
            {
                var ConfigFile = new IniFile(ParseIniFile.GetTheConfigFile());

                var GetLevelXpMultiplier = ConfigFile.ReadInteger("GeneralSettings", "LevelXpMultiplier");

                return GetLevelXpMultiplier != 0 ? GetLevelXpMultiplier : 30;
            }
        }
    }
}