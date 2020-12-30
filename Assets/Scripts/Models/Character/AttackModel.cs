using System;


namespace BeastHunter
{   
    [Serializable]
    public sealed class AttackModel
    {
        #region Fields

        public string Name;
        public string AnimationName;
        public Damage AttackDamage;
        public WeaponItem WeaponItem;
        public HandsEnum AttackType;

        public float AttackTime;
        public float AttackStaminaCost;

        #endregion
    }
}

