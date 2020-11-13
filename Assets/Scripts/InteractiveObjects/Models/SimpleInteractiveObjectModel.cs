using UnityEngine;


namespace BeastHunter
{
    public class SimpleInteractiveObjectModel : BaseInteractiveObjectModel, IUpdate, IHaveInteractiveBehavior
    {
        #region Properties

        public GameObject Prefab { get; protected set; }
        public Transform CanvasObject { get; protected set; }
        public InteractableObjectBehavior InteractiveBehavior { get; protected set; }
        public bool IsActivated { get; set; }

        #endregion


        #region ClassLifeCycle

        public SimpleInteractiveObjectModel(GameObject prefab, SimpleInteractiveObjectData data)
        {
            Prefab = prefab;
            CanvasObject = Prefab.transform.GetChild(0);
            CanvasObject.gameObject.SetActive(false);
            InteractiveBehavior = Prefab.GetComponentInChildren<InteractableObjectBehavior>();
            InteractiveObjectData = data;
            IsActivated = (InteractiveObjectData as SimpleInteractiveObjectData).IsActivated;
            IsInteractive = false;
        }

        #endregion


        #region IUpdate

        public virtual void Updating()
        {
            if (IsInteractive) CanvasObject.transform.LookAt(Services.SharedInstance.
                CameraService.CharacterCamera.transform);
        }

        #endregion
    }
}

