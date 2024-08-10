using UnityEngine;

namespace RTS.Core
{
    public static class MathExtensions
    {
        public static float Sigmoid(float x, float easeStrength = 1)
        {
            return 1 / (1 + Mathf.Exp((x * -6 * easeStrength) + 2 * easeStrength));
        }

        public static float SineEaseInOut(float x)
        {
            return -(Mathf.Cos(Mathf.PI * x) - 1) / 2;
        }

        public static float EaseInOutBack(float x)
        {
            const float c1 = 1.70158f;
            const float c2 = c1 * 1.525f;

            return x < 0.5
              ? (Mathf.Pow(2 * x, 2) * ((c2 + 1) * 2 * x - c2)) / 2
              : (Mathf.Pow(2 * x - 2, 2) * ((c2 + 1) * (x * 2 - 2) + c2) + 2) / 2;
        }
    }
}