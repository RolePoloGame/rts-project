using UnityEngine;

namespace RTS.Core
{
    public static class VectorExtensions
    {
        public static Vector3 WithX(this Vector3 vector, float value) => new(value, vector.y, vector.z);
        public static Vector3 WithY(this Vector3 vector, float value) => new(vector.x, value, vector.z);
        public static Vector3 WithZ(this Vector3 vector, float value) => new(vector.x, vector.y, value);
        public static Vector2 WithX(this Vector2 vector, float value) => new(value, vector.y);
        public static Vector2 WithY(this Vector2 vector, float value) => new(vector.x, value);
        public static Vector3 ToVector3XZ(this Vector2 vector, float value = 0.0f) => new(vector.x, value, vector.y);
    }
}
