using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;


namespace Extensions
{
    public static partial class ArrayExtensions
    {
        public static T RandomObject<T>(this IList<T> list) 
	    {
		    return (list.Count > 0) ? list[Random.Range(0, list.Count)] : default(T);
        }

        public static List<T> RandomObjects<T>(this IEnumerable<T> list, int elementsCount)
        {
            elementsCount = Mathf.Clamp(elementsCount, 1, list.Count());

            return list.OrderBy(arg => Guid.NewGuid()).Take(elementsCount).ToList();
        }

        public static T LastObject<T>(this IList<T> list) 
	    {
		    return (list.Count > 0) ? list[list.Count - 1] : default(T);
	    }

        public static T FirstObject<T>(this IList<T> list) 
	    {
		    return (list.Count > 0) ? list[0] : default(T);
	    }

        public static T[] RemoveAt<T>(this T[] source, int index)
        {
            T[] result = source;

            if (index >= 0 && index < source.Length)
            {
                result = new T[source.Length - 1];

                if (index > 0)
                {
                    Array.Copy(source, 0, result, 0, index);
                }

                if (index < source.Length - 1)
                {
                    Array.Copy(source, index + 1, result, index, source.Length - index - 1);
                }
            }

            return result;
        }

        public static List<T> Diff<T>(IList<T> array1, IList<T> array2)
	    {
		    var subset = new List<T>();

		    foreach (T e1 in array1)
		    {
			    if (!array2.Contains(e1))
			    {
				    subset.Add(e1);
			    }
		    }

		    foreach (T e2 in array2)
		    {
			    if (!array1.Contains(e2))
			    {
				    subset.Add(e2);
			    }
		    }

		    return subset;
	    }

        public static bool ArrayEqual<T>(this IList<T> array1, IList<T> array2) where T : Object
        {
            bool result;
            
            if (array2 == null || array1 == null)
            {
                result = false;
            }
            else
            {
                result = (array1.Count == array2.Count);

                if (result)
                {
                    for (int i = 0; i < array1.Count && result; i++)
                    {
                        if (array1[i])
                        {
                            result &= (array1[i] == (array2[i]));
                        }
                    }
                }
            }

            return result;
        }

        public static T[] EnsureLength<T>(this T[] array, int desiredLength, bool ensureAtLeast = false)
	    {
		    if ((array == null) ||
		        (((ensureAtLeast) ? (array.Length < desiredLength) : (array.Length != desiredLength))))
		    {
			    return new T[desiredLength];
		    }
		    else
		    {
			    return array;
		    }
        }

        public static bool ArrayContains<T>(this T[] array, T value) where T : Object
        {
            bool result = (array != null) && (value != null);
            if (result)
            {
                result = false;
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i] == value)
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }

        public static L InitWith<L, T>(this L list, T with, int count) where L : IList<T>
        {
            list.Clear();

            for (int i = 0; i < count; i++)
            {
                list.Add(with);
            }

            return list;
        }
           
        public static L InitWith<L, T>(this L list, Func<T> with, int count) where L : IList<T>
        {
            list.Clear();

            for (int i = 0; i < count; i++)
            {
                list.Add(with());
            }

            return list;
        }

        public static L InitWith<L, T>(this L list, Func<int, T> with, int count) where L : IList<T>
        {
            list.Clear();

            for (int i = 0; i < count; i++)
            {
                list.Add(with(i));
            }

            return list;
        }

        public static T[] FillWith<T>(this T[] array, T with)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = with;
            }

            return array;
        }

        public static T[] FillWith<T>(this T[] array, Func<T> with)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = with();
            }

            return array;
        }

        public static T[] FillWith<T>(this T[] array, Func<int, T> with)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = with(i);
            }

            return array;
        }

        public static T[,] FillWith<T>(this T[,] array, T with)
        {
            int w = array.GetLength(0);
            int h = array.GetLength(1);
            for (int j = 0; j < h; j++)
            {
                for (int i = 0; i < w; i++)
                {
                    array[i, j] = with;
                }
            }

            return array;
        }

        public static T[,] FillWith<T>(this T[,] array, Func<T> with)
        {
            int w = array.GetLength(0);
            int h = array.GetLength(1);
            for (int j = 0; j < h; j++)
            {
                for (int i = 0; i < w; i++)
                {
                    array[i, j] = with();
                }
            }

            return array;
        }

        public static T[,] FillWith<T>(this T[,] array, Func<int, int, T> with)
        {
            int w = array.GetLength(0);
            int h = array.GetLength(1);
            for (int j = 0; j < h; j++)
            {
                for (int i = 0; i < w; i++)
                {
                    array[i, j] = with(i, j);
                }
            }

            return array;
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Range(0, n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        /// <summary>
        /// Add element if it isn't added yet
        /// </summary>
        /// <returns>Indicates added element or not</returns>
        public static bool AddExclusive<T>(this IList<T> list, T element)
        {
            bool result = false;
            
            if (!list.Contains(element))
            {
                result = true;
                list.Add(element);
            }
            
            return result;
        }

        /// <summary>
        /// Good for tiny Lists instead of Distinct()
        /// </summary>
        public static void AddRangeExclusive<T>(this IList<T> list, IList<T> elements)
        {
            for (int i = 0; i < elements.Count; i++)
            {
                list.AddExclusive(elements[i]);
            }
        }

        /// <summary>
        /// Add element if it isn't added yet
        /// </summary>
        /// <returns>Indicates added element or not</returns>
        public static bool InsertExclusive<T>(this IList<T> list, int index, T element)
        {
            bool result = false;
            
            if (!list.Contains(element))
            {
                result = true;
                
                index = index.Clamp(0, list.Count);
                if (index == list.Count)
                {
                    list.Add(element);
                }
                else
                {
                    list.Insert(index, element);
                }
            }
            
            return result;
        }

        /// <summary>
        /// Return 'true' in func to break the cycle
        /// </summary>
        public static void ForEach<T>(this IEnumerable<T> list, Func<T, bool> breakFunc)
        {
            if (breakFunc == null)
            {
                return;
            }
            foreach (T element in list)
            {
                if (breakFunc(element))
                {
                    break;
                }
            }
        }        
        
        public static bool IsOneOf<T>(this T self, params T[] elem)
        {
            return elem.Contains(self);
        }  
		
        public static T[] Concat<T>(this T[] x, T[] y)
        {
            if (x == null) throw new ArgumentNullException("x");
            if (y == null) throw new ArgumentNullException("y");
            var oldLen = x.Length;
            Array.Resize<T>(ref x, x.Length + y.Length);
            Array.Copy(y, 0, x, oldLen, y.Length);
            return x;
        }
        
        public static int ReturnNearestIndex(this Vector3[] nodes, Vector3 destination)
        {            
            var nearestDistance = Mathf.Infinity;
            var index = 0;
            var length = nodes.Length;
            for (var i = 0; i < length; i++)
            {
                var distanceToNode = (destination + nodes[i]).sqrMagnitude;
                if (!(nearestDistance > distanceToNode)) continue;
                nearestDistance = distanceToNode;
                index = i;
            }

            return index;
        }
    }
}
