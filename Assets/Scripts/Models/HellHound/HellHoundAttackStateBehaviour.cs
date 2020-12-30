using System;
using UnityEngine;

namespace BeastHunter
{
    /// <summary>tracking the start and end of animations (script is attached to animations)</summary>
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
