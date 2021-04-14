using UnityEngine;


namespace BeastHunter
{
    public class HubMapUIShopService
    {
        private const float SELL_PRICE_PERCENT = 0.6f;


        public int CountSellPrice(HubMapUIBaseItemModel item)
        {
            return CountSellPrice(GetItemPrice(item));
        }

        public int GetItemPrice(HubMapUIBaseItemModel item)
        {
            return item.ShopPrice;
        }

        private int CountSellPrice(int fullPrice)
        {
            return Mathf.RoundToInt(fullPrice * SELL_PRICE_PERCENT);
        }
    }
}
