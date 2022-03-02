using System;
using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewModel", menuName = "Spawner", order = 0)]
    public sealed class EntitySpawnerData : BaseInteractiveObjectData, ISpawnerable   
    {
        #region Fields

        public float TimeToSpawn = 10.0f;

        #endregion


        #region ISpawnerable

        [SerializeField] private SpawnerableClass _spawnerData;
        public SpawnerableClass SpawnerData => _spawnerData;

        #endregion


        #region BaseInteractiveObjectData

        public override void Interact(BaseInteractiveObjectModel interactiveObjectModel)
        {
            interactiveObjectModel.IsInteractive = true;
            _spawnerData.Initialize();
            //var timer = _spawnerData.GetSpawnerTrigger as TimerTrigger;
            //TimeToSpawn = timer.TimeToSpawn;
        }

        public override void MakeInteractive(BaseInteractiveObjectModel interactiveObjectModel, ITrigger interactiveTrigger, Collider enteredCollider)
        {
            interactiveObjectModel.IsInteractive = true;
        }

        public override void MakeNotInteractive(BaseInteractiveObjectModel interactiveObjectModel, ITrigger interactiveTrigger, Collider enteredCollider)
        {
            interactiveObjectModel.IsInteractive = false;
        }

        #endregion


        #region Methods


        public void Act(BaseInteractiveObjectModel interactiveObjectModel)
        {
            if (_spawnerData == null)
            {
                Interact(interactiveObjectModel);
            }
            EntitySpawnerModel model = interactiveObjectModel as EntitySpawnerModel;

            if (model.IsInteractive)
            {
                //var pos = new LocationPosition(Vector3.forward, Vector3.zero, Vector3.one);
                //SpawnerData.Spawn(pos, model.SpawnerTransform.position, model.TimeElapsed);
                model.TimeElapsed += Time.deltaTime;
                if (model.TimeElapsed >= TimeToSpawn)
                {
                    model.TimeElapsed = 0.0f;
                    var pos = new LocationPosition(Vector3.forward, Vector3.zero, Vector3.one);
                    SpawnerData.Spawn(pos, model.SpawnerTransform.position);
                }

            }
        }


        #endregion
    }
}

