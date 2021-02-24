namespace BeastHunter
{
    public sealed class BossBehavior : InteractableObjectBehavior
    {
        public void SetType(InteractableObjectType type)
        {
            _type = type;
        }
    }
}

