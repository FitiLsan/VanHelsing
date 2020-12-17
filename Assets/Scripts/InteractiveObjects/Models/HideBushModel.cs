using System.Collections.Generic;
using UnityEngine;
using Extensions;

namespace BeastHunter
{
    public class HideBushModel : SimpleInteractiveObjectModel
    {
        #region Fields

        private HideBushData _data;

        #endregion


        #region Properties

        public bool IsBurning { get; set; }
        public HashSet<InteractableObjectBehavior> DamageObjects { get; set; }
        public float BurningTimer { get; set; }
        public float DealDamageCooldownTimer { get; set; }
        public GameObject Fire { get; }
        public GameObject Burnt { get; }

        #endregion


        #region ClassLifeCycle

        public HideBushModel(GameObject prefab, HideBushData data) : base(prefab, data)
        {
            _data = data;
            InteractableObjectBehavior[] ioBehaviors = prefab.GetComponentsInChildren<InteractableObjectBehavior>();
            InteractableObjectBehavior bushColliderIO = ioBehaviors.GetInteractableObjectByType(InteractableObjectType.Sphere);
            bushColliderIO.OnFilterHandler += FilterCollision;
            bushColliderIO.OnTriggerEnterHandler += OnTriggerEnter;
            bushColliderIO.OnTriggerExitHandler += OnTriggerExit;
            Collider bushCollider = bushColliderIO.GetComponent<SphereCollider>();

            Fire = prefab.transform.Find(_data.FireViewName).gameObject;
            Burnt = prefab.transform.Find(_data.BurnedViewName).gameObject;
        }

        #endregion


        #region Methods

        private bool FilterCollision(Collider collider) => _data.FilterCollision(collider, IsBurning);
        private void OnTriggerEnter(ITrigger trigger, Collider collider) => _data.TriggerEnter(collider, this);
        private void OnTriggerExit(ITrigger trigger, Collider collider) => _data.TriggerExit(collider, DamageObjects);

        public override void Updating()
        {
            if (IsBurning)
            {
                _data.Burning(this);
            }
        }

        public void Clean()
        {
            DamageObjects = null;
            Object.Destroy(Prefab.transform.Find(_data.MainColliderName).gameObject);
            Object.Destroy(Prefab.GetComponent<SphereCollider>());
            Object.Destroy(Prefab.GetComponent<InteractableObjectBehavior>());
            Object.Destroy(Prefab.transform.Find(_data.NormalViewName).gameObject);
            Object.Destroy(Fire);
        }

        #endregion
    }
}

