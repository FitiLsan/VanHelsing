using BaseScripts;
using UnityEngine;

namespace ContextTest
{
    public class SphereController : BaseController
    {
        #region Fields
        public SphereModel Model { get; private set; }

        public SphereView View { get; private set; }

        private readonly GameContext _context;
        #endregion


        #region ClassLifeCycle

        public SphereController(GameContext context, Services services, SphereData spheredata)
        {
            _context = context;
            GameObject instance = GameObject.Instantiate(spheredata.prefab);
            View = new SphereView(); //UI
            Model = new SphereModel(instance, spheredata);
        }

        #endregion


        #region Tick

        public override void Tick()
        {
            Move();
            ChangeBox();
        }

        #endregion


        #region OnAwake


        public override void OnAwake()
        {

        }

        #endregion


        #region Metods

        void Move()
        {
            Model.SphereTransform.position = Vector3.MoveTowards
                (Model.SphereTransform.position,
                Model.SphereTarget.transform.position,
                Model.SphereSpeed);
        }

        void ChangeBox()
        {
            Model.SphereRadius.radius = 0.5f;
        }

        #endregion
    }
}


