using System;
using UnityEngine;


namespace BeastHunter
{
    [Serializable]
    public struct TrapStruct
    {
        #region Fields

        [SerializeField] private string _trapName;
        [SerializeField] private GameObject _trapPrefabInHands;
        [SerializeField] private GameObject _trapPrefabProjection;
        [SerializeField] private Damage _trapDamage;
        [SerializeField] private Vector3 _trapPositionEulers;

        [SerializeField] private float _duration;
        [SerializeField] private float _projectionDistanceFromCharacter;
        [SerializeField] private float _placingHeight;
        [SerializeField] private float _totalTimeToPlaceTrap;
        [SerializeField] private float _timeBeforeTrapAppear;
        [SerializeField] private float _timeToDestroyAfterActivation;

        [SerializeField] private int _chargeAmount;

        #endregion


        #region Properties

        public string TrapName => _trapName;
        public GameObject TrapPrefabInHands => _trapPrefabInHands;
        public GameObject TrapPrefabProjection => _trapPrefabProjection;
        public Damage TrapDamage => _trapDamage;
        public Vector3 TrapPositionEulers => _trapPositionEulers;

        public float Duration => _duration;
        public float ProjectionDistanceFromCharacter => _projectionDistanceFromCharacter;
        public float PlacingHeight => _placingHeight;
        public float TotalTimeToPlaceTrap => _totalTimeToPlaceTrap;
        public float TimeBeforeTrapAppear => _timeBeforeTrapAppear;
        public float TImeToDestroyAfterActivation => _timeToDestroyAfterActivation;

        public int ChargeAmount => _chargeAmount;

        #endregion
    }
}

