using UnityEngine;

namespace RTS.Core
{
    public static class ColorExtensions
    {
        /// <summary>Blends the specified colors together.</summary>
        /// <param name="color">Color to blend onto the background color.</param>
        /// <param name="backColor">Color to blend the other color onto.</param>
        /// <param name="amount">How much of <paramref name="color"/> to keep,
        /// “on top of” <paramref name="backColor"/>.</param>
        /// <returns>The blended colors.</returns>
        public static Color Blend(this Color color, Color backColor, double amount = 0.5f)
        {
            float r = (float)(color.r * amount + backColor.r * (1 - amount));
            float g = (float)(color.g * amount + backColor.g * (1 - amount));
            float b = (float)(color.b * amount + backColor.b * (1 - amount));
            return new Color(r, g, b);
        }

        public static Color Average(this Gradient gradient, int width = 10)
        {
            Color col = Color.white;
            float step = 1 / (float)width;
            for (int i = 0; i < width; i++)
                col = col.Blend(gradient.Evaluate(i * step));
            return col;
        }

        public static Color ApplyIntensity(this Color color, float intensity)
        {
            float fraction = Mathf.Pow(2, intensity);
            color = new(fraction * color.r, fraction * color.g, fraction * color.b);
            return color;
        }
    }
}