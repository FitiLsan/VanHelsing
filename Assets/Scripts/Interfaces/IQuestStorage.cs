namespace BeastHunter
{
    public interface IQuestStorage : ISaveManager
    {
        Quest GetQuestById(int id);
    }
}