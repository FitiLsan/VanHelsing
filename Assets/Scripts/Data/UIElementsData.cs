using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "UIElementsData")]
    public sealed class UIElementsData : ScriptableObject
    {
        #region Fields

        [SerializeField] private GameObject _timeSkipPrefab;
        [SerializeField] private GameObject _weaponWheelPrefab;
        [SerializeField] private GameObject _buttonsInformationPrefab;

        #endregion


        #region Properties

        public GameObject TimeSkipPrefab => _timeSkipPrefab;
        public GameObject WeaponWheelPrefab => _weaponWheelPrefab;
        public GameObject ButtonsInformationPrefab => _buttonsInformationPrefab;

        #endregion
    }
}

