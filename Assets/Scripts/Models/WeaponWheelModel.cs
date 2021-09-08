namespace BeastHunter
{
    public class WeaponWheelModel
    {
        #region Constants

        private const float WEAPON_WHEEL_MAX_CYCLE_DISTANCE = 15f;
        private const float WEAPON_WHEEL_DISTANCE_TO_SET_WEAPON = 7.5f;
        private const float WEAPON_WHEEL_PARENT_IMAGE_NON_DEDICATED_ALFA = 0.3f;
        private const float WEAPON_WHEEL_CHILD_IMAGE_NON_DEDICATED_ALFA = 0.4f;
        private const float WEAPON_WHEEL_PARENT_IMAGE_DEDICATED_ALFA = 0.6f;
        private const float WEAPON_WHEEL_CHILD_IMAGE_DEDICATED_ALFA = 0.7f;
        private const float WEAPON_WHEEL_PARENT_IMAGE_NON_DEDICATED_SCALE = 0.7f;
        private const float WEAPON_WHEEL_CHILD_IMAGE_NON_DEDICATED_SCALE = 0.75f;
        private const float WEAPON_WHEEL_IMAGE_DEDICATED_SCALE = 0.85f;

        private const string WEAPON_WHEEL_PANEL_NAME = "Panel";
        private const string WEAPON_WHEEL_CYCLE_NAME = "Cycle";

        #endregion


        #region Fields

        private WeaponCircle _closestWeaponOnWheel;
        private float _weaponWheelDistance;
        private bool _isWeaponWheelOpen;

        #endregion


        #region Properties

        public float WeaponWheelMaxCycleDistance => WEAPON_WHEEL_MAX_CYCLE_DISTANCE;
        public float WeaponWheelDistanceToSetWeapon => WEAPON_WHEEL_DISTANCE_TO_SET_WEAPON;
        public float WeaponWheelParentImageNonDedicatedAlfa => WEAPON_WHEEL_PARENT_IMAGE_NON_DEDICATED_ALFA;
        public float WeaponWheelChildImageNonDedicatedAlfa => WEAPON_WHEEL_CHILD_IMAGE_NON_DEDICATED_ALFA;
        public float WeaponWheelParentImageDedicatedAlfa => WEAPON_WHEEL_PARENT_IMAGE_DEDICATED_ALFA;
        public float WeaponWheelChildImageDedicatedAlfa => WEAPON_WHEEL_CHILD_IMAGE_DEDICATED_ALFA;
        public float WeaponWheelParentImageNonDedicatedScale => WEAPON_WHEEL_PARENT_IMAGE_NON_DEDICATED_SCALE;
        public float WeaponWheelChildImageNonDedicatedScale => WEAPON_WHEEL_CHILD_IMAGE_NON_DEDICATED_SCALE;
        public float WeaponWheelImageDedicatedScale => WEAPON_WHEEL_IMAGE_DEDICATED_SCALE;

        public string WeaponWheelPanelName => WEAPON_WHEEL_PANEL_NAME;
        public string WeaponWheelCycleName => WEAPON_WHEEL_CYCLE_NAME;

        public WeaponCircle ClosestWeaponOnWheel => _closestWeaponOnWheel;
        public float WeaponWheelDistance => _weaponWheelDistance;
        public bool IsWeaponWheelOpen => _isWeaponWheelOpen;

        #endregion


        #region Methods

        public void AssignClosestWeaponOnWheel(WeaponCircle value)
        {
            _closestWeaponOnWheel = value;
        }

        public void AssightIsWeaponWheelOpen(bool state)
        {
            _isWeaponWheelOpen = state;
        }

        #endregion
    }
}
