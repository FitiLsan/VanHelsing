using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    public sealed class BossSkills
    {
        #region Constants

        private const float LOOK_TO_TARGET_SPEED = 1f;
        private const float PART_OF_NONE_ATTACK_TIME_LEFT = 0.15f;
        private const float PART_OF_NONE_ATTACK_TIME_RIGHT = 0.3f;
        private const float ANGLE_SPEED = 150f;
        private const float ANGLE_TARGET_RANGE_MIN = 20f;
        private const float DISTANCE_TO_START_ATTACK = 4f;
        private const float DELAY_HAND_TRIGGER = 0.2f;

        private const int DEFAULT_ATTACK_ID = 0;
        private const float DEFAULT_ATTACK_RANGE_MIN = 3f;
        private const float DEFAULT_ATTACK_RANGE_MAX = 5f;
        private const float DEFAULT_ATTACK_COOLDOWN = 3f; //3
        private const bool DEFAULT_ATTACK_READY = true;

        private const int HORIZONTAL_FIST_ATTACK_ID = 1;
        private const float HORIZONTAL_FIST_ATTACK_RANGE_MIN = 3f;
        private const float HORIZONTAL_FIST_ATTACK_RANGE_MAX = 5f;
        private const float HORIZONTAL_FIST_ATTACK_COOLDOWN = 7f; //20f
        private const bool HORIZONTAL_ATTACK_READY = true;

        private const int STOMP_SPLASH_ATTACK_ID = 2;
        private const float STOMP_SPLASH_ATTACK_RANGE_MIN = 0f;
        private const float STOMP_SPLASH_ATTACK_RANGE_MAX = 5f;
        private const float STOMP_SPLASH_ATTACK_COOLDOWN = 15f;
        private const bool STOMP_ATTACK_READY = false;

        private const int RAGE_OF_FOREST_ATTACK_ID = 3;
        private const float RAGE_OF_FOREST_ATTACK_RANGE_MIN = 10f;
        private const float RAGE_OF_FOREST_ATTACK_RANGE_MAX = 30f;
        private const float RAGE_OF_FOREST_ATTACK_COOLDOWN = 30f; //120
        private const bool RAGE_OF_FOREST_ATTACK_READY = false;

        private const int POISON_SPORES_ATTACK_ID = 4;
        private const float POISON_SPORES_ATTACK_RANGE_MIN = 10f;
        private const float POISON_SPORES_ATTACK_RANGE_MAX = 20f;
        private const float POISON_SPORES_ATTACK_COOLDOWN = 2f; //20
        private const bool POISON_ATTACK_READY = false;


        #endregion

        private BossStateMachine _stateMachine;
        public Dictionary<int, BaseSkill> _attackStateSkillDictionary { get; private set; } = new Dictionary<int, BaseSkill>();
        public Dictionary<int, int> _readyAttackSkillDictionary { get; private set; } = new Dictionary<int, int>();

        public BaseSkill _defaultSkill { get; private set; }
        public BaseSkill _horizontalAttackSkill { get; private set; }
        public BaseSkill _stompSplashSkill { get; private set; }
        public BaseSkill _rageOfForestSkill { get; private set; }
        public BaseSkill _poisonSporesSkill { get; private set; }


        public BossSkills(BossStateMachine stateMachine)
        {
            _stateMachine = stateMachine;

            _defaultSkill = new DefaultSkill(DEFAULT_ATTACK_ID, DEFAULT_ATTACK_RANGE_MIN, DEFAULT_ATTACK_RANGE_MAX, DEFAULT_ATTACK_COOLDOWN, DEFAULT_ATTACK_READY, _attackStateSkillDictionary, _stateMachine);
            _horizontalAttackSkill = new HorizontalAttackSkill(HORIZONTAL_FIST_ATTACK_ID, HORIZONTAL_FIST_ATTACK_RANGE_MIN, HORIZONTAL_FIST_ATTACK_RANGE_MAX, HORIZONTAL_FIST_ATTACK_COOLDOWN, HORIZONTAL_ATTACK_READY, _attackStateSkillDictionary, _stateMachine);
            _stompSplashSkill = new StompSplashAttackSkill(STOMP_SPLASH_ATTACK_ID, STOMP_SPLASH_ATTACK_RANGE_MIN, STOMP_SPLASH_ATTACK_RANGE_MAX, STOMP_SPLASH_ATTACK_COOLDOWN, STOMP_ATTACK_READY, _attackStateSkillDictionary, _stateMachine);
            _rageOfForestSkill = new RageOfForestAttackSkill(RAGE_OF_FOREST_ATTACK_ID, RAGE_OF_FOREST_ATTACK_RANGE_MIN, RAGE_OF_FOREST_ATTACK_RANGE_MAX, RAGE_OF_FOREST_ATTACK_COOLDOWN, RAGE_OF_FOREST_ATTACK_READY, _attackStateSkillDictionary, _stateMachine);
            _poisonSporesSkill = new PoisonSporesAttackSkill(POISON_SPORES_ATTACK_ID, POISON_SPORES_ATTACK_RANGE_MIN, POISON_SPORES_ATTACK_RANGE_MAX, POISON_SPORES_ATTACK_COOLDOWN, POISON_ATTACK_READY, _attackStateSkillDictionary, _stateMachine);

            _attackStateSkillDictionary.Add(_defaultSkill.AttackId, _defaultSkill);
            _attackStateSkillDictionary.Add(_horizontalAttackSkill.AttackId, _horizontalAttackSkill);
            _attackStateSkillDictionary.Add(_stompSplashSkill.AttackId, _stompSplashSkill);
            _attackStateSkillDictionary.Add(_rageOfForestSkill.AttackId, _rageOfForestSkill);
            _attackStateSkillDictionary.Add(_poisonSporesSkill.AttackId, _poisonSporesSkill);
        }
    }
}