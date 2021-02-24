using System;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{
    [Serializable]
    public class TwoHeadedSnakeSettings
    {
        #region Private fields

        [Header("Scene information fields. Default: 0, 0, 0")]
        [Tooltip("Vector 3 prefab position on the scene, ")]
        [SerializeField]
        private Vector3 _instantiatePosition = Vector3.zero;

        [Header("Prefab rigitbody information fields")]
        [Tooltip("Snake rigitbody mass between 0 and 10.")]
        [Range(0.0f, 10.0f)]
        [SerializeField] private float _rigitbodyMass = 1.0f;

        [Tooltip("Snake rigitbody drag between 0 and 10.")]
        [Range(0.0f, 10.0f)]
        [SerializeField] private float _rigitbodyDrag = 0.0f;

        [Tooltip("Snake rigitbody angular drag between 0 and 10.")]
        [Range(0.0f, 10.0f)]
        [SerializeField] private float _rigitbodyAngularDrag = 0.5f;

        [Tooltip("Is snake rigitbody kinematic")]
        [SerializeField] private bool _isRigitbodyKinematic = true;

        [Header("Prefab capsule collider information fields")]

        [Tooltip("Capsule collider center position")]
        [SerializeField] private Vector3 _capsuleColliderCenter = new Vector3(0.0f, 0.75f,0.2f);

        [Tooltip("Capsule collider radius between 0 and 2.")]
        [Range(0.0f, 2.0f)]
        [SerializeField] private float _capsuleColliderRadius = 0.2f;

        [Tooltip("Capsule collider height between 0 and 5.")]
        [Range(0.0f, 5.0f)]
        [SerializeField] private float _capsuleColliderHeight = 1.5f;

        [Header("Prefab sphere trigger information fields")]

        [Tooltip("Sphere trigger radius between 0 and 20.")]
        [Range(0.0f, 20.0f)]
        [SerializeField] private float _sphereColliderRadius = 10.0f;

        [Header("NavMeshAgent settings")]
        [Tooltip("Default: 2.0")]
        [SerializeField] private float _maxRoamingSpeed = 2.0f;
        [Tooltip("Default: 50.0")]
        [SerializeField] private float _maxChasingSpeed = 50.0f;
        [Tooltip("Default: 1.0")]
        [SerializeField] private float _stoppingDistance = 1.0f ;
        [Tooltip("Default: 450.0")]
        [SerializeField] private float _angularSpeed = 450.0f;

        [Tooltip("Acceleration between 0 and 20.")]
        [Range(0.0f, 20.0f)]
        [SerializeField] private float _navMeshAcceleration = 10.0f;

        [Tooltip("Agent types index, min: 0, max: agent count")]
        [SerializeField] private int _navMeshAgentTypeIndex = 0;

        [Header("Damage")]
        [SerializeField] private float _physicalDamage;
        [SerializeField] private float _stunProbability;

        [Header("Attacks")]
        [Tooltip("The speed at which the snake will turn towards the player during attacks. Default: 0.01")]
        [SerializeField]  private float _attacksTurnSpeed = 0.01f;
        [Tooltip("The maximum distance from which the snake will try to attack direct. Default: 3")]
        [SerializeField] private float _tailAttackDistance = 5.0f;
        [Tooltip("The maximum distance from which the snake will try to attack direct. Default: 3")]
        [SerializeField] private float _twinHeadAttackDistance = 4.0f;
        [Tooltip("Default: 2")]
        [SerializeField] private float _attackCooldown = 2.0f;
        
        [Header("Roaming setting")]
        [Tooltip("The radius the snake will move away from the spawn point. Default: 50.0")]
        [SerializeField] private float _wanderingRadius = 50.0f;

        [Tooltip("How often will the snake walk. Default: 75.0")]
        [SerializeField] private float _roamingChance = 75.0f;

        [Header("Chasing")]
        [Tooltip("Turn rate near target. Default: 0.05")]
        [SerializeField] private float _chasingTurnSpeedNearTarget = 0.05f;
        [Tooltip("Distance at which ChasingTurnSpeedNearTarget starts to operate. Default: 8.0")]
        [SerializeField] private float _chasingTurnDistanceNearTarget = 8.0f;

        [Header("Escape setting")]
        [Tooltip("Default: 3")]
        [SerializeField] private float _escapingSpeed = 3.0f;
        [Tooltip("between 5 and 50. Default: 30")]
        [Range(5.0f, 50.0f)]
        [SerializeField] private float _percentEscapeHealth = 30.0f;
        [Tooltip("Default: 50")]
        [SerializeField] private float _escapeDistance = 50.0f;

        [Header("Idling setting")]
        [Tooltip("Default: 5.0")]
        [SerializeField] private float _idlingMinTime = 5.0f;
        [Tooltip("Default: 10.0")]
        [SerializeField] private float _idlingMaxTime = 10.0f;

        [Header("Resting")]
        [Tooltip("The chance is calculated from idling chance. Default: 20.0")]
        [SerializeField] private float _restingChance = 20.0f;
        [Tooltip("Default: 30.0")]
        [SerializeField] private float _restingMinTime = 30.0f;
        [Tooltip("Default: 60.0")]
        [SerializeField] private float _restingMaxTime = 60.0f;

        [Header("Debug message")]
        [Tooltip("Display debugging messages")]
        [SerializeField] private bool _debugMessages = false;

        [Header("HP Bar")]
        [Tooltip("Position HP bar in a prefab")]
        [SerializeField] private Vector3 _positionHpBar = new Vector3(0.0f, 3.2f, 0.0f);
        [Tooltip("between 0.2 and 1. Default: 1")]
        [Range(0.2f, 1.0f)]
        [SerializeField] private float _damegedHealthFadeTimerMax = 1f;
        [Tooltip("Speed fade amount. Between 1 and 10. Default: 5")]
        [Range(1f, 10.0f)]
        [SerializeField] private float _fadeAmount = 5f;
        [Tooltip("between 0.2 and 1.5. Default: 1")]
        [Range(0.2f, 1.5f)]
        private float _damagedTxtFameTimer = 1f;
        [Tooltip("Speed damage text fade amount. Between 1 and 10. Default: 5")]
        [Range(1f, 10.0f)]
        [SerializeField] private float _txtFadeAmount = 5f;
        [Tooltip("HP Bar hide timer. Between 1 and 2. Default: 2")]
        [Range(1f, 2.0f)]
        [SerializeField] private float _hpBarHideTimer = 2f;

        #endregion


        #region Properties

        public Vector3 InstantiatePosition => _instantiatePosition;

        public float RigitbodyMass => _rigitbodyMass;
        public float RigitbodyDrag => _rigitbodyDrag;
        public float RigitbodyAngularDrag => _rigitbodyAngularDrag;
        public bool IsRigitbodyKinematic => _isRigitbodyKinematic;

        public Vector3 CapsuleColliderCenter => _capsuleColliderCenter;
        public float CapsuleColliderRadius => _capsuleColliderRadius;
        public float CapsuleColliderHeight => _capsuleColliderHeight;

        public float SphereColliderRadius => _sphereColliderRadius;

        public float MaxRoamingSpeed => _maxRoamingSpeed;
        public float MaxChasingSpeed => _maxChasingSpeed;
        public float StoppingDistance => _stoppingDistance;
        public float AngularSpeed => _angularSpeed;
        public float NavMeshAcceleration => _navMeshAcceleration;
        public int NavMeshAgentTypeIndex => _navMeshAgentTypeIndex;

        public float RoamingChance => _roamingChance;
        public float WanderingRadius => _wanderingRadius;

        public float IdlingMinTime => _idlingMinTime;
        public float IdlingMaxTime => _idlingMaxTime;

        public float RestingChance => _restingChance;
        public float RestingMinTime => _restingMinTime;
        public float RestingMaxTime => _restingMaxTime;

        public float EscapingSpeed =>_escapingSpeed;
        public float PercentEscapeHealth => _percentEscapeHealth;
        public float EscapeDistance => _escapeDistance;

        public float PhysicalDamage => _physicalDamage;
        public float StunProbability => _stunProbability;

        public float AttacksTurnSpeed => _attacksTurnSpeed;
        public float TailAttackDistance => _tailAttackDistance;
        public float TwinHeadAttackDistance => _twinHeadAttackDistance;
        public float AttackCooldown => _attackCooldown; 
        public float ChasingTurnSpeedNearTarget => _chasingTurnSpeedNearTarget;
        public float ChasingTurnDistanceNearTarget => _chasingTurnDistanceNearTarget;

        public Vector3 PositionHpBar => _positionHpBar;
        public float DamegedHealthFadeTimerMax => _damegedHealthFadeTimerMax;
        public float FadeAmount => _fadeAmount;
        public float DamagedTxtFameTimer => _damagedTxtFameTimer;
        public float TxtFadeAmount => _txtFadeAmount;
        public float HpBarHideTimer => _hpBarHideTimer;
        public bool DebugMessages => _debugMessages;

        #endregion
    }
}
