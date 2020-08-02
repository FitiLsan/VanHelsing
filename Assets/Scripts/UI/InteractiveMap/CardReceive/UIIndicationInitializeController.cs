using UnityEngine;


namespace BeastHunter
{
    public class UIIndicationInitializeController : IAwake
    {
        #region Field

        GameContext _context;
        GameObject _parent;

        #endregion


        #region ClassLifeCycle

        public UIIndicationInitializeController(GameContext context)
        {
            _context = context;
            _parent = GameObject.Find("MapPanel");
        }

        #endregion


        #region IAwake

        public void OnAwake()
        {
            var UIIndicationData = Data.UIIndicationData;
            GameObject instance = GameObject.Instantiate(UIIndicationData.UIIndicationStruct.Prefab, _parent.transform);

            UIIndicationModel UIIndication = new UIIndicationModel(instance, UIIndicationData, _context);
            _context.UIIndicationModel = UIIndication;
        }
        #endregion
    }
}