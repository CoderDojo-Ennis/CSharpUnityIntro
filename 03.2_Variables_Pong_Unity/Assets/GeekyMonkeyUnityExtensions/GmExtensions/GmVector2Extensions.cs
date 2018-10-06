using System;
using System.Collections.Generic;
using UnityEngine;

namespace GeekyMonkey
{
    public static class GmVector2Extensions
    {
        /// <summary>
        /// Generate an integer hash for a vector 2 rounded to the nearest 0.1 unit. Will only work for values up to 1000
        /// </summary>
        /// <param name="v">The vector 3</param>
        /// <returns>Hash long with bits from x and y</returns>
        public static ulong Hash(this Vector2 v, float accuracy = 0.5f)
        {
            ulong hash;
            unchecked
            {
                int x2 = Mathf.RoundToInt(v.x / accuracy);
                int y2 = Mathf.RoundToInt(v.y / accuracy);
                hash = (((ulong)Math.Abs(x2)) << 33);
                hash |= (((ulong)Math.Abs(y2)) << 3);

                hash |= ((x2 < 0) ? 0u : 1u);
                hash |= ((y2 < 0) ? 0u : 2u);
            }
            return hash;
        }

        /// <summary>
        /// Is this vector nearly equal to another vector within a cube of variance
        /// </summary>
        /// <param name="a">This vectorr</param>
        /// <param name="b">Vector to compare to</param>
        /// <param name="maxDifference">Allowed difference</param>
        /// <returns>True if similar</returns>
        public static Boolean IsApproximately(this Vector2 a, Vector2 b, float maxDifference)
        {
            if (!a.x.IsApproximately(b.x, maxDifference))
            {
                return false;
            }
            return (a.y.IsApproximately(b.y, maxDifference));
        }

        /// <summary>
        /// Does the Vector2 have any infinity values
        /// </summary>
        /// <param name="point">Value to check</param>
        /// <returns>True if any part is infinity</returns>
        public static bool IsInfinity(this Vector2 point)
        {
            return (float.IsInfinity(point.x) || float.IsInfinity(point.y));
        }

        /* todo
        /// <summary>
        /// Rotate this point around a pivot point
        /// </summary>
        /// <param name="point">This point</param>
        /// <param name="pivot">Pivot point</param>
        /// <param name="angles">Euler Angles (degrees)</param>
        /// <returns>Transformed point</returns>
        public static Vector3 RotatePointAroundPivot(this Vector3 point, Vector3 pivot, Vector3 angles)
        {
            Vector3 dir = point - pivot; // get point direction relative to pivot
            dir = Quaternion.Euler(angles) * dir; // rotate it
            point = dir + pivot; // calculate rotated point
            return point; // return it
        }
        /// <summary>
        /// Rotate this point around a pivot point
        /// </summary>
        /// <param name="point">This point</param>
        /// <param name="pivot">Pivot point</param>
        /// <param name="angles">Quaterion Angles</param>
        /// <returns>Transformed point</returns>
        public static Vector3 RotatePointAroundPivot(this Vector3 point, Vector3 pivot, Quaternion angles)
        {
            Vector3 dir = point - pivot; // get point direction relative to pivot
            dir = angles * dir; // rotate it
            point = dir + pivot; // calculate rotated point
            return point; // return it
        }
        */

        /// <summary>
        /// Set the X value only
        /// </summary>
        /// <param name="v">Vector2 to modify</param>
        /// <param name="x">New X value</param>
        public static Vector2 WithX(this Vector2 v, float x)
        {
            v.Set(x, v.y);
            return v;
        }

        /// <summary>
        /// Set the Y value only
        /// </summary>
        /// <param name="v">Vector2 to modify</param>
        /// <param name="y">New Y value</param>
        public static void SetY(this Vector2 v, float y)
        {
            v.Set(v.x, y);
        }

        /// <summary>
        /// Get the mean from a list of vectors
        /// </summary>
        /// <param name="positions">Vectors to calculate from</param>
        /// <returns>Mean Vector</returns>
        public static Vector3 GetMeanVector(this IList<Vector2> positions)
        {
            int vectorCount = positions.Count;
            if (vectorCount == 0)
            {
                return Vector2.zero;
            }

            Vector2 sum = Vector2.zero;
            int count = 0;
            for (int i = 0; i < vectorCount; i++)
            {
                if (!positions[i].IsInfinity())
                {
                    sum += positions[i];
                    count++;
                }
            }
            return sum / count;
        }

        /// <summary>
        /// Snap Vector2 to nearest grid position
        /// </summary>
        /// <param name="vector2">Sloppy position</param>
        /// <param name="gridSize">Grid size</param>
        /// <returns>Snapped position</returns>
        public static Vector2 Snap(this Vector2 vector2, float gridSize = 1.0f)
        {
            if (gridSize == 0f)
            {
                return vector2;
            }

            return new Vector2(
                Mathf.Round(vector2.x / gridSize) * gridSize,
                Mathf.Round(vector2.y / gridSize) * gridSize);
        }

        /// <summary>
        /// Snap Vector3 to nearest grid position with offset
        /// </summary>
        /// <param name="vector2">Sloppy position</param>
        /// <param name="gridSize">Grid size</param>
        /// <returns>Snapped position</returns>
        public static Vector2 SnapOffset(this Vector2 vector2, Vector2 offset, float gridSize = 1.0f)
        {
            if (gridSize == 0f)
            {
                return vector2;
            }

            Vector2 snapped = vector2 - offset;
            snapped = new Vector2(
                Mathf.Round(snapped.x / gridSize) * gridSize,
                Mathf.Round(snapped.y / gridSize) * gridSize);
            return snapped + offset;
        }
    }
}
