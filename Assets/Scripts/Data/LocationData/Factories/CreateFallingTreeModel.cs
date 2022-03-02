using UnityEngine;


namespace BeastHunter
{
    public sealed class CreateFallingTreeModel : CreateInteractiveObjectModel
    {
        #region Constants

        private const InteractiveObjectType CREATED_DATA_TYPE = InteractiveObjectType.FallingTree;

        #endregion


        #region Methods

        public override bool CanCreateModel(InteractiveObjectType data) => CREATED_DATA_TYPE == data;

        //public override SimpleInteractiveObjectModel CreateModel(GameObject instance, SimpleInteractiveObjectData data) =>
        //    new FallingTreeModel(instance, (FallingTreeData)data);
        public override BaseInteractiveObjectModel CreateModel(GameObject instance, BaseInteractiveObjectData data) =>
            new FallingTreeModel(instance, (FallingTreeData)data);

        #endregion
    }
}

