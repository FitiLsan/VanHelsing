using UnityEngine;
using UniRx;
using System.Collections.Generic;
using Extensions;
using System;

namespace BeastHunter
{
    [CreateAssetMenu(fileName = "HideBushData", menuName = "CreateData/SimpleInteractiveObjects/HideBushData", order = 0)]
    public sealed class HideBushData : SimpleInteractiveObjectData, IDealDamage
    {
        #region SerializeFields

        [Header("HideBushData")]
        [Tooltip("On/off hide bush debug messages")]
        [SerializeField] private bool _debugMessages;
        [Tooltip("Damage for 1 tick when burning")]
        [SerializeField] private Damage _damage;
        [SerializeField] private Vector3 _prefabPosition;
        [SerializeField] private Vector3 _prefabEulers;
        [Tooltip("Bush burning time. Default: 30.0")]
        [SerializeField] private float _burningTime;
        [Tooltip("Damage cooldown when burning. Default: 1.0")]
        [SerializeField] private float _dealDamageCooldown;

        [Header("Prefab child gameobject names")]
        [Tooltip("Child gameobject containing main bush collider")]
        [SerializeField] private string _mainColliderName;
        [Tooltip("Child gameobject containing bush view in normal state")]
        [SerializeField] private string _normalViewName;
        [Tooltip("Child gameobject containing fire effect for burning state")]
        [SerializeField] private string _fireViewName;
        [Tooltip("Child gameobject containing bush view after burning")]
        [SerializeField] private string _burnedViewName;

        #endregion


        #region Fields

        private Action _startBurningMsg;
        private Action _burnedMsg;
        private Action _burningDamageTickMsg;
        private Action<string> _addDamageListMsg;
        private Action<string> _removeDamageListMsg;
        private Action<string> _dealDamageMsg;

        #endregion


        #region Properties

        public Vector3 PrefabPosition => _prefabPosition;
        public Vector3 PrefabEulers => _prefabEulers;
        public string MainColliderName => _mainColliderName;
        public string NormalViewName => _normalViewName;
        public string FireViewName => _fireViewName;
        public string BurnedViewName => _burnedViewName;

        #endregion


        #region ClassLifeCycle

        public HideBushData()
        {
            _burningTime = 20.0f;
            _dealDamageCooldown = 1.5f;
            _mainColliderName = "MainCollider";
            _normalViewName = "NormalView";
            _fireViewName = "FireView";
            _burnedViewName = "BurnedView";
        }

        #endregion


        #region UnityMethods

        private void OnEnable()
        {
            DebugMessages(_debugMessages);
        }

        #endregion


        #region Methods

        public override void MakeInteractive(BaseInteractiveObjectModel interactiveObjectModel,
            ITrigger interactiveTrigger, Collider enteredCollider)
        {
            MessageBroker.Default.Publish(new OnPlayerReachHidePlaceEventClass(true));
        }

        public override void MakeNotInteractive(BaseInteractiveObjectModel interactiveObjectModel,
            ITrigger interactiveTrigger, Collider exitedCollider)
        {
            MessageBroker.Default.Publish(new OnPlayerReachHidePlaceEventClass(false));
            MessageBroker.Default.Publish(new OnPlayerHideEventClass(false));
        }

        protected override void Activate(SimpleInteractiveObjectModel interactiveObjectModel)
        {
            // TODO
        }

        protected override void Deactivate(SimpleInteractiveObjectModel interactiveObjectModel)
        {
            // TODO
        }

        public bool FilterCollision(Collider collider, bool isBurning)
        {
            InteractableObjectBehavior behaviorIO = collider.GetComponent<InteractableObjectBehavior>();

            if (!isBurning)
            {
                return behaviorIO != null
                    && behaviorIO.Type == InteractableObjectType.Fire;
                    //&& behaviorIO.IsInteractable;     //OFF - only for test, if ON - crossbow bolt with fireIO don't work on test
            }
            else
            {
                return behaviorIO != null
                    && (behaviorIO.Type == InteractableObjectType.Player
                    || behaviorIO.Type == InteractableObjectType.Enemy);
            }
        }

        public void TriggerEnter(Collider collider, HideBushModel model)
        {
            InteractableObjectBehavior behaviorIO = collider.GetComponent<InteractableObjectBehavior>();

            if (behaviorIO.Type == InteractableObjectType.Fire)
            {
                SetBurningState(model);
            }
            else
            {
                _addDamageListMsg?.Invoke(behaviorIO.gameObject.ToString());
                model.DamageObjects.Add(behaviorIO);
            }
        }

        public void TriggerExit(Collider collider, HashSet<InteractableObjectBehavior> damageObjects)
        {
            InteractableObjectBehavior behaviorIO = collider.GetComponent<InteractableObjectBehavior>();
            if (behaviorIO.Type != InteractableObjectType.Fire)
            {
                _removeDamageListMsg?.Invoke(behaviorIO.gameObject.ToString());
                damageObjects.Remove(behaviorIO);
            }
        }

        public void Burning(HideBushModel model)
        {
            if (model.BurningTimer <= 0)
            {
                _burnedMsg?.Invoke();

                model.IsBurning = false;
                model.DamageObjects.Clear();
                model.Burnt.SetActive(true);
                model.Clean();
            }

            if (model.DealDamageCooldownTimer <= 0)
            {
                _burningDamageTickMsg?.Invoke();

                HashSet<int> damageDone = new HashSet<int>();
                foreach (InteractableObjectBehavior behaviorIO in model.DamageObjects)
                {
                    int gameObjectID = behaviorIO.transform.GetMainParent().GetInstanceID();
                    if (!damageDone.Contains(gameObjectID)) //to avoid causing double damage to the object in the case of multiple colliders with behaviorIO
                    {
                        DealDamage(behaviorIO, _damage);
                        damageDone.Add(gameObjectID);
                    }
                }
                model.DealDamageCooldownTimer = _dealDamageCooldown;
            }

            model.BurningTimer -= Time.deltaTime;
            model.DealDamageCooldownTimer -= Time.deltaTime;
        }

        private void SetBurningState(HideBushModel model)
        {
            _startBurningMsg?.Invoke();

            model.DamageObjects = new HashSet<InteractableObjectBehavior>();
            model.Fire.SetActive(true);
            model.BurningTimer = _burningTime;
            model.DealDamageCooldownTimer = _dealDamageCooldown;
            model.IsBurning = true;
        }

        private void DebugMessages(bool switcher)
        {
            if (switcher)
            {
                _startBurningMsg = () => Debug.Log("The bush caught fire");
                _burnedMsg = () => Debug.Log("The bush burned");
                _burningDamageTickMsg = () => Debug.Log("Fire damage tick");
                _addDamageListMsg = (entity) => Debug.Log(entity + " added to bush deal damage list");
                _removeDamageListMsg = (entity) => Debug.Log(entity + " removed from bush deal damage list");
                _dealDamageMsg = (entity) => Debug.Log("Burning bush deal damage to " + entity);
            }
        }

        #endregion


        #region IDealDamage

        public void DealDamage(InteractableObjectBehavior enemy, Damage damage)
        {
            Damage countDamage = Services.SharedInstance.AttackService
                .CountDamage(damage, enemy.transform.GetMainParent().gameObject.GetInstanceID());

            _dealDamageMsg?.Invoke(enemy.gameObject.ToString());
            enemy.TakeDamageEvent(countDamage);
        }

        #endregion
    }
}

