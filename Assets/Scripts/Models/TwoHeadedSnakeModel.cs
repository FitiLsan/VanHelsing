using System;
using UnityEngine;
using UnityEngine.AI;


namespace BeastHunter
{
    public class TwoHeadedSnakeModel : EnemyModel
    {

        #region Fields

        private TwoHeadedSnakeData _twoHeadedSnakeData;

        #endregion


        #region Properties

        public CapsuleCollider THSnakeCapsuleCollider { get; }
        public SphereCollider THSnakeSphereCollider { get; }
        public Rigidbody THSnakeRigitbody { get; }
        public NavMeshAgent THSnakeNavAgent { get; }
        #endregion


        #region ClassLifeCycle

        public TwoHeadedSnakeModel(GameObject prefab, TwoHeadedSnakeData twoHeadedSnakeData, Vector3 groundPosition)
        {

            if (prefab.GetComponent<Rigidbody>() != null)
            {
                THSnakeRigitbody = prefab.GetComponent<Rigidbody>();
            }
            else
            {
                THSnakeRigitbody = prefab.AddComponent<Rigidbody>();
                THSnakeRigitbody.freezeRotation = true;
                THSnakeRigitbody.mass = 1f;
                THSnakeRigitbody.drag = 0.0f;
                THSnakeRigitbody.angularDrag = 0.0f;
            }

            THSnakeRigitbody.isKinematic = true;

            if (prefab.GetComponent<CapsuleCollider>() != null)
            {
                THSnakeCapsuleCollider = prefab.GetComponent<CapsuleCollider>();
            }
            else
            {
                THSnakeCapsuleCollider = prefab.AddComponent<CapsuleCollider>();
                THSnakeCapsuleCollider.center = new Vector3(0f,0.75f,0.2f);
                THSnakeCapsuleCollider.radius = 0.2f;
                THSnakeCapsuleCollider.height = 1.5f;
            }

            THSnakeCapsuleCollider.transform.position = groundPosition;

            if (prefab.GetComponent<SphereCollider>() != null)
            {
                THSnakeSphereCollider = prefab.GetComponent<SphereCollider>();
                THSnakeSphereCollider.isTrigger = true;
            }
            else
            {
                THSnakeSphereCollider = prefab.AddComponent<SphereCollider>();
                THSnakeSphereCollider.center = new Vector3(0f, 0.75f, 0.0f);
                THSnakeSphereCollider.radius = 7f;
                THSnakeSphereCollider.isTrigger = true;
            }

            if (prefab.GetComponent<NavMeshAgent>() != null)
            {
                THSnakeNavAgent = prefab.GetComponent<NavMeshAgent>();
            }
            else
            {
                THSnakeNavAgent = prefab.AddComponent<NavMeshAgent>();
            }

            THSnakeNavAgent.agentTypeID = 1;


        }

        #endregion


        #region NpcModel

        public override void OnAwake()
        {
            
        }

        public override void Execute()
        {
            
        }

        public override EnemyStats GetStats()
        {
            return _twoHeadedSnakeData.BaseStats;
        }

        public override void OnTearDown()
        {
            
        }

        public override void DoSmth(string how)
        {
            
        }

        public override void TakeDamage(Damage damage)
        {
            if (!IsDead)
            {
                _twoHeadedSnakeData.TakeDamage(this, damage);
            }
        }

        #endregion

    }
}
