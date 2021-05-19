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
        private Action BattleCirclingStateMsg;
        private Action SearchingStateMsg;
        private Action EscapingStateMsg;
        private Action TakingDamageMsg;
        private Action JumpingMsg;
        private Action AttackJumpingMsg;
        private Action TailAttackMsg;
        private Action TwinHeadAttackMsg;
        private Action OnDeadMsg;
        private Action OnLostSightEnemyMsg;
        private Action<float> IdlingTimerMsg;
        private Action<float> RestingTimerMsg;
        private Action<float> SearchingTimerMsg;
        private Action<string> OnDetectionEnemyMsg;
        private Action<InteractableObjectBehavior> OnHitEnemyMsg;

        #endregion


        #region Fields
        
        private float _sqrTailAttackDistance;
        private float _sqrTwinHeadAttackDistance;
        private float _sqrChasingTurnDistanceNearTarget;

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
            _sqrTailAttackDistance = settings.TailAttackDistance;
            _sqrTwinHeadAttackDistance = settings.TwinHeadAttackDistance * settings.TwinHeadAttackDistance;
            _sqrChasingTurnDistanceNearTarget = settings.ChasingTurnDistanceNearTarget;
            DebugMessages(settings.DebugMessages);

        }


        #endregion

        #region Methods

        public override void Act(EnemyModel enemyModel)
        {
            Execute(enemyModel as TwoHeadedSnakeModel);
        }

        public void Execute(TwoHeadedSnakeModel model)
        {
            float rotateDirection = GetRotateDirection(model.Transform, ref model.rotatePosition1, ref model.rotatePosition2);
            model.Animator.SetFloat("Velosity", model.NavMeshAgent.velocity.sqrMagnitude);
            
            switch (model.BehaviourState)
            {
                case BehaviourState.None:
                    NoneStateMsg?.Invoke();
                    model.NavMeshAgent.SetDestination(model.Rigidbody.position);

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

                    model.BehaviourState = selectedState;
                    break;
                case BehaviourState.Roaming:
                    if (model.NavMeshAgent.remainingDistance <= model.NavMeshAgent.stoppingDistance)
                    {
                        model.BehaviourState = ChangeState(BehaviourState.None, model);
                    }
                    break;
                case BehaviourState.Idling:

                    model.timer -= Time.deltaTime;
                    if (model.timer <= 0)
                    {
                        model.BehaviourState = ChangeState(BehaviourState.None, model);
                    }

                    break;
                case BehaviourState.Resting:

                    model.timer -= Time.deltaTime;
                    if (model.timer <= 0)
                    {
                        model.BehaviourState = ChangeState(BehaviourState.None, model);
                    }

                    break;
                case BehaviourState.Chasing:

                    if (model.ChasingTarget == null)
                    {
                        model.BehaviourState = ChangeState(BehaviourState.None, model);
                    }
                    else
                    {
                        if (CurrentHealthPercent(model.CurrentStats.BaseStats.CurrentHealthPoints) < settings.
                            PercentEscapeHealth && !model.isAttacking)
                        {
                            model.BehaviourState = ChangeState(BehaviourState.Escaping, model);
                        }
                        else
                        {
                            model.NavMeshAgent.SetDestination(model.ChasingTarget.position);

                            float sqrDistanceToEnemy = (model.ChasingTarget.position - model.Rigidbody.position).sqrMagnitude;
                            
                            if (sqrDistanceToEnemy <= _sqrChasingTurnDistanceNearTarget)
                            {
                                model.Transform.rotation = model.isAttacking ?
                                    SmoothTurn(model.ChasingTarget.position - model.Transform.position, model.Transform.forward, settings.AttacksTurnSpeed) :
                                    SmoothTurn(model.ChasingTarget.position - model.Transform.position, model.Transform.forward, settings.ChasingTurnSpeedNearTarget);
                                
                            }

                            if (!model.isAttacking)
                            {

                                AttackStateControl(model);

                            }
                        }
                        
                    }

                    break;
                case BehaviourState.Escaping:

                    if (model.NavMeshAgent.remainingDistance <= model.NavMeshAgent.stoppingDistance)
                    {
                        if (model.ChasingTarget == null)
                        {
                            model.BehaviourState = ChangeState(BehaviourState.None, model);
                        }
                        else
                        {
                            Vector3 navMeshPoint;
                            bool isSetDestination = false;

                            for (int i = 0; i < 100; i++)
                            {
                                if (!RandomBorderCircleNavMeshPoint(model.ChasingTarget.position, settings.EscapeDistance, settings.EscapeDistance * 2, out navMeshPoint))
                                {
                                    Debug.LogError(this + ": not found NavMesh in case BehaviourState.Escaping");
                                    model.BehaviourState = ChangeState(BehaviourState.Chasing, model);
                                    break;
                                }

                                isSetDestination = model.NavMeshAgent.SetDestination(navMeshPoint);
                                if (isSetDestination) break;
                            }

                            if (!isSetDestination)
                            {
                                Debug.LogError(this + ": impossible to reach the destination point in case BehaviourState.Escaping");
                                model.BehaviourState = ChangeState(BehaviourState.Chasing, model);
                            }
                        }
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
            model.BehaviourState = ChangeState(BehaviourState.Chasing, model);
        }
        public void OnLostEnemy(Collider collider, TwoHeadedSnakeModel model)
        {
            if (collider.transform.Equals(model.ChasingTarget))
            {
                OnLostSightEnemyMsg?.Invoke();
                model.ChasingTarget = null;
            }
        }
        public void OnHitEnemy(Collider collider, TwoHeadedSnakeModel model)
        {
            Damage damage = new Damage()
            {
                ElementDamageType = ElementDamageType.None,
                ElementDamageValue = 0,
                PhysicalDamageType = PhysicalDamageType.Cutting,
                PhysicalDamageValue = settings.PhysicalDamage
            };

            InteractableObjectBehavior enemy = collider.gameObject.GetComponent<InteractableObjectBehavior>();
            OnHitEnemyMsg?.Invoke(enemy);

            if (enemy != null) enemy.TakeDamageEvent(damage);
            else Debug.LogError(this + " not found enemy InteractableObjectBehavior");


            SwitcherColladers(model.TailAttackColliders, false);
            SwitcherColladers(model.TwinHeadAttackColliders, false);
        }

        public void OnAttackStateEnter(TwoHeadedSnakeModel model)
        {
            
            model.isAttacking = true;
            SwitcherColladers(model.TailAttackColliders, true);
            SwitcherColladers(model.TwinHeadAttackColliders, true);
        }

        public void OnAttackStateExit(TwoHeadedSnakeModel model)
        {
            
            model.isAttacking = false;
            SwitcherColladers(model.TailAttackColliders, false);
            SwitcherColladers(model.TwinHeadAttackColliders, false);
        }
        #endregion


        #region SetStates

        private BehaviourState ChangeState(BehaviourState newState, TwoHeadedSnakeModel model)
        {
            //exit from current state
            switch (model.BehaviourState)
            {
                case BehaviourState.None:
                    break;

                case BehaviourState.Roaming:
                    break;

                case BehaviourState.Idling:
                    break;

                case BehaviourState.Chasing:
                    break;

                case BehaviourState.BattleCircling:
                    // model.Animator.SetBool("BattleCircling", false);
                    break;

                case BehaviourState.Escaping:
                    break;

                case BehaviourState.Resting:
                    //Need a Resting animation
                    //model.Animator.SetTrigger("RestingEnd");
                    break;

                case BehaviourState.Searching:
                    break;
                default: return model.BehaviourState;
            }

            //enter to new state
            switch (newState)
            {
                case BehaviourState.None:
                    return BehaviourState.None;

                case BehaviourState.Roaming:
                    return SetRoamingState(model.NavMeshAgent, model.SpawnPoint, ref model.timer);

                case BehaviourState.Idling:
                    return SetIdlingState(ref model.timer);

                case BehaviourState.Chasing:
                    return SetChasingState(model.NavMeshAgent);
                
                case BehaviourState.BattleCircling:
                   // return SetBattleCirclingState(model.NavMeshAgent, model.Animator, model.chasingTarget.position, ref model.timer);
                   return BehaviourState.BattleCircling;

                case BehaviourState.Escaping:
                    return SetEscapingState(model.NavMeshAgent, model.ChasingTarget.position);

                case BehaviourState.Resting:
                    return SetRestingState(model.Animator, ref model.timer);

                case BehaviourState.Searching:
                   // return SetSearchingState(model.NavMeshAgent, model.SpawnPoint, ref model.timer);
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

            navMeshAgent.speed = settings.MaxRoamingSpeed;
            navMeshAgent.acceleration = settings.NavMeshAcceleration;

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

        private BehaviourState SetRestingState(Animator animator, ref float timer)
        {
            RestingStateMsg?.Invoke();

            timer = Random.Range(settings.RestingMinTime, settings.RestingMaxTime);
            RestingTimerMsg?.Invoke(timer);

           //Resting animation

            return BehaviourState.Resting;
        }

        private BehaviourState SetChasingState(NavMeshAgent navMeshAgent)
        {
            ChasingStateMsg?.Invoke();

            navMeshAgent.updateRotation = true;
            navMeshAgent.stoppingDistance = settings.StoppingDistance;
            navMeshAgent.acceleration = settings.NavMeshAcceleration;
            navMeshAgent.speed = settings.MaxChasingSpeed;

            return BehaviourState.Chasing;
        }

        private BehaviourState SetEscapingState(NavMeshAgent navMeshAgent, Vector3 chasingTargetPosition)
        {
            EscapingStateMsg?.Invoke();

            navMeshAgent.speed = settings.EscapingSpeed;
            navMeshAgent.stoppingDistance = 0;
            navMeshAgent.updateRotation = true;

            Vector3 navMeshPoint;
            bool isSetDestination = false;

            for (int i = 0; i < 100; i++)
            {
                if (!RandomBorderCircleNavMeshPoint(chasingTargetPosition, settings.EscapeDistance, settings.EscapeDistance * 2, out navMeshPoint))
                {
                    Debug.LogError(this + ": not found NavMesh in SetEscapingState method");
                    return SetChasingState(navMeshAgent);
                }

                isSetDestination = navMeshAgent.SetDestination(navMeshPoint);
                if (isSetDestination) break;
            }

            if (!isSetDestination)
            {
                Debug.LogError(this + ": impossible to reach the destination point in SetEscapingState method");
                return SetChasingState(navMeshAgent);
            }

            return BehaviourState.Escaping;
        }

        private void AttackStateControl(TwoHeadedSnakeModel model)
        {
            float sqrDistanceToEnemy = (model.ChasingTarget.position - model.Rigidbody.position).sqrMagnitude;

            model.attackCoolDownTimer -= Time.deltaTime;
            
            if (model.attackCoolDownTimer <= 0)
            {
               
                
                int rollDice = Random.Range(1, 100);
                
                if (rollDice < 50)
                {
                    bool onTailAttack = (sqrDistanceToEnemy < _sqrTailAttackDistance);
                    if (onTailAttack)
                    {

                        TailAttack(model.Animator);
                        model.attackCoolDownTimer = settings.AttackCooldown;
                    }
                }
                else
                {
                    bool onTwinHeadAttack = (sqrDistanceToEnemy < _sqrTwinHeadAttackDistance);
                    if (onTwinHeadAttack)
                    {

                        TwinHeadAttack(model.Animator);
                        model.attackCoolDownTimer = settings.AttackCooldown;
                    }
                }
                

            }

        }

        private void TailAttack(Animator animator)
        {
            TailAttackMsg?.Invoke();
            animator.Play("TailAttack");
        }

        private void TwinHeadAttack(Animator animator)
        {
            TwinHeadAttackMsg?.Invoke();
            animator.Play("TwinHeadAttack");
        }

        #endregion


        #endregion


        #region Helpers

        private Quaternion SmoothTurn(Vector3 targetDirection, Vector3 forward, float speed)
        {
            Vector3 newDirection = Vector3.RotateTowards(forward, targetDirection, speed, 0.0f);
            newDirection.y = forward.y;
            return Quaternion.LookRotation(newDirection);
        }

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

        /// <summary>Searches for a random NavMesh point on the boundary of a circle. If successful saves it to out variable</summary>
        /// <param name="center">center of circle</param>
        /// <param name="radius">radius of circle</param>
        /// <param name="searchDistance">the length of the ray that searches for NavMesh</param>
        /// <param name="navMeshPoint">out parameter for save the found NavMesh point</param>
        /// <returns>true if successful</returns>
        private bool RandomBorderCircleNavMeshPoint(Vector3 center, float radius, float searchDistance, out Vector3 navMeshPoint)
        {
            navMeshPoint = default;
            NavMeshHit navMeshHit;
            Vector3 randomPoint;
            Vector2 randomPoint2D;
            Vector2 center2D = new Vector2(center.x, center.z);

            for (int i = 0; i < 100; i++)
            {
                randomPoint2D = new Vector2(Random.value - 0.5f, Random.value - 0.5f).normalized * radius + center2D;
                randomPoint = new Vector3(randomPoint2D.x, center.y, randomPoint2D.y);

                if (NavMesh.SamplePosition(randomPoint, out navMeshHit, searchDistance, NavMesh.AllAreas))
                {
                    navMeshPoint = navMeshHit.position;
                    return true;
                }
            }
            return false;
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

        /// <summary>Current health in percent</summary>
        private float CurrentHealthPercent(float currentHealth)
        {
            return currentHealth * 100 / StartStats.BaseStats.MaximalHealthPoints;
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
                
                BattleCirclingStateMsg = () => Debug.Log("The snake is battle circling");
                SearchingStateMsg = () => Debug.Log("The snake is searching");
                EscapingStateMsg = () => Debug.Log("The snake is escaping");
                TakingDamageMsg = () => Debug.Log("The snake is taking damage");
                JumpingMsg = () => Debug.Log("The snake is jumping");
                AttackJumpingMsg = () => Debug.Log("The snake is jumping attack");
                TailAttackMsg = () => Debug.Log("The snake is attacking tail");
                TwinHeadAttackMsg = () => Debug.Log("The snake is attacking twin head");
                OnDeadMsg = () => Debug.Log("Snake is dead");
                IdlingTimerMsg = (timer) => Debug.Log("Idling time = " + timer);
                RestingTimerMsg = (timer) => Debug.Log("Resting timer = " + timer);
                SearchingTimerMsg = (timer) => Debug.Log("Searching timer = " + timer);
                OnHitEnemyMsg = (enemy) => Debug.Log("The snake is deal damage to " + enemy);
                OnDetectionEnemyMsg = (colliderName) => Debug.Log("The snake noticed " + colliderName);
                OnLostSightEnemyMsg = () => Debug.Log("The snake lost sight of the target");
                
            }
        }

        private void SwitcherColladers(Collider[] colliders, bool enableSwitcher)
        {
            foreach (var collider in colliders)
            {
                collider.enabled = enableSwitcher;
            }
        }

        #endregion


        #region EnemyData

        public override void TakeDamage(EnemyModel model, Damage damage)
        {
            TwoHeadedSnakeModel twoHeadedSnakeModel = model as TwoHeadedSnakeModel;
            base.TakeDamage(model, damage);

            TakingDamageMsg?.Invoke();
            twoHeadedSnakeModel.Animator.SetTrigger("TakeDamage");

            if (model.CurrentStats.BaseStats.IsDead)
            {
                OnDeadMsg?.Invoke();
                twoHeadedSnakeModel.Animator.SetTrigger("Dead");
                twoHeadedSnakeModel.NavMeshAgent.enabled = false;
            }

            if (twoHeadedSnakeModel.ChasingTarget == null
                && (twoHeadedSnakeModel.BehaviourState == BehaviourState.Roaming
                    || twoHeadedSnakeModel.BehaviourState == BehaviourState.Idling
                    || twoHeadedSnakeModel.BehaviourState == BehaviourState.Resting))
            {
                twoHeadedSnakeModel.BehaviourState = ChangeState(BehaviourState.Searching, twoHeadedSnakeModel);
            }
        }

        #endregion
    }
}
