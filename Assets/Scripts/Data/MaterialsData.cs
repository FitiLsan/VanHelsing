using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "MaterialsData", menuName = "MainData/MaterialData")]
    public sealed class MaterialsData : ScriptableObject
    {
        #region Fields

        [SerializeField] private Material _metalLockTrapMaterial;
        [SerializeField] private Material _transparentMetalLockTrapMaterial;

        #endregion


        #region Properties

        public Material MetalLockTrapMaterial => _metalLockTrapMaterial;
        public Material TransparentMetalLockTrapMaterial => _transparentMetalLockTrapMaterial;

        #endregion
    }
}

