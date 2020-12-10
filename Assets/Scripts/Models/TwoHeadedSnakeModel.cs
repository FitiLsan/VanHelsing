using System;
using UnityEngine;
using UnityEngine.AI;


namespace BeastHunter
{
    public class TwoHeadedSnakeModel : EnemyModel
    {

        #region Fields

        private TwoHeadedSnakeData _twoHeadedSnakeData;
        private InteractableObjectBehavior[] _interactableObjects;
        private InteractableObjectBehavior _detectionSphereIO;
        private SphereCollider _detectionSphere;

        public TwoHeadedSnakeData.BehaviourState behaviourState;
        public Transform ChasingTarget;
        public float Timer;

        public float RotatePosition1;
        public float RotatePosition2;

        #endregion


        #region Properties

        public CapsuleCollider CapsuleCollider { get; }
        public Rigidbody Rigitbody { get; }
        public NavMeshAgent NavMeshAgent { get; }

        public TwoHeadedSnakeSettings Settings { get; }
        public GameObject TwoHeadedSnake { get; }
        public Vector3 SpawnPoint;
        public Animator Animator { get; }
        public Transform Transform { get; }
        #endregion


        #region ClassLifeCycle

        public TwoHeadedSnakeModel(GameObject prefab, TwoHeadedSnakeData twoHeadedSnakeData, Vector3 spawnPosition)
        {
            _twoHeadedSnakeData = twoHeadedSnakeData;
            Settings = _twoHeadedSnakeData.settings;
            TwoHeadedSnake = prefab;
            SpawnPoint = spawnPosition;

            Transform = TwoHeadedSnake.transform;
            behaviourState = TwoHeadedSnakeData.BehaviourState.None;

            if (TwoHeadedSnake.GetComponent<Rigidbody>() != null)
            {
                Rigitbody = TwoHeadedSnake.GetComponent<Rigidbody>();
            }
            else
            {
                Rigitbody = TwoHeadedSnake.AddComponent<Rigidbody>();
                Rigitbody.freezeRotation = true;
                Rigitbody.mass = Settings.RigitbodyMass;
                Rigitbody.drag = Settings.RigitbodyDrag;
                Rigitbody.angularDrag = Settings.RigitbodyAngularDrag;
            }

            Rigitbody.isKinematic = Settings.IsRigitbodyKinematic;

            if (TwoHeadedSnake.GetComponent<CapsuleCollider>() != null)
            {
                CapsuleCollider = TwoHeadedSnake.GetComponent<CapsuleCollider>();
            }
            else
            {
                CapsuleCollider = TwoHeadedSnake.AddComponent<CapsuleCollider>();
                CapsuleCollider.center = Settings.CapsuleColliderCenter;
                CapsuleCollider.radius = Settings.CapsuleColliderRadius;
                CapsuleCollider.height = Settings.CapsuleColliderHeight;
            }

            CapsuleCollider.transform.position = SpawnPoint;

            if (TwoHeadedSnake.GetComponent<NavMeshAgent>() != null)
            {
                NavMeshAgent = TwoHeadedSnake.GetComponent<NavMeshAgent>();
            }
            else
            {
                NavMeshAgent = TwoHeadedSnake.AddComponent<NavMeshAgent>();
                
            }

            NavMeshAgent.speed = Settings.MaxRoamingSpeed;
            NavMeshAgent.acceleration = Settings.NavMeshAcceleration;
            NavMeshAgent.agentTypeID = GetAgentTypeIDByIndex(Settings.NavMeshAgentTypeIndex);
            NavMeshAgent.stoppingDistance = Settings.StoppingDistance;
            NavMeshAgent.angularSpeed = Settings.AngularSpeed;
            
            Animator = TwoHeadedSnake.GetComponent<Animator>();

            _interactableObjects = TwoHeadedSnake.GetComponentsInChildren<InteractableObjectBehavior>();
            
            _detectionSphereIO = GetInteractableObject(InteractableObjectType.Sphere);
            _detectionSphereIO.OnFilterHandler = Filter;
            _detectionSphereIO.OnTriggerEnterHandler = OnDetectionEnemy;
            _detectionSphereIO.OnTriggerExitHandler = OnLostEnemy;

            _detectionSphere = _detectionSphereIO.GetComponent<SphereCollider>();
            _detectionSphere.radius = Settings.SphereColliderRadius;
        }

        #endregion


        #region NpcModel

        public override void OnAwake()
        {
            
        }

        public override void Execute()
        {
            _twoHeadedSnakeData.Act(this);
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
            _twoHeadedSnakeData.Do(how);
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

        private bool Filter(Collider collider) => _twoHeadedSnakeData.Filter(collider);
        private void OnDetectionEnemy(ITrigger trigger, Collider collider) => _twoHeadedSnakeData.OnDetectionEnemy(collider, this);
        private void OnLostEnemy(ITrigger trigger, Collider collider) => _twoHeadedSnakeData.OnLostEnemy(collider, this);

        #endregion


        #region Helpers

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

        private InteractableObjectBehavior GetInteractableObject(InteractableObjectType type)
        {
            for (int i = 0; i < _interactableObjects.Length; i++)
            {
                if (_interactableObjects[i].Type == type) return _interactableObjects[i];
            }
            Debug.LogWarning(this + "  not found InteractableObject of type " + type);
            return null;
        }

        #endregion

    }
}
