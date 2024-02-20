using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace PAG.Events
{
    public class EventManager
    {
        #region Variables

        // A list of EventCallbacks for events that should be automatically unregistered after being
        // raised once.
        public List<EventCallback> autoUnregisterEvents = new List<EventCallback>();

        // A dictionary that maps EventTypes to UnityActions that represent their callbacks.
        private Dictionary<EventTypes, UnityAction<EventArgs>> events = 
            new Dictionary<EventTypes, UnityAction<EventArgs>>();

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the EventManager by creating a dictionary of UnityActions for each EventType 
        /// and registering the AutoUnregister method for the LevelFinish event.
        /// </summary>
        public void Initialize()
        {
            foreach (EventTypes foo in Enum.GetValues(typeof(EventTypes)))
            {
                EventHolder<EventArgs> eventHolder = new EventHolder<EventArgs>();
                events.Add(foo, eventHolder.OnEvent);
            }

            // Automaticly unregisters all the events in autoUnregisterEvents list when level finished.
            Register(EventTypes.LevelFinish, AutoUnregister);
        }

        /// <summary>
        /// Invokes the specified event by invoking its associated UnityAction with the provided 
        /// EventArgs, if it exists.
        /// </summary>
        /// <param name="eventType">The EventType to invoke.</param>
        /// <param name="eventArgs">The EventArgs to provide to the event's callback function.</param>
        public void InvokeEvent(EventTypes eventType, EventArgs eventArgs = null)
        {
            events[eventType]?.Invoke(eventArgs);
        }

        /// <summary>
        /// Registers a callback function for the specified event, and optionally adds the event 
        /// to the auto-unregister list.
        /// </summary>
        /// <param name="eventType">The EventType to register the callback function for.</param>
        /// <param name="callback">The callback function to register for the event.</param>
        /// <param name="autoUnregister">Whether or not to add the event to the auto-unregister list.</param>
        public void Register(EventTypes eventType, UnityAction<EventArgs> callback, bool autoUnregister = false)
        {
            events[eventType] += callback;
            if (autoUnregister)
            {
                autoUnregisterEvents.Add(new EventCallback(eventType, callback));
            }
        }

        /// <summary>
        /// Unregisters a callback function for the specified event.
        /// </summary>
        /// <param name="eventType">The EventType to unregister the callback function from.</param>
        /// <param name="callback">The callback function to unregister from the event.</param>
        public void Unregister(EventTypes eventType, UnityAction<EventArgs> callback)
        {
            events[eventType] -= callback;
        }

        /// <summary>
        /// Unregisters all events in the auto-unregister list and clears the list.
        /// </summary>
        /// <param name="args">Unused parameter for the LevelFinish event callback.</param>
        public void AutoUnregister(EventArgs args)
        {
            foreach (var item in autoUnregisterEvents)
            {
                Unregister(item.eventType, item.callback);
            }
            autoUnregisterEvents.Clear();
        }
        #endregion
    }
}
