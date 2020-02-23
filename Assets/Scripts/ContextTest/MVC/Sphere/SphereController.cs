using BaseScripts;
using UnityEngine;


namespace BeastHunter
{
    public sealed class SphereController : BaseController
    {
        #region Fields

        private readonly GameContext _context;

        #endregion


        #region ClassLifeCycle

        public SphereController(GameContext context, Services services)
        {
            _context = context;
        }

        #endregion


        #region Tick

        public override void Tick()
        {
            _context._sphereModel.Move();
            _context._sphereModel.ChangeBox();
        }

        #endregion


        #region OnAwake


        public override void OnAwake()
        {
            var SphereData = Data.SphereData;
            GameObject instance = GameObject.Instantiate(SphereData.prefab);
            SphereModel Sphere = new SphereModel(instance, SphereData);
            _context._sphereModel = Sphere;
        }

        #endregion        
    }
}


