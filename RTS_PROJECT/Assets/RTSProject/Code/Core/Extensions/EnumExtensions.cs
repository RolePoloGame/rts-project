using System;

namespace RTS.Core
{
    public static class EnumExtensions
    {
        public static int GetEnumLength<T>() where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an enumerated type");

            return Enum.GetNames(typeof(T)).Length;
        }

        public static T GetRandomEnum<T>(int seed) where T : struct, IConvertible
        {
            Random random = new(seed);
            return (T)(object)random.Next(GetEnumLength<T>());
        }

        public static T Next<T>(this T value) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an enumerated type");
            var length = GetEnumLength<T>();
            int current = (int)(object)value;
            current++;
            if (current >= length)
                current = 0;
            return (T)(object)current;
        }

        public static T Previous<T>(this T value) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an enumerated type");
            var length = GetEnumLength<T>();
            int current = (int)(object)value;
            current--;
            if (current < 0)
                current = length - 1;
            return (T)(object)current;
        }
    }
}