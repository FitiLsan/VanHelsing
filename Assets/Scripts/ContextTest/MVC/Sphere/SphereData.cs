

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "NewModel", menuName = "CreateModel/Sphere", order = 0)]
public sealed class SphereData : ScriptableObject
{

    #region Fields
  
  
    public SphereStruct SphereStruct;
   
    [SerializeField] private float _speed;
  

 

   





    #endregion




    #region Metods


    public void Move(Transform transform, Transform target, float speed)
    {


     
      
        transform.Translate(Vector3.right  * speed * Time.deltaTime);





        if (transform.position.x >= 10)
        {

            transform.eulerAngles = new Vector3(0, 5, 0);
        }
        else
        if (transform.position.x <= 15)
        {

            transform.eulerAngles = new Vector3(0, -45, 0);
        }

        if (transform.position.x >= 30)
        {

            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        if (transform.position.x <= 25)
        {

            transform.eulerAngles = new Vector3(0, -95, 0);
        }
        if (transform.position.x <= -4.5)
        {

            transform.eulerAngles = new Vector3(0, -180, 0);
        }
        if (transform.position.x <= -45)
        {

            transform.eulerAngles = new Vector3(0, 45, 0);
        }
        if (transform.position.z <= -5)
        {

            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        if (transform.position.x <= -60)
        {

            transform.eulerAngles = new Vector3(0, -95, 0);
        }
        if (transform.position.z >= 70)
        {

            transform.eulerAngles = new Vector3(0, 30, 0);
        }

    }


    public void ChangeSphereCollider(SphereCollider SphereCollider, float SphereRadius)
    {
   
      
        if (SphereCollider != null)
        {
            SphereCollider.radius = SphereRadius;
        }       

    }

    #endregion
}
