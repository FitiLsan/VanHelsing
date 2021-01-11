using UnityEngine;
using UnityEngine.UI;

namespace BeastHunter
{
    public class EnemyHealthBarModel
    {

        #region Fields

        private EnemyHealthBarData _enemyHealthBarData;

        private Image _canvasHPImage;
        private GameObject _enemyHealthBarObject;

        #endregion


        #region Propirties

        public Image CanvasHPImage => _canvasHPImage;
        public GameObject EnemyHealthBarObject => _enemyHealthBarObject;

        #endregion

        #region ClassLifeCycle

        public EnemyHealthBarModel(GameObject prefab, EnemyHealthBarData enemyHealthBarData)
        {
            _enemyHealthBarData = enemyHealthBarData;

            Transform healthBar = prefab.transform.Find("EnemyHealthBar");

            Vector3 correctHealthBarPosition = new Vector3(_enemyHealthBarData.HealthBarPosition.x,
                Data.PlayerHealthBarData.HealthBarPosition.y + _enemyHealthBarData.HealthBarPosition.y);
            healthBar.position = healthBar.position + correctHealthBarPosition;
            _enemyHealthBarObject = healthBar.gameObject;
            _enemyHealthBarObject.SetActive(false);

            _canvasHPImage = healthBar.Find("Canvas").Find("Bar").GetComponent<Image>();
            
        }

        #endregion

    }
}
