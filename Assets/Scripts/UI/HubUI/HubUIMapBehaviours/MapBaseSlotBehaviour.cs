using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


namespace BeastHunterHubUI
{
    public abstract class MapBaseSlotBehaviour : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        #region Constants

        private const float DOUBLECLICK_TIME = 0.5f;

        #endregion


        #region Fields

        [SerializeField] protected Image _itemImage;

        private GameObject _draggedObject;
        private float _lastClickTime;
        protected int _slotIndex;
        protected bool _isInteractable;
        protected bool _isDragAndDropOn;

        #endregion


        #region Properties

        public Action<int> OnBeginDragItemHandler { get; set; }
        public Action<int> OnEndDragItemHandler { get; set; }
        public Action<int> OnDroppedItemHandler { get; set; }
        public Action<int> OnPointerEnterHandler { get; set; }
        public Action<int> OnPointerExitHandler { get; set; }
        public Action<int> OnDoubleClickButtonHandler { get; set; }

        #endregion


        #region Methods

        public virtual void FillSlotInfo(int slotIndex, bool isDragAndDropOn)
        {
            _isInteractable = true;
            _itemImage.enabled = false;
            _slotIndex = slotIndex;
            _isDragAndDropOn = isDragAndDropOn;
        }

        public virtual void FillSlot(Sprite sprite)
        {
            if (sprite != null)
            {
                _itemImage.enabled = true;
            }
            else
            {
                _itemImage.enabled = false;
            }
            _itemImage.sprite = sprite;
        }

        public virtual void SetInteractable(bool flag)
        {
            _isInteractable = flag;
        }

        public virtual void RemoveAllListeners()
        {
            OnBeginDragItemHandler = null;
            OnDroppedItemHandler = null;
            OnEndDragItemHandler = null;
            OnPointerEnterHandler = null;
            OnPointerExitHandler = null;
            OnDoubleClickButtonHandler = null;
        }

        #endregion


        #region IPointerClickHandler

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_isInteractable)
            {
                if (Time.time < _lastClickTime + DOUBLECLICK_TIME)
                {
                        OnDoubleClickButtonHandler?.Invoke(_slotIndex);
                }
                _lastClickTime = Time.time;
             }
        }

        #endregion


        #region IBeginDragHandler

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            if (_isInteractable && _isDragAndDropOn)
            {
                if (_itemImage.sprite != null)
                {
                    _draggedObject = Instantiate(_itemImage.gameObject, gameObject.transform.root.Find("Canvas"));

                    RectTransform draggedObjectRectTransform = _draggedObject.GetComponent<RectTransform>();
                    draggedObjectRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                    draggedObjectRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                    draggedObjectRectTransform.pivot = new Vector2(0.5f, 0.5f);

                    Rect itemImageRect = _itemImage.gameObject.GetComponent<RectTransform>().rect;
                    draggedObjectRectTransform.sizeDelta = new Vector2(itemImageRect.width, itemImageRect.height);

                    FillSlot(null);

                    OnBeginDragItemHandler?.Invoke(_slotIndex);
                }
            }
        }

        #endregion


        #region IDragHandler

        public virtual void OnDrag(PointerEventData eventData)
        {
            if (_draggedObject != null)
            {
                _draggedObject.transform.position = Mouse.current.position.ReadValue();
            }
        }

        #endregion


        #region IEndDragHandler

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            if (_draggedObject != null)
            {
                Destroy(_draggedObject);
                OnEndDragItemHandler?.Invoke(_slotIndex);
            }
        }

        #endregion


        #region IDropHandler

        public virtual void OnDrop(PointerEventData eventData)
        {
            if (_isInteractable && _isDragAndDropOn)
            {
                OnDroppedItemHandler?.Invoke(_slotIndex);
            }
        }

        #endregion


        #region IPointerEnterHandler

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            if (_itemImage.sprite != null && _isInteractable)
            {
                OnPointerEnterHandler?.Invoke(_slotIndex);
            }
        }

        #endregion


        #region IPointerExitHandler

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            if (_itemImage.sprite != null)
            {
                OnPointerExitHandler?.Invoke(_slotIndex);
            }
        }

        #endregion
    }
}
