using UnityEngine;


namespace BeastHunterHubUI
{
    [System.Serializable]
    public class WorkRoomProgressLevel<T> where T : BaseWorkRoomProgress
    {
        [SerializeField] private int _level;
        [SerializeField] private T _progress;


        public int Level => _level;
        public T Progress => _progress;
    }
}
