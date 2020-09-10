using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

namespace BeastHunter
{
    [RequireComponent(typeof(Image))]
    public class DragAndDropCell : MonoBehaviour, IDropHandler
    {
        public class DropEventDescriptor
        {
            public TriggerType triggerType;
            public DragAndDropCell sourceCell;
            public DragAndDropCell destinationCell;
            public DragAndDropItem item;
            public bool permission;
        }

        public SlotSize SlotSize;
        public BodyParts BodyParts = BodyParts.None;
        public CellType cellType = CellType.Swap;
        public bool unlimitedSource = false;
        public bool IsOnInventory = false;

        private DragAndDropItem myDadItem;

        void OnEnable()
        {
            DragAndDropItem.OnItemDragStartEvent += OnAnyItemDragStart;
            DragAndDropItem.OnItemDragEndEvent += OnAnyItemDragEnd;
            UpdateMyItem();
        }

        void OnDisable()
        {
            DragAndDropItem.OnItemDragStartEvent -= OnAnyItemDragStart;
            DragAndDropItem.OnItemDragEndEvent -= OnAnyItemDragEnd;
            StopAllCoroutines();
        }

        private void OnAnyItemDragStart(DragAndDropItem item)
        {
            UpdateMyItem();
            if (myDadItem != null)
            {
                myDadItem.MakeRaycast(false);
                if (myDadItem == item)
                {
                    switch (cellType)
                    {
                        case CellType.DropOnly:
                            DragAndDropItem.icon.SetActive(false);
                            break;
                    }
                }
            }
        }

        private void OnAnyItemDragEnd(DragAndDropItem item)
        {
            UpdateMyItem();
            if (myDadItem != null)
            {
                myDadItem.MakeRaycast(true);
            }
        }

        public void OnDrop(PointerEventData data)
        {
            if (DragAndDropItem.icon != null)
            {
                DragAndDropItem item = DragAndDropItem.draggedItem;
                DragAndDropCell sourceCell = DragAndDropItem.sourceCell;
                if (DragAndDropItem.icon.activeSelf == true)
                {
                    if ((item != null) && (sourceCell != this))
                    {
                        DropEventDescriptor desc = new DropEventDescriptor();
                        switch (cellType)
                        {
                            case CellType.Swap:
                                UpdateMyItem();
                                switch (sourceCell.cellType)
                                {
                                    case CellType.Swap:
                                        desc.item = item;
                                        desc.sourceCell = sourceCell;
                                        desc.destinationCell = this;
                                        SendRequest(desc);
                                        StartCoroutine(NotifyOnDragEnd(desc));
                                        if (desc.permission == true)
                                        {
                                            if (myDadItem != null)
                                            {
                                                DropEventDescriptor descAutoswap = new DropEventDescriptor();
                                                descAutoswap.item = myDadItem;
                                                descAutoswap.sourceCell = this;
                                                descAutoswap.destinationCell = sourceCell;
                                                SendRequest(descAutoswap);
                                                StartCoroutine(NotifyOnDragEnd(descAutoswap));
                                                if (descAutoswap.permission == true)
                                                {
                                                    SwapItems(sourceCell, this);
                                                }
                                                else
                                                {
                                                    PlaceItem(item, desc.destinationCell);
                                                }
                                            }
                                            else
                                            {
                                                PlaceItem(item, desc.destinationCell);
                                            }
                                        }
                                        break;
                                    default:
                                        desc.item = item;
                                        desc.sourceCell = sourceCell;
                                        desc.destinationCell = this;
                                        SendRequest(desc);
                                        StartCoroutine(NotifyOnDragEnd(desc));
                                        if (desc.permission == true)
                                        {
                                            PlaceItem(item, desc.destinationCell);
                                        }
                                        break;
                                }
                                break;
                            case CellType.DropOnly:
                                desc.item = item;
                                desc.sourceCell = sourceCell;
                                desc.destinationCell = this;
                                SendRequest(desc);
                                StartCoroutine(NotifyOnDragEnd(desc));
                                if (desc.permission == true)
                                {
                                    Destroy(item.gameObject);
                                    DestroyItem();
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
                if (item != null)
                {
                    if (item.GetComponentInParent<DragAndDropCell>() == null)
                    {
                        Destroy(item.gameObject);
                    }
                }
                UpdateMyItem();
                sourceCell.UpdateMyItem();
            }
        }

        private void PlaceItem(DragAndDropItem item, DragAndDropCell destinationCell)
        {
            if (item != null)
            {
                
                DestroyItem();
                myDadItem = null;
                if (item.ItemData.ItemStruct.SlotSize.GetHashCode() <= destinationCell.SlotSize.GetHashCode())
                {
                    DragAndDropCell cell = item.GetComponentInParent<DragAndDropCell>();
                    if (cell != null)
                    {
                        if (cell.unlimitedSource == true)
                        {
                            string itemName = item.name;
                            item = Instantiate(item);
                            item.name = itemName;
                        }
                    }
                    item.transform.SetParent(transform, false);
                    item.transform.localPosition = Vector3.zero;
                    item.MakeRaycast(true);
                    myDadItem = item;
                    UpdateSlotInfo(cell, destinationCell);
                }
                else
                {
                    Debug.Log("SlotSize equal " + destinationCell.SlotSize + " it is too small for that item that size equal " + item.ItemData.ItemStruct.SlotSize);
                }
            }
        }

        private void DestroyItem()
        {
            UpdateMyItem();
            if (myDadItem != null)
            {
                DropEventDescriptor desc = new DropEventDescriptor();
                desc.triggerType = TriggerType.ItemWillBeDestroyed;
                desc.item = myDadItem;
                desc.sourceCell = this;
                desc.destinationCell = this;
                if (myDadItem != null)
                {
                    Destroy(myDadItem.gameObject);
                }
            }
            myDadItem = null;
        }

        private bool SendRequest(DropEventDescriptor desc)
        {
            bool result = false;
            if (desc != null)
            {
                desc.triggerType = TriggerType.DropRequest;
                desc.permission = true;
                result = desc.permission;
            }
            return result;
        }

        private IEnumerator NotifyOnDragEnd(DropEventDescriptor desc)
        {
            while (DragAndDropItem.draggedItem != null)
            {
                yield return new WaitForEndOfFrame();
            }
            desc.triggerType = TriggerType.DropEventEnd;
        }

        public void UpdateMyItem()
        {
            myDadItem = GetComponentInChildren<DragAndDropItem>();
        }

        public DragAndDropItem GetItem()
        {
            return myDadItem;
        }

        public void AddItem(DragAndDropItem newItem, DragAndDropCell destinationCell)
        {
            if (newItem != null)
            {
                PlaceItem(newItem, destinationCell);
                DropEventDescriptor desc = new DropEventDescriptor();
                desc.triggerType = TriggerType.ItemAdded;
                desc.item = newItem;
                desc.sourceCell = this;
                desc.destinationCell = this;
            }
        }

        public void RemoveItem()
        {
            DestroyItem();
        }

        public void SwapItems(DragAndDropCell firstCell, DragAndDropCell secondCell)
        {
            if ((firstCell != null) && (secondCell != null))
            {
                DragAndDropItem firstItem = firstCell.GetItem();
                DragAndDropItem secondItem = secondCell.GetItem();
                if(firstItem.ItemData.ItemStruct.SlotSize.GetHashCode() <= secondCell.SlotSize.GetHashCode()
                    && secondItem.ItemData.ItemStruct.SlotSize.GetHashCode() <= firstCell.SlotSize.GetHashCode())
                {
                    if (firstItem != null)
                    {
                        firstItem.transform.SetParent(secondCell.transform, false);
                        firstItem.transform.localPosition = Vector3.zero;
                        firstItem.MakeRaycast(true);
                    }
                    if (secondItem != null)
                    {
                        secondItem.transform.SetParent(firstCell.transform, false);
                        secondItem.transform.localPosition = Vector3.zero;
                        secondItem.MakeRaycast(true);
                    }
                    firstCell.UpdateMyItem();
                    secondCell.UpdateMyItem();
                    UpdateSlotInfo(firstCell, secondCell);
                }
            }
        }
        public void UpdateSlotInfo(DragAndDropCell firstCell, DragAndDropCell secondCell)
        {
            if(firstCell.IsOnInventory && secondCell.IsOnInventory)
            {
                if(firstCell.BodyParts == secondCell.BodyParts)
                {
                    Services.SharedInstance.InventoryService.UpdatePocketInfo(firstCell.BodyParts);
                }
                else
                {
                    Services.SharedInstance.InventoryService.UpdatePocketInfo(secondCell.BodyParts);
                    Services.SharedInstance.InventoryService.UpdatePocketInfo(firstCell.BodyParts);
                }
            }
            else if (firstCell.IsOnInventory)
            {
                Services.SharedInstance.InventoryService.UpdatePocketInfo(firstCell.BodyParts);
            }
            else if (secondCell.IsOnInventory)
            {
                Services.SharedInstance.InventoryService.UpdatePocketInfo(secondCell.BodyParts);
            }
            else
            {
                Debug.Log("No one cell IsOnInventory");
            }
        }
    }
}

