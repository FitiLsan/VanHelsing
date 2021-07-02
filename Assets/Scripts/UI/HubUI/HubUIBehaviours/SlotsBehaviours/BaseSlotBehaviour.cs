using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;


namespace BeastHunterHubUI
{
    public abstract class BaseSlotBehaviour<EntityType, StorageType> : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler where StorageType : Enum
    {
        #region Constants

        private const float DOUBLECLICK_TIME = 0.5f;

        #endregion


        #region Fields

        [SerializeField] protected Image _changeableImage;
        [SerializeField] protected GameObject _objectForDrag;

        protected GameObject _currentDraggedObject;
        protected StorageType _storageType;
        private float _lastClickTime;

        #endregion


        #region Properties

        protected virtual int _storageSlotIndex { get; set; }

        public Action<int, StorageType> OnBeginDragHandler { get; set; }
        public Action<int, StorageType> OnEndDragHandler { get; set; }
        public Action<int, StorageType> OnDropHandler { get; set; }
        public Action<int, StorageType> OnPointerEnterHandler { get; set; }
        public Action<int, StorageType> OnPointerExitHandler { get; set; }
        public Action<int, StorageType> OnDoubleClickButtonHandler { get; set; }

        public bool IsDragAndDropOn { get; set; }
        public bool IsInteractable { get; private set; }

        #endregion


        #region UnityMethods


        private void OnDestroy()
        {
            if (_currentDraggedObject != null)
            {
                Destroy(_currentDraggedObject);
            }
        }

        #endregion


        #region Methods

        protected abstract void FillSlot(EntityType entityModel);
        protected abstract void ClearSlot();

        public virtual void Initialize(StorageType storageType, int storageSlotIndex)
        {
            _storageType = storageType;
            _storageSlotIndex = storageSlotIndex;
            IsInteractable = true;
            IsDragAndDropOn = true;
            _changeableImage.enabled = false;
        }

        public void UpdateSlot(EntityType entityModel)
        {
            if (entityModel != null)
            {
                FillSlot(entityModel);
                _changeableImage.enabled = true;
            }
            else
            {
                _changeableImage.enabled = false;
                ClearSlot();
            }
        }

        public virtual void SetInteractable(bool flag)
        {
            IsInteractable = flag;
        }

        public virtual void RemoveAllListeners()
        {
            OnBeginDragHandler = null;
            OnDropHandler = null;
            OnEndDragHandler = null;
            OnPointerEnterHandler = null;
            OnPointerExitHandler = null;
            OnDoubleClickButtonHandler = null;
        }

        protected virtual void OnBeginDragLogic()
        {
            _currentDraggedObject = Instantiate(_objectForDrag, gameObject.transform.root.Find("Canvas"));

            RectTransform draggedObjectRectTransform = _currentDraggedObject.GetComponent<RectTransform>();
            draggedObjectRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            draggedObjectRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            draggedObjectRectTransform.pivot = new Vector2(0.5f, 0.5f);

            Rect objectForDragRect = _objectForDrag.GetComponent<RectTransform>().rect;
            draggedObjectRectTransform.sizeDelta = new Vector2(objectForDragRect.width, objectForDragRect.height);
        }

        protected virtual void OnEndDragLogic()
        {
            Destroy(_currentDraggedObject);
        }

        protected virtual bool IsPointerEnterOn()
        {
            return _changeableImage.sprite != null && IsInteractable;
        }

        protected virtual bool IsPointerExitOn()
        {
            return _changeableImage.sprite != null && IsInteractable;
        }

        protected virtual void OnDropLogic() { }
        protected virtual void OnPointerEnterLogic() { }
        protected virtual void OnPointerExitLogic() { }

        #endregion


        #region IBeginDragHandler

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (IsDragAndDropOn && IsInteractable)
            {
                if (_changeableImage.sprite != null)
                {
                    OnBeginDragLogic();
                    OnBeginDragHandler?.Invoke(_storageSlotIndex, _storageType);
                }
            }
        }

        #endregion


        #region IDragHandler

        public void OnDrag(PointerEventData eventData)
        {
            if (_currentDraggedObject != null)
            {
                _currentDraggedObject.transform.position = Mouse.current.position.ReadValue();
            }
        }

        #endregion


        #region IDrophandler

        public void OnDrop(PointerEventData eventData)
        {
            if (IsDragAndDropOn && IsInteractable)
            {
                OnDropLogic();
                OnDropHandler?.Invoke(_storageSlotIndex, _storageType);
            }
        }

        #endregion


        #region IEndDragHandler

        public void OnEndDrag(PointerEventData eventData)
        {
            //for debug:
            //System.Collections.Generic.List<RaycastResult> results = new System.Collections.Generic.List<RaycastResult>();
            //EventSystem.current.RaycastAll(eventData, results);
            //Debug.Log("Drop on " + results[0].gameObject.name);

            if (_currentDraggedObject != null)
            {
                OnEndDragLogic();
                OnEndDragHandler?.Invoke(_storageSlotIndex, _storageType);
            }
        }

        #endregion


        #region IPointerEnterHandler

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (IsPointerEnterOn())
            {
                OnPointerEnterLogic();
                OnPointerEnterHandler?.Invoke(_storageSlotIndex, _storageType);
            }
        }

        #endregion


        #region IPointerExitHandler

        public void OnPointerExit(PointerEventData eventData)
        {
            if (IsPointerExitOn())
            {
                OnPointerExitLogic();
                OnPointerExitHandler?.Invoke(_storageSlotIndex, _storageType);
            }
        }

        #endregion


        #region IPointerClickHandler

        public void OnPointerClick(PointerEventData eventData)
        {
            if (IsInteractable)
            {
                if (Time.time < _lastClickTime + DOUBLECLICK_TIME)
                {
                    OnDoubleClickButtonHandler?.Invoke(_storageSlotIndex, _storageType);
                }
                _lastClickTime = Time.time;
            }
        }

        #endregion
    }
}
