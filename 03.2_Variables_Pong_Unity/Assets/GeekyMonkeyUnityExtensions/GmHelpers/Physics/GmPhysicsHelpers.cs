using UnityEngine;
using System;

namespace GeekyMonkey
{
    public static class GmPhysicsHelper
    {
        /// <summary>
        /// The bitmask set of layers that given layer can collide against.
        /// </summary>
        /// <param name="layer">The layer to get the collision mask for</param>
        /// <returns>Bitmask of the 32 collision layers</returns>
        public static uint GetCollisionMask(int layer)
        {
            if (layer < 0 || layer > 31)
            {
                throw new ArgumentException($"GetCollisionMask layer must be between 0-31. {layer} is not valid.", nameof(layer));
            }

            uint mask = 0;
            for (int i = 0; i < 32; i++)
            {
                mask |= (((uint)(Physics.GetIgnoreLayerCollision(layer, i) ? 0 : 1)) << i);
            }
            return mask;
        }
    }
}
