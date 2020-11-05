using UnityEngine;


namespace BeastHunter
{
    [CreateAssetMenu(fileName = "Data", menuName = "BossSO")]
    public class BossSOPattern : ScriptableObject
    {
        #region Enums

        [Header("Boss Type")]
        public EnemyType BossTypes;

        [Header("Boss Subtypes")]
        public AnimaliaSubtypes AnimaliaSubtypes;
        public MentaliaSubtypes MentaliaSubtypes;
        public ParazitaliaSubtypes ParazitaliaSubtypes;
        public SpiritualiaSubtypes SpiritualiaSubtypes;
        public VegetaliaSuptypes VegetaliaSuptypes;

        [Header("Boss Size")]
        public BossSize Size;

        [Header("Boss Sociality")]
        public BossSociality Sociality;

        [Header("Boss Habitates")]
        public BossHabitat[] Habitat;

        [Header("Objects of Attraction")]
        public AttractionOrAvoidance[] Attraction;

        [Header("Objects of Avoidance")]
        public AttractionOrAvoidance[] Avoidance;

        [Header("Weakness Material")]
        public WeaknessOrImmunityMaterial[] Weakness;

        [Header("Immunity Material")]
        public WeaknessOrImmunityMaterial[] Immunity;

        [Header("Interaction With The Environment")]
        public InteractionWithTheEnvironment[] Interaction;

        [Header("Behavior If Boss Sees A Hunter")]
        public IfSeesTheHunter IfSeesTheHunter;

        [Header("Time, When Boss Is Active")]
        public ActivityTime[] Activity;

        [Header("Nesting Place")]
        public NestingGrounds[] Nestings;

        [Header("Way To Move")]
        public WayToMove WayToMove;

        #endregion
    }

}
