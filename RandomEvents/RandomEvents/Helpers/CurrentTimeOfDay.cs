using TaleWorlds.CampaignSystem;

namespace Bannerlord.RandomEvents.Helpers
{
    public abstract class CurrentTimeOfDay
    {
        private static int CurrentHour => CampaignTime.Now.GetHourOfDay;

        public static bool IsNight => CurrentHour < 6;

        public static bool IsMorning => CurrentHour >= 6 && CurrentHour < 12;

        public static bool IsMidday => CurrentHour >= 12 && CurrentHour < 18;

        public static bool IsEvening => CurrentHour >= 18;
    }
}