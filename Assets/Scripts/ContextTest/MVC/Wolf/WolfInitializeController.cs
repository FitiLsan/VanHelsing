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
            GameObject instance = GameObject.Instantiate(WolfData.WolfStruct.Prefab);
            WolfModel Wolf = new WolfModel(instance, WolfData);
            _context._wolfModel = Wolf;
        }

        #endregion

    }
}

