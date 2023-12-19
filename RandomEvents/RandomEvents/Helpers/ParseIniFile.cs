using System.IO;
using System.Reflection;

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