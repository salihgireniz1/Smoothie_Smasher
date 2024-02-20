using System;
using UnityEngine.Events;

namespace PAG.Events
{
    /// <summary>
    /// A class that represents a callback function for a specific event type.
    /// </summary>
    public class EventCallback
    {
        // The type of event that this callback is for.
        public EventTypes eventType;

        // The callback function that will be invoked when the event is raised.
        public UnityAction<EventArgs> callback;

        /// <summary>
        /// Creates a new instance of the EventCallback class with the specified event type and callback function.
        /// </summary>
        /// <param name="eventType">The type of event that this callback is for.</param>
        /// <param name="callback">The callback function that will be invoked when the event is raised.</param>
        public EventCallback(EventTypes eventType, UnityAction<EventArgs> callback)
        {
            this.eventType = eventType;
            this.callback = callback;
        }
    }
}
