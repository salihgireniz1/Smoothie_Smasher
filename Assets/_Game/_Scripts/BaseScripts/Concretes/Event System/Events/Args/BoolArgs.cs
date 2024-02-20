using System;

namespace PAG.Events
{
    /// <summary>
    /// A custom event arguments class used to represent a single boolean value.
    /// </summary>
    public class BoolArgs : EventArgs
    {
        /// <summary>
        /// The boolean value represented by this event arguments object.
        /// </summary>
        public bool value;

        /// <summary>
        /// Constructs a new instance of the BoolArgs class with the specified boolean value.
        /// </summary>
        /// <param name="v">The boolean value to be represented by this event arguments object.</param>
        public BoolArgs(bool v)
        {
            this.value = v;
        }
    }
}