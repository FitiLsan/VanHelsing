using System;


namespace BeastHunter
{
    public sealed class InputModel
    {
        #region Fields

        public InputStruct inputStruct;

        #endregion


        #region Properties

        public Action OnJump { get; set; }
        public Action OnDodge { get; set; }
        public Action OnAttack { get; set; }
        public Action OnTargetLock { get; set; }
        public Action OnBattleExit { get; set; }

        #endregion
    }
}

