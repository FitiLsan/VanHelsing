using UnityEngine;

namespace Assets.Scripts.EntityScripts.Example.Butterfly
{
    [CreateAssetMenu(fileName = "NewData", menuName = "CreateData/Butterfly", order = 4)]
    public sealed class ButterflyData : ScriptableObject
    {
        #region Fields

        public ButterflyStruct ButterflyStruct;
        private float _angle = 0;

        
     
        #endregion

        #region Metods

        public void Move(Transform transform, float speed, float radius)
        {

            _angle += Time.deltaTime;
            var x = Mathf.Cos(_angle * speed) * radius;
            var z = Mathf.Sin(_angle * speed) * radius;
            transform.position = new Vector3(x, 1f, z) + new Vector3(-3.58f, 1f, 15.43f);

        }



        #endregion

    }
}
