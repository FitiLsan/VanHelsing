using UnityEngine;


namespace BeastHunter
{
    public class ButterflyModel
    {

        #region Fields

        public BoxCollider ButterflyCollider { get; }
        public Transform ButterflyTransform;
        public ButterflyData ButterflyData;
        public ButterflyStruct ButterflyStruct;
        public GameObject Player;

        #endregion


        #region ClassLifeCycle

        public ButterflyModel(GameObject prefab, ButterflyData butterflydata)
        {
            ButterflyData = butterflydata;
            ButterflyStruct = butterflydata.ButterflyStruct;
            ButterflyTransform = prefab.transform;
            ButterflyCollider = prefab.gameObject.GetComponent<BoxCollider>();
            Player = GameObject.FindGameObjectWithTag("Player");
        }

        #endregion


        #region Methods

        public void Initilize()
        {
            ButterflyData.Move(ButterflyTransform, ButterflyStruct.MoveSpeed);
        }

        #endregion
    }
}
