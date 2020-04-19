using UnityEngine;


namespace BeastHunter
{
    public class BattleIdleState : CharacterBaseState
    {
        #region Properties

        public Collider ClosestEnemy { get; private set; }

        #endregion



        #region ClassLifeCycle

        public BattleIdleState(CharacterModel characterModel, InputModel inputModel) : base(characterModel, inputModel)
        {
            CanExit = true;
            CanBeOverriden = true;
        }

        #endregion

        #region Methods

        public override void Initialize()
        {

        }

        public override void Execute()
        {
            GetClosestEnemy();
            StayOnOnePlace();
            StayInBattle();
        }

        public override void OnExit()
        {

        }

        private void StayInBattle()
        {
            _characterModel.IsInBattleMode = true;
        }

        private void StayOnOnePlace()
        {
            _characterModel.CharacterData.MoveForward(_characterModel.CharacterTransform, 0);

            if (_characterModel.IsTargeting)
            {
                if (ClosestEnemy != null && _characterModel.IsTargeting)
                {
                    Vector3 lookPos = ClosestEnemy.transform.position - _characterModel.CharacterTransform.position;
                    lookPos.y = 0;
                    Quaternion rotation = Quaternion.LookRotation(lookPos);
                    _characterModel.CharacterTransform.rotation = rotation;
                }
            }
        }

        private void GetClosestEnemy()
        {
            Collider enemy = null;

            float minimalDistance = _characterModel.CharacterCommonSettings.SphereColliderRadius;
            float countDistance = minimalDistance;

            foreach (var collider in _characterModel.EnemiesInTrigger)
            {
                countDistance = Mathf.Sqrt((_characterModel.CharacterTransform.position -
                    collider.transform.position).sqrMagnitude);

                if (countDistance < minimalDistance)
                {
                    minimalDistance = countDistance;
                    enemy = collider;
                }
            }

            ClosestEnemy = enemy;
        }

        #endregion
    }
}


