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
		public bool DebugMode { get; private set; } = true;

		[JsonProperty]
		public bool HideInaccessibleDialogue { get; private set; } = true;

		[JsonProperty]
		public int HintLevel { get; private set; } = 1;

		public float GeneralLevelXpMultiplier { get; private set; } = 40;
	}
}
