using UnityEngine;
using UnityEngine.UI;


namespace BeastHunterHubUI
{
    class MapShopItemSlotBehaviour : MapItemSlotBehaviour
    {
        #region Fields

        [SerializeField] private Image _noSaleImage;

        #endregion


        #region Methods

        public void SetAvailability(bool isAvailability)
        {
            _noSaleImage.enabled = !isAvailability;
        }

        #endregion
    }
}
