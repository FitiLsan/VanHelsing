using System.Collections.Generic;
using DatabaseWrapper;
using Interfaces;
using UnityEngine;

namespace Items
{
    public class DbItemStorage : IItemStorage
    {
        private readonly IItemSaveAgent _agent;
        
        public DbItemStorage(IItemSaveAgent agent)
        {
            _agent = agent;
        }
        
        public Item GetItemById(int id)
        {
            return new Item(ItemTemplateRepository.GetById(id));
        }

        public void NewEntry(Item item)
        {
            _agent.NewEntry(item);
        }

        public List<Item> LoadInventory()
        {
            return _agent.LoadInventory();
        }

        public Dictionary<int, Item> LoadEquipment()
        {
            return _agent.LoadEquipment();
        }

        public void SaveEquipment(Dictionary<int, Item> equipment)
        {
            _agent.SaveEquipment(equipment);
        }

        public void SaveInventory(List<Item> inventory)
        {
            _agent.SaveInventory(inventory);
        }
    }
}