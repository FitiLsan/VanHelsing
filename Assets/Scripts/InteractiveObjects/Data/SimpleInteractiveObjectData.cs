using UnityEngine;


namespace BeastHunter
{
    public abstract class SimpleInteractiveObjectData : BaseInteractiveObjectData
    {
        #region Fields

        [SerializeField] private bool _isActivated;
        [SerializeField] private string _canvasObjectName;

        #endregion


        #region Properties

        public bool IsActivated => _isActivated;
        public string CanvasObjectName => _canvasObjectName;

        #endregion


        #region Methods

        public override void Interact(BaseInteractiveObjectModel interactiveObjectModel)
        {
            SimpleInteractiveObjectModel simpleInteractiveObjectModel = interactiveObjectModel as 
                SimpleInteractiveObjectModel;
            if (simpleInteractiveObjectModel.IsActivated)
            {
                Deactivate(simpleInteractiveObjectModel);
            }
            else
            {
                Activate(simpleInteractiveObjectModel);
            }

            simpleInteractiveObjectModel.IsActivated = !simpleInteractiveObjectModel.IsActivated;
        }

        protected abstract void Activate(SimpleInteractiveObjectModel interactiveObjectModel);

        protected abstract void Deactivate(SimpleInteractiveObjectModel interactiveObjectModel);

        #endregion
    }
}

