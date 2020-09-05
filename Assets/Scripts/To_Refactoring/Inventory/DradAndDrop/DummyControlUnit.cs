using UnityEngine;
using UnityEngine.UI;
using System.Collections;


namespace BeastHunter
{
    public class DummyControlUnit : MonoBehaviour
    {
        public void AddItemInFreeCell(DragAndDropItem item)
        {
            foreach (DragAndDropCell cell in GetComponentsInChildren<DragAndDropCell>())
            {
                if (cell != null)
                {
                    if (cell.GetItem() == null)
                    {
                        cell.AddItem(Instantiate(item.gameObject).GetComponent<DragAndDropItem>(), cell);
                        break;
                    }
                }
            }
        }

        public void RemoveFirstItem()
        {
            foreach (DragAndDropCell cell in GetComponentsInChildren<DragAndDropCell>())
            {
                if (cell != null)
                {
                    if (cell.GetItem() != null)
                    {
                        cell.RemoveItem();
                        break;
                    }
                }
            }
        }
    }
}

