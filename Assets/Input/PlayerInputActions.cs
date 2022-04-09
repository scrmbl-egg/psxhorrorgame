// GENERATED AUTOMATICALLY FROM 'Assets/Input/PlayerInputActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerInputActions : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputActions"",
    ""maps"": [
        {
            ""name"": ""PlayerThing"",
            ""id"": ""a4b17d47-ffca-4895-8c99-4c24caf56af9"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""13a6a2a1-d44e-45a4-b34f-eeecc08b0eab"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Run"",
                    ""type"": ""Button"",
                    ""id"": ""791f44ff-41de-48f7-997e-4bc6f2c7fc16"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Hold""
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""3331d737-a237-4f22-a721-a5319a8c4102"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": ""InvertVector2(invertX=false,invertY=false),ScaleVector2(x=0.05,y=0.05)"",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""da41b73e-b247-4933-8da8-e595e59c2dd0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Fire"",
                    ""type"": ""Button"",
                    ""id"": ""96915eea-c15e-45ef-95c5-1d766f851a5d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Aim"",
                    ""type"": ""Button"",
                    ""id"": ""0dd4c8cb-1b6d-4bb8-95fa-e5ddc4e74f0f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Reload"",
                    ""type"": ""Button"",
                    ""id"": ""3a2b0695-9037-44ad-8c4f-b003bcae1666"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Tap""
                },
                {
                    ""name"": ""CheckAmmo"",
                    ""type"": ""Button"",
                    ""id"": ""7119d716-b2d5-4f56-bae5-f5b8b781b12f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Hold""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""b3626fa1-d873-4ee0-9daf-3e28a0a9a1fc"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""d8cb98aa-6787-4fd9-8b23-ff64d72503ca"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""97401c41-704e-42e9-a879-9e63acdd06af"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""0c698418-bfbc-4adc-8a98-88e7df1549d2"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""57fc753b-7da0-4d93-835d-38ef50bce7b2"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""1e73c71e-89b8-4864-8c1e-069bfb9594a7"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""14e432af-3a7d-4698-bbe9-d8e7e73406dc"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a096a657-79a9-4fe3-9ec8-608fa71882c6"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e07a8319-1af7-464c-ab61-9956121c8fdf"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a2a5bade-9b42-43a8-a339-76fbeaced1d3"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bf8dcf97-dfd9-4696-8c1d-ac48bcc1ea90"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6a331d37-1ef2-4d39-8846-d3d12aee4d10"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e0b2efe5-b4d0-4d6f-8fa4-29143fe3a795"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b0d8837d-e871-45f5-aa31-73b702869918"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""57190941-3b90-4b4b-9372-868bdee9ea4f"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Reload"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""64dff91f-ded0-4536-8f6f-aeb3a1090c6d"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Reload"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""87f91f1d-0431-4095-8592-928b136fd0a5"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""CheckAmmo"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b52d400a-3318-4c18-a40d-49c5b14062d1"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""CheckAmmo"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fe031853-ea2d-41f3-95ae-217bc653f1a7"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Run"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5af5964d-8445-47b6-8e75-eadf002027dc"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Run"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""KeyboardMouse"",
            ""bindingGroup"": ""KeyboardMouse"",
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
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // PlayerThing
        m_PlayerThing = asset.FindActionMap("PlayerThing", throwIfNotFound: true);
        m_PlayerThing_Move = m_PlayerThing.FindAction("Move", throwIfNotFound: true);
        m_PlayerThing_Run = m_PlayerThing.FindAction("Run", throwIfNotFound: true);
        m_PlayerThing_Look = m_PlayerThing.FindAction("Look", throwIfNotFound: true);
        m_PlayerThing_Interact = m_PlayerThing.FindAction("Interact", throwIfNotFound: true);
        m_PlayerThing_Fire = m_PlayerThing.FindAction("Fire", throwIfNotFound: true);
        m_PlayerThing_Aim = m_PlayerThing.FindAction("Aim", throwIfNotFound: true);
        m_PlayerThing_Reload = m_PlayerThing.FindAction("Reload", throwIfNotFound: true);
        m_PlayerThing_CheckAmmo = m_PlayerThing.FindAction("CheckAmmo", throwIfNotFound: true);
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

    // PlayerThing
    private readonly InputActionMap m_PlayerThing;
    private IPlayerThingActions m_PlayerThingActionsCallbackInterface;
    private readonly InputAction m_PlayerThing_Move;
    private readonly InputAction m_PlayerThing_Run;
    private readonly InputAction m_PlayerThing_Look;
    private readonly InputAction m_PlayerThing_Interact;
    private readonly InputAction m_PlayerThing_Fire;
    private readonly InputAction m_PlayerThing_Aim;
    private readonly InputAction m_PlayerThing_Reload;
    private readonly InputAction m_PlayerThing_CheckAmmo;
    public struct PlayerThingActions
    {
        private @PlayerInputActions m_Wrapper;
        public PlayerThingActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_PlayerThing_Move;
        public InputAction @Run => m_Wrapper.m_PlayerThing_Run;
        public InputAction @Look => m_Wrapper.m_PlayerThing_Look;
        public InputAction @Interact => m_Wrapper.m_PlayerThing_Interact;
        public InputAction @Fire => m_Wrapper.m_PlayerThing_Fire;
        public InputAction @Aim => m_Wrapper.m_PlayerThing_Aim;
        public InputAction @Reload => m_Wrapper.m_PlayerThing_Reload;
        public InputAction @CheckAmmo => m_Wrapper.m_PlayerThing_CheckAmmo;
        public InputActionMap Get() { return m_Wrapper.m_PlayerThing; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerThingActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerThingActions instance)
        {
            if (m_Wrapper.m_PlayerThingActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_PlayerThingActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_PlayerThingActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_PlayerThingActionsCallbackInterface.OnMove;
                @Run.started -= m_Wrapper.m_PlayerThingActionsCallbackInterface.OnRun;
                @Run.performed -= m_Wrapper.m_PlayerThingActionsCallbackInterface.OnRun;
                @Run.canceled -= m_Wrapper.m_PlayerThingActionsCallbackInterface.OnRun;
                @Look.started -= m_Wrapper.m_PlayerThingActionsCallbackInterface.OnLook;
                @Look.performed -= m_Wrapper.m_PlayerThingActionsCallbackInterface.OnLook;
                @Look.canceled -= m_Wrapper.m_PlayerThingActionsCallbackInterface.OnLook;
                @Interact.started -= m_Wrapper.m_PlayerThingActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_PlayerThingActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_PlayerThingActionsCallbackInterface.OnInteract;
                @Fire.started -= m_Wrapper.m_PlayerThingActionsCallbackInterface.OnFire;
                @Fire.performed -= m_Wrapper.m_PlayerThingActionsCallbackInterface.OnFire;
                @Fire.canceled -= m_Wrapper.m_PlayerThingActionsCallbackInterface.OnFire;
                @Aim.started -= m_Wrapper.m_PlayerThingActionsCallbackInterface.OnAim;
                @Aim.performed -= m_Wrapper.m_PlayerThingActionsCallbackInterface.OnAim;
                @Aim.canceled -= m_Wrapper.m_PlayerThingActionsCallbackInterface.OnAim;
                @Reload.started -= m_Wrapper.m_PlayerThingActionsCallbackInterface.OnReload;
                @Reload.performed -= m_Wrapper.m_PlayerThingActionsCallbackInterface.OnReload;
                @Reload.canceled -= m_Wrapper.m_PlayerThingActionsCallbackInterface.OnReload;
                @CheckAmmo.started -= m_Wrapper.m_PlayerThingActionsCallbackInterface.OnCheckAmmo;
                @CheckAmmo.performed -= m_Wrapper.m_PlayerThingActionsCallbackInterface.OnCheckAmmo;
                @CheckAmmo.canceled -= m_Wrapper.m_PlayerThingActionsCallbackInterface.OnCheckAmmo;
            }
            m_Wrapper.m_PlayerThingActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Run.started += instance.OnRun;
                @Run.performed += instance.OnRun;
                @Run.canceled += instance.OnRun;
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
                @Fire.started += instance.OnFire;
                @Fire.performed += instance.OnFire;
                @Fire.canceled += instance.OnFire;
                @Aim.started += instance.OnAim;
                @Aim.performed += instance.OnAim;
                @Aim.canceled += instance.OnAim;
                @Reload.started += instance.OnReload;
                @Reload.performed += instance.OnReload;
                @Reload.canceled += instance.OnReload;
                @CheckAmmo.started += instance.OnCheckAmmo;
                @CheckAmmo.performed += instance.OnCheckAmmo;
                @CheckAmmo.canceled += instance.OnCheckAmmo;
            }
        }
    }
    public PlayerThingActions @PlayerThing => new PlayerThingActions(this);
    private int m_KeyboardMouseSchemeIndex = -1;
    public InputControlScheme KeyboardMouseScheme
    {
        get
        {
            if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("KeyboardMouse");
            return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    public interface IPlayerThingActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnRun(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnFire(InputAction.CallbackContext context);
        void OnAim(InputAction.CallbackContext context);
        void OnReload(InputAction.CallbackContext context);
        void OnCheckAmmo(InputAction.CallbackContext context);
    }
}
