using UnityEngine;


namespace BeastHunter
{
    public class WeaponCircle : MonoBehaviour
    {
        #region Fields

        [SerializeField] private WeaponData _weaponData;
        [SerializeField] private TrapData _trapData;
        [SerializeField] private Vector3 _anchorPosition;

        #endregion


        #region Properties

        public WeaponData WeaponData => _weaponData;
        public TrapData TrapData => _trapData;
        public Vector3 AnchorPosition => _anchorPosition;

        public bool IsNotEmpty
        {
            get
            {
                return WeaponData != null || TrapData != null;
            }
        }

        #endregion
    }
}

