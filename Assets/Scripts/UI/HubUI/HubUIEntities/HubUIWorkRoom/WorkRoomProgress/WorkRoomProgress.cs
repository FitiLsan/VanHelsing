using UnityEngine;


namespace BeastHunterHubUI
{
    [System.Serializable]
    public class WorkRoomProgress : BaseWorkRoomProgress
    {
        [SerializeField] int _ordersSlots;

        public int OrderSlots => _ordersSlots;
    }
}
