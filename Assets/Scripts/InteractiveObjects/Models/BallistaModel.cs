using UniRx;
using UnityEngine;


namespace BeastHunter
{
    public sealed class BallistaModel : SimpleInteractiveObjectModel, IAwake, ITearDown
    {
        #region Fields
        private const float VERTICAL_ROTATE_MODIFIER = 50f;
        private const float HORIZONTAL_ROTATE_MODIFIER = 37.5f;

        public Rigidbody BoltRb;
        public GameObject Bolt;

        public Transform BoltPointTransform;
        public Transform HorizontalTurn;
        public Transform VerticalTurn;
        
        private bool _isLoad;
        private bool _isLoading;
        private bool _isShooting;

        private Animator _animator;
        private InputModel _inputModel;
        private BallistaData _ballistaData;
        private GameContext _context;
        private bool _isMoving;
        private BoltController _bolt;

        #endregion

        #region Properties

        public BallistaAnimationController BallistaAnimationController { get; private set; }
        public bool IsActive { get; set; }
        #endregion


        #region ClassLifeCycle

        public BallistaModel(GameObject prefab, BallistaData data, GameContext context) : base(prefab, data)
        {
            BallistaAnimationController = prefab.GetComponent<BallistaAnimationController>();
            BoltRb = BallistaAnimationController.BoltRb;
            Bolt = BallistaAnimationController.Bolt;
            BoltPointTransform = BallistaAnimationController.BoltPointTransform;
            HorizontalTurn = BallistaAnimationController.HorizontalTurn;
            VerticalTurn = BallistaAnimationController.VerticalTurn;
            _bolt = BallistaAnimationController.Bolt.GetComponent<BoltController>();
            _animator = prefab.GetComponent<Animator>();
            IsNeedControl = true;
            MessageBroker.Default.Receive<PlayerInteractionCatch>().Subscribe(data.CallTriggerInteraction);
            _context = context;
            context.CharacterModel.StartControlEvent += AttachListeners;
            context.CharacterModel.StopControlEvent += DetachListeners;
        }

        #endregion

        #region Methods

        public void Reload()
        {
            if (_isLoad || _isShooting || _isLoading)
            {
                return;
            }
            _isLoading = true;
            _animator.SetTrigger("StartLoad");
            DG.Tweening.DOVirtual.DelayedCall(2.5f, OnLoading);
            void OnLoading()
            {
                _isLoad = true;
                _isLoading = false;
                _animator.SetBool("isLoad", _isLoad);
            }
        }
        public void Shoot()
        {
            if (!_isLoad || _isLoading || _isShooting)
            {
                return;
            }
            _bolt._isActive = true;
            _isShooting = true;
            _isLoad = false;
            _animator.SetTrigger("Shoot");
            BoltRb.isKinematic = false;
            BoltRb.AddForce(BoltRb.transform.up * -1 * 100f, ForceMode.Impulse);
            BoltRb.transform.SetParent(null);
            DG.Tweening.DOVirtual.DelayedCall(1f, () => _animator.SetBool("isLoad", _isShooting = false));
            DG.Tweening.DOVirtual.DelayedCall(0.5f, () => SetNewBolt());
        }

        private void SetNewBolt()
        {
            var newBolt = GameObject.Instantiate(Bolt, BoltPointTransform.position, BoltPointTransform.rotation, BoltPointTransform);
            BoltRb = newBolt.GetComponent<Rigidbody>();
            BoltRb.isKinematic = true;
            _bolt = newBolt.GetComponent<BoltController>();
            _bolt._isActive = false;
        }

        private void VertialTurnRotation(float direction)
        {
            if (_isMoving)
            {
                VerticalTurn.rotation *= Quaternion.Euler(direction * VERTICAL_ROTATE_MODIFIER * Time.deltaTime, 0, 0);
            }
        }

        private void HorizontalTurnRotation(float direction)
        {
            if (_isMoving)
            {
                HorizontalTurn.rotation *= Quaternion.Euler(direction * HORIZONTAL_ROTATE_MODIFIER * Time.deltaTime, 0, 0);
            }
        }

        private void AttachListeners()
        {
            if (IsActive)
            {
                _inputModel.OnAttack += () => Shoot();
                _inputModel.OnAttack += () => Reload();
                _isMoving = true;
            }
        }

        private void DetachListeners()
        {
            _inputModel.OnAttack -= () => Shoot();
            _inputModel.OnAttack -= () => Reload();
            _isMoving = false;
            _bolt._isActive = false;
        }

        public void OnAwake()
        {
            _inputModel = _context.InputModel;
        }

        public override void Updating()
        {
            base.Updating();
            VertialTurnRotation(_inputModel.InputTotalAxisY);
            HorizontalTurnRotation(_inputModel.InputTotalAxisX);
        }

        public void TearDown()
        {
            _context.CharacterModel.StartControlEvent -= AttachListeners;
            _context.CharacterModel.StopControlEvent -= DetachListeners;
        }

        #endregion
    }
}

