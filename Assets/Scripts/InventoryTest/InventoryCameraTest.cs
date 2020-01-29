using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryCameraTest : MonoBehaviour
{
    [SerializeField] private Transform Player;
    [SerializeField] private Transform InventoryLook;
    [SerializeField] private Vector3 offsetPosition;
    [SerializeField] private Space offsetPositionSpace = Space.Self;
    [SerializeField] private bool lookAt = true;
    [SerializeField] private bool inInventory = false;
    [SerializeField] private GameObject InventoryCanvas;
    private void Update()
    {
        if (Input.GetKey("i"))
        {
            inInventory = !inInventory;
        }
        if (inInventory)
        {
            offsetPosition = new Vector3(-0.05f, 1.6f);
            Refresh(InventoryLook);
            InventoryCanvas.SetActive(true);
        }
        else if (!inInventory)
        {
            offsetPosition = new Vector3(-6, 4);
            Refresh(Player);
            InventoryCanvas.SetActive(false);
        }
    }

    public void Refresh(Transform target)
    {
        if (offsetPositionSpace == Space.Self)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.TransformPoint(offsetPosition), 0.2f);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position + offsetPosition,0.2f);
        }       
        if (lookAt)
        {
            transform.LookAt(target);
        }
        else
        {
            transform.rotation = target.rotation;
        }
    }
}
