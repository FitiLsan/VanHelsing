namespace BeastHunterHubUI
{
    public abstract class BaseCharacterStorage : BaseStorage<CharacterModel, CharacterStorageType>
    {
        public BaseCharacterStorage(CharacterStorageType storageType) : base(storageType) { }
    }
}
