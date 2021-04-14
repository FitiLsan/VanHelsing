using UnityEngine;
using UnityEngine.UI;


namespace BeastHunter
{
    class HubMapUIShopSlotBehaviour : HubMapUIStorageSlotBehaviour
    {
        [SerializeField] private Image _noReputationImage;


        public void SetAvailability(bool isAvailability)
        {
            _noReputationImage.enabled = !isAvailability;
        }
    }
}
