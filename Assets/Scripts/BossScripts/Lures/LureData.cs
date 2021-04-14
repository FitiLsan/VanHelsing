using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewLureData", menuName = "Enemy/Lures/CreateNewLure", order = 0)]
    public class LureData : ScriptableObject
    {
        public GameObject ItemPrefab;
        public string Name;
        public string Description;
        public LureTypeEnum LureType;
        [HideInInspector]
        public LureSmellTypeEnum LureSmellType;
        public bool IsEatable;
        public bool CanPickUp;

        private void Awake()
        {
            LureSmellType = CreateSmell(LureType);
        }

        private LureSmellTypeEnum CreateSmell(LureTypeEnum lureType)
        {
            switch(lureType)
            {
                case LureTypeEnum.fungus:
                    return LureSmellTypeEnum.fungal;
                case LureTypeEnum.meat:
                    return LureSmellTypeEnum.meaty;
                case LureTypeEnum.charcoal:
                    return LureSmellTypeEnum.smoky;
                default:
                    return LureSmellTypeEnum.none;
            }
        }
    }
}