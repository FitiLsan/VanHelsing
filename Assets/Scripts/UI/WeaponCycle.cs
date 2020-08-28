using UnityEngine;
using UnityEngine.UI;


namespace BeastHunter
{
    public class WeaponCycle : MonoBehaviour
    {
        #region Constants

        private const float DISTANCE_TO_ZERO = 14f;

        #endregion


        #region Fields

        private Text _mainText;
        private float _distance;

        #endregion


        #region Methods

        private void Awake()
        {
            _mainText = GetComponentInChildren<Text>();
        }

        void Update()
        {
            _distance = Vector3.Distance(transform.localPosition, Vector3.zero);

            if(_distance <= DISTANCE_TO_ZERO)
            {
                _mainText.text = "";
            }
        }

        #endregion
    }
}

