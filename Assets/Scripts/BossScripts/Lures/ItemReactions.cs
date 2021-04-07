using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{
    [Serializable]
    public class ItemReactions
    {
        [SerializeField]
        public List<LureTypeEnum> ScaryItems = new List<LureTypeEnum>();
        [SerializeField]
        public List<LureTypeEnum> AttractiveItems = new List<LureTypeEnum>();
        [SerializeField]
        public List<LureSmellTypeEnum> ScarySmell = new List<LureSmellTypeEnum>();
        [SerializeField]
        public List<LureSmellTypeEnum> AttractiveSmell = new List<LureSmellTypeEnum>();
    }
}