using UnityEngine;
using UnityEngine.UI;


namespace BeastHunter
{
    public sealed class EffectTester : MonoBehaviour
    {
        #region Fields

        public BaseBuff buff;
        private Image bg;
        private Text _buffName;
        private new Transform camera;

        #endregion


        #region UnityMethods

        private void Awake()
        {
            bg = GetComponentInChildren<Image>();
            _buffName = GetComponentInChildren<Text>();
        }

        private void Start()
        {
            _buffName.text = $"{buff.Name} ({buff.Type})";
            camera = Services.SharedInstance.CameraService.CharacterCamera?.transform;
        }

        private void Update()
        {        
            if (camera != null)
            {
                bg.transform.LookAt(camera);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            var obj = other.GetComponent<InteractableObjectBehavior>();
            if (obj != null && !other.isTrigger)
            {
                if (obj.Type == InteractableObjectType.Player || obj.Type == InteractableObjectType.Enemy)
                {
                    var instanceID = other.transform.root.gameObject.GetInstanceID();

                    if (buff is TemporaryBuff)
                    {
                        Services.SharedInstance.BuffService.AddTemporaryBuff(instanceID, buff as TemporaryBuff);
                    }
                    else
                    {
                        Services.SharedInstance.BuffService.AddPermanentBuff(instanceID, buff as PermanentBuff);
                    }
                    gameObject.SetActive(false);
                    DG.Tweening.DOVirtual.DelayedCall(3, () => gameObject.SetActive(true));
                }
            }
        }

        #endregion
    }
}