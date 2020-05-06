namespace Character
{
    /// <summary>
    /// Все характеристики персонажа и существ
    /// первая цифра отвечает за группу, вторая за конкретный стат
    /// </summary>
    public enum Stats
    {
        Constitution = 0x01,
        Health = 0x02,
        Stamina = 0x03,
        Recovery = 0x04,
        PhysicalResistance = 0x05,
        AlchemicalResistance = 0x06,
        Strength = 0x11,
        AttackPower = 0x12,
        CarryWeight = 0x13,
        CritPower = 0x14,
        Might = 0x15,
        Resilience = 0x16,
        Dexterity = 0x21,
        Haste = 0x22,
        Speed = 0x23,
        Reaction = 0x24,
        CritChance = 0x25,
        Dodge = 0x26,
        SelfControl = 0x31,
        Morale = 0x32,
        Patience = 0x33,
        Courage = 0x34,
        Discipline = 0x35,
        Discretion = 0x36,
        Intellect = 0x41,
        Memory = 0x42,
        Mastery = 0x43,
        Perception = 0x44,
        Calculation = 0x45,
        SixthSense = 0x46,
        Hope = 0x51,
        Mana = 0x52,
        SpellCritChance = 0x53,
        SpellCritPower = 0x54,
        Luck = 0x55,
        Zeal = 0x56
    }
}