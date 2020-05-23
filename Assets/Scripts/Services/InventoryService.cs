using UnityEngine;
using System;
using System.Collections.Generic;


namespace BeastHunter
{
    public sealed class InventoryService : Service
    {
        #region Fields

        public Dictionary<BodyParts, ClothItem> Clothes = new Dictionary<BodyParts, ClothItem>();
        public WeaponItem Feast;

        #endregion


        #region Properties

        public Action OnLeftWeaponChangeStart { get; set; } // TO activate
        public Action OnRightWeaponChangeStart { get; set; } // TO activate
        public Action OnLeftWeaponChangeEnd { get; set; } // TO activate
        public Action OnRightWeaponChangeEnd { get; set; } // TO activate

        #endregion


        #region ClassLifeCycles

        public InventoryService(Contexts contexts) : base(contexts)
        {
            Clothes.Add(BodyParts.Head, null);
            Clothes.Add(BodyParts.Torso, Data.Jacket);
            Clothes.Add(BodyParts.Arms, null);
            Clothes.Add(BodyParts.Hips, null);
            Clothes.Add(BodyParts.Legs, null);
            Clothes.Add(BodyParts.Feet, null);
            Feast = Data.Feast;     
        }

        #endregion


        #region Methods

        public void SetItemInRandomPocket(BaseItem item)
        {
            foreach (var clothItem in Clothes)
            {
                if (clothItem.Value != null && clothItem.Value.SetItemInEmptyPocket(item))
                {
                    break;
                }
            }

            ShowInventoryFull();
        }

        public void SetItemInRandomPocket(BodyParts bodyPart, BaseItem item)
        {
            if(!Clothes[bodyPart].SetItemInEmptyPocket(item)) ShowInventoryFull();
        }

        public void SetCloth(BodyParts bodyPart, ClothItem clothItem)
        {
            Clothes[bodyPart] = clothItem;
        }

        private void ShowInventoryFull()
        {
            //TODO - shows UI when inventory can not take the item
        }

        public WeaponItem[] GetAllWeapons()
        {
            List<WeaponItem> weaponItems = new List<WeaponItem>();

            foreach (var cloth in Clothes)
            {
                if (cloth.Value != null)
                {
                    var weapons = cloth.Value.GetWeapons();
                    weaponItems.AddRange(weapons);
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
                    characterModel.LeftHandWeapon = weapon;
                    characterModel.RightHandWeapon = weapon;
                    characterModel.LeftHandWeaponObject = GameObject.Instantiate(weapon.WeaponPrefab);
                    characterModel.LeftHandWeaponObject.transform.SetParent(characterModel.LeftHand);
                    characterModel.LeftHandWeaponObject.transform.localPosition = weapon.PrefabPositionInHand;
                    characterModel.LeftHandWeaponObject.transform.localEulerAngles = weapon.PrefabRotationInHand;
                    weaponHitBoxBehaviour = characterModel.LeftHandWeaponObject.GetComponent<WeaponHitBoxBehavior>();
                    weaponHitBoxBehaviour.IsInteractable = false;
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

        #endregion
    }
}
