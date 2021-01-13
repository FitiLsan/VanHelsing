using UnityEngine;

namespace BeastHunter
{
    [CreateAssetMenu(fileName = "EnemyHealthBarData")]
    public class EnemyHealthBarData : ScriptableObject
    {
        #region SerializedFields

        [Header("Enemy health bar")]
        [Tooltip("Default: x:30.0f, y:50.0f")]
        [SerializeField] private Vector2 _healthBarPosition = new Vector2(30f, 50f);

        #endregion



        #region Properties

        public Vector2 HealthBarPosition => _healthBarPosition;
        #endregion

    }
}
