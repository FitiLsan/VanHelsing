using System;
using UnityEngine;


namespace BeastHunterHubUI
{
    [Serializable]
    public class CharacterHeadPart
    {
        #region Fields

        [SerializeField] CharacterHeadPartType _type;
        [SerializeField] string _name;
        [SerializeField] bool _isActivateByDefault;

        #endregion


        #region Properties

        public CharacterHeadPartType Type => _type;
        public string Name => _name;
        public bool IsActivateByDefault => _isActivateByDefault;

        #endregion


        #region ClassLifeCycle

        public CharacterHeadPart(CharacterHeadPartType type, string name)
        {
            _type = type;
            _name = name;
            _isActivateByDefault = true;
        }

        #endregion
    }
}
