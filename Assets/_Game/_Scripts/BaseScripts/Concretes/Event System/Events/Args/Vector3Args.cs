using System;
using UnityEngine;

namespace PAG.Events
{
    /// <summary>
    /// A custom event arguments class used to represent a single Vector3 value.
    /// </summary>
    public class Vector3Args : EventArgs
    {
        // Three floats representing the x, y, and z components of a Vector3.
        public float x;
        public float y;
        public float z;

        // A temporary Vector3 object used to convert between Vector3 and Vector3Args.
        private Vector3 toVector3;

        /// <summary>
        /// Constructs a new instance of the Vector3Args class with default values.
        /// </summary>
        public Vector3Args()
        {

        }

        /// <summary>
        /// Constructs a new instance of the Vector3Args class with the specified Vector3 value.
        /// </summary>
        /// <param name="vector3">The Vector3 value to be represented by this event arguments object.</param>
        public Vector3Args(Vector3 vector3)
        {
            FromVector(vector3);
        }

        /// <summary>
        /// Constructs a new instance of the Vector3Args class with the specified x, y, and z components.
        /// </summary>
        /// <param name="x">The x component of the Vector3 value to be represented by this event arguments object.</param>
        /// <param name="y">The y component of the Vector3 value to be represented by this event arguments object.</param>
        /// <param name="z">The z component of the Vector3 value to be represented by this event arguments object.</param>
        public Vector3Args(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// Sets the x, y, and z components of this event arguments object based on the specified Vector3 value.
        /// </summary>
        /// <param name="vector3">The Vector3 value to be represented by this event arguments object.</param>
        public void FromVector(Vector3 vector3)
        {
            x = vector3.x;
            y = vector3.y;
            z = vector3.z;
        }

        /// <summary>
        /// Resets the x, y, and z components of this event arguments object to 0.
        /// </summary>
        public void Reset()
        {
            x = 0;
            y = 0;
            z = 0;
        }

        /// <summary>
        /// Returns a new Vector3 object with the same x, y, and z components as this event arguments object.
        /// </summary>
        /// <returns>A new Vector3 object with the same x, y, and z components as this event arguments object.</returns>
        public Vector3 ToVector()
        {
            toVector3.x = x;
            toVector3.y = y;
            toVector3.z = z;
            return toVector3;
        }
    }
}