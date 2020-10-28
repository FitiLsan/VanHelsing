using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BeastHunter
{

    [CreateAssetMenu(fileName = "NewModel", menuName = "CreateData/Butterfly", order = 2)]
    public sealed class ButterflyData : ScriptableObject
    {


        #region Constants


        private const float HOP_FREQUENCY = 0.5f;
        private const float MAX_HOP_FREQUENCY = 0.01f;

        private const float MIN_JUMP_HEIGHT = 160f;
        private const float MAX_JUMP_HEIGHT = 230f;

        private const float MIN_HOP_ANGLE = -10f;
        private const float MAX_HOP_ANGLE =  10f;


        #endregion


        #region Fields

        public ButterflyStruct ButterflyStruct;

        #endregion


        #region Metods

        public void Flee(ButterflyModel butterfly)
        {
            butterfly.TimeLeft -= Time.deltaTime;
            butterfly.TimeElapsed += Time.deltaTime;
            if (butterfly.TimeLeft < 0.0f)
            {
                bool canHop = false;
                if (butterfly.TimeElapsed >= MAX_HOP_FREQUENCY)
                {
                    canHop = true;
                }
                if (canHop)
                { 
                    butterfly.ButterflyRigidbody.AddForce(butterfly.Direction);
                    butterfly.Direction = new Vector3(Random.Range(MIN_HOP_ANGLE, MAX_HOP_ANGLE) * ButterflyStruct.MoveSpeed, Random.Range(MIN_JUMP_HEIGHT, MAX_JUMP_HEIGHT), Random.Range(MIN_HOP_ANGLE, MAX_HOP_ANGLE) * ButterflyStruct.MoveSpeed);
                   
                    butterfly.TimeLeft = HOP_FREQUENCY;
                    butterfly.TimeElapsed = 0.0f;
                }
            }
            if (butterfly.ButterflyTransform.position.y <= butterfly.ButterflyEdge.y || butterfly.ButterflyTransform.position.x < butterfly.ButterflyEdge.x || butterfly.ButterflyTransform.position.z > butterfly.ButterflyEdge.z)
            {
                butterfly.ButterflyRigidbody.velocity = new Vector3(0, 0, 0);
                butterfly.ButterflyTransform.position = butterfly.ButterflyStartPosition;
            }
            if(butterfly.ButterflyTransform.position.y < 0.4f)
            {
                 butterfly.ButterflyRigidbody.AddForce(new Vector3(0, MIN_JUMP_HEIGHT, 0));
            }
        }
        
        #endregion
    }
}