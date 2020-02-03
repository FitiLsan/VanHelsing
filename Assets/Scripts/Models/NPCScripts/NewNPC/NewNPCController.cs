using UnityEngine;
using Models;


public class NewNPCController : BaseScripts.BaseController
{
    #region PrivateData

    public NewNPCModel _npcModel;

    #endregion


    #region Methods

    public override void OnAwake()
    {
        if (!IsActive || _npcModel.IsDead) return;
        base.OnAwake();

        _npcModel.OnAwake();
    }

    public override void Tick()
    {
        if (!IsActive || _npcModel.IsDead) return;
        base.Tick();

        _npcModel.Tick();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!IsActive || _npcModel.IsDead) return;
        _npcModel.OnTriggerEnter(other);
    }

    public void OnTriggerStay(Collider other)
    {
        if (!IsActive || _npcModel.IsDead) return;

        _npcModel.OnTriggerStay(other);
    }

    public void OnTriggerExit(Collider other)
    {
        if (!IsActive || _npcModel.IsDead) return;

        _npcModel.OnTriggerExit(other);
    }

    public void Init(NewNPCModel model, GameObject prefab)
    {
        if (!IsActive) return;

        _npcModel = model;
        _npcModel.Init(prefab);
    }

    #endregion
}







