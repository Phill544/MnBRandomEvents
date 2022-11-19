﻿using TaleWorlds.Localization;

namespace CryingBuffalo.RandomEvents.Helpers
{
    //In English, demonyms are always capitalized.
    public static class Demonym
    {
        public static string GetTheDemonym(string culture, bool noun)
        {
            var citizenName = noun ? culture switch
                {
                    "Empire" => new TextObject("{=Demonym_Empire_Noun}an Imperial").ToString(),
                    "Vlandia" => new TextObject("{=Demonym_Vlandia_Noun}a Vlandian").ToString(),
                    "Sturgia" => new TextObject("{=Demonym_Sturgia_Noun}a Sturgian").ToString(),
                    "Battania" => new TextObject("{=Demonym_Battania_Noun}a Battanian").ToString(),
                    "Aserai" => new TextObject("{=Demonym_Aserai_Noun}an Aserai").ToString(),
                    "Khuzait" => new TextObject("{=Demonym_Khuzait_Noun}a Khuzait").ToString(),
                    _ => new TextObject("{=Demonym_Error}Error - Could not fetch Demonym").ToString()
                }
                : culture switch
                {
                    "Empire" => new TextObject("{=Demonym_Empire}Imperial").ToString(),
                    "Vlandia" => new TextObject("{=Demonym_Vlandia}Vlandian").ToString(),
                    "Sturgia" => new TextObject("{=Demonym_Sturgia}Sturgian").ToString(),
                    "Battania" => new TextObject("{=Demonym_Battania}Battanian").ToString(),
                    "Aserai" => new TextObject("{=Demonym_Aserai}Aserai").ToString(),
                    "Khuzait" => new TextObject("{=Demonym_Khuzait}Khuzait").ToString(),
                    _ => new TextObject("{=Demonym_Error}Error - Could not fetch Demonym").ToString()
                };

            return citizenName;
        }
    }
}