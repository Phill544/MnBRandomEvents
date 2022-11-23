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
                var Hour = CampaignTime.Now.ToHours;
                var num = MathF.Floor(Hour);

                return num < 6;
            }
        }


        public static bool IsMorning
        {
            get
            {
                var Hour = CampaignTime.Now.ToHours;
                var num = MathF.Floor(Hour);

                return num >= 6 && num < 12;
            }
        }
        
        public static bool IsMidday
        {
            get
            {
                var Hour = CampaignTime.Now.ToHours;
                var num = MathF.Floor(Hour);

                return num >= 12 && num < 18;
            }
        }
        
        public static bool IsEvening
        {
            get
            {
                var Hour = CampaignTime.Now.ToHours;
                var num = MathF.Floor(Hour);

                return num >= 18;
            }
        }
    }
}