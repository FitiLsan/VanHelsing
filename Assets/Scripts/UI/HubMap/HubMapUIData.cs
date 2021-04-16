using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{
    [CreateAssetMenu(fileName = "HubMapData", menuName = "CreateData/HubMapData", order = 0)]
    public class HubMapUIData : ScriptableObject
    {
        #region Fields

        [Header("UI prefabs")]
        [SerializeField] private GameObject _citizenUIPrefab;
        [SerializeField] private GameObject _locationTextUIPrefab;
        [SerializeField] private GameObject _characterUIPrefab;
        [SerializeField] private GameObject _equipmentSlotUIPrefab;
        [SerializeField] private GameObject _inventorySlotUIPrefab;
        [SerializeField] private GameObject _shopSlotUIPrefab;
        [SerializeField] private GameObject _answerButtonUIPrefab;
        [SerializeField] private GameObject _characters3DViewRenderingPrefab;
        [SerializeField] private GameObject _characterBackpuckSlotUIPrefab;

        [Header("UI settings")]
        [SerializeField] private bool _mapOnStartEnabled;
        [SerializeField] private Vector3 _characters3DViewRenderingObjectPosition;

        [Header("Objects on map")]
        [SerializeField] private HubMapUIMapObjectData[] _mapObjects;

        [Header("Game content for UI")]
        [SerializeField] private HubMapUIContextData _contextData;

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

        public HubMapUIMapObjectData[] MapObjects => (HubMapUIMapObjectData[])_mapObjects.Clone();

        public HubMapUIContextData ContextData => _contextData;

        public Sprite WeaponSlotIcon => _weaponSlotIcon;
        public Sprite PocketItemSlotIcon => _pocketItemSlotIcon;

        #endregion


        #region Methods

        public Sprite GetClothSlotSpriteByType(HubMapUIClothesType clothType)
        {
            switch (clothType)
            {
                case HubMapUIClothesType.Arms: return _armSlotIcon;
                case HubMapUIClothesType.Back: return _backSlotIcon;
                case HubMapUIClothesType.Hands: return _handSlotIcon;
                case HubMapUIClothesType.Head: return _headSlotIcon;
                case HubMapUIClothesType.Hips: return _hipsSlotIcon;
                case HubMapUIClothesType.Legs: return _legSlotIcon;
                case HubMapUIClothesType.Shoulders: return _shoulderSlotIcon;
                case HubMapUIClothesType.Torso: return _torsoSlotIcon;
                case HubMapUIClothesType.Ring: return _ringSlotIcon;
                case HubMapUIClothesType.Amulet: return _amuletSlotIcon;
                case HubMapUIClothesType.Belt: return _beltSlotIcon;
                default:
                    Debug.LogError(this + ": incorrect cloth type");
                    return null;
            }
        }

        #endregion
    }
}
