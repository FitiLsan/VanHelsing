using UnityEngine;


namespace BeastHunterHubUI
{
    [CreateAssetMenu(fileName = "WorkRoom", menuName = "CreateData/HubUIData/WorkRoom", order = 0)]
    public class WorkRoomSO : ScriptableObject
    {
        [SerializeField] private WorkRoomData _workRoomData;


        public WorkRoomData WorkRoomData => _workRoomData;
    }
}
