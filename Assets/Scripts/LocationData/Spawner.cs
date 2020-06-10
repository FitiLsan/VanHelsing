using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    #region Fields

    public Vector3 ObjectVector;
    public List<GameObject> SpawnDayPrefabs;
    public List<GameObject> SpawnNightPrefabs;
    public bool IsNight = false;

    #endregion

    #region Awake

    private void Awake()
    {
        transform.position = ObjectVector;
        if (IsNight == true)
        {
            foreach (GameObject prefabs in SpawnDayPrefabs)
            {
                prefabs.GetComponent<OnAwake>
            }
        }
    }

    #endregion
}
