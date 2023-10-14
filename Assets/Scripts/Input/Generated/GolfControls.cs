//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.6.3
//     from Assets/Game Data/Input/GolfControls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @GolfControls: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @GolfControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""GolfControls"",
    ""maps"": [
        {
            ""name"": ""Gameplay"",
            ""id"": ""850c8726-c4ac-45b8-a75c-8df874728544"",
            ""actions"": [
                {
                    ""name"": ""Aim"",
                    ""type"": ""Value"",
                    ""id"": ""a0541e6b-092d-435c-80f9-86e70f1a6299"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": ""AxisDeadzone"",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Power"",
                    ""type"": ""Value"",
                    ""id"": ""188b8e96-8658-4ef6-8cfa-299f8bea6a1a"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": ""AxisDeadzone,Clamp(min=-1,max=1)"",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Swing"",
                    ""type"": ""Button"",
                    ""id"": ""8504576d-01c4-4692-9142-8d385e48c0d8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""e55f09fa-36bb-44e8-a775-8f62c22c00bd"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""CycleClubRight"",
                    ""type"": ""Button"",
                    ""id"": ""1fff8b21-7079-46ee-85e1-e4039d27aba1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""CycleClubLeft"",
                    ""type"": ""Button"",
                    ""id"": ""1c452c77-e1c7-49fc-9b35-597fa9b57c99"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""00ba54cf-2904-46ae-9fc2-74b2e0f8a160"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""dc7a0882-c2e5-4938-be2a-d2a32db728c5"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""DesktopControlScheme"",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""1537521b-b15b-41fc-96a1-cad5201e9b90"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""DesktopControlScheme"",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""d935173f-b657-481b-8263-78106031262d"",
                    ""path"": ""<Gamepad>/dpad/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""DesktopControlScheme"",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d4f0c5bd-1738-4052-ad97-ef897f2e8136"",
                    ""path"": ""<Gamepad>/leftStick/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""DesktopControlScheme"",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4a30c05b-e4d0-4bd0-832d-834c85124e73"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""DesktopControlScheme"",
                    ""action"": ""Swing"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""01d6eb9a-1734-44c2-8fcd-aaabd244e9e2"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""DesktopControlScheme"",
                    ""action"": ""Swing"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""afd0ea65-76e2-42ad-8cac-43c329aef8f8"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Swing"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""afc4e8e9-aa69-4f76-9ab6-26ca2f755ea2"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""DesktopControlScheme"",
                    ""action"": ""CycleClubRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e05fd86a-9c09-4cd0-b48e-804c670d71e4"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""DesktopControlScheme"",
                    ""action"": ""CycleClubRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""21d082d1-7cd9-40c1-837d-e17268f795e4"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CycleClubRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d012bd52-d2a8-4ee2-8d28-f3deaa8a8e05"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CycleClubLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a02acfdf-5036-440c-b9e7-8a4b1d5e8387"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CycleClubLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""ad8509fd-88ad-4d0c-a0e4-4abeeb9485ff"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Power"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""7dde71a2-2fa9-42f6-b6b9-6c714fda7b5e"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""DesktopControlScheme"",
                    ""action"": ""Power"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""5bee1187-a76e-43e9-a9da-b2e120bc6e29"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""DesktopControlScheme"",
                    ""action"": ""Power"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""f43a6112-e5f5-4f80-8b50-1923a6736cf6"",
                    ""path"": ""<Gamepad>/dpad/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""DesktopControlScheme"",
                    ""action"": ""Power"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5344b3dc-30c2-417b-a29b-a491aeb5df5d"",
                    ""path"": ""<Gamepad>/leftStick/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""DesktopControlScheme"",
                    ""action"": ""Power"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e845f919-16dc-448b-ab8d-284c443c0ea7"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""DesktopControlScheme"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""48c3269a-7fc1-4a57-ba74-d7fe1fba6d0e"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""fd3c0e60-5337-48fa-a533-865a1f091f2b"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""a9b2ae2f-2e15-41bb-b24d-79bb9ed64b48"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""1ff038f7-0455-4ac6-828d-c75191aeb609"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""870ee256-683b-451f-bc81-bda8581dac99"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""DesktopControlScheme"",
            ""bindingGroup"": ""DesktopControlScheme"",
            ""devices"": []
        }
    ]
}");
        // Gameplay
        m_Gameplay = asset.FindActionMap("Gameplay", throwIfNotFound: true);
        m_Gameplay_Aim = m_Gameplay.FindAction("Aim", throwIfNotFound: true);
        m_Gameplay_Power = m_Gameplay.FindAction("Power", throwIfNotFound: true);
        m_Gameplay_Swing = m_Gameplay.FindAction("Swing", throwIfNotFound: true);
        m_Gameplay_Look = m_Gameplay.FindAction("Look", throwIfNotFound: true);
        m_Gameplay_CycleClubRight = m_Gameplay.FindAction("CycleClubRight", throwIfNotFound: true);
        m_Gameplay_CycleClubLeft = m_Gameplay.FindAction("CycleClubLeft", throwIfNotFound: true);
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

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Gameplay
    private readonly InputActionMap m_Gameplay;
    private List<IGameplayActions> m_GameplayActionsCallbackInterfaces = new List<IGameplayActions>();
    private readonly InputAction m_Gameplay_Aim;
    private readonly InputAction m_Gameplay_Power;
    private readonly InputAction m_Gameplay_Swing;
    private readonly InputAction m_Gameplay_Look;
    private readonly InputAction m_Gameplay_CycleClubRight;
    private readonly InputAction m_Gameplay_CycleClubLeft;
    public struct GameplayActions
    {
        private @GolfControls m_Wrapper;
        public GameplayActions(@GolfControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Aim => m_Wrapper.m_Gameplay_Aim;
        public InputAction @Power => m_Wrapper.m_Gameplay_Power;
        public InputAction @Swing => m_Wrapper.m_Gameplay_Swing;
        public InputAction @Look => m_Wrapper.m_Gameplay_Look;
        public InputAction @CycleClubRight => m_Wrapper.m_Gameplay_CycleClubRight;
        public InputAction @CycleClubLeft => m_Wrapper.m_Gameplay_CycleClubLeft;
        public InputActionMap Get() { return m_Wrapper.m_Gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
        public void AddCallbacks(IGameplayActions instance)
        {
            if (instance == null || m_Wrapper.m_GameplayActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_GameplayActionsCallbackInterfaces.Add(instance);
            @Aim.started += instance.OnAim;
            @Aim.performed += instance.OnAim;
            @Aim.canceled += instance.OnAim;
            @Power.started += instance.OnPower;
            @Power.performed += instance.OnPower;
            @Power.canceled += instance.OnPower;
            @Swing.started += instance.OnSwing;
            @Swing.performed += instance.OnSwing;
            @Swing.canceled += instance.OnSwing;
            @Look.started += instance.OnLook;
            @Look.performed += instance.OnLook;
            @Look.canceled += instance.OnLook;
            @CycleClubRight.started += instance.OnCycleClubRight;
            @CycleClubRight.performed += instance.OnCycleClubRight;
            @CycleClubRight.canceled += instance.OnCycleClubRight;
            @CycleClubLeft.started += instance.OnCycleClubLeft;
            @CycleClubLeft.performed += instance.OnCycleClubLeft;
            @CycleClubLeft.canceled += instance.OnCycleClubLeft;
        }

        private void UnregisterCallbacks(IGameplayActions instance)
        {
            @Aim.started -= instance.OnAim;
            @Aim.performed -= instance.OnAim;
            @Aim.canceled -= instance.OnAim;
            @Power.started -= instance.OnPower;
            @Power.performed -= instance.OnPower;
            @Power.canceled -= instance.OnPower;
            @Swing.started -= instance.OnSwing;
            @Swing.performed -= instance.OnSwing;
            @Swing.canceled -= instance.OnSwing;
            @Look.started -= instance.OnLook;
            @Look.performed -= instance.OnLook;
            @Look.canceled -= instance.OnLook;
            @CycleClubRight.started -= instance.OnCycleClubRight;
            @CycleClubRight.performed -= instance.OnCycleClubRight;
            @CycleClubRight.canceled -= instance.OnCycleClubRight;
            @CycleClubLeft.started -= instance.OnCycleClubLeft;
            @CycleClubLeft.performed -= instance.OnCycleClubLeft;
            @CycleClubLeft.canceled -= instance.OnCycleClubLeft;
        }

        public void RemoveCallbacks(IGameplayActions instance)
        {
            if (m_Wrapper.m_GameplayActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IGameplayActions instance)
        {
            foreach (var item in m_Wrapper.m_GameplayActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_GameplayActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public GameplayActions @Gameplay => new GameplayActions(this);
    private int m_DesktopControlSchemeSchemeIndex = -1;
    public InputControlScheme DesktopControlSchemeScheme
    {
        get
        {
            if (m_DesktopControlSchemeSchemeIndex == -1) m_DesktopControlSchemeSchemeIndex = asset.FindControlSchemeIndex("DesktopControlScheme");
            return asset.controlSchemes[m_DesktopControlSchemeSchemeIndex];
        }
    }
    public interface IGameplayActions
    {
        void OnAim(InputAction.CallbackContext context);
        void OnPower(InputAction.CallbackContext context);
        void OnSwing(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnCycleClubRight(InputAction.CallbackContext context);
        void OnCycleClubLeft(InputAction.CallbackContext context);
    }
}
