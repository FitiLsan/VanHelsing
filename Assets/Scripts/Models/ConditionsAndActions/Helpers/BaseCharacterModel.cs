namespace Models.ConditionsAndActions.Helpers
{
    /// <summary>
    ///     Базовая модель персонажа
    /// </summary>
    public class BaseCharacterModel
    {
        public float Health;
        public float RunSpeed;
        public float Speed;

        public BaseCharacterModel(float Health, float Speed, float RunSpeed)
        {
            this.Health = Health;
            this.Speed = Speed;
            this.RunSpeed = RunSpeed;
        }
    }
}