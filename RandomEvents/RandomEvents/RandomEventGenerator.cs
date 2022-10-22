using CryingBuffalo.RandomEvents.Events;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;

namespace CryingBuffalo.RandomEvents
{
	public class RandomEventGenerator 
	{
		private struct WeightedEventData
		{
			public float accumulatedWeight;
			public RandomEventData RandomEventData;
		}

		private List<WeightedEventData> weightedEvents = new List<WeightedEventData>();
		private float accumulatedWeight;

		private void AddEvent(RandomEventData data)
		{
			accumulatedWeight += data.chanceWeight;
			weightedEvents.Add(new WeightedEventData { RandomEventData = data, accumulatedWeight = accumulatedWeight });
		}

		public void AddEvents(IEnumerable<RandomEventData> data)
		{
			foreach (var eventData in data)
			{
				AddEvent(eventData);
			}
		}

		public RandomEventData GetRandom()
		{
			float rand = MBRandom.RandomFloatRanged(0.0f, accumulatedWeight);

			return (from weightedEvent in weightedEvents where weightedEvent.accumulatedWeight >= rand select weightedEvent.RandomEventData).FirstOrDefault();
		}

		public RandomEventData GetEvent(string id)
		{
			return weightedEvents.FirstOrDefault((x) => x.RandomEventData.eventType.ToLower() == id.ToLower()).RandomEventData;
		}
	}
}
