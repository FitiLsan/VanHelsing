using System;
using UnityEngine;
using UnityEngine.EventSystems;


namespace BeastHunterHubUI
{
    class MapCharacterView3DModelBehaviour : MonoBehaviour, IDragHandler, IDropHandler
    {
        #region Constants

        private const float ROTATE_SPEED = 100.0f;

        #endregion


        #region Properties

        public Action OnDropHandler { get; set; }
        public GameObject RotateObject { get; set; }

        #endregion


        #region IDragHandler

        public void OnDrag(PointerEventData eventData)
        {
            if (RotateObject != null)
            {
                float rotX = HubUIServices.SharedInstance.MainInput.Player.MouseLook.ReadValue<Vector2>().x * ROTATE_SPEED * Mathf.Deg2Rad;
                RotateObject.transform.Rotate(Vector3.up, -rotX);
            }
        }

        #endregion


        #region IDropHandler

        public void OnDrop(PointerEventData eventData)
        {
            OnDropHandler?.Invoke();
        }

        #endregion
    }
}
