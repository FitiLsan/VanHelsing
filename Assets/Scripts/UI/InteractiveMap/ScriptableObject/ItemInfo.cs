using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewItem", menuName = "CreateModel/ItemInfo", order = 0)]
    public class ItemInfo : ScriptableObject
    {
        public int ItemId;
        public string ItemName;
        public ItemType ItemType;
        public int QuestId;
        public Sprite ItemImage;
    }
}