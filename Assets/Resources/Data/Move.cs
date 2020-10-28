using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public Transform Center;
    public float angle = 0;

    public float speed = 1;
    public float radius = 1f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime);
        angle += Time.deltaTime;
        var x = Mathf.Cos(angle * speed) * radius;
        var z = Mathf.Sin(angle * speed) * radius;
        transform.position = new Vector3(x, Center.position.y, z) + Center.position;

        //transform.RotateAround(Center.position, Vector3.up, 10 * Time.deltaTime );
    }
}
