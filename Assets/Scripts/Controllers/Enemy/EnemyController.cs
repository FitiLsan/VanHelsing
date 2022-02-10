using System.Linq;


namespace BeastHunter
{
    public class EnemyController : IAwake, IUpdate, ITearDown
    {
        #region Fields

        private readonly GameContext _context;

        #endregion


        #region ClassLifeCycles

        public EnemyController(GameContext context)
        {
            _context = context;
        }

        #endregion


        #region IAwake

        public void OnAwake()
        {
            foreach(var model in _context.NpcModels)
            {
                if(model.Value is IAwake modelAwake) modelAwake.OnAwake();
            }
        }

        #endregion


        #region IUpdate

        public void Updating()
        {
            foreach (var model in _context.NpcModels.Values)
            {
                model.ThisEnemyData.Act(model);
            }
        }

        #endregion


        #region ITearDownController

        public void TearDown()
        {
            foreach (var model in _context.NpcModels)
            {
                if (model.Value is ITearDown modelTearDown) modelTearDown.TearDown();
            }
        }

        #endregion


        //#region Methods

        //public void TakeDamage(int gameObjectId, Damage damage)
        //{
        //    _context.NpcModels[gameObjectId].TakeDamage(damage);
        //}

        //#endregion
    }
}

