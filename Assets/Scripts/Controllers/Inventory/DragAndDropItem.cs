using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;


namespace BeastHunter
{
    public class DragAndDropItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public static bool dragDisabled = false;
        private Image _image;
        public BaseItem ItemData;

        public static DragAndDropItem draggedItem;
        public static GameObject icon;
        public static DragAndDropCell sourceCell;
        private static GameObject Discription;

        public delegate void DragEvent(DragAndDropItem item);
        public static event DragEvent OnItemDragStartEvent;
        public static event DragEvent OnItemDragEndEvent;

        private static Canvas canvas;
        private static string canvasName = "DragAndDropCanvas";
        private static int canvasSortOrder = 100;

        float clicked = 0;
        float clicktime = 0;
        float clickdelay = 0.5f;

        void Awake()
        {
            if (canvas == null)
            {
                GameObject canvasObj = new GameObject(canvasName);
                canvas = canvasObj.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.sortingOrder = canvasSortOrder;
            }
            if(ItemData != null)
            {
                LoadIcon();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            ShowDicription();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Object.Destroy(Discription);
        }

        public void ShowDicription()
        {
            Discription = Instantiate(Resources.Load("Inventory/Discription"), canvas.transform) as GameObject;
            Discription.transform.localPosition = new Vector3(-620, -280);
            Text DiscriptionText = Discription.GetComponent<Text>();
            DiscriptionText.text = ItemData.ItemStruct.Discription;
        }

        public void OnPointerDown(PointerEventData data)
        {
            clicked++;
            if (clicked == 1) clicktime = Time.time;

            if (clicked > 1 && Time.time - clicktime < clickdelay)
            {
                clicked = 0;
                clicktime = 0;
                if(ItemData.ItemStruct.ItemType == ItemType.Cloth)
                {
                    ItemData = Services.SharedInstance.InventoryService.SetCloth(ItemData as ClothItem);
                    if(ItemData != null)
                    {
                        LoadIcon();
                        Object.Destroy(Discription);
                        ShowDicription();
                        ResetConditions();
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                }

            }
            else if (clicked > 2 || Time.time - clicktime > 1) clicked = 0;

        }


        public void LoadIcon()
        {
            if (ItemData.ItemStruct.Icon != null)
            {
                _image = GetComponent<Image>();
                _image.sprite = ItemData.ItemStruct.Icon;
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (dragDisabled == false)
            {
                sourceCell = GetCell();
                draggedItem = this;

                icon = new GameObject();
                icon.transform.SetParent(canvas.transform);
                icon.name = "Icon";
                Image myImage = GetComponent<Image>();
                myImage.raycastTarget = false;
                Image iconImage = icon.AddComponent<Image>();
                iconImage.raycastTarget = false;
                iconImage.sprite = myImage.sprite;
                RectTransform iconRect = icon.GetComponent<RectTransform>();
                RectTransform myRect = GetComponent<RectTransform>();
                iconRect.pivot = new Vector2(0.5f, 0.5f);
                iconRect.anchorMin = new Vector2(0.5f, 0.5f);
                iconRect.anchorMax = new Vector2(0.5f, 0.5f);
                iconRect.sizeDelta = new Vector2(myRect.rect.width, myRect.rect.height);

                if (OnItemDragStartEvent != null)
                {
                    OnItemDragStartEvent(this);
                }
            }
        }

        public void OnDrag(PointerEventData data)
        {
            if (icon != null)
            {
                icon.transform.position = Input.mousePosition;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            ResetConditions();
        }

        private void ResetConditions()
        {
            if (icon != null)
            {
                Destroy(icon);
            }
            if (OnItemDragEndEvent != null)
            {
                OnItemDragEndEvent(this);
            }
            draggedItem = null;
            icon = null;
            sourceCell = null;
        }

        public void MakeRaycast(bool condition)
        {
            Image image = GetComponent<Image>();
            if (image != null)
            {
                image.raycastTarget = condition;
            }
        }

        public DragAndDropCell GetCell()
        {
            return GetComponentInParent<DragAndDropCell>();
        }

        void OnDisable()
        {
            ResetConditions();
        }
    }
}

