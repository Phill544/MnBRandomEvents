using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryingBuffalo.RandomEvents
{
	class GeneralSettings
	{
		[JsonIgnore]
		public bool DebugMode { get; private set; } = false;

		[JsonProperty]
		public bool HideInaccessibleDialogue { get; private set; } = true;

		[JsonProperty]
		public int HintLevel { get; private set; } = 1;

		[JsonProperty]
		public int MinimumInGameHours { get; private set; } = 120;

		[JsonProperty]
		public int MinimumRealMinutes { get; private set; } = 10;

		[JsonProperty]
		public int MaximumRealMinutes { get; private set; } = 20;

		[JsonProperty]
		public float GeneralLevelXpMultiplier { get; private set; } = 40;
	}
}
