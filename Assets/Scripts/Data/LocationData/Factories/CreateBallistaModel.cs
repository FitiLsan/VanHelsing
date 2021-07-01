using UnityEngine;


namespace BeastHunter
{
    public sealed class CreateBallistaModel : CreateInteractiveObjectModel
    {
        #region Constants

        private const InteractiveObjectType CREATED_DATA_TYPE = InteractiveObjectType.Ballista;
        private GameContext _context;

        public CreateBallistaModel(GameContext context)
        {
            _context = context;
        }

        #endregion


        #region Methods

        public override bool CanCreateModel(InteractiveObjectType data) => CREATED_DATA_TYPE == data;

        public override BaseInteractiveObjectModel CreateModel(GameObject instance, BaseInteractiveObjectData data) =>
        new BallistaModel(instance, (BallistaData)data, _context);

        #endregion
    }
}

