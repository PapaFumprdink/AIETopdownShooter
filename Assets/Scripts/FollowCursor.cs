using UnityEngine;
using UnityEngine.InputSystem;

public class FollowCursor : MonoBehaviour
{
    private void OnEnable()
    {
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        Cursor.visible = true;
    }

    private void LateUpdate()
    {
        transform.position = Mouse.current.position.ReadValue();
    }
}
