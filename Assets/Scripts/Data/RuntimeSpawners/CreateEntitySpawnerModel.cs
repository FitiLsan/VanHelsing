using UnityEngine;


namespace BeastHunter
{
    public sealed class CreateEntitySpawnerModel : CreateInteractiveObjectModel
    {
        #region Constants

        private const InteractiveObjectType CREATED_DATA_TYPE = InteractiveObjectType.EntitySpawner;

        #endregion


        #region Methods

        public override bool CanCreateModel(InteractiveObjectType data) => CREATED_DATA_TYPE == data;

        public override BaseInteractiveObjectModel CreateModel(GameObject instance, BaseInteractiveObjectData data) =>
            new EntitySpawnerModel(instance, (EntitySpawnerData)data);

        #endregion
    }
}