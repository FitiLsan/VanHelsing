using Extensions;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    public sealed class FallingTreeModel : SimpleInteractiveObjectModel
    {
        #region Fields

        private FallingTreeData _data;
        private InteractableObjectBehavior[] _interactableObjects;

        #endregion


        #region Properties

        public Collider InteractiveTrigger { get; }
        public Rigidbody Rigidbody { get; private set; }
        public InteractableObjectBehavior DealDamageBehaviour { get; }
        public Collider DealDamageCollider { get; }
        public float DeactivateTimer { get; set; }
        /// <summary>Entities inside deal damage collider (Key: entity ID, value: entity last IOBehavior)</summary>
        public Dictionary<int, InteractableObjectBehavior> StayCollisionEntities { get; private set; }

        #endregion


        #region ClassLifeCycle

        public FallingTreeModel(GameObject prefab, FallingTreeData data) : base(prefab, data)
        {
            _data = data;

            Services.SharedInstance.PhysicsService.FindGround(Prefab.transform.position, out Vector3 groundPosition);
            Prefab.transform.position = new Vector3(groundPosition.x, groundPosition.y+_data.PrefabOffsetY, groundPosition.z);

            Transform viewTransform = Prefab.transform.Find(data.ViewChildName);
            viewTransform.localScale = data.TreeSize;
            viewTransform.localRotation = Quaternion.Euler(new Vector3(0, -90.0f, data.TreeTilt));

            Rigidbody = prefab.GetComponentInChildren<Rigidbody>();
            Rigidbody.mass = data.Mass;
            Rigidbody.drag = data.Drag;
            Rigidbody.angularDrag = data.AngularDrag;

            _interactableObjects = prefab.GetComponentsInChildren<InteractableObjectBehavior>();
            InteractiveTrigger = _interactableObjects.GetInteractableObjectByType(InteractableObjectType.ActiveObject).GetComponent<Collider>();
            DealDamageBehaviour = _interactableObjects.GetInteractableObjectByType(InteractableObjectType.HitBox);
            DealDamageCollider = DealDamageBehaviour.GetComponent<Collider>();

            StayCollisionEntities = new Dictionary<int, InteractableObjectBehavior>();
        }

        #endregion


        #region SimpleInteractiveObjectModel

        public override void Updating()
        {
            if (IsActivated)
            {
                _data.Act(this);
            }
            else
            {
                base.Updating();
            }
        }

        #endregion


        #region Methods

        public void Clean()
        {
            _data = null;

            if (InteractiveTrigger != null)
            {
                Object.Destroy(InteractiveTrigger);
            }

            if (CanvasObject != null)
            {
                Object.Destroy(CanvasObject.gameObject);
            }

            for (int i = 0; i< _interactableObjects.Length; i++)
            {
                if (_interactableObjects[i] != null)
                {
                    Object.Destroy(_interactableObjects[i]);
                }
            }
            _interactableObjects = null;

            if (Rigidbody != null)
            {
                Object.Destroy(Rigidbody);
            }

            if (DealDamageCollider != null)
            {
                Object.Destroy(DealDamageCollider.gameObject);
            }

            StayCollisionEntities.Clear();
            StayCollisionEntities = null;
        }

        #endregion
    }
}

