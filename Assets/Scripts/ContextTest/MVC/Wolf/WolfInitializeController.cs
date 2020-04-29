using UnityEngine;


namespace BeastHunter
{
    public sealed class WolfInitializeController : IAwake
    {
        #region Fields

        GameContext _context;

        #endregion


        #region ClassLifeCycle

        public WolfInitializeController(GameContext context, Services services)
        {
            _context = context;
        }

        #endregion


        #region IAwake

        public void OnAwake()
        {
            var WolfData = Data.WolfData;
            WolfData.WolfStruct.SpawnPosition = GameObject.FindGameObjectWithTag("Spawner").transform;
            WolfData.WolfStruct.PatrolWaypointsList.Clear();
            WolfData.WolfStruct.PatrolWaypoints = GameObject.FindGameObjectWithTag("PatrolWayPointsForest");
            foreach(Transform waypoint in WolfData.WolfStruct.PatrolWaypoints.transform)
            {
                WolfData.WolfStruct.PatrolWaypointsList.Add(waypoint.position);
            }
            GameObject instance = GameObject.Instantiate(WolfData.WolfStruct.Prefab, WolfData.WolfStruct.SpawnPosition);
            WolfModel Wolf = new WolfModel(instance, WolfData);
            _context.WolfModel.Add(Wolf);
        }

        #endregion

    }
}

