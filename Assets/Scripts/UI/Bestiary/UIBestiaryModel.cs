using UnityEngine;
using UnityEngine.UI;


namespace BeastHunter
{
    public sealed class UIBestiaryModel
    {
        #region Fields

        public GameContext Context;

        #endregion

        #region Properties

        public Transform BestiaryTransform { get; }
        public UIBestiarySwitcher uiBestiarySwitcher { get; }
        public UIBestiaryData UIBestiaryData { get; }
        public UIBestiaryStruct UIBestiaryStruct { get; }

        #endregion

        #region ClassLifeCycle

        public UIBestiaryModel(GameObject prefab, UIBestiaryData UIbestiaryData, GameContext context)
        {
            UIBestiaryData = UIbestiaryData;
            UIBestiaryStruct = UIbestiaryData.UIBestiaryStruct;
            uiBestiarySwitcher = prefab.GetComponent<UIBestiarySwitcher>();
            uiBestiarySwitcher.uiBestiaryData = UIbestiaryData;
            BestiaryTransform = prefab.transform;
            UIBestiaryData.Model = this;
            Context = context;
        }

        #endregion

        #region Metods

        public void Execute()
        {
            UIBestiaryData.Update();
        }
    
        #endregion
    }
}
