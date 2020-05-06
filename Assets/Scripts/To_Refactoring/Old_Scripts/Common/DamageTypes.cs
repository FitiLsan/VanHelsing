using System;

namespace Common
{
    /// <summary>
    /// Множество типов урона
    /// </summary>
    [Flags]
    public enum DamageTypes
    {
        Physical = 0x001,
        Spell = 0x002,
        Fire = 0x004,
        Cold = 0x008,
        Lightning = 0x010,
        Acid = 0x020,
        Poison = 0x040,
        Force = 0x080,
        Thunder = 0x100,
        Psychic = 0x200,
        Necrotic = 0x400,
        Radiant = 0x800,
        Bludgeoning = 0x1000,
        Piercing = 0x2000,
        Slashing = 0x4000
    }
}