using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunter
{
    public class HardBark : BossBaseSkill
    {
        private bool _isAllowed;

        public HardBark((bool, int, float, float, float, bool, bool) skillInfo, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine) 
            : base(skillInfo, skillDictionary, stateMachine)
        {
        }

        //public HardBark(int Id, float RangeMin, float RangeMax, float Cooldown, bool IsReady, Dictionary<int, BossBaseSkill> skillDictionary, BossStateMachine stateMachine) 
        //    : base(Id, RangeMin, RangeMax, Cooldown, IsReady, skillDictionary, stateMachine)
        //{
        //}

        public override void StopSkill()
        {
            
        }

        public override void UseSkill(int id)
        {
            if (_isAllowed && _skillDictionary[id].IsSkillReady)
            {
                Debug.Log("Hard Bark Skill");
                _bossData.SetNavMeshAgent(_bossModel, _bossModel.BossNavAgent, _bossModel.BossTransform.position, 0);
                _bossModel.BossAnimator.Play($"HardBark", 0, 0f);

                SwitchBark(true);

                DelayCall(() => SwitchBark(false), 15f, out var call);
                ReloadSkill(id);
                SwitchAllowed(false);
            }
        }

        private void SwitchBark(bool isOn)
        {
            if (isOn)
            {
                _bossModel.barkBuffEffect.Play();
                //BuffService + def  - attackSpeed
            }
            else
            {
                _bossModel.barkBuffEffect.Stop();
            }
        }     
        internal override void SwitchAllowed(bool isOn)
        {
            _isAllowed = isOn;
        }
    }
}