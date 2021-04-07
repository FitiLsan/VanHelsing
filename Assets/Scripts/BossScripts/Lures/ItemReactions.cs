using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{
    [Serializable]
    public struct ItemReactions
    {
        [Header("Boss Item Reactions")]
        [SerializeField] private List<LureData> ScaryItems;
        [SerializeField] private List<LureData> AttractiveItems;
        [SerializeField] private List<LureSmellTypeEnum> ScarySmell;
        [SerializeField] private List<LureSmellTypeEnum> AttractiveSmell;
    }
}