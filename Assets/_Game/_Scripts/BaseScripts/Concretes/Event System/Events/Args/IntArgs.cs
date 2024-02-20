using System;

namespace PAG.Events
{
    /// <summary>
    /// A custom event arguments class used to represent a single integer value.
    /// </summary>
    public class IntArgs : EventArgs
    {
        /// <summary>
        /// The integer value represented by this event arguments object.
        /// </summary>
        public int value;

        /// <summary>
        /// Constructs a new instance of the IntArgs class with the specified integer value.
        /// </summary>
        /// <param name="v">The integer value to be represented by this event arguments object.</param>
        public IntArgs(int v)
        {
            this.value = v;
        }
    }
}
