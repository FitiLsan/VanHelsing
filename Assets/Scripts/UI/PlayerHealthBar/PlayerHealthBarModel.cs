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

            float previousHealthThreshold = 0;
            for (int i = 0; i < sectionsAmount; i++)
            {
                Transform newSection = GameObject.Instantiate(Data.PlayerHealthBarData.HealthSectionPrefab).transform;
                newSection.parent = healthBar;

                Image updatableImage = newSection.GetChild(0).GetComponent<Image>();
                float upperTresholdInPercent = data.HealthSectionsPercentThresholds[i];
                float sectionSizeInPercent = upperTresholdInPercent - previousHealthThreshold;
                _healthSections[i] = new HealthSection(updatableImage, upperTresholdInPercent, sectionSizeInPercent);

                float sectionScale = sectionsAmount / (100 / _healthSections[i].SectionSizeInPercent);
                newSection.localScale = new Vector3(sectionScale, 1, 1);

                previousHealthThreshold = upperTresholdInPercent;
            }
        }

        #endregion


        #region Methods

        /// <summary>Updates health bar sections filling</summary>
        /// <param name="currentHealthPercent">Current health percentage</param>
        /// <returns>Current maximum health threshold as a percentage</returns>
        public float HealthFillUpdate(float currentHealthPercent)
        {
            float currentHealthThreshold = 0;
            for (int i = 0; i < _healthSections.Length; i++)
            {
                if (currentHealthPercent <= _healthSections[i].UpperTresholdInPercent && currentHealthPercent > _healthSections[i].LowerHTresholdInPercent)
                {
                    float healthPercentForSection = currentHealthPercent - _healthSections[i].LowerHTresholdInPercent;
                    _healthSections[i].UpdatableImage.fillAmount = healthPercentForSection / _healthSections[i].SectionSizeInPercent;
                    currentHealthThreshold = _healthSections[i].UpperTresholdInPercent;
                }
                else if (currentHealthPercent < _healthSections[i].LowerHTresholdInPercent)
                {
                    _healthSections[i].UpdatableImage.fillAmount = 0;
                }
                else if (currentHealthPercent > _healthSections[i].UpperTresholdInPercent)
                {
                    _healthSections[i].UpdatableImage.fillAmount = 1;
                }
            }
            return currentHealthThreshold;
        }

        #endregion


        #region PrivateData

        private class HealthSection
        {
            #region Properties

            public Image UpdatableImage { get; private set; }
            public float UpperTresholdInPercent { get; private set; }
            public float LowerHTresholdInPercent { get; private set; }
            public float SectionSizeInPercent { get; private set; }

            #endregion


            #region ClassLifeCycle

            public HealthSection(Image updatableImage, float upperTresholdInPercent, float sectionSizeInPercent)
            {
                UpdatableImage = updatableImage;
                UpperTresholdInPercent = upperTresholdInPercent;
                SectionSizeInPercent = sectionSizeInPercent;
                LowerHTresholdInPercent = upperTresholdInPercent - sectionSizeInPercent;
            }

            #endregion
        }

        #endregion
    }
}
