using UnityEngine;

public class AddForcer : MonoBehaviour
{
    [SerializeField] private float _speed;

    void Start()
    {
        var rigidbody = GetComponent<Rigidbody>();
        rigidbody.AddForce(transform.forward * _speed, ForceMode.Impulse);
    }
}
