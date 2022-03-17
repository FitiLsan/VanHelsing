using UnityEngine;
using RootMotion.Dynamics;

public class ColliderController : MonoBehaviour
{
    public Collider _collider;

    public void InitializationCollider()
    {
        //_collider = GetComponent<Collider>();
        _collider = FindObjectOfType<PuppetMasterProp>().GetComponent<PuppetMasterProp>().colliders[1];
        //_collider = GetComponent<PuppetMasterProp>().colliders[1];
    }

    public void GetCollider()
    {
        _collider.enabled = true;
        Debug.Log("Collider On");
    }

    public void DropCollider()
    {
        _collider.enabled = false;
        Debug.Log("Collider Off");
    }
}
