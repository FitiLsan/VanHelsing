using Models;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class StaminaView : MonoBehaviour
    {
        [SerializeField] private float _currentStamina;
        [SerializeField] private float _currentStaminaPercent;
        [SerializeField] private Image _scaleImage;
        private StaminaModel staminaModel;


        private void Awake()
        {
            staminaModel = GameObject.FindGameObjectWithTag("Player").GetComponent<StaminaModel>();
            _scaleImage = GetComponent<Image>();
        }


        private void Update()
        {
            _currentStamina = staminaModel.Stamina;
            StaminaShow();
        }

        private void StaminaShow()
        {
            _currentStaminaPercent = _currentStamina * 0.01f;
            _scaleImage.fillAmount = _currentStaminaPercent;
        }
    }
}