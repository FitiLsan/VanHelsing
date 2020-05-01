using System;
using System.Collections.Generic;
using Character;
using Common;
using Events;
using Events.Args;
using Extensions;
using Items;
using UnityEngine;

namespace DatabaseWrapper
{
    public static class ItemTemplateRepository
    {
        private static Dictionary<int, ItemDto> _cache = new Dictionary<int, ItemDto>();
        private static Locale _locale = Locale.RU;

        private static readonly Dictionary<Locale, string> _localeTables = new Dictionary<Locale, string>
        {
            //TODO: add another languages
            {Locale.RU, "item_template_locale_ru"}
        };

        /// <summary>
        /// Получаем предмет из базы по Ид
        /// </summary>
        /// <param name="id">Ид желаемого предмета</param>
        /// <returns></returns>
        /// <exception cref="Exception">Исключения при работе с базой</exception>
        public static ItemDto GetById(int id)
        {
            if (_cache.ContainsKey(id)) return _cache[id];
            try
            {
                var dt = DatabaseWrapper.GetTable($"select * from 'item_template' where Id={id} limit 1;");
                if (dt == null || dt.Rows.Count == 0) throw new Exception($"There is no item with id[{id}]");
                var dr = dt.Rows[0];
                var item = new ItemDto
                {
                    Id = dr.GetInt(ITEM_TEMPLATE_ID),
                    Type = (ItemClasses) dr.GetInt(ITEM_TEMPLATE_CLASS),
                    ModelId = dr.GetInt(ITEM_TEMPLATE_MODELID),
                    IconId = dr.GetInt(ITEM_TEMPLATE_ICONID),
                    Flags = (ItemFlags) dr.GetInt(ITEM_TEMPLATE_FLAGS),
                    Quality = (ItemQuality) dr.GetInt(ITEM_TEMPLATE_QUALITY),
                    BuyPrice = dr.GetInt(ITEM_TEMPLATE_BUYPRICE),
                    SellPrice = dr.GetInt(ITEM_TEMPLATE_SELLPRICE),
                    SlotType = (InventorySlots) dr.GetInt(ITEM_TEMPLATE_INVENTORYSLOT),
                    Level = dr.GetInt(ITEM_TEMPLATE_ITEMLEVEL),
                    RequiredLevel = dr.GetInt(ITEM_TEMPLATE_REQUIREDLEVEL),
                    MaxCount = dr.GetInt(ITEM_TEMPLATE_MAXCOUNT),
                    Weight = dr.GetFloat(ITEM_TEMPLATE_WEIGHT),
                    AdditionalWeight = dr.GetFloat(ITEM_TEMPLATE_ADDITIONALWEIGHT),
                    DmgMin1 = dr.GetInt(ITEM_TEMPLATE_DMGMIN1),
                    DmgMax1 = dr.GetInt(ITEM_TEMPLATE_DMGMAX1),
                    DmgType1 = (DamageTypes) dr.GetInt(ITEM_TEMPLATE_DMGTYPE1),
                    DmgMin2 = dr.GetInt(ITEM_TEMPLATE_DMGMIN2),
                    DmgMax2 = dr.GetInt(ITEM_TEMPLATE_DMGMAX2),
                    DmgType2 = (DamageTypes) dr.GetInt(ITEM_TEMPLATE_DMGTYPE2),
                    Armor = dr.GetInt(ITEM_TEMPLATE_ARMOR),
                    BaseDelay = dr.GetInt(ITEM_TEMPLATE_DELAY),
                    AmmoType = (ItemClasses) dr.GetInt(ITEM_TEMPLATE_AMMOTYPE),
                    Range = dr.GetFloat(ITEM_TEMPLATE_RANGE),
                    PageId = dr.GetInt(ITEM_TEMPLATE_PAGEID),
                    StartQuest = dr.GetInt(ITEM_TEMPLATE_STARTQUEST),
                    Sheath = (SheathTypes) dr.GetInt(ITEM_TEMPLATE_SHEATH),
                    LockId = dr.GetInt(ITEM_TEMPLATE_LOCKID),
                    BaseBlockChance = dr.GetFloat(ITEM_TEMPLATE_BLOCKCHANCE),
                    BaseBlockValue = dr.GetInt(ITEM_TEMPLATE_BLOCKVALUE),
                    SetId = dr.GetInt(ITEM_TEMPLATE_ITEMSET),
                    MaxDurability = dr.GetInt(ITEM_TEMPLATE_MAXDURABILITY),
                    ScriptName = dr.GetString(ITEM_TEMPLATE_SCRIPTNAME),
                    MaxDuration = dr.GetInt(ITEM_TEMPLATE_DURATION),
                    MaxStack = dr.GetInt(ITEM_TEMPLATE_MAXSTACK)
                };
                //For 0 - num of zones in db
                for (var i = 0; i < 3; i++) item.UsableInZones.Add(dr.GetInt(ITEM_TEMPLATE_ZONEID1 + i));
                //Required Stats
                for (var i = 0; i < 3; i++)
                {
                    var t = dr.GetInt(ITEM_TEMPLATE_REQUIREDSTATTYPE1 + i * 2);
                    if (t == 0) continue;
                    var v = dr.GetInt(ITEM_TEMPLATE_REQUIREDSTATVALUE1 + i * 2);
                    item.RequiredStats.Add((Stats) t, v);
                }

                //BonusStats
                for (var i = 0; i < 6; i++)
                {
                    var t = dr.GetInt(ITEM_TEMPLATE_STATTYPE1 + i * 2);
                    if (t == 0) continue;
                    var v = dr.GetInt(ITEM_TEMPLATE_STATVALUE1 + i * 2);
                    item.BonusStats.Add((Stats) t, v);
                }
                //TODO: Spells and effects

                var dtl = DatabaseWrapper.GetTable(
                    $"select * from '{_localeTables[GetCurrentLocale()]}' where ItemId={item.Id} limit 1;");
                var drl = dtl.Rows[0];
                item.Name = drl.GetString(ITEM_TEMPLATE_LOCALE_TITLE);
                item.FlavorText = drl.GetString(ITEM_TEMPLATE_LOCALE_FLAVORTEXT);

                _cache.Add(item.Id, item);
                return item;
            }
            catch (Exception e)
            {
                Debug.LogError($"{DateTime.Now.ToShortTimeString()}    Item with id[{id}] fires exception    {e}\n");
                throw;
            }
        }

        /// <summary>
        /// Инитиализация внутренних систем. Необходимо перед работой.
        /// </summary>
        public static void Init()
        {
            EventManager.StartListening(GameEventTypes.ItemDrop, OnItemDrop);
        }

        private static void OnItemDrop(EventArgs arg0)
        {
            if (!(arg0 is ItemArgs idArgs)) return;
            if (_cache.ContainsKey(idArgs.Item.Id))
                _cache.Remove(idArgs.Item.Id);
        }

        /// <summary>
        ///     Устанавливаем язык
        /// </summary>
        /// <param name="locale"></param>
        public static void SetLocale(Locale locale)
        {
            _locale = locale;
        }

        /// <summary>
        ///     Чистим in-memory cache
        /// </summary>
        public static void ClearCache()
        {
            _cache = new Dictionary<int, ItemDto>();
        }

        /// <summary>
        ///     Получаем язык, который установлен в этом классе
        /// </summary>
        /// <returns></returns>
        public static Locale GetCurrentLocale()
        {
            return _locale;
        }

        #region ColumnDefinitions

        //Table : item_template
        private const int ITEM_TEMPLATE_ID = 0;
        private const int ITEM_TEMPLATE_CLASS = 1;
        private const int ITEM_TEMPLATE_MODELID = 2;
        private const int ITEM_TEMPLATE_ICONID = 3;
        private const int ITEM_TEMPLATE_QUALITY = 4;
        private const int ITEM_TEMPLATE_FLAGS = 5;
        private const int ITEM_TEMPLATE_BUYPRICE = 6;
        private const int ITEM_TEMPLATE_SELLPRICE = 7;
        private const int ITEM_TEMPLATE_INVENTORYSLOT = 8;
        private const int ITEM_TEMPLATE_ITEMLEVEL = 9;
        private const int ITEM_TEMPLATE_REQUIREDLEVEL = 10;
        private const int ITEM_TEMPLATE_MAXCOUNT = 11;
        private const int ITEM_TEMPLATE_WEIGHT = 12;
        private const int ITEM_TEMPLATE_ADDITIONALWEIGHT = 13;
        private const int ITEM_TEMPLATE_REQUIREDSTATTYPE1 = 14;
        private const int ITEM_TEMPLATE_REQUIREDSTATVALUE1 = 15;
        private const int ITEM_TEMPLATE_REQUIREDSTATTYPE2 = 16;
        private const int ITEM_TEMPLATE_REQUIREDSTATVALUE2 = 17;
        private const int ITEM_TEMPLATE_REQUIREDSTATTYPE3 = 18;
        private const int ITEM_TEMPLATE_REQUIREDSTATVALUE3 = 19;
        private const int ITEM_TEMPLATE_STATTYPE1 = 20;
        private const int ITEM_TEMPLATE_STATVALUE1 = 21;
        private const int ITEM_TEMPLATE_STATTYPE2 = 22;
        private const int ITEM_TEMPLATE_STATVALUE2 = 23;
        private const int ITEM_TEMPLATE_STATTYPE3 = 24;
        private const int ITEM_TEMPLATE_STATVALUE3 = 25;
        private const int ITEM_TEMPLATE_STATTYPE4 = 26;
        private const int ITEM_TEMPLATE_STATVALUE4 = 27;
        private const int ITEM_TEMPLATE_STATTYPE5 = 28;
        private const int ITEM_TEMPLATE_STATVALUE5 = 29;
        private const int ITEM_TEMPLATE_STATTYPE6 = 30;
        private const int ITEM_TEMPLATE_STATVALUE6 = 31;
        private const int ITEM_TEMPLATE_DMGMIN1 = 32;
        private const int ITEM_TEMPLATE_DMGMAX1 = 33;
        private const int ITEM_TEMPLATE_DMGTYPE1 = 34;
        private const int ITEM_TEMPLATE_DMGMIN2 = 35;
        private const int ITEM_TEMPLATE_DMGMAX2 = 36;
        private const int ITEM_TEMPLATE_DMGTYPE2 = 37;
        private const int ITEM_TEMPLATE_ARMOR = 38;
        private const int ITEM_TEMPLATE_DELAY = 39;
        private const int ITEM_TEMPLATE_AMMOTYPE = 40;
        private const int ITEM_TEMPLATE_RANGE = 41;
        private const int ITEM_TEMPLATE_SPELLID1 = 42;
        private const int ITEM_TEMPLATE_SPELLTRIGGER1 = 43;
        private const int ITEM_TEMPLATE_SPELLPROC1 = 44;
        private const int ITEM_TEMPLATE_SPELLCOOLDOWN1 = 45;
        private const int ITEM_TEMPLATE_SPELLSHAREGROUP1 = 46;
        private const int ITEM_TEMPLATE_SPELLCHARGES1 = 47;
        private const int ITEM_TEMPLATE_SPELLID2 = 48;
        private const int ITEM_TEMPLATE_SPELLTRIGGER2 = 49;
        private const int ITEM_TEMPLATE_SPELLPROC2 = 50;
        private const int ITEM_TEMPLATE_SPELLCOOLDOWN2 = 51;
        private const int ITEM_TEMPLATE_SPELLSHAREGROUP2 = 52;
        private const int ITEM_TEMPLATE_SPELLCHARGES2 = 53;
        private const int ITEM_TEMPLATE_PAGEID = 54;
        private const int ITEM_TEMPLATE_STARTQUEST = 55;
        private const int ITEM_TEMPLATE_SHEATH = 56;
        private const int ITEM_TEMPLATE_LOCKID = 57;
        private const int ITEM_TEMPLATE_BLOCKCHANCE = 58;
        private const int ITEM_TEMPLATE_BLOCKVALUE = 59;
        private const int ITEM_TEMPLATE_ITEMSET = 60;
        private const int ITEM_TEMPLATE_MAXDURABILITY = 61;
        private const int ITEM_TEMPLATE_ZONEID1 = 62;
        private const int ITEM_TEMPLATE_ZONEID2 = 63;
        private const int ITEM_TEMPLATE_ZONEID3 = 64;
        private const int ITEM_TEMPLATE_DURATION = 65;
        private const int ITEM_TEMPLATE_SCRIPTNAME = 66;
        private const int ITEM_TEMPLATE_MAXSTACK = 67;

        //Table : item_template_locale_xx
        private const int ITEM_TEMPLATE_LOCALE_ID = 0;
        private const int ITEM_TEMPLATE_LOCALE_ITEMID = 1;
        private const int ITEM_TEMPLATE_LOCALE_TITLE = 2;
        private const int ITEM_TEMPLATE_LOCALE_FLAVORTEXT = 3;

        #endregion
    }
}