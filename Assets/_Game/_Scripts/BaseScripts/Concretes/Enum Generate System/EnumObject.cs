using UnityEngine;
using Sirenix.OdinInspector;
using PAG.Editor;

namespace PAG.Utility
{
    /// <summary>
    /// A scriptable object that represents an enum with a specified name and elements.
    /// </summary>
    [CreateAssetMenu(fileName = "Enum List", menuName = "Scriptable Objects/Enum Object")]
    public class EnumObject : ScriptableObject
    {
        // The name of the enum.
        public string enumName;

        // The elements of the enum.
        public string[] elements;

        // A button that generates or updates the enum when clicked.
        [Button("Create/Update Enum")]
        public void CreateEnum()
        {
            // Generate or update the enum using the specified name and elements.
            EnumGenerator.Generate(enumName, elements);
        }
    }
}
