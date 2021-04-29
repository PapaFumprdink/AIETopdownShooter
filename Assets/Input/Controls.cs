// GENERATED AUTOMATICALLY FROM 'Assets/Input/Controls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Controls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Controls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controls"",
    ""maps"": [
        {
            ""name"": ""General"",
            ""id"": ""ed452ca7-7f8f-4a6d-b4bf-d7157851f0f2"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""7925d09e-e198-49d8-9562-5cc558079ec6"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Fire"",
                    ""type"": ""Button"",
                    ""id"": ""c7cfb4d8-14a8-44f8-bfca-89b6f3e8d6b9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": ""ScaleVector2(x=0.1,y=-0.1)"",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Reload"",
                    ""type"": ""Button"",
                    ""id"": ""b6e90416-723f-4390-bb9d-41f5c6517673"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": ""ScaleVector2(x=0.1,y=-0.1)"",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""688add96-df9c-4ecd-8071-6c43d9e1a959"",
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
                    ""id"": ""0367cfb9-20e5-4ea3-ad20-72f3147a9d79"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""c62fd0bf-f72a-4b2b-ac6a-9ad1f4c01177"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""b715cdfc-ae78-4907-aa40-522d0c49e801"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""68374c42-6322-49a2-8caf-721c259a11b0"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""c2611267-6bdc-4731-8128-31379d85a775"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f1f39ed9-c86f-40cb-b732-06289b947f49"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Reload"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // General
        m_General = asset.FindActionMap("General", throwIfNotFound: true);
        m_General_Movement = m_General.FindAction("Movement", throwIfNotFound: true);
        m_General_Fire = m_General.FindAction("Fire", throwIfNotFound: true);
        m_General_Reload = m_General.FindAction("Reload", throwIfNotFound: true);
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

    // General
    private readonly InputActionMap m_General;
    private IGeneralActions m_GeneralActionsCallbackInterface;
    private readonly InputAction m_General_Movement;
    private readonly InputAction m_General_Fire;
    private readonly InputAction m_General_Reload;
    public struct GeneralActions
    {
        private @Controls m_Wrapper;
        public GeneralActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_General_Movement;
        public InputAction @Fire => m_Wrapper.m_General_Fire;
        public InputAction @Reload => m_Wrapper.m_General_Reload;
        public InputActionMap Get() { return m_Wrapper.m_General; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GeneralActions set) { return set.Get(); }
        public void SetCallbacks(IGeneralActions instance)
        {
            if (m_Wrapper.m_GeneralActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_GeneralActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_GeneralActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_GeneralActionsCallbackInterface.OnMovement;
                @Fire.started -= m_Wrapper.m_GeneralActionsCallbackInterface.OnFire;
                @Fire.performed -= m_Wrapper.m_GeneralActionsCallbackInterface.OnFire;
                @Fire.canceled -= m_Wrapper.m_GeneralActionsCallbackInterface.OnFire;
                @Reload.started -= m_Wrapper.m_GeneralActionsCallbackInterface.OnReload;
                @Reload.performed -= m_Wrapper.m_GeneralActionsCallbackInterface.OnReload;
                @Reload.canceled -= m_Wrapper.m_GeneralActionsCallbackInterface.OnReload;
            }
            m_Wrapper.m_GeneralActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Fire.started += instance.OnFire;
                @Fire.performed += instance.OnFire;
                @Fire.canceled += instance.OnFire;
                @Reload.started += instance.OnReload;
                @Reload.performed += instance.OnReload;
                @Reload.canceled += instance.OnReload;
            }
        }
    }
    public GeneralActions @General => new GeneralActions(this);
    public interface IGeneralActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnFire(InputAction.CallbackContext context);
        void OnReload(InputAction.CallbackContext context);
    }
}
