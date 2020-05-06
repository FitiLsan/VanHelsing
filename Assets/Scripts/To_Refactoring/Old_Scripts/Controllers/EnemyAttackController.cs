using Interfaces;
using UnityEngine;

namespace Controllers
{
    /// <summary>
    /// Контроллер атак врагов
    /// </summary>
    public class EnemyAttackController : MonoBehaviour
    {
        private const float Damage = 10;
        private Collider _target;

        private void AttackTarget()
        {
            {
                Debug.Log($"target  attack {_target}");
                var d = _target.GetComponent<IDamageable>();
                d?.TakeDamage(Damage);
            }
        }

        private void OnTriggerEnter(Collider collider)
        {
            _target = collider;
            Debug.Log($"Detect TARGETDETECTOR => {_target}");

            AttackTarget();
        }
    }
}