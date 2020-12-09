using System;
using UnityEngine;

namespace Extensions
{
    public static partial class MathExtensions
    {
        public static bool Between(this int target, int min, int max)
        {
            if (min > max)
            {
                int temp = min;
                min = max;
                max = temp;
            }
            return target >= min && target < max;
        }

        public static bool Between(this float target, float min, float max)
        {
            return target >= min && target < max;
        }
        public static bool Between(this TimeSpan target, TimeSpan min, TimeSpan max)
        {
            return target >= min && target < max;
        }

        public static bool Between(this DateTime target, DateTime min, DateTime max)
        {
            return target >= min && target < max;
        }

        public static bool BetweenStrictly(this int target, int min, int max)
        {
            if (min > max)
            {
                var temp = min;
                min = max;
                max = temp;
            }
            return target > min && target < max;
        }

        public static bool BetweenStrictly(this float target, float min, float max)
        {
            if (min > max)
            {
                var temp = min;
                min = max;
                max = temp;
            }
            return target > min && target < max;
        }
    }
}