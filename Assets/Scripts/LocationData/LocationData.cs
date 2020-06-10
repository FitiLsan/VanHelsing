using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewData", menuName = "CreateData/LocationData", order = 0)]
public class LocationData : ScriptableObject
{
    #region Fields

    public List<GameObject> SpawnPoints = new List<GameObject>();

    #endregion

    #region Methods

    #endregion
}
