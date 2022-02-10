using UnityEngine;


namespace BeastHunter
{
    public sealed class EntitySpawnerModel : BaseInteractiveObjectModel, IUpdate //ITimerTrigger with 
    {
        #region Fields

        public float TimeElapsed = 0.0f;
        //public float TimeToSpawn = 10.0f;

        #endregion


        #region Properties

        public Transform SpawnerTransform { get; }

        #endregion


        #region ClassLifeCycle

        public EntitySpawnerModel(GameObject objectOnScene, EntitySpawnerData data)
        {
            InteractiveObjectData = data;
            InteractiveObjectData.Interact(this);
            SpawnerTransform = objectOnScene.transform;
        }

        #endregion


        #region IUpdate

        public void Updating()
        {
            (InteractiveObjectData as EntitySpawnerData).Act(this);
        }

        #endregion
    }
}

