using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BaseEntitiesInitializeController : BaseEntitiesController
{
    #region IAwake

    public override void OnAwake()
    {
        ManagerUpdate.AddTo(this);
    }

    #endregion
}
