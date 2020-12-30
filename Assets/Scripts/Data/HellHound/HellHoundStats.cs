using System;
using UnityEngine;


namespace BeastHunter
{
    [Serializable]
    public struct HellHoundStats
    {
        #region Fields

        [Tooltip("Display debugging messages")]
        [SerializeField] private bool _debugMessages;

        [SerializeField] private Damage _damage;

        [Tooltip("The radius the dog will move away from the spawn point. Default: 50.0")]
        [SerializeField] private float _wanderingRadius;
        [Tooltip("Player detection radius. Default: 30.0")]
        [SerializeField] private float _detectionRadius;
        [Tooltip("How often will the dog walk. Default: 75.0")]
        [SerializeField] private float _roamingChance;
        [Tooltip("Non-attack jump animation speed. Default: 1.2")]
        [SerializeField] private float _jumpingSpeedRate;

        [Header("Attacks")]
        [Tooltip("The speed at which the dog will turn towards the player during attacks. Default: 0.5")]
        [SerializeField] private float _attacksTurnSpeed;
        [Tooltip("The maximum distance from which the dog will try to attack direct. Default: 1.5")]
        [SerializeField] private float _attackDirectDistance;
        [Tooltip("The maximum distance from which the dog will try to attack bottom. Default: 1.25")]
        [SerializeField] private float _attackBottomDistance;
        [Tooltip("The maximum distance from which the dog will conduct an attacking jump. Default: 3.5")]
        [SerializeField] private float _attackJumpMaxDistance;
        [Tooltip("The minimum distance from which the dog will conduct an attacking jump. Default: 2.5")]
        [SerializeField] private float _attackJumpMinDistance;
        [Tooltip("Jumping speed during attack. Default: 10.0")]
        [SerializeField] private float _attackJumpSpeed;
        [Tooltip("Jumping attack cooldown. Default: 2.0")]
        [SerializeField] private float _attackJumpCooldown;

        [Header("Idling")]
        [Tooltip("Default: 5.0")]
        [SerializeField] private float _idlingMinTime;
        [Tooltip("Default: 10.0")]
        [SerializeField] private float _idlingMaxTime;

        [Header("Resting")]
        [Tooltip("The chance is calculated from idling chance. Default: 20.0")]
        [SerializeField] private float _restingChance;
        [Tooltip("Default: 30.0")]
        [SerializeField] private float _restingMinTime;
        [Tooltip("Default: 60.0")]
        [SerializeField] private float _restingMaxTime;

        [Header("Escaping")]
        [Tooltip("Must be greater than a detection radius. Default: 50.0")]
        [SerializeField] private float _escapeDistance;
        [Tooltip("The percentage of health at which the escape state is enabled. Default: 30.0")]
        [SerializeField] private float _percentEscapeHealth;
        [Tooltip("The maximum speed at which the dog runs away. Default: 7.0")]
        [SerializeField] private float _escapingSpeed;

        [Header("Searching")]
        [Tooltip("The time during which the dog searches for the player. Default: 45.0")]
        [SerializeField] private float _searchingTime;
        [Tooltip("Dog movement speed during search. Default: 5.0")]
        [SerializeField] private float _searchingSpeed;

        [Header("BackJumping")]
        [Tooltip("Distance at which the jump will occur. Default: 1.0")]
        [SerializeField] private float _backJumpDistance;
        [Tooltip("The longer the length, the further the point is put, which the jump must reach. Default: 1.5")]
        [SerializeField] private float _backJumpLength;
        [Tooltip("The higher the speed, the further the landing will be, but not further BackJumpDistance. Default: 5.0")]
        [SerializeField] private float _backJumpSpeed;
        [Tooltip("The higher the speed, the faster the animation ends. Default: 2.0")]
        [SerializeField] private float _backJumpAnimationSpeedRate;
        [Range(0, 1), Tooltip("The higher, the more pronounced the jump will be. At zero, the dog will not jump. Default: 0.5")]
        [SerializeField] private float _backJumpAnimationIntensity;

        [Header("BattleCircling")]
        [Tooltip("The distance from the player that the dog will try to keep after back jumping. Default: 3.0")]
        [SerializeField] private float _battleCirclingRadius;
        [Tooltip("The speed at which the dog will move during the stage. Default: 3.0")]
        [SerializeField] private float _battleCirclingSpeed;
        [Tooltip("Default: 1.0")]
        [SerializeField] private float _battleCirclingMinTime;
        [Tooltip("Default: 3.0")]
        [SerializeField] private float _battleCirclingMaxTime;
        [Tooltip("Checking distance from the enemy at which the dog will interrupt the battle circling state. Default: 5.0")]
        [SerializeField] private float _battleCirclingMaxDistance;

        [Header("Chasing")]
        [Tooltip("Turn rate near target. Default: 0.1")]
        [SerializeField] private float _chasingTurnSpeedNearTarget;
        [Tooltip("Distance at which ChasingTurnSpeedNearTarget starts to operate. Default: 3.0")]
        [SerializeField] private float _chasingTurnDistanceNearTarget;

        [Header("ChasingBraking")]
        [Tooltip("Increased braking when approaching a target during chasing state." +
            "The dog will slow down more accurately as it approaches the target," +
            " but this behavior looks calmer. Default: false")]
        [SerializeField] private bool _chasingBraking;
        [Tooltip("Braking speed rate. Default: 0.5")]
        [SerializeField] private float _chasingBrakingSpeedRate;
        [Tooltip("Minimum braking speed. Default: 2.0")]
        [SerializeField] private float _chasingBrakingMinSpeed;
        [Tooltip("Distance at which braking starts. Default: 6.0")]
        [SerializeField] private float _chasingBrakingMaxDistance;
        [Tooltip("Distance at which minimum braking speed (ChasingBrakingMinSpeed) begins to operate. Default: 2.0")]
        [SerializeField] private float _chasingBrakingMinDistance;

        [Header("NavMeshAgent")]
        [Tooltip("Default: 1.0")]
        [SerializeField] private float _maxRoamingSpeed;
        [Tooltip("Default: 10.0")]
        [SerializeField] private float _maxChasingSpeed;
        [Tooltip("Default: 450.0")]
        [SerializeField] private float _angularSpeed;
        [Tooltip("Default: 20.0")]
        [SerializeField] private float _acceleration;
        [Tooltip("Default: 1.0")]
        [SerializeField] private float _stoppingDistance;
        [Tooltip("Default: 0.0")]
        [SerializeField] private float _baseOffsetByY;

        #endregion


        #region Properties

        public Damage Damage => _damage;
        public bool DebugMessages => _debugMessages;
        public float WanderingRadius => _wanderingRadius;
        public float DetectionRadius => _detectionRadius;
        public float RoamingChance => _roamingChance;
        public float JumpingSpeedRate => _jumpingSpeedRate;
        public float AttacksTurnSpeed => _attacksTurnSpeed;
        public float AttackDirectDistance => _attackDirectDistance;
        public float AttackBottomDistance => _attackBottomDistance;
        public float AttackJumpMaxDistance => _attackJumpMaxDistance;
        public float AttackJumpMinDistance => _attackJumpMinDistance;
        public float AttackJumpSpeed => _attackJumpSpeed;
        public float AttackJumpCooldown => _attackJumpCooldown;
        public float IdlingMinTime => _idlingMinTime;
        public float IdlingMaxTime => _idlingMaxTime;
        public float RestingChance => _restingChance;
        public float RestingMinTime => _restingMinTime;
        public float RestingMaxTime => _restingMaxTime;
        public float EscapeDistance => _escapeDistance;
        public float PercentEscapeHealth => _percentEscapeHealth;
        public float EscapingSpeed => _escapingSpeed;
        public float SearchingTime => _searchingTime;
        public float SearchingSpeed => _searchingSpeed;
        public float BackJumpDistance => _backJumpDistance;
        public float BackJumpLength => _backJumpLength;
        public float BackJumpSpeed => _backJumpSpeed;
        public float BackJumpAnimationSpeedRate => _backJumpAnimationSpeedRate;
        public float BackJumpAnimationIntensity => _backJumpAnimationIntensity;
        public float BattleCirclingRadius => _battleCirclingRadius;
        public float BattleCirclingSpeed => _battleCirclingSpeed;
        public float BattleCirclingMinTime => _battleCirclingMinTime;
        public float BattleCirclingMaxTime => _battleCirclingMaxTime;
        public float BattleCirclingMaxDistance => _battleCirclingMaxDistance;
        public float ChasingTurnSpeedNearTarget => _chasingTurnSpeedNearTarget;
        public float ChasingTurnDistanceNearTarget => _chasingTurnDistanceNearTarget;
        public bool ChasingBraking => _chasingBraking;
        public float ChasingBrakingSpeedRate => _chasingBrakingSpeedRate;
        public float ChasingBrakingMinSpeed => _chasingBrakingMinSpeed;
        public float ChasingBrakingMaxDistance => _chasingBrakingMaxDistance;
        public float ChasingBrakingMinDistance => _chasingBrakingMinDistance;
        public float MaxRoamingSpeed => _maxRoamingSpeed;
        public float MaxChasingSpeed => _maxChasingSpeed;
        public float AngularSpeed => _angularSpeed;
        public float Acceleration => _acceleration;
        public float StoppingDistance => _stoppingDistance;
        public float BaseOffsetByY => _baseOffsetByY;

        #endregion
    }
}
