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

        public CapsuleCollider TwoHeadedSnakeCapsuleCollider { get; }
        public SphereCollider TwoHeadedSnakeSphereCollider { get; }
        public Rigidbody TwoHeadedSnakeRigitbody { get; }
        public NavMeshAgent TwoHeadedSnakeNavAgent { get; }

        public TwoHeadedSnakeData TwoHeadedSnakeData { get; }
        public TwoHeadedSnakeSettings TwoHeadedSnakeSettings { get; }
        #endregion


        #region ClassLifeCycle

        public TwoHeadedSnakeModel(GameObject prefab, TwoHeadedSnakeData twoHeadedSnakeData, Vector3 spawnPosition)
        {
            TwoHeadedSnakeData = twoHeadedSnakeData;
            TwoHeadedSnakeSettings = TwoHeadedSnakeData.twoHeadedSnakeSettings;

            if (prefab.GetComponent<Rigidbody>() != null)
            {
                TwoHeadedSnakeRigitbody = prefab.GetComponent<Rigidbody>();
            }
            else
            {
                TwoHeadedSnakeRigitbody = prefab.AddComponent<Rigidbody>();
                TwoHeadedSnakeRigitbody.freezeRotation = true;
                TwoHeadedSnakeRigitbody.mass = TwoHeadedSnakeSettings.RigitbodyMass;
                TwoHeadedSnakeRigitbody.drag = TwoHeadedSnakeSettings.RigitbodyDrag;
                TwoHeadedSnakeRigitbody.angularDrag = TwoHeadedSnakeSettings.RigitbodyAngularDrag;
            }

            TwoHeadedSnakeRigitbody.isKinematic = TwoHeadedSnakeSettings.IsRigitbodyKinematic;

            if (prefab.GetComponent<CapsuleCollider>() != null)
            {
                TwoHeadedSnakeCapsuleCollider = prefab.GetComponent<CapsuleCollider>();
            }
            else
            {
                TwoHeadedSnakeCapsuleCollider = prefab.AddComponent<CapsuleCollider>();
                TwoHeadedSnakeCapsuleCollider.center = TwoHeadedSnakeSettings.CapsuleColliderCenter;
                TwoHeadedSnakeCapsuleCollider.radius = TwoHeadedSnakeSettings.CapsuleColliderRadius;
                TwoHeadedSnakeCapsuleCollider.height = TwoHeadedSnakeSettings.CapsuleColliderHeight;
            }

            TwoHeadedSnakeCapsuleCollider.transform.position = spawnPosition;

            if (prefab.GetComponent<SphereCollider>() != null)
            {
                TwoHeadedSnakeSphereCollider = prefab.GetComponent<SphereCollider>();
                TwoHeadedSnakeSphereCollider.isTrigger = true;
            }
            else
            {
                TwoHeadedSnakeSphereCollider = prefab.AddComponent<SphereCollider>();
                TwoHeadedSnakeSphereCollider.center = TwoHeadedSnakeSettings.SphereColliderCenter;
                TwoHeadedSnakeSphereCollider.radius = TwoHeadedSnakeSettings.SphereColliderRadius;
                TwoHeadedSnakeSphereCollider.isTrigger = true;
            }

            if (prefab.GetComponent<NavMeshAgent>() != null)
            {
                TwoHeadedSnakeNavAgent = prefab.GetComponent<NavMeshAgent>();
            }
            else
            {
                TwoHeadedSnakeNavAgent = prefab.AddComponent<NavMeshAgent>();
                
            }

            TwoHeadedSnakeNavAgent.acceleration = TwoHeadedSnakeSettings.NavMeshAcceleration;

            TwoHeadedSnakeNavAgent.agentTypeID = GetAgentTypeIDByIndex(TwoHeadedSnakeSettings.NavMeshAgentTypeIndex);

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

        #region Private methods

        private int GetAgentTypeIDByIndex(int agentIndex)
        {
            int agentTypeCount = NavMesh.GetSettingsCount();
            int agentTypeID = NavMesh.GetSettingsByIndex(agentIndex).agentTypeID;

            if (agentIndex > agentTypeCount - 1)
            {
                Debug.Log($"Nav Mesh Agent Type Index #{agentIndex} not exist," +
                          $" max index = #{agentTypeCount - 1}. Nav Mesh Agent Type Index" +
                          $" changed to #0 ");

                agentIndex = 0;
                agentTypeID = NavMesh.GetSettingsByIndex(agentIndex).agentTypeID;

                Debug.Log($"Agent type name \"{NavMesh.GetSettingsNameFromID(agentTypeID)}\"" +
                          $" - index #{agentIndex}");
                return agentTypeID;
            }

            agentTypeID = NavMesh.GetSettingsByIndex(agentIndex).agentTypeID;
            Debug.Log($"Agent type name \"{NavMesh.GetSettingsNameFromID(agentTypeID)}\"" +
                      $" - index #{agentIndex}");
            return agentTypeID;
        }

        #endregion

    }
}
