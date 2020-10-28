using UnityEngine;

namespace Assets.Scripts.EntityScripts.Example.Butterfly
{
    public sealed class ButterflyModel
    {
        #region Properties
        public SphereCollider SphereCollider { get; }
        public Transform ButterflyTransform { get; }
        public ButterflyData ButterflyData;
        public ButterflyStruct ButterflyStruct;

        #endregion

        #region ClassLifeCycle

        public ButterflyModel(GameObject prefab, ButterflyData butterflydata)
        {
            ButterflyData = butterflydata;
            ButterflyStruct = butterflydata.ButterflyStruct;
            ButterflyTransform = prefab.transform;
            SphereCollider = prefab.gameObject.GetComponent<SphereCollider>();
        }

        #endregion

        #region Metods

        public void Initilize()
        {
            ButterflyData.Move(ButterflyTransform,  ButterflyStruct.MoveSpeed, ButterflyStruct.FlightRadius);
            
        }

        #endregion
    }
}
