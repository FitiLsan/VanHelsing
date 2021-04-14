using System;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{
    [Serializable]
    public class HubMapUICharacterClothesModuleParts
    {
        [SerializeField] HubMapUIClothesType _type;
        [SerializeField] List<string> _names;


        public HubMapUIClothesType Type => _type;
        public List<string> Names => _names;


        public HubMapUICharacterClothesModuleParts(HubMapUIClothesType type, List<string> names)
        {
            _type = type;
            _names = names;
        }
    }
}
