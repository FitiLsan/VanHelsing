﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{
    public class EnemyController : IAwake, IUpdate, ITearDown
    {
        #region Fields

        private readonly GameContext _context;

        #endregion


        #region ClassLifeCycles

        public EnemyController(GameContext context)
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
                //npcBehaviour.Stats = _context.NpcModels[npcBehaviour.GameObject.GetInstanceID()].GetStats().BaseStats;
                npcBehaviour.SetDoSmthEvent(DoSmth);
                npcBehaviour.SetTakeDamageEvent(TakeDamage);
                npcBehaviour.SetDealDamageEvent(DealDamage);
            }
        }

        #endregion


        #region IUpdate

        public void Updating()
        {
            foreach (var npc in _context.EnemyModels.Values)
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
                var npcBehavior = trigger as InteractableObjectBehavior;
                

                npcBehavior.DeleteDoSmthEvent(DoSmth);
                npcBehavior.DeleteTakeDamageEvent(TakeDamage);
                npcBehavior.DeleteDealDamageEvent(DealDamage);
            }
        }

        #endregion


        #region Methods

        public void DoSmth(int gameObjectId, string how)
        {
            _context.EnemyModels[gameObjectId].DoSmth(how + gameObjectId);
        }

        public void TakeDamage(int gameObjectId, Damage damage)
        {
            _context.EnemyModels[gameObjectId].TakeDamage(damage);
        }

        public void DealDamage(int gameObjectId, InteractableObjectBehavior enemy, Damage damage)
        {
            //BaseStatsClass enemyStats = _context.NpcModels[enemy.GameObject.GetInstanceID()].GetStats().BaseStats;
            //or BaseStatsClass enemyStats = _context.PlayerModel.GetStats().BaseStats;
            //BaseStatsClass stats = _context.NpcModels[gameObjectId].GetStats().BaseStats;
            //Services.SharedInstance.AttackService.CountDamage();
            if (enemy != null && damage != null)
            {
                enemy.TakeDamageEvent(damage);
            }
        }

        #endregion
    }
}

