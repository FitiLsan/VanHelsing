using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewModel", menuName = "CreateData/TwoHeadedSnakeData", order = 9)]
    public sealed class TwoHeadedSnakeData : EnemyData
    {
        #region Enum Behevior data 
        public enum BehaviourState
        {
            None,
            Roaming,
            Idling,
            Chasing,
            JumpingBack,
            BattleCircling,
            Escaping,
            Resting,
            Searching
        }

        #endregion


        #region Debug messages

        private Action NoneStateMsg;
        private Action IdlingStateMsg;
        private Action RoamingStateMsg;
        private Action RestingStateMsg;
        private Action ChasingStateMsg;
        private Action BackJumpingStateMsg;
        private Action BattleCirclingStateMsg;
        private Action SearchingStateMsg;
        private Action EscapingStateMsg;
        private Action TakingDamageMsg;
        private Action JumpingMsg;
        private Action AttackJumpingMsg;
        private Action AttackDirectMsg;
        private Action AttackBottomMsg;
        private Action OnDeadMsg;
        private Action OnLostSightEnemyMsg;
        private Action<float> IdlingTimerMsg;
        private Action<float> RestingTimerMsg;
        private Action<float> SearchingTimerMsg;
        private Action<string> OnDetectionEnemyMsg;
        private Action<InteractableObjectBehavior> OnHitEnemyMsg;

        #endregion


        #region Fields

        public TwoHeadedSnakeSettings settings;

        #endregion


        #region ClassLifeCycles

        public TwoHeadedSnakeData()
        {
            

        }

        #endregion


        #region UnityMethods

        private void OnEnable()
        {

            DebugMessages(settings.DebugMessages);

        }


        #endregion

        #region Methods

        public void Act(TwoHeadedSnakeModel model)
        {

            float rotateDirection = GetRotateDirection(model.Transform, ref model.RotatePosition1, ref model.RotatePosition2);
           // model.Animator.SetFloat("RotateDirection", rotateDirection);
            //model.Animator.SetFloat("MovementSpeed", model.NavMeshAgent.velocity.sqrMagnitude);

            switch (model.behaviourState)
            {
                case BehaviourState.None:
                    NoneStateMsg?.Invoke();
                    model.NavMeshAgent.SetDestination(model.Rigitbody.position);

                    BehaviourState selectedState;
                    float rollDice = Random.Range(1, 100);
                    if (rollDice < settings.RoamingChance)
                    {
                        selectedState = ChangeState(BehaviourState.Roaming, model);
                    }
                    else
                    {
                        rollDice = Random.Range(1, 100);
                        if (rollDice < settings.RestingChance)
                        {
                            selectedState = ChangeState(BehaviourState.Resting, model);
                        }
                        else
                        {
                            selectedState = ChangeState(BehaviourState.Idling, model);
                        }
                    }

                    model.behaviourState = selectedState;
                    break;
                case BehaviourState.Roaming:
                    if (model.NavMeshAgent.remainingDistance <= model.NavMeshAgent.stoppingDistance)
                    {
                        model.behaviourState = ChangeState(BehaviourState.None, model);
                    }
                    break;
                default:
                    break;

            }

        }

        #region EnemyModel

        public bool Filter(Collider collider)
        {
            return !collider.isTrigger
                   && collider.GetComponentInChildren<PlayerBehavior>() != null;
        }
        public void OnDetectionEnemy(Collider collider, TwoHeadedSnakeModel model)
        {
            OnDetectionEnemyMsg?.Invoke(collider.name);
            model.ChasingTarget = collider.transform;
            //model.behaviourState = ChangeState(BehaviourState.Chasing, model);
        }
        public void OnLostEnemy(Collider collider, TwoHeadedSnakeModel model)
        {
            if (collider.transform.Equals(model.ChasingTarget))
            {
                OnLostSightEnemyMsg?.Invoke();
                model.ChasingTarget = null;
            }
        }

        #endregion


        #region SetStates

        private BehaviourState ChangeState(BehaviourState newState, TwoHeadedSnakeModel model)
        {
            //exit from current state
            switch (model.behaviourState)
            {
                case BehaviourState.None:
                    break;

                case BehaviourState.Roaming:
                    break;

                case BehaviourState.Idling:
                    break;

                case BehaviourState.Chasing:
                    break;

                case BehaviourState.JumpingBack:
                    break;

                case BehaviourState.BattleCircling:
                    // model.Animator.SetBool("BattleCircling", false);
                    break;

                case BehaviourState.Escaping:
                    break;

                case BehaviourState.Resting:
                    //model.Animator.SetTrigger("RestingEnd");
                    break;

                case BehaviourState.Searching:
                    break;
                default: return model.behaviourState;
            }

            //enter to new state
            switch (newState)
            {
                case BehaviourState.None:
                    return BehaviourState.None;

                case BehaviourState.Roaming:
                    return SetRoamingState(model.NavMeshAgent, model.SpawnPoint, ref model.Timer);

                case BehaviourState.Idling:
                    return SetIdlingState(ref model.Timer);
                    

                case BehaviourState.Chasing:
                    //return SetChasingState(model.NavMeshAgent);
                    return BehaviourState.Chasing;

                case BehaviourState.JumpingBack:
                   // return SetJumpingBackState(model.NavMeshAgent, model.Animator, model.Rigidbody);
                   return BehaviourState.JumpingBack;

                case BehaviourState.BattleCircling:
                   // return SetBattleCirclingState(model.NavMeshAgent, model.Animator, model.ChasingTarget.position, ref model.Timer);
                   return BehaviourState.BattleCircling;

                case BehaviourState.Escaping:
                    // return SetEscapingState(model.NavMeshAgent, model.ChasingTarget.position);
                    return BehaviourState.Escaping;

                case BehaviourState.Resting:
                    //return SetRestingState(model.Animator, ref model.Timer);
                    return BehaviourState.Resting;

                case BehaviourState.Searching:
                   // return SetSearchingState(model.NavMeshAgent, model.SpawnPoint, ref model.Timer);
                   return BehaviourState.Searching;

                default: return newState;
            }
            
        }

        private BehaviourState SetIdlingState(ref float timer)
        {
            IdlingStateMsg?.Invoke();

            timer = Random.Range(settings.IdlingMinTime, settings.IdlingMaxTime);
            IdlingTimerMsg?.Invoke(timer);

            return BehaviourState.Idling;
        }

        private BehaviourState SetRoamingState(NavMeshAgent navMeshAgent, Vector3 spawnPoint, ref float timer)
        {
            RoamingStateMsg?.Invoke();

            Vector3 navMeshPoint;
            bool isSetDestination = false;

            for (int i = 0; i < 100; i++)
            {
                if (!RandomInsideSphereNavMeshPoint(spawnPoint, settings.WanderingRadius, settings.WanderingRadius * 2, out navMeshPoint))
                {
                    Debug.LogError(this + ": not found NavMesh in SetRoamingState method");
                    return SetIdlingState(ref timer);
                }

                isSetDestination = navMeshAgent.SetDestination(navMeshPoint);
                if (isSetDestination) break;
            }

            if (!isSetDestination)
            {
                Debug.LogError(this + ": impossible to reach the destination point in SetRoamingState method");
                return SetIdlingState(ref timer);
            }

            return BehaviourState.Roaming;
        }


        #endregion


        #endregion

        #region Helpers
        /// <summary>Get the direction of the turn</summary>
        /// <param name="transform">GO transform</param>
        /// <param name="rotatePosition1">Previous rotation value</param>
        /// <param name="rotatePosition2">Current rotation value</param>
        /// <returns>If value is negative turn goes left</returns>
        private float GetRotateDirection(Transform transform, ref float rotatePosition1, ref float rotatePosition2)
        {
            rotatePosition1 = rotatePosition2;
            rotatePosition2 = transform.rotation.eulerAngles.y;
            return rotatePosition2 - rotatePosition1;
        }

        /// <summary>Searches for a random NavMesh point inside sphere. If successful saves it to out variable</summary>
        /// <param name="center">center of sphere</param>
        /// <param name="radius">radius of sphere</param>
        /// <param name="searchDistance">the length of the ray that searches for NavMesh</param>
        /// <param name="navMeshPoint">out parameter for save the found NavMesh point</param>
        /// <returns>true if successful</returns>
        private bool RandomInsideSphereNavMeshPoint(Vector3 center, float radius, float searchDistance, out Vector3 navMeshPoint)
        {
            navMeshPoint = default;
            NavMeshHit navMeshHit;
            Vector3 randomPoint;

            for (int i = 0; i < 100; i++)
            {
                randomPoint = Random.insideUnitSphere * radius + center;
                if (NavMesh.SamplePosition(randomPoint, out navMeshHit, searchDistance, NavMesh.AllAreas))
                {
                    navMeshPoint = navMeshHit.position;
                    return true;
                }
            }
            return false;
        }

        /// <summary>Subscribes to message delegates</summary>
        /// <param name="switcher">on/off debug messages</param>
        private void DebugMessages(bool switcher)
        {
            if (switcher)
            {
                NoneStateMsg = () => Debug.Log("State selection");
                IdlingStateMsg = () => Debug.Log("The snake is idling");
                RoamingStateMsg = () => Debug.Log("The snake is roaming");
                RestingStateMsg = () => Debug.Log("The snake is resting");
                ChasingStateMsg = () => Debug.Log("The snake is chasing");
                BackJumpingStateMsg = () => Debug.Log("The snake is jumping back");
                BattleCirclingStateMsg = () => Debug.Log("The snake is battle circling");
                SearchingStateMsg = () => Debug.Log("The snake is searching");
                EscapingStateMsg = () => Debug.Log("The snake is escaping");
                TakingDamageMsg = () => Debug.Log("The snake is taking damage");
                JumpingMsg = () => Debug.Log("The snake is jumping");
                AttackJumpingMsg = () => Debug.Log("The snake is jumping attack");
                AttackDirectMsg = () => Debug.Log("The snake is attacking direct");
                AttackBottomMsg = () => Debug.Log("The snake is attacking bottom");
                OnDeadMsg = () => Debug.Log("Hell hound is dead");
                IdlingTimerMsg = (timer) => Debug.Log("Idling time = " + timer);
                RestingTimerMsg = (timer) => Debug.Log("Resting timer = " + timer);
                SearchingTimerMsg = (timer) => Debug.Log("Searching timer = " + timer);
                OnHitEnemyMsg = (enemy) => Debug.Log("The snake is deal damage to " + enemy);
                OnDetectionEnemyMsg = (colliderName) => Debug.Log("The snake noticed " + colliderName);
                OnLostSightEnemyMsg = () => Debug.Log("The snake lost sight of the target");
            }
        }

        #endregion
    }
}
