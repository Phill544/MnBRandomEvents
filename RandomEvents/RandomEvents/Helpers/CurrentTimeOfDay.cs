using TaleWorlds.CampaignSystem;
using TaleWorlds.Library;

namespace CryingBuffalo.RandomEvents.Helpers
{
    public abstract class CurrentTimeOfDay
    {
        public static bool IsNight
        {
            get
            {
                var num = CampaignTime.Now.GetHourOfDay;
                return num < 6;
            }
        }


        public static bool IsMorning
        {
            get
            {
                var num = CampaignTime.Now.GetHourOfDay;
                return num >= 6 && num < 12;
            }
        }
        
        public static bool IsMidday
        {
            get
            {
                var num = CampaignTime.Now.GetHourOfDay;
                return num >= 12 && num < 18;
            }
        }
        
        public static bool IsEvening
        {
            get
            {
                var num = CampaignTime.Now.GetHourOfDay;
                return num >= 18;
            }
        }
    }
}