using System.Collections.Generic;
using BaseScripts;


namespace BeastHunter
{
    public class ControllersStart : BaseController
    {
        #region Fields

        protected readonly List<IAwake> _awakeControllers;
        protected readonly List<ITick> _tickControllers;

        #endregion


        #region ClassLifeCycles

        protected ControllersStart()
        {
            _awakeControllers = new List<IAwake>();
            _tickControllers = new List<ITick>();
        }

        #endregion


        #region Tick

        public override void Tick()
        {
            for (var index = 0; index < _tickControllers.Count; index++)
            {
                _tickControllers[index].Tick();
            }
        }

        #endregion


        #region OnAwake

        public override void OnAwake()
        {
            for (var index = 0; index < _awakeControllers.Count; index++)
            {
                _awakeControllers[index].OnAwake();
            }
        }

        #endregion


        #region Metods

        protected virtual ControllersStart Add(IController controller)
        {
            if (controller is ITick tickController)
            {
                _tickControllers.Add(tickController);
            }

            if (controller is IAwake awakeController)
            {
                _awakeControllers.Add(awakeController);
            }
            return this;
        }

        #endregion
    }
}
