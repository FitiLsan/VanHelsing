using System.IO;
using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "Data", menuName = "DataTest")]
    public sealed class Data : ScriptableObject
    {
        #region Fields

        public static SphereData _sphereData;

        public static StartDialogueData _startDialogueData;
    

        #endregion


        #region Properties

        public static SphereData SphereData
        {
            get
            {
                if (_sphereData == null)
                {
                    _sphereData = Resources.Load<SphereData>("Data/SphereData");
                }
                return _sphereData;
            }
        }

        public static StartDialogueData StartDialogueData
        {
            get
            {
                if (_startDialogueData == null)
                {
                    _startDialogueData = Resources.Load<StartDialogueData>("Data/StartDialogueData");
                }
                return _startDialogueData;
            }
        }

        #endregion
    }
}