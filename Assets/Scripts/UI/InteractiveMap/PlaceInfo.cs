using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewPlace", menuName = "CreateModel/PlaceInfo", order = 0)]
    public class PlaceInfo : ScriptableObject
    {
        public int PlaceId;
        public string PlaceName;
    }
}