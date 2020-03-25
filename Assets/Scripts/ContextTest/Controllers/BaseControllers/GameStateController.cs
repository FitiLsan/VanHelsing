using System.Collections.Generic;


namespace BeastHunter
{
    public abstract class GameStateController
    {
        #region Fields

        private readonly List<ControllersStart> _updateFeatures;
        private readonly List<ControllersStart> _fixedUpdateFeatures;
        private readonly List<ControllersStart> _lateUpdateFeatures;

        #endregion


        #region ClassLifeCycles

        protected GameStateController(int capacity = 8)
        {
            _updateFeatures = new List<ControllersStart>(capacity);
            _fixedUpdateFeatures = new List<ControllersStart>(capacity);
            _lateUpdateFeatures = new List<ControllersStart>(capacity);
        }

        #endregion


        #region Metods

        protected void AddUpdateFeature(ControllersStart controller)
        {
            _updateFeatures.Add(controller);
        }

        protected void AddFixedUpdateFeature(ControllersStart controller)
        {
            _fixedUpdateFeatures.Add(controller);
        }

        protected void AddLateUpdateFeature(ControllersStart controller)
        {
            _lateUpdateFeatures.Add(controller);
        }

        public void Initialize()
        {
            foreach (var feature in _updateFeatures)
            {
                feature.OnAwake();
            }

            foreach(var feature in _fixedUpdateFeatures)
            {
                feature.OnAwake();
            }

            foreach(var feature in _lateUpdateFeatures)
            {
                feature.OnAwake();
            }
        }

        public void Updating(UpdateType updateType)
        {
            List<ControllersStart> features = null;
            switch (updateType)
            {
                case UpdateType.Update:
                    features = _updateFeatures;
                    break;

                case UpdateType.Late:
                    features = _lateUpdateFeatures;
                    break;

                case UpdateType.Fixed:
                    features = _fixedUpdateFeatures;
                    break;

                default:
                    break;
            }

            foreach (var feature in features)
            {
                feature.Updating();
            }
        }

        public void Cleanup(UpdateType updateType)
        {
            List<ControllersStart> features = null;
            switch (updateType)
            {
                case UpdateType.Fixed:
                    features = _fixedUpdateFeatures;
                    break;

                case UpdateType.Update:
                    features = _updateFeatures;
                    break;

                case UpdateType.Late:
                    features = _lateUpdateFeatures;
                    break;

                default:
                    break;
            }

            foreach (var feature in features)
            {
                feature.Cleanup();
            }
        }

        public void TearDown()
        {
            foreach (var feature in _fixedUpdateFeatures)
            {
                feature.TearDown();
            }

            foreach (var feature in _updateFeatures)
            {
                feature.TearDown();
            }

            foreach (var feature in _lateUpdateFeatures)
            {
                feature.TearDown();
            }
        }
        #endregion
    }
}