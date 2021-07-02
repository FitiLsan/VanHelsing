using System.Collections.Generic;
using UnityEngine;


namespace BeastHunterHubUI
{
    [System.Serializable]
    public class BaseWorkRoomData<T> where T : BaseWorkRoomProgress
    {
        #region Fields

        [SerializeField] private WorkRoomType _roomType;
        [SerializeField] private SkillType _usedSkill;
        [SerializeField] private string _name;
        [SerializeField] private int _level;
        [SerializeField] private List<WorkRoomProgressLevel<T>> _progressScheme;
        [SerializeField] private CharacterSO _chiefWorker;
        [SerializeField] private List<CharacterSO> _assistants;

        #endregion


        #region Properties

        public WorkRoomType RoomType => _roomType;
        public SkillType UsedSkill => _usedSkill;
        public string Name => _name;
        public int Level => _level;
        public CharacterSO ChiefWorker => _chiefWorker;
        public List<CharacterSO> Assistants => _assistants;
        public List<WorkRoomProgressLevel<T>> ProgressScheme => _progressScheme;

        #endregion
    }
}
