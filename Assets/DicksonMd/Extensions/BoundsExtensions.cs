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

        public static Vector3 RandomPointInside(this BoxCollider collider)
        {
            return new Vector3(
                Random.Range(-collider.size.x / 2 - collider.center.x, collider.size.x / 2 + collider.center.x),
                Random.Range(-collider.size.y / 2 - collider.center.y, collider.size.y / 2 + collider.center.y),
                Random.Range(-collider.size.z / 2 - collider.center.z, collider.size.z / 2 + collider.center.z)
            );
        }
    }
}