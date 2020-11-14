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
            InteractiveObjectData = data;
            CanvasObject = Prefab.transform.Find(data.CanvasObjectName);

            if(CanvasObject != null)
            {
                CanvasObject.gameObject.SetActive(false);
            }
            else
            {
                throw new System.Exception("Can't find canvas object on simple interactive object prefab");
            }
            
            InteractiveBehavior = Prefab.GetComponentInChildren<InteractableObjectBehavior>();   
            

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

