using UnityEngine;

namespace ExtensionMethods
{
    public static class Extension
    {
        public static Vector2 AsVector2(this Vector3 _v)
        {
            return new Vector2(_v.x, _v.y);
        }

        public static void AddExplosionForce(this Rigidbody2D rb, float explosionForce, Vector2 explosionPosition, float explosionRadius, float upwardsModifier = 0.0F, ForceMode2D mode = ForceMode2D.Force)
        {
            var explosionDir = rb.position - explosionPosition;
            var explosionDistance = explosionDir.magnitude;

            // Normalize without computing magnitude again
            if (upwardsModifier == 0)
                explosionDir /= explosionDistance;
            else
            {
                // From Rigidbody.AddExplosionForce doc:
                // If you pass a non-zero value for the upwardsModifier parameter, the direction
                // will be modified by subtracting that value from the Y component of the centre point.
                explosionDir.y += upwardsModifier;
                explosionDir.Normalize();
            }

            rb.AddForce(Mathf.Lerp(0, explosionForce, (1 - explosionDistance)) * explosionDir, mode);
        }
        public static float SignedAngleTo(this Vector2 a, Vector2 b)
        {
            return Mathf.Atan2(a.x * b.y - a.y * b.x, a.x * b.x + a.y * b.y) * Mathf.Rad2Deg;
        }

        /// <summary>
        /// Returns the signed angle between this vector and the +X axis.
        /// </summary>
        /// <returns>The signed angle, reprenting the direction of this in degrees.</returns>
        /// <param name="a">Vector this was called on.</param>
        public static float SignedAngle(this Vector2 a)
        {
            return Mathf.Atan2(a.y, a.x) * Mathf.Rad2Deg;
        }

        /// <summary>
        /// Returns the signed angle between this 3D vector and another,
        /// with respect to some orthogonal "up" vector.  If looking in
        /// the "up" direction, then + angles are counter-clockwise.
        /// </summary>
        /// <returns>The signed angle, in degrees, from A to B.</returns>
        /// <param name="a">Vector this was called on.</param>
        /// <param name="b">Vector to measure the angle to.</param>
        public static float SignedAngleTo(this Vector3 a, Vector3 b, Vector3 up)
        {
            return Mathf.Atan2(
                Vector3.Dot(up.normalized, Vector3.Cross(a, b)),
                Vector3.Dot(a, b)) * Mathf.Rad2Deg;
        }
    }
}