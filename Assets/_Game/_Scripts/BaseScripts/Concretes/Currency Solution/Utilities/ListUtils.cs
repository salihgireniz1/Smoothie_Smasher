using System.Collections.Generic;

namespace PAG.Utility
{
    /// <summary>
    /// A static utility class for working with lists.
    /// </summary>
    public static class ListUtils
    {
        /// <summary>
        /// Generates a list of string representations of the items in the input list,
        /// with whitespace removed from each item's ToString() result.
        /// </summary>
        /// <typeparam name="T">The type of the items in the input list.</typeparam>
        /// <param name="items">The input list of items.</param>
        /// <returns>A new list of string representations of the input items, with whitespace removed.</returns>
        public static List<string> GenerateStringList<T>(List<T> items)
        {
            List<string> elements = new List<string>(items.Count);
            for (int i = 0; i < items.Count; i++)
            {
                // Remove any whitespace or blank characters from the string representation of the item
                string itemName = items[i].ToString().Replace(" ", string.Empty);
                elements.Add(itemName);
            }
            return elements;
        }
    }
}
