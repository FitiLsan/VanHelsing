using UnityEngine;


namespace BeastHunter
{
    public class SurvivorModel : TestCharacterModel
    {
        #region Fields

        private readonly InteractableObjectBehavior _behavior;

        #endregion


        #region Properties

        public InteractableObjectBehavior Behavior => _behavior;

        #endregion
        
        
        #region ClassLifeCycle

        public SurvivorModel(GameObject prefab, TestCharacterData testCharacterData) : base(prefab, testCharacterData)
        {
            _behavior = prefab.AddComponent<SurvivorBehavior>();
        }

        #endregion


        #region Methods

        public void KillSurvivor()
        {
            _prefab.SetActive(false);
            
            Debug.Log("Die human!");
        }

        #endregion
    }
}