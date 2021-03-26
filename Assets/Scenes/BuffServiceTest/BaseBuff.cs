using UnityEngine;
using System;

namespace BeastHunter
{
    [Serializable]
    public class BaseBuff : ScriptableObject
    {
        public string Name;
        public string Description;
        public BuffType Type;
        public Sprite Sprite;
        public BuffEffect[] Effects;
    }
}