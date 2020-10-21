using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "MaterialsData")]
    public sealed class MaterialsData : ScriptableObject
    {
        #region Fields

        [SerializeField] private Material _metalLockTrapMaterial;
        [SerializeField] private Material _transparentMetalLockTrapMaterial;
        [SerializeField] private Material _spoonTrapWoodMaterial;
        [SerializeField] private Material _spoonTrapHoldersMaterial;
        [SerializeField] private Material _acidMaterial;

        #endregion


        #region Properties

        public Material MetalLockTrapMaterial => _metalLockTrapMaterial;
        public Material TransparentMetalLockTrapMaterial => _transparentMetalLockTrapMaterial;
        public Material SpoonTrapWoodMaterial => _spoonTrapWoodMaterial;
        public Material SpoonTrapHoldersMaterial => _spoonTrapHoldersMaterial;
        public Material AcidMaterial => _acidMaterial;

        #endregion
    }
}

