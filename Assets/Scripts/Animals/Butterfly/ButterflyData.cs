using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewData", menuName = "CreateData/Butterfly", order = 0)]
    public class ButterflyData : ScriptableObject
    {
        #region Fields

        public float RotateAngle;
        public ButterflyStruct ButterflyStruct;

        #endregion


        #region Methods

        public void Move(Transform transform, float speed)
        {
            RotateAngle = Random.Range(-1, 1);
            transform.Translate(RotateAngle, 0, speed * Time.deltaTime);
        }

        public void Run(Transform transform, GameObject player)
        {
            transform.Translate(0, 0, 0);
        }

        #endregion
    }
}
