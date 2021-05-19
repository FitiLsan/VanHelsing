using System;
using UnityEngine;

namespace BeastHunter
{
    [Serializable]
    public sealed class SpawnerableClass
    {
        #region Fields

        private InitializationService _initializeService = null;
        //private SpawnerTrigger _spawnerTrigger = null;
        //private CreateSpawnerTrigger _spawnerTriggerFatory; //public for inspector

        public SpawnerTriggerType trigger;
        public EnemyData spawnData;

        #endregion


        #region Properties

        //public SpawnerTrigger GetSpawnerTrigger { get => _spawnerTrigger; }


        #endregion


        #region Methods

        public void Initialize()
        {
            _initializeService = Services.SharedInstance.InitializationService;
            //_spawnerTriggerFatory = new CreateSpawnerTrigger();
            //_spawnerTrigger = _spawnerTriggerFatory.CreateTrigger(trigger);
        }

        public void Spawn(LocationPosition position) //, BaseInteractiveObjectModel interactiveObjectModel
        {
            Vector3 pos = Vector3.zero;
            Spawn(position, pos);
        }


        public void Spawn(LocationPosition position, Vector3 parentPosition) //, BaseInteractiveObjectModel interactiveObjectModel
        {
            //if (_initializeService == null)
            //{
            //    _initializeService = Services.SharedInstance.InitializationService;
            //}

            if (ExecuteTrigger())
            {
                _initializeService.InitializeEnemy(spawnData, position, parentPosition);
            }
        }

        private bool ExecuteTrigger() //BaseInteractiveObjectModel interactiveObjectModel
        {
            return true;
            //if (trigger.HasFlag(SpawnerTriggerType.Timer))
            //{
                
            //    var timerTrigger = _spawnerTrigger as TimerTrigger;
            //    timeElapsed += Time.deltaTime;
            //    Debug.Log(timeElapsed);
            //    if (timeElapsed >= timerTrigger.TimeToSpawn)
            //    {
            //        timeElapsed = 0.0f;
            //        args[0] = timeElapsed;
            //        return true;
            //    }
            //    args[0] = timeElapsed;
            //}
            //return false;
        }

        #endregion
    }
}
