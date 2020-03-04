using System.Collections.Generic;


namespace BeastHunter
{
    public class ControllersStart : IAwake, IUpdate
    {
        #region Fields

        protected readonly List<IAwake> _awakeControllers;
        protected readonly List<IUpdate> _updateControllers;

        #endregion


        #region ClassLifeCycles

        protected ControllersStart()
        {
            _awakeControllers = new List<IAwake>();
            _updateControllers = new List<IUpdate>();
        }

        #endregion


        #region Updating

        public void Updating()
        {
            for (var index = 0; index < _updateControllers.Count; index++)
            {
                _updateControllers[index].Updating();
            }
        }

        #endregion


        #region OnAwake

        public void OnAwake()
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
            if (controller is IUpdate updateController)
            {
                _updateControllers.Add(updateController);
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
