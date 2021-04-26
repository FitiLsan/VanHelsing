namespace BeastHunter
{
    public struct CombinedEffect
    {
        public EffectType firstEffect;
        public EffectType secondEffect;

        public CombinedEffect(EffectType firstEffect, EffectType secondEffect)
        {
            this.firstEffect = firstEffect;
            this.secondEffect = secondEffect;
        }

    }
}