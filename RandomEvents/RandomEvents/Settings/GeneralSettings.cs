using Newtonsoft.Json;

namespace CryingBuffalo.RandomEvents.Settings
{
    internal class GeneralSettings
    {
        [JsonIgnore]
        public bool DebugMode { get; private set; } = false;

        [JsonProperty]
        public bool HideInaccessibleDialogue { get; private set; } = true;

        [JsonProperty]
        public int HintLevel { get; private set; } = 1;
    }
}