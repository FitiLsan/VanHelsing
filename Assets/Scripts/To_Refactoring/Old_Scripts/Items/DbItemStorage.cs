using System.Collections.Generic;
using DatabaseWrapper;
using Interfaces;
using UnityEngine;

namespace Items
{
    public class DbItemStorage : IItemStorage
    {
        public Item GetItemById(int id)
        {
            return new Item(ItemTemplateRepository.GetById(id));
        }

        public void NewEntry(Item item)
        {
            Debug.LogError("Not implemented yet (save/load system required)");
        }

        public List<Item> LoadInventory()
        {
            Debug.LogError("Not implemented yet (save/load system required)");
            return new List<Item>();
        }

        public Dictionary<int, Item> LoadEquipment()
        {
            Debug.LogError("Not implemented yet (save/load system required)");
            return new Dictionary<int, Item>
            {
                {0, null}, //Head
                {1, null}, //Neck
                {2, null}, //Shoulder
                {3, null}, //Chest,
                {4, null}, //Waist
                {5, null}, //Legs
                {6, null}, //Feet
                {7, null}, //Wrist
                {8, null}, //Hand
                {9, null}, //Finger1
                {10, null}, //Finger2
                {11, null}, //Trinket1
                {12, null}, //Trinket2
                {13, null}, //Back
                {14, null}, //MainHand or two-handed
                {15, null}, //OffHand
                {16, null}, //Ammo
                {17, null}, //Bag1
                {18, null}, //Bag2
                {19, null} //Bag3
            };
        }

        public void SaveEquipment(Dictionary<int, Item> equipment)
        {
            Debug.LogError("Not implemented yet (save/load system required)");
        }

        public void SaveInventory(List<Item> inventory)
        {
            Debug.LogError("Not implemented yet (save/load system required)");
        }
    }
}