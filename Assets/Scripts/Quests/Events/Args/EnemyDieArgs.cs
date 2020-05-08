using System;


namespace BeastHunter
{
    public sealed class EnemyDieArgs : EventArgs
    {
        #region Properties

        public int Id { get; }
        public int FamilyId { get; }

        #endregion


        #region ClassLifeCycle

        public EnemyDieArgs(int id, int familyId)
        {
            Id = id;
            FamilyId = familyId;
        }

        #endregion
    }
}