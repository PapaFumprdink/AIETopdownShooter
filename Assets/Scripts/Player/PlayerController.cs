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

    [SerializeField] private Transform m_LookContainer;

    private Controls m_Controls;
    private Camera m_MainCamera;

    public Vector2 MovementDirection => m_Controls.General.Movement.ReadValue<Vector2>();
    public bool WantsToFire => m_Controls.General.Fire.ReadValue<float>() > Deadzone;
    public bool IsAimingDownSights => m_Controls.General.ADS.ReadValue<float>() > Deadzone;
    public bool UseCursor => true;

    private void Awake()
    {
        m_MainCamera = Camera.main;

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
        var mousePosition = Mouse.current.position.ReadValue();
        var mouseWorldPosition = m_MainCamera.ScreenToWorldPoint(mousePosition);
        var lookDirection = (mouseWorldPosition - transform.position).normalized;
        var angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

        m_LookContainer.rotation = Quaternion.Euler(0, 0, angle);
    }
}
