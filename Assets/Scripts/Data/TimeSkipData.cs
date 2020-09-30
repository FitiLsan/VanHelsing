using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "TimeSkipData")]
    public sealed class TimeSkipData : ScriptableObject
    {
        #region Fields

        public GameObject UIPrefab;

        #endregion


        #region Methods

        public void WaitTill(int day, int time)
        {
            //TODO
        }

        #endregion
    }
}
