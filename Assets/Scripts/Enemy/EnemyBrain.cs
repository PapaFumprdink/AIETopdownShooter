using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
[DisallowMultipleComponent]
[RequireComponent(typeof(EnemyActions))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class EnemyBrain : MonoBehaviour
{
    [SerializeField] protected TargetType[] m_TargetableTypes;
    [SerializeField] protected float m_TargetMaxRange;
    [SerializeField] protected float m_ObstacleAvoidanceRange;
    [SerializeField] protected float m_ObstacleAvoidanceMaxAngle;

    public EnemyActions EnemyActions { get; private set; }
    public Rigidbody2D AttachedRigidbody { get; private set; }
    public bool WantsToFire { get => EnemyActions.WantsToFire; set => EnemyActions.WantsToFire = value; }

    public abstract void Think();

    protected virtual void Awake ()
    {
        AttachedRigidbody = GetComponent<Rigidbody2D>();
        EnemyActions = GetComponent<EnemyActions>();
    }

    private void Update()
    {
        Think();
    }

    protected Targetable GetTarget ()
    {
        foreach (TargetType type in m_TargetableTypes)
        {
            if (Targetable.TryFindTarget(type, transform.position, m_TargetMaxRange, out Targetable bestTarget))
            {
                return bestTarget;
            }
        }

        return null;
    }

    protected Vector2 CalulateObstacleAvoidance(GameObject target, Vector2 initialDirection)
    {
        var initialHit = Physics2D.Raycast(transform.position, initialDirection, m_ObstacleAvoidanceRange);
        if (!initialHit || initialHit.transform.gameObject == target.gameObject)
        {
            return initialDirection;
        }

        Vector2 leftDirection = Quaternion.Euler(0, 0, -m_ObstacleAvoidanceMaxAngle) * initialDirection;
        Vector2 rightDirection = Quaternion.Euler(0, 0, m_ObstacleAvoidanceMaxAngle) * initialDirection;

        RaycastHit2D leftHit = Physics2D.Raycast(transform.position, leftDirection, m_ObstacleAvoidanceRange);
        RaycastHit2D rightHit = Physics2D.Raycast(transform.position, rightDirection, m_ObstacleAvoidanceRange);

        Debug.DrawRay(transform.position, initialDirection * m_ObstacleAvoidanceRange, Color.red);
        Debug.DrawRay(transform.position, leftDirection * m_ObstacleAvoidanceRange, leftHit ? Color.red : Color.green);
        Debug.DrawRay(transform.position, rightDirection * m_ObstacleAvoidanceRange, rightHit ? Color.red : Color.green);

        if (!leftHit && rightHit)
        {
            return leftDirection;
        }

        if (!rightHit && leftHit)
        {
            return rightDirection;
        }

        if (leftHit.distance > rightHit.distance)
        {
            return leftDirection;
        }

        if (rightHit.distance > leftHit.distance)
        {
            return rightDirection;
        }

        return initialDirection;
    }

    protected void MoveTowards(MonoBehaviour behaviour) => MoveTowards(behaviour.transform);
    protected void MoveTowards(Transform transform) => MoveTowards(transform.gameObject);
    protected void MoveTowards (GameObject gameObject)
    {
        var directionToPoint = (gameObject.transform.position - transform.position).normalized;
        directionToPoint = CalulateObstacleAvoidance(gameObject, directionToPoint);
        EnemyActions.MovementDirection = directionToPoint;
    }

    protected void FaceTowards(MonoBehaviour behaviour) => FaceTowards(behaviour.gameObject);
    protected void FaceTowards(GameObject gameObject) => FaceTowards(gameObject.transform);
    protected void FaceTowards(Transform transform) => FaceTowards(transform.position);
    protected void FaceTowardsAngle(Quaternion rotation) => FaceTowardsAngle(rotation.z);
    protected void FaceTowardsAngle(float angle) => FaceTowards(Util.VectorFromAngle(angle) + (Vector2)transform.position);
    protected void FaceTowards(Vector3 position)
    {
        EnemyActions.FaceDirection = (position - transform.position).normalized;
    }

    protected void ClearMovement ()
    {
        EnemyActions.MovementDirection = Vector2.zero;
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_TargetMaxRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, 0, -m_ObstacleAvoidanceMaxAngle) * Vector3.right * m_ObstacleAvoidanceRange);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, 0, m_ObstacleAvoidanceMaxAngle) * Vector3.right * m_ObstacleAvoidanceRange);

        Gizmos.color = Color.white;
    }
}
