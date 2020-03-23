using System;

namespace ML.Lib
{
    public static class ComparableExtensions
    {
        public static T Bound<T>(this T t, T min, T max) where T : IComparable<T>
        {
            if (t.CompareTo(max) > 0)
                return max;

            if (t.CompareTo(min) < 0)
                return min;

            return t;
        }

    }



}