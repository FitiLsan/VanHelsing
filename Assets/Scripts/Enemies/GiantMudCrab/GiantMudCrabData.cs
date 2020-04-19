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
            if (giantMudCrabStruct.CanAttack && !giantMudCrabStruct.ShouldDigIn)
            {
                if (DistanceBetweenTargetAndPrefab <= giantMudCrabStruct.AttackRange)
                {
                    CrabAgent.isStopped = true;
                    CrabAgent.ResetPath();
                    if (Time.time >= NextAttackRate)
                    {
                        Debug.Log("Attacking");
                        new GiantMudCrabProjectile(giantMudCrabStruct.AttackDamage, Target.transform, Prefab.transform.GetChild(0), giantMudCrabStruct.CrabProjectile);
                        NextAttackRate = giantMudCrabStruct.AttackSpeed + Time.time;
                    }
                }
                else if (DistanceBetweenTargetAndPrefab > giantMudCrabStruct.AttackRange)
                {
                    CrabAgent.destination = Target.transform.position;
                }
            }
        }

        public void DigIn(GameObject Prefab, GameObject Target, float TriggerDistance)
        {
            float DistanceBetweenTargetAndPrefab = Vector2.Distance(Target.transform.position, Prefab.transform.position);
            if (DistanceBetweenTargetAndPrefab < TriggerDistance)
                return;
        }

        public void Chase(GameObject Target, NavMeshAgent CrabAgent, bool IsChase)
        {
            if (IsChase)
            {

            }
        }

        public void BackHome(Vector3 HomePoint, Transform Pregab)
        {

        }

        #endregion
    }
}

