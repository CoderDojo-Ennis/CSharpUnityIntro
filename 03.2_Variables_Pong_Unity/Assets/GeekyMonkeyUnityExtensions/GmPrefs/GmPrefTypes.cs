using UnityEngine;

namespace GeekyMonkey
{
    /// <summary>
    /// GmPref that stores a float value
    /// </summary>
    public class FloatPref : GmPref<float>
    {
    };

    /// <summary>
    /// GmPref that stores an int value
    /// </summary>
    public class IntPref : GmPref<int> {
    };

    /// <summary>
    /// GmPref that stores a string value
    /// </summary>

    public class StringPref : GmPref<string> {
        /// <summary>
        /// Strings can skip serialization
        /// </summary>
        protected override string Serialize()
        {
            return internalValue;
        }
        /// <summary>
        /// Strings can skip deserialization
        /// </summary>
        protected override object Deserialize(string newString)
        {
            return newString;
        }
    };

    /// <summary>
    /// GmPref that stores a vector2 value
    /// </summary>
    public class Vector2Pref : GmPref<Vector2>
    {
        /* Probably not needed if the object is already serializable
        protected override string Serialize()
        {
            return internalValue.x + "," + internalValue.y;
        }
        protected override Vector2 Deserialize(string newString)
        {
            var parts = newString.Split(new char[] { ',' });
            return new Vector2(float.Parse(parts[0]), float.Parse(parts[1]));
        }
        */
    }
}
