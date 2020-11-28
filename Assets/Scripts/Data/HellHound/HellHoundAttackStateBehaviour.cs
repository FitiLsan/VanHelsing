using System;
using UnityEngine;

namespace BeastHunter
{
    class HellHoundAttackStateBehaviour : StateMachineBehaviour
    {
        public event Action OnStateEnterHandler;
        public event Action OnStateExitHandler;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            OnStateEnterHandler?.Invoke();
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            OnStateExitHandler?.Invoke();
        }
    }
}
