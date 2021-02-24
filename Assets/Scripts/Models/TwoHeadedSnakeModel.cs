using UnityEngine;
using UnityEngine.AI;


namespace BeastHunter
{
    public class TwoHeadedSnakeModel : EnemyModel
    {

        #region Fields

        private int HEAD_COLLIDER_COUNT = 2;
        private int TAIL_COLLIDER_COUNT = 4;

        private InteractableObjectBehavior[] _interactableObjects;
        private InteractableObjectBehavior _detectionSphereIO;
        private SphereCollider _detectionSphere;
        private TwoHeadedSnakeAttackStateBehaviour[] _attackStates;
        public TwoHeadedSnakeData.BehaviourState BehaviourState;
        public Transform ChasingTarget;

        public float timer;
        public float attackCoolDownTimer;
        public float rotatePosition1;
        public float rotatePosition2;

        public bool isAttacking;

        #endregion


        #region Properties

        public CapsuleCollider CapsuleCollider { get; }
        public Rigidbody Rigidbody { get; }
        public NavMeshAgent NavMeshAgent { get; }
        public TwoHeadedSnakeSettings Settings { get; }
        public GameObject TwoHeadedSnake { get; }
        public Animator Animator { get; }
        public Transform Transform { get; }
        public InteractableObjectBehavior [] WeaponsIO { get; }
        public Collider[] TailAttackColliders { get; private set; }
        public Collider[] TwinHeadAttackColliders { get; private set; }
        public Vector3 SpawnPoint { get; private set; }

        #endregion


        #region ClassLifeCycle

        public TwoHeadedSnakeModel(GameObject objectOnScene, TwoHeadedSnakeData data) : 
            base(objectOnScene, data)
        {
            Settings = (ThisEnemyData as TwoHeadedSnakeData).settings;
            TwoHeadedSnake = objectOnScene;
            attackCoolDownTimer = 0;

            Transform = TwoHeadedSnake.transform;
            SpawnPoint = Transform.position;
            BehaviourState = TwoHeadedSnakeData.BehaviourState.None;

            if (TwoHeadedSnake.GetComponent<Rigidbody>() != null)
            {
                Rigidbody = TwoHeadedSnake.GetComponent<Rigidbody>();
            }
            else
            {
                Rigidbody = TwoHeadedSnake.AddComponent<Rigidbody>();
                Rigidbody.freezeRotation = true;
                Rigidbody.mass = Settings.RigitbodyMass;
                Rigidbody.drag = Settings.RigitbodyDrag;
                Rigidbody.angularDrag = Settings.RigitbodyAngularDrag;
            }

            Rigidbody.isKinematic = Settings.IsRigitbodyKinematic;

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

            WeaponsIO = TwoHeadedSnake.GetComponentsInChildren<WeaponHitBoxBehavior>();
          
            for (int i = 0; i < WeaponsIO.Length; i++)
            {
                WeaponsIO[i].OnFilterHandler = Filter;
                WeaponsIO[i].OnTriggerEnterHandler = OnHitEnemy;
            }

            AddAttackColliderCollection(WeaponsIO);

            _attackStates = Animator.GetBehaviours<TwoHeadedSnakeAttackStateBehaviour>();
            for (int i = 0; i < _attackStates.Length; i++)
            {
                _attackStates[i].OnStateEnterHandler += OnAttackStateEnter;
                _attackStates[i].OnStateExitHandler += OnAttackStateExit;
            }
        }

        #endregion


        #region NpcModel

        public override void TakeDamage(Damage damage)
        {
            if (!CurrentStats.BaseStats.IsDead)
            {
                ThisEnemyData.TakeDamage(this, damage);
            }
        }

        #endregion


        #region Private methods

        private bool Filter(Collider collider) => (ThisEnemyData as TwoHeadedSnakeData).Filter(collider);
        private void OnDetectionEnemy(ITrigger trigger, Collider collider) => (ThisEnemyData as TwoHeadedSnakeData).OnDetectionEnemy(collider, this);
        private void OnLostEnemy(ITrigger trigger, Collider collider) => (ThisEnemyData as TwoHeadedSnakeData).OnLostEnemy(collider, this);
        private void OnHitEnemy(ITrigger trigger, Collider collider) => (ThisEnemyData as TwoHeadedSnakeData).OnHitEnemy(collider, this);
        private void OnAttackStateEnter() => (ThisEnemyData as TwoHeadedSnakeData).OnAttackStateEnter(this);
        private void OnAttackStateExit() => (ThisEnemyData as TwoHeadedSnakeData).OnAttackStateExit(this);

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

        private void AddAttackColliderCollection(InteractableObjectBehavior[] weaponBehaviors)
        {
            TwinHeadAttackColliders = new Collider[HEAD_COLLIDER_COUNT];
            TailAttackColliders = new Collider[TAIL_COLLIDER_COUNT];
            
            int headCountIndex = 0;
            int tailCountIndex = 0;

            for (int i = 0; i < weaponBehaviors.Length; i++)
            {
                if (weaponBehaviors[i].name == "Bone14" || weaponBehaviors[i].name == "Bone14(mirrored)")
                {
                   
                    TwinHeadAttackColliders[headCountIndex] = weaponBehaviors[i].GetComponent<BoxCollider>();
                    TwinHeadAttackColliders[headCountIndex].enabled = false;
                    headCountIndex++;

                }
                else 
                {
                   
                    TailAttackColliders[tailCountIndex] = weaponBehaviors[i].GetComponent<BoxCollider>();
                    TailAttackColliders[tailCountIndex].enabled = false;
                    tailCountIndex++;

                }
            }      
        }

        #endregion
    }
}
