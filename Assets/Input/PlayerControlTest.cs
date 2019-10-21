// GENERATED AUTOMATICALLY FROM 'Assets/Input/PlayerControlTest.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class PlayerControlTest : IInputActionCollection, IDisposable
{
    private InputActionAsset asset;
    public PlayerControlTest()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControlTest"",
    ""maps"": [
        {
            ""name"": ""Gameplay"",
            ""id"": ""1e716631-bec0-4085-bd8e-6a03ca068cee"",
            ""actions"": [
                {
                    ""name"": ""A"",
                    ""type"": ""Button"",
                    ""id"": ""c4c36d34-f648-4c55-a0da-6d2161986b3b"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""0666bd50-0e54-4c0f-904c-b8f6a6a2b22e"",
                    ""expectedControlType"": ""Stick"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Rotate"",
                    ""type"": ""Value"",
                    ""id"": ""0cdb0d89-2d34-41f6-b150-8e8690a9912b"",
                    ""expectedControlType"": ""Stick"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RightTrigger"",
                    ""type"": ""Value"",
                    ""id"": ""5778ed5d-4d78-4a57-8f7b-878166599f97"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LeftTrigger"",
                    ""type"": ""Button"",
                    ""id"": ""c43ac115-d17f-469d-8c3e-0e85cee72f44"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""964634c9-3299-47bb-8f6b-8c0066e51513"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""A"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b211f9b5-b4d9-47be-a0c9-e1571c7a072b"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4ab7f95c-072e-4e3a-b342-aa4596892502"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f9020328-594d-4e20-ba11-4f62aa8aecc9"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RightTrigger"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""127051a6-910b-42da-ae24-3c06c67d9a82"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftTrigger"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Menu"",
            ""id"": ""e0170e8c-b2e6-42a6-9567-392bb1c8ba22"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Button"",
                    ""id"": ""ce986f63-c136-400d-8c4f-411a324f1591"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Select"",
                    ""type"": ""Button"",
                    ""id"": ""4d18909a-d327-402e-92a6-518aede8aff1"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""b36e0d51-60d0-47bf-b908-21a983f2d98a"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""380dd29e-cf4f-480c-95be-0a5980c6c664"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Keyboard Test"",
            ""id"": ""2b3380d9-4089-48f9-b376-0a7ea7aab20f"",
            ""actions"": [
                {
                    ""name"": ""Test ACtion"",
                    ""type"": ""Button"",
                    ""id"": ""f96e4d22-5088-4770-a475-8f220deb6d87"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RightTrigger"",
                    ""type"": ""Value"",
                    ""id"": ""07cbd4f1-a9fa-41dd-b783-c0bb3d9e4410"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""8fabc886-f1da-4697-9e4d-39ca538fd95d"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Test ACtion"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5ab75009-017f-44ff-a2f2-19ef6879c5f4"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RightTrigger"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Gameplay
        m_Gameplay = asset.FindActionMap("Gameplay", throwIfNotFound: true);
        m_Gameplay_A = m_Gameplay.FindAction("A", throwIfNotFound: true);
        m_Gameplay_Move = m_Gameplay.FindAction("Move", throwIfNotFound: true);
        m_Gameplay_Rotate = m_Gameplay.FindAction("Rotate", throwIfNotFound: true);
        m_Gameplay_RightTrigger = m_Gameplay.FindAction("RightTrigger", throwIfNotFound: true);
        m_Gameplay_LeftTrigger = m_Gameplay.FindAction("LeftTrigger", throwIfNotFound: true);
        // Menu
        m_Menu = asset.FindActionMap("Menu", throwIfNotFound: true);
        m_Menu_Move = m_Menu.FindAction("Move", throwIfNotFound: true);
        m_Menu_Select = m_Menu.FindAction("Select", throwIfNotFound: true);
        // Keyboard Test
        m_KeyboardTest = asset.FindActionMap("Keyboard Test", throwIfNotFound: true);
        m_KeyboardTest_TestACtion = m_KeyboardTest.FindAction("Test ACtion", throwIfNotFound: true);
        m_KeyboardTest_RightTrigger = m_KeyboardTest.FindAction("RightTrigger", throwIfNotFound: true);
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

    // Gameplay
    private readonly InputActionMap m_Gameplay;
    private IGameplayActions m_GameplayActionsCallbackInterface;
    private readonly InputAction m_Gameplay_A;
    private readonly InputAction m_Gameplay_Move;
    private readonly InputAction m_Gameplay_Rotate;
    private readonly InputAction m_Gameplay_RightTrigger;
    private readonly InputAction m_Gameplay_LeftTrigger;
    public struct GameplayActions
    {
        private PlayerControlTest m_Wrapper;
        public GameplayActions(PlayerControlTest wrapper) { m_Wrapper = wrapper; }
        public InputAction @A => m_Wrapper.m_Gameplay_A;
        public InputAction @Move => m_Wrapper.m_Gameplay_Move;
        public InputAction @Rotate => m_Wrapper.m_Gameplay_Rotate;
        public InputAction @RightTrigger => m_Wrapper.m_Gameplay_RightTrigger;
        public InputAction @LeftTrigger => m_Wrapper.m_Gameplay_LeftTrigger;
        public InputActionMap Get() { return m_Wrapper.m_Gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
        public void SetCallbacks(IGameplayActions instance)
        {
            if (m_Wrapper.m_GameplayActionsCallbackInterface != null)
            {
                A.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnA;
                A.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnA;
                A.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnA;
                Move.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMove;
                Move.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMove;
                Move.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMove;
                Rotate.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRotate;
                Rotate.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRotate;
                Rotate.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRotate;
                RightTrigger.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRightTrigger;
                RightTrigger.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRightTrigger;
                RightTrigger.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRightTrigger;
                LeftTrigger.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLeftTrigger;
                LeftTrigger.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLeftTrigger;
                LeftTrigger.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLeftTrigger;
            }
            m_Wrapper.m_GameplayActionsCallbackInterface = instance;
            if (instance != null)
            {
                A.started += instance.OnA;
                A.performed += instance.OnA;
                A.canceled += instance.OnA;
                Move.started += instance.OnMove;
                Move.performed += instance.OnMove;
                Move.canceled += instance.OnMove;
                Rotate.started += instance.OnRotate;
                Rotate.performed += instance.OnRotate;
                Rotate.canceled += instance.OnRotate;
                RightTrigger.started += instance.OnRightTrigger;
                RightTrigger.performed += instance.OnRightTrigger;
                RightTrigger.canceled += instance.OnRightTrigger;
                LeftTrigger.started += instance.OnLeftTrigger;
                LeftTrigger.performed += instance.OnLeftTrigger;
                LeftTrigger.canceled += instance.OnLeftTrigger;
            }
        }
    }
    public GameplayActions @Gameplay => new GameplayActions(this);

    // Menu
    private readonly InputActionMap m_Menu;
    private IMenuActions m_MenuActionsCallbackInterface;
    private readonly InputAction m_Menu_Move;
    private readonly InputAction m_Menu_Select;
    public struct MenuActions
    {
        private PlayerControlTest m_Wrapper;
        public MenuActions(PlayerControlTest wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Menu_Move;
        public InputAction @Select => m_Wrapper.m_Menu_Select;
        public InputActionMap Get() { return m_Wrapper.m_Menu; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MenuActions set) { return set.Get(); }
        public void SetCallbacks(IMenuActions instance)
        {
            if (m_Wrapper.m_MenuActionsCallbackInterface != null)
            {
                Move.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnMove;
                Move.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnMove;
                Move.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnMove;
                Select.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnSelect;
                Select.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnSelect;
                Select.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnSelect;
            }
            m_Wrapper.m_MenuActionsCallbackInterface = instance;
            if (instance != null)
            {
                Move.started += instance.OnMove;
                Move.performed += instance.OnMove;
                Move.canceled += instance.OnMove;
                Select.started += instance.OnSelect;
                Select.performed += instance.OnSelect;
                Select.canceled += instance.OnSelect;
            }
        }
    }
    public MenuActions @Menu => new MenuActions(this);

    // Keyboard Test
    private readonly InputActionMap m_KeyboardTest;
    private IKeyboardTestActions m_KeyboardTestActionsCallbackInterface;
    private readonly InputAction m_KeyboardTest_TestACtion;
    private readonly InputAction m_KeyboardTest_RightTrigger;
    public struct KeyboardTestActions
    {
        private PlayerControlTest m_Wrapper;
        public KeyboardTestActions(PlayerControlTest wrapper) { m_Wrapper = wrapper; }
        public InputAction @TestACtion => m_Wrapper.m_KeyboardTest_TestACtion;
        public InputAction @RightTrigger => m_Wrapper.m_KeyboardTest_RightTrigger;
        public InputActionMap Get() { return m_Wrapper.m_KeyboardTest; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(KeyboardTestActions set) { return set.Get(); }
        public void SetCallbacks(IKeyboardTestActions instance)
        {
            if (m_Wrapper.m_KeyboardTestActionsCallbackInterface != null)
            {
                TestACtion.started -= m_Wrapper.m_KeyboardTestActionsCallbackInterface.OnTestACtion;
                TestACtion.performed -= m_Wrapper.m_KeyboardTestActionsCallbackInterface.OnTestACtion;
                TestACtion.canceled -= m_Wrapper.m_KeyboardTestActionsCallbackInterface.OnTestACtion;
                RightTrigger.started -= m_Wrapper.m_KeyboardTestActionsCallbackInterface.OnRightTrigger;
                RightTrigger.performed -= m_Wrapper.m_KeyboardTestActionsCallbackInterface.OnRightTrigger;
                RightTrigger.canceled -= m_Wrapper.m_KeyboardTestActionsCallbackInterface.OnRightTrigger;
            }
            m_Wrapper.m_KeyboardTestActionsCallbackInterface = instance;
            if (instance != null)
            {
                TestACtion.started += instance.OnTestACtion;
                TestACtion.performed += instance.OnTestACtion;
                TestACtion.canceled += instance.OnTestACtion;
                RightTrigger.started += instance.OnRightTrigger;
                RightTrigger.performed += instance.OnRightTrigger;
                RightTrigger.canceled += instance.OnRightTrigger;
            }
        }
    }
    public KeyboardTestActions @KeyboardTest => new KeyboardTestActions(this);
    public interface IGameplayActions
    {
        void OnA(InputAction.CallbackContext context);
        void OnMove(InputAction.CallbackContext context);
        void OnRotate(InputAction.CallbackContext context);
        void OnRightTrigger(InputAction.CallbackContext context);
        void OnLeftTrigger(InputAction.CallbackContext context);
    }
    public interface IMenuActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnSelect(InputAction.CallbackContext context);
    }
    public interface IKeyboardTestActions
    {
        void OnTestACtion(InputAction.CallbackContext context);
        void OnRightTrigger(InputAction.CallbackContext context);
    }
}
