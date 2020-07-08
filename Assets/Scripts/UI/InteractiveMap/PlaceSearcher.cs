using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeastHunter
{

    public sealed class PlaceSearcher
    {
        public GameContext Context;
        public PlaceSearcher(GameContext context)
        {
            Context = context;
            ToSearchPlace.ToSearchPlaceEvent += OnSearch;
        }

        public void OnSearch(List<ItemInfo> itemList)
        {
            foreach (var item in itemList)
            {
                if (item.ItemType == ItemType.QuestItem)
                {
                    if (Context.QuestModel.ActiveQuests.Contains(item.QuestId))
                    {
                        Services.SharedInstance.EventManager.TriggerEvent(GameEventTypes.ItemAcquired, new ItemArgs(item.ItemId));
                    }
                }
            }
        }
    }
}