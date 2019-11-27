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
                    ""name"": ""ZRightTrigger"",
                    ""type"": ""Value"",
                    ""id"": ""2f721df3-313a-4941-8a75-cbb69e35b24a"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ZRightTriggerUp"",
                    ""type"": ""Value"",
                    ""id"": ""db2e9d4a-7df2-4c9d-85b9-9ce4bc0705ed"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=1)""
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
                },
                {
                    ""name"": """",
                    ""id"": ""bcf2a641-4d95-4a7d-95ce-7177d2f765f6"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ZRightTrigger"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""04d5324b-433d-4669-af34-2c0350a5dc32"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ZRightTriggerUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Menu"",
            ""id"": ""d6f803fc-8fca-4e7b-a316-9e135fddfd4e"",
            ""actions"": [
                {
                    ""name"": ""Select"",
                    ""type"": ""Button"",
                    ""id"": ""b3485470-9408-4f1a-a0e2-f8a2be60e1fd"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""2a536f1e-29a4-4452-9a94-7ebcb023331a"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Ready"",
                    ""type"": ""Button"",
                    ""id"": ""652e6783-ffc1-4e11-9acc-b9548ef875d2"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Leave"",
                    ""type"": ""Button"",
                    ""id"": ""6726a714-eccd-4751-9f31-cb4dd2ee0363"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""ac0661bf-d183-4dc6-92dc-5650ca39da6a"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5b5f1728-e22b-4a1a-a3c2-04e23a3d3fed"",
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
                    ""id"": ""4ef192ba-f2ea-46a1-83d6-254fa579ae1b"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Ready"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""75f105bd-9c5e-432d-8621-dbbf1d2fecf7"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Leave"",
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
        m_Gameplay_ZRightTrigger = m_Gameplay.FindAction("ZRightTrigger", throwIfNotFound: true);
        m_Gameplay_ZRightTriggerUp = m_Gameplay.FindAction("ZRightTriggerUp", throwIfNotFound: true);
        m_Gameplay_LeftTrigger = m_Gameplay.FindAction("LeftTrigger", throwIfNotFound: true);
        // Menu
        m_Menu = asset.FindActionMap("Menu", throwIfNotFound: true);
        m_Menu_Select = m_Menu.FindAction("Select", throwIfNotFound: true);
        m_Menu_Move = m_Menu.FindAction("Move", throwIfNotFound: true);
        m_Menu_Ready = m_Menu.FindAction("Ready", throwIfNotFound: true);
        m_Menu_Leave = m_Menu.FindAction("Leave", throwIfNotFound: true);
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
    private readonly InputAction m_Gameplay_ZRightTrigger;
    private readonly InputAction m_Gameplay_ZRightTriggerUp;
    private readonly InputAction m_Gameplay_LeftTrigger;
    public struct GameplayActions
    {
        private PlayerControlTest m_Wrapper;
        public GameplayActions(PlayerControlTest wrapper) { m_Wrapper = wrapper; }
        public InputAction @A => m_Wrapper.m_Gameplay_A;
        public InputAction @Move => m_Wrapper.m_Gameplay_Move;
        public InputAction @Rotate => m_Wrapper.m_Gameplay_Rotate;
        public InputAction @RightTrigger => m_Wrapper.m_Gameplay_RightTrigger;
        public InputAction @ZRightTrigger => m_Wrapper.m_Gameplay_ZRightTrigger;
        public InputAction @ZRightTriggerUp => m_Wrapper.m_Gameplay_ZRightTriggerUp;
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
                ZRightTrigger.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnZRightTrigger;
                ZRightTrigger.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnZRightTrigger;
                ZRightTrigger.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnZRightTrigger;
                ZRightTriggerUp.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnZRightTriggerUp;
                ZRightTriggerUp.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnZRightTriggerUp;
                ZRightTriggerUp.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnZRightTriggerUp;
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
                ZRightTrigger.started += instance.OnZRightTrigger;
                ZRightTrigger.performed += instance.OnZRightTrigger;
                ZRightTrigger.canceled += instance.OnZRightTrigger;
                ZRightTriggerUp.started += instance.OnZRightTriggerUp;
                ZRightTriggerUp.performed += instance.OnZRightTriggerUp;
                ZRightTriggerUp.canceled += instance.OnZRightTriggerUp;
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
    private readonly InputAction m_Menu_Select;
    private readonly InputAction m_Menu_Move;
    private readonly InputAction m_Menu_Ready;
    private readonly InputAction m_Menu_Leave;
    public struct MenuActions
    {
        private PlayerControlTest m_Wrapper;
        public MenuActions(PlayerControlTest wrapper) { m_Wrapper = wrapper; }
        public InputAction @Select => m_Wrapper.m_Menu_Select;
        public InputAction @Move => m_Wrapper.m_Menu_Move;
        public InputAction @Ready => m_Wrapper.m_Menu_Ready;
        public InputAction @Leave => m_Wrapper.m_Menu_Leave;
        public InputActionMap Get() { return m_Wrapper.m_Menu; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MenuActions set) { return set.Get(); }
        public void SetCallbacks(IMenuActions instance)
        {
            if (m_Wrapper.m_MenuActionsCallbackInterface != null)
            {
                Select.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnSelect;
                Select.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnSelect;
                Select.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnSelect;
                Move.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnMove;
                Move.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnMove;
                Move.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnMove;
                Ready.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnReady;
                Ready.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnReady;
                Ready.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnReady;
                Leave.started -= m_Wrapper.m_MenuActionsCallbackInterface.OnLeave;
                Leave.performed -= m_Wrapper.m_MenuActionsCallbackInterface.OnLeave;
                Leave.canceled -= m_Wrapper.m_MenuActionsCallbackInterface.OnLeave;
            }
            m_Wrapper.m_MenuActionsCallbackInterface = instance;
            if (instance != null)
            {
                Select.started += instance.OnSelect;
                Select.performed += instance.OnSelect;
                Select.canceled += instance.OnSelect;
                Move.started += instance.OnMove;
                Move.performed += instance.OnMove;
                Move.canceled += instance.OnMove;
                Ready.started += instance.OnReady;
                Ready.performed += instance.OnReady;
                Ready.canceled += instance.OnReady;
                Leave.started += instance.OnLeave;
                Leave.performed += instance.OnLeave;
                Leave.canceled += instance.OnLeave;
            }
        }
    }
    public MenuActions @Menu => new MenuActions(this);
    public interface IGameplayActions
    {
        void OnA(InputAction.CallbackContext context);
        void OnMove(InputAction.CallbackContext context);
        void OnRotate(InputAction.CallbackContext context);
        void OnRightTrigger(InputAction.CallbackContext context);
        void OnZRightTrigger(InputAction.CallbackContext context);
        void OnZRightTriggerUp(InputAction.CallbackContext context);
        void OnLeftTrigger(InputAction.CallbackContext context);
    }
    public interface IMenuActions
    {
        void OnSelect(InputAction.CallbackContext context);
        void OnMove(InputAction.CallbackContext context);
        void OnReady(InputAction.CallbackContext context);
        void OnLeave(InputAction.CallbackContext context);
    }
}
