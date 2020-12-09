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

        [Header("Prefab nav mesh settings")]
        [Tooltip("Default: 2.0")]
        [SerializeField] private float _maxRoamingSpeed = 2.0f;

        [Tooltip("Acceleration between 0 and 20.")]
        [Range(0.0f, 20.0f)]
        [SerializeField] private float _navMeshAcceleration = 10.0f;

        [Tooltip("Agent types index, min: 0, max: agent count")]
        [SerializeField] private int _navMeshAgentTypeIndex = 0;

        [Header("Roaming setting")]
        [Tooltip("The radius the dog will move away from the spawn point. Default: 50.0")]
        [SerializeField] private float _wanderingRadius = 50.0f;

        [Tooltip("How often will the dog walk. Default: 75.0")]
        [SerializeField] private float _roamingChance = 75.0f;

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
        public float NavMeshAcceleration => _navMeshAcceleration;
        public int NavMeshAgentTypeIndex => _navMeshAgentTypeIndex;

        public float RoamingChance => _roamingChance;
        public float WanderingRadius => _wanderingRadius;

        public float IdlingMinTime => _idlingMinTime;
        public float IdlingMaxTime => _idlingMaxTime;

        public float RestingChance => _restingChance;
        public float RestingMinTime => _restingMinTime;
        public float RestingMaxTime => _restingMaxTime;


        public bool DebugMessages => _debugMessages;
        #endregion
    }
}
