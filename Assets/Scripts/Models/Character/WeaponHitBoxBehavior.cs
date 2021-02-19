namespace BeastHunter
{
    public sealed class WeaponHitBoxBehavior : HitBoxBehavior
    {
        public void SetType(InteractableObjectType type)
        {
            _type = type;
        }
    }
}
