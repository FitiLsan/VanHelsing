using UnityEngine;


namespace BeastHunterHubUI
{
    [System.Serializable]
    public class WorkRoomData
    {
        [SerializeField] private BaseWorkRoomData<WorkRoomProgress> _baseWorkRoomData;
        [SerializeField] private ItemOrderData[] _orders;


        public BaseWorkRoomData<WorkRoomProgress> BaseWorkRoomData => _baseWorkRoomData;
        public ItemOrderData[] Orders => (ItemOrderData[])_orders?.Clone();
    }
}
