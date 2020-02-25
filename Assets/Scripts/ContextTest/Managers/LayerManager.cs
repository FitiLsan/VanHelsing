using UnityEngine;


namespace BeastHunter
{
    public static class LayerManager
    {
        #region Fields

        private const string IGNORE_RAYCAST = "Ignore Raycast";
        private const string WATER = "Water";
        private const string ENVIRONMENT = "Environment";
        private const string DEFAULT = "Default";
        private const string NONCOLLIDABLE = "Non-Collidable";
        private const string GROUND = "Ground";
        private const string UI = "UI";

        public const int DEFAULT_LAYER = 0;

        #endregion


        #region Properties

        public static int IgnoreRaycastLayer { get; }
        public static int EnvironmentLayer { get; }
        public static int DefaultLayer { get; }
        public static int GroundLayer { get; }
        public static int UiLayer { get; }
        public static int NonCollidableLayer { get; }

        #endregion


        #region ClassLifeCycles

        static LayerManager()
        {
            IgnoreRaycastLayer = LayerMask.GetMask(IGNORE_RAYCAST, WATER, NONCOLLIDABLE);
            EnvironmentLayer = LayerMask.GetMask(ENVIRONMENT, GROUND);
            DefaultLayer = LayerMask.GetMask(DEFAULT);
            GroundLayer = LayerMask.GetMask(GROUND);
            UiLayer = LayerMask.GetMask(UI);
            NonCollidableLayer = LayerMask.NameToLayer(NONCOLLIDABLE);
        }

        #endregion
    }
}
