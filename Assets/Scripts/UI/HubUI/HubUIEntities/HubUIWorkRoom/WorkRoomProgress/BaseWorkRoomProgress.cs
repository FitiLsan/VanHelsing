using UnityEngine;


namespace BeastHunterHubUI
{
    [System.Serializable]
    public class BaseWorkRoomProgress
    {
        [SerializeField] int _assistansAmount;

        public int AssistansAmount => _assistansAmount;
    }
}
