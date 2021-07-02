using System;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunterHubUI
{
    [CreateAssetMenu(fileName = "DialogsData", menuName = "CreateData/HubUIData/DialogsData", order = 0)]
    public class DialogsPool : ScriptableObject, ISerializationCallbackReceiver
    {
        #region Fields

        [SerializeField] private List<DialogSerializable> _dialogsPool;

        #endregion


        #region Properties

        public Dictionary<int, CitizenDialog> GetDialog { get; private set; }

        #endregion


        #region ISerializationCallbackReceiver

        public void OnAfterDeserialize()
        {
            GetDialog = new Dictionary<int, CitizenDialog>();

            for (int i = 0; i < _dialogsPool.Count; i++)
            {
                if (_dialogsPool[i].CitizenData != null)
                {
                    if (!GetDialog.ContainsKey(_dialogsPool[i].CitizenData.GetInstanceID()))
                    {
                        GetDialog.Add(_dialogsPool[i].CitizenData.GetInstanceID(), _dialogsPool[i].CitizenDialog);
                    }
                    else
                    {
                        Debug.LogError("The same character is assigned to different dialogues." +
                            " Only one dialogue can be attached to a character! ");
                    }
                }
            }
        }

        public void OnBeforeSerialize() { }

        #endregion
    }
}

