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
    public interface IGameplayActions
    {
        void OnA(InputAction.CallbackContext context);
        void OnMove(InputAction.CallbackContext context);
        void OnRotate(InputAction.CallbackContext context);
        void OnRightTrigger(InputAction.CallbackContext context);
        void OnLeftTrigger(InputAction.CallbackContext context);
    }
}
