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

        public UIBestiaryData UIBestiaryData { get; }
        public UIBestiaryStruct UIBestiaryStruct { get; }

        #endregion

        #region ClassLifeCycle

        public UIBestiaryModel(GameObject prefab, UIBestiaryData UIbestiaryData, GameContext context)
        {
            UIBestiaryData = UIbestiaryData;
            UIBestiaryStruct = UIbestiaryData.UIBestiaryStruct;
            UIBestiaryData.Model = this;
            Context = context;
        }

        #endregion

        #region Metods

        public void Execute()
        {
        }
    
        #endregion
    }
}
