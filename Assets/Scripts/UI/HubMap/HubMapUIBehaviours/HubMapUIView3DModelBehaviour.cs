using System;
using UnityEngine;
using UnityEngine.EventSystems;


namespace BeastHunter
{
    class HubMapUIView3DModelBehaviour : MonoBehaviour, IDragHandler, IDropHandler
    {
        private const float ROTATE_SPEED = 100.0f;

        public Action OnDropHandler { get; set; }
        public GameObject RotateObject { get; set; }

        public void OnDrag(PointerEventData eventData)
        {
            if (RotateObject != null)
            {
                float rotX = Input.GetAxis("Mouse X") * ROTATE_SPEED * Mathf.Deg2Rad;
                RotateObject.transform.Rotate(Vector3.up, -rotX);
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            OnDropHandler?.Invoke();
        }
    }
}
