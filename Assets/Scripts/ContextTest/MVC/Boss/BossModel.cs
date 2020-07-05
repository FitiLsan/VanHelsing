using UnityEngine;
using UnityEngine.AI;

namespace BeastHunter
{
    public sealed class BossModel
    {
        #region PrivateData

        private enum BossBehaviourState
        {
            None = 0,
            WalkingAround = 1,
            GoToObjectOfAtraction = 2,
            Waiting = 3
        }

        #endregion


        #region Fields

        private BossBehaviourState _bossState;

        public Vector3 ObjectOfAtraction;

        public float TimeToWait;

        public float MovingTime;
        public float TimeLeft;

        public float CalmSpeed;
        public float ExcitementSpeed;
        public float StopDistance;

        public float RotateAngle;

        public BossData BossData;

        public NavMeshAgent Agent;

        #endregion


        #region Properties

        public Rigidbody BossRigidbody { get; }
        public Transform BossTranform { get; set; }

        #endregion


        #region ClassLifeCycle

        public BossModel(GameObject prefab, BossData bossData)
        {
            BossData = bossData;
            BossTranform = prefab.transform;
            BossRigidbody = prefab.GetComponent<Rigidbody>();
            _bossState = BossBehaviourState.WalkingAround;
        }

        #endregion


        #region Methods

        public void Execute()
        {
            TimeLeft = MovingTime;
            if (_bossState != BossBehaviourState.GoToObjectOfAtraction)
            {
                _bossState = BossBehaviourState.WalkingAround;
            }
            switch (_bossState)
            {
                case BossBehaviourState.WalkingAround:
                    {
                        WalkAround();
                        break;
                    }
                case BossBehaviourState.GoToObjectOfAtraction:
                    {
                        break;
                    }
            }
        }

        public void ChangeDirection()
        {
            RotateAngle = Random.Range(0, 360);
            BossTranform.Rotate(0, RotateAngle, 0);
        }

        public void WalkAround()
        {
            TimeLeft -= Time.deltaTime;
            if (TimeLeft <= 0)
            {
                ChangeDirection();
                TimeLeft = MovingTime;
            }
            else
            {
                BossTranform.Translate(0, 0, CalmSpeed * Time.deltaTime);
            }
        }



        #endregion
    }
}
