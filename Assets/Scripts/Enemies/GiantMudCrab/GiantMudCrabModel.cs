using UnityEngine;
using UnityEngine.AI;


public sealed class GiantMudCrabModel
{
    #region Properties

    public SphereCollider GiantMudCrabCollider { get; }
    public Transform GiantMudCrabTransform { get; }
    public GiantMudCrabData GiantMudCrabData;
    public GiantMudCrabStruct GiantMudCrabStruct;
    public NavMeshAgent CrabAgent;
    public GameObject Player;
    public GameObject Crab;
    public Transform CrabMouth;

    #endregion


    #region ClassLifeCycle

    public GiantMudCrabModel(GameObject prefab, GiantMudCrabData giantMudCrabData)
    {
        GiantMudCrabData = giantMudCrabData;
        GiantMudCrabStruct = giantMudCrabData.GiantMudCrabStruct;
        GiantMudCrabTransform = prefab.transform;
        Player = GameObject.FindGameObjectWithTag("Player");
        Crab = prefab;
        CrabMouth = prefab.transform.GetChild(0);
    }

    #endregion


    #region Metods

    public void Execute()
    {
        GiantMudCrabData.Patrol(CrabAgent, GiantMudCrabStruct.SpawnPoint, GiantMudCrabStruct.PatrolDistance, GiantMudCrabStruct.IsPatrol);
        GiantMudCrabData.Attack(GiantMudCrabStruct, CrabAgent, Player, Crab);
    }

    #endregion
}