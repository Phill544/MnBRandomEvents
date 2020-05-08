using CryingBuffalo.RandomEvents.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

		public void AddEvent(RandomEventData data)
		{
			accumulatedWeight += data.ChanceWeight;
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

			foreach (WeightedEventData weightedEvent in weightedEvents)
			{
				if (weightedEvent.accumulatedWeight >= rand)
				{
					return weightedEvent.RandomEventData;
				}
			}
			return null;
		}

		public RandomEventData GetEvent(string id)
		{
			return weightedEvents.FirstOrDefault((x) => x.RandomEventData.EventType.ToLower() == id.ToLower()).RandomEventData;
		}
	}
}
