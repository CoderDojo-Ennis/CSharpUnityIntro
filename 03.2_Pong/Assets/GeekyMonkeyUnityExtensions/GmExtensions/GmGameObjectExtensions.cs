using System.Collections.Generic;
using UnityEngine;

namespace GeekyMonkey
{
    public static class GmGameObjectExtensions
    {
        /// <summary>
        /// Delete all children of this game object
        /// </summary>
        /// <param name="go">Parent game object</param>
        public static void DeleteAllChildren(this GameObject go)
        {
            foreach (Transform t in go.transform)
            {
                GameObject.Destroy(t.gameObject);
            }
        }

        /// <summary>
        /// Get child objects with tag - recursive - excludes self
        /// </summary>
        /// <param name="go">GameObject starting point</param>
        /// <param name="tag">Tag to find</param>
        /// <param name="includeInactive">Includ inactive components?</param>
        /// <param name="includeSelf">Includ this parent game object?</param>
        /// <returns>Array of matching gameobjects</returns>
        public static GameObject[] GetChildrenWithTag(this GameObject go, string tag, bool includeInactive, bool includeSelf = false)
        {
            int parentInstanceId = go.GetInstanceID();
            var childTransforms = go.transform.GetComponentsInChildren<Transform>(includeInactive);
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

        /// <summary>
        /// Get components in children with tag - recursive - excludes self
        /// </summary>
        /// <typeparam name="T">Component type</typeparam>
        /// <param name="gameObject">Parent game object</param>
        /// <param name="tag">Tag to find</param>
        /// <param name="includeInactive">Includ inactive game objects?</param>
        /// <param name="includeSelf">Includ this parent game object?</param>
        /// <returns>List of component references</returns>
        public static T[] GetComponentsInChildrenWithTag<T>(this GameObject gameObject, string tag, bool includeInactive, bool includeSelf = false)
            where T : Component
        {
            var childrenWithTag = gameObject.GetChildrenWithTag(tag, includeInactive, includeSelf);
            var results = new List<T>(childrenWithTag.Length);
            foreach (GameObject child in childrenWithTag)
            {
                T component = child.GetComponent<T>();
                if (component != null)
                {
                    results.Add(component);
                }
            }

            return results.ToArray();
        }

        /// <summary>
        /// Get Component in Parents
        /// </summary>
        /// <typeparam name="T">Type of component</typeparam>
        /// <param name="gameObject">Parent object</param>
        /// <param name="includeInactive">Includ inactive game objects?</param>
        /// <param name="includeSelf">Includ this parent game object?</param>
        /// <returns>Component or null if not found</returns>
        public static T GetComponentInParents<T>(this GameObject gameObject, bool includeInactive, bool includeSelf = false)
            where T : Component
        {
            Transform startTransform = includeSelf ? gameObject.transform : gameObject.transform.parent;
            for (Transform t = startTransform; t != null; t = t.parent)
            {
                if (includeInactive || t.gameObject.activeSelf)
                {
                    T result = t.GetComponent<T>();
                    if (result != null)
                    {
                        return result;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Get components in parents
        /// </summary>
        /// <typeparam name="T">Type of component to find</typeparam>
        /// <param name="gameObject">Starting game object</param>
        /// <param name="includeInactive">Include inactive game objects?</param>
        /// <param name="includeSelf">Includ this parent game object?</param>
        /// <returns>List of components which could be empty</returns>
        public static T[] GetComponentsInParents<T>(this GameObject gameObject, bool includeInactive, bool includeSelf = false)
            where T : Component
        {
            var results = new List<T>();
            Transform startTransform = includeSelf ? gameObject.transform : gameObject.transform.parent;
            for (Transform t = startTransform; t != null; t = t.parent)
            {
                if (includeInactive || t.gameObject.activeSelf)
                {
                    T result = t.GetComponent<T>();
                    if (result != null)
                    {
                        results.Add(result);
                    }
                }
            }

            return results.ToArray();
        }

        /// <summary>
        /// The set of layers that GameObject can collide against, based on the game object's layer
        /// </summary>
        /// <param name="gameObject">The game object</param>
        /// <returns>Bitmask of the 32 collision layers</returns>
        public static uint GetCollisionMask(this GameObject gameObject)
        {
            int layer = gameObject.layer;
            return GmPhysicsHelper.GetCollisionMask(layer);
        }

        /// <summary>
        /// Is this object a descendent of some other object
        /// </summary>
        /// <param name="childObject">This object</param>
        /// <param name="ancestorObjectToFind">Parent object to find</param>
        /// <returns>True of the given parent is an ancesctor of this object</returns>
        public static bool IsDescendentOf(this GameObject childObject, GameObject ancestorObjectToFind)
        {
            if (ancestorObjectToFind == null)
            {
                return true;
            }
            return childObject.IsDescendentOf(ancestorObjectToFind.transform);
        }

        /// <summary>
        /// Is this object a descendent of some other object
        /// </summary>
        /// <param name="childObject">This object</param>
        /// <param name="ancestorObjectToFind">Parent object to find</param>
        /// <returns>True of the given parent is an ancesctor of this object</returns>
        public static bool IsDescendentOf(this GameObject childObject, Transform ancestorObjectToFind)
        {
            if (ancestorObjectToFind == null)
            {
                return true;
            }
            Transform parent = childObject.transform.parent;
            while (parent != null)
            {
                if (parent == ancestorObjectToFind)
                {
                    return true;
                }
                parent = parent.parent;
            }
            return false;
        }

        /// <summary>
        /// Checks if a GameObject has been destroyed.
        /// </summary>
        /// <param name="gameObject">GameObject reference to check for destructedness</param>
        /// <returns>If the game object has been marked as destroyed by UnityEngine</returns>
        public static bool IsDestroyed(this GameObject gameObject)
        {
            // UnityEngine overloads the == opeator for the GameObject type
            // and returns null when the object has been destroyed, but 
            // actually the object is still there but has not been cleaned up yet
            // if we test both we can determine if the object has been destroyed.
#pragma warning disable IDE0041 // Use 'is null' check
            return gameObject == null && !ReferenceEquals(gameObject, null);
#pragma warning restore IDE0041 // Use 'is null' check
        }
    }
}
