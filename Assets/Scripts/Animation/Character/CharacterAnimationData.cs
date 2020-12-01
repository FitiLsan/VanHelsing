using UnityEngine;


namespace BeastHunter
{
    [System.Serializable]
    public struct CharacterAnimationData
    {
        #region Fields

        [SerializeField] private string _movementAnimationName;
        [SerializeField] private string _strafingAnimationName;
        [SerializeField] private string _slidingForwardAnimationName;
        [SerializeField] private string _longDodgeAnimationName;
        [SerializeField] private string _shortDodgeAnimationName;
        [SerializeField] private string _trapPlacingAnimationName;

        [SerializeField] private string _xAxisAnimatorParameterName;
        [SerializeField] private string _yAxisAnimatorParameterName;
        [SerializeField] private string _movementSpeedAnimatorParameterName;
        [SerializeField] private string _xMouseAxisAnimatorParameterName;
        [SerializeField] private string _crouchLevelAnimatorParameterName;
        [SerializeField] private string _xDodgeAxisAnimatorParameterName;
        [SerializeField] private string _yDodgeAxisAnimatorParameterName;

        [Range(0f, 1f)]
        [SerializeField] private float _aimingLookToWeightIK;
        [Range(0f, 1f)]
        [SerializeField] private float _aimingLookToBodyWeightIK;
        [Range(0f, 1f)]
        [SerializeField] private float _aimingLookToHeadWeightIK;
        [Range(0f, 1f)]
        [SerializeField] private float _aimingLookToEyesWeightIK;
        [Range(0f, 1f)]
        [SerializeField] private float _aimingLookToClampWeightIK;

        #endregion


        #region Properties


        public string MovementAnimationName => _movementAnimationName;
        public string StrafingAnimationName => _strafingAnimationName;
        public string SlidingForwardAnimationName => _slidingForwardAnimationName;
        public string LongDodgeAnimationName => _longDodgeAnimationName;
        public string ShortDodgeAnimationName => _shortDodgeAnimationName;
        public string TrapPlacingAnimationName => _trapPlacingAnimationName;

        public string XAxisAnimatorParameterName => _xAxisAnimatorParameterName;
        public string YAxisAnimatorParameterName => _yAxisAnimatorParameterName;
        public string MovementSpeedAnimatorParameterName => _movementSpeedAnimatorParameterName;
        public string XMouseAxisAnimatorParameterName => _xMouseAxisAnimatorParameterName;
        public string CrouchLevelAnimatorParameterName => _crouchLevelAnimatorParameterName;
        public string XDodgeAxisAnimatorParameterName => _xDodgeAxisAnimatorParameterName;
        public string YDodgeAxisAnimatorParameterName => _yDodgeAxisAnimatorParameterName;

        public float AimingLookToWeightIK => _aimingLookToWeightIK;
        public float AimingLookToBodyWeightIK => _aimingLookToBodyWeightIK;
        public float AimingLookToHeadWeightIK => _aimingLookToHeadWeightIK;
        public float AimingLookToEyesWeightIK => _aimingLookToEyesWeightIK;
        public float AimingLookToClampWeightIK => _aimingLookToClampWeightIK;

        #endregion
    }
}

