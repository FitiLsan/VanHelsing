using UnityEngine;
using UnityEngine.UI;

namespace BeastHunter
{
    public class WeaponWheelView : MonoBehaviour
    {
        #region Fields

        private GameObject _weaponWheelUI;
        private Transform _weaponWheelTransform;
        private Text _weaponWheelText;
        private WeaponCircle[] _weaponWheelItems;

        #endregion


        #region Property

        public GameObject WeaponWheelUI => _weaponWheelUI;
        public Transform WeaponWheelTransform => _weaponWheelTransform;
        public Text WeaponWheelText => _weaponWheelText;
        public WeaponCircle[] WeaponWheelItems => _weaponWheelItems;

        #endregion


        #region Methods

        public void Init(string wheelPanelName, string wheelCyrcleName, GameObject weaponUI)
        {
            _weaponWheelUI = weaponUI;
            _weaponWheelTransform = WeaponWheelUI.transform.
                Find(wheelPanelName).Find(wheelCyrcleName).transform;
            _weaponWheelItems = WeaponWheelUI.transform.Find(wheelPanelName).
                GetComponentsInChildren<WeaponCircle>();
            _weaponWheelText = WeaponWheelUI.transform.GetComponentInChildren<Text>();
        }

        #endregion
    }
}
