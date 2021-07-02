using System;
using System.Collections.Generic;
using UnityEngine;


namespace BeastHunterHubUI
{
    [CreateAssetMenu(fileName = "ItemDataPools", menuName = "CreateData/HubUIData/ItemDataPools", order = 0)]
    public class ItemSOPools : ScriptableObject
    {
        #region Fields

        [SerializeField] private ClothesItemSO[] _clothesItemsPool;
        [SerializeField] private WeaponItemSO[] _weaponItemsPool;
        [SerializeField] private PocketItemSO[] _pocketItemsPool;

        private Dictionary<int, List<WeaponItemSO>> _weaponDataPoolDic;
        private Dictionary<int, Dictionary<ClothesType, List<ClothesItemSO>>> _clothesDataPoolDic;

        #endregion


        #region UnityMethods

        private void OnEnable()
        {
            InitializeWeaponPoolDictionary();
            InitializeClothesPoolDictionary();
        }

        private void OnDisable()
        {
            _weaponDataPoolDic.Clear();
            _clothesDataPoolDic.Clear();

            _weaponDataPoolDic = null;
            _weaponDataPoolDic = null;
        }

        #endregion


        #region Methods

        public PocketItemSO GetRandomPocketItem()
        {
            return _pocketItemsPool[UnityEngine.Random.Range(0, _pocketItemsPool.Length)];
        }

        public List<ClothesItemSO> GetClothesSOListByRankAndType(int rank, ClothesType clothesType)
        {
            if (_clothesDataPoolDic.ContainsKey(rank))
            {
                return _clothesDataPoolDic[rank][clothesType];
            }
            else
            {
                Debug.LogError("No clothes of the requested rank");
                return null;
            }
        }

        public ClothesItemSO GetRandomClothesSOByRankAndType(int rank, ClothesType clothesType)
        {
            List<ClothesItemSO> clothesList = GetClothesSOListByRankAndType(rank, clothesType);
            return clothesList[UnityEngine.Random.Range(0, clothesList.Count)];
        }

        public List<WeaponItemSO> GetWeaponSOListByRank(int rank)
        {
            if (_weaponDataPoolDic.ContainsKey(rank))
            {
                return _weaponDataPoolDic[rank];
            }
            else
            {
                Debug.LogError("No weapon of the requested rank");
                return null;
            }
        }

        public WeaponItemSO GetRandomWeaponSOByRank(int rank)
        {
            List<WeaponItemSO> weaponDataList = GetWeaponSOListByRank(rank);
            return weaponDataList[UnityEngine.Random.Range(0, weaponDataList.Count)];
        }

        public WeaponItemSO GetRandomWeaponSOByRanks(int[] ranks)
        {
            List<int> ranksChecked = new List<int>();

            for (int i = 0; i < ranks.Length; i++)
            {
                if (_weaponDataPoolDic.ContainsKey(ranks[i]))
                {
                    ranksChecked.Add(ranks[i]);
                }
            }

            int randomRankIndex = UnityEngine.Random.Range(0, ranksChecked.Count);
            return GetRandomWeaponSOByRank(randomRankIndex);
        }

        public ClothesItemSO GetRandomClothesSOByRanksAndType(int[] ranks, ClothesType clothesType)
        {
            List<int> ranksChecked = new List<int>();

            for (int i = 0; i < ranks.Length; i++)
            {
                if (_clothesDataPoolDic.ContainsKey(ranks[i]))
                {
                    ranksChecked.Add(ranks[i]);
                }
            }

            int randomRankIndex = UnityEngine.Random.Range(0, ranksChecked.Count);
            return GetRandomClothesSOByRankAndType(randomRankIndex, clothesType);
        }

        private void InitializeWeaponPoolDictionary()
        {
            _weaponDataPoolDic = new Dictionary<int, List<WeaponItemSO>>();

            if(_weaponItemsPool != null)
            {
                for (int i = 0; i < _weaponItemsPool.Length; i++)
                {
                    int itemRank = _weaponItemsPool[i].Rank;
                    if (!_weaponDataPoolDic.ContainsKey(itemRank))
                    {
                        _weaponDataPoolDic.Add(itemRank, new List<WeaponItemSO>());
                    }
                    _weaponDataPoolDic[itemRank].Add(_weaponItemsPool[i]);
                }
            }
        }

        private void InitializeClothesPoolDictionary()
        {
            _clothesDataPoolDic = new Dictionary<int, Dictionary<ClothesType, List<ClothesItemSO>>>();

            if(_clothesItemsPool != null)
            {
                for (int i = 0; i < _clothesItemsPool.Length; i++)
                {
                    int itemRank = _clothesItemsPool[i].Rank;
                    if (!_clothesDataPoolDic.ContainsKey(itemRank))
                    {
                        Dictionary<ClothesType, List<ClothesItemSO>> clothesTypesDic = new Dictionary<ClothesType, List<ClothesItemSO>>();
                        foreach (ClothesType clothesType in Enum.GetValues(typeof(ClothesType)))
                        {
                            clothesTypesDic.Add(clothesType, new List<ClothesItemSO>());
                        }
                        _clothesDataPoolDic.Add(itemRank, clothesTypesDic);

                    }
                    _clothesDataPoolDic[itemRank][_clothesItemsPool[i].ClothesType].Add(_clothesItemsPool[i]);
                }
            }
        }

        #endregion
    }
}
