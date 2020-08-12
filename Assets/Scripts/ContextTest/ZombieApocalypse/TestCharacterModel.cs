using UnityEngine;


namespace BeastHunter
{
    public abstract class TestCharacterModel
    {
        #region Fields
        
        protected readonly Transform _transform;
        protected readonly GameObject _prefab;
        protected Animator _animator;
        protected readonly TestCharacterData _testCharacterData;
        protected TestCharacterStruct _testCharacterStruct;
        protected Vector3 _targetDirection;
        protected static readonly int Run = Animator.StringToHash("Run");

        #endregion


        #region Properties

        public Transform Transform => _transform;

        public Vector3 TargetDirection
        {
            get => _targetDirection;
            set => _targetDirection = value;
        }

        public TestCharacterStruct TestCharacterStruct
        {
            get => _testCharacterStruct;
            set => _testCharacterStruct = value;
        }

        #endregion
        

        #region ClassLifeCycle

        public TestCharacterModel(GameObject prefab, TestCharacterData testCharacterData)
        {
            _prefab = prefab;
            _testCharacterData = testCharacterData;
            _testCharacterStruct = _testCharacterData.TestCharacterStruct;
            _transform = _prefab.transform;
            _transform.position = _testCharacterStruct.StartPosition;
            _animator = _prefab.GetComponent<Animator>();
            
            var skinnedMeshRenderer = _prefab.GetComponentInChildren<SkinnedMeshRenderer>();
            _testCharacterData.ChangeCharacterMaterial(skinnedMeshRenderer, _testCharacterStruct.Material);
        }

        #endregion


        #region Metods

        public void Execute()
        {
            if (_transform.position == _targetDirection)
            {
                _animator.SetBool(Run, false);
            }
            else
            {
                _animator.SetBool(Run, true);
                _testCharacterData.Move(_transform, _targetDirection, _testCharacterStruct.MoveSpeed);
            }
        }

        #endregion
    }
}