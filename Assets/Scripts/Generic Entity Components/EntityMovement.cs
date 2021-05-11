using System;
using UnityEngine;

[SelectionBase]
[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
public class EntityMovement : MonoBehaviour
{
    [SerializeField] private float m_MaxSpeed;
    [SerializeField] private float m_Acceleration;

    [Space]
    [SerializeField] private Transform m_LookContainer;

    private IMovementProvider m_MovementProvider;
    private Rigidbody2D m_Rigidbody;

    private void Awake()
    {
        // Get Movement provider off gameobject
        m_MovementProvider = GetComponent<IMovementProvider>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Check if we have an input provider.
        if (m_MovementProvider != null)
        {
            Move();
            Turn();
        }
        else
        {
            // If there is no movement provider, this component can be disabled as it is useless.
            enabled = false;
        }
    }

    private void Move()
    {
        // Calculate the movement acceleration by finding the vector between the
        // current velocity and the velocity we want to reach. Then this can be
        // added to the rigidbody with the acceleration force to move the player.
        Vector2 movementDirection = Vector2.ClampMagnitude(m_MovementProvider.MovementDirection, 1f);
        Vector2 currentVelocity = m_Rigidbody.velocity;
        Vector2 targetVelocity = movementDirection * m_MaxSpeed;
        Vector2 velocityDifference = (targetVelocity - currentVelocity) / m_MaxSpeed;
        Vector2 acceleration = velocityDifference * m_Acceleration;

        m_Rigidbody.velocity += acceleration * Time.deltaTime;
    }

    private void Turn()
    {
        // Face the player towards the face direction
        Vector2 faceDirection = m_MovementProvider.FaceDirection;
        m_LookContainer.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(faceDirection.y, faceDirection.x) * Mathf.Rad2Deg);
    }
}
