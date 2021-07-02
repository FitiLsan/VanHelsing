using System.Text;
using UnityEngine;


namespace BeastHunterHubUI
{
    public class ShopService
    {
        #region Constants

        private const float SELL_PRICE_PERCENT = 0.6f;

        #endregion


        private HubUIContext _context;


        public ShopService(HubUIContext context)
        {
            _context = context;
        }


        #region Methods

        public int CountSellPrice(BaseItemModel item)
        {
            return CountSellPrice(GetItemPrice(item));
        }

        public int GetItemPrice(BaseItemModel item)
        {
            return item.ShopPrice;
        }

        private int CountSellPrice(int fullPrice)
        {
            return Mathf.RoundToInt(fullPrice * SELL_PRICE_PERCENT);
        }

        public bool IsPossibleToBuyShopItem(BaseItemModel item, CityModel city)
        {
            if (item != null)
            {
                return
                    item.RequiredReputationForSaleInShop <= city.PlayerReputation &&
                    _context.Player.GoldAmount >= GetItemPrice(item);
            }
            else
            {
                return true;
            }
        }

        public bool IsPossibleToBuyShopItem(BaseItemModel item, CityModel city, out string message)
        {
            bool flag = true;
            StringBuilder sb = new StringBuilder();

            if (item != null)
            {
                if (city.PlayerReputation < item.RequiredReputationForSaleInShop)
                {
                    flag = false;

                    sb.AppendFormat("Недостаточно репутации для покупки");
                    sb.AppendLine();
                    sb.AppendFormat($"Необходимая репутация: {item.RequiredReputationForSaleInShop}");
                }
                if (_context.Player.GoldAmount < GetItemPrice(item))
                {
                    flag = false;

                    if (sb.Length > 0)
                    {
                        sb.AppendLine();
                    }
                    sb.AppendFormat("Недостаточно денег");
                }
            }

            message = sb.ToString();
            return flag;
        }

        public void SellItem(CityModel city, int inventorySlotIndex)
        {
            ItemSlotStorage playerInventory = _context.Player.Inventory;
            BaseItemModel sellingItem = playerInventory.GetElementBySlot(inventorySlotIndex);

            if (sellingItem != null)
            {
                if (city.BuyBackStorage.PutElementToFirstEmptySlot(sellingItem))
                {
                    playerInventory.RemoveElement(inventorySlotIndex);
                    _context.Player.AddGold(CountSellPrice(sellingItem));
                }
                else
                {
                    HubUIServices.SharedInstance.GameMessages.Notice("BuyBack storage is full");
                }
            }
        }

        public void BuyBackItem(CityModel city, int buyBackStorageSlotIndex)
        {
            ItemSlotStorage buyBackStorage = city.BuyBackStorage;
            BaseItemModel buyingItem = buyBackStorage.GetElementBySlot(buyBackStorageSlotIndex);
            if (buyingItem != null)
            {
                if (_context.Player.GoldAmount >= CountSellPrice(buyingItem))
                {
                    if (_context.Player.Inventory.PutElementToFirstEmptySlot(buyingItem))
                    {
                        buyBackStorage.RemoveElement(buyBackStorageSlotIndex);
                        _context.Player.TakeGold(CountSellPrice(buyingItem));
                    }
                    else
                    {
                        HubUIServices.SharedInstance.GameMessages.Notice("Inventory storage is full");
                    }
                }
                else
                {
                    HubUIServices.SharedInstance.GameMessages.Notice("Not enough gold");
                }
            }
        }

        public void BuyItem(CityModel city, int shopStorageSlotIndex)
        {
            ItemSlotStorage shopStorage = city.ShopStorage;
            BaseItemModel buyingItem = shopStorage.GetElementBySlot(shopStorageSlotIndex);

            if (buyingItem != null)
            {
                if (IsPossibleToBuyShopItem(buyingItem, city))
                {
                    if (_context.Player.Inventory.PutElementToFirstEmptySlot(buyingItem))
                    {
                        shopStorage.RemoveElement(shopStorageSlotIndex);
                        _context.Player.TakeGold(GetItemPrice(buyingItem));
                    }
                    else
                    {
                        HubUIServices.SharedInstance.GameMessages.Notice("Inventory storage is full");
                    }
                }
                else
                {
                    HubUIServices.SharedInstance.GameMessages.Notice("Not enough gold");
                }
            }
        }

        #endregion
    }
}
