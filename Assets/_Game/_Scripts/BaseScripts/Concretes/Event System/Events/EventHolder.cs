using UnityEngine.Events;

namespace PAG.Events
{
    /// <summary>
    /// A generic class that holds an event with a single argument of type T.
    /// </summary>
    /// <typeparam name="T">The type of the event argument.</typeparam>
    public class EventHolder<T>
    {
        // The Unity event that will be raised.
        public UnityAction<T> OnEvent;

        /// <summary>
        /// Creates a new instance of the EventHolder class.
        /// </summary>
        public EventHolder()
        {
            // This constructor is empty because there is no need to initialize anything in this class.
        }
    }
}