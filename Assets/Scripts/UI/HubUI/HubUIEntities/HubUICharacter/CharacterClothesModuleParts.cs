using System;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunterHubUI
{
    [Serializable]
    public class CharacterClothesModuleParts
    {
        #region Fields

        [SerializeField] ClothesType _type;
        [SerializeField] List<string> _names;

        #endregion


        #region Properties

        public ClothesType Type => _type;
        public List<string> Names => _names;

        #endregion


        #region ClassLifeCycle

        public CharacterClothesModuleParts(ClothesType type, List<string> names)
        {
            _type = type;
            _names = names;
        }

        #endregion
    }
}
