using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{
    public class NpcController : IAwake, IUpdate, ITearDown
    {
        #region Fields

        private readonly GameContext _context;

        #endregion


        #region ClassLifeCycles

        public NpcController(GameContext context)
        {
            _context = context;
        }

        #endregion


        #region IAwake

        public void OnAwake()
        {
            var npc = _context.GetListInteractable();
            foreach (var trigger in npc)
            {
                var npcBehaviour = trigger as InteractableObjectBehavior;
                npcBehaviour.SetDoSmthEvent(DoSmth);
            }
        }

        #endregion


        #region IUpdate

        public void Updating()
        {
            foreach (var npc in _context.NpcModels.Values)
            {
                npc.Execute();
            }
        }

        #endregion


        #region ITearDownController

        public void TearDown()
        {
            var npc = _context.GetListInteractable();
            foreach (var trigger in npc)
            {
                var npcBehaviour = trigger as InteractableObjectBehavior;
                npcBehaviour.DeleteDoSmthEvent(DoSmth);
            }
        }

        #endregion

        public void DoSmth(int gameObjectId, string how)
        {
            _context.NpcModels[gameObjectId].DoSmth(how + gameObjectId);
        }
    }
}

