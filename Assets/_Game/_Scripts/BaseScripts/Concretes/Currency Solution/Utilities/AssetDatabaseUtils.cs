using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace PAG.Utility
{
    /// <summary>
    /// A static class containing utility methods for interacting with the Unity Asset Database.
    /// </summary>
    public static class AssetDatabaseUtils
    {
#if UNITY_EDITOR
        /// <summary>
        /// Finds all assets of the specified type in the Unity Asset Database.
        /// </summary>
        /// <typeparam name="T">The type of asset to search for.</typeparam>
        /// <returns>A list of all assets of the specified type in the project.</returns>
        /// <remarks>
        /// This method searches the entire project for assets of the specified type, so it may take some time
        /// to execute for large projects. Use it sparingly and consider caching the results if you need to call
        /// it frequently.
        /// </remarks>
        public static List<T> FindAssetsByType<T>() where T : UnityEngine.Object
        {
            // Construct a query string to find all assets of the specified type
            string query = "t:" + typeof(T).Name;

            // Find all asset GUIDs that match the query
            string[] assetGUIDs = AssetDatabase.FindAssets(query);

            // Create a list to store the matching assets
            List<T> assets = new List<T>();

            // Iterate over all matching asset GUIDs
            for (int i = 0; i < assetGUIDs.Length; i++)
            {
                // Get the asset path for the current GUID
                string assetPath = AssetDatabase.GUIDToAssetPath(assetGUIDs[i]);

                // Load the asset at the path
                T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);

                // If the asset is not null, add it to the list
                if (asset != null)
                {
                    Debug.Log("Found asset of type " + typeof(T).Name + " at path: " + assetPath);
                    assets.Add(asset);
                }
            }

            // Return the list of matching assets
            return assets;
        }
#endif
    }
}