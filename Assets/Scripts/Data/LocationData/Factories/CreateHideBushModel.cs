using UnityEngine;


namespace BeastHunter
{
    public sealed class CreateHideBushModel : CreateInteractiveObjectModel
    {
        #region Constants

        private const InteractiveObjectType CREATED_DATA_TYPE = InteractiveObjectType.HideBush;

        #endregion


        #region Methods

        public override bool CanCreateModel(InteractiveObjectType data) => CREATED_DATA_TYPE == data;

        //public override SimpleInteractiveObjectModel CreateModel(GameObject instance, SimpleInteractiveObjectData data) =>
        //    new HideBushModel(instance, (HideBushData)data);
        public override BaseInteractiveObjectModel CreateModel(GameObject instance, BaseInteractiveObjectData data) =>
            new HideBushModel(instance, (HideBushData)data);

        #endregion
    }
}

