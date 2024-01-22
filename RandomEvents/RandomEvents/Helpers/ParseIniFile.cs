using System.IO;
using System.Reflection;
using TaleWorlds.Library;

namespace Bannerlord.RandomEvents.Helpers
{
    public abstract class ParseIniFile
    {
        public static string GetTheConfigFile()
        {
            var strExeFilePath = Assembly.GetExecutingAssembly().Location;
            var strWorkPath = Path.GetDirectoryName(strExeFilePath);
            // ReSharper disable once AssignNullToNotNullAttribute
            var finalPath = Path.GetFullPath(Path.Combine(strWorkPath, @"..\..\ModuleData\RandomEvents_EventConfiguration.ini"));

            if (!File.Exists(finalPath))
            {
                InformationManager.DisplayMessage(new InformationMessage("Random Events ini file not found, Generating a new one.", RandomEventsSubmodule.Ini_Color));

                CreateDefaultIniFile(finalPath);
            }

            return finalPath;
        }

        private static void CreateDefaultIniFile(string filePath)
        {
            File.WriteAllText(filePath, DefaultIni.Content());
        }
    }
}