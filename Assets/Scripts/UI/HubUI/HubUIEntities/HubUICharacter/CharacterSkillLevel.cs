using UnityEngine;


namespace BeastHunterHubUI
{
    [System.Serializable]
    public class CharacterSkillLevel
    {
        #region Fields

        [SerializeField] SkillType _skillType;
        [SerializeField] int _skillLevel;

        #endregion


        #region Properties

        public SkillType SkillType => _skillType;
        public int SkillLevel => _skillLevel;

        #endregion


        #region ClassLifeCycle

        public CharacterSkillLevel(SkillType skillType, int skillLevel)
        {
            _skillType = skillType;
            _skillLevel = skillLevel;
        }

        #endregion
    }
}
