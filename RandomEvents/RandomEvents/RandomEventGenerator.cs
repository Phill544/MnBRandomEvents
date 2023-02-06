using System.Collections.Generic;
using System.Linq;
using CryingBuffalo.RandomEvents.Events;
using TaleWorlds.Core;

namespace CryingBuffalo.RandomEvents
{
    public class RandomEventGenerator 
    {
        private struct WeightedEventData
        {
            public float accumulatedWeight;
            public RandomEventData randomEventData;
        }

        private readonly List<WeightedEventData> weightedEvents = new List<WeightedEventData>();
        private float accumulatedWeight;

        private void AddEvent(RandomEventData data)
        {
            accumulatedWeight += data.chanceWeight;
            weightedEvents.Add(new WeightedEventData { randomEventData = data, accumulatedWeight = accumulatedWeight });
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
            var rand = MBRandom.RandomFloatRanged(0.0f, accumulatedWeight);

            return (from weightedEvent in weightedEvents where weightedEvent.accumulatedWeight >= rand select weightedEvent.randomEventData).FirstOrDefault();
        }

        public RandomEventData GetEvent(string id)
        {
            return weightedEvents.FirstOrDefault(x => x.randomEventData.eventType.ToLower() == id.ToLower()).randomEventData;
        }
        
        
    }
}