using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;

namespace Bannerlord.RandomEvents.Helpers
{
    public static class RandomSelection<T>
    {
        public static T GetRandomElement(IEnumerable<T> enumerable)
        {
            var enumerable1 = enumerable as T[] ?? enumerable.ToArray();
            return enumerable1.ElementAt(MBRandom.RandomInt(0, enumerable1.Length));
        }
    }
}