using Extensions;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    public sealed class BouldersModel : SimpleInteractiveObjectModel
    {
        #region Fields

        private BouldersData _data;
        private InteractableObjectBehavior[] _interactableObjects;

        #endregion


        #region Properties

        public Collider InteractiveTrigger { get; }
        public Rigidbody[] Rigidbodies { get; private set; }
        public List<InteractableObjectBehavior> BoulderBehaviours { get; }
        public float Timer { get; set; }
        /// <summary>contains boolean values of boulders that contains information about whether the boulder has been added to the destroy queue</summary>
        public Dictionary<int, bool> DestroyInfoDictionary { get; private set; }

        #endregion


        #region ClassLifeCycle

        public BouldersModel(GameObject prefab, BouldersData data) : base(prefab, data)
        {
            _data = data;
            DestroyInfoDictionary = new Dictionary<int, bool>();

            Services.SharedInstance.PhysicsService.FindGround(Prefab.transform.position, out Vector3 groundPosition);
            Prefab.transform.position = new Vector3(groundPosition.x, groundPosition.y+_data.PrefabOffsetY, groundPosition.z);

            Rigidbodies = prefab.GetComponentsInChildren<Rigidbody>();
            for (int i = 0; i < Rigidbodies.Length; i++)
            {
                Rigidbodies[i].mass = data.Mass;
                Rigidbodies[i].drag = data.Drag;
                Rigidbodies[i].angularDrag = data.AngularDrag;
                Rigidbodies[i].mass = data.Mass;
            }

            _interactableObjects = prefab.GetComponentsInChildren<InteractableObjectBehavior>();
            InteractiveTrigger = _interactableObjects.GetInteractableObjectByType(InteractableObjectType.ActiveObject).GetComponent<CapsuleCollider>();

            BoulderBehaviours = _interactableObjects.GetInteractableObjectsByType(InteractableObjectType.HitBox);
            PhysicMaterial physicMaterial = new PhysicMaterial() { bounciness = data.Bounciness, dynamicFriction = 0, staticFriction = 0 };
            for (int i = 0; i< BoulderBehaviours.Count; i++)
            {
                BoulderBehaviours[i].GetComponent<SphereCollider>().material = physicMaterial;
                DestroyInfoDictionary.Add(BoulderBehaviours[i].GetInstanceID(), false);
            }
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
            DestroyInfoDictionary = null;

            if (CanvasObject != null)
            {
                Object.Destroy(CanvasObject);
            }

            if (InteractiveTrigger != null)
            {
                Object.Destroy(InteractiveTrigger);
            }

            for (int i = 0; i< _interactableObjects.Length; i++)
            {
                if (_interactableObjects[i] != null)
                {
                    Object.Destroy(_interactableObjects[i]);
                }
            }
            _interactableObjects = null;

            for (int i = 0; i < Rigidbodies.Length; i++)
            {
                if (Rigidbodies[i] != null)
                {
                    Object.Destroy(Rigidbodies[i]);
                }
            }
            Rigidbodies = null;
        }

        #endregion
    }
}

