using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewSmallCatapultTrapData", menuName = "CreateTrapData/CreateSmallCatapultData", order = 0)]
    public class SmallCatapultTrapData : TrapData
    {
        #region Fields

        [SerializeField] private ProjectileData _launchingProjectileData;
        [SerializeField] private float _timeToLaunchProjectile;
        [SerializeField] private float _launchForceUp;
        [SerializeField] private float _launchForceForward;
        [SerializeField] private string _launchParentObjectPath;

        #endregion


        #region Properties

        public float TimeToLaunchProjectile => _timeToLaunchProjectile;
        public float LaunchForceUp => _launchForceUp;
        public float LaunchForceForward => _launchForceForward;
        public string LaunchParentObjectPath => _launchParentObjectPath;

        #endregion


        #region Methods

        public override void Place(GameContext context, TrapModel trapModel)
        {
            base.Place(context, trapModel);

            Context.TrapModels.Add(CurrentTrapModel.
                TrapObjectInFrontOfCharacter.GetInstanceID(), CurrentTrapModel);

            CurrentTrapModel.TrapObjectInFrontOfCharacter.GetComponent<Animation>().enabled = true;

            Collider[] trapColliders = CurrentTrapModel.TrapObjectInFrontOfCharacter.GetComponentsInChildren<Collider>();

            foreach (var collider in trapColliders)
            {
                if(collider is SphereCollider)
                {
                    (collider as SphereCollider).radius = TrapStruct.Duration;
                }
                collider.enabled = true;
            }

            Renderer[] trapRenderers = CurrentTrapModel.TrapObjectInFrontOfCharacter.GetComponentsInChildren<Renderer>();

            foreach (var renderer in trapRenderers)
            {
                if (renderer.enabled)
                {
                    if (renderer.GetComponent<Collider>() != null)
                    {
                        renderer.material = Data.MaterialsData.SpoonTrapHoldersMaterial;
                    }
                    else
                    {
                        renderer.material = Data.MaterialsData.SpoonTrapWoodMaterial;
                    }
                }
                else
                {
                    renderer.enabled = true;
                }
            }

            TrapBehaviour newTrapBehavior = CurrentTrapModel.TrapObjectInFrontOfCharacter.
                GetComponent<TrapBehaviour>();
            newTrapBehavior.enabled = true;
            newTrapBehavior.IsInteractable = true;

            newTrapBehavior.OnFilterHandler += OnTriggerFilter;
            newTrapBehavior.OnTriggerEnterHandler += OnTriggerEnterSomething;

            TrapsAmount--;
        }

        public override bool OnTriggerFilter(Collider enteredCollider)
        {
            return !enteredCollider.isTrigger && enteredCollider.GetComponent<InteractableObjectBehavior>()?.
                Type == InteractableObjectType.Enemy;
        }

        public override void OnTriggerEnterSomething(ITrigger activatedTrapITrigger, Collider enteredCollider)
        {
            Activate(activatedTrapITrigger, enteredCollider);
        }

        private void Activate(ITrigger activatedTrapITrigger, Collider enteredCollider)
        {
            TrapModel activatedTrapModel;
            Context.TrapModels.TryGetValue(activatedTrapITrigger.GameObject.GetInstanceID(), out activatedTrapModel);

            activatedTrapModel.TrapObjectInFrontOfCharacter.transform.Find(_launchParentObjectPath).
                gameObject.SetActive(true);

            if (activatedTrapITrigger != null)
            {
                if (activatedTrapModel.ChargeAmount > 0)
                {
                    activatedTrapITrigger.GameObject.transform.LookAt(enteredCollider.transform);
                    activatedTrapModel.TrapObjectInFrontOfCharacter.GetComponentInChildren<Animation>().Play();
                    TimeRemaining launch = new TimeRemaining(() => LaunchProjectile(activatedTrapModel, 
                        enteredCollider), TimeToLaunchProjectile);
                    launch.AddTimeRemaining(TimeToLaunchProjectile);
                }
                else
                {
                    DeleteTrap(activatedTrapModel);
                }
            }
        }

        private void LaunchProjectile(TrapModel activatedTrapModel, Collider target)
        {
            Transform _launchParentObect = activatedTrapModel.TrapObjectInFrontOfCharacter.
                transform.Find(_launchParentObjectPath);
            Vector3 projectileForceVector = (target.transform.position - _launchParentObect.position +
                    Vector3.up * LaunchForceUp) * LaunchForceForward;
            new ProjectileInitializeController(Context, _launchingProjectileData, _launchParentObect.position,
                projectileForceVector, ForceMode.Impulse);
            DeleteTrap(activatedTrapModel);
        }

        private void DeleteTrap(TrapModel activatedTrapModel)
        {
            TrapBehaviour activatedTrapBehavior = activatedTrapModel.TrapObjectInFrontOfCharacter.
                GetComponent<TrapBehaviour>();
            activatedTrapBehavior.OnFilterHandler -= OnTriggerFilter;
            activatedTrapBehavior.OnTriggerEnterHandler -= OnTriggerEnterSomething;

            Context.TrapModels.Remove(activatedTrapModel.TrapObjectInFrontOfCharacter.GetInstanceID());
            Destroy(activatedTrapModel.TrapObjectInFrontOfCharacter, TrapStruct.TImeToDestroyAfterActivation);
        }

        #endregion
    }
}

