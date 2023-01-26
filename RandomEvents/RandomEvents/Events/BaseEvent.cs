using System;

namespace CryingBuffalo.RandomEvents.Events
{

    public abstract class BaseEvent
    {
        /// <summary>
        /// The data associated with the random event
        /// </summary>
        public readonly RandomEventData randomEventData;

        /// <summary>
        /// The code that's called when the event has been completed
        /// </summary>
        public Action onEventCompleted;

        /// <summary>
        /// Called to initialise the event
        /// </summary>
        public abstract void StartEvent();

        /// <summary>
        /// Called when the event is canceled early
        /// </summary>
        public abstract void CancelEvent();

        /// <summary>
        /// Determines whether all of the prerequisites have been met in order to run the event
        /// </summary>
        /// <returns></returns>
        public abstract bool CanExecuteEvent();

        protected BaseEvent(RandomEventData randomEventData)
        {
            this.randomEventData = randomEventData;
        }
    }

    public abstract class RandomEventData
    {
        /// <summary>
        /// ID of the event
        /// </summary>
        public readonly string eventType;

        /// <summary>
        /// The weighted value that this event will be selected
        /// </summary>
        public readonly float chanceWeight;

        protected RandomEventData(string eventType, float chanceWeight)
        {
            this.eventType = eventType;
            this.chanceWeight = chanceWeight;
        }

        public abstract BaseEvent GetBaseEvent();

    }
}