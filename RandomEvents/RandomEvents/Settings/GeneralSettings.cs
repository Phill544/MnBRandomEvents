using Newtonsoft.Json;

namespace CryingBuffalo.RandomEvents.Settings
{
    internal class GeneralSettings
    {

        [JsonProperty]
        public bool HideInaccessibleDialogue { get; private set; } = true;

        [JsonProperty]
        public int HintLevel { get; private set; } = 1;
    }
}