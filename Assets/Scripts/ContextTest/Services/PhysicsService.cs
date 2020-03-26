using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    public class PhysicsService : Service
    {
        #region Fields

        private const int COLLIDER_OBJECT_SIZE = 20;

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

        public List<ITrigger> GetObjectsInRadius(Vector2 position, float radius, int layerMask = LayerManager.DEFAULT_LAYER)
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

        #endregion
    }
}