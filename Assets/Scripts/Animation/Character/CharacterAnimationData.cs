using UnityEngine;


namespace BeastHunter
{
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

        [SerializeField] private string _attackAnimationPrefix;

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

        public string AttackAnimationPrefix => _attackAnimationPrefix;

        #endregion

        //private const string MOVEMENT_ANIMATION_NAME = "Movement";
        //private const string STRAFE_ANIMATION_NAME = "Strafe";
        //private const string SLIDE_FORWARD_ANIMATION_NAME = "SlideForward";
        //private const string JUMP_FORWARD_ANIMATION_NAME = "JumpForward";
        //private const string DODGE_ANIMATION_NAME = "Dodge";
        //private const string TRAP_PLACING_ANIMATION_NAME = "TrapPlacing";

        //private const string AXIS_X_ANIMATOR_PARAMETER_NAME = "AxisX";
        //private const string AXIS_Y_ANIMATOR_PARAMETER_NAME = "AxisY";
        //private const string MOVE_SPEED_ANIMATOR_PARAMETER_NAME = "MoveSpeed";
        //private const string MOUSE_AXIS_X_ANIMATOR_PARAMETER_NAME = "MouseAxisX";
        //private const string CROUCH_LEVEL_ANIMATOR_PARAMETER_NAME = "CrouchLevel";
        //private const string DODGE_AXIS_X_ANIMATOR_PARAMETER_NAME = "DodgeAxisX";
        //private const string DODGE_AXIS_Y_ANIMATOR_PARAMETER_NAME = "DodgeAxisY";

        //private const string NOT_ARMED_ATTACK_ANIMATION_NAME_PREFIX = "NotArmedAttack_";
        //private const string ARMED_ATTACK_ANIMATION_NAME_PREFIX = "Attack_";

    }
}

