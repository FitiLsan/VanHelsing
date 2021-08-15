using System;
using UnityEngine;
using UnityEngine.UI;

namespace BeastHunter
{
    public class WeaponWheelController
    {
        #region Events

        public event Action<TrapData> OnNewTrap;
        public event Action<WeaponData> OnNewWeapon;
        public event Action OnWeaponChange;
        public event Action OnWheelOpen;
        public event Action OnWheelClose;

        #endregion


        #region Fields

        private WeaponWheelModel _weaponWheelModel;
        private WeaponWheelView _weaponWheelView;
        private readonly CharacterModel _characterModel;
        private readonly InputModel _inputModel;

        #endregion


        #region ClassLifeCycles

        public WeaponWheelController(CharacterModel characterModel, InputModel inputModel)
        {
            _characterModel = characterModel;
            _inputModel = inputModel;
            _weaponWheelModel = new WeaponWheelModel();
            _weaponWheelView = GameObject.Instantiate(Data.UIElementsData.WeaponWheelPrefab).GetComponent<WeaponWheelView>();
            _weaponWheelView.Init(_weaponWheelModel.WeaponWheelPanelName, _weaponWheelModel.WeaponWheelCycleName);
            InitAllWeaponItemsOnWheel();
            CloseWeaponWheel();
        }

        #endregion


        #region IUpdate

        public void Updating()
        {
            ControlWeaponWheel();
        }

        #endregion


        #region WeaponWheelControls

        private void ControlWeaponWheelOpen(bool doOpen)
        {
            if (doOpen)
            {
                OpenWeaponWheel();
            }
            else
            {
                CloseWeaponWheel();
            }
        }

        private void OpenWeaponWheel()
        {
            _weaponWheelView.WeaponWheelUI.SetActive(true);
            OnWheelOpen?.Invoke();
            _weaponWheelView.WeaponWheelTransform.localPosition = Vector3.zero;
            _weaponWheelModel.AssignClosestWeaponOnWheel(null);
            _weaponWheelModel.AssightIsWeaponWheelOpen(true);
        }

        private void CloseWeaponWheel()
        {
            _weaponWheelView.WeaponWheelUI.SetActive(false);
            OnWheelClose?.Invoke();
            _weaponWheelModel.AssightIsWeaponWheelOpen(false);

            if (_weaponWheelModel.ClosestWeaponOnWheel != null)
            {
                if (_weaponWheelModel.ClosestWeaponOnWheel.WeaponData != null)
                {
                    if (_weaponWheelModel.ClosestWeaponOnWheel.WeaponData != _characterModel.CurrentWeaponData.Value)
                    {
                        OnWeaponChange?.Invoke();
                        OnNewWeapon?.Invoke(_weaponWheelModel.ClosestWeaponOnWheel.WeaponData);
                    }
                }
                else if (_weaponWheelModel.ClosestWeaponOnWheel.TrapData != null)
                {
                    if (_weaponWheelModel.ClosestWeaponOnWheel.TrapData != _characterModel.CurrentPlacingTrapModel.Value?.TrapData)
                    {
                        OnWeaponChange?.Invoke();
                        OnNewTrap?.Invoke(_weaponWheelModel.ClosestWeaponOnWheel.TrapData);
                    }
                }
                else
                {
                    OnWeaponChange?.Invoke();
                }
            }
        }

        private void ControlWeaponWheel()
        {
            if (_weaponWheelModel.IsWeaponWheelOpen)
            {
                _weaponWheelView.WeaponWheelTransform.localPosition = new Vector3(
                    Mathf.Clamp(_weaponWheelView.WeaponWheelTransform.localPosition.x + _inputModel.MouseInputX,
                    -_weaponWheelModel.WeaponWheelMaxCycleDistance, _weaponWheelModel.WeaponWheelMaxCycleDistance),
                    Mathf.Clamp(_weaponWheelView.WeaponWheelTransform.localPosition.y + _inputModel.MouseInputY,
                    -_weaponWheelModel.WeaponWheelMaxCycleDistance, _weaponWheelModel.WeaponWheelMaxCycleDistance),
                    _weaponWheelView.WeaponWheelTransform.localPosition.z);

                float distanceFromCenter = (_weaponWheelView.WeaponWheelTransform.localPosition - Vector3.zero).sqrMagnitude;

                if (distanceFromCenter > _weaponWheelModel.WeaponWheelMaxCycleDistance)
                {
                    _weaponWheelView.WeaponWheelTransform.localPosition *= _weaponWheelModel.WeaponWheelMaxCycleDistance / distanceFromCenter;
                }

                if (distanceFromCenter > _weaponWheelModel.WeaponWheelDistanceToSetWeapon)
                {
                    _weaponWheelModel.AssignClosestWeaponOnWheel(GetClosestWeaponItemInWheel());
                }
                else
                {
                    DisableAllWeaponItemsInWheel();
                    _weaponWheelModel.AssignClosestWeaponOnWheel(null);
                }
            }
        }

        private WeaponCircle GetClosestWeaponItemInWheel()
        {
            float minimalDistance = Mathf.Infinity;
            float currentDistance = minimalDistance;
            WeaponCircle chosenWeapon = null;

            for (int item = 0; item < _weaponWheelView.WeaponWheelItems.Length; item++)
            {
                currentDistance = (_weaponWheelView.WeaponWheelItems[item].AnchorPosition - 
                    _weaponWheelView.WeaponWheelTransform.localPosition).
                    sqrMagnitude;

                if (currentDistance < minimalDistance)
                {
                    minimalDistance = currentDistance;
                    chosenWeapon = _weaponWheelView.WeaponWheelItems[item];
                }
            }

            if (_weaponWheelModel.ClosestWeaponOnWheel != chosenWeapon)
            {
                SetActivityOfElementOnWeaponWheel(_weaponWheelModel.ClosestWeaponOnWheel, false);
                SetActivityOfElementOnWeaponWheel(chosenWeapon, true);
            }

            return chosenWeapon;
        }

        private void DisableAllWeaponItemsInWheel()
        {
            foreach (var item in _weaponWheelView.WeaponWheelItems)
            {
                SetActivityOfElementOnWeaponWheel(item, false);
            }
        }

        private void InitAllWeaponItemsOnWheel()
        {
            foreach (var item in _weaponWheelView.WeaponWheelItems)
            {
                Image[] images = item.GetComponentsInChildren<Image>();
                RectTransform[] rects = item.GetComponentsInChildren<RectTransform>();

                if (item.WeaponData != null)
                {
                    item.GetComponentsInChildren<Image>()[1].sprite = item.WeaponData.WeaponImage;
                    images[1].color = new Color(1f, 1f, 1f, _weaponWheelModel.WeaponWheelChildImageNonDedicatedAlfa);
                }
                else if (item.TrapData != null)
                {
                    item.GetComponentsInChildren<Image>()[1].sprite = item.TrapData.TrapImage;
                    images[1].color = new Color(1f, 1f, 1f, _weaponWheelModel.WeaponWheelChildImageNonDedicatedAlfa);
                }
                else
                {
                    images[1].color = new Color(1f, 1f, 1f, 0f);
                }

                images[0].color = new Color(1f, 1f, 1f, _weaponWheelModel.WeaponWheelParentImageNonDedicatedAlfa);
                rects[0].localScale = new Vector3(_weaponWheelModel.WeaponWheelParentImageNonDedicatedAlfa,
                    _weaponWheelModel.WeaponWheelParentImageNonDedicatedScale, 1f);
                rects[1].localScale = new Vector3(_weaponWheelModel.WeaponWheelChildImageNonDedicatedScale,
                    _weaponWheelModel.WeaponWheelParentImageNonDedicatedScale, 1f);
            }
        }

        private void SetActivityOfElementOnWeaponWheel(WeaponCircle item, bool doActivate)
        {
            if (item != null)
            {
                Image[] images = item.GetComponentsInChildren<Image>();
                RectTransform[] rects = item.GetComponentsInChildren<RectTransform>();

                if (doActivate && item.IsNotEmpty)
                {
                    images[0].color = new Color(1f, 1f, 1f, _weaponWheelModel.WeaponWheelParentImageDedicatedAlfa);
                    images[1].color = new Color(1f, 1f, 1f, _weaponWheelModel.WeaponWheelChildImageDedicatedAlfa);

                    rects[0].localScale = new Vector3(_weaponWheelModel.WeaponWheelImageDedicatedScale,
                        _weaponWheelModel.WeaponWheelImageDedicatedScale, 1f);
                    rects[1].localScale = new Vector3(_weaponWheelModel.WeaponWheelImageDedicatedScale,
                        _weaponWheelModel.WeaponWheelImageDedicatedScale, 1f);

                    if (item.WeaponData != null)
                    {
                        _weaponWheelView.WeaponWheelText.text = item.WeaponData.WeaponName;
                    }
                    else if (item.TrapData != null)
                    {
                        _weaponWheelView.WeaponWheelText.text = item.TrapData.TrapStruct.TrapName;
                    }
                }
                else if (item.IsNotEmpty)
                {
                    images[0].color = new Color(1f, 1f, 1f, _weaponWheelModel.WeaponWheelParentImageNonDedicatedAlfa);
                    images[1].color = new Color(1f, 1f, 1f, _weaponWheelModel.WeaponWheelChildImageNonDedicatedAlfa);

                    rects[0].localScale = new Vector3(_weaponWheelModel.WeaponWheelParentImageNonDedicatedScale,
                        _weaponWheelModel.WeaponWheelParentImageNonDedicatedScale, 1f);
                    rects[1].localScale = new Vector3(_weaponWheelModel.WeaponWheelChildImageNonDedicatedScale,
                        _weaponWheelModel.WeaponWheelParentImageNonDedicatedScale, 1f);

                    _weaponWheelView.WeaponWheelText.text = string.Empty;
                }
            }
        }

        #endregion
    }
}
