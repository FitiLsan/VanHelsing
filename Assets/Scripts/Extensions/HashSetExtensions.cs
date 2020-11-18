using System.Collections.Generic;


namespace Extensions
{
    public static partial class HashSetExtensions
    {
        public static bool IsNullOrEmpty<T>(this HashSet<T> set)
        {
            return set == null || set.Count == 0;
        }

        public static void AddRange<T>(this HashSet<T> set, IEnumerable<T> range)
        {
            foreach (T item in range)
            {
                set.Add(item);
            }
        }

        public static void AddRangeIfNotContains<T>(this HashSet<T> set, IEnumerable<T> range)
        {
            foreach (T item in range)
            {
                set.AddIfNotContains(item);
            }
        }

        public static void AddIfNotContains<T>(this HashSet<T> set, T item)
        {
            if (!set.Contains(item))
            {
                set.Add(item);
            }
        }

        public static T ItemAtIndexLinear<T>(this HashSet<T> set, int index)
        {
            if (set.IsNullOrEmpty() || index >= set.Count)
            {
                return default(T);
            }

            T result = default(T);
            int seeker = 0;
            set.ForEach(item =>
            {
                if (seeker == index)
                {
                    result = item;
                    return true;
                }

                seeker++;
                return false;
            });

            return result;
        }
    }
}
