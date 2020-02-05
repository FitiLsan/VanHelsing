using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ContextTest
{
    public sealed class MainControllers : ControllersStart
    {
        #region ClassLifeCycles

        public MainControllers(GameContext context, Services services)
        {
            Add(new SphereController(context, services, Data.SphereData));
        }

        #endregion
    }
}
