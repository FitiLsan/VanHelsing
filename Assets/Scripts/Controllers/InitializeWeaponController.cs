using UnityEngine;
using System;
using RootMotion.Dynamics;


namespace BeastHunter
{
    public sealed class InitializeWeaponController
    {
        #region Field

        private readonly GameContext _context;
        private readonly CharacterModel _characterModel;
        private readonly WeaponData _weaponData;

        private readonly Predicate<Collider> _onHitBoxFilter;
        private readonly Action<ITrigger, Collider> _onHitBoxHit;

        #endregion


        #region ClassLifeCycle

        public InitializeWeaponController(GameContext context, WeaponData weaponData, Predicate<Collider> onHitBoxFilter,
            Action<ITrigger, Collider> onHitBoxHit, ref Action onWeaponChange)
        {
            _context = context;
            _characterModel = _context.CharacterModel;
            _weaponData = weaponData;

            _onHitBoxFilter = onHitBoxFilter;
            _onHitBoxHit = onHitBoxHit;
            onWeaponChange += RemoveWeapon;

            _characterModel.CurrentWeaponData.Value = _weaponData;
            CreateWeapon(weaponData);
        }

        #endregion


        #region Methods

        private void CreateWeapon(WeaponData weaponData)
        {
            if (weaponData is OneHandedWeaponData)
            {
                OneHandedWeaponData newWeaponData = weaponData as OneHandedWeaponData;
                GameObject newWeapon = GameObject.Instantiate(newWeaponData.ActualWeapon.WeaponPrefab);
                newWeaponData.Init(newWeapon);

                switch (newWeaponData.InWhichHand)
                {
                    case HandsEnum.Left:
                        _characterModel.CurrentWeaponLeft = newWeapon;
                        _characterModel.WeaponBehaviorLeft = newWeapon.GetComponentInChildren<WeaponHitBoxBehavior>();

                        if (_characterModel.WeaponBehaviorLeft != null)
                        {
                            _characterModel.WeaponBehaviorLeft.OnFilterHandler += _onHitBoxFilter;
                            _characterModel.WeaponBehaviorLeft.OnTriggerEnterHandler += _onHitBoxHit;
                        }
                        break;
                    case HandsEnum.Right:
                        _characterModel.CurrentWeaponRight = newWeapon;
                        _characterModel.WeaponBehaviorRight = newWeapon.GetComponentInChildren<WeaponHitBoxBehavior>();

                        if (_characterModel.WeaponBehaviorRight != null)
                        {
                            _characterModel.WeaponBehaviorRight.OnFilterHandler += _onHitBoxFilter;
                            _characterModel.WeaponBehaviorRight.OnTriggerEnterHandler += _onHitBoxHit;
                        }
                        break;
                    default:
                        break;
                }

                _characterModel.PuppetMaster.propMuscles[(int)newWeaponData.InWhichHand - 1].
                    currentProp = newWeapon.GetComponent<PuppetMasterProp>();
            }
            else
            {
                TwoHandedWeaponData newWeaponData = weaponData as TwoHandedWeaponData;

                if (newWeaponData.Type == WeaponType.Shooting)
                {
                    newWeaponData.OnHit += _onHitBoxHit;
                }

                switch (newWeaponData.InWhichHand)
                {
                    case HandsEnum.Left:

                        _characterModel.CurrentWeaponLeft = GameObject.Instantiate(newWeaponData.
                            LeftActualWeapon.WeaponPrefab);
                        _characterModel.WeaponBehaviorLeft = _characterModel.CurrentWeaponLeft.
                            GetComponentInChildren<WeaponHitBoxBehavior>();

                        if (_characterModel.WeaponBehaviorLeft != null)
                        {
                            _characterModel.WeaponBehaviorLeft.OnFilterHandler += _onHitBoxFilter;
                            _characterModel.WeaponBehaviorLeft.OnTriggerEnterHandler += _onHitBoxHit;
                        }

                        _characterModel.PuppetMaster.propMuscles[(int)(HandsEnum.Left) - 1].
                            currentProp = _characterModel.CurrentWeaponLeft.GetComponent<PuppetMasterProp>();
                        break;
                    case HandsEnum.Right:

                        _characterModel.CurrentWeaponRight = GameObject.Instantiate(newWeaponData.
                            RightActualWeapon.WeaponPrefab);
                        _characterModel.WeaponBehaviorRight = _characterModel.CurrentWeaponRight.
                            GetComponentInChildren<WeaponHitBoxBehavior>();

                        if (_characterModel.WeaponBehaviorRight != null)
                        {
                            _characterModel.WeaponBehaviorRight.OnFilterHandler += _onHitBoxFilter;
                            _characterModel.WeaponBehaviorRight.OnTriggerEnterHandler += _onHitBoxHit;
                        }

                        _characterModel.PuppetMaster.propMuscles[(int)(HandsEnum.Right) - 1].
                            currentProp = _characterModel.CurrentWeaponRight.GetComponent<PuppetMasterProp>();
                        break;
                    case HandsEnum.Both:

                        _characterModel.CurrentWeaponLeft = GameObject.Instantiate(newWeaponData.
                            LeftActualWeapon.WeaponPrefab);
                        _characterModel.CurrentWeaponRight = GameObject.Instantiate(newWeaponData.
                            RightActualWeapon.WeaponPrefab);

                        _characterModel.WeaponBehaviorLeft = _characterModel.CurrentWeaponLeft.
                            GetComponentInChildren<WeaponHitBoxBehavior>();
                        _characterModel.WeaponBehaviorRight = _characterModel.CurrentWeaponRight.
                            GetComponentInChildren<WeaponHitBoxBehavior>();

                        if (_characterModel.WeaponBehaviorLeft != null)
                        {
                            _characterModel.WeaponBehaviorLeft.OnFilterHandler += _onHitBoxFilter;
                            _characterModel.WeaponBehaviorLeft.OnTriggerEnterHandler += _onHitBoxHit;
                        }

                        if (_characterModel.WeaponBehaviorRight != null)
                        {
                            _characterModel.WeaponBehaviorRight.OnFilterHandler += _onHitBoxFilter;
                            _characterModel.WeaponBehaviorRight.OnTriggerEnterHandler += _onHitBoxHit;
                        }

                        _characterModel.PuppetMaster.propMuscles[(int)(HandsEnum.Left) - 1].
                            currentProp = _characterModel.CurrentWeaponLeft.GetComponent<PuppetMasterProp>();
                        _characterModel.PuppetMaster.propMuscles[(int)(HandsEnum.Right) - 1].
                            currentProp = _characterModel.CurrentWeaponRight.GetComponent<PuppetMasterProp>();
                        break;
                    default:
                        break;
                }

                newWeaponData.Init(_characterModel.CurrentWeaponLeft, _characterModel.CurrentWeaponRight);
            }
        }

        private void RemoveWeapon()
        {
            if (_characterModel.CurrentWeaponData.Value != null)
            {
                if (_characterModel.CurrentWeaponLeft != null)
                {
                    if (_characterModel.WeaponBehaviorLeft != null)
                    {
                        _characterModel.WeaponBehaviorLeft.OnFilterHandler -= _onHitBoxFilter;
                        _characterModel.WeaponBehaviorLeft.OnTriggerEnterHandler -= _onHitBoxHit;
                    }

                    _characterModel.PuppetMaster.propMuscles[(int)(HandsEnum.Left) - 1].currentProp = null;
                    GameObject.Destroy(_characterModel.CurrentWeaponLeft, 0.1f); // TO REFACTOR
                }

                if (_characterModel.CurrentWeaponRight != null)
                {
                    if (_characterModel.WeaponBehaviorRight != null)
                    {
                        _characterModel.WeaponBehaviorRight.OnFilterHandler -= _onHitBoxFilter;
                        _characterModel.WeaponBehaviorRight.OnTriggerEnterHandler -= _onHitBoxHit;
                    }

                    _characterModel.PuppetMaster.propMuscles[(int)(HandsEnum.Right) - 1].currentProp = null;
                    GameObject.Destroy(_characterModel.CurrentWeaponRight, 0.1f); // TO REFACTOR
                }

                if (_characterModel.CurrentWeaponData.Value.Type == WeaponType.Shooting)
                {
                    _characterModel.CurrentWeaponData.Value.OnHit -= _onHitBoxHit;
                }
            }

            _characterModel.CurrentWeaponData.Value = null;
            _characterModel.CurrentWeaponLeft = null;
            _characterModel.CurrentWeaponRight = null;
        }

        #endregion
    }
}

