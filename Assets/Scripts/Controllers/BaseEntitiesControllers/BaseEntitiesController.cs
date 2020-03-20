using BaseScripts;
using Models;
using UnityEngine;


public class BaseEntitiesController : BaseController
{
    [SerializeField] private BaseEntityScriptableObject _baseEntityScriptableObject;

    #region ITick

    public override void Tick()
    {
        _baseEntityScriptableObject.EntityBehaviour(gameObject.transform);
    }

    #endregion


    #region IAwake

    public override void OnAwake()
    {
        ManagerUpdate.AddTo(this);
    }

    #endregion
}
