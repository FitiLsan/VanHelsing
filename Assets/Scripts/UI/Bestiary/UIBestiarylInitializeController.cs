using UnityEngine;

namespace BeastHunter
{
    public class UIBestiarylInitializeController : IAwake
    {
        #region Properties

        GameContext _context;

        #endregion

        #region ClassLifeCycle

        public UIBestiarylInitializeController(GameContext context)
        {
            _context = context;
        }

        #endregion

        #region Methods

        public void OnAwake()
        {
            var UIbestiaryData = Data.UIBestiaryData;
            GameObject instance = GameObject.Instantiate(UIbestiaryData.UIBestiaryStruct.Prefab);
            UIBestiaryModel uiBestiary = new UIBestiaryModel(instance, UIbestiaryData, _context);
            _context.UIBestiaryModel = uiBestiary;

            if (UIbestiaryData.UIBestiaryStruct.BossDescription.Count > UIbestiaryData.UIBestiaryStruct.BossImage.Count)
            {
                UIbestiaryData.UIBestiaryStruct.BossDescription.RemoveAt(UIbestiaryData.UIBestiaryStruct.BossDescription.Count);
            } 
            else if (UIbestiaryData.UIBestiaryStruct.BossDescription.Count < UIbestiaryData.UIBestiaryStruct.BossImage.Count)
            {
                UIbestiaryData.UIBestiaryStruct.BossImage.RemoveAt(UIbestiaryData.UIBestiaryStruct.BossImage.Count);
            }

            UIbestiaryData.PageCount = UIbestiaryData.UIBestiaryStruct.BossDescription.Count;
            Services.SharedInstance.EventManager.TriggerEvent(GameEventTypes.BestiaryCreated, null);
        }

        #endregion
    }
}
