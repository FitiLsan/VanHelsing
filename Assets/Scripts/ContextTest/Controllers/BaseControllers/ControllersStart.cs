using System.Collections.Generic;


namespace BeastHunter
{
    public class ControllersStart : IAwake, IUpdate, ICleanup, ITearDown
    {
        #region Fields

        protected readonly List<IAwake> _awakeControllers;
        protected readonly List<IUpdate> _updateControllers;
        protected readonly List<ICleanup> _cleanupControllers;
        protected readonly List<ITearDown> _tearDownControllers;

        #endregion


        #region ClassLifeCycles

        protected ControllersStart()
        {
            _awakeControllers = new List<IAwake>();
            _updateControllers = new List<IUpdate>();
            _cleanupControllers = new List<ICleanup>();
            _tearDownControllers = new List<ITearDown>();
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


        #region ICleanup

        public virtual void Cleanup()
        {
            for (var index = 0; index < _cleanupControllers.Count; index++)
            {
                _cleanupControllers[index].Cleanup();
            }
        }

        #endregion


        #region ITearDown

        public virtual void TearDown()
        {
            for (var index = 0; index < _tearDownControllers.Count; index++)
            {
                _tearDownControllers[index].TearDown();
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

            if (controller is ICleanup cleanupController)
            {
                _cleanupControllers.Add(cleanupController);
            }

            if (controller is ITearDown tearDownController)
            {
                _tearDownControllers.Add(tearDownController);
            }
            return this;
        }

        #endregion
    }
}
