using System.Collections.Generic;


namespace BeastHunter
{
    public abstract class GameStateController
    {
        #region Fields

        private readonly List<ControllersStart> _updateFeatures;

        #endregion


        #region ClassLifeCycles

        protected GameStateController(int capacity = 8)
        {
            _updateFeatures = new List<ControllersStart>(capacity);
        }

        #endregion


        #region Metods

        public void Initialize()
        {

            foreach (var feature in _updateFeatures)
            {
                feature.OnAwake();
            }

        }

        protected void AddUpdateFeature(ControllersStart controller)
        {
            _updateFeatures.Add(controller);
        }

        public void Updating(UpdateType updateType)
        {
            List<ControllersStart> features = null;
            switch (updateType)
            {
                case UpdateType.Update:
                    features = _updateFeatures;
                    break;

                default:
                    break;
            }

            foreach (var feature in features)
            {
                feature.Updating();
            }
        }
        #endregion
    }
}