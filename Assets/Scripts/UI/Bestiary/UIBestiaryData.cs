using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewData", menuName = "CreateData/BestiaryData", order = 0)]
    public class UIBestiaryData : ScriptableObject
    {
        #region Fields

        public UIBestiaryStruct UIBestiaryStruct;
        public UIBestiaryModel Model;

        public int PageCount;
        public int PageId = 0;

        #endregion

        #region Methods

        public void Update()
        {
            Model.BestiaryTransform.GetChild(0).GetChild(2).GetComponent<Image>().sprite = UIBestiaryStruct.BossImage[PageId];
            Model.BestiaryTransform.GetChild(0).GetChild(3).GetComponent<Text>().text = UIBestiaryStruct.BossDescription[PageId];
        }

        #endregion
    }
}
