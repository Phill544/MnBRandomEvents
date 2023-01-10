﻿using CryingBuffalo.RandomEvents.Helpers;
using Ini.Net;

namespace CryingBuffalo.RandomEvents.Settings
{
    public abstract class GeneralSettings
    {
        public abstract class DebugMode
        {
            public static bool IsActive()
            {
                var ConfigFile = new IniFile(ParseIniFile.GetTheFile());

                var debugMode = ConfigFile.ReadBoolean("GeneralSettings", "DebugMode");

                return debugMode;
            }
        }
        
        public abstract class SkillChecks
        {
            public static bool IsDisabled()
            {
                var ConfigFile = new IniFile(ParseIniFile.GetTheFile());

                var skillChecks = ConfigFile.ReadBoolean("GeneralSettings", "DisableSkillChecks");

                return skillChecks;
            }
        }
        
        public abstract class SupernaturalEvents
        {
            public static bool IsDisabled()
            {
                var ConfigFile = new IniFile(ParseIniFile.GetTheFile());

                var SupernaturalEvents = ConfigFile.ReadBoolean("GeneralSettings", "DisableSupernatural");

                return SupernaturalEvents;
            }
        }
        
        public abstract class Basic
        {
            public static int GetMinimumInGameHours()
            {
                var ConfigFile = new IniFile(ParseIniFile.GetTheFile());

                var MinimumInGameHours = ConfigFile.ReadInteger("GeneralSettings", "MinimumInGameHours");

                return MinimumInGameHours != 0 ? MinimumInGameHours : 24;
            }
            
            public static int GetMinimumRealMinutes()
            {
                var ConfigFile = new IniFile(ParseIniFile.GetTheFile());

                var MinimumRealMinutes = ConfigFile.ReadInteger("GeneralSettings", "MinimumRealMinutes");

                return MinimumRealMinutes != 0 ? MinimumRealMinutes : 5;
            }
            
            public static int GetMaximumRealMinutes()
            {
                var ConfigFile = new IniFile(ParseIniFile.GetTheFile());

                var MaximumRealMinutes = ConfigFile.ReadInteger("GeneralSettings", "MaximumRealMinutes");

                return MaximumRealMinutes != 0 ? MaximumRealMinutes : 30;
            }
            
            public static int GetLevelXpMultiplier()
            {
                var ConfigFile = new IniFile(ParseIniFile.GetTheFile());

                var GetLevelXpMultiplier = ConfigFile.ReadInteger("GeneralSettings", "LevelXpMultiplier");

                return GetLevelXpMultiplier != 0 ? GetLevelXpMultiplier : 30;
            }
        }
    }
}