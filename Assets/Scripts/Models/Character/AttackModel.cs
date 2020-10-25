using System;


namespace BeastHunter
{   
    [Serializable]
    public class AttackModel
    {
        #region Fields

        public string Name;
        public Damage AttackDamage;
        public WeaponItem WeaponItem;
        public HandsEnum AttackType;

        public float AttackTime;
        public float AttackStaminaCost;

        #endregion
    }
}

