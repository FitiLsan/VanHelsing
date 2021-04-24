using System;
using UnityEngine;

namespace BeastHunterHubUI
{
    [Serializable]
    public struct MapDataStruct
    {
        #region Fields

        [Header("Prefabs")]
        [SerializeField] private GameObject _citizenUIPrefab;
        [SerializeField] private GameObject _locationTextUIPrefab;
        [SerializeField] private GameObject _characterUIPrefab;
        [SerializeField] private GameObject _equipmentSlotUIPrefab;
        [SerializeField] private GameObject _inventorySlotUIPrefab;
        [SerializeField] private GameObject _shopSlotUIPrefab;
        [SerializeField] private GameObject _answerButtonUIPrefab;
        [SerializeField] private GameObject _characters3DViewRenderingPrefab;
        [SerializeField] private GameObject _characterBackpuckSlotUIPrefab;

        [Header("Settings")]
        [SerializeField] private bool _mapOnStartEnabled;
        [SerializeField] private float _tooltipShowingDelay;
        [SerializeField] private float _charactersPanelSwipeStep;
        [SerializeField] private Vector3 _characters3DViewRenderingObjectPosition;

        [Header("Objects on map")]
        [SerializeField] private MapObjectData[] _mapObjects;

        [Header("Equipment slot type sprites")]
        [SerializeField] Sprite _weaponSlotIcon;
        [SerializeField] Sprite _armSlotIcon;
        [SerializeField] Sprite _backSlotIcon;
        [SerializeField] Sprite _handSlotIcon;
        [SerializeField] Sprite _headSlotIcon;
        [SerializeField] Sprite _hipsSlotIcon;
        [SerializeField] Sprite _legSlotIcon;
        [SerializeField] Sprite _shoulderSlotIcon;
        [SerializeField] Sprite _torsoSlotIcon;
        [SerializeField] Sprite _ringSlotIcon;
        [SerializeField] Sprite _amuletSlotIcon;
        [SerializeField] Sprite _beltSlotIcon;
        [SerializeField] Sprite _pocketItemSlotIcon;

        #endregion


        #region Properties

        public GameObject CitizenUIPrefab => _citizenUIPrefab;
        public GameObject ShopSlotUIPrefab => _shopSlotUIPrefab;
        public GameObject LocationTextUIPrefab => _locationTextUIPrefab;
        public GameObject CharacterUIPrefab => _characterUIPrefab;
        public GameObject EquipmentSlotUIPrefab => _equipmentSlotUIPrefab;
        public GameObject InventorySlotUIPrefab => _inventorySlotUIPrefab;
        public GameObject AnswerButtonUIPrefab => _answerButtonUIPrefab;
        public GameObject Characters3DViewRenderingPrefab => _characters3DViewRenderingPrefab;
        public Vector3 Characters3DViewRenderingObjectPosition => _characters3DViewRenderingObjectPosition;
        public GameObject CharacterBackpuckSlotUIPrefab => _characterBackpuckSlotUIPrefab;
        public bool MapOnStartEnabled => _mapOnStartEnabled;
        public MapObjectData[] MapObjects => _mapObjects;
        public Sprite WeaponSlotIcon => _weaponSlotIcon;
        public Sprite PocketItemSlotIcon => _pocketItemSlotIcon;
        public float TooltipShowingDelay => _tooltipShowingDelay;
        public float CharactersPanelSwipeStep => _charactersPanelSwipeStep;

        #endregion


        #region Methods

        public Sprite GetClothSlotSpriteByType(ClothesType clothType)
        {
            switch (clothType)
            {
                case ClothesType.Arms: return _armSlotIcon;
                case ClothesType.Back: return _backSlotIcon;
                case ClothesType.Hands: return _handSlotIcon;
                case ClothesType.Head: return _headSlotIcon;
                case ClothesType.Hips: return _hipsSlotIcon;
                case ClothesType.Legs: return _legSlotIcon;
                case ClothesType.Shoulders: return _shoulderSlotIcon;
                case ClothesType.Torso: return _torsoSlotIcon;
                case ClothesType.Ring: return _ringSlotIcon;
                case ClothesType.Amulet: return _amuletSlotIcon;
                case ClothesType.Belt: return _beltSlotIcon;
                default:
                    Debug.LogError(this + ": incorrect cloth type");
                    return null;
            }
        }

        #endregion
    }
}
