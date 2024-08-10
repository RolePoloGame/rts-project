using UnityEngine;

namespace RTS.Core
{
    public static class StringExtensions
    {
        private const string BOLD_TAG = "b";
        private const string CURSIVE_TAG = "i";
        private const string COLOR_TAG = "color";
        private const string SIZE_TAG = "size";
        public static string Colored(this bool boolean) => boolean.ToString().Color(boolean ? UnityEngine.Color.green : UnityEngine.Color.red);
        public static string Bold(this string text) => text.AppendTag(BOLD_TAG);
        public static string Italic(this string text) => text.AppendTag(CURSIVE_TAG);
        public static string Color(this string text, Color color) => text.AppendTag(COLOR_TAG, ColorUtility.ToHtmlStringRGB(color));
        public static string Size(this string text, float size) => text.AppendTag(SIZE_TAG, size.ToString());
        private static string AppendTag(this string text, string tag, string value = "")
        {
            var tagValue = string.IsNullOrEmpty(value) ? string.Empty : $"={value}";
            return $"<{tag}{tagValue}>{text}</{tag}>";
        }
    }
}