﻿using UnityEngine;


namespace Extensions
{
    public static partial class GameObjectExtensions
    {
        public static void AskLayer(this GameObject obj, int lvl)
        {
            obj.gameObject.layer = lvl;
            if (obj.transform.childCount <= 0) return;
            foreach (Transform d in obj.transform)
            {
                AskLayer(d.gameObject, lvl);
            }
        }

        // public static void RendererSetActive(Transform renderer)
        // {
        //     var component = renderer.gameObject.GetComponent<Renderer>();
        //     if (component)
        //         component.enabled = _isVisible;
        // }

        public static void AskColor(GameObject obj, Color color)
        {
            if (obj.TryGetComponent<Renderer>(out Renderer renderer))
            {
                foreach (var curMaterial in renderer.materials)
                {
                    curMaterial.color = color;
                }
            }
            
            if (obj.transform.childCount <= 0) return;
            foreach (Transform d in obj.transform)
            {
                AskColor(d.gameObject, color);
            }
        }
        
        public static void DisableRigidBody(this GameObject obj)
        {
            var rigidbodies = obj.GetComponentsInChildren<Rigidbody>();
            foreach (var rb in rigidbodies)
            {
                rb.isKinematic = true;
            }
        }
        
        public static void EnableRigidBody(this GameObject obj, float force)
        {
            EnableRigidBody(obj);
            obj.GetComponent<Rigidbody>().AddForce(obj.transform.forward * force);
        }
        
        public static void EnableRigidBody(this GameObject obj)
        {
            var rigidbodies = obj.GetComponentsInChildren<Rigidbody>();
            foreach (var rb in rigidbodies)
            {
                rb.isKinematic = false;
            }
        }
        
        public static void ConstraintsRigidBody(this GameObject self, RigidbodyConstraints rigidbodyConstraints)
        {
            var rigidbodies = self.GetComponentsInChildren<Rigidbody>();
            foreach (var rb in rigidbodies)
            {
                rb.constraints = rigidbodyConstraints;
            }
        }

        public static bool TryGetComponent<T>(this GameObject obj, out T component)
        {
            bool doesComponentExists = false;
            component = default(T);

            if (typeof(T) == typeof(Component))
            {
                T newComponent = obj.GetComponent<T>();

                if(newComponent != null)
                {
                    component = newComponent;
                    doesComponentExists = true;
                }
            }

            return doesComponentExists;
        }
    }
}