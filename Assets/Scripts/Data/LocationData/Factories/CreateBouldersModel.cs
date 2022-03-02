using UnityEngine;


namespace BeastHunter
{
    public class CreateBouldersModel : CreateInteractiveObjectModel
    {
        #region Constants

        private const InteractiveObjectType CREATED_DATA_TYPE = InteractiveObjectType.Boulders;

        #endregion


        #region Methods

        public override bool CanCreateModel(InteractiveObjectType data) => CREATED_DATA_TYPE == data;

        //public override SimpleInteractiveObjectModel CreateModel(GameObject instance, SimpleInteractiveObjectData data) =>
        //    new BouldersModel(instance, (BouldersData)data);
        public override BaseInteractiveObjectModel CreateModel(GameObject instance, BaseInteractiveObjectData data) =>
            new BouldersModel(instance, (BouldersData)data);

        #endregion
    }
}

