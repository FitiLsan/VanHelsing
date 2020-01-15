using System;

namespace Events.Args
{
    /// <summary>
    ///     Use this when npc dies. Id - NPC id, FamilyId - id of group (ex. Bandits, Animals...)
    /// </summary>
    public class EnemyDieArgs : EventArgs
    {
        public EnemyDieArgs(int id, int familyId)
        {
            Id = id;
            FamilyId = familyId;
        }

        public int Id { get; }
        public int FamilyId { get; }
    }
}