using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{
    public sealed class IOModel : EnemyModel
    {
        #region Fields

        public Color CurrentColor;

        #endregion


        #region Properties

        public IOData IOData { get; }
        public GameObject IO { get; }
        public Transform IOTransform { get; }

        #endregion


        #region ClassLifeCycle

        public IOModel(GameObject prefab, IOData ioData)
        {
            IOData = ioData;
            IO = prefab;
            IOTransform = prefab.transform;
            CurrentColor = Color.red;
        }

        #endregion


        #region NpcModel

        public override void OnAwake()
        {
            
        }

        public override void Execute()
        {
            if (!IsDead)
            {
                IOData.Act(this);
            }
        }

        public override EnemyStats GetStats()
        {
            return IOData.BaseStats;
        }

        public override void DoSmth(string how)
        {
            IOData.Do(how);
        }

        public override void TakeDamage(Damage damage)
        {
            throw new System.NotImplementedException();
        }

        public override void OnTearDown()
        {

        }

        #endregion
    }
}
