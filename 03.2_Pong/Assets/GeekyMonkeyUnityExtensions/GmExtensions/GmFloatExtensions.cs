using System;
using UnityEngine;

namespace GeekyMonkey
{
    public static class GmFloatExtensions
    {
        /// <summary>
        /// Is this number nearly equal to another number
        /// </summary>
        /// <param name="a">This number</param>
        /// <param name="b">Number to compare to</param>
        /// <param name="maxDifference">Allowed difference</param>
        /// <returns>True if similar</returns>
        public static Boolean IsApproximately(this float a, float b, float maxDifference)
        {
            return ((a < b) ? (b - a) : (a - b)) <= maxDifference;
        }

        /// <summary>
        /// Clamp a float number to a given range with a simpler syntax: MyFloat.Clamp(0, 1)
        /// </summary>
        /// <param name="val">Current value</param>
        /// <param name="min">Minimum value</param>
        /// <param name="max">Maximum value</param>
        /// <returns>New clamped value (original variable is unchanged)</returns>
        public static float Clamp(this float val, float min, float max)
        {
            if (max < min)
            {
                (min, max) = (max, min);
            }
            return Mathf.Clamp(val, min, max);
        }
    }
}
