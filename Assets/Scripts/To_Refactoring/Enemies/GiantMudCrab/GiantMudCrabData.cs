using UnityEngine;
using UnityEngine.AI;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewData", menuName = "CreateData/GiantMudCrab", order = 0)]
    public sealed class GiantMudCrabData : ScriptableObject
    {
        #region Fields

        public GiantMudCrabStruct GiantMudCrabStruct;
        private float _idleTime = 0;
        private Vector3 _destination;
        public float NextAttackRate = 0;

        #endregion


        #region Metods

        public void Patrol(NavMeshAgent CrabAgent, Vector3 SpawnPoint, float PatrolDistance, bool IsPatrol)
        {
            _idleTime -= Time.deltaTime;
            if (CrabAgent.pathPending || CrabAgent.remainingDistance > 0.1f)
                return;
            else if (IsPatrol && _idleTime <= 0)
            {
                _destination = PatrolDistance * Random.insideUnitCircle;
            }
            if (_idleTime <= 0)
            {
                _idleTime = Random.Range(0, 15);
            }
            CrabAgent.destination = _destination;
        }

        public void Attack(GiantMudCrabStruct giantMudCrabStruct, NavMeshAgent CrabAgent, GameObject Target, GameObject Prefab)
        {
            float DistanceBetweenTargetAndPrefab = Vector3.Distance(Target.transform.position, Prefab.transform.position);
            if (giantMudCrabStruct.CanAttack && !giantMudCrabStruct.IsDigIn)
            {
                if (DistanceBetweenTargetAndPrefab <= giantMudCrabStruct.AttackRange)
                {
                    CrabAgent.isStopped = true;
                    CrabAgent.ResetPath();
                    RotateTowards(Target.transform, Prefab.transform);
                    if (Time.time >= NextAttackRate)
                    {
                        Debug.Log("Attacking");
                        new GiantMudCrabProjectile(giantMudCrabStruct.AttackDamage, giantMudCrabStruct.Stats, Target.transform, Prefab.transform.GetChild(0), giantMudCrabStruct.CrabProjectile);
                        NextAttackRate = giantMudCrabStruct.AttackSpeed + Time.time;
                    }
                }
                else if (DistanceBetweenTargetAndPrefab > giantMudCrabStruct.AttackRange)
                {
                    CrabAgent.destination = Target.transform.position;
                }
            }
        }

        public bool DigIn(GameObject Prefab, GameObject Target, float DiggingDistance, float CurrentHealth, float MaxHealth, bool IsDigIn)
        {
            float DistanceBetweenTargetAndPrefab = Vector3.Distance(Target.transform.position, Prefab.transform.position);
            if (DistanceBetweenTargetAndPrefab < DiggingDistance && CurrentHealth <= MaxHealth / 3)
            {
                IsDigIn = true;
                return IsDigIn;
            }
            IsDigIn = false;
            return IsDigIn;
        }

        public void BackHome(Vector3 HomePoint, Transform Prefab)
        {

        }

        public void RotateTowards(Transform target, Transform Prefab)
        {
            Vector3 direction = (target.position - Prefab.transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            Prefab.transform.rotation = Quaternion.Slerp(Prefab.transform.rotation, lookRotation, Time.deltaTime * 3);
        }
        #endregion
    }
}

