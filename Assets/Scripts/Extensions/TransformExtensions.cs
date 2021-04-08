using System.Collections.Generic;
using UnityEngine;


namespace Extensions
{
    public static partial class TransformExtensions
    {
        public static Transform GetMainParent(this Transform obj)
        {
            while (obj.parent != null)
            {
                obj = obj.parent;
            }

            return obj;
        }

        public static Transform FindDeep(this Transform obj, string name)
        {
            if (obj.name == name)
            {
                return obj;
            }

            var count = obj.childCount;
            for (var i = 0; i < count; ++i)
            {
                var posObj = obj.GetChild(i).FindDeep(name);
                if (posObj != null)
                {
                    return posObj;
                }
            }

            return null;
        }

        public static bool TryFind(this Transform obj, string name, out Transform foundObj)
        {
            bool isObjectFounded = false;
            foundObj = default;

            Transform foundedObject = obj.Find(name);

            if (foundedObject != null)
            {
                foundObj = foundedObject;
                isObjectFounded = true;
            }
            else
            {
                throw new System.NullReferenceException("Can't find transform of object: " + 
                    obj.name + " with name: " + name);
            }

            return isObjectFounded;
        }

        public static bool TryFindDeep(this Transform obj, string name, out Transform foundObj)
        {
            bool isObjectFounded = false;
            foundObj = default;

            Transform foundedObject = obj.FindDeep(name);

            if (foundedObject != null)
            {
                foundObj = foundedObject;
                isObjectFounded = true;
            }
            else
            {
                throw new System.NullReferenceException("Can't find transform of object: " +
                    obj.name + " with name: " + name);
            }

            return isObjectFounded;
        }

        public static List<T> GetAll<T>(this Transform obj)
        {
            var results = new List<T>();
            obj.GetComponentsInChildren(results);
            return results;
        }
    }
}
