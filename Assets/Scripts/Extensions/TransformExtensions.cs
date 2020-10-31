using System.Collections.Generic;
using UnityEngine;


namespace Extensions
{
    public static partial class TransformExtensions
    {
        public static Transform FindDeep(this Transform obj, string id)
        {
            if (obj.name == id)
            {
                return obj;
            }

            var count = obj.childCount;
            for (var i = 0; i < count; ++i)
            {
                var posObj = obj.GetChild(i).FindDeep(id);
                if (posObj != null)
                {
                    return posObj;
                }
            }

            return null;
        }

        public static List<T> GetAll<T>(this Transform obj)
        {
            var results = new List<T>();
            obj.GetComponentsInChildren(results);
            return results;
        }
    }
}
