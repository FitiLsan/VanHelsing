using System;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{
    [Serializable]
    public struct TwoHeadedSnakeSettings
    {
        #region Private fields

        [Header("Scene information fields. Default: 0, 0, 0")]
        [Tooltip("Vector 3 prefab position on the scene, ")]
        [SerializeField]
        private Vector3 _instantiatePosition;

        [Header("Prefab rigitbody information fields")]
        [Tooltip("Snake rigitbody mass between 0 and 10.")]
        [Range(0.0f, 10.0f)]
        [SerializeField] private float _rigitbodyMass;

        [Tooltip("Snake rigitbody drag between 0 and 10.")]
        [Range(0.0f, 10.0f)]
        [SerializeField] private float _rigitbodyDrag;

        [Tooltip("Snake rigitbody angular drag between 0 and 10.")]
        [Range(0.0f, 10.0f)]
        [SerializeField] private float _rigitbodyAngularDrag;

        [Tooltip("Is snake rigitbody kinematic")]
        [SerializeField] private bool _isRigitbodyKinematic;

        [Header("Prefab capsule collider information fields")]

        [Tooltip("Capsule collider center position")]
        [SerializeField] private Vector3 _capsuleColliderCenter;

        [Tooltip("Capsule collider radius between 0 and 2.")]
        [Range(0.0f, 2.0f)]
        [SerializeField] private float _capsuleColliderRadius;

        [Tooltip("Capsule collider height between 0 and 5.")]
        [Range(0.0f, 5.0f)]
        [SerializeField] private float _capsuleColliderHeight;

        [Header("Prefab sphere trigger information fields")]

        [Tooltip("Sphere trigger center position")]
        [SerializeField] private Vector3 _sphereColliderCenter;

        [Tooltip("Sphere trigger radius between 0 and 20.")]
        [Range(0.0f, 20.0f)]
        [SerializeField] private float _sphereColliderRadius;

        [Tooltip("Sphere trigger radius increace in battle between 1 and 2.")]
        [Range(1f, 2f)]
        [SerializeField] private float _sphereColliderRadiusIncrease;

        [Tooltip("Sphere trigger radius decreace when player sneaking between 1 and 2.")]
        [Range(1f, 2f)]
        [SerializeField] private float _sphereColliderRadiusDecrease;

        [Header("Prefab nav mesh settings")]

        [Tooltip("Acceleration between 0 and 20.")]
        [Range(0.0f, 20.0f)]
        [SerializeField] private float _navMeshAcceleration;

        [Tooltip("Agent types index, min: 0, max: agent count")]
        [SerializeField] private int _navMeshAgentTypeIndex;
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

        public Vector3 SphereColliderCenter => _sphereColliderCenter;
        public float SphereColliderRadius => _sphereColliderRadius;
        public float SphereColliderRadiusIncrease => _sphereColliderRadiusIncrease;
        public float SphereColliderRadiusDecreace => _sphereColliderRadiusDecrease;

        public float NavMeshAcceleration => _navMeshAcceleration;
        public int NavMeshAgentTypeIndex => _navMeshAgentTypeIndex;
        #endregion
    }
}
