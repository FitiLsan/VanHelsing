using UnityEngine;
using UnityEngine.UI;


namespace BeastHunterHubUI
{
    class MapShopSlotBehaviour : MapStorageSlotBehaviour
    {
        #region Fields

        [SerializeField] private Image _noReputationImage;

        #endregion


        #region Methods

        public void SetAvailability(bool isAvailability)
        {
            _noReputationImage.enabled = !isAvailability;
        }

        #endregion
    }
}
