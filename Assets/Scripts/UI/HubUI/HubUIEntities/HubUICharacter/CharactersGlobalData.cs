using Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunterHubUI
{
    [Serializable]
    public class CharactersGlobalData
    {
        #region Fields

        [SerializeField] private ClothesType[] _clothesSlots;
        [SerializeField] private int _backpuckSlotAmount;
        [SerializeField] private int _weaponSetsAmount;
        [SerializeField] private int _charactersAmountForHire;

        [Header("Character prefab")]
        [SerializeField] private GameObject _view3DModelPrefab;
        [SerializeField] private RuntimeAnimatorController _view3DModelAnimatorController;

        [Header("HubUI 3D-rendering")]
        [SerializeField] private GameObject _characters3DViewRenderingPrefab;
        [SerializeField] private Vector3 _characters3DViewRenderingObjectPosition;

        [Header("Character randomizer settings")]
        [SerializeField] private GameObject _modularCharactersPrefab;
        [SerializeField] private string _modularCharactersChildGOForModulesName;
        [SerializeField, Range(0, 1)] private float _femaleGenderChance;
        [SerializeField] private string[] _femaleNamesPool;
        [SerializeField] private string[] _maleNamesPool;
        [SerializeField] private Material[] _fantasyHeroMaterialsPool;
        [Tooltip("cannot be greater than clothes slots count")]
        [SerializeField] private int _minClothesEquipItemsAmount;
        [SerializeField, Range(0, 1)] private float _hairlessChance;
        [SerializeField, Range(0, 1)] private float _maleFacialHairChance;

        #endregion


        #region Properties

        public int AmountForHire => _charactersAmountForHire;
        public int BackpackSlotAmount => _backpuckSlotAmount;
        public int WeaponSetsAmount => _weaponSetsAmount;
        public ClothesType[] ClothesSlots => (ClothesType[])_clothesSlots.Clone();
        public GameObject View3DModelPrefab => _view3DModelPrefab;
        public RuntimeAnimatorController View3DModelAnimatorController => _view3DModelAnimatorController;
        public GameObject ModularCharactersPrefab => _modularCharactersPrefab;
        public string ModularCharactersChildGOForModulesName => _modularCharactersChildGOForModulesName;
        public float FemaleGenderChance => _femaleGenderChance;
        public string[] FemaleNamesPool => _femaleNamesPool;
        public string[] MaleNamesPool => _maleNamesPool;
        public Material[] FantasyHeroMaterialsPool => _fantasyHeroMaterialsPool;
        public int MinClothesEquipItemsAmount => _minClothesEquipItemsAmount;
        public float HairlessChance => _hairlessChance;
        public float MaleFacialHairChance => _maleFacialHairChance;
        public GameObject Character3DViewModelRendering { get; private set; }
        public Camera CharacterPortraitCamera { get; private set; }

        #endregion


        #region Methods

        public void InitializeCharacter3DViewModelRendering()
        {
            Character3DViewModelRendering =
                GameObject.Instantiate(_characters3DViewRenderingPrefab, _characters3DViewRenderingObjectPosition, Quaternion.identity);
            CharacterPortraitCamera = Character3DViewModelRendering.transform.Find("CameraForPortrait").GetComponent<Camera>();
            CharacterPortraitCamera.enabled = false;
        }

        public Sprite GetCharacterPortrait()
        {
            RenderTexture portraitRenderTexture = CharacterPortraitCamera.targetTexture;
            Rect portraitRect = new Rect(0, 0, portraitRenderTexture.width, portraitRenderTexture.height);

            RenderTexture currentRenderTexture = RenderTexture.active;
            RenderTexture.active = portraitRenderTexture;

            CharacterPortraitCamera.Render();

            Texture2D portraitTexture = new Texture2D(portraitRenderTexture.width, portraitRenderTexture.height);
            portraitTexture.ReadPixels(portraitRect, 0, 0);
            portraitTexture.Apply();

            RenderTexture.active = currentRenderTexture;
            CharacterPortraitCamera.enabled = false;
            portraitRenderTexture.Release();

            return Sprite.Create(portraitTexture, portraitRect, new Vector2());
        }

        public void BindModuleToCharacter(GameObject module, GameObject characterModel)
        {
            SkinnedMeshRenderer skinnedMeshRenderer = module.GetComponent<SkinnedMeshRenderer>();
            if (skinnedMeshRenderer != null)
            {
                Transform[] bonesFromOriginal = skinnedMeshRenderer.bones;
                Transform rootBoneFromOriginal = skinnedMeshRenderer.rootBone;

                Transform newRootBone = characterModel.transform.FindDeep(rootBoneFromOriginal.name);
                Transform[] allBonesInNewRootBone = newRootBone.GetComponentsInChildren<Transform>();

                Transform[] newBones = new Transform[bonesFromOriginal.Length];

                for (int i = 0; i < bonesFromOriginal.Length; i++)
                {
                    for (int j = 0; j < allBonesInNewRootBone.Length; j++)
                    {
                        if (bonesFromOriginal[i].name == allBonesInNewRootBone[j].name)
                        {
                            newBones[i] = allBonesInNewRootBone[j];
                        }
                    }
                }

                skinnedMeshRenderer.bones = newBones;
                skinnedMeshRenderer.rootBone = newRootBone;
            }
            else
            {
                Debug.LogError($"{module.name} not contain SkinnedMeshRenderer component");
            }
        }

        public List<GameObject> GetModulePartsByNames(IEnumerable<string> modulePartsNames)
        {
            List<GameObject> moduleParts = new List<GameObject>();
            if (modulePartsNames != null)
            {
                foreach (string modulePartName in modulePartsNames)
                {
                    GameObject modulePart = GetModulePartByName(modulePartName);
                    if (modulePart != null)
                    {
                        moduleParts.Add(modulePart);
                    }
                }
            }
            return moduleParts;
        }

        public GameObject GetModulePartByName(string modulePartName)
        {
            Transform transform = _modularCharactersPrefab.transform.FindDeep(modulePartName);
            if (transform != null)
            {
                return transform.gameObject;
            }
            else
            {
                Debug.LogError($"Module {modulePartName} is not found in {_modularCharactersPrefab.name}");
                return null;
            }
        }

        #endregion
    }
}
