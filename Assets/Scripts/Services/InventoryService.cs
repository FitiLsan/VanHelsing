using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;

namespace BeastHunter
{
    public sealed class InventoryService : Service
    {
        #region Fields

        public SlotStruct SlotStruct;
        public Dictionary<BodyParts, ClothItem> Clothes = new Dictionary<BodyParts, ClothItem>();
        public Dictionary<BodyParts, List<GameObject>> ClothesMeshes = new Dictionary<BodyParts, List<GameObject>>();
        public Dictionary<BodyParts, List<GameObject>> AccessoriesMeshes = new Dictionary<BodyParts, List<GameObject>>();
        public GameObject PlayerDoll;
        public GameObject InventoryUI;
        public Material Material;
        public string PlayerSavePrefabPath = "Assets/AdditiveAssets/PolygonFantasyHeroCharacters/Prefabs/ModularCharactersPlayerVersion.prefab";
        public List<GameObject> ShouldersMeshes = new List<GameObject>();
        public List<GameObject> HandsMeshes = new List<GameObject>();
        public List<GameObject> LegsMeshes = new List<GameObject>();
        public List<GameObject> FootsMeshes = new List<GameObject>();
        public List<GameObject> HeadMeshes = new List<GameObject>();


        #region Lists

        public List<GameObject> headAllElements = new List<GameObject>();
        public List<GameObject> headNoElements = new List<GameObject>();
        public List<GameObject> eyebrow = new List<GameObject>();
        public List<GameObject> facialHair = new List<GameObject>();
        public List<GameObject> torso = new List<GameObject>();
        public List<GameObject> arm_Upper_Right = new List<GameObject>();
        public List<GameObject> arm_Upper_Left = new List<GameObject>();
        public List<GameObject> arm_Lower_Right = new List<GameObject>();
        public List<GameObject> arm_Lower_Left = new List<GameObject>();
        public List<GameObject> hand_Right = new List<GameObject>();
        public List<GameObject> hand_Left = new List<GameObject>();
        public List<GameObject> hips = new List<GameObject>();
        public List<GameObject> leg_Right = new List<GameObject>();
        public List<GameObject> leg_Left = new List<GameObject>();

        public List<GameObject> headAllElementsf = new List<GameObject>();
        public List<GameObject> headNoElementsf = new List<GameObject>();
        public List<GameObject> eyebrowf = new List<GameObject>();
        public List<GameObject> facialHairf = new List<GameObject>();
        public List<GameObject> torsof = new List<GameObject>();
        public List<GameObject> arm_Upper_Rightf = new List<GameObject>();
        public List<GameObject> arm_Upper_Leftf = new List<GameObject>();
        public List<GameObject> arm_Lower_Rightf = new List<GameObject>();
        public List<GameObject> arm_Lower_Leftf = new List<GameObject>();
        public List<GameObject> hand_Rightf = new List<GameObject>();
        public List<GameObject> hand_Leftf = new List<GameObject>();
        public List<GameObject> hipsf = new List<GameObject>();
        public List<GameObject> leg_Rightf = new List<GameObject>();
        public List<GameObject> leg_Leftf = new List<GameObject>();

        public List<GameObject> headCoverings_Base_Hair = new List<GameObject>();
        public List<GameObject> headCoverings_No_FacialHair = new List<GameObject>();
        public List<GameObject> headCoverings_No_Hair = new List<GameObject>();
        public List<GameObject> all_Hair = new List<GameObject>();
        public List<GameObject> all_Head_Attachment = new List<GameObject>();
        public List<GameObject> chest_Attachment = new List<GameObject>();
        public List<GameObject> back_Attachment = new List<GameObject>();
        public List<GameObject> shoulder_Attachment_Right = new List<GameObject>();
        public List<GameObject> shoulder_Attachment_Left = new List<GameObject>();
        public List<GameObject> elbow_Attachment_Right = new List<GameObject>();
        public List<GameObject> elbow_Attachment_Left = new List<GameObject>();
        public List<GameObject> hips_Attachment = new List<GameObject>();
        public List<GameObject> knee_Attachement_Right = new List<GameObject>();
        public List<GameObject> knee_Attachement_Left = new List<GameObject>();
        public List<GameObject> all_12_Extra = new List<GameObject>();
        public List<GameObject> elf_Ear = new List<GameObject>();
        #endregion

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
            FillDictionary();
            InventoryUI = GameObject.Instantiate(Resources.Load("Canvas")) as GameObject;
            FillSlotStruct();
            ShowSlots();
            InventoryUI.SetActive(false);
            Activate();
            EndCustomization();
        }

        #endregion


        #region Methods

        private void FillDictionary()
        {
            Clothes.Add(BodyParts.Head, Data.Helm);
            Clothes.Add(BodyParts.Shoulders, null);
            Clothes.Add(BodyParts.Torso, Data.Jacket);
            Clothes.Add(BodyParts.Arms, null);
            Clothes.Add(BodyParts.Hips, null);
            Clothes.Add(BodyParts.Legs, null);
            Clothes.Add(BodyParts.Feet, null);
        }

        private void FillSlotStruct()
        {
            SlotStruct.HeadSlots = FindSlotsPlaces(SlotStruct.HeadSlots, BodyParts.Head.GetHashCode());
            SlotStruct.ShouldersSlots = FindSlotsPlaces(SlotStruct.ShouldersSlots, BodyParts.Shoulders.GetHashCode());
            SlotStruct.TorsoSlots = FindSlotsPlaces(SlotStruct.TorsoSlots, BodyParts.Torso.GetHashCode());
            SlotStruct.LegsSlots = FindSlotsPlaces(SlotStruct.LegsSlots, BodyParts.Legs.GetHashCode());
            SlotStruct.ArmsSlots = FindSlotsPlaces(SlotStruct.ArmsSlots, BodyParts.Arms.GetHashCode());
            SlotStruct.HipsSlots = FindSlotsPlaces(SlotStruct.HipsSlots, BodyParts.Hips.GetHashCode());
        }

        public void Activate()
        {
            InventoryUI.SetActive(true);
            PlayerDoll = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath("Assets/AdditiveAssets/PolygonFantasyHeroCharacters/Prefabs/ModularCharacters.prefab", typeof(GameObject)));
            BuildLists();
            BuildDictionaryMeshes();
            LoadPlayerBase();
            LoadMeshView();
        }

        private void BuildDictionaryMeshes()
        {
            AccessoriesMeshes.Add(BodyParts.Head, all_Head_Attachment);
            AccessoriesMeshes.Add(BodyParts.Torso, back_Attachment);
            AccessoriesMeshes.Add(BodyParts.Shoulders, shoulder_Attachment_Right);
            AccessoriesMeshes.Add(BodyParts.Arms, elbow_Attachment_Right);
            AccessoriesMeshes.Add(BodyParts.Hips, hips_Attachment);
            AccessoriesMeshes.Add(BodyParts.Legs, knee_Attachement_Right);
            ClothesMeshes.Add(BodyParts.Head, HeadMeshes);
            ClothesMeshes.Add(BodyParts.Shoulders, ShouldersMeshes);
            ClothesMeshes.Add(BodyParts.Torso, torso);
            ClothesMeshes.Add(BodyParts.Arms, HandsMeshes);
            ClothesMeshes.Add(BodyParts.Hips, hips);
            ClothesMeshes.Add(BodyParts.Legs, LegsMeshes);
            ClothesMeshes.Add(BodyParts.Feet, null);
        }

        private void ClearUnactiveMeshes()
        {
            foreach(KeyValuePair<BodyParts, List<GameObject>> meshList in ClothesMeshes)
            {
                if(meshList.Value != null)
                {
                    foreach (GameObject mesh in meshList.Value)
                    {
                        if (mesh.activeInHierarchy == false)
                        {
                            mesh.GetComponent<SkinnedMeshRenderer>().sharedMesh = null;
                        }
                    }
                }
            }
            foreach (KeyValuePair<BodyParts, List<GameObject>> meshList in AccessoriesMeshes)
            {
                foreach (GameObject mesh in meshList.Value)
                {
                    if (!mesh.activeSelf)
                    {
                        if (mesh.GetComponent<SkinnedMeshRenderer>())
                        {
                            mesh.GetComponent<SkinnedMeshRenderer>().sharedMesh = null;
                        }
                    }
                }
            }
        }

        private void ClearMeshes()
        {
            ClothesMeshes.Clear();
            AccessoriesMeshes.Clear();
            LegsMeshes.Clear();
            HandsMeshes.Clear();
            ShouldersMeshes.Clear();
            HeadMeshes.Clear();
            FootsMeshes.Clear();
        }

        private void LoadPlayerBase()
        {
            ActivateItem(headAllElements[1]);
            ActivateItem(eyebrow[0]);
            ActivateItem(facialHair[0]);
            ActivateItem(hand_Right[0]);
            ActivateItem(hand_Left[0]);
        }

        public void EndCustomization()
        {
            ClearUnactiveMeshes();
            ClearMeshes();
            PrefabUtility.SaveAsPrefabAsset(PlayerDoll, PlayerSavePrefabPath);
            GameObject.Destroy(PlayerDoll);
            InventoryUI.SetActive(false);
        }

        private void LoadMeshView()
        {
            SetMeshView(BodyParts.Head);
            SetMeshView(BodyParts.Torso);
            SetMeshView(BodyParts.Shoulders);
            SetMeshView(BodyParts.Arms);
            SetMeshView(BodyParts.Hips);
            SetMeshView(BodyParts.Legs);
        }

        private void SetMeshView(BodyParts bodyPart)
        {
            ClothItem cloth;
            Clothes.TryGetValue(bodyPart, out cloth);
            if(cloth != null)
            {
                List<GameObject> activateList = new List<GameObject>();
                foreach (GameObject mesh in ClothesMeshes[bodyPart])
                {
                    for(int i = 0; i < cloth.MeshView.Length; i++)
                    {
                        if (mesh.name == cloth.MeshView[i].name)
                        {
                            activateList.Add(mesh);
                        }
                        else
                        {
                            mesh.SetActive(false);
                        }
                    }
                }
                if(cloth.MeshAccessories != null)
                {
                    if (cloth.MeshAccessories.Length == 0)
                    {
                        foreach (GameObject mesh in AccessoriesMeshes[bodyPart])
                        {
                            mesh.SetActive(false);
                        }
                    }
                    else
                    {
                        foreach (GameObject mesh in AccessoriesMeshes[bodyPart])
                        {
                            for (int i = 0; i < cloth.MeshAccessories.Length; i++)
                            {
                                if (mesh.name == cloth.MeshAccessories[i].name)
                                {
                                    activateList.Add(mesh);
                                }
                                else
                                {
                                    mesh.SetActive(false);
                                }
                            }
                        }
                    }
                }
                foreach (GameObject mesh in activateList)
                {
                    ActivateItem(mesh);
                }
            }
            else
            {
                if(ClothesMeshes[bodyPart] != null)
                {
                    foreach (GameObject mesh in ClothesMeshes[bodyPart])
                    {
                        string meshIndex = mesh.name.Split('_').Last();
                        if(meshIndex == "00")
                        {
                            ActivateItem(mesh);
                        }
                    }
                }
                else
                {
                    Debug.Log("No mesh list in dictionary");
                }
            }
        }

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

        public ClothItem SetCloth(ClothItem clothItem)
        {
            ClothItem ClothToStorage;
            if (Clothes[clothItem.BodyParts] != null)
            {
                ClothToStorage = Clothes[clothItem.BodyParts];
                Clothes[clothItem.BodyParts] = clothItem;
                CreateSlots(clothItem.BodyParts, clothItem.PocketInfos);
                SetMeshView(clothItem.BodyParts);
                return ClothToStorage;
            }
            Clothes[clothItem.BodyParts] = clothItem;
            CreateSlots(clothItem.BodyParts, clothItem.PocketInfos);
            SetMeshView(clothItem.BodyParts);
            return null;
        }

        Transform[] FindSlotsPlaces(Transform[] Slots, int Type)
        {
            Transform CurrentBodyPart = InventoryUI.transform.GetChild(0).transform.GetChild(0).transform.GetChild(Type);
            Slots = new Transform[CurrentBodyPart.childCount];
            for (int i = 0; i < CurrentBodyPart.childCount; i++)
            {
                Slots[i] = CurrentBodyPart.transform.GetChild(i);
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

        private void BuildList(List<GameObject> targetList, string characterPart)
        {
            Transform[] rootTransform = PlayerDoll.GetComponentsInChildren<Transform>();
            Transform targetRoot = null;
            foreach (Transform t in rootTransform)
            {
                if (t.gameObject.name == characterPart)
                {
                    targetRoot = t;
                    break;
                }
            }
            targetList.Clear();
            for (int i = 0; i < targetRoot.childCount; i++)
            {
                GameObject go = targetRoot.GetChild(i).gameObject;
                go.SetActive(false);
                targetList.Add(go);
                
                if (!Material)
                {
                    if (go.GetComponent<SkinnedMeshRenderer>())
                        Material = go.GetComponent<SkinnedMeshRenderer>().material;
                }
            }
        }

        private void BuildLists()
        {
            BuildList(headAllElements, "Male_Head_All_Elements");
            BuildList(headNoElements, "Male_Head_No_Elements");
            BuildList(eyebrow, "Male_01_Eyebrows");
            BuildList(facialHair, "Male_02_FacialHair");
            BuildList(torso, "Male_03_Torso");
            BuildList(arm_Upper_Right, "Male_04_Arm_Upper_Right");
            BuildList(arm_Upper_Left, "Male_05_Arm_Upper_Left");
            BuildList(arm_Lower_Right, "Male_06_Arm_Lower_Right");
            BuildList(arm_Lower_Left, "Male_07_Arm_Lower_Left");
            BuildList(hand_Right, "Male_08_Hand_Right");
            BuildList(hand_Left, "Male_09_Hand_Left");
            BuildList(hips, "Male_10_Hips");
            BuildList(leg_Right, "Male_11_Leg_Right");
            BuildList(leg_Left, "Male_12_Leg_Left");
            
            BuildList(headAllElementsf, "Female_Head_All_Elements");
            BuildList(headNoElementsf, "Female_Head_No_Elements");
            BuildList(eyebrowf, "Female_01_Eyebrows");
            BuildList(facialHairf, "Female_02_FacialHair");
            BuildList(torsof, "Female_03_Torso");
            BuildList(arm_Upper_Rightf, "Female_04_Arm_Upper_Right");
            BuildList(arm_Upper_Leftf, "Female_05_Arm_Upper_Left");
            BuildList(arm_Lower_Rightf, "Female_06_Arm_Lower_Right");
            BuildList(arm_Lower_Leftf, "Female_07_Arm_Lower_Left");
            BuildList(hand_Rightf, "Female_08_Hand_Right");
            BuildList(hand_Leftf, "Female_09_Hand_Left");
            BuildList(hipsf, "Female_10_Hips");
            BuildList(leg_Rightf, "Female_11_Leg_Right");
            BuildList(leg_Leftf, "Female_12_Leg_Left");
            
            BuildList(all_Hair, "All_01_Hair");
            BuildList(all_Head_Attachment, "All_02_Head_Attachment");
            BuildList(headCoverings_Base_Hair, "HeadCoverings_Base_Hair");
            BuildList(headCoverings_No_FacialHair, "HeadCoverings_No_FacialHair");
            BuildList(headCoverings_No_Hair, "HeadCoverings_No_Hair");
            BuildList(chest_Attachment, "All_03_Chest_Attachment");
            BuildList(back_Attachment, "All_04_Back_Attachment");
            BuildList(shoulder_Attachment_Right, "All_05_Shoulder_Attachment_Right");
            BuildList(shoulder_Attachment_Left, "All_06_Shoulder_Attachment_Left");
            BuildList(elbow_Attachment_Right, "All_07_Elbow_Attachment_Right");
            BuildList(elbow_Attachment_Left, "All_08_Elbow_Attachment_Left");
            BuildList(hips_Attachment, "All_09_Hips_Attachment");
            BuildList(knee_Attachement_Right, "All_10_Knee_Attachement_Right");
            BuildList(knee_Attachement_Left, "All_11_Knee_Attachement_Left");
            BuildList(elf_Ear, "Elf_Ear");

            ShouldersMeshes.AddRange(arm_Upper_Left);
            ShouldersMeshes.AddRange(arm_Upper_Right);
            HandsMeshes.AddRange(arm_Lower_Right);
            HandsMeshes.AddRange(arm_Lower_Left);
            LegsMeshes.AddRange(leg_Left);
            LegsMeshes.AddRange(leg_Right);
            HeadMeshes.AddRange(headCoverings_Base_Hair);
            HeadMeshes.AddRange(headCoverings_No_FacialHair);
            HeadMeshes.AddRange(headCoverings_No_Hair);
            knee_Attachement_Right.AddRange(knee_Attachement_Left);
            shoulder_Attachment_Right.AddRange(shoulder_Attachment_Left);
            elbow_Attachment_Right.AddRange(elbow_Attachment_Left);
        }

        void ActivateItem(GameObject go)
        {
            go.SetActive(true);
        }

        #endregion
    }
}
