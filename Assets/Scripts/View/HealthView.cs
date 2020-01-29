using Models;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class HealthView : MonoBehaviour
    {
        [SerializeField] private float _currentHealth;
        [SerializeField] private float _currentHealthPercent;
        [SerializeField] private Image _HealthScaleImage;
        private HealthModel healthModel;

        private void Awake()
        {
            healthModel = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthModel>();
            _HealthScaleImage = GetComponent<Image>();
        }


        private void Update()
        {
            _currentHealth = healthModel.health;
            HealthShow();
        }

        private void HealthShow()
        {
            _currentHealthPercent = _currentHealth * 0.01f;
            _HealthScaleImage.fillAmount = _currentHealthPercent;
            ;
        }
    }
}