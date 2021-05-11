using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class CursorController : MonoBehaviour
{
    [SerializeField] private CustomCursor m_CursorData;
    [SerializeField] private Image[] m_CursorImageLayers;

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

        foreach (Image cursorImage in m_CursorImageLayers)
        {
            if (cursorImage)
            {
                // Update the cursors visuals.
                cursorImage.sprite = m_CursorData.CursorIcon;
                cursorImage.fillAmount = m_CursorData.FillPercent;
            }
        }
    }
}
