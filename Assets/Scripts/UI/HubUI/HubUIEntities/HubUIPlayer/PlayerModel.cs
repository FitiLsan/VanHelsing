using System;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunterHubUI
{
    public class PlayerModel
    {
        #region Properties

        public Action<int> OnChangeGoldAmount { get; set; }

        public int GoldAmount { get; private set; }
        public ItemStorage Inventory { get; private set; }
        public List<CharacterModel> HiredCharacters { get; private set; }

        #endregion


        #region ClassLifeCycle

        public PlayerModel(PlayerSettingsStruct settings, CharactersSettingsStruct charactersSettings)
        {
            GoldAmount = settings.GoldAmount;

            HiredCharacters = new List<CharacterModel>();
            for (int i = 0; i < settings.StartHiredCharacters.Length; i++)
            {
                HiredCharacters.Add(new CharacterModel(settings.StartHiredCharacters[i], charactersSettings));
            }

            Inventory = new ItemStorage(settings.InventorySlotsAmount, ItemStorageType.GeneralInventory);
            for (int i = 0; i < settings.StartInventoryItems.Length; i++)
            {
                BaseItemModel itemModel = HubUIServices.SharedInstance.
                    ItemInitializeService.InitializeItemModel(settings.StartInventoryItems[i]);
                Inventory.PutItem(i, itemModel);
            }
        }

        #endregion


        #region Methods

        public void HireCharacter(CharacterModel character)
        {
            HiredCharacters.Add(character);
        }

        public void FireCharacter(CharacterModel character)
        {
            HiredCharacters.Remove(character);
        }

        public bool AddGold(int goldAmount)
        {
            if(goldAmount > 0)
            {
                GoldAmount += goldAmount;
                OnChangeGoldAmount?.Invoke(GoldAmount);
                return true;
            }
            Debug.LogError("goldAmount parameter is less than or equal to zero");
            return false;
        }

        public bool TakeGold(int goldAmount)
        {
            if (goldAmount > 0)
            {
                GoldAmount -= goldAmount;
                OnChangeGoldAmount?.Invoke(GoldAmount);
                return true;
            }
            Debug.LogError("goldAmount parameter is less than or equal to zero");
            return false;
        }

        #endregion
    }
}
