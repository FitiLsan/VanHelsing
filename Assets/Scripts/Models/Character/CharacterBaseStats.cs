using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using Models;
using UnityEngine;

namespace Models
{
    /// <summary>
    /// Содержит значение статов персонажа.
    /// </summary>
    [CreateAssetMenu(fileName = "NewStats", menuName = "RPG_Van_Helsing/CharacterBaseStats", order = 0)]
    public class CharacterBaseStats : ScriptableObject
    {
        public List<StatContainer> StatsList = new List<StatContainer>();
        public int Level;
    }

    [Serializable]
    public class StatContainer
    {
        public Stats Name;
        public int Value;

        public StatContainer(Stats name, int value)
        {
            Name = name;
            Value = value;
        }
    }

}
