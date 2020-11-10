using UnityEngine;


namespace BeastHunter
{
    public sealed class TrapModel
    {
        #region Fields

        public readonly TrapData TrapData;
        public readonly TrapStruct TrapStruct;

        public readonly GameObject TrapObjectInFrontOfCharacter;
        public readonly GameObject TrapObjectInHands;

        #endregion


        #region Properties

        public int ChargeAmount { get; set; }

        #endregion


        #region ClassLifeCycle

        public TrapModel(GameObject trapInHands, GameObject trapInFrontOfCharacter, TrapData trapData)
        {
            TrapData = trapData;
            TrapStruct = trapData.TrapStruct;
            TrapObjectInFrontOfCharacter = trapInFrontOfCharacter;
            TrapObjectInHands = trapInHands;
            ChargeAmount = TrapStruct.ChargeAmount;
        }

        #endregion
    }
}

