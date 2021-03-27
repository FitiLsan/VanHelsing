using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BeastHunter
{
    public class EffectTester : MonoBehaviour
    {    
        public BaseBuff buff;
        private Image bg;
        private Text _buffName;

        private void Awake()
        {
            bg = GetComponentInChildren<Image>();
            _buffName = GetComponentInChildren<Text>();
        }

        private void Start()
        {
            _buffName.text = $"{buff.Name} ({buff.Type})";
            
        }

        private void Update()
        {
            var camera = Services.SharedInstance.CameraService.CharacterCamera.transform;
            if (camera != null)
            {
                bg.transform.LookAt(camera);
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            var obj = other.GetComponent<InteractableObjectBehavior>();
            if (obj != null && !other.isTrigger &&  obj.Type == InteractableObjectType.Enemy) //obj.Type == InteractableObjectType.Player ||
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
            }
         

        }
    }
}