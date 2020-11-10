using UnityEngine;


namespace Extensions
{
    public static partial class ComponentExtensions
    {
        public static T GetOrAddComponent<T>(this Component child) where T : Component
        {
            T result = child.GetComponent<T>() ?? child.gameObject.AddComponent<T>();
            return result;
        }
        
    }
}
