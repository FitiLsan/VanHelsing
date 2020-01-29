using System.Collections.Generic;
using Character;

namespace Models
{
    /// <summary>
    ///     Базовый класс для "живых" сущностей мира.
    ///     Так же обрати внимание на уже имеющийся: <see cref="Models.ConditionsAndActions.Helpers.BaseCharacterModel"/>
    /// </summary>
    public class BaseCharacterModel_ver2
    {
        /// <summary>
        /// Текущие значения статов (базовые с учетом прокаки и предметов)
        /// </summary>
        public Dictionary<Stats, int> StatsList;

        /// <summary>
        /// Базовые значения статов
        /// </summary>
        public Dictionary<Stats, int> BaseStatsList;

        /// <summary>
        /// Уровень персонажа
        /// </summary>
        /// <value></value>
        public int Level
        {
            get => _level;
            set
            {
                if(value < 1)
                    _level = 1;

                else
                    _level = value;
            }
        }
        private int _level;

        /// <summary>
        /// Очки здоровья
        /// </summary>
        /// <value></value>
        public int HealthPoints 
        { 
            get => _hp;
            set
            {
                if(value < 0)
                    _hp = 0;
                else if(value > MaxHealthPoints)
                        _hp = MaxHealthPoints;
                else _hp = value;
            }
        }
        private int _hp;

        /// <summary>
        /// Очки выносливости
        /// </summary>
        /// <value></value>
        public int StaminaPoints
        { 
            get => _sp;
            set
            {
                if(value < 0)
                    _sp = 0;
                else if(value > MaxStaminaPoints)
                        _sp = MaxStaminaPoints;
                else _sp = value;
            }
        }
        private int _sp;

        /// <summary>
        /// Очки маны
        /// </summary>
        /// <value></value>
        public int ManaPoints
        {
            get => _mp;
            set
            {
                if(value < 0)
                    _mp = 0;
                else if(value > MaxManaPoints)
                        _mp = MaxManaPoints;
                else _mp = value;
            }
        }
        private int _mp;

        /// <summary>
        /// Очки боевого духа
        /// </summary>
        /// <value></value>
        public int MoralPoints
        {
            get => _mop;
            set
            {
                if(value < 0)
                    _mop = 0;
                else if(value > MaxMoralPoints)
                        _mop = MaxMoralPoints;
                else _mop = value;
            }
        }
        private int _mop;

        /// <summary>
        /// Макс. очков здоровья
        /// </summary>
        /// <value></value>
        public int MaxHealthPoints { get => StatsList[Stats.Health]; }

        /// <summary>
        /// Макс. очков выносливости
        /// </summary>
        /// <value></value>
        public int MaxStaminaPoints { get => StatsList[Stats.Stamina]; }

        /// <summary>
        /// Макс. очков маны
        /// </summary>
        /// <value></value>
        public int MaxManaPoints { get => StatsList[Stats.Mana]; }

        /// <summary>
        /// Макс. очков боевого духа
        /// </summary>
        /// <value></value>
        public int MaxMoralPoints { get => StatsList[Stats.Morale]; }

        public BaseCharacterModel_ver2(CharacterBaseStats allStats)
        {
            Level = allStats.Level;

            StatsList = new Dictionary<Stats, int>();
            BaseStatsList = new Dictionary<Stats, int>();

            foreach(var container in allStats.StatsList)
            {
                BaseStatsList.Add(container.Name, container.Value);
                StatsList.Add(container.Name, container.Value);
            }

            HealthPoints = MaxHealthPoints;
            StaminaPoints = MaxStaminaPoints;
            ManaPoints = MaxManaPoints;
            MoralPoints = MaxMoralPoints;
        }
    }

}

