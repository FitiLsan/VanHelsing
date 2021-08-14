using System;
using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewModel", menuName = "CreateData/Butterfly", order = 3)]
    public class ButterflyData : EnemyData
    {
        #region Fields

        public ButterflyStats Stats;

        #endregion


        #region Properties

        private static PhysicsService PhysicsService => Services.SharedInstance.PhysicsService;

        #endregion


        #region Methods

        public void Act(ButterflyModel butterfly)
        {
            if (!butterfly.IsDead)
            {
                switch (butterfly.State)
                {
                    case ButterflyState.Calm:
                        ActCalm(butterfly);
                        break;
                    case ButterflyState.Curious:
                        ActCurious(butterfly);
                        break;
                    case ButterflyState.Escape:
                        ActEscape(butterfly);
                        break;
                    case ButterflyState.None:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                ExecuteState(butterfly);
            }
        }

        private void ExecuteState(ButterflyModel butterfly)
        {
            var speed = butterfly.State != ButterflyState.Escape ? Stats.NormalSpeed : Stats.EscapeSpeed;
            var position = butterfly.GameObject.transform.position;
            var direction = (butterfly.TargetCoordinates - position);
            var velocityToDestiny = direction.normalized * speed;
            butterfly.Transform.LookAt(butterfly.TargetCoordinates);
            butterfly.Rigidbody.velocity = direction.magnitude <= 0.1f 
                ? Vector3.zero 
                : velocityToDestiny;
        }

        private void ActEscape(ButterflyModel butterfly)
        {
            var playersPosition = butterfly.Player.transform.position;
            butterfly.TargetCoordinates = GetAboveGroundCoordinates(playersPosition +
                (butterfly.GameObject.transform.position - playersPosition) *
                Stats.MovingObjectsEscapeRadius, butterfly.Transform.position, Stats.NormalFlyingHeight);
            if (!CheckNearbyPlayer(butterfly, Stats.MovingObjectsEscapeRadius * 2))
            {
                butterfly.State = ButterflyState.Calm;
            }
        }

        private void ActCurious(ButterflyModel butterfly)
        {
            var playerPosition = butterfly.Player.transform.position;
            var isPlayerResting = CheckPlayerResting(butterfly, Stats.MaximumRestingSpeed);
            if (!isPlayerResting)
                butterfly.State = ButterflyState.Escape;
            var isPlayerInRadius = CheckNearbyPlayer(butterfly, Stats.MovingObjectsEscapeRadius);
            var isPlayerCallCuriosity = isPlayerInRadius && !CheckNearbyPlayer(butterfly, 1.0f) && isPlayerResting;
            if (isPlayerCallCuriosity)
            {
                butterfly.TargetCoordinates = GetAboveGroundCoordinates(
                    playerPosition + butterfly.Player.transform.forward,
                    butterfly.Transform.position, playerPosition.y);
            }
            if (!isPlayerInRadius)
            {
                butterfly.State = ButterflyState.Calm;
            }
        }

        private void ActCalm(ButterflyModel butterfly)
        {
            var position = butterfly.Transform.position;
            butterfly.TargetCoordinates = GetAboveGroundCoordinates(
                position,
                position, Stats.NormalFlyingHeight);
            if (CheckNearbyPlayer(butterfly, Stats.MovingObjectsEscapeRadius) && !CheckNearbyPlayer(butterfly, 1.0f))
            {
                butterfly.State = CheckPlayerResting(
                    butterfly, 
                    Stats.MaximumRestingSpeed)
                    ? ButterflyState.Curious
                    : ButterflyState.Escape;
            }
        }

        private static bool CheckPlayerResting(
            ButterflyModel butterfly, 
            float maxSpeed) =>
            butterfly.PlayerSpeed < maxSpeed;

        private static bool CheckNearbyPlayer(
            ButterflyModel butterfly, 
            float radius) =>
            (butterfly.Player.transform.position - butterfly.Transform.position)
            .magnitude < radius;


        private static Vector3 GetAboveGroundCoordinates(
            Vector3 targetCoordinates,
            Vector3 currentCoordinates, 
            float height)
        {
            if (PhysicsService.CheckGround(
                targetCoordinates,
                height * 2,
                out var targetPoint))
            {
                var targetHeight = targetPoint.y + height;
                targetCoordinates.y = targetHeight;
                return targetCoordinates;
            }
            if (PhysicsService.CheckGround(
                currentCoordinates,
                height * 2,
                out var currentPoint))
            {
                var currentHeight = currentPoint.y + height;
                targetCoordinates.y = currentHeight;
                return targetCoordinates;
            }
            targetCoordinates.y = currentCoordinates.y;
            return targetCoordinates;
        }

        public override void TakeDamage(EnemyModel instance, Damage damage)
        {
            base.TakeDamage(instance, damage);

            if (instance.IsDead)
            {
                var butterfly = (ButterflyModel)instance;
                butterfly.Rigidbody.velocity = Vector3.zero;
                butterfly.Rigidbody.useGravity = true;
                butterfly.Rigidbody.constraints = RigidbodyConstraints.None;
            }
        }

        #endregion
    }
}
