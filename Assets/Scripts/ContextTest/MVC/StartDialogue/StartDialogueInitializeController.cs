using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{
    public sealed class StartDialogueInitializeController : IAwake
    {
        #region Field

        GameContext _context;

        #endregion

        #region ClassLifeCycle

        public StartDialogueInitializeController(GameContext context, Services services)
        {
            _context = context;
        }

        #endregion

        #region IAwake

        public void OnAwake()
        {
            var StartDialogueData = Data.StartDialogueData;
            GameObject instance = GameObject.Instantiate(StartDialogueData.StartDialogueStruct.Prefab,StartDialogueData.StartDialogueStruct.PlayerTransform);
            StartDialogueModel StartDialogue = new StartDialogueModel(instance, StartDialogueData, _context);
            _context._startDialogueModel = StartDialogue;
        }

        #endregion
    }
}
