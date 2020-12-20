using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


namespace BeastHunter
{
    public class TwoHeadedSnakeModel : EnemyModel
    {

        #region Fields

        private const int HEAD_COLLIDER_COUNT = 2;
        private const int TAIL_COLLIDER_COUNT = 4;

        private TwoHeadedSnakeData _twoHeadedSnakeData;
        private InteractableObjectBehavior[] _interactableObjects;
        private InteractableObjectBehavior _detectionSphereIO;
        private SphereCollider _detectionSphere;
        private TwoHeadedSnakeAttackStateBehaviour[] _attackStates;
        private Collider[] _tailAttackColliders;
        private Collider[] _twinHeadAttackColliders;
        private Image _canvasHPImage;
        private Transform _canvasHPObject;

        public TwoHeadedSnakeData.BehaviourState behaviourState;
        public Transform chasingTarget;

        public float timer;
        public float attackCoolDownTimer;

        public bool isAttacking;

        public float rotatePosition1;
        public float rotatePosition2;

        #endregion


        #region Properties

        public CapsuleCollider CapsuleCollider { get; }
        public Rigidbody Rigidbody { get; }
        public NavMeshAgent NavMeshAgent { get; }

        public TwoHeadedSnakeSettings Settings { get; }
        public GameObject TwoHeadedSnake { get; }
        public Vector3 SpawnPoint;
        public Animator Animator { get; }
        public Transform Transform { get; }
        public InteractableObjectBehavior [] WeaponsIO { get; }
        public Collider[] TailAttackColliders { get => _tailAttackColliders; }
        public Collider[] TwinHeadAttackColliders { get => _twinHeadAttackColliders; }
        public Image CanvasHPImage { get => _canvasHPImage; }
        public Transform CanvesHPObject { get => _canvasHPObject; }
        
        #endregion


        #region ClassLifeCycle

        public TwoHeadedSnakeModel(GameObject prefab, TwoHeadedSnakeData twoHeadedSnakeData, Vector3 spawnPosition)
        {
            
            _twoHeadedSnakeData = twoHeadedSnakeData;
            Settings = _twoHeadedSnakeData.settings;
            TwoHeadedSnake = prefab;
            SpawnPoint = spawnPosition;
            attackCoolDownTimer = 0;
            
            Transform = TwoHeadedSnake.transform;
            behaviourState = TwoHeadedSnakeData.BehaviourState.None;

            _canvasHPObject = prefab.transform.Find("CanvasObject");
            _canvasHPObject.position = _canvasHPObject.position + Settings.PositionHpBar;
            _canvasHPImage = _canvasHPObject.GetComponentInChildren<Image>();

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

            CurrentHealth = _twoHeadedSnakeData.BaseStats.MainStats.MaxHealth;
            IsDead = false;

        }

        #endregion


        #region NpcModel

        public override void OnAwake()
        {
            
        }

        public override void Execute()
        {

            if (!IsDead)
            {
                _twoHeadedSnakeData.Act(this);
                CanvesHPObject.LookAt(Services.SharedInstance.CameraService.CurrentActiveCamera.Value.transform);
                CanvasHPImage.fillAmount = CurrentHealth / _twoHeadedSnakeData.BaseStats.MainStats.MaxHealth;
            }
            else
            {
                //CanvasHPImage.fillAmount = 0;
                CanvesHPObject.gameObject.SetActive(false);
            }

        }

        public override EnemyStats GetStats()
        {
            return _twoHeadedSnakeData.BaseStats;
        }

        public override void OnTearDown()
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

        private bool Filter(Collider collider) => _twoHeadedSnakeData.Filter(collider);
        private void OnDetectionEnemy(ITrigger trigger, Collider collider) => _twoHeadedSnakeData.OnDetectionEnemy(collider, this);
        private void OnLostEnemy(ITrigger trigger, Collider collider) => _twoHeadedSnakeData.OnLostEnemy(collider, this);
        private void OnHitEnemy(ITrigger trigger, Collider collider) => _twoHeadedSnakeData.OnHitEnemy(collider, this);
        private void OnAttackStateEnter() => _twoHeadedSnakeData.OnAttackStateEnter(this);
        private void OnAttackStateExit() => _twoHeadedSnakeData.OnAttackStateExit(this);
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
            _twinHeadAttackColliders = new Collider[HEAD_COLLIDER_COUNT];
            _tailAttackColliders = new Collider[TAIL_COLLIDER_COUNT];
            
            int headCountIndex = 0;
            int tailCountIndex = 0;

            for (int i = 0; i < weaponBehaviors.Length; i++)
            {

                if (weaponBehaviors[i].name == "Bone14" || weaponBehaviors[i].name == "Bone14(mirrored)")
                {
                   
                    _twinHeadAttackColliders[headCountIndex] = weaponBehaviors[i].GetComponent<BoxCollider>();
                    _twinHeadAttackColliders[headCountIndex].enabled = false;
                    headCountIndex++;

                }
                else 
                {
                   
                    _tailAttackColliders[tailCountIndex] = weaponBehaviors[i].GetComponent<BoxCollider>();
                    _tailAttackColliders[tailCountIndex].enabled = false;
                    tailCountIndex++;

                }

            }

         
        }
        #endregion

    }
}
