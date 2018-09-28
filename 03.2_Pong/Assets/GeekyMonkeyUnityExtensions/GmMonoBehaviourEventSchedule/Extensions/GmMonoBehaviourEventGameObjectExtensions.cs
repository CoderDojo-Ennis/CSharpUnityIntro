using UnityEngine;

namespace GeekyMonkey
{
    public static class GmMonoBehaviourEventGameObjectExtensions
    {
        /// <summary>
        /// Glide the position over time
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="mb"></param>
        /// <param name="startPos"></param>
        /// <param name="targetPos"></param>
        /// <param name="seconds"></param>
        /// <param name="realtime"></param>
        /// <returns></returns>
        public static GmMonoBehaviourEventPromise GlidePosition(this Transform transform, MonoBehaviour mb, Vector3 startPos, Vector3 targetPos, float seconds, bool realtime)
        {
            int steps = 40;
            float step = 0;

            transform.position = startPos;

            return mb.Repeat(seconds / steps, steps, () =>
            {
                step++;
                transform.position = Vector3.Lerp(startPos, targetPos, step / steps);
            }, realtime);
        }

        /// <summary>
        /// Glide the local position over time
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="mb"></param>
        /// <param name="startPos"></param>
        /// <param name="targetPos"></param>
        /// <param name="seconds"></param>
        /// <param name="realtime"></param>
        /// <returns></returns>
        public static GmMonoBehaviourEventPromise GlideLocalPosition(this Transform transform, MonoBehaviour mb, Vector3 startPos, Vector3 targetPos, float seconds, bool realtime)
        {
            int steps = 40;
            float step = 0;

            transform.localPosition = startPos;

            return mb.Repeat(seconds / steps, steps, () =>
            {
                step++;
                transform.localPosition = Vector3.Lerp(startPos, targetPos, step / steps);
            }, realtime);
        }
    }
}
