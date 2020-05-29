using System.Collections.Generic;


namespace BeastHunter
{
    public sealed class InputModel
    {
        #region Fields

        public Dictionary<string, InputEventTypes> inputOnButtonDown;

        public float InputAxisX;
        public float InputAxisY;
        public float InputTotalAxisX;
        public float InputTotalAxisY;

        public bool IsInputRun;

        #endregion


        #region ClassLifeCycle

        public InputModel()
        {
            inputOnButtonDown = new Dictionary<string, InputEventTypes>();
        }

        #endregion
    }
}

