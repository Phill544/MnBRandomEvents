using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;

namespace CryingBuffalo.RandomEvents.Helpers
{
	public static class RandomSelection<T>
	{
		public static T GetRandomElement(IEnumerable<T> enumerable)
		{
			return enumerable.ElementAt(MBRandom.RandomInt(0, enumerable.Count()));
		}
	}
}
