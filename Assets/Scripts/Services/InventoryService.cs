using UnityEngine;
using System;
using System.Collections.Generic;


namespace BeastHunter
{
    public sealed class InventoryService : Service
    {
        #region Fields

        public SlotStruct SlotStruct;
        public Dictionary<BodyParts, ClothItem> Clothes = new Dictionary<BodyParts, ClothItem>();
        public WeaponItem Feast;
        public GameObject InventoryUI;

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
            Clothes.Add(BodyParts.Head, Data.Helm);
            Clothes.Add(BodyParts.Shoulders, null);
            Clothes.Add(BodyParts.Torso, Data.Jacket);
            Clothes.Add(BodyParts.Arms, null);
            Clothes.Add(BodyParts.Hips, null);
            Clothes.Add(BodyParts.Legs, null);
            Clothes.Add(BodyParts.Feet, null);
            InventoryUI = GameObject.Instantiate(Resources.Load("Canvas")) as GameObject;
            SlotStruct.HeadSlots = FindSlotsPlaces(SlotStruct.HeadSlots, BodyParts.Head.GetHashCode());
            SlotStruct.ShouldersSlots = FindSlotsPlaces(SlotStruct.ShouldersSlots, BodyParts.Shoulders.GetHashCode());
            SlotStruct.TorsoSlots = FindSlotsPlaces(SlotStruct.TorsoSlots, BodyParts.Torso.GetHashCode());
            ShowSlots();
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

        public void SetCloth(ClothItem clothItem)
        {
            Clothes[clothItem.BodyParts] = clothItem;
            CreateSlots(clothItem.BodyParts, clothItem.PocketInfos);
        }

        Transform[] FindSlotsPlaces(Transform[] Slots, int Type)
        {
            Transform CurrentBodyPart = InventoryUI.transform.GetChild(0).transform.GetChild(0).transform.GetChild(Type);
            Slots = new Transform[CurrentBodyPart.childCount];
            for (int i = 0; i < CurrentBodyPart.childCount; i++)
            {
                Slots[i] = CurrentBodyPart.transform.GetChild(i);
                Debug.Log(CurrentBodyPart.name);
                Debug.Log(Slots[i].transform.localPosition);
            }
            return Slots;
        }

        public void UpdatePocketInfo(BodyParts bodyPart)
        {
            Transform[] Slots = (Transform[])SlotStruct.GetType().GetField(bodyPart.ToString() + "Slots").GetValue(SlotStruct);
            ClothItem cloth;
            Clothes.TryGetValue(bodyPart, out cloth);
            for (int i = 0; i < cloth.PocketInfos.Length; i++)
            {
                if (Slots[i].transform.GetChild(0).childCount != 0)
                {
                    Transform Item = Slots[i].transform.GetChild(0).transform.GetChild(0);
                    DragAndDropItem dragAndDropItem = Item.GetComponent<DragAndDropItem>();
                    if (dragAndDropItem.ItemData)
                    {
                        cloth.PocketInfos[i].ItemInPocket = dragAndDropItem.ItemData;
                    }
                }
                else
                {
                    cloth.PocketInfos[i].ItemInPocket = null;
                }
            }
            Clothes[bodyPart] = cloth;
            CreateSlots(bodyPart, cloth.PocketInfos);
        }

        public void ShowSlots()
        {
            foreach (KeyValuePair<BodyParts, ClothItem> Cloth in Clothes)
            {
                if(Cloth.Value != null)
                {
                    CreateSlots(Cloth.Key, Cloth.Value.PocketInfos);
                }
            }
        }

        public void CreateSlots(BodyParts BodyPart, PocketInfo[] pocketInfo)
        {
            Transform[] SlotPlaces = (Transform[])SlotStruct.GetType().GetField(BodyPart.ToString() + "Slots").GetValue(SlotStruct);
            foreach (Transform child in SlotPlaces)
            {
                if (child.childCount != 0)
                {
                    GameObject.Destroy(child.GetChild(0).gameObject);
                }
            }
            if (pocketInfo.Length != 0)
            {
                if(SlotPlaces.Length >= pocketInfo.Length)
                {
                    for (int i = 0; i < pocketInfo.Length; i++)
                    {
                        GameObject slot = GameObject.Instantiate(Resources.Load("Inventory/Cell"), SlotPlaces[i]) as GameObject;
                        if (pocketInfo[i].SlotEnum.GetHashCode() != 0)
                        {
                            DragAndDropCell dragAndDropCell = slot.GetComponent<DragAndDropCell>();
                            dragAndDropCell.SlotSize = pocketInfo[i].SlotEnum;
                            dragAndDropCell.IsOnInventory = true;
                            dragAndDropCell.BodyParts = BodyPart;
                            if(pocketInfo[i].ItemInPocket != null)
                            {
                                if(pocketInfo[i].ItemInPocket.ItemStruct.SlotSize.GetHashCode() <= dragAndDropCell.SlotSize.GetHashCode())
                                {
                                    GameObject item = GameObject.Instantiate(Resources.Load("Inventory/Item"), slot.transform) as GameObject;
                                    DragAndDropItem dragAndDropItem = item.GetComponent<DragAndDropItem>();
                                    dragAndDropItem.ItemData = pocketInfo[i].ItemInPocket;
                                    dragAndDropItem.LoadIcon();
                                }
                                else
                                {
                                    throw new Exception("Item in pocket have size bigger than pocket size");
                                }
                            }
                            
                        }
                    }
                }
                else
                {
                    throw new Exception("Cloth have too many slots in " + BodyPart);
                }
            }
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
