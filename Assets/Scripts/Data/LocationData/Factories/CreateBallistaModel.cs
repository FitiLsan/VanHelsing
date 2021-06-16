using UnityEngine;


namespace BeastHunter
{
    public sealed class CreateBallistaModel : CreateInteractiveObjectModel
    {
        #region Constants

        private const InteractiveObjectType CREATED_DATA_TYPE = InteractiveObjectType.Ballista;

        #endregion


        #region Methods

        public override bool CanCreateModel(InteractiveObjectType data) => CREATED_DATA_TYPE == data;

        public override SimpleInteractiveObjectModel CreateModel(GameObject instance, SimpleInteractiveObjectData data) =>
            new BallistaModel(instance, (BallistaData)data);

        #endregion
    }
}

