using System;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunterHubUI
{
    public abstract class BaseWorkRoomModel<T> where T : BaseWorkRoomProgress
    {
        #region Properties

        public Action OnLevelUpHandler { get; set; }

        public WorkRoomType RoomType { get; private set; }
        public SkillType UsedSkill { get; private set; }
        public string Name { get; private set; }
        public int Level { get; private set; }
        public CharacterSlotStorage ChiefWorkplace { get; private set; }
        public CharacterSlotStorage AssistantWorkplaces { get; private set; }
        public abstract Dictionary<int,T> ProgressScheme { get; protected set; }
        public float OrderTimeReducePercent { get; private set; }
        public int AssistansGeneralSkillLevel { get; private set; }

        #endregion


        #region ClassLifeCycle

        public BaseWorkRoomModel(BaseWorkRoomData<T> roomStruct)
        {
            RoomType = roomStruct.RoomType;
            UsedSkill = roomStruct.UsedSkill;
            Name = roomStruct.Name;
            OrderTimeReducePercent = 1;

            ProgressScheme = new Dictionary<int, T>();
            for (int i = 0; i < roomStruct.ProgressScheme.Count; i++)
            {
                if (!ProgressScheme.ContainsKey(roomStruct.ProgressScheme[i].Level))
                {
                    ProgressScheme.Add(roomStruct.ProgressScheme[i].Level, roomStruct.ProgressScheme[i].Progress);
                }
                else
                {
                    Debug.LogError("Incorrect input parameter: progress scheme list contain two identical values of level number");
                }
            }

            if (!ProgressScheme.ContainsKey(roomStruct.Level))
            {
                Debug.LogError($"Incorrect input data: the room level {roomStruct.Level} is not contained in the progress schema");
            }
            Level = roomStruct.Level;

            ChiefWorkplace = new CharacterSlotStorage(1, CharacterStorageType.ChiefWorkplace);
            AssistantWorkplaces = new CharacterSlotStorage(ProgressScheme[Level].AssistansAmount, CharacterStorageType.AssistantWorkplaces);

            ChiefWorkplace.OnPutElementToSlotHandler += OnChiefAddBase;
            ChiefWorkplace.OnTakeElementFromSlotHandler += OnChiefRemoveBase;
            if (roomStruct.ChiefWorker != null)
            {
                ChiefWorkplace.PutElementToFirstEmptySlot(new CharacterModel(roomStruct.ChiefWorker.CharacterStruct));
            }

            AssistantWorkplaces.OnPutElementToSlotHandler += OnAssistantAddBase;
            AssistantWorkplaces.OnTakeElementFromSlotHandler += OnAssistantRemoveBase;
            if (roomStruct.Assistants != null)
            {
                for (int i = 0; i < roomStruct.Assistants.Count; i++)
                {
                    if (!AssistantWorkplaces.PutElementToFirstEmptySlot(new CharacterModel(roomStruct.Assistants[i].CharacterStruct)))
                    {
                        Debug.LogError("Incorrect input parameter: assistans amount more than the number of slots");
                        break;
                    }
                }
            }
        }

        #endregion


        #region Methods

        protected abstract void RoomImprove();
        protected abstract float CountOrderTimeReducePercent();
        protected abstract void OnChiefAdd(CharacterModel character);
        protected abstract void OnChiefRemove(CharacterModel character);
        protected abstract void OnAssistantAdd(CharacterModel character);
        protected abstract void OnAssistantRemove(CharacterModel character);

        public void LevelUp()
        {
            if (ProgressScheme.ContainsKey(Level + 1))
            {
                Level += 1;
                AssistantWorkplaces.AddSlots(ProgressScheme[Level].AssistansAmount - AssistantWorkplaces.GetSlotsCount());
                RoomImprove();
                OnLevelUp();
            }
        }

        private void OnChiefAddBase(CharacterStorageType storageType, int slotIndex, CharacterModel character)
        {
            character.IsAssignedToWork = true;
            RecountOrderTimeReducePercent();
            OnChiefAdd(character);
        }

        private void OnChiefRemoveBase(CharacterStorageType storageType, int slotIndex, CharacterModel character)
        {
            character.IsAssignedToWork = false;
            RecountOrderTimeReducePercent();
            OnChiefRemove(character);
        }

        private void OnAssistantAddBase(CharacterStorageType storageType, int slotIndex, CharacterModel character)
        {
            character.IsAssignedToWork = true;
            RecountAssistansGeneralSkillLevel();
            RecountOrderTimeReducePercent();
            OnAssistantAdd(character);
        }

        private void OnAssistantRemoveBase(CharacterStorageType storageType, int slotIndex, CharacterModel character)
        {
            character.IsAssignedToWork = false;
            RecountAssistansGeneralSkillLevel();
            RecountOrderTimeReducePercent();
            OnAssistantRemove(character);
        }

        private void RecountAssistansGeneralSkillLevel()
        {
            AssistansGeneralSkillLevel = 0;
            for (int i = 0; i < AssistantWorkplaces.GetSlotsCount(); i++)
            {
                CharacterModel assistant = AssistantWorkplaces.GetElementBySlot(i);
                if (assistant != null)
                {
                    AssistansGeneralSkillLevel = Mathf.Clamp(AssistansGeneralSkillLevel + assistant.Skills[UsedSkill], 0, 100);
                }
            }
        }

        private void OnLevelUp()
        {
            OnLevelUpHandler?.Invoke();
        }

        private void RecountOrderTimeReducePercent()
        {
            OrderTimeReducePercent = CountOrderTimeReducePercent();
        }

        #endregion
    }
}
