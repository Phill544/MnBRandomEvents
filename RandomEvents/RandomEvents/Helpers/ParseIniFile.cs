using System.IO;
using System.Reflection;

namespace CryingBuffalo.RandomEvents.Helpers
{
    public abstract class ParseIniFile
    {
        public static string GetTheConfigFile()
        {
            var strExeFilePath = Assembly.GetExecutingAssembly().Location;
            var strWorkPath = Path.GetDirectoryName(strExeFilePath);

            var finalPath = Path.GetFullPath(Path.Combine(strWorkPath, @"..\..\ModuleData\RandomEvents_EventConfiguration.ini"));
            
            return finalPath;
        }
    }
}