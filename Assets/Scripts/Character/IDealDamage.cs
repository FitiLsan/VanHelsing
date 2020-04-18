using UnityEngine;

namespace BeastHunter
{
    public interface IDealDamage
    {
        void DealDamage(Collider enemy, DamageStruct damage);
    }
}

