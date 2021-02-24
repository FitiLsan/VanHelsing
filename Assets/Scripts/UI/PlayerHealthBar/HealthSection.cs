using UnityEngine.UI;


namespace BeastHunter
{
    public class HealthSection
    {
        #region Properties

        public Image UpdatableImage { get; private set; }
        public float UpperTresholdPart { get; private set; }
        public float LowerHTresholdPart { get; private set; }
        public float SectionSize { get; private set; }

        #endregion


        #region ClassLifeCycle

        public HealthSection(Image updatableImage, float upperTresholdInPercent, float sectionSizeInPercent)
        {
            UpdatableImage = updatableImage;
            UpperTresholdPart = upperTresholdInPercent;
            SectionSize = sectionSizeInPercent;
            LowerHTresholdPart = upperTresholdInPercent - sectionSizeInPercent;
        }

        #endregion
    }
}

