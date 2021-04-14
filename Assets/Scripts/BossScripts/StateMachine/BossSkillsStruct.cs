namespace BeastHunter
{
    [System.Serializable]
    public struct BossSkillStruct
    {
        public AttackStateSkillsSettings AttackStateSkills;
        public DefenceStateSkillsSettings DefenceStateSkills;
        public ChasingStateSkillsSettings ChasingStateSkills;
        public RetreatingStateSkillsSettings RetreatingStateSkills;
        public SearchingStateSkillsSettings SearchingStateSkills;
        public DeadStateSkillsSettings DeadStateSkills;
        public NonStateSkillsSettings NonStateSkills;
    }
}