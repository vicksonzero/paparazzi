using UnityEngine;

namespace DicksonMd.Extensions
{
    public static class BoundsExtensions
    {
        public static float Area(this Bounds bounds)
        {
            // Debug.Log($"Area = {bounds.size.x * bounds.size.z}");
            return bounds.size.x * bounds.size.z;
        }

        public static float Volume(this Bounds bounds)
        {
            return bounds.size.x * bounds.size.y * bounds.size.z;
        }
        
        public static Vector3 RandomPointInside(this Bounds bounds)
        {
            return new Vector3(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y),
                Random.Range(bounds.min.z, bounds.max.z)
            );
        }
    }
}