using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewCharacter", menuName = "CreateData/TestCharacter", order = 0)]
    public sealed class TestCharacterData : ScriptableObject
    {
        #region Fields

        [SerializeField]
        private TestCharacterStruct _testCharacterStruct;

        #endregion


        #region Properties

        public TestCharacterStruct TestCharacterStruct => _testCharacterStruct;

        #endregion
        
        
        #region Metods

        public void Move(Transform transform, Vector3 target, float speed)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed);
            transform.rotation = Quaternion.LookRotation(
                Vector3.RotateTowards(transform.forward, target - transform.position, float.MaxValue, 0f)
            );
        }

        public void ChangeCharacterMaterial(SkinnedMeshRenderer skinnedMeshRenderer, Material material)
        {
            if (skinnedMeshRenderer != null && material != null)
            {
                skinnedMeshRenderer.material = material;
            }
        }

        #endregion
    }
}