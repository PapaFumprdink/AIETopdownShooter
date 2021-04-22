using System;
using UnityEngine;
using UnityEngine.InputSystem;

[SelectionBase]
[DisallowMultipleComponent]
public class PlayerController : MonoBehaviour, IMovementProvider, IWeaponInputProvider
{
    private const float Deadzone = 0.5f;

    public event Action JumpEvent;
    public event Action FireEvent;
    public event Action ReloadEvent;

    private Controls m_Controls;
    private Camera m_MainCamera;

    public Vector2 MovementDirection => m_Controls.General.Movement.ReadValue<Vector2>();
    public bool WantsToFire => m_Controls.General.Fire.ReadValue<float>() > Deadzone;
    public bool UseCursor => true;
    public Vector2 Direction { get; private set; }

    private void Awake()
    {
        // Stores the main camera for performance
        m_MainCamera = Camera.main;

        // Instantates the controls and subscribes to all used events.
        m_Controls = new Controls();

        m_Controls.General.Jump.performed += (ctx) => JumpEvent?.Invoke();
        m_Controls.General.Fire.performed += (ctx) => FireEvent?.Invoke();
        m_Controls.General.Reload.performed += (ctx) => ReloadEvent?.Invoke();
    }

    private void OnEnable()
    {
        m_Controls.Enable();
    }

    private void OnDisable()
    {
        m_Controls.Disable();
    }

    private void Update()
    {
        UpdateLookDirection();
    }

    private void UpdateLookDirection()
    {
        // Finds the world position of the mouse, then sets the look direction to face towards it.
        var mousePosition = Mouse.current.position.ReadValue();
        var mouseWorldPosition = m_MainCamera.ScreenToWorldPoint(mousePosition);
        Direction = (mouseWorldPosition - transform.position).normalized;
    }
}
