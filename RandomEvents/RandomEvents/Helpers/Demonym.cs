namespace CryingBuffalo.RandomEvents.Helpers
{
    public static class Demonym
    {
        public static string GetTheDemonym(string culture, bool noun)
        {
            var citizenName = noun ? culture switch
                {
                    "Empire" => "an imperial",
                    "Vlandia" => "a vlandian",
                    "Sturgia" => "a sturgian",
                    "Battania" => "a battanian",
                    "Aserai" => "an aserai",
                    "Khuzait" => "a khuzait",
                    _ => "ERROR_DEMONYM"
                }
                : culture switch
                {
                    "Empire" => "imperial",
                    "Vlandia" => "vlandian",
                    "Sturgia" => "sturgian",
                    "Battania" => "battanian",
                    "Aserai" => "aserai",
                    "Khuzait" => "khuzait",
                    _ => "ERROR_DEMONYM"
                };

            return citizenName;
        }
    }
}