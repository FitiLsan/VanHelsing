using BaseScripts;
using UnityEngine;


public class CubeController : BaseController
{

    public CubeModel Model { get; private set; }

    public CubeView View { get; private set; }

    #region ClassLifeCycle

    public CubeController(CubeScriptableObj cubedata)
    {
        GameObject instance = GameObject.Instantiate(cubedata.prefab);
        View = new CubeView(); //UI
        Model = new CubeModel(instance, cubedata);
    }

    #endregion


    #region ITick

    public override void Tick()
    {
        Move();
        ChangeBox();
    }

    #endregion


    #region Metods

    public override void OnAwake()
    {
        ManagerUpdate.AddTo(this);
    }

    void Move()
    {
        Model.CubeTransform.position =Vector3.MoveTowards
            (Model.CubeTransform.position,
            Model.CubeTarget.transform.position,
            Model.CubeSpeed);
    }

    void ChangeBox()
    {
        Model.CubeBox.size = new Vector3(0.5f, 0.5f, 0.5f);
    }

    #endregion


}
