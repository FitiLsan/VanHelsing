using UnityEngine;


namespace BeastHunterHubUI
{
    [System.Serializable]
    public class CharacterData
    {
        public string Name;
        public Sprite Portrait;
        public bool IsFemale;
        public int Rank;
        public Material DefaultMaterial;
        public BaseItemSO[] BackpackItems;
        public ClothesItemSO[] ClothesEquipmentItems;
        public PocketItemSO[] PocketItems;
        public WeaponItemSO[] WeaponEquipmentItems;
        public CharacterClothesModuleParts[] DefaultModuleParts;
        public CharacterHeadPart[] DefaultHeadParts;
        public CharacterSkillLevel[] Skills;
    }
}
