using System.Collections.Generic;
using UnityEngine;

namespace GeekyMonkey
{
    public static class GmMonoBehaviourExtensions
    {
        /// <summary>
        /// Find child object by name - Recursive
        /// </summary>
        /// <param name="mb">Starting behavour</param>
        /// <param name="childName">Child object name</param>
        /// <param name="includeSelf">Includ this parent game object?</param>
        /// <returns>Game object or null if not found</returns>
        static public GameObject FindChildByName(this MonoBehaviour mb, string childName, bool includeSelf = false)
        {
            Transform[] ts = mb.transform.GetComponentsInChildren<Transform>(includeInactive: true);
            int parentInstanceId = mb.gameObject.GetInstanceID();
            foreach (Transform t in ts)
            {
                // Exclude parent
                if (includeSelf || t.gameObject.GetInstanceID() != parentInstanceId)
                {
                    if (t.gameObject.name == childName)
                    {
                        return t.gameObject;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Get child objects with tag - recursive
        /// </summary>
        /// <param name="mb">Behaviour starting point</param>
        /// <param name="tag">Tag to find</param>
        /// <param name="includeInactive">Includ inactive components?</param>
        /// <param name="includeSelf">Includ this parent game object?</param>
        /// <returns>Array of matching gameobjects</returns>
        public static GameObject[] GetChildrenWithTag(this MonoBehaviour mb, string tag, bool includeInactive, bool includeSelf = false)
        {
            int parentInstanceId = mb.gameObject.GetInstanceID();
            var childTransforms = mb.GetComponentsInChildren<Transform>(includeInactive);
            List<GameObject> taggedChildren = new List<GameObject>(childTransforms.Length);
            for (var i = 0; i < childTransforms.Length; i++)
            {
                // Exclude self
                if (includeSelf || parentInstanceId != childTransforms[i].gameObject.GetInstanceID())
                {
                    if (childTransforms[i].CompareTag(tag))
                    {
                        taggedChildren.Add(childTransforms[i].gameObject);
                    }
                }
            }

            return taggedChildren.ToArray();
        }
    }
}
