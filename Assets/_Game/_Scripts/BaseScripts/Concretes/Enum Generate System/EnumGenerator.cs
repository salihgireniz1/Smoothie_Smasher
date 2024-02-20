using UnityEngine;
using UnityEditor;
using System.IO;

namespace PAG.Editor
{
    /// <summary>
    /// Utility class for generating enums with specified values.
    /// </summary>
    public class EnumGenerator : MonoBehaviour
    {
        /// <summary>
        /// Generates a new enum with the specified name and elements.
        /// </summary>
        /// <param name="eName">The name of the enum to generate.</param>
        /// <param name="elements">An array of string values to use as the enum elements.</param>
        public static void Generate(string eName, string[] elements)
        {
            // Construct the file path and name based on the specified enum name.
            string folderPath = "Assets/_Game/_Scripts/Enums/";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            string filePathAndName = folderPath + eName + ".cs";

            // Write the enum definition to the file.
            using (StreamWriter streamWriter = new StreamWriter(filePathAndName))
            {
                // Write the enum declaration line.
                streamWriter.WriteLine("public enum " + eName);
                streamWriter.WriteLine("{");

                // Write each element on a separate line.
                for (int i = 0; i < elements.Length; i++)
                {
                    if (i < elements.Length - 1)
                    {
                        streamWriter.WriteLine("\t" + elements[i] + ",");
                    }
                    else
                    {
                        streamWriter.WriteLine("\t" + elements[i]);
                    }
                }

                // Close the enum definition.
                streamWriter.WriteLine("}");
            }
#if UNITY_EDITOR
            // Refresh the asset database so the new enum is visible in the project.
            AssetDatabase.Refresh();
#endif
        }

    }
}
