using UnityEngine;
using System;
using System.Collections.Generic;


namespace BeastHunter
{
    public sealed class InventoryService : Service
    {
        #region Fields

        public ClothItem Head;
        public ClothItem Torso;
        public ClothItem Arms;
        public ClothItem Legs;
        public WeaponItem Feast;

        public Action OnLeftWeaponChange { get; set; }
        public Action OnRightWeaponChange { get; set; }

        #endregion


        #region ClassLifeCycles

        public InventoryService(Contexts contexts) : base(contexts)
        {
            Feast = Data.Feast;
            Torso = Data.Jacket;
            
        }

        #endregion


        #region Methods

        public WeaponItem[] GetWeapons()
        {
            List<WeaponItem> weaponItems = new List<WeaponItem>();

            var weapons = GetWeaponFromColth(Head);
            if (weapons != null)
                weaponItems.AddRange(weapons);

            weapons = GetWeaponFromColth(Torso);
            if (weapons != null)
                weaponItems.AddRange(weapons);

            weapons = GetWeaponFromColth(Arms);
            if (weapons != null)
                weaponItems.AddRange(weapons);

            weapons = GetWeaponFromColth(Legs);
            if (weapons != null)
                weaponItems.AddRange(weapons);

            return weaponItems.ToArray();
        }

        private WeaponItem[] GetWeaponFromColth(ClothItem cloth)
        {
            List<WeaponItem> weaponItems = new List<WeaponItem>();

            if (cloth != null)
            {
                for (int i = 0; i < cloth.pocketInfos.Length; i++)
                {
                    if (cloth.pocketInfos[i].ItemInPocket.ItemStruct.ItemType == ItemType.Weapon)
                        weaponItems.Add(cloth.pocketInfos[i].ItemInPocket as WeaponItem);
                }
            }

            return weaponItems.ToArray();
        }

        public void SetWeaponInLeftHand(CharacterModel characterModel, WeaponItem weapon, out WeaponHitBoxBehavior weaponHitBox)
        {
            WeaponHitBoxBehavior weaponHitBoxBehaviour = null;

            if (weapon.WeaponHandType == WeaponHandType.TwoHanded)
            {
                if (characterModel.LeftHandWeaponObject == null)
                {

                }
                else
                {
                    if(characterModel.LeftHandWeapon.WeaponHandType == WeaponHandType.TwoHanded)
                    {
                        // set weapon 
                    }
                    else
                    {
                        // set another arm free
                    }
                }
            }
            else
            {
                if(characterModel.LeftHandWeaponObject == null)
                {
                    characterModel.LeftHandWeapon = weapon;
                    characterModel.LeftHandWeaponObject = GameObject.Instantiate(weapon.WeaponPrefab);
                    characterModel.LeftHandWeaponObject.transform.SetParent(characterModel.LeftHand);
                    characterModel.LeftHandWeaponObject.transform.localPosition = weapon.PrefabPositionInHand;
                    characterModel.LeftHandWeaponObject.transform.localEulerAngles = weapon.PrefabRotationInHand;
                    weaponHitBoxBehaviour = characterModel.LeftHandWeaponObject.GetComponent<WeaponHitBoxBehavior>();
                    weaponHitBoxBehaviour.IsInteractable = false;
                }
                else
                {

                }
                // set in current arm
            }

            weaponHitBox = weaponHitBoxBehaviour;
        }

        public void SetWeaponInRightHand(CharacterModel characterModel, WeaponItem weapon, out WeaponHitBoxBehavior weaponHitBox)
        {
            WeaponHitBoxBehavior weaponHitBoxBehaviour = null;

            if (weapon.WeaponHandType == WeaponHandType.TwoHanded)
            {
                if (characterModel.RightHandWeaponObject == null)
                {
                    characterModel.RightHandWeapon = weapon;
                    characterModel.LeftHandWeapon = weapon;
                    characterModel.RightHandWeaponObject = GameObject.Instantiate(weapon.WeaponPrefab);
                    characterModel.RightHandWeaponObject.transform.SetParent(characterModel.RightHand);
                    characterModel.RightHandWeaponObject.transform.localPosition = weapon.PrefabPositionInHand;
                    characterModel.RightHandWeaponObject.transform.localEulerAngles = weapon.PrefabRotationInHand;
                    weaponHitBoxBehaviour = characterModel.RightHandWeaponObject.GetComponent<WeaponHitBoxBehavior>();
                    weaponHitBoxBehaviour.IsInteractable = false;
                }
                else
                {
                    if (characterModel.RightHandWeapon.WeaponHandType == WeaponHandType.TwoHanded)
                    {
                        // set weapon 
                    }
                    else
                    {
                        // set another arm free
                    }
                }
            }
            else
            {
                if (characterModel.RightHandWeaponObject == null)
                {
                    characterModel.RightHandWeapon = weapon;
                    characterModel.RightHandWeaponObject = GameObject.Instantiate(weapon.WeaponPrefab);
                    characterModel.RightHandWeaponObject.transform.SetParent(characterModel.RightHand);
                    characterModel.RightHandWeaponObject.transform.localPosition = weapon.PrefabPositionInHand;
                    characterModel.RightHandWeaponObject.transform.localEulerAngles = weapon.PrefabRotationInHand;
                    weaponHitBoxBehaviour = characterModel.RightHandWeaponObject.GetComponent<WeaponHitBoxBehavior>();
                    weaponHitBoxBehaviour.IsInteractable = false;
                }
                else
                {

                }
                // set in current arm
            }

            weaponHitBox = weaponHitBoxBehaviour;
        }

        public void RemoveWeaponFromLeftHand(CharacterModel characterModel)
        {
            // TODO
        }

        public void RemoveWeaponFromRightHand(CharacterModel characterModel)
        {
            // TODO
        }

        public void RemoveAllWeapon(CharacterModel characterModel)
        {
            // TODO
        }

        public void HideWepons(CharacterModel characterModel)
        {
            if(characterModel.LeftHandWeaponObject != null)
            {
                characterModel.LeftHandWeaponObject.SetActive(false);
            }
            if(characterModel.RightHandWeaponObject != null)
            {
                characterModel.RightHandWeaponObject.SetActive(false);
            }

            characterModel.IsWeaponInHands = false;
        }

        public void ShowWeapons(CharacterModel characterModel)
        {
            if (characterModel.LeftHandWeaponObject != null)
            {
                characterModel.LeftHandWeaponObject.SetActive(true);
            }
            if (characterModel.RightHandWeaponObject != null)
            {
                characterModel.RightHandWeaponObject.SetActive(true);
            }

            characterModel.IsWeaponInHands = true;
        }

        //TODO make a void methods "SetClothHead" with dictionary etc. and "SetItemInRandomPocket"

        #endregion
    }
}
