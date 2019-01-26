using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{

    public static class UtilityFunctions
    {
        /// <summary>
        /// Shuffles a list in place.
        /// </summary>
        public static void Shuffle<T>(this IList<T> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                T element = list[i];
                int newPosition = UnityEngine.Random.Range(0, list.Count);
                T otherElement = list[newPosition];
                list[i] = otherElement;
                list[newPosition] = element;
            }
        }

        /// <summary>Returns a random element from the list.</summary>
        public static T GetRandomElement<T>(this IList<T> list, T defaultValue = default(T))
        {
            if (list.Count > 0)
            {
                int index = UnityEngine.Random.Range(0, list.Count);
                return list[index];
            }
            return defaultValue;
        }
    }
}
