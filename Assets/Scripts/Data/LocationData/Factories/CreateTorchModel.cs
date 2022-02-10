using UnityEngine;


namespace BeastHunter
{
    public sealed class CreateTorchModel : CreateInteractiveObjectModel
    {
        #region Constants

        private const InteractiveObjectType CREATED_DATA_TYPE = InteractiveObjectType.Torch;

        #endregion


        #region Methods

        public override bool CanCreateModel(InteractiveObjectType data) => CREATED_DATA_TYPE == data;

        //public override SimpleInteractiveObjectModel CreateModel(GameObject instance, SimpleInteractiveObjectData data) =>
            //new TorchModel(instance, (TorchData)data);
        public override BaseInteractiveObjectModel CreateModel(GameObject instance, BaseInteractiveObjectData data) =>
            new TorchModel(instance, (TorchData)data);

        #endregion
    }
}

