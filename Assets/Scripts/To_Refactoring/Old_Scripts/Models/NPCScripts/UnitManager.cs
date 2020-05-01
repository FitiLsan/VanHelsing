using System.Collections.Generic;
using UnityEngine;

namespace Models.NPCScripts
{
    public class UnitManager : MonoBehaviour
    {
        private Dictionary<string, Enemy.Enemy> enemyPull;
        private int id;
        private float timer;

        private void Awake()
        {
            CreatePull();
            foreach (var a in enemyPull.Values)
            {
                a.EnemyAwake();
                a.SetDieMethod(DeactivateUnit);
            }
        }

        private void FixedUpdate()
        {
            var deltaTime = Time.deltaTime;
            foreach (var a in enemyPull.Values) a.EnemyUpdate(deltaTime);
        }

        private void CreatePull()
        {
            id = 1;
            enemyPull = new Dictionary<string, Enemy.Enemy>();
            var startEnemies = FindObjectsOfType<Enemy.Enemy>();
            foreach (var a in startEnemies)
            {
                a.name = "Проклятый охотник" + id;
                enemyPull.Add(a.name, a);
                id++;
            }
        }

        private void DeactivateUnit(string unitName)
        {
            enemyPull[unitName].gameObject.SetActive(false);
        }
    }
}