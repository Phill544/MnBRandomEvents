using Newtonsoft.Json;
using System;

namespace CryingBuffalo.RandomEvents.Events
{

	public abstract class BaseEvent
	{
		/// <summary>
		/// The data associated with the random event
		/// </summary>
		public RandomEventData RandomEventData = null;

		/// <summary>
		/// The code that's called when the event has been completed
		/// </summary>
		public Action OnEventCompleted;

		/// <summary>
		/// Called to initialise the event
		/// </summary>
		public abstract void StartEvent();

		/// <summary>
		/// Called to clean up the event once completed
		/// </summary>
		public abstract void StopEvent();

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
			RandomEventData = randomEventData;
		}
	}

	public class RandomEventData
	{
		/// <summary>
		/// ID of the event
		/// </summary>
		[JsonIgnore]
		public RandomEventType EventType = RandomEventType.Unknown;

		/// <summary>
		/// The weighted value that this event will be selected
		/// </summary>
		public float ChanceWeight;

		public RandomEventData(RandomEventType eventType, float chanceWeight)
		{
			EventType = eventType;
			ChanceWeight = chanceWeight;
		}
	}
}
