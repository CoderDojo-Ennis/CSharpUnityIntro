using System;
using System.Collections.Generic;
using UnityEngine;

namespace GeekyMonkey
{
    public static class GmVector3Extensions
    {
        /// <summary>
        /// Is this vector nearly equal to another vector within a cube of variance
        /// </summary>
        /// <param name="a">This vectorr</param>
        /// <param name="b">Vector to compare to</param>
        /// <param name="maxDifference">Allowed difference</param>
        /// <returns>True if similar</returns>
        public static Boolean IsApproximately(this Vector3 a, Vector3 b, float maxDifference)
        {
            if (!a.x.IsApproximately(b.x, maxDifference))
            {
                return false;
            }
            if (!a.y.IsApproximately(b.y, maxDifference))
            {
                return false;
            }
            return (a.z.IsApproximately(b.z, maxDifference));
        }

        /// <summary>
        /// Generate an integer hash for a vector 3 rounded to the nearest 0.1 unit. Will only work for values up to 1000
        /// </summary>
        /// <param name="v">The vector 3</param>
        /// <returns>Hash long with bits from x, y, and z</returns>
        public static ulong Hash(this Vector3 v, float accuracy = 0.5f)
        {
            ulong hash;
            unchecked
            {
                int x2 = Mathf.RoundToInt(v.x / accuracy);
                int y2 = Mathf.RoundToInt(v.y / accuracy);
                int z2 = Mathf.RoundToInt(v.z / accuracy);
                hash = (((ulong)Math.Abs(x2)) << 43);
                hash |= (((ulong)Math.Abs(y2)) << 23);
                hash |= (((ulong)Math.Abs(z2)) << 3);

                hash |= ((x2 < 0) ? 0u : 1u);
                hash |= ((y2 < 0) ? 0u : 2u);
                hash |= ((z2 < 0) ? 0u : 4u);
            }
            return hash;
        }

        /// <summary>
        /// Does the Vector3 have any infinity values
        /// </summary>
        /// <param name="point">Value to check</param>
        /// <returns>True if any part is infinity</returns>
        public static bool IsInfinity(this Vector3 point)
        {
            return (float.IsInfinity(point.x) || float.IsInfinity(point.y) || float.IsInfinity(point.z));
        }

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

        /// <summary>
        /// Return a new Vector3 with a new X value
        /// </summary>
        /// <param name="v">Vector3 to copy from</param>
        /// <param name="x">New X value</param>
        /// <returns>New Vector3 that's a copy of the original, but with a new X value</returns>
        /// <remarks>Unfortunately, structs can't have extension methods that modify the values in C# as of 2018</remarks>
        public static Vector3 WithX(this Vector3 v, float x)
        {
            v.Set(x, v.y, v.z);
            return v;
        }

        /// <summary>
        /// Return a new Vector3 with a new Y value
        /// </summary>
        /// <param name="v">Vector3 to copy from</param>
        /// <param name="x">New Y value</param>
        /// <returns>New Vector3 that's a copy of the original, but with a new Y value</returns>
        /// <remarks>Unfortunately, structs can't have extension methods that modify the values in C# as of 2018</remarks>
        public static Vector3 WithY(this Vector3 v, float y)
        {
            v.Set(v.x, y, v.z);
            return v;
        }

        /// <summary>
        /// Return a new Vector3 with a new Z value
        /// </summary>
        /// <param name="v">Vector3 to copy from</param>
        /// <param name="x">New Z value</param>
        /// <returns>New Vector3 that's a copy of the original, but with a new Z value</returns>
        /// <remarks>Unfortunately, structs can't have extension methods that modify the values in C# as of 2018</remarks>
        public static Vector3 WithZ(this Vector3 v, float z)
        {
            v.Set(v.x, v.y, z);
            return v;
        }

        /// <summary>
        /// Get the mean from a list of vectors
        /// </summary>
        /// <param name="positions">Vectors to calculate from</param>
        /// <returns>Mean Vector</returns>
        public static Vector3 GetMeanVector(this IList<Vector3> positions)
        {
            int vectorCount = positions.Count;
            if (vectorCount == 0)
            {
                return Vector3.zero;
            }

            Vector3 sum = Vector3.zero;
            int count = 0;
            for (int i = 0; i < vectorCount; i++)
            {
                if (!positions[i].IsInfinity())
                {
                    sum += positions[i];
                    count++;
                }
            }
            return sum / vectorCount;
        }

        /// <summary>
        /// Snap Vector3 to nearest grid position
        /// </summary>
        /// <param name="vector3">Sloppy position</param>
        /// <param name="gridSize">Grid size</param>
        /// <returns>Snapped position</returns>
        public static Vector3 Snap(this Vector3 vector3, float gridSize = 1.0f)
        {
            if (gridSize == 0f)
            {
                return vector3;
            }

            return new Vector3(
                Mathf.Round(vector3.x / gridSize) * gridSize,
                Mathf.Round(vector3.y / gridSize) * gridSize,
                Mathf.Round(vector3.z / gridSize) * gridSize);
        }

        /// <summary>
        /// Snap Vector3 to nearest grid position with offset
        /// </summary>
        /// <param name="vector3">Sloppy position</param>
        /// <param name="gridSize">Grid size</param>
        /// <returns>Snapped position</returns>
        public static Vector3 SnapOffset(this Vector3 vector3, Vector3 offset, float gridSize = 1.0f)
        {
            if (gridSize == 0f)
            {
                return vector3;
            }

            Vector3 snapped = vector3 - offset;
            snapped = new Vector3(
                Mathf.Round(snapped.x / gridSize) * gridSize,
                Mathf.Round(snapped.y / gridSize) * gridSize,
                Mathf.Round(snapped.z / gridSize) * gridSize);
            return snapped + offset;
        }
    }
}
