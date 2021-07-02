﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{
    public class IOInitializeController : IAwake
    {
        #region Fields

        protected GameContext _context;

        #endregion

        #region ClassLifeCycles

        public IOInitializeController(GameContext context)
        {
            _context = context;
        }

        #endregion


        #region IAwake

        public void OnAwake() // link with LocationData
        {
            var data = Resources.Load<IOData>("Data/IOData");
            GameObject instance = GameObject.Instantiate(data.BaseStats.Prefab);
            IOModel npc = new IOModel(instance, data);
            _context.EnemyModels.Add(instance.GetInstanceID(), npc);

            GameObject instance2 = GameObject.Instantiate(data.BaseStats.Prefab, new Vector3(2.0f, 1.5f, 2.0f), Quaternion.identity);
            IOModel npc2 = new IOModel(instance2, data);
            _context.EnemyModels.Add(instance2.GetInstanceID(), npc2);
        }

        #endregion
    }
}
