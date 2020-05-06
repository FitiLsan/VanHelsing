using UnityEngine;
using UnityEngine.AI;


namespace BeastHunter
{
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

        public float CurrentHealth;

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
            CurrentHealth = GiantMudCrabData.GiantMudCrabStruct.CurrentHealth;
        }

        #endregion


        #region Metods

        public void Execute()
        {
            if (!GiantMudCrabStruct.IsDead)
            {
                if (!GiantMudCrabStruct.IsDigIn)
                {
                    GiantMudCrabData.Patrol(CrabAgent, GiantMudCrabStruct.SpawnPoint, GiantMudCrabStruct.PatrolDistance, GiantMudCrabStruct.IsPatrol);
                    GiantMudCrabData.Attack(GiantMudCrabStruct, CrabAgent, Player, Crab);
                }
                GiantMudCrabStruct.IsDigIn = GiantMudCrabData.DigIn(Crab, Player, GiantMudCrabStruct.DiggingDistance, CurrentHealth, GiantMudCrabStruct.MaxHealth, GiantMudCrabStruct.IsDigIn);
            }
        }

        #endregion
    }
}
