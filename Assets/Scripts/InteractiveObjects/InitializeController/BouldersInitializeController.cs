using UnityEngine;


namespace BeastHunter
{
    public class BouldersInitializeController : InteractiveObjectInitializeController
    {
        #region ClassLifeCycle

        public BouldersInitializeController(GameContext context) : base(context)
        {
        }

        #endregion


        #region InteractiveObjectInitializeController

        protected override void Initialize()
        {
            var bouldersData = Data.BoulderObjectData;

            if (bouldersData.Prefab == null)
            {
                bouldersData.Prefab = Resources.Load<GameObject>("Boulders");
            }


            # region for scene "Iogan blockout first test leveldesign":

            GameObject gameobject = Object.Instantiate(Data.BoulderObjectData.Prefab, new Vector3(514.99f, 14.172f, 764.55f), Quaternion.identity);
            _context.InteractableObjectModels.Add(gameobject.GetInstanceID(), new BouldersModel(gameobject, Data.BoulderObjectData));

            #endregion


            #region only for scene "BouldersTest":

            //GameObject gameobject = GameObject.Instantiate(Data.BoulderObjectData.Prefab,
            //    new Vector3(-6.70f, 20.52f, 32.1f), Quaternion.Euler(new Vector3(0, -90, 0)));
            //_context.InteractableObjectModels.Add(gameobject.GetInstanceID(), new BouldersModel(gameobject, Data.BoulderObjectData));

            //GameObject gameobject2 = GameObject.Instantiate(Data.BoulderObjectData.Prefab,
            //    new Vector3(-6.70f, 20.52f, 16.08f), Quaternion.Euler(new Vector3(0, -90, 0)));
            //_context.InteractableObjectModels.Add(gameobject2.GetInstanceID(), new BouldersModel(gameobject2, Data.BoulderObjectData));

            //GameObject gameobject3 = GameObject.Instantiate(Data.BoulderObjectData.Prefab,
            //    new Vector3(-6.70f, 20.52f, 1.33f), Quaternion.Euler(new Vector3(0, -90, 0)));
            //_context.InteractableObjectModels.Add(gameobject3.GetInstanceID(), new BouldersModel(gameobject3, Data.BoulderObjectData));

            //GameObject gameobject4 = GameObject.Instantiate(Data.BoulderObjectData.Prefab,
            //    new Vector3(-6.70f, 20.52f, -13.83f), Quaternion.Euler(new Vector3(0, -90, 0)));
            //_context.InteractableObjectModels.Add(gameobject4.GetInstanceID(), new BouldersModel(gameobject4, Data.BoulderObjectData));

            //GameObject gameobject5 = GameObject.Instantiate(Data.BoulderObjectData.Prefab,
            //    new Vector3(-6.70f, 20.52f, -29.19f), Quaternion.Euler(new Vector3(0, -90, 0)));
            //_context.InteractableObjectModels.Add(gameobject5.GetInstanceID(), new BouldersModel(gameobject5, Data.BoulderObjectData));

            //GameObject gameobject6 = GameObject.Instantiate(Data.BoulderObjectData.Prefab,
            //    new Vector3(6.70f, 20.52f, -29.19f), Quaternion.Euler(new Vector3(0, 90, 0)));
            //_context.InteractableObjectModels.Add(gameobject6.GetInstanceID(), new BouldersModel(gameobject6, Data.BoulderObjectData));

            //GameObject gameobject7 = GameObject.Instantiate(Data.BoulderObjectData.Prefab,
            //    new Vector3(6.70f, 20.52f, -13.83f), Quaternion.Euler(new Vector3(0, 90, 0)));
            //_context.InteractableObjectModels.Add(gameobject7.GetInstanceID(), new BouldersModel(gameobject7, Data.BoulderObjectData));

            //GameObject gameobject8 = GameObject.Instantiate(Data.BoulderObjectData.Prefab,
            //    new Vector3(6.70f, 20.52f, 1.33f), Quaternion.Euler(new Vector3(0, 90, 0)));
            //_context.InteractableObjectModels.Add(gameobject8.GetInstanceID(), new BouldersModel(gameobject8, Data.BoulderObjectData));

            //GameObject gameobject9 = GameObject.Instantiate(Data.BoulderObjectData.Prefab,
            //    new Vector3(6.70f, 20.52f, 16.08f), Quaternion.Euler(new Vector3(0, 90, 0)));
            //_context.InteractableObjectModels.Add(gameobject9.GetInstanceID(), new BouldersModel(gameobject9, Data.BoulderObjectData));

            //GameObject gameobject10 = GameObject.Instantiate(Data.BoulderObjectData.Prefab,
            //    new Vector3(6.70f, 20.52f, 32.1f), Quaternion.Euler(new Vector3(0, 90, 0)));
            //_context.InteractableObjectModels.Add(gameobject10.GetInstanceID(), new BouldersModel(gameobject10, Data.BoulderObjectData));

            #endregion
        }

        #endregion
    }
}

