using UnityEngine;


namespace BeastHunterHubUI
{
    public class ClothesItemModel : BaseItemModel
    {
        #region Properties

        public ClothesType ClothesType { get; private set; }
        public int PocketsAmount { get; private set; }
        public Material FantasyHeroMaterial { get; private set; }
        public string[] PartsNamesAllGender { get; private set; }
        public string[] PartsNamesMale { get; private set; }
        public string[] PartsNamesFemale { get; private set; }
        public CharacterHeadPartType[] DisabledHeadParts { get; private set; }

        #endregion


        #region ClassLifeCycle

        public ClothesItemModel(ClothesItemSO data) : base(data)
        {
            ClothesType = data.ClothesType;
            PocketsAmount = data.PocketsAmount;
            FantasyHeroMaterial = data.FantasyHeroMaterial;
            PartsNamesAllGender = data.ClothesPartsNamesAllGender;
            PartsNamesMale = data.ClothesPartsNamesMale;
            PartsNamesFemale = data.ClothesPartsNamesFemale;
            DisabledHeadParts = data.DisabledHeadParts;
        }

        #endregion
    }
}
