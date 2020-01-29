using Character;
using System;

namespace Models
{
    /// <summary>
    /// Инкапсулирует зависимости между дчоерними и родительскими характеристиками.
    /// </summary>
    public static class BaseStatsDependency
    {        
        
        /// <summary>
        /// Выполняет расчет значения дочерней х-ки в зависимости от родительской
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        /// <returns></returns>
        public static int ParentToChildCalculation(StatContainer parent, Stats child)
        {
            int result = -1;

            Stats parentStat = parent.Name;

            if (((int)parentStat & 0x0f) != 0x01) // проверка: не указана ли по ошибке конкретная стата
                throw new ArgumentException($"Х-ка {parentStat} не является группой.");

            switch (parentStat)
            {
                case Stats.Constitution:
                    result = ConstitutionTo(parent.Value, child);
                    break;
                case Stats.Strength:
                    result = StrengthTo(parent.Value, child);
                    break;
                case Stats.Dexterity:
                    result = DexterityTo(parent.Value, child);
                    break;
                case Stats.SelfControl:
                    result = SelfControlTo(parent.Value, child);
                    break;
                case Stats.Intellect:
                    result = IntellectTo(parent.Value, child);
                    break;
                case Stats.Hope:
                    result = HopeTo(parent.Value, child);
                    break;
                default: throw new ArgumentException("Базовая характеристика не обнаружена");
            }

            return result;
        }

        /* 
        *   Далее идет семейство методов для расчета дочерних статов в зависимости
        *       от базовых.
        *
        *   Поскольку встречается много одинаковых зависимостей, можно было бы
        *       написать один метод (в целях оптимизации кода), но так читабельней.
        *
        */

        private static int ConstitutionTo(int parentStatValue, Stats child)
        {
            int result = -1; // -1 просто так

            switch (child)
            {
                case Stats.Health:
                case Stats.Stamina:
                    result = parentStatValue * 10;
                    break;
                case Stats.Recovery:
                case Stats.PhysicalResistance:
                case Stats.AlchemicalResistance:
                    result = parentStatValue;
                    break;
                default: throw new ArgumentException("Дочерняя характеристика не обнаружена");
            }

            return result;
        }

        private static int StrengthTo(int parentStatValue, Stats child)
        {
            int result = -1; // -1 просто так

            switch (child)
            {
                case Stats.AttackPower:
                case Stats.CarryWeight:
                case Stats.Might:
                case Stats.Resilience:
                    result = parentStatValue;
                    break;
                case Stats.CritPower:
                    result = 50 + parentStatValue;
                    break;
                default: throw new ArgumentException("Дочерняя характеристика не обнаружена");
            }

            return result;
        }

        private static int DexterityTo(int parentStatValue, Stats child)
        {
            int result = -1; // -1 просто так

            switch (child)
            {
                case Stats.Haste:
                case Stats.Speed:
                case Stats.Reaction:
                case Stats.CritChance:
                case Stats.Dodge:
                    result = parentStatValue;
                    break;
                default: throw new ArgumentException("Дочерняя характеристика не обнаружена");
            }

            return result;
        }

        private static int SelfControlTo(int parentStatValue, Stats child)
        {
            int result = -1; // -1 просто так

            switch (child)
            {
                case Stats.Morale:
                    result = parentStatValue * 10;
                    break;
                case Stats.Patience:
                case Stats.Courage:
                case Stats.Discipline:
                case Stats.Discretion:
                    result = parentStatValue;
                    break;
                default: throw new ArgumentException("Дочерняя характеристика не обнаружена");
            }

            return result;
        }

        private static int IntellectTo(int parentStatValue, Stats child)
        {
            int result = -1; // -1 просто так

            switch (child)
            {
                case Stats.Memory:
                case Stats.Mastery:
                case Stats.Perception:
                case Stats.Calculation:
                case Stats.SixthSense:
                    result = parentStatValue;
                    break;
                default: throw new ArgumentException("Дочерняя характеристика не обнаружена");
            }

            return result;
        }

        private static int HopeTo(int parentStatValue, Stats child)
        {
            int result = -1; // -1 просто так

            switch (child)
            {
                case Stats.Mana:
                    result = parentStatValue * 10;
                    break;
                case Stats.SpellCritChance:
                case Stats.SpellCritPower:
                case Stats.Luck:
                case Stats.Zeal:
                    result = parentStatValue;
                    break;
                default: throw new ArgumentException("Дочерняя характеристика не обнаружена");
            }

            return result;
        }
    }
}