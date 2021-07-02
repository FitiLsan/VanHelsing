using System.Collections.Generic;
using UnityEngine;


namespace BeastHunterHubUI
{
    public class RandomCharacterService
    {
        public List<CharacterModel> Get(int amount = 1)
        {
            List<CharacterModel> list = new List<CharacterModel>();

            if (amount > 0)
            {
                CharacterRandomizer randomizer = new CharacterRandomizer();

                for (int i = 0; i < amount; i++)
                {
                    CharacterData characterStruct = GetRandomCharacterStruct(randomizer);
                    CharacterModel model = new CharacterModel(characterStruct);
                    list.Add(model);
                }
            }
            else
            {
                Debug.LogError("Incorrect input parameter: the value must be greater than zero");
            }

            return list;
        }

        private CharacterData GetRandomCharacterStruct(CharacterRandomizer randomizer)
        {
            CharacterData characterStruct = new CharacterData();
            bool isFemale = randomizer.IsFemale() ? true : false;
            int rank = randomizer.GetRank();
            ClothesItemSO[] clothes = randomizer.GetRandomClothes(rank).ToArray();
            int pocketsAmount = CountPockets(clothes);

            characterStruct.Rank = rank;
            characterStruct.IsFemale = isFemale;
            characterStruct.Name = randomizer.GetRandomNameFromPool(isFemale);
            characterStruct.DefaultMaterial = randomizer.GetRandomMaterialFromPool();

            characterStruct.BackpackItems = randomizer.GetRandomBackpackItems().ToArray();
            characterStruct.WeaponEquipmentItems = randomizer.GetRandomWeapon(rank).ToArray();
            characterStruct.ClothesEquipmentItems = clothes;
            characterStruct.PocketItems = randomizer.GetRandomPocketItems(pocketsAmount).ToArray();
            characterStruct.DefaultHeadParts = randomizer.GetDefaultHeadModuleParts(isFemale).ToArray();
            characterStruct.DefaultModuleParts = randomizer.GetDefaultBodyModules(isFemale).ToArray();
            characterStruct.Skills = randomizer.GetSkills().ToArray();

            return characterStruct;
        }

        private int CountPockets(IEnumerable<ClothesItemSO> clothes)
        {
            int pocketsAmount = 0;

            foreach (ClothesItemSO cloth in clothes)
            {
                pocketsAmount += cloth.PocketsAmount;
            }

            return pocketsAmount;
        }
    }
}
