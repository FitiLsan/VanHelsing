using UnityEngine;

namespace BeastHunter
{
    public interface IDealDamage
    {
        void DealDamage(InteractableObjectBehavior enemy, DamageStruct damage);
    }
}

