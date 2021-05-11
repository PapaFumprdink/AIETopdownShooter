using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class CursorController : MonoBehaviour
{
    [SerializeField] private CustomCursor m_CursorData;
    [SerializeField] private Image m_CursorImage;

    private void OnEnable()
    {
        // Hide the 'real' cursor when the custom one is displayed.
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        // Show the cursor when the custom one is disabled.
        Cursor.visible = true;
    }

    private void LateUpdate()
    {
        // Set the cursor's position to the mouse position.
        transform.position = Mouse.current.position.ReadValue();

        // Update the cursors visuals.
        m_CursorImage.sprite = m_CursorData.CursorIcon;
        m_CursorImage.fillAmount = m_CursorData.FillPercent;
    }
}
