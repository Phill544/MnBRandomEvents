using System.IO;

namespace CryingBuffalo.RandomEvents.Helpers
{
    public abstract class ParseIniFile
    {
        public static string GetTheFile()
        {
            var strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var strWorkPath = Path.GetDirectoryName(strExeFilePath);

            var finalPath = Path.GetFullPath(Path.Combine(strWorkPath, @"..\..\ModuleData\RandomEvents_EventConfiguration.ini"));
            
            return finalPath;
        }
    }
}