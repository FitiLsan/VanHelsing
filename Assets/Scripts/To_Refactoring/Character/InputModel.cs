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
        public Action OnAttackLeft { get; set; }
        public Action OnAttackRight { get; set; }
        public Action OnTargetLock { get; set; }
        public Action OnBattleExit { get; set; }
        public Action OnUse { get; set; }
        public Action OnDance { get; set; }

        #endregion
    }
}

