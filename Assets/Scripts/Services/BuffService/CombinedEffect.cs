namespace BeastHunter
{
    public struct CombinedEffect
    {
        public BuffEffectType firstEffect;
        public BuffEffectType secondEffect;

        public CombinedEffect(BuffEffectType firstEffect, BuffEffectType secondEffect)
        {
            this.firstEffect = firstEffect;
            this.secondEffect = secondEffect;
        }

    }
}