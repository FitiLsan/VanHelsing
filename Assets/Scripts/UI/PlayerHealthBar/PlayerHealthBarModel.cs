using UnityEngine;
using UnityEngine.UI;


namespace BeastHunter
{
    public class PlayerHealthBarModel
    {
        #region Fields

        private HealthSection[] _healthSections;

        #endregion


        #region ClassLifeCycle

        public PlayerHealthBarModel(GameObject prefab, PlayerHealthBarData data)
        {
            Transform healthBar = prefab.transform.GetChild(0);
            healthBar.GetComponent<HorizontalLayoutGroup>().spacing = data.DistanceBetweenSections;

            RectTransform rectTransform = healthBar.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector3(data.HealthBarSize.x, data.HealthBarSize.y);
            rectTransform.position = new Vector3(data.HealthBarPosition.x, data.HealthBarPosition.y, 0);

            int sectionsAmount = data.HealthSectionsPercentThresholds.Length;
            _healthSections = new HealthSection[sectionsAmount];

            float previousHealthThreshold = 0f;
            for (int i = 0; i < sectionsAmount; i++)
            {
                Transform newSection = GameObject.Instantiate(Data.PlayerHealthBarData.HealthSectionPrefab, healthBar).
                    transform;

                Image updatableImage = newSection.GetChild(0).GetComponent<Image>();
                float upperTresholdPart = data.HealthSectionsPercentThresholds[i] / 100f;
                float sectionSize = upperTresholdPart - previousHealthThreshold;

                _healthSections[i] = new HealthSection(updatableImage, upperTresholdPart, sectionSize);

                float sectionScale = sectionsAmount / (100 / _healthSections[i].SectionSize);
                newSection.localScale = new Vector3(sectionScale, 1, 1);

                previousHealthThreshold = upperTresholdPart;
                
            }
        }

        #endregion


        #region Methods

        /// <summary>Updates health bar sections filling</summary>
        /// <param name="currentHealthPart">Current health percentage</param>
        /// <returns>Current maximum health threshold as a percentage</returns>
        public float HealthFillUpdate(float currentHealthPart)
        {
            float currentHealthThreshold = 0;

            for (int i = 0; i < _healthSections.Length; i++)
            {
                if (currentHealthPart <= _healthSections[i].UpperTresholdPart && currentHealthPart > _healthSections[i].LowerHTresholdPart)
                {
                    float healthPercentForSection = currentHealthPart - _healthSections[i].LowerHTresholdPart;
                    _healthSections[i].UpdatableImage.fillAmount = healthPercentForSection / _healthSections[i].SectionSize;
                    currentHealthThreshold = _healthSections[i].UpperTresholdPart;
                }
                else if (currentHealthPart <= _healthSections[i].LowerHTresholdPart)
                {
                    _healthSections[i].UpdatableImage.fillAmount = 0;
                }
                else if (currentHealthPart >= _healthSections[i].UpperTresholdPart)
                {
                    _healthSections[i].UpdatableImage.fillAmount = 1;
                }
            }
            return currentHealthThreshold;
        }

        #endregion
    }
}
