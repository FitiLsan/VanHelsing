using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace BeastHunter
{ 
    [Serializable]
    public struct UIBestiaryStruct
    {
        #region Fields

        [TextArea(5, 15)]
        public List<string> BossDescription;
        public List<Sprite> BossImage;

        public GameObject Prefab;

        #endregion
    }
}
