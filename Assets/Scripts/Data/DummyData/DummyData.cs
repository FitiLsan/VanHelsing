using UnityEngine;
using DG.Tweening;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "NewDummy", menuName = "CreateData/Dummy", order = 2)]
    public sealed class DummyData : EnemyData
    {
        #region Fields

        [SerializeField] private GameObject _damageTextObject;
        [SerializeField] private float _dummyHeight;
        [SerializeField] private float _damageTextTime;
        [SerializeField] private float _damageTextHeight;
        [SerializeField] private float _pushAngle;
        [SerializeField] private float _rotationAngle;
        [SerializeField] private float _pushTime;
        [SerializeField] private float _reviveTime;

        #endregion


        #region Properties

        public GameObject DamageTextObject => _damageTextObject;
        public float DummyHeight => _dummyHeight;

        #endregion


        #region Methods

        public override void Act(EnemyModel enemyModel)
        {
            // todo
        }

        public void CreateDamageObject(Damage damage, DummyModel model)
        {
            model.RefreshDamageTextSequence(true);
            model.DamageTextObject.position = model.ObjectOnScene.transform.position +
                Vector3.up * _dummyHeight;
            model.DamageText.text = Mathf.Floor(damage.GetTotalDamage()).ToString();
            model.DamageText.color = model.DamageTextColor;
            model.DamageTextSequence.Append(model.DamageTextObject.DOMove(model.DamageTextObject.position + 
                Vector3.up * _damageTextHeight, _damageTextTime));
            model.DamageTextSequence.Join(model.DamageText.DOColor(Color.clear, _damageTextTime));
            model.DamageTextSequence.OnUpdate(() => model.DamageTextObject.LookAt(Services.SharedInstance.CameraService.
                CurrentActiveCamera.Value.transform));
            PushDummy(model);
        }

        public void PushDummy(DummyModel model)
        {
            model.RefreshDummyPushSeqnece(false);
            float xRotation = Random.Range(-_pushAngle, _pushAngle);
            float zRotation = Random.Range(-_pushAngle, _pushAngle);
            float yRotation = Random.Range(-_rotationAngle, _rotationAngle);
            model.DummyPushSequence.Append(model.ObjectOnScene.transform.
                DORotate(new Vector3(xRotation, yRotation, zRotation), _pushTime));
            model.DummyPushSequence.Append(model.ObjectOnScene.transform.
                DORotate(new Vector3(0f, yRotation, 0f), _reviveTime));
        }

        #endregion
    }
}

