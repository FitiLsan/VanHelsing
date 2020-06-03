using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewData", menuName = "CreateData/Butterfly", order = 0)]
    public class ButterflyData : ScriptableObject
    {
        #region Fields


        public float Angle = 0;
        public float Radius = 10f;
        public ButterflyStruct ButterflyStruct;

        #endregion


        #region Methods

        public void Move(Transform transform, float speed)
        {
            Angle += Time.deltaTime;

            var x = Mathf.Cos(Angle * speed) * Radius;
            var y = Mathf.Sin(Angle * speed) * Radius;
            transform.position = new Vector3(y, 0, x);
        }

        public void Run(Transform transform, GameObject player)
        {
            transform.Translate(0, 0, 0);
        }

        #endregion
    }
}
