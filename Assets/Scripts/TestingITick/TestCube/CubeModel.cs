using UnityEngine;


public class CubeModel
{


    #region Property

    public GameObject CubeTarget;

    public BoxCollider CubeBox;

    public Transform CubeTransform;

    public float CubeSpeed;

    #endregion


    #region ClassLifeCycle

    public CubeModel (GameObject prefab, CubeScriptableObj cubedata)
    {
        CubeSpeed = cubedata.speed;
        CubeTransform = prefab.transform;
        CubeTarget = cubedata.Target;
        CubeBox = prefab.gameObject.GetComponent<BoxCollider>();
    }

    #endregion


}
