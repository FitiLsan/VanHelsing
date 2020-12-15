using UnityEngine;
using Extensions;


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
            InteractiveObjectData = data;

            if(!data.CanvasObjectName.Equals("") && Prefab.transform.
                TryFind(data.CanvasObjectName, out Transform canvasObject))
            {
                CanvasObject = canvasObject;
                CanvasObject.gameObject.SetActive(false);
            }

            if (Prefab.TryGetComponentInChildren(out InteractableObjectBehavior interactiveObjectBehavior))
            {
                InteractiveBehavior = interactiveObjectBehavior;
            }

            IsActivated = data.IsActivated;
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

