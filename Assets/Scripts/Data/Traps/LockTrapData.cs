using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewLockTrapData", menuName = "CreateTrapData/CreateTrapLockData", order = 0)]
    public sealed class LockTrapData : TrapData
    {
        #region Methods

        public override void Place(GameContext context, TrapModel trapModel)
        {
            base.Place(context, trapModel);

            Context.TrapModels.Add(CurrentTrapModel.
                TrapObjectInFrontOfCharacter.GetInstanceID(), CurrentTrapModel);

            CurrentTrapModel.TrapObjectInFrontOfCharacter.GetComponent<Collider>().enabled = true;
            CurrentTrapModel.TrapObjectInFrontOfCharacter.GetComponent<Animator>().enabled = true;

            TrapBehaviour newTrapBehavior = CurrentTrapModel.TrapObjectInFrontOfCharacter.
                GetComponent<TrapBehaviour>();
            newTrapBehavior.enabled = true;
            newTrapBehavior.IsInteractable = true;

            newTrapBehavior.OnFilterHandler += OnTriggerFilter;
            newTrapBehavior.OnTriggerEnterHandler += OnTriggerEnterSomething;

            CurrentTrapModel.TrapObjectInFrontOfCharacter.GetComponentInChildren<Renderer>().
                material = Data.MaterialsData.MetalLockTrapMaterial;

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

        private void Activate(ITrigger activatedTrapITrigger, Collider other)
        {
            TrapModel activatedTrapModel;
            Context.TrapModels.TryGetValue(activatedTrapITrigger.GameObject.GetInstanceID(), out activatedTrapModel);

            if(activatedTrapITrigger != null)
            {
                if (activatedTrapModel.ChargeAmount > 0)
                {
                    Context.NpcModels[other.gameObject.GetInstanceID()].TakeDamage(
                        Services.SharedInstance.AttackService.CountDamage(TrapStruct.TrapDamage,
                            Context.NpcModels[other.gameObject.GetInstanceID()].GetStats().MainStats));

                    activatedTrapModel.TrapObjectInFrontOfCharacter.GetComponent<Animator>().Play(AnimationName);
                    activatedTrapModel.ChargeAmount--;

                    other.transform.position = activatedTrapModel.TrapObjectInFrontOfCharacter.transform.position;

                    DeleteTrap(activatedTrapModel);
                }
                else
                {
                    DeleteTrap(activatedTrapModel);
                }
            }
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

