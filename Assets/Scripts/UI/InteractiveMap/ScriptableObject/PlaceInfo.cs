using UnityEngine;
using UnityEngine.UI;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewPlace", menuName = "CreateModel/PlaceInfo", order = 0)]
    public class PlaceInfo : ScriptableObject
    {
        public int PlaceId;
        public string PlaceName;
        public Image PlaceImage;
        [TextArea(3,30)]
        public string PlaceDescription;
        public (int, int) PlaceCoordinates; 
    }
}