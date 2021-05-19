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
                    ""name"": ""Use"",
                    ""type"": ""Button"",
                    ""id"": ""a65d95ba-e2ff-4d86-9a2c-babef11054c5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""00b7db60-f2b3-4660-ab89-f50c851ff90e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Aim"",
                    ""type"": ""Button"",
                    ""id"": ""5f905a3c-84c4-49b0-940f-1b8b931617e6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
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
                    ""interactions"": ""Press""
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
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""MouseLook"",
                    ""type"": ""PassThrough"",
                    ""id"": ""faa3a3e2-b371-4694-ae9c-c9f1dc7d4131"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": ""ScaleVector2(x=0.5,y=0.5),StickDeadzone(min=0.02,max=1)"",
                    ""interactions"": """"
                },
                {
                    ""name"": ""NumberOne"",
                    ""type"": ""Button"",
                    ""id"": ""f29dbde1-072b-4c9f-89ff-a012ecb55e2f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""NumberTwo"",
                    ""type"": ""Button"",
                    ""id"": ""ef1ee716-dd89-4196-a4cd-0b7b9b0dc61f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""NumberThree"",
                    ""type"": ""Button"",
                    ""id"": ""0318adc5-b05c-493c-a3ff-a066d6abf983"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""NumberFour"",
                    ""type"": ""Button"",
                    ""id"": ""56507f0d-a0e2-43b7-a2c6-0e03e8332ba2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""4a45336d-8602-4393-b06c-069302b96a2b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Enter"",
                    ""type"": ""Button"",
                    ""id"": ""46550685-f3a5-4892-8fe5-142a2ef2bee6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""WeaponRemove"",
                    ""type"": ""Button"",
                    ""id"": ""edfbec3a-3250-46fa-8b56-188d64db6fd3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""647e8af3-d6b6-41ed-a630-e35b0273ad6c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
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
                    ""path"": ""<Keyboard>/c"",
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
                },
                {
                    ""name"": """",
                    ""id"": ""d6585d80-827a-42df-a534-ba27353a19f3"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and mouse"",
                    ""action"": ""NumberOne"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""41c65a0e-a4c3-40f5-9d75-05dc72196117"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and mouse"",
                    ""action"": ""NumberTwo"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1915d8af-db08-49aa-b6e7-f3368b8011cd"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and mouse"",
                    ""action"": ""NumberThree"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""af1dfc37-7b91-4be1-acea-7708aa02a2ad"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and mouse"",
                    ""action"": ""NumberFour"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""79455b0e-30f6-42bd-8073-1d73441ec506"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and mouse"",
                    ""action"": ""Use"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""631bc6ce-f8a5-4cac-b58a-5490d3a0cb9c"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and mouse"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b5fd9aad-943b-424d-a709-2fb56746036d"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and mouse"",
                    ""action"": ""Enter"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""52bc05bb-f258-471c-b33a-0eb23e76ff19"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and mouse"",
                    ""action"": ""WeaponRemove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7402df7d-c25d-4d5d-98ef-8c4031d8a2bb"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and mouse"",
                    ""action"": ""Jump"",
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
        m_Player_Use = m_Player.FindAction("Use", throwIfNotFound: true);
        m_Player_Attack = m_Player.FindAction("Attack", throwIfNotFound: true);
        m_Player_Aim = m_Player.FindAction("Aim", throwIfNotFound: true);
        m_Player_Run = m_Player.FindAction("Run", throwIfNotFound: true);
        m_Player_SneakSlide = m_Player.FindAction("Sneak/Slide", throwIfNotFound: true);
        m_Player_ButtonsInfo = m_Player.FindAction("ButtonsInfo", throwIfNotFound: true);
        m_Player_Bestiary = m_Player.FindAction("Bestiary", throwIfNotFound: true);
        m_Player_WeaponWheel = m_Player.FindAction("WeaponWheel", throwIfNotFound: true);
        m_Player_MouseLook = m_Player.FindAction("MouseLook", throwIfNotFound: true);
        m_Player_NumberOne = m_Player.FindAction("NumberOne", throwIfNotFound: true);
        m_Player_NumberTwo = m_Player.FindAction("NumberTwo", throwIfNotFound: true);
        m_Player_NumberThree = m_Player.FindAction("NumberThree", throwIfNotFound: true);
        m_Player_NumberFour = m_Player.FindAction("NumberFour", throwIfNotFound: true);
        m_Player_Cancel = m_Player.FindAction("Cancel", throwIfNotFound: true);
        m_Player_Enter = m_Player.FindAction("Enter", throwIfNotFound: true);
        m_Player_WeaponRemove = m_Player.FindAction("WeaponRemove", throwIfNotFound: true);
        m_Player_Jump = m_Player.FindAction("Jump", throwIfNotFound: true);
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
    private readonly InputAction m_Player_Use;
    private readonly InputAction m_Player_Attack;
    private readonly InputAction m_Player_Aim;
    private readonly InputAction m_Player_Run;
    private readonly InputAction m_Player_SneakSlide;
    private readonly InputAction m_Player_ButtonsInfo;
    private readonly InputAction m_Player_Bestiary;
    private readonly InputAction m_Player_WeaponWheel;
    private readonly InputAction m_Player_MouseLook;
    private readonly InputAction m_Player_NumberOne;
    private readonly InputAction m_Player_NumberTwo;
    private readonly InputAction m_Player_NumberThree;
    private readonly InputAction m_Player_NumberFour;
    private readonly InputAction m_Player_Cancel;
    private readonly InputAction m_Player_Enter;
    private readonly InputAction m_Player_WeaponRemove;
    private readonly InputAction m_Player_Jump;
    public struct PlayerActions
    {
        private @MainInput m_Wrapper;
        public PlayerActions(@MainInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Player_Movement;
        public InputAction @Use => m_Wrapper.m_Player_Use;
        public InputAction @Attack => m_Wrapper.m_Player_Attack;
        public InputAction @Aim => m_Wrapper.m_Player_Aim;
        public InputAction @Run => m_Wrapper.m_Player_Run;
        public InputAction @SneakSlide => m_Wrapper.m_Player_SneakSlide;
        public InputAction @ButtonsInfo => m_Wrapper.m_Player_ButtonsInfo;
        public InputAction @Bestiary => m_Wrapper.m_Player_Bestiary;
        public InputAction @WeaponWheel => m_Wrapper.m_Player_WeaponWheel;
        public InputAction @MouseLook => m_Wrapper.m_Player_MouseLook;
        public InputAction @NumberOne => m_Wrapper.m_Player_NumberOne;
        public InputAction @NumberTwo => m_Wrapper.m_Player_NumberTwo;
        public InputAction @NumberThree => m_Wrapper.m_Player_NumberThree;
        public InputAction @NumberFour => m_Wrapper.m_Player_NumberFour;
        public InputAction @Cancel => m_Wrapper.m_Player_Cancel;
        public InputAction @Enter => m_Wrapper.m_Player_Enter;
        public InputAction @WeaponRemove => m_Wrapper.m_Player_WeaponRemove;
        public InputAction @Jump => m_Wrapper.m_Player_Jump;
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
                @Use.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnUse;
                @Use.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnUse;
                @Use.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnUse;
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
                @NumberOne.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnNumberOne;
                @NumberOne.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnNumberOne;
                @NumberOne.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnNumberOne;
                @NumberTwo.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnNumberTwo;
                @NumberTwo.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnNumberTwo;
                @NumberTwo.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnNumberTwo;
                @NumberThree.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnNumberThree;
                @NumberThree.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnNumberThree;
                @NumberThree.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnNumberThree;
                @NumberFour.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnNumberFour;
                @NumberFour.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnNumberFour;
                @NumberFour.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnNumberFour;
                @Cancel.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCancel;
                @Cancel.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCancel;
                @Cancel.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCancel;
                @Enter.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnEnter;
                @Enter.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnEnter;
                @Enter.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnEnter;
                @WeaponRemove.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnWeaponRemove;
                @WeaponRemove.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnWeaponRemove;
                @WeaponRemove.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnWeaponRemove;
                @Jump.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnJump;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Use.started += instance.OnUse;
                @Use.performed += instance.OnUse;
                @Use.canceled += instance.OnUse;
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
                @NumberOne.started += instance.OnNumberOne;
                @NumberOne.performed += instance.OnNumberOne;
                @NumberOne.canceled += instance.OnNumberOne;
                @NumberTwo.started += instance.OnNumberTwo;
                @NumberTwo.performed += instance.OnNumberTwo;
                @NumberTwo.canceled += instance.OnNumberTwo;
                @NumberThree.started += instance.OnNumberThree;
                @NumberThree.performed += instance.OnNumberThree;
                @NumberThree.canceled += instance.OnNumberThree;
                @NumberFour.started += instance.OnNumberFour;
                @NumberFour.performed += instance.OnNumberFour;
                @NumberFour.canceled += instance.OnNumberFour;
                @Cancel.started += instance.OnCancel;
                @Cancel.performed += instance.OnCancel;
                @Cancel.canceled += instance.OnCancel;
                @Enter.started += instance.OnEnter;
                @Enter.performed += instance.OnEnter;
                @Enter.canceled += instance.OnEnter;
                @WeaponRemove.started += instance.OnWeaponRemove;
                @WeaponRemove.performed += instance.OnWeaponRemove;
                @WeaponRemove.canceled += instance.OnWeaponRemove;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
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
        void OnUse(InputAction.CallbackContext context);
        void OnAttack(InputAction.CallbackContext context);
        void OnAim(InputAction.CallbackContext context);
        void OnRun(InputAction.CallbackContext context);
        void OnSneakSlide(InputAction.CallbackContext context);
        void OnButtonsInfo(InputAction.CallbackContext context);
        void OnBestiary(InputAction.CallbackContext context);
        void OnWeaponWheel(InputAction.CallbackContext context);
        void OnMouseLook(InputAction.CallbackContext context);
        void OnNumberOne(InputAction.CallbackContext context);
        void OnNumberTwo(InputAction.CallbackContext context);
        void OnNumberThree(InputAction.CallbackContext context);
        void OnNumberFour(InputAction.CallbackContext context);
        void OnCancel(InputAction.CallbackContext context);
        void OnEnter(InputAction.CallbackContext context);
        void OnWeaponRemove(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
    }
}
