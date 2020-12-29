using Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "FallingTreeData", menuName = "CreateData/SimpleInteractiveObjects/FallingTreeData", order = 0)]
    public sealed class FallingTreeData : SimpleInteractiveObjectData, IDealDamage
    {
        #region SerializeFields

        [Header("FallingTreeData")]
        [SerializeField] private bool _debugMessages;
        [SerializeField] private Damage _damage;
        [SerializeField] private Vector3 _prefabPosition;
        [SerializeField] private Vector3 _prefabEulers;
        [Tooltip("Default: (x: 1.5, y: 3.0, z: 1.5)")]
        [SerializeField] private Vector3 _treeSize;
        [Tooltip("Angle of inclination of the tree. Default: -25.0")]
        [SerializeField] private float _treeTilt;
        [SerializeField] private float _prefabOffsetY;
        [Tooltip("The time after which the tree physics is turned off. Default: 20.0")]
        [SerializeField] private float _timeToDeactivate;
        [Tooltip("Minimum speed at which the tree deals damage. Default: 3.0")]
        [SerializeField] private float _hitSpeed;
        [Tooltip("The name of prefab child gameobject containing the tree view")]
        [SerializeField] private string _viewChildName;

        [Header("Rigidbody")]
        [Tooltip("Default: 1000.0")]
        [SerializeField] private float _mass;
        [Tooltip("Default: 0.0")]
        [SerializeField] private float _drag;
        [Tooltip("Default: 0.05")]
        [SerializeField] private float _angularDrag;

        #endregion


        #region Fields

        private float _sqrHitSpeed;
        private Action _activateMsg;
        private Action _deactivateMsg;
        private Action<string> _dealDamageMsg;

        #endregion


        #region Properties

        public Vector3 PrefabPosition => _prefabPosition ;
        public Vector3 PrefabEulers => _prefabEulers;
        public float Mass => _mass;
        public float Drag => _drag;
        public float AngularDrag => _angularDrag;
        public float PrefabOffsetY => _prefabOffsetY;
        public string ViewChildName => _viewChildName;
        public Vector3 TreeSize => _treeSize;
        public float TreeTilt => _treeTilt;

        #endregion


        #region ClassLifeCycle

        public FallingTreeData()
        {
            _mass = 1000.0f;
            _drag = 0.0f;
            _angularDrag = 0.05f;
            _timeToDeactivate = 20.0f;
            _prefabOffsetY = 0.0f;
            _hitSpeed = 3.0f;
            _treeSize = new Vector3(1.5f , 3.0f, 1.5f);
            _treeTilt = -25.0f;
        }

        #endregion


        #region UnityMethods

        private void OnEnable()
        {
            _sqrHitSpeed = _hitSpeed * _hitSpeed;
            DebugMessages(_debugMessages);
        }

        #endregion

        #region SimpleInteractiveObjectData

        public override void MakeInteractive(BaseInteractiveObjectModel interactiveObjectModel, 
            ITrigger interactiveTrigger, Collider enteredCollider)
        {
            FallingTreeModel model = interactiveObjectModel as FallingTreeModel;
            if (!model.IsActivated)
            {
                model.CanvasObject.gameObject.SetActive(true);
                model.IsInteractive = true;
            }
        }

        public override void MakeNotInteractive(BaseInteractiveObjectModel interactiveObjectModel, 
            ITrigger interactiveTrigger, Collider exitedCollider)
        {
            FallingTreeModel model = interactiveObjectModel as FallingTreeModel;
            if (!model.IsActivated)
            {
                model.IsInteractive = false;
                model.CanvasObject.gameObject.SetActive(false);
            }
        }

        public override void Interact(BaseInteractiveObjectModel interactiveObjectModel)
        {
            FallingTreeModel model = interactiveObjectModel as FallingTreeModel;

            if (!model.IsActivated)
            {
                model.IsActivated = true;
                model.IsInteractive = false;
                Activate(model);
            }
        }

        protected override void Activate(SimpleInteractiveObjectModel interactiveObjectModel)
        {
            _activateMsg?.Invoke();

            FallingTreeModel model = interactiveObjectModel as FallingTreeModel;

            model.InteractiveTrigger.enabled = false;
            model.CanvasObject.gameObject.SetActive(false);

            model.DeactivateTimer = _timeToDeactivate;
            model.Rigidbody.isKinematic = false;

            model.DealDamageBehaviour.OnFilterHandler += CollisionFilter;
            model.DealDamageBehaviour.OnTriggerEnterHandler += (p1, p2) => TriggerEnter(p2, model);
            model.DealDamageBehaviour.OnTriggerExitHandler += (p1, p2) => TriggerExit(p2, model);
            model.DealDamageCollider.enabled = true;
        }

        protected override void Deactivate(SimpleInteractiveObjectModel interactiveObjectModel)
        {
            _deactivateMsg?.Invoke();
            (interactiveObjectModel as FallingTreeModel).Clean();
        }

        #endregion


        #region FallingTreeModel

        public void Act(FallingTreeModel model)
        {
            //note: list for adding the dictionary values to be changed after foreach-cycle
            List<int> changingDictionaryValues = new List<int>();

            foreach(KeyValuePair<int, InteractableObjectBehavior> kvp in model.StayCollisionEntities)
            {
                if (kvp.Value != null && model.Rigidbody.velocity.sqrMagnitude > _sqrHitSpeed)
                {
                    DealDamage(kvp.Value, _damage);
                    changingDictionaryValues.Add(kvp.Key);
                }
            }

            for (int i = 0; i < changingDictionaryValues.Count; i++)
            {
                if (model.StayCollisionEntities.ContainsKey(changingDictionaryValues[i]))
                {
                    //note: set value as null means the entity has already taken damage
                    model.StayCollisionEntities[changingDictionaryValues[i]] = null;
                }
            }

            model.DeactivateTimer -= Time.deltaTime;
            if (model.DeactivateTimer <= 0)
            {
                model.IsActivated = false;
                Deactivate(model);
            }
        }

        public bool CollisionFilter(Collider collider)
        {
            if (!collider.isTrigger)
            {
                InteractableObjectBehavior objectBehavior = collider.GetComponentInChildren<InteractableObjectBehavior>();
                return objectBehavior != null
                    && (objectBehavior.Type == InteractableObjectType.Enemy
                    || objectBehavior.Type == InteractableObjectType.Player);
            }
            return false;
        }

        public void TriggerEnter(Collider collider, FallingTreeModel model)
        {
            int entityID = collider.transform.GetMainParent().GetInstanceID();
            InteractableObjectBehavior entityIO = collider.GetComponent<InteractableObjectBehavior>();

            if (!model.StayCollisionEntities.ContainsKey(entityID))
            {
                model.StayCollisionEntities.Add(entityID, entityIO);
            }
            else if (model.StayCollisionEntities[entityID] != null)
            {
                //note: overwrite to the last entity IOBehavior, that touched the trigger
                model.StayCollisionEntities[entityID] = entityIO;
            }

            if (model.Rigidbody.velocity.sqrMagnitude > _sqrHitSpeed && model.StayCollisionEntities[entityID] != null)
            {
                DealDamage(entityIO, _damage);
                //note: set value as null means the entity has already taken damage
                model.StayCollisionEntities[entityID] = null;
            }
        }

        public void TriggerExit(Collider collider, FallingTreeModel model)
        {
            int entityID = collider.transform.GetMainParent().GetInstanceID();
            if (model.StayCollisionEntities.ContainsKey(entityID) && model.StayCollisionEntities[entityID] != null)
            {
                //note: the entity is removed from the dictionary only if it has not yet taken damage (IOBehavior!=null)
                model.StayCollisionEntities.Remove(collider.transform.GetMainParent().GetInstanceID());
            }
        }

        #endregion


        #region Methods

            private void DebugMessages(bool switcher)
        {
            if (switcher)
            {
                _activateMsg = () => Debug.Log("FallingTree activated");
                _deactivateMsg = () => Debug.Log("FallingTree deactivated");
                _dealDamageMsg = (enemy) => Debug.Log("FallingTree deal damage to " + enemy);
            }
        }

        #endregion


        #region IDealDamage

        public void DealDamage(InteractableObjectBehavior enemy, Damage damage)
        {
            Damage countDamage = Services.SharedInstance.AttackService
                .CountDamage(damage, enemy.transform.GetMainParent().gameObject.GetInstanceID());

            _dealDamageMsg?.Invoke(enemy.ToString());
            enemy.TakeDamageEvent(countDamage);

        }

        #endregion
    }
}

