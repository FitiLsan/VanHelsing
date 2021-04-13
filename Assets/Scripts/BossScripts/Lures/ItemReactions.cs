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
        [SerializeField] public List<LureData> FoodItems;
        [SerializeField] public List<LureData> AttractiveItems;
        [SerializeField] public List<LureData> ScaryItems;

        [SerializeField] public List<LureSmellTypeEnum> AttractiveSmell;
        [SerializeField] public List<LureSmellTypeEnum> ScarySmell;

    }
}