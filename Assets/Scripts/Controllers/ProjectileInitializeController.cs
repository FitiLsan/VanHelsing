using UnityEngine;


namespace BeastHunter
{
    public class ProjectileInitializeController : MonoBehaviour
    {
        #region Field

        private readonly GameContext _context;
        private readonly ProjectileData _projectileData;

        #endregion


        #region ClassLifeCycle

        public ProjectileInitializeController(GameContext context, ProjectileData projectileData, 
            Vector3 position, Vector3 forceVector, ForceMode forceMode = ForceMode.Impulse)
        {
            _context = context;
            _projectileData = projectileData;
            InstantiateProjectile(position, forceVector, forceMode);
        }

        #endregion


        #region Methods

        private void InstantiateProjectile(Vector3 position, Vector3 forceVector, ForceMode forceMode)
        {
            _projectileData.Launch(_context, position, forceVector, forceMode);
        }

        #endregion
    }
}

