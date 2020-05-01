using Models.NPCScripts.Enemy;

namespace Models.ConditionsAndActions.Helpers.Components
{
    public delegate void StatusProperty(ConditionArgs Args);

    public delegate void ConditionsUpdate(BaseCharacterModel CharacterModel,
        ref EnemySpecifications enemySpecifications, float deltaTime);
}