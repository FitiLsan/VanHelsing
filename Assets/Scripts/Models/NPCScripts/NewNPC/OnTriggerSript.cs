using UnityEngine;


[RequireComponent(typeof(Collider))]
public class OnTriggerSript : MonoBehaviour
{
    public NewNPCController _npcController;

    private void OnTriggerEnter(Collider other)
    {
        _npcController.OnTriggerEnter(other);
    }

    private void OnTriggerStay(Collider other)
    {
        _npcController.OnTriggerStay(other);
    }

    private void OnTriggerExit(Collider other)
    {
        _npcController.OnTriggerExit(other);
    }

}
