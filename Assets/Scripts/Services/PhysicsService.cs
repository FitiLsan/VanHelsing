using UnityEngine;
using System.Collections.Generic;


namespace BeastHunter
{
    public sealed class PhysicsService : Service
    {
        #region Fields

        private const int COLLIDER_OBJECT_SIZE = 20;
        private const float GROUND_CHECK_TOP_VALUE = 10000f;

        private readonly Collider[] _collidedObjects;
        private readonly RaycastHit[] _castBuffer;
        private readonly List<ITrigger> _triggeredObjects;

        #endregion


        #region ClassLifeCycles

        public PhysicsService(Contexts contexts) : base(contexts)
        {
            _collidedObjects = new Collider[COLLIDER_OBJECT_SIZE];
            _castBuffer = new RaycastHit[64];
            _triggeredObjects = new List<ITrigger>();
        }

        #endregion


        #region Methods

        public void DrawLine(Vector3 position, Vector3 direction, float distance)
        {
            Debug.DrawLine(position, position + direction * distance, Color.red);
            Debug.LogError("The line is done");
        }

        public bool MakeRaycast(Vector3 position, Vector3 direction, out RaycastHit rayHit, float distance)
        {
            return Physics.Raycast(position, direction, out rayHit, distance);  
        }

        public bool CheckGround(Vector3 position, float distanceRay, out Vector3 hitPoint)
        {
            hitPoint = Vector3.zero;

            bool isHit = Physics.Raycast(position, Vector3.down, out RaycastHit hit, distanceRay);

            if (isHit)
            {
                hitPoint = hit.point;
            }

            return isHit;
        }

        public bool FindGround(Vector3 position, out Vector3 hitPoint)
        {
            hitPoint = Vector3.zero;

            bool isHit = Physics.Raycast(position, Vector3.down, out RaycastHit hit);

            if (isHit)
            {
                hitPoint = hit.point;
            }

            return isHit;
        }
        
        public static Vector3 GetGroundedPositionStatic(Vector3 position)
        {
            Vector3 groundedPosition = position;
                
            bool isHit = Physics.Raycast(new Vector3(position.x, GROUND_CHECK_TOP_VALUE, position.z), 
                Vector3.down, out RaycastHit hit);
        
            if (isHit)
            {
                groundedPosition = hit.point;
            }
        
            return groundedPosition;
        }
        
        public Vector3 GetGroundedPosition(Vector3 position)
        {
            return GetGroundedPositionStatic(position);
        }

        public List<ITrigger> GetObjectsInRadius(Vector3 position, float radius, int layerMask = LayerManager.DEFAULT_LAYER)
        {
            _triggeredObjects.Clear();
            ITrigger trigger;

            int collidersCount = Physics.OverlapSphereNonAlloc(position, radius, _collidedObjects, layerMask);

            for (int i = 0; i < collidersCount; i++)
            {
                trigger = _collidedObjects[i].GetComponent<ITrigger>();

                if (trigger != null && !_triggeredObjects.Contains(trigger))
                {
                    _triggeredObjects.Add(trigger);
                }
            }

            return _triggeredObjects;
        }

        public HashSet<ITrigger> SphereCastObject(Vector2 center, float radius, HashSet<ITrigger> outBuffer,
            int layerMask = LayerManager.DEFAULT_LAYER)
        {
            outBuffer.Clear();

            int hitCount = Physics.SphereCastNonAlloc(center,
                radius,
                Vector2.zero,
                _castBuffer,
                0.0f,
                layerMask);

            for (int i = 0; i < hitCount; i++)
            {
                ITrigger carTriggerProvider = _castBuffer[i].collider.GetComponent<ITrigger>();
                if (carTriggerProvider != null)
                {
                    outBuffer.Add(carTriggerProvider);
                }
            }

            return outBuffer;
        }

        public ITrigger GetNearestObject(Vector3 targetPosition, HashSet<ITrigger> objectBuffer)
        {
            float nearestDistance = Mathf.Infinity;
            ITrigger result = null;

            foreach (ITrigger trigger in objectBuffer)
            {
                float objectDistance = (targetPosition - trigger.GameObject.transform.position).sqrMagnitude;
                if (objectDistance >= nearestDistance)
                {
                    continue;
                }

                nearestDistance = objectDistance;
                result = trigger;
            }

            return result;
        }

        public List<GameObject> GetObjectsInRadiusByTag(Vector3 position, float radius, string tagName)
        {
            Collider[] collidedObjectsByTag = new Collider[200];
            var layer = LayerManager.DefaultLayer;
            int colliderCount = Physics.OverlapSphereNonAlloc(position, radius, collidedObjectsByTag, layer);
            List <GameObject> colliderListByTag = new List<GameObject>(); 
            for (int i = 0; i < colliderCount; i++)
            {
                var obj = collidedObjectsByTag[i].gameObject;

                if (obj != null && obj.tag == tagName)
                {
                    colliderListByTag.Add(obj);
                }
            }
            return colliderListByTag;
        }

        #endregion
    }
}