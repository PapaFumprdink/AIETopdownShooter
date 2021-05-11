using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
public class DoorJoint : MonoBehaviour
{
    [SerializeField] private Vector3 m_PivotOffset;
    [SerializeField] private float m_TargetRotation;
    [SerializeField] private float m_SpringForce;

    private Rigidbody2D m_Rigidbody;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Rigidbody.centerOfMass = m_PivotOffset;
    }

    private void FixedUpdate()
    {
        Vector2 targetDirection = Util.VectorFromAngle(m_TargetRotation).normalized;
        Vector2 currentDirection = Util.VectorFromAngle(transform.eulerAngles.z).normalized;
        float cross = Vector3.Cross(targetDirection, currentDirection).z;

        m_Rigidbody.angularVelocity += -cross * m_SpringForce * Time.deltaTime;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position + m_PivotOffset, 0.05f);
        Gizmos.color = Color.white;
    }
}
