using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewNpc", menuName = "CreateModel/NpcInfo", order = 0)]
    public class NpcInfo :ScriptableObject
    {
        public int NpcId;
        public string NpcName;
        public Sprite NpcImage;
    }
}