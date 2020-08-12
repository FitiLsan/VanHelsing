using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    [CreateAssetMenu(fileName = "NewCube", menuName = "CreateCube", order = 7)]
    public sealed class CubeData : ScriptableObject
    {
        #region Fields

        public CubeStruct CubeStruct;

        #endregion


        #region Metods

        public void Move(Transform transform, Transform target, float speed)
        {
            transform.position = Vector3.MoveTowards
                (transform.position,
                target.transform.position,
                speed);
        }

        
    }
    #endregion

