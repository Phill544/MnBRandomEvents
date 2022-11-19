using TaleWorlds.Localization;

namespace CryingBuffalo.RandomEvents.Helpers
{
    public static class Demonym
    {
        public static string GetTheDemonym(string culture, bool noun)
        {
            var citizenName = noun ? culture switch
                {
                    "Empire" => new TextObject("{=Demonym_Empire_Noun}an imperial").ToString(),
                    "Vlandia" => new TextObject("{=Demonym_Vlandia_Noun}a vlandian").ToString(),
                    "Sturgia" => new TextObject("{=Demonym_Sturgia_Noun}a sturgian").ToString(),
                    "Battania" => new TextObject("{=Demonym_Battania_Noun}a battanian").ToString(),
                    "Aserai" => new TextObject("{=Demonym_Aserai_Noun}an aserai").ToString(),
                    "Khuzait" => new TextObject("{=Demonym_Khuzait_Noun}a khuzait").ToString(),
                    _ => new TextObject("{=Demonym_Error}Error - Could not fetch Demonym").ToString()
                }
                : culture switch
                {
                    "Empire" => new TextObject("{=Demonym_Empire}imperial").ToString(),
                    "Vlandia" => new TextObject("{=Demonym_Vlandia}vlandian").ToString(),
                    "Sturgia" => new TextObject("{=Demonym_Sturgia}sturgian").ToString(),
                    "Battania" => new TextObject("{=Demonym_Battania}battanian").ToString(),
                    "Aserai" => new TextObject("{=Demonym_Aserai}aserai").ToString(),
                    "Khuzait" => new TextObject("{=Demonym_Khuzait}khuzait").ToString(),
                    _ => new TextObject("{=Demonym_Error}Error - Could not fetch Demonym").ToString()
                };

            return citizenName;
        }
    }
}