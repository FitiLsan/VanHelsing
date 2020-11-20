using System.Collections.Generic;
using System.Linq;


namespace Extensions
{
    public static partial class ListExtensions
    {
        public static T ReturnRandom<T>(this List<T> list, T[] itemsToExclude)
        {
            var val = list[UnityEngine.Random.Range(0, list.Count)];

            while (itemsToExclude.Contains(val))
                val = list[UnityEngine.Random.Range(0, list.Count)];

            return val;
        }

        public static T ReturnRandom<T>(this List<T> list)
        {
            var val = list[UnityEngine.Random.Range(0, list.Count)];
            return val;
        }
    }
}
