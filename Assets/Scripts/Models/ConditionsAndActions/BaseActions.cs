using System.Collections.Generic;
using UnityEngine;

namespace Models.ConditionsAndActions
{
    public abstract class BaseActions : ScriptableObject
    {
        public BaseActions()
        {
            AllActions = new Dictionary<string, bool>();

            #region Добавляем все действия

            AllActions.Add("Standing", Standing);
            AllActions.Add("Running", Running);
            AllActions.Add("Walking", Walking);
            AllActions.Add("Falling", Falling);

            #endregion

            Standing = true;
        }

        protected Dictionary<string, bool> AllActions { get; }

        #region Действия

        /// <summary>
        ///     Персонаж бездействует
        /// </summary>
        public virtual bool Standing
        {
            get => AllActions["Standing"];

            set => AllActions["Standing"] = value;
        }

        /// <summary>
        ///     Персонаж бежит
        /// </summary>
        public virtual bool Running
        {
            get => AllActions["Standing"];

            set => AllActions["Standing"] = value;
        }

        /// <summary>
        ///     Персонаж идет
        /// </summary>
        public virtual bool Walking
        {
            get => AllActions["Standing"];

            set => AllActions["Standing"] = value;
        }

        /// <summary>
        ///     Персонаж падает
        /// </summary>
        public virtual bool Falling
        {
            get => AllActions["Falling"];

            set => AllActions["Falling"] = value;
        }

        #endregion
    }
}