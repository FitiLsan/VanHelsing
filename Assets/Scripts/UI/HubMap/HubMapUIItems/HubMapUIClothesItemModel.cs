using UnityEngine;


namespace BeastHunter
{
    public class HubMapUIClothesItemModel : HubMapUIBaseItemModel
    {
        #region Properties

        public HubMapUIClothesType ClothesType { get; private set; }
        public int PocketsAmount { get; private set; }
        public Material FantasyHeroMaterial { get; private set; }
        public string[] PartsNamesAllGender { get; private set; }
        public string[] PartsNamesMale { get; private set; }
        public string[] PartsNamesFemale { get; private set; }
        public HubMapUICharacterHeadParts[] DisabledHeadParts { get; private set; }

        #endregion


        public HubMapUIClothesItemModel(HubMapUIClothesItemData data) : base(data)
        {
            ClothesType = data.ClothesType;
            PocketsAmount = data.PocketsAmount;
            FantasyHeroMaterial = data.FantasyHeroMaterial;
            PartsNamesAllGender = data.ClothesPartsNamesAllGender;
            PartsNamesMale = data.ClothesPartsNamesMale;
            PartsNamesFemale = data.ClothesPartsNamesFemale;
            DisabledHeadParts = data.DisabledHeadParts;
        }
    }
}
