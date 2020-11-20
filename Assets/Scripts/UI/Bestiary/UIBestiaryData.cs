using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewData", menuName = "CreateData/BestiaryData", order = 0)]
    public class UIBestiaryData : ScriptableObject
    {
        #region Fields


        public List<GameObject> BestiaryPage = new List<GameObject>();

        public UIBestiaryStruct UIBestiaryStruct;
        public UIBestiaryModel Model;
        
        
        #endregion
    }
}
