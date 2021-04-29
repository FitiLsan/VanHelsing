namespace BeastHunter
{
    public sealed class AttackService : IService
    {
        #region Fields

        private readonly GameContext _context;
        private Damage _damage;
        private Stats _receiverStats;
        private Stats _dealerStats;

        #endregion


        #region ClassLifeCycles

        public AttackService(GameContext context)
        {
            _context = context;
            _damage = new Damage();
        }

        #endregion


        #region Methods

        public Damage CountDamage(Damage baseDamage, int receiverID, Stats dealerStats = null, WeaponData usedWeapon = null)
        {
            var powerModifier = true;
            if (dealerStats==null)
            {
                dealerStats = new Stats();
                powerModifier = false;
            }
                _dealerStats = dealerStats;
                _receiverStats = _context.CharacterModel.InstanceID == receiverID ?
                _context.CharacterModel.CurrentStats : _context.NpcModels[receiverID].CurrentStats;

            if (usedWeapon == null)
            {
                _damage.PhysicalDamageValue =
                    (baseDamage.PhysicalDamageValue + GetPhysicalPower(baseDamage.PhysicalDamageType, powerModifier)) * (1 - GetPhysicalResistance(baseDamage.PhysicalDamageType));
                _damage.ElementDamageValue =
                    (baseDamage.ElementDamageValue + GetElementPower(baseDamage.ElementDamageType, powerModifier)) * (1 - GetElementResistance(baseDamage.ElementDamageType));
            }
            else
            {
                switch (usedWeapon.Type)
                {
                    case WeaponType.Melee:
                        {
                            _damage.PhysicalDamageValue =
                                (baseDamage.PhysicalDamageValue + GetPhysicalPower(baseDamage.PhysicalDamageType, powerModifier)) * (1 - GetPhysicalResistance(baseDamage.PhysicalDamageType)) *
                                usedWeapon.CurrentAttack.WeaponItem.Weight;
                            _damage.ElementDamageValue =
                                (baseDamage.ElementDamageValue + GetElementPower(baseDamage.ElementDamageType, powerModifier)) * (1 - GetElementResistance(baseDamage.ElementDamageType)) *
                             usedWeapon.CurrentAttack.WeaponItem.Weight;
                        }
                        break;
                    case WeaponType.Shooting:
                        // What unique weapon value ? Mb distance ?
                        {
                            _damage.PhysicalDamageValue =
                                (baseDamage.PhysicalDamageValue + GetPhysicalPower(baseDamage.PhysicalDamageType, powerModifier)) * (1 - GetPhysicalResistance(baseDamage.PhysicalDamageType)); //* usedWeapon.CurrentAttack.WeaponItem.Weight;
                            _damage.ElementDamageValue =
                                (baseDamage.ElementDamageValue + GetElementPower(baseDamage.ElementDamageType, powerModifier)) * (1 - GetElementResistance(baseDamage.ElementDamageType)); //* usedWeapon.CurrentAttack.WeaponItem.Weight;
                        }
                        break;
                    case WeaponType.Throwing:
                        break;
                    default:
                        break;
                }
            }
            _damage.PhysicalDamageType = baseDamage.PhysicalDamageType;
            _damage.ElementDamageType = baseDamage.ElementDamageType;
            _damage.IsEffectDamage = baseDamage.IsEffectDamage;
            return _damage;
        }

        public void DealDamage(Damage countedDamage, int receiverID)
        {
            if(receiverID == _context.CharacterModel.InstanceID)
            {
                _context.CharacterModel.PlayerBehavior.TakeDamageEvent(countedDamage);
            }
            else if(_context.NpcModels.ContainsKey(receiverID))
            {
                _context.NpcModels[receiverID].TakeDamage(countedDamage);
            }
        }

        public void CountAndDealDamage(Damage baseDamage, int receiverID, Stats dealerStats = null, WeaponData usedWeapon = null)
        {
            var damage = CountDamage(baseDamage, receiverID, dealerStats, usedWeapon);
            DealDamage(damage, receiverID);
        }

        public float GetElementResistance(ElementDamageType type)
        {
            float resistanceValue;
            switch (type)
            {
                case ElementDamageType.Fire:
                    {
                         resistanceValue = _receiverStats.DefenceStats.FireDamageResistance;
                    }
                    break;
                case ElementDamageType.Water:
                    {
                        resistanceValue = _receiverStats.DefenceStats.WaterDamageResistance;
                    }
                    break;
                case ElementDamageType.Ice:
                    {
                        resistanceValue = _receiverStats.DefenceStats.IceDamageResistance;
                    }
                    break;
                case ElementDamageType.Electricity:
                    {
                        resistanceValue = _receiverStats.DefenceStats.ElectricityDamageResistance;
                    }
                    break;
                case ElementDamageType.Oil:
                    {
                        resistanceValue = _receiverStats.DefenceStats.OilDamageResistance;
                    }
                    break;
                case ElementDamageType.Toxin:
                    {
                        resistanceValue = _receiverStats.DefenceStats.ToxinDamageResistance;
                    }
                    break;
                case ElementDamageType.Gas:
                    {
                        resistanceValue = _receiverStats.DefenceStats.GasDamageResistance;
                    }
                    break;
                case ElementDamageType.SmokeAndSteam:
                    {
                        resistanceValue = _receiverStats.DefenceStats.SmokeAndSteamDamageResistance;
                    }
                    break;
                default:
                     resistanceValue = 0;
                    break;
            }
            return resistanceValue;
        }

        public float GetPhysicalResistance(PhysicalDamageType type)
        {
            float resistanceValue;
            switch (type)
            {
                case PhysicalDamageType.Cutting:
                    {
                        resistanceValue = _receiverStats.DefenceStats.CuttingDamageResistance;
                    }
                    break;
                case PhysicalDamageType.Piercing:
                    {
                        resistanceValue = _receiverStats.DefenceStats.PiercingDamageResistance;
                    }
                    break;
                case PhysicalDamageType.Chopping:
                    {
                        resistanceValue = _receiverStats.DefenceStats.ChoppingDamageResistance;
                    }
                    break;
                case PhysicalDamageType.Crushing:
                    {
                        resistanceValue = _receiverStats.DefenceStats.CrushingDamageResistance;
                    }
                    break;
                case PhysicalDamageType.Penetration:
                    {
                        resistanceValue = _receiverStats.DefenceStats.PenetrationDamageResistance;
                    }
                    break;
                case PhysicalDamageType.Explosion:
                    {
                        resistanceValue = _receiverStats.DefenceStats.ExplosionProbabilityResistance;
                    }
                    break;
                default:
                    resistanceValue = 0f;
                    break;
            }
            return resistanceValue;
        }

        public float GetPhysicalPower(PhysicalDamageType type, bool isNeedCheckPower)
        {
            if(!isNeedCheckPower)
            {
                return 0;
            }
            float powerValue;
            switch (type)
            {
                case PhysicalDamageType.Cutting:
                    {
                        powerValue = _dealerStats.AttackStats.CuttingPower;
                    }
                    break;
                case PhysicalDamageType.Piercing:
                    {
                        powerValue = _dealerStats.AttackStats.PiersingPower;
                    }
                    break;
                case PhysicalDamageType.Chopping:
                    {
                        powerValue = _dealerStats.AttackStats.ChoppingPower;
                    }
                    break;
                case PhysicalDamageType.Crushing:
                    {
                        powerValue = _dealerStats.AttackStats.CrushingPower;
                    }
                    break;
                case PhysicalDamageType.Penetration:
                    {
                        powerValue = _dealerStats.AttackStats.PenetrationPower;
                    }
                    break;
                case PhysicalDamageType.Explosion:
                    {
                        powerValue = _dealerStats.AttackStats.ExplosionPower;
                    }
                    break;
                default:
                    powerValue = 0f;
                    break;
            }
            return powerValue;
        }

        public float GetElementPower(ElementDamageType type, bool isNeedCheckPower)
        {
            if (!isNeedCheckPower)
            {
                return 0;
            }
            float powerValue;
            switch (type)
            {
                case ElementDamageType.Fire:
                    {
                        powerValue = _receiverStats.AttackStats.FirePower;
                    }
                    break;
                case ElementDamageType.Water:
                    {
                        powerValue = _receiverStats.AttackStats.WaterPower;
                    }
                    break;
                case ElementDamageType.Ice:
                    {
                        powerValue = _receiverStats.AttackStats.IcePower;
                    }
                    break;
                case ElementDamageType.Electricity:
                    {
                        powerValue = _receiverStats.AttackStats.ElectricityPower;
                    }
                    break;
                case ElementDamageType.Oil:
                    {
                        powerValue = _receiverStats.AttackStats.OilPower;
                    }
                    break;
                case ElementDamageType.Toxin:
                    {
                        powerValue = _receiverStats.AttackStats.ToxinPower;
                    }
                    break;
                case ElementDamageType.Gas:
                    {
                        powerValue = _receiverStats.AttackStats.GasPower;
                    }
                    break;
                case ElementDamageType.SmokeAndSteam:
                    {
                        powerValue = _receiverStats.AttackStats.SmokeAndSteamPower;
                    }
                    break;
                default:
                    powerValue = 0;
                    break;
            }
            return powerValue;
        }
        #endregion
    }
}

