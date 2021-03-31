// GENERATED AUTOMATICALLY FROM 'Assets/Resources/MainInput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @MainInput : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @MainInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""MainInput"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""4a8b6eb7-2ea8-4036-a225-68cdaabf2702"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""fb7d6fc6-9170-4b39-aa5d-d000eff39d89"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""00b7db60-f2b3-4660-ab89-f50c851ff90e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Aim"",
                    ""type"": ""Button"",
                    ""id"": ""5f905a3c-84c4-49b0-940f-1b8b931617e6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Run"",
                    ""type"": ""Button"",
                    ""id"": ""d3946758-7fe1-4723-81f2-93cc157965f5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""Sneak/Slide"",
                    ""type"": ""Button"",
                    ""id"": ""6cfc29b0-33bd-47b9-a805-1175344c1c88"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ButtonsInfo"",
                    ""type"": ""Button"",
                    ""id"": ""1c78a590-3156-41f1-ab87-54de3ecad34f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Bestiary"",
                    ""type"": ""Button"",
                    ""id"": ""4bb513fd-6f28-4538-a8f7-3f2737e3d45e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""WeaponWheel"",
                    ""type"": ""Button"",
                    ""id"": ""ed0de79e-7a4c-42ed-8645-f6d035bf7419"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Hold""
                },
                {
                    ""name"": ""MouseLook"",
                    ""type"": ""PassThrough"",
                    ""id"": ""faa3a3e2-b371-4694-ae9c-c9f1dc7d4131"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": ""Hold""
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""8da0e29a-9ec5-4840-825f-effcf553f01d"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""bd7c1e23-c26d-4f5b-a217-47506533e357"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""db75258f-4c5f-4184-b9fa-a2f699e26394"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""b799f297-3e7e-486b-babf-f59e972a4b90"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""6557e9db-6484-49c6-8a32-3af11f09f5d0"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and mouse"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""102bae32-9ac4-4f2d-a150-e9ae9e9d9263"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and mouse"",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5472759b-78d3-47f6-a4a1-4b898e4fedcb"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and mouse"",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a2e6cdce-1519-4db0-a16c-ac20a99b3ed9"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and mouse"",
                    ""action"": ""Run"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""007a1269-4a6d-4c48-8b89-24c8eda7013e"",
                    ""path"": ""<Keyboard>/leftCtrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and mouse"",
                    ""action"": ""Sneak/Slide"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a9bd2a56-20bd-42ca-8348-894ecd212041"",
                    ""path"": ""<Keyboard>/backquote"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and mouse"",
                    ""action"": ""ButtonsInfo"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1bb2ff08-8ac4-4ace-8464-350ef569fb11"",
                    ""path"": ""<Keyboard>/b"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Keyboard and mouse"",
                    ""action"": ""Bestiary"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d0e99002-06b5-4ccb-b994-37762a92710c"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and mouse"",
                    ""action"": ""WeaponWheel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4ce24872-6adc-49aa-9b15-14cfbe6ea77d"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and mouse"",
                    ""action"": ""MouseLook"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard and mouse"",
            ""bindingGroup"": ""Keyboard and mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Xbox controller"",
            ""bindingGroup"": ""Xbox controller"",
            ""devices"": [
                {
                    ""devicePath"": ""<XInputController>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""PS controller"",
            ""bindingGroup"": ""PS controller"",
            ""devices"": [
                {
                    ""devicePath"": ""<DualShockGamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Movement = m_Player.FindAction("Movement", throwIfNotFound: true);
        m_Player_Attack = m_Player.FindAction("Attack", throwIfNotFound: true);
        m_Player_Aim = m_Player.FindAction("Aim", throwIfNotFound: true);
        m_Player_Run = m_Player.FindAction("Run", throwIfNotFound: true);
        m_Player_SneakSlide = m_Player.FindAction("Sneak/Slide", throwIfNotFound: true);
        m_Player_ButtonsInfo = m_Player.FindAction("ButtonsInfo", throwIfNotFound: true);
        m_Player_Bestiary = m_Player.FindAction("Bestiary", throwIfNotFound: true);
        m_Player_WeaponWheel = m_Player.FindAction("WeaponWheel", throwIfNotFound: true);
        m_Player_MouseLook = m_Player.FindAction("MouseLook", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_Movement;
    private readonly InputAction m_Player_Attack;
    private readonly InputAction m_Player_Aim;
    private readonly InputAction m_Player_Run;
    private readonly InputAction m_Player_SneakSlide;
    private readonly InputAction m_Player_ButtonsInfo;
    private readonly InputAction m_Player_Bestiary;
    private readonly InputAction m_Player_WeaponWheel;
    private readonly InputAction m_Player_MouseLook;
    public struct PlayerActions
    {
        private @MainInput m_Wrapper;
        public PlayerActions(@MainInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Player_Movement;
        public InputAction @Attack => m_Wrapper.m_Player_Attack;
        public InputAction @Aim => m_Wrapper.m_Player_Aim;
        public InputAction @Run => m_Wrapper.m_Player_Run;
        public InputAction @SneakSlide => m_Wrapper.m_Player_SneakSlide;
        public InputAction @ButtonsInfo => m_Wrapper.m_Player_ButtonsInfo;
        public InputAction @Bestiary => m_Wrapper.m_Player_Bestiary;
        public InputAction @WeaponWheel => m_Wrapper.m_Player_WeaponWheel;
        public InputAction @MouseLook => m_Wrapper.m_Player_MouseLook;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @Attack.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAttack;
                @Attack.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAttack;
                @Attack.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAttack;
                @Aim.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAim;
                @Aim.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAim;
                @Aim.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAim;
                @Run.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRun;
                @Run.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRun;
                @Run.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRun;
                @SneakSlide.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSneakSlide;
                @SneakSlide.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSneakSlide;
                @SneakSlide.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSneakSlide;
                @ButtonsInfo.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnButtonsInfo;
                @ButtonsInfo.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnButtonsInfo;
                @ButtonsInfo.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnButtonsInfo;
                @Bestiary.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnBestiary;
                @Bestiary.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnBestiary;
                @Bestiary.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnBestiary;
                @WeaponWheel.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnWeaponWheel;
                @WeaponWheel.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnWeaponWheel;
                @WeaponWheel.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnWeaponWheel;
                @MouseLook.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMouseLook;
                @MouseLook.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMouseLook;
                @MouseLook.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMouseLook;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Attack.started += instance.OnAttack;
                @Attack.performed += instance.OnAttack;
                @Attack.canceled += instance.OnAttack;
                @Aim.started += instance.OnAim;
                @Aim.performed += instance.OnAim;
                @Aim.canceled += instance.OnAim;
                @Run.started += instance.OnRun;
                @Run.performed += instance.OnRun;
                @Run.canceled += instance.OnRun;
                @SneakSlide.started += instance.OnSneakSlide;
                @SneakSlide.performed += instance.OnSneakSlide;
                @SneakSlide.canceled += instance.OnSneakSlide;
                @ButtonsInfo.started += instance.OnButtonsInfo;
                @ButtonsInfo.performed += instance.OnButtonsInfo;
                @ButtonsInfo.canceled += instance.OnButtonsInfo;
                @Bestiary.started += instance.OnBestiary;
                @Bestiary.performed += instance.OnBestiary;
                @Bestiary.canceled += instance.OnBestiary;
                @WeaponWheel.started += instance.OnWeaponWheel;
                @WeaponWheel.performed += instance.OnWeaponWheel;
                @WeaponWheel.canceled += instance.OnWeaponWheel;
                @MouseLook.started += instance.OnMouseLook;
                @MouseLook.performed += instance.OnMouseLook;
                @MouseLook.canceled += instance.OnMouseLook;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    private int m_KeyboardandmouseSchemeIndex = -1;
    public InputControlScheme KeyboardandmouseScheme
    {
        get
        {
            if (m_KeyboardandmouseSchemeIndex == -1) m_KeyboardandmouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard and mouse");
            return asset.controlSchemes[m_KeyboardandmouseSchemeIndex];
        }
    }
    private int m_XboxcontrollerSchemeIndex = -1;
    public InputControlScheme XboxcontrollerScheme
    {
        get
        {
            if (m_XboxcontrollerSchemeIndex == -1) m_XboxcontrollerSchemeIndex = asset.FindControlSchemeIndex("Xbox controller");
            return asset.controlSchemes[m_XboxcontrollerSchemeIndex];
        }
    }
    private int m_PScontrollerSchemeIndex = -1;
    public InputControlScheme PScontrollerScheme
    {
        get
        {
            if (m_PScontrollerSchemeIndex == -1) m_PScontrollerSchemeIndex = asset.FindControlSchemeIndex("PS controller");
            return asset.controlSchemes[m_PScontrollerSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnAttack(InputAction.CallbackContext context);
        void OnAim(InputAction.CallbackContext context);
        void OnRun(InputAction.CallbackContext context);
        void OnSneakSlide(InputAction.CallbackContext context);
        void OnButtonsInfo(InputAction.CallbackContext context);
        void OnBestiary(InputAction.CallbackContext context);
        void OnWeaponWheel(InputAction.CallbackContext context);
        void OnMouseLook(InputAction.CallbackContext context);
    }
}
