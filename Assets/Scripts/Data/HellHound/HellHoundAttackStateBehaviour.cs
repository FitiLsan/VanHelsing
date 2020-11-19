using System;
using UnityEngine;

namespace BeastHunter
{
    class HellHoundAttackStateBehaviour : StateMachineBehaviour
    {
        public event Action OnStateExitHandler;

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            OnStateExitHandler?.Invoke();
        }
    }
}
